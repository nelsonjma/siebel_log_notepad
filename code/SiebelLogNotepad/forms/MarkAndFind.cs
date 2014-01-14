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

        // default text
        private string _selectedText;

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

            // initialize selected text
            _selectedText = string.Empty;
        }

        /// <summary>
        ///  If is new text then restart find variables
        /// </summary>
        private void CheckIfNewText()
        {
            if (_selectedText != textBoxFind.Text)
                _fn.InitializeFindVariables();

            // send text to buffer so that in can be compared in the next button click;
            _selectedText = textBoxFind.Text;
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
                CheckIfNewText();

                if (textBoxFind.Text == string.Empty) return;

                if (radioButtonTree.Checked)
                    _fn.MarkTreeNode(textBoxFind.Text, _defaultColor);
                else
                    _fn.MarkTextBox(textBoxFind.Text, _defaultColor);

                MessageBox.Show(@"Done", @"Mark", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Mark All", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // find in the tree or the textbox
        private void buttonFindNext_Click(object sender, EventArgs e)
        {
            try
            {
                CheckIfNewText();

                if (textBoxFind.Text == string.Empty) return;

                if (radioButtonTree.Checked)
                    _fn.FindInTree(textBoxFind.Text, _defaultColor);
                else
                    _fn.FindInTextBox(textBoxFind.Text, _defaultColor, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Find Next", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonFindPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                CheckIfNewText();

                if (textBoxFind.Text == string.Empty) return;
                    
                if (radioButtonTree.Checked)
                    MessageBox.Show(@"Option not available in Tree", @"Find Previous", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    _fn.FindInTextBox(textBoxFind.Text, _defaultColor, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Find Previous", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
