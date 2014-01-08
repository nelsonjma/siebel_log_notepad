using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SiebelLogNotepad.forms
{
    public partial class MarkAndFind : Form
    {
        // default color
        private Color _defaultColor;

        // connect to notepad form to execute mark and find methods
        private readonly FormNotepad _fn;

        public MarkAndFind(FormNotepad fn)
        {
            InitializeComponent();

            // default color
            _defaultColor = Color.DarkOrange;

            // set Change Color background color = to default color
            buttonChangeColor.BackColor = _defaultColor;

            // connect to notepad form to execute mark and find methods
            _fn = fn;
        }


        /*********************************** Button Ctrls ***********************************/
        // Change color 
        private void buttonChangeColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            DialogResult result = cd.ShowDialog();
            
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // change default color
                _defaultColor = cd.Color;

                // set Change Color background color = to default color
                buttonChangeColor.BackColor = _defaultColor;
            }
        }

        // Mark text in tree nodes or textbox
        private void buttonMarkAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonTree.Checked && textBoxFind.Text != string.Empty)
                    _fn.MarkTreeNode(textBoxFind.Text, _defaultColor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Mark All", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonTree.Checked && textBoxFind.Text != string.Empty)
                    _fn.Find(textBoxFind.Text, _defaultColor);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Find Next", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
