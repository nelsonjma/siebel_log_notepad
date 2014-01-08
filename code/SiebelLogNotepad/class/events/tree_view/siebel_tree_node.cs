using System.Windows.Forms;

namespace Events.TreeView
{
    class SiebelTreeNode : TreeNode
    {
        public EventData EventData { get; set; }

        public SiebelTreeNode()
        {
            base.Text = string.Empty;
            base.ImageKey = "default.png";
        }

        public SiebelTreeNode(EventData evtData)
        {
            EventData = evtData;

            base.Text = EventData.Name;
            base.ImageKey = EventData.ImageKey == string.Empty
                        ? "default.png"
                        : EventData.ImageKey;
        }
    }
}



