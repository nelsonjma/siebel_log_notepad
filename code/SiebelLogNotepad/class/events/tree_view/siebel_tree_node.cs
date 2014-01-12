using System.Globalization;
using System.Windows.Forms;

namespace Events.TreeView
{
    class SiebelTreeNode : TreeNode
    {
        public EventData EventData { get; set; }
        public TreeLabel TreeLabel { get; set; }

        public SiebelTreeNode()
        {
            TreeLabel = null;
            EventData = null;

            base.Text = string.Empty;
            base.ImageKey = @"default.png";
        }

        public SiebelTreeNode(EventData evtData, TreeLabel treeLabel)
        {
            EventData = evtData;
            TreeLabel = treeLabel;

            base.Text = TreeLabel != null 
                        ? FormatText() 
                        : EventData.Name;

            base.ImageKey = EventData.ImageKey == string.Empty
                        ? "default.png"
                        : EventData.ImageKey;
        }

        private string FormatText()
        {
            if (TreeLabel == null || EventData == null) return string.Empty;

            string name = string.Empty;

            name += TreeLabel.LogLine ? EventData.Line.ToString(CultureInfo.InvariantCulture) + " - " : string.Empty;
            name += TreeLabel.LogDateTime ? EventData.LogDateTime + " - " : string.Empty;
            name += TreeLabel.LogEvent ? EventData.LogEvent + " - " : string.Empty;
            name += TreeLabel.LogSubEvent ? EventData.LogSubEvent + " - " : string.Empty;
            name += TreeLabel.Name ? EventData.Name + " - " : string.Empty;
            name += TreeLabel.LogMessage ? EventData.LogMessage + " - " : string.Empty;

            name = name.TrimEnd();

            return name[name.Length - 1] == '-' 
                ? name.Substring(0, name.Length - 1) 
                : name;
        }
    }
}



