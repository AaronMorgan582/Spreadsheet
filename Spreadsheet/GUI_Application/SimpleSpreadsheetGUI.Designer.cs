/// <summary>
///   Original Author: Joe Zachary
///   Further Authors: H. James de St. Germain
///   
///   Dates          : 2012-ish - Original 
///                    2020     - Updated for use with ASP Core
///                    
///   This code represents a Windows Form element for a Spreadsheet
///   
///   This code is the "auto-generated" portion of the SimpleSpreadsheetGUI.
///   See the SimpleSpreadsheetGUI.cs for "hand-written" code.
///  
/// </summary>

using SpreadsheetGrid_Framework;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CS3500_Spreadsheet_GUI_Example
{
    partial class SimpleSpreadsheetGUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bg_worker = new BackgroundWorker();
            this.autoSaveLabel = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainControlArea = new System.Windows.Forms.FlowLayoutPanel();
            this.grid_widget = new SpreadsheetGrid_Framework.SpreadsheetGridWidget();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Save_Button = new System.Windows.Forms.Button();
            this.Input_Textbox = new System.Windows.Forms.TextBox();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.MainControlArea.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem, this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(584, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openMenuItem,
            this.saveAsMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // helpMeToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(139, 41);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // saveAsMenuItem
            // 
            this.saveAsMenuItem.Name = "saveAsMenuItem";
            this.saveAsMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveAsMenuItem.Text = "Save As...";
            this.saveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(403, 48);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.OpenMenuItem_Click);
            // 
            // MainControlArea
            // 
            this.MainControlArea.AutoSize = true;
            this.MainControlArea.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MainControlArea.BackColor = System.Drawing.Color.Coral;
            this.MainControlArea.Controls.Add(this.Save_Button);
            this.MainControlArea.Controls.Add(this.Input_Textbox);
            this.MainControlArea.Controls.Add(this.autoSaveLabel);
            this.MainControlArea.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.MainControlArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainControlArea.Location = new System.Drawing.Point(3, 3);
            this.MainControlArea.MinimumSize = new System.Drawing.Size(100, 100);
            this.MainControlArea.Name = "MainControlArea";
            this.MainControlArea.Size = new System.Drawing.Size(578, 100);
            this.MainControlArea.TabIndex = 4;
            //
            // Background Worker
            //
            this.bg_worker.DoWork += SetCell;
            this.bg_worker.RunWorkerCompleted += SetCellDone;
            // 
            // grid_widget
            // 
            this.grid_widget.AutoSize = true;
            this.grid_widget.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.grid_widget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_widget.Location = new System.Drawing.Point(3, 103);
            this.grid_widget.MaximumSize = new System.Drawing.Size(2100, 2000);
            this.grid_widget.Name = "grid_widget";
            this.grid_widget.Size = new System.Drawing.Size(578, 231);
            this.grid_widget.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.MainControlArea, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grid_widget, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 337);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // Save_Button
            // 
            this.Save_Button.Location = new System.Drawing.Point(3, 3);
            this.Save_Button.Name = "Save_Button";
            this.Save_Button.Size = new System.Drawing.Size(75, 23);
            this.Save_Button.TabIndex = 0;
            this.Save_Button.Text = "Save";
            this.Save_Button.UseVisualStyleBackColor = true;
            this.Save_Button.Click += new System.EventHandler(this.Save_Button_Click);
            // 
            // autoSaveLabel
            // 
            this.autoSaveLabel.AutoSize = true;
            this.autoSaveLabel.Visible = false;
            //this.autoSaveLabel.Location = new System.Drawing.Point(614, 0);
            this.autoSaveLabel.Location = new System.Drawing.Point(200, 200);
            this.autoSaveLabel.Name = "autoSaveLabel";
            this.autoSaveLabel.Size = new System.Drawing.Size(79, 29);
            this.autoSaveLabel.TabIndex = 3;
            this.autoSaveLabel.Text = "Autosaving...";
            // 
            // Input_Textbox
            // 
            this.Input_Textbox.Location = new System.Drawing.Point(170, 3);
            this.Input_Textbox.Name = "Input_Textbox";
            this.Input_Textbox.Size = new System.Drawing.Size(100, 20);
            this.Input_Textbox.TabIndex = 2;
            this.Input_Textbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Input_Textbox_KeyPress);
            // 
            // SimpleSpreadsheetGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "SimpleSpreadsheetGUI";
            this.Text = "Spreadsheet";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.MainControlArea.ResumeLayout(false);
            this.MainControlArea.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SimpleSpreadsheetGUI_FormClosing);
        }

        #endregion

        
        internal SpreadsheetGridWidget grid_widget;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;

        private FlowLayoutPanel MainControlArea;
        private TableLayoutPanel tableLayoutPanel1;
        private Button Save_Button;
        internal TextBox Input_Textbox;

        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem saveAsMenuItem;
        private ToolStripMenuItem openMenuItem;
        private BackgroundWorker bg_worker;
        private Label autoSaveLabel;

    }
}

