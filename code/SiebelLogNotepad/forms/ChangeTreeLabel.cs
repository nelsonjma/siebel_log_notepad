using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiebelLogNotepad.forms
{
    public partial class ChangeTreeLabel : Form
    {
        private DataSet _treeLabel;

        private readonly string _filePath;

        public ChangeTreeLabel()
        {
            InitializeComponent();

            _filePath = Directory.GetCurrentDirectory() + @"\cfg\tree_labels.xml";

            _treeLabel = new DataSet();

            // load existing list
            LoadList();
        }

        private void SaveList()
        {
            try
            {
                _treeLabel.WriteXml(_filePath, XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Save Tree Label", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadList()
        {
            try
            {
                if (ListExists())
                {
                    _treeLabel = new DataSet();
                    _treeLabel.ReadXml(_filePath, XmlReadMode.ReadSchema);

                    // build checklist
                    BuildCheckListInfo();
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
            _treeLabel = new DataSet();
            DataTable tlDt = _treeLabel.Tables.Add();

            //-- Add columns to the data table
            tlDt.Columns.Add("status", typeof(bool));
            tlDt.Columns.Add("name", typeof(string));

            // save new list
            tlDt.Rows.Add(true, "processed_text");
            tlDt.Rows.Add(false, "log_line");
            tlDt.Rows.Add(false, "log_event");
            tlDt.Rows.Add(false, "log_subevent");
            tlDt.Rows.Add(false, "log_datetime");
            tlDt.Rows.Add(false, "log_message");

            SaveList();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Save List ?", @"Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            
            SaveCheckListInfo();
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

        private void StoreEventInDt(bool status, string name)
        {
            DataRow[] dr = _treeLabel.Tables[0].Select("name = '" + name + "'");

            if (dr.Length == 0) return;

            dr[0]["status"] = status;
        }

        private void SaveCheckListInfo()
        {
            StoreEventInDt(checkBoxProcessedText.Checked, "processed_text");
            StoreEventInDt(checkBoxLine.Checked, "log_line");
            StoreEventInDt(checkBoxLogEvent.Checked, "log_event");
            StoreEventInDt(checkBoxLogSubEvent.Checked, "log_subevent");
            StoreEventInDt(checkBoxLogDateTime.Checked, "log_datetime");
            StoreEventInDt(checkBoxLogMessage.Checked, "log_message");
        }

        private void BuildCheckListInfo()
        {
            try
            {
                foreach (DataRow dr in _treeLabel.Tables[0].Rows)
                {
                    switch (dr.ItemArray[1].ToString())
                    {
                        case "processed_text":
                            checkBoxProcessedText.Checked = Convert.ToBoolean(dr.ItemArray[0]);
                            break;
                        case "log_line":
                            checkBoxLine.Checked = Convert.ToBoolean(dr.ItemArray[0]);
                            break;
                        case "log_event":
                            checkBoxLogEvent.Checked = Convert.ToBoolean(dr.ItemArray[0]);
                            break;
                        case "log_subevent":
                            checkBoxLogSubEvent.Checked = Convert.ToBoolean(dr.ItemArray[0]);
                            break;
                        case "log_datetime":
                            checkBoxLogDateTime.Checked = Convert.ToBoolean(dr.ItemArray[0]);
                            break;
                        case "log_message":
                            checkBoxLogMessage.Checked = Convert.ToBoolean(dr.ItemArray[0]);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Tree Label Build CheckList", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

        
}
