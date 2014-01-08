using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiebelLogNotepad
{
    /// <summary>
    /// Open/Save Dialog Box
    /// </summary>
    class DialogBox
    {
        private string FileDialogDefaultExt { get; set; }   // -> "xml";
        private string FileDialogFilters { get; set; }      // -> "XML files (*.xml)|*.xml";

        /// <summary>
        /// Open/Save Dialog Box
        /// </summary>
        /// <param name="defaultExt">sample: xml</param>
        /// <param name="filters">sample: XML files (*.xml)|*.xml</param>
        public DialogBox(string defaultExt, string filters)
        {
            FileDialogDefaultExt = defaultExt;
            FileDialogFilters = filters;
        }

        public string OpenFile()
        {
            string filename = string.Empty;

            OpenFileDialog ofd = null;

            try
            {
                ofd = new OpenFileDialog { AddExtension = true, DefaultExt = FileDialogDefaultExt, Filter = FileDialogFilters };

                if (ofd.ShowDialog() == DialogResult.OK) filename = ofd.FileName;
            }
            catch (Exception ex)
            {
                throw new Exception("Open DialogBox error: " + ex.Message);
            }
            finally
            {
                if (ofd != null) ofd.Dispose();    
            }

            return filename;
        }

        public string SaveFile()
        {
            string filename = string.Empty;

            SaveFileDialog sfd = null;

            try
            {
                sfd = new SaveFileDialog { AddExtension = true, DefaultExt = FileDialogDefaultExt, Filter = FileDialogFilters };

                if (sfd.ShowDialog() == DialogResult.OK) filename = sfd.FileName;
            }
            catch (Exception ex)
            {
                throw new Exception("Open DialogBox error: " + ex.Message);
            }
            finally
            {
                if (sfd != null) sfd.Dispose();
            }

            return filename;
        }
    }
}
