using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        // siebel log file
        private string _openLogFile;
        
        // siebel config file
        private string _openConfigFile;

        // current folder
        private readonly string _defaultPath;

        private readonly string _originalFormHeaderText;

        private BackgroundWorker _eventLoadBackgroundWorker;

        // find next events
        private SiebelTreeNode _findedSiebelTreeNode;
        private readonly List<int> _findedSiebelNodePos;

        private FastColoredTextBox _fastColorTb;

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

            // store form header text
            _originalFormHeaderText = base.Text;

            // find next initialization
            _findedSiebelTreeNode = null;
            _findedSiebelNodePos = new List<int>();
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
                    Zoom = 100
                };

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

        /*********************************** Find Next Tree Events ***********************************/
        /// <summary>
        /// Find text in nodes
        /// </summary>
        public void Find(string value, Color clr)
        {
            try
            {
                if (_findedSiebelTreeNode == null)
                    FindFirst(value, clr);
                else
                    FindNext(_findedSiebelTreeNode, _findedSiebelNodePos[_findedSiebelNodePos.Count - 1], value, clr);
            }
            catch (Exception ex)
            {
                throw new Exception("Find error: " + ex.Message);
            }
            
        }

        /// <summary>
        /// Find first node
        /// </summary>
        private void FindFirst(string value, Color clr)
        {
            TreeNodeCollection tnc = treeViewSiebelTree.Nodes;

            for (int i = 0; i < tnc.Count; i++)
            {
                SiebelTreeNode stn = (SiebelTreeNode)tnc[i];

                _findedSiebelNodePos.Add(i);

                if (FindAuxiliar(stn, 0, value, clr)) return;

                _findedSiebelNodePos.RemoveAt(_findedSiebelNodePos.Count - 1);
            }
        }

        /// <summary>
        /// Find Next Position
        /// </summary>
        private void FindNext(SiebelTreeNode tns, int iPos, string value, Color clr)
        {
            while (true)
            {
                if (FindAuxiliar(tns, iPos, value, clr)) return;

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
                    FindFirst(value, clr);

                    return;
                }
            }
        }

        /// <summary>
        /// Sub method used used in MoveToFirstFind and FindFromPrevious
        /// </summary>
        private bool FindAuxiliar(SiebelTreeNode tns, int iPos, string value, Color clr)
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
                if (FindAuxiliar(tn, 0, value, clr)) return true;

                // if not the one remove the node position
                _findedSiebelNodePos.RemoveAt(_findedSiebelNodePos.Count - 1);
            }

            return false;
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
                MessageBox.Show(ex.Message, "Load Tree", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                SiebelTreeView stv = (SiebelTreeView) e.Result;

                // get icons
                treeViewSiebelTree.ImageList = stv.LoadEventIcons();
                
                // add the nodes to three
                treeViewSiebelTree.Nodes.Add(stv.GetTreeEvents(LoadIgnoreEvents()).GetTreeNodes());
                
                // set treeview default icon
                treeViewSiebelTree.SelectedImageKey = @"default.png";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Write Events TreeView", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Background Worker - reports progress
        private void EventLoadBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try { this.Text = _originalFormHeaderText + @" " + e.ProgressPercentage + @"%"; } catch {}            
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
                    MessageBox.Show(@"No file to load", @"Load Tree", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                }

                // Load TreeView
                LoadTree();

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
            // Load TreeView
            LoadTree();
        }

        // open mark & find form
        private void buttonMarkAndFind_Click(object sender, EventArgs e)
        {
            // if no nodes go away
            if (treeViewSiebelTree.Nodes.Count == 0) return;

            MarkAndFind maf = new MarkAndFind(this);
            maf.Show();
        }
    }
}
