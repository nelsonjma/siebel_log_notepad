using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace SiebelLogNotepad.Forms
{
    public partial class IgnoreEvents : Form
    {
        private readonly string _filePath;

        private DataSet _ignoreDataSet;

        public IgnoreEvents()
        {
            InitializeComponent();

            _filePath = Directory.GetCurrentDirectory() + @"\cfg\ignore_events.xml";

            _ignoreDataSet = new DataSet();

            // load existing list
            LoadList();
        }

        private void SaveList()
        {
            try
            {
                _ignoreDataSet.WriteXml(_filePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Save Ignore List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadList()
        {
            try
            {
                if (ListExists())
                {
                    _ignoreDataSet.ReadXml(_filePath, XmlReadMode.ReadSchema);

                    // connect the list to the gridview
                    ConnectListToGridView();
                }
                else
                {
                    // if does not exists create new list
                    CreateNewList();

                    // reload list
                    LoadList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Load Ignore List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateNewList()
        {
            //-- Instantiate the data set and table
            _ignoreDataSet = new DataSet();
            DataTable ignDt = _ignoreDataSet.Tables.Add();

            //-- Add columns to the data table
            ignDt.Columns.Add("active", typeof(bool));
            ignDt.Columns.Add("event", typeof(string));
            ignDt.Columns.Add("sub_event", typeof(string));
            ignDt.Columns.Add("log_message", typeof(string));

            // save new list
            SaveList();
        }

        /************** Auxiliar Methods **************/
        private bool ListExists()
        {
            try
            {
                return File.Exists(_filePath);
            }
            catch (Exception ex)
            {
                throw new Exception("check if exists error: " + ex.Message);
            }
        }

        private void ConnectListToGridView()
        {
            dataGridViewIgnoreList.DataSource = _ignoreDataSet.Tables[0];
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Save List ?", @"Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                SaveList();
        }
    }
}
