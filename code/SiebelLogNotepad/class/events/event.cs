using System.Collections.Generic;
using System.Collections.Specialized;
using Events.TreeView;

namespace Events
{
    class Event
    {
        // list of in/out sons
        public List<Event> Events { get; set; }

        // list of neutral sons
        public List<EventData> EventElements { get; set; }

        // list of all elements positions
        public List<int> EventElementsPosition { get; set; }

        // current position
        public int EventCurrentPosition { get; set; }

        // master element information
        public EventData Master { get; set; }

        // parrent connection its like a pointer to parent
        public Event ParentEvent { get; set; }

        // events that will be ignored
        public List<IgnoreEvent> EventsToIgnore { get; set; }

        // class that contains information of what to show
        public TreeLabel TreeLabel { get; set; }

        public Event()
        {
            TreeLabel = null;
            Events = new List<Event>();
            EventElements = new List<EventData>();
            EventElementsPosition = new List<int>();
            EventCurrentPosition = -1;
            Master = null;
            ParentEvent = null;
            EventsToIgnore = new List<IgnoreEvent>();
        }

        public SiebelTreeNode GetTreeNodes()
        {
            SiebelTreeNode tn = Master == null
                ? new SiebelTreeNode()
                : new SiebelTreeNode(Master, TreeLabel);

            foreach (int pos in EventElementsPosition)
            {
                if (pos >= 0)
                {
                    if (!IsEventToIgnore(EventElements[pos]))
                        tn.Nodes.Add(new SiebelTreeNode(EventElements[pos], TreeLabel));
                }
                else
                {
                    Events[(pos*-1) - 1].EventsToIgnore = EventsToIgnore;
                    Events[(pos*-1) - 1].TreeLabel = TreeLabel;

                    if (!IsEventToIgnore(Events[(pos * -1) - 1].Master))
                        tn.Nodes.Add(Events[(pos * -1) - 1].GetTreeNodes());
                }
            }

            return tn;
        }

        /********************************************* Private *********************************************/
        // check if event is to be ignored
        private bool IsEventToIgnore(EventData evt)
        {
            foreach (IgnoreEvent ie in EventsToIgnore)
            {
                string logEvent = ie.LogEvent == string.Empty ? ie.LogEvent : evt.LogEvent;
                string logSubEvent = ie.LogSubEvent == string.Empty ? ie.LogSubEvent : evt.LogSubEvent;
                string logMessage = ie.LogMessage == string.Empty ? ie.LogMessage : evt.LogMessage;
                string processedLogMessage = ie.LogMessage == string.Empty ? ie.LogMessage : evt.Name;

                // if not equal leave.
                if (logEvent != ie.LogEvent) continue;
                
                // if not equal leave.
                if (logSubEvent != ie.LogSubEvent) continue;
                
                if ( logMessage.Contains(ie.LogMessage) || processedLogMessage.Contains(ie.LogMessage))
                    return true;
            }

            return false;
        }
    }

    class EventData
    {
        public string Name { get; set; }
        public long Position { get; set; }
        public int Line { get; set; }
        public string ImageKey { get; set; }
        
        public string LogEvent { get; set; }
        public string LogSubEvent { get; set; }
        public string LogLevel { get; set; }
        public string LogDateTime { get; set; }
        public string LogMessage { get; set; }
    }

    class IgnoreEvent
    {
        public string LogEvent { get; set; }
        public string LogSubEvent { get; set; }
        public string LogMessage { get; set; }
    }

    class TreeLabel
    {
        public bool Name { get; set; }
        public bool LogLine { get; set; }
        public bool LogEvent { get; set; }
        public bool LogSubEvent { get; set; }
        public bool LogDateTime { get; set; }
        public bool LogMessage { get; set; }
    }
}
