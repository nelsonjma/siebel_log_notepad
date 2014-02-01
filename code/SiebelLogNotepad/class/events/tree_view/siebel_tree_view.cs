using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using EventTypeCfg;

namespace Events.TreeView
{
    class SiebelTreeView
    {
        // siebel log file
        private readonly string _logFile;
        
        // siebel xml config file
        private readonly string _cfgFile;

        private EventTypeList _etl;

        // event temporary variables
        private Event _evt;
        private Event _curEvt;

        // log deep level indicator
        private int _level;

        private readonly string _curDefaultPath;

        // load events thread
        private Thread _thEvent;

        // calculate percentage
        private long _logFilelength;
        private long _logFileCurPos;

        private bool _eventLoadStop;

        // thread control
        private readonly Mutex _eventLoadMutex;

        // this queue will contain IN/OUT reference to event data so that we can connect the {IN} with the {OUT}
        private Dictionary<int, List<EventData>> _filoQueueInOut;

        public SiebelTreeView(string logFile, string cfgFile, string curDefaultPath)
        {
            _logFile = logFile;
            _cfgFile = cfgFile;

            _etl = null;

            _evt = null;
            _curEvt = null;

            _level = 0;

            _curDefaultPath = curDefaultPath;

            _thEvent = null;

            _logFilelength = -1;
            _logFileCurPos = -1;

            _eventLoadMutex = new Mutex();

        }

        /******************************* Public *******************************/
        /// <summary>
        /// Start loading events from log file
        /// </summary>
        public void LoadEvents()
        {
            string logFile = _logFile;

            _thEvent = new Thread(() => LoadEvents(logFile));

            // this is required so that when the program terminate the thread does not became a gost
            _thEvent.IsBackground = true;
            _thEvent.Start();

            // wait to start;
            while(!_thEvent.IsAlive) Thread.Sleep(1);
        }

        /// <summary>
        /// Check if events are still loading
        /// </summary>
        public bool AreEventsLoading()
        {
            return _thEvent.IsAlive;
        }

        /// <summary>
        /// Stops thread execution
        /// </summary>
        public void StopEventsLoading()
        {
            _eventLoadMutex.WaitOne();
            _eventLoadStop = true;
            _eventLoadMutex.ReleaseMutex();
        }

        // returns load status in percentage
        public int  LoadEventsPercentageDone()
        {
            _eventLoadMutex.WaitOne();
            long logFilelength = _logFilelength;
            long logFileCurPos = _logFileCurPos;
            _eventLoadMutex.ReleaseMutex();


            // if thread is dead then cya later
            if (!_thEvent.IsAlive) return 100;

            // if any of the variables are negative then WTF
            if (logFilelength <= -1 || logFileCurPos <= -1) return 0;

            /*
             * 100 -- file length
             * X -- current length
             */
            try { return (int)((100 * logFileCurPos) / logFilelength); } catch { return 0; }
        }

        /// <summary>
        /// Gets event icons to load in tree view
        /// </summary>
        public ImageList LoadEventIcons()
        {
            string[] imageList = Directory.GetFiles(_curDefaultPath + @"\img\", "*.png");

            ImageList iconList = new ImageList();

            foreach (string imgFile in imageList)
                iconList.Images.Add(imgFile.Substring(imgFile.LastIndexOf('\\') + 1), Image.FromFile(imgFile));

            return iconList;
        }

        /// <summary>
        /// Returns events so taht in can be load in to tree view, with ignore list
        /// </summary>
        public Event GetTreeEvents(List<IgnoreEvent> listIgnoreEventMsgs, TreeLabel treeLabel)
        {
            _curEvt = _evt;
            _curEvt.EventsToIgnore = listIgnoreEventMsgs;
            _curEvt.TreeLabel = treeLabel;

            return _curEvt;
        }

        public Event GetTreeEvents()
        {
            _curEvt = _evt;

            return _curEvt;
        }

        /******************************* Private *******************************/
        /// <summary>
        /// Build the event tree inside a thread...
        /// </summary>
        private void LoadEvents(string logFile)
        {
            /*
             * Siebel 8 Log
             *   :Log Detail                         :Description
             *   0 =>    SQLParseAndExecute		    	    Event Type alias
             *   1 =>    Prepare + Execute	    		    Event Subtype
             *   2 =>    4		    	    			    Event Severity					
             *   3 =>    0       						    SARM ID                 // just exists in siebel 8 ++
             *   4 =>    2003-05-13 14:07:38				    Date and time of log	
             *   5 =>    Time: 0s, Rows: 0, Avg. Time: 0s	Log message
             *   
             *   SQLParseAndExecute Prepare + Execute 4 0 2003-05-13 14:07:38 Time: 0s, Rows: 0, Avg. Time: 0s
             */

            try
            {
                

                FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                long fileLenght = fs.Length;
                //StreamReader sr = new StreamReader(fs, Encoding.UTF8);

                // Get file length
                _eventLoadMutex.WaitOne();
                _logFilelength = fileLenght;
                _eventLoadMutex.ReleaseMutex();

                // Loads xml config file
                LoadConfigInformation();

                // initialize in/out events queue
                InitializeEventInOutQueue();

                // current line position
                int curLinePos = -1;

                // log deep level indicator
                int level = 0;

                // line position storate
                List<long> fileStartPos = new List<long> {0};

                // pre loads line and event list
                List<EventType> lineTypeList = _etl.ListEvents.Where(x => x.EventName == "OneLineEvent").ToList();
                List<EventType> eventTypeList = _etl.ListEvents.Where(x => x.EventName != "OneLineEvent").ToList();

                // get starting line positions
                while (fs.Position < fileLenght)
                {
                    // new line \n
                    if (fs.ReadByte() != 10) continue;

                    fileStartPos.Add(fs.Position);
                }

                // process lines
                foreach (long filePos in fileStartPos)
                {
                    long curStreamPos = filePos;

                    fs.Seek(filePos, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);

                    string curLine = sr.ReadLine();

                    // increment line
                    curLinePos++;

                    // if nothing to do leave
                    if (string.IsNullOrEmpty(curLine)) continue;

                    if (curLine.IndexOf('\t') <= 0 && lineTypeList.Count > 0)       // if no tabs and if we have line types to process
                        level = ProcessOneLineEvents(ref lineTypeList, curLine, level, curStreamPos, curLinePos);
                    else if (curLine.IndexOf('\t') > 0 && eventTypeList.Count > 0)  // if tabs and if we have event types to process
                        level = ProcessEvents(ref eventTypeList, curLine, level, curStreamPos, curLinePos);

                    // sends current log position to variable so that it can be used to get percentage
                    _eventLoadMutex.WaitOne();
                    if (curLinePos%500 == 0) 
                        _logFileCurPos = curStreamPos;

                    if (_eventLoadStop) break;

                    _eventLoadMutex.ReleaseMutex();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Load Events From File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Loads config file information, so taht it can be used in log message processing
        /// </summary>
        private void LoadConfigInformation()
        {
            try
            {
                _etl = new EventTypeList();
                _etl.GetEvents(_cfgFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Builds event tree so that it can be used in tree view
        /// </summary>
        private EventData TreeBuilder(int level, string outMessage, long position, int line, string imageKey,
                                        string logEvent, string logSubEvent, string logLevel, string logDatetime, string logMessage)
        {
            EventData evtPos = null;

            if (_evt == null)
            {
                _evt = new Event();
                _curEvt = _evt;
            }

            if (level > _level)
            {
                // create son
                _curEvt.Events.Add(new Event());

                // store son position
                _curEvt.EventElementsPosition.Add((_curEvt.Events.Count) * (-1));

                // store parent event
                _curEvt.Events[_curEvt.Events.Count - 1].ParentEvent = _curEvt;

                // move to son event
                _curEvt = _curEvt.Events[_curEvt.Events.Count - 1];

                _curEvt.Master = new EventData()
                {
                    Name = outMessage,
                    Position = position,
                    Line = line,
                    ImageKey = imageKey,
                    
                    LogEvent = logEvent,
                    LogSubEvent = logSubEvent,
                    LogLevel = logLevel,
                    LogDateTime = logDatetime,
                    LogMessage = logMessage
                };

                evtPos = _curEvt.Master;
            }
            else if (level < _level)
            {
                // go to dad if exists
                if (_curEvt.ParentEvent != null)
                    _curEvt = _curEvt.ParentEvent;
            }
            else if (_curEvt != null)
            {
                // store elements
                _curEvt.EventElements.Add(new EventData()
                {
                    Name = outMessage,
                    Position = position,
                    Line = line,
                    ImageKey = imageKey,

                    LogEvent = logEvent,
                    LogSubEvent = logSubEvent,
                    LogLevel = logLevel,
                    LogDateTime = logDatetime,
                    LogMessage = logMessage
                });

                // store element position
                _curEvt.EventElementsPosition.Add(_curEvt.EventElements.Count - 1);
            }

            _level = level;

            return evtPos;
        }

        /* Process Events */

        /// <summary>
        /// process events divided by tabs
        /// </summary>
        private int ProcessEvents(ref List<EventType> eventTypeList, string curLine, int level, long curStreamPos, int curLinePos)
        {
            string[] lineParts = curLine.Split('\t');

            // if line parts are not siebel 7 or 8 format then do nothing
            if (lineParts.Length != 5 && lineParts.Length != 6) return level;

            int increment = lineParts.Length == 5 ? 0 : 1;

            string logEvent = lineParts[0];
            string logSubEvent = lineParts[1];
            string logLevel = lineParts[2];
            string logDateTime = lineParts[3 + increment];
            string logMessage = lineParts[4 + increment];

            for (int i = 0; i < eventTypeList.Count; i++)
            {
                EventType etl = eventTypeList[i];

                if (!etl.IsEvent(logEvent, logSubEvent, logMessage)) continue;

                // store old level value;
                int auxLevel = level;

                // change level if necessary
                level = etl.MoveLevel(level);

                string eventName = etl.OutMessage();

                EventData eventData = TreeBuilder(
                                        level, eventName, curStreamPos, curLinePos, etl.GetImg(),
                                        logEvent, logSubEvent, logLevel, logDateTime, logMessage
                                        );

                // if event level is -- then get the {IN} part of the event
                if (auxLevel > level)
                    AddOutToEventReference(eventName, i);

                // store the {IN} part in a QUEUE
                if (eventData != null)
                    AddInToEventRefereceQueue(ref eventData, i);

                break;
            }

            return level;
        }

        /// <summary>
        /// process the complete line
        /// </summary>
        private int ProcessOneLineEvents(ref List<EventType> eventTypeList, string curLine, int level, long curStreamPos, int curLinePos)
        {
            foreach (EventType etl in eventTypeList)
            {
                if (!etl.IsEvent(curLine, null, null)) continue;

                string eventName = etl.OutMessage();
                
                TreeBuilder(
                    level, eventName, curStreamPos, curLinePos, etl.GetImg(),
                    string.Empty, string.Empty, string.Empty, string.Empty, curLine
                    );
            }

            return level;
        }

        /* Event In/Out Ctrl */
        /// <summary>
        /// Initialize event In Out Message Ctrl
        /// </summary>
        private void InitializeEventInOutQueue()
        {
            if (_etl == null) return;

            _filoQueueInOut = new Dictionary<int, List<EventData>>();

            for (int i = 0; i < _etl.ListEvents.Count; i++)
                _filoQueueInOut.Add(i, new List<EventData>());
        }

        /// <summary>
        /// Add event to reference queue
        /// </summary>
        private void AddInToEventRefereceQueue(ref EventData evt, int eventTypePosition)
        {
            _filoQueueInOut[eventTypePosition].Add(evt);
        }

        /// <summary>
        /// Add OUT part to event
        /// </summary>
        private void AddOutToEventReference(string outPart, int eventTypePosition)
        {
            // there is no event to connect WTF is this SHIT...
            if (_filoQueueInOut[eventTypePosition].Count == 0) return;

            // get event reference
            EventData evt = _filoQueueInOut[eventTypePosition][_filoQueueInOut[eventTypePosition].Count - 1];

            // remove last element
            _filoQueueInOut[eventTypePosition].RemoveAt(_filoQueueInOut[eventTypePosition].Count - 1);

            // add {OUT} part
            evt.Name = evt.Name.Replace("{OUT}", outPart);
        }

    }
}
