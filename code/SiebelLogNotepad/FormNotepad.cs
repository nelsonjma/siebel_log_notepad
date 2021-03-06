﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Events.TreeView;
using FastColoredTextBoxNS;
using SiebelLogNotepad.Forms;
using Events;
using SiebelLogNotepad.forms;

namespace SiebelLogNotepad
{
    public partial class FormNotepad : Form
    {
        // sintax highlight
        private TextStyle _boldStyle;
        private TextStyle _italicStyle;
        private TextStyle _blueStyle;
        private TextStyle _greenStyle;
        private TextStyle _darkredStyle;
        private TextStyle _darkorangeStyle;
        private TextStyle _darkvioletStyle;
        
        private string _boldStyleText;
        private string _italicStyleText;
        private string _blueStyleText;
        private string _greenStyleText;
        private string _darkredStyleText;
        private string _darkorangeStyleText;
        
        // siebel log file
        private string _openLogFile;
        
        // siebel config file
        private string _openConfigFile;

        // current folder
        private readonly string _defaultPath;

        // default title
        private readonly string _originalFormHeaderText;

        private BackgroundWorker _eventLoadBackgroundWorker;

        // find in tree
        private SiebelTreeNode _findedSiebelTreeNode;
        private List<int> _findedSiebelNodePos;

        // find in text
        private List<int> _findedInTextBox;
        private int _findedInTextBoxPos;
        private string _findedInTextBoxValue;

        // textbox
        private FastColoredTextBox _fastColorTb;

        // tree selected event
        private SiebelTreeNode _selectedNode;

        // contains the information that will be loaded to the three view, this exists so that when user refreshs there is no need to load file again.
        private SiebelTreeView _siebelTreeView;

        // bookmark color
        private Color _bookmarkColor;

        public FormNotepad()
        {
            InitializeComponent();

            // initialization
            _defaultPath = Directory.GetCurrentDirectory();
            _openConfigFile = _defaultPath + @"\cfg\default_cfg.xml";
            _openLogFile = null;

            // start focus on tree so that open 
            //IMPEMENT FOCUS IN TREE SO THAT FIRST BUTTON DONT LOOK GAY

            // Loads ToolTips
            InitializeTooltip();

            // Event Load Background Worker Initialization
            InitializeEventsLoadBackGroundWorker();

            // Initialize TextBox
            InitializeFastColoredTextBox();

            // Initialize TextBox SintaxHighLight
            InitializeSintraxHighlight();

            // store form header text
            _originalFormHeaderText = base.Text;

            // initialize find 
            InitializeFindVariables();

            // set the default line = to a line that does not exists
            _selectedNode = null;

            // initialize bookmark color
            _bookmarkColor = Color.DarkRed;

            // set the object that contains tree nodes to null
            _siebelTreeView = null;
        }

        /// <summary>
        /// Create tooltip
        /// </summary>
        private void InitializeTooltip()
        {
            // Open Log File
            ToolTip ttOpenLog = new ToolTip{ AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttOpenLog.SetToolTip(buttonOpenLog, "Open Siebel Log File");

            // Open Config File
            ToolTip ttOpenConfigFile = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttOpenConfigFile.SetToolTip(buttonOpenConfig, "Open Xml Config File");

            // Ignore Events
            ToolTip ttIgnoreEvents = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttIgnoreEvents.SetToolTip(buttonIgnore, "Ignore Events");

            // Ignore Events
            ToolTip ttStatistics = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttStatistics.SetToolTip(buttonStatistics, "Statistics");

            // Refresh Tree
            ToolTip ttRefreshTree = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttRefreshTree.SetToolTip(buttonRefreshTree, "Refresh Tree");

            // Find In Tree
            ToolTip ttFindInTree = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttFindInTree.SetToolTip(buttonMarkAndFind, "Mark & Find");

            // Expand Tree
            ToolTip ttExpand = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttExpand.SetToolTip(buttonExpandTree, "Expand Tree");

            // Collapse Tree
            ToolTip ttCollapse = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttCollapse.SetToolTip(buttonCollapse, "Collapse Tree");

            // Change Tree Text
            ToolTip ttChangeText = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttChangeText.SetToolTip(buttonChangeTree, "Change Text In Tree");

            // Go To Line
            ToolTip ttGoToLine = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttGoToLine.SetToolTip(buttonGoToLine, "Go To Line");

            // Bookmark line
            ToolTip ttBookMarkLine = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttBookMarkLine.SetToolTip(buttonBookMark, "Bookmark Line");

            // Bookmark color cfg
            ToolTip ttBookMarkCfg = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttBookMarkCfg.SetToolTip(buttonBookMarkCfg, "Bookmark Line Color Config");

            // Copy text
            ToolTip ttCopy = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttCopy.SetToolTip(buttonCopy, "Copy node text");

            // Bookmark color cfg
            ToolTip ttAbout = new ToolTip { AutoPopDelay = 1000, InitialDelay = 1000, ReshowDelay = 500, ShowAlways = true };
            ttAbout.SetToolTip(buttonAbout, "About Form");
        }

        /*********************************** Go To Line / Add Color To Text ***********************************/
        /// <summary>
        /// Move to line in textbox
        /// </summary>
        private void GoToTextBoxLine(int line)
        {
            try
            {
                _fastColorTb.Navigate(line);

                _fastColorTb.SelectionLength = _fastColorTb.Lines[line].Length;

                _fastColorTb.Focus();
                //SendKeys.SendWait("{RIGHT}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Go To Line", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Bookmark with default color
        /// </summary>
        private void BookmarkLine(int line, Color clr)
        {
            _fastColorTb.BookmarkColor = clr;

            _fastColorTb.BookmarkLine(line);
        }

        /// <summary>
        /// Bookmark with default color
        /// </summary>
        private void BookmarkLine(int line)
        {
            _fastColorTb.BookmarkColor = _bookmarkColor;

            _fastColorTb.BookmarkLine(line);
        }

        /// <summary>
        /// Remove bookmark color
        /// </summary>
        /// <param name="line"></param>
        /// <param name="clr"></param>
        private void UnBookmarkLine(int line)
        {
            _fastColorTb.Bookmarks.Remove(line);
        }

        /*********************************** Fast Colored TextBox ***********************************/
        /// <summary>
        /// Initialize Fast Color TextBox
        /// </summary>
        private void InitializeFastColoredTextBox()
        {
            _fastColorTb = null;

            try
            {
                _fastColorTb = new FastColoredTextBox
                {
                    AutoScrollMinSize = new Size(480, 45),
                    BackBrush = null,
                    CharHeight = 15,
                    CharWidth = 7,
                    Cursor = Cursors.IBeam,
                    DelayedEventsInterval = 300,
                    DisabledColor = Color.FromArgb(100, 180, 180, 180),
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 9.75F),
                    IsReplaceMode = false,
                    Location = new Point(0, 86),
                    Name = "fctb",
                    Paddings = new Padding(0),
                    SelectionColor = Color.FromArgb(50, 0, 0, 255),
                    Size = new Size(100, 100),
                    TabIndex = 0,
                    Zoom = 100,
                    LineNumberColor = Color.Black
                };

                _fastColorTb.VisibleRangeChangedDelayed += FastColorTextBox_VisibleRangeChangedDelayed;

                panelTextBox.Controls.Add(_fastColorTb);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Fast Color TextBox Initialization", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load TextBox in Thread
        delegate void LoadTextBoxDelegate(FastColoredTextBox fctBox);

        public void LoadTextBox(FastColoredTextBox fctBox)
        {
            if (fctBox.InvokeRequired)
            {
                LoadTextBoxDelegate loadFile = (LoadTextBox);

                if (!fctBox.IsDisposed)
                    fctBox.Invoke(loadFile, fctBox);
            }
            else
            {
                try
                {
                    if (string.IsNullOrEmpty(_openLogFile)) return;

                    _fastColorTb.OpenBindingFile(_openLogFile, Encoding.UTF8);
                    _fastColorTb.IsChanged = false;
                    _fastColorTb.ClearUndo();

                    GC.Collect();
                    GC.GetTotalMemory(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, @"Fast Color TextBox Load File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Load file to textbox
        /// </summary>
        private void LoadFileToTextBox()
        {
            // wait some time before start
            Thread.Sleep(100);

            Thread th = new Thread(() => LoadTextBox(_fastColorTb)) { IsBackground = true };
            th.Start();

            while (!th.IsAlive) Thread.Sleep(1);
        }

        /*************** Fast Colored TextBox Sintax Highlight ***************/
        /// <summary>
        /// Initialize sintax highlight
        /// </summary>
        private void InitializeSintraxHighlight()
        {
            _boldStyle          = new TextStyle(null, null, FontStyle.Bold | FontStyle.Underline);
            _italicStyle        = new TextStyle(null, null, FontStyle.Bold | FontStyle.Italic);
            _blueStyle          = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
            _greenStyle         = new TextStyle(Brushes.Green, null, FontStyle.Bold | FontStyle.Italic);
            _darkredStyle       = new TextStyle(Brushes.DarkRed, null, FontStyle.Bold);
            _darkorangeStyle    = new TextStyle(Brushes.DarkOrange, null, FontStyle.Bold);
            _darkvioletStyle    = new TextStyle(Brushes.DarkViolet, null, FontStyle.Bold);

            _boldStyleText          = string.Empty; try { _boldStyleText        = RwFile.ReadXml(_defaultPath + @"\cfg\sintax_highlight.xml", "bold")[0]; } catch {}
            _italicStyleText        = string.Empty; try { _italicStyleText      = RwFile.ReadXml(_defaultPath + @"\cfg\sintax_highlight.xml", "italic")[0]; } catch { }
            _blueStyleText          = string.Empty; try { _blueStyleText        = RwFile.ReadXml(_defaultPath + @"\cfg\sintax_highlight.xml", "blue")[0]; } catch { }
            _greenStyleText         = string.Empty; try { _greenStyleText       = RwFile.ReadXml(_defaultPath + @"\cfg\sintax_highlight.xml", "green")[0]; } catch { }
            _darkredStyleText       = string.Empty; try { _darkredStyleText     = RwFile.ReadXml(_defaultPath + @"\cfg\sintax_highlight.xml", "darkred")[0]; } catch { }
            _darkorangeStyleText    = string.Empty; try { _darkorangeStyleText  = RwFile.ReadXml(_defaultPath + @"\cfg\sintax_highlight.xml", "darkorange")[0]; } catch { }
        }

        /// <summary>
        /// Apply sintax highlight
        /// </summary>
        private void SintraxHighlightVisibleRange()
        {
            const int margin = 2000;

            //expand visible range (+- margin)
            var startLine = Math.Max(0, _fastColorTb.VisibleRange.Start.iLine - margin);
            var endLine = Math.Min(_fastColorTb.LinesCount - 1, _fastColorTb.VisibleRange.End.iLine + margin);
            var range = new Range(_fastColorTb, 0, startLine, 0, endLine);
            
            //clear folding markers
            //range.ClearFoldingMarkers();
            
            //set markers for folding
            //range.SetFoldingMarkers(@"N\d\d00", @"N\d\d99");
            
            //range.ClearStyle(StyleIndex.All);

            range.SetStyle(_darkvioletStyle, @"N\d+");

            if (_boldStyleText != string.Empty) range.SetStyle(_boldStyle, _boldStyleText);
            if (_italicStyleText != string.Empty) range.SetStyle(_italicStyle, _italicStyleText);
            if (_blueStyleText != string.Empty) range.SetStyle(_blueStyle, _blueStyleText);
            if (_greenStyleText != string.Empty) range.SetStyle(_greenStyle, _greenStyleText);
            if (_darkredStyleText != string.Empty) range.SetStyle(_darkredStyle, _darkredStyleText);
            if (_darkorangeStyleText != string.Empty) range.SetStyle(_darkorangeStyle, _darkorangeStyleText);
        }

        private void FastColorTextBox_VisibleRangeChangedDelayed(object sender, EventArgs e)
        {
            SintraxHighlightVisibleRange();
        }

        /*********************************** Find Next Tree Events ***********************************/
        /****** Find in text ******/
        /// <summary>
        /// First find text in textbox it will generate the line list
        /// </summary>
        public void FindInTextBox(string value, Color clr, int direction)
        {
            // if text is equal then find next
            if (_findedInTextBoxValue == value)
            {
                FindInTextBox(clr, direction); return;
            }

            // if != then store value
            _findedInTextBoxValue = value;

            // get lines
            _findedInTextBox = _fastColorTb.FindLines(value, RegexOptions.IgnoreCase);


            int posForCurrentLine = _findedInTextBox.FindIndex(x => x == _fastColorTb.Selection.Start.iLine);

            // initialize current position
            _findedInTextBoxPos = posForCurrentLine >= 0 ? posForCurrentLine : 0;

            // find line and move to the first -1 is to ignore increment or decrement
            FindInTextBox(clr, -1);
        }

        /// <summary>
        /// Move forward or backward
        /// </summary>
        public void FindInTextBox(Color clr, int direction)
        {
            if (_findedInTextBox.Count == 0)
                { MessageBox.Show(@"Not found try again", @"Find", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

            // decrement / increment position
            switch (direction)
            {
                case 0: _findedInTextBoxPos++; break;
                case 1: _findedInTextBoxPos--; break;
            }

            // if position is bigger then stay on top else if less then zero then stay on zero
            if (_findedInTextBoxPos >= _findedInTextBox.Count) 
                _findedInTextBoxPos = _findedInTextBox.Count - 1;
            else if (_findedInTextBoxPos < 0) 
                _findedInTextBoxPos = 0;

            GoToTextBoxLine(_findedInTextBox[_findedInTextBoxPos]);

            // bookmark line
            BookmarkLine(_findedInTextBox[_findedInTextBoxPos], clr);
        }

        /****** Find in tree ******/
        /// <summary>
        /// Find text in nodes
        /// </summary>
        public void FindInTree(string value, Color clr)
        {
            try
            {
                if (_findedSiebelTreeNode == null)
                    FindFirstInTree(value, clr);
                else
                    FindNextInTree(_findedSiebelTreeNode, _findedSiebelNodePos[_findedSiebelNodePos.Count - 1], value, clr);
            }
            catch (Exception ex)
            {
                throw new Exception("Find error: " + ex.Message);
            }
            
        }

        /// <summary>
        /// Find first node
        /// </summary>
        private void FindFirstInTree(string value, Color clr)
        {
            TreeNodeCollection tnc = treeViewSiebelTree.Nodes;

            for (int i = 0; i < tnc.Count; i++)
            {
                SiebelTreeNode stn = (SiebelTreeNode)tnc[i];

                _findedSiebelNodePos.Add(i);

                if (FindAuxiliarInTree(stn, 0, value, clr)) return;

                _findedSiebelNodePos.RemoveAt(_findedSiebelNodePos.Count - 1);
            }
        }

        /// <summary>
        /// Find Next Position
        /// </summary>
        private void FindNextInTree(SiebelTreeNode tns, int iPos, string value, Color clr)
        {
            while (true)
            {
                if (FindAuxiliarInTree(tns, iPos, value, clr)) return;

                // go to parent if nothing to do
                if (tns.Parent != null)
                {
                    tns = (SiebelTreeNode)tns.Parent;

                    // remove before going to parent
                    _findedSiebelNodePos.RemoveAt(_findedSiebelNodePos.Count - 1);

                    // add one position so that it will start in the next position
                    _findedSiebelNodePos[_findedSiebelNodePos.Count - 1] = _findedSiebelNodePos[_findedSiebelNodePos.Count - 1] + 1;

                    iPos = _findedSiebelNodePos[_findedSiebelNodePos.Count - 1];
                }
                else
                {
                    _findedSiebelTreeNode = null;

                    // move to first position
                    FindFirstInTree(value, clr);

                    return;
                }
            }
        }

        /// <summary>
        /// Sub method used used in MoveToFirstFind and FindFromPrevious
        /// </summary>
        private bool FindAuxiliarInTree(SiebelTreeNode tns, int iPos, string value, Color clr)
        {
            for (int i = iPos; i < tns.Nodes.Count; i++)
            {
                // move position
                if (_findedSiebelNodePos.Count > 0)
                    _findedSiebelNodePos[_findedSiebelNodePos.Count - 1] = i;

                SiebelTreeNode tn = (SiebelTreeNode)tns.Nodes[i];

                if (tn.Text.Contains(value))
                {
                    tn.EnsureVisible();
                    tn.BackColor = clr;

                    _findedSiebelNodePos[_findedSiebelNodePos.Count - 1] = i + 1; // move one caracter forward
                    _findedSiebelTreeNode = tns;

                    return true;
                }

                // if no nodes, just continue
                if (tn.Nodes.Count == 0) continue;

                // add current branch to list
                _findedSiebelNodePos.Add(i);

                // if this branch is the one leave
                if (FindAuxiliarInTree(tn, 0, value, clr)) return true;

                // if not the one remove the node position
                _findedSiebelNodePos.RemoveAt(_findedSiebelNodePos.Count - 1);
            }

            return false;
        }

        /// <summary>
        /// Initialize find variables
        /// </summary>
        public void InitializeFindVariables()
        {
            // find next in tree initialization
            _findedSiebelTreeNode = null;
            _findedSiebelNodePos = new List<int>();

            // find next in textbox initialization
            _findedInTextBox = new List<int>();
            _findedInTextBoxPos = 0;
            _findedInTextBoxValue = string.Empty;
        }

        /*********************************** Mark Tree Events ***********************************/
        /// <summary>
        /// Mark nodes with diferent colot
        /// </summary>
        public void MarkTreeNode(string value, Color clr)
        {
            try
            {
                TreeNodeCollection tnc = treeViewSiebelTree.Nodes;

                foreach (SiebelTreeNode stn in tnc)
                    MarkTreeNode(stn, value, clr);
            }
            catch (Exception ex)
            {
                throw new Exception("Mark all error: " + ex.Message);
            }
            
        }

        /// <summary>
        ///  Travels tree branchs to add color to nodes that contains the search text
        /// </summary>
        private static void MarkTreeNode(SiebelTreeNode stn, string value, Color clr)
        {
            foreach (SiebelTreeNode tn in stn.Nodes)
            {
                // if the text properties match, color the item
                if (tn.Text.Contains(value)) tn.BackColor = clr;

                MarkTreeNode(tn, value, clr);
            }
        }

        /// <summary>
        /// Mark text with color
        /// </summary>
        public void MarkTextBox(string value, Color clr)
        {
            List<int> linesToMark = _fastColorTb.FindLines(value, RegexOptions.IgnoreCase);

            foreach (int line  in linesToMark)
                BookmarkLine(line, clr);
        }

        /*********************************** Load Events Ctrls ***********************************/
        /// <summary>
        /// Load Siebel Log TreeView 
        /// </summary>
        private void LoadTree()
        {
            try
            {
                if (string.IsNullOrEmpty(_openLogFile)) return;
                
                // store necessary information to send to background worker
                Dictionary<string, object> argsBkWorkerEvents = new Dictionary<string, object>
                {
                    {"OpenLogFile",     _openLogFile},
                    {"OpenConfigFile",  _openConfigFile},
                    {"DefaultPath",     _defaultPath}
                };

                // clean old nodes
                treeViewSiebelTree.Nodes.Clear();

                // start background worker
                _eventLoadBackgroundWorker.RunWorkerAsync(argsBkWorkerEvents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Load Tree", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Initialize Background Worker Events/Variables
        /// </summary>
        private void InitializeEventsLoadBackGroundWorker()
        {
            try
            {
                _eventLoadBackgroundWorker = new BackgroundWorker();
                
                _eventLoadBackgroundWorker.ProgressChanged += EventLoadBackgroundWorker_ProgressChanged;
                _eventLoadBackgroundWorker.RunWorkerCompleted += EventLoadBackgroundWorker_RunWorkerCompleted;
                _eventLoadBackgroundWorker.DoWork += EventLoadBackgroundWorker_DoWork;

                _eventLoadBackgroundWorker.WorkerReportsProgress = true;
                _eventLoadBackgroundWorker.WorkerSupportsCancellation = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Load TreeView Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Requests the events load background worker to end
        /// </summary>
        private void CloseEventsLoadBackGroundWorker()
        {
            _eventLoadBackgroundWorker.CancelAsync();
        }
        
        // Background Worker - run operations
        private void EventLoadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                bool auxCancel = false;

                BackgroundWorker bw = (BackgroundWorker)sender;

                // input arguments
                Dictionary<string, object> argsBkWorkerEvents = (Dictionary<string, object>)e.Argument;

                string openLogFile =    (string)argsBkWorkerEvents["OpenLogFile"];
                string openConfigFile = (string)argsBkWorkerEvents["OpenConfigFile"];
                string defaultPath =    (string)argsBkWorkerEvents["DefaultPath"];

                SiebelTreeView stv = new SiebelTreeView(openLogFile, openConfigFile, defaultPath);

                // start events processing
                stv.LoadEvents();

                // wait to event load ends
                while (stv.AreEventsLoading())
                {
                    Thread.Sleep(100);

                    // if is to stop dont talk to load events class anymore
                    if (auxCancel) continue;

                    if (bw.CancellationPending)
                    {
                        // send signal to load events thread end
                        stv.StopEventsLoading(); 
                        auxCancel = true;
                    }
                    
                    bw.ReportProgress(stv.LoadEventsPercentageDone());
                }

                // if shit appends then set percentage to 0 and set the results to null
                if (bw.CancellationPending)
                {
                    bw.ReportProgress(0);
                    e.Result = null;
                    return;
                }

                // if not canceled returns 100% and set the results to the class that represents the event nodes
                bw.ReportProgress(100);
                e.Result = stv;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Load Events to TreeView", MessageBoxButtons.OK, MessageBoxIcon.Error); 

                e.Result = null;
            }
        }

        // Background Worker - end operations
        private void EventLoadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Result == null) return;

                _siebelTreeView = (SiebelTreeView)e.Result;

                LoadSiebelTreeView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Write Events TreeView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Background Worker - reports progress
        private void EventLoadBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                string file = !string.IsNullOrEmpty(_openLogFile)
                        ? " - " + _openLogFile.Substring(_openLogFile.LastIndexOf(@"\", StringComparison.Ordinal) + 1) + " - "
                        : " ";

                Text = _originalFormHeaderText + file + e.ProgressPercentage + @"%";
            }
            catch { }            
        }

        /// <summary>
        /// Load List Events to be Ignored
        /// </summary>
        private List<IgnoreEvent> LoadIgnoreEvents()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(_defaultPath + @"\cfg\ignore_events.xml", XmlReadMode.ReadSchema);

                List<IgnoreEvent> ignoreEventList = new List<IgnoreEvent>();

                if (ds.Tables.Count <= 0) return ignoreEventList;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr.ItemArray[0].Equals(true))
                    {
                        ignoreEventList.Add(new IgnoreEvent
                        {
                            LogEvent = dr.ItemArray[1].ToString().Trim(),
                            LogSubEvent= dr.ItemArray[2].ToString().Trim(),
                            LogMessage = dr.ItemArray[3].ToString()
                        });   
                    }
                }

                return ignoreEventList;
            }
            catch (Exception ex)
            {
                throw new Exception("Load Ignore Events Xml Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Load Tree Label from file
        /// </summary>
        private TreeLabel LoadTreeLabel()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(_defaultPath + @"\cfg\tree_labels.xml", XmlReadMode.ReadSchema);

                TreeLabel tl = new TreeLabel();

                if (ds.Tables.Count <= 0) return tl;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                   switch (dr["name"].ToString())
                    {
                        case "processed_text":
                            tl.Name = Convert.ToBoolean(dr["status"]);
                            break;
                        case "log_line":
                            tl.LogLine = Convert.ToBoolean(dr["status"]);
                            break;
                        case "log_event":
                            tl.LogEvent = Convert.ToBoolean(dr["status"]);
                            break;
                        case "log_subevent":
                            tl.LogSubEvent = Convert.ToBoolean(dr["status"]);
                            break;
                        case "log_datetime":
                            tl.LogDateTime = Convert.ToBoolean(dr["status"]);
                            break;
                        case "log_message":
                            tl.LogMessage = Convert.ToBoolean(dr["status"]);
                            break;
                    }
                }

                return tl;
            }
            catch (Exception ex)
            {
                throw new Exception("Load Tree Label Xml Error: " + ex.Message);
            }
        }

        private void LoadSiebelTreeView()
        {
            if (_siebelTreeView == null) return;

            // get icons
            treeViewSiebelTree.ImageList = _siebelTreeView.LoadEventIcons();

            // add the nodes to three with ignore list and tree label information
            treeViewSiebelTree.Nodes.Add(_siebelTreeView.GetTreeEvents(LoadIgnoreEvents(), LoadTreeLabel()).GetTreeNodes());

            // set treeview default icon
            treeViewSiebelTree.SelectedImageKey = @"default.png";
        }

        /*********************************** Button Ctrls ***********************************/
        // Open Log File
        private void buttonOpenLog_Click(object sender, EventArgs e)
        {
            try
            {
                DialogBox db = new DialogBox("log", "Siebel Log (*.log)|*.log");
                string auxLogFileName = db.OpenFile();

                _openLogFile = auxLogFileName != string.Empty ? auxLogFileName : null;

                // if not file to show leave
                if (string.IsNullOrEmpty(_openLogFile))
                {
                    MessageBox.Show(@"No file to load", @"Load Tree", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                if (MessageBox.Show(@"Build Tree ?", @"Open File", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Load TreeView
                    LoadTree();
                }
                else
                {
                    // clean old nodes if they exists, you are opening new file, you dont want to see old tree file
                    treeViewSiebelTree.Nodes.Clear();
                }


                // Load file to textbox in the end so that does not overload loading process
                LoadFileToTextBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Open Dialog Box", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Open Config File
        private void buttonOpenConfig_Click(object sender, EventArgs e)
        {
            DialogBox db = new DialogBox("xml", "Siebel Event Config File (*.xml)|*.xml");
            string auxCfgFileName = db.OpenFile();

            if (auxCfgFileName != string.Empty) _openConfigFile = auxCfgFileName;
        }

        // Expand Tree
        private void buttonExpandTree_Click(object sender, EventArgs e)
        {
            try
            {
                treeViewSiebelTree.ExpandAll();

                MessageBox.Show(@"Done", @"Expand Tree", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Expand Tree", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Compact tree
        private void buttonCollapse_Click(object sender, EventArgs e)
        {
            try
            {
                treeViewSiebelTree.CollapseAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Collapse Tree", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // operations that will execute when appl is closing
        private void FormNotepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // close events loading background worker safely (or not :))
                CloseEventsLoadBackGroundWorker();
            }
            catch {}
        }

        // open ignore list form
        private void buttonIgnore_Click(object sender, EventArgs e)
        {
            IgnoreEvents ie = new IgnoreEvents();
            ie.Show();
        }

        // refresh current list
        private void buttonRefreshTree_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show(@"Yes to reload file again. No to just refresh the tree", @"Refresh",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                LoadTree(); // Load TreeView
            else
            {
                // clean old nodes
                treeViewSiebelTree.Nodes.Clear();

                // fill tree with stuff
                LoadSiebelTreeView();
            }
                
        }

        // open mark & find form
        private void buttonMarkAndFind_Click(object sender, EventArgs e)
        {
            MarkAndFind maf = new MarkAndFind(this, treeViewSiebelTree.Nodes.Count == 0);
            maf.Show();
        }

        // node mouse click
        private void treeViewSiebelTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SiebelTreeNode stn = (SiebelTreeNode)e.Node;

            if (stn == null || stn.EventData == null || stn.EventData.Line <= 0) return;

            _selectedNode = stn;
        }

        // select the node and move to line
        private void buttonGoToLine_Click(object sender, EventArgs e)
        {
            if (_selectedNode == null || (_selectedNode != null && _selectedNode.EventData == null)) return;

            GoToTextBoxLine(_selectedNode.EventData.Line);
        }

        // bookmark line (just adds color)
        private void buttonBookMark_Click(object sender, EventArgs e)
        {
            int line = _fastColorTb.Selection.Start.iLine;

            if (_fastColorTb.Bookmarks.Count > 0)
            {
                Bookmark isLine = null;

                try { isLine = _fastColorTb.Bookmarks.First(x => x.LineIndex == line); } catch {}
                
                if (isLine == null)
                    BookmarkLine(line);
                else
                    UnBookmarkLine(line);    
            }
            else
                BookmarkLine(line);            
        }

        // change bookmark color
        private void buttonBookMarkCfg_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            DialogResult result = cd.ShowDialog();

            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // change default color
                _bookmarkColor = cd.Color;
            }
        }

        // change tree label
        private void buttonChangeTree_Click(object sender, EventArgs e)
        {
            ChangeTreeLabel ctl = new ChangeTreeLabel();
            ctl.Show();
        }

        // copy tree node
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (_selectedNode == null) return;

            Clipboard.SetText(_selectedNode.Text);
        }

        // about box open
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.Show();
        }

        private void FormNotepad_Load(object sender, EventArgs e)
        {

        }
    }
}
