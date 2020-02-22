﻿/// <summary>
///   Original Author: Joe Zachary
///   Further Authors: H. James de St. Germain
///   
///   Dates          : 2012-ish - Original 
///                    2020     - Updated for use with ASP Core
///                    
///   This code represents a Windows Form element for a Spreadsheet. It includes
///   a Menu Bar with two operations (close/new) as well as the GRID of the spreadsheet.
///   The GRID is a separate class found in SpreadsheetGridWidget
///   
///   This code represents manual elements added to the GUI as well as the ability
///   to show a pop up of information, and the event handlers for a New window and to Close the window.
///
///   See the SimpleSpreadsheetGUIExample.Designer.cs for "auto-generated" code.
///   
///   This code relies on the SpreadsheetPanel "widget"
///  
/// </summary>

using SpreadsheetGrid_Framework;
using SS;
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace CS3500_Spreadsheet_GUI_Example
{
    public partial class SimpleSpreadsheetGUI : Form
    {
        AbstractSpreadsheet spreadsheet;
        string[] letters = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private string saveFilePath;
        public SimpleSpreadsheetGUI()
        {
            this.grid_widget      = new SpreadsheetGridWidget();

            spreadsheet = new Spreadsheet(s => true, s => s.ToUpper(), "six");

            // Call the AutoGenerated code
            InitializeComponent();

            // Add event handler and select a start cell
            grid_widget.SelectionChanged += DisplaySelection;
            grid_widget.SetSelection(0, 0, false);

        }

        public SimpleSpreadsheetGUI(string filename)
        {
            this.grid_widget = new SpreadsheetGridWidget();

            spreadsheet = spreadsheet = new Spreadsheet(filename, s => true, s => s.ToUpper(), "six");
            saveFilePath = filename;
            // Call the AutoGenerated code
            InitializeComponent();

            // Add event handler and select a start cell
            grid_widget.SelectionChanged += DisplaySelection;
            grid_widget.SetSelection(0, 0, false);

        }

        /// <summary>
        /// Given a spreadsheet, find the current selected cell and
        /// create a popup that contains the information from that cell
        /// </summary>
        /// <param name="ss"></param>
        private void DisplaySelection(SpreadsheetGridWidget ss)
        {
            int row, col;

            string value;
            ss.GetSelection(out col, out row);
            ss.GetValue(col, row, out value);

            string name = letters[col] + row;
            object toboloie = spreadsheet.GetCellValue(name);
            sample_textbox.Text = spreadsheet.GetCellContents(name).ToString();
        }

        // Deals with the New menu
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            Spreadsheet_Window.getAppContext().RunForm(new SimpleSpreadsheetGUI());
        }

        private void openMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Spreadsheet Files (*.sprd) | *.sprd |All files (*.*) | *.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                Spreadsheet_Window.getAppContext().RunForm(new SimpleSpreadsheetGUI(open.FileName));
                
                // Ask TA about this
                Close();
            }
        }

        // Deals with the Close menu
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Spreadsheet Files (*.sprd) | *.sprd |All files (*.*) | *.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                spreadsheet.Save(save.FileName);
                saveFilePath = save.FileName;
            }
        }

        /// <summary>
        /// Example of how to use a button
        /// </summary>
        /// <param name="sender"> not used </param>
        /// <param name="e"> not used </param>
        private void sample_button_Click(object sender, EventArgs e)
        {
            //grid_widget.SetValue(4, 5, "hello");
            //grid_widget.SetSelection(4, 5);
            if(saveFilePath != null)
            {
                spreadsheet.Save(saveFilePath);
                MessageBox.Show("Saved.");
            }
            else
            {
                Save();
            }
        }

        /// <summary>
        /// Checkbox handler
        /// </summary>
        /// <param name="sender"> the checkbox (note the casting operator as)</param>
        /// <param name="e">not used</param>
        private void sample_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                grid_widget.SetValue(1, 1, "checked");
            }
            else
            {
                grid_widget.SetValue(1, 1, "not checked");
            }

        }

        /// <summary>
        /// Textbox handler
        /// </summary>
        /// <param name="sender"> the textbox </param>
        /// <param name="e">not used</param>
        private void sample_textbox_TextChanged(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            int col,row;

            grid_widget.GetSelection(out col, out row);

            grid_widget.SetValue(col, row, box.Text);

        }

        private void sample_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Return)
            {
                TextBox box = sender as TextBox;

                int col, row;

                grid_widget.GetSelection(out col, out row);
                string cellName = letters[col] + (row + 1);
                spreadsheet.SetContentsOfCell(cellName, box.Text);
                grid_widget.SetValue(col, row, spreadsheet.GetCellValue(cellName).ToString());
            }
        }
    }
}
