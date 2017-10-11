
namespace ESnail.Device
{
    partial class frmAdapterManagerEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdapterManagerEditor));
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.tabsAdapterManager = new System.Windows.Forms.TabControl();
            this.tabToolList = new System.Windows.Forms.TabPage();
            this.panelToolList = new System.Windows.Forms.Panel();
            this.grbToolList = new System.Windows.Forms.GroupBox();
            this.lvAdapters = new System.Windows.Forms.ListView();
            this.clhAdapterID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhAdapterName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhAdapterType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhAdapterVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolbarAdapter = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdRefreshAdapters = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdRemoveAdapter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdAddAdapter = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.dlgOpenAdapterDll = new System.Windows.Forms.OpenFileDialog();
            this.panelEditor.SuspendLayout();
            this.tabsAdapterManager.SuspendLayout();
            this.tabToolList.SuspendLayout();
            this.panelToolList.SuspendLayout();
            this.grbToolList.SuspendLayout();
            this.toolbarAdapter.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 505);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(549, 22);
            this.StatusBar.TabIndex = 0;
            this.StatusBar.Text = "statusStrip1";
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.tabsAdapterManager);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(5);
            this.panelEditor.Size = new System.Drawing.Size(549, 505);
            this.panelEditor.TabIndex = 1;
            // 
            // tabsAdapterManager
            // 
            this.tabsAdapterManager.Controls.Add(this.tabToolList);
            this.tabsAdapterManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsAdapterManager.Location = new System.Drawing.Point(5, 5);
            this.tabsAdapterManager.Name = "tabsAdapterManager";
            this.tabsAdapterManager.SelectedIndex = 0;
            this.tabsAdapterManager.Size = new System.Drawing.Size(539, 495);
            this.tabsAdapterManager.TabIndex = 0;
            // 
            // tabToolList
            // 
            this.tabToolList.Controls.Add(this.panelToolList);
            this.tabToolList.Location = new System.Drawing.Point(4, 22);
            this.tabToolList.Name = "tabToolList";
            this.tabToolList.Padding = new System.Windows.Forms.Padding(5);
            this.tabToolList.Size = new System.Drawing.Size(531, 469);
            this.tabToolList.TabIndex = 1;
            this.tabToolList.Text = "Tool List";
            this.tabToolList.UseVisualStyleBackColor = true;
            // 
            // panelToolList
            // 
            this.panelToolList.Controls.Add(this.grbToolList);
            this.panelToolList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToolList.Location = new System.Drawing.Point(5, 5);
            this.panelToolList.Name = "panelToolList";
            this.panelToolList.Padding = new System.Windows.Forms.Padding(3);
            this.panelToolList.Size = new System.Drawing.Size(521, 459);
            this.panelToolList.TabIndex = 0;
            // 
            // grbToolList
            // 
            this.grbToolList.BackColor = System.Drawing.Color.Transparent;
            this.grbToolList.Controls.Add(this.lvAdapters);
            this.grbToolList.Controls.Add(this.toolbarAdapter);
            this.grbToolList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbToolList.Location = new System.Drawing.Point(3, 3);
            this.grbToolList.Name = "grbToolList";
            this.grbToolList.Padding = new System.Windows.Forms.Padding(8, 3, 8, 7);
            this.grbToolList.Size = new System.Drawing.Size(515, 453);
            this.grbToolList.TabIndex = 1;
            this.grbToolList.TabStop = false;
            this.grbToolList.Text = "Tool List";
            // 
            // lvAdapters
            // 
            this.lvAdapters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clhAdapterID,
            this.clhAdapterName,
            this.clhAdapterType,
            this.clhAdapterVersion});
            this.lvAdapters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAdapters.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lvAdapters.FullRowSelect = true;
            this.lvAdapters.GridLines = true;
            this.lvAdapters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvAdapters.Location = new System.Drawing.Point(8, 42);
            this.lvAdapters.MultiSelect = false;
            this.lvAdapters.Name = "lvAdapters";
            this.lvAdapters.Size = new System.Drawing.Size(499, 404);
            this.lvAdapters.TabIndex = 4;
            this.lvAdapters.UseCompatibleStateImageBehavior = false;
            this.lvAdapters.View = System.Windows.Forms.View.Details;
            this.lvAdapters.ItemActivate += new System.EventHandler(this.lvAdapters_ItemActivate);
            this.lvAdapters.Click += new System.EventHandler(this.lvAdapters_Click);
            // 
            // clhAdapterID
            // 
            this.clhAdapterID.Text = "ID";
            this.clhAdapterID.Width = 34;
            // 
            // clhAdapterName
            // 
            this.clhAdapterName.Text = "Name";
            this.clhAdapterName.Width = 109;
            // 
            // clhAdapterType
            // 
            this.clhAdapterType.Text = "Type";
            this.clhAdapterType.Width = 250;
            // 
            // clhAdapterVersion
            // 
            this.clhAdapterVersion.Text = "Version";
            this.clhAdapterVersion.Width = 100;
            // 
            // toolbarAdapter
            // 
            this.toolbarAdapter.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarAdapter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.cmdRefreshAdapters,
            this.toolStripSeparator2,
            this.cmdRemoveAdapter,
            this.toolStripSeparator4,
            this.cmdAddAdapter,
            this.toolStripSeparator5});
            this.toolbarAdapter.Location = new System.Drawing.Point(8, 17);
            this.toolbarAdapter.Name = "toolbarAdapter";
            this.toolbarAdapter.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarAdapter.Size = new System.Drawing.Size(499, 25);
            this.toolbarAdapter.TabIndex = 3;
            this.toolbarAdapter.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdRefreshAdapters
            // 
            this.cmdRefreshAdapters.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdRefreshAdapters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdRefreshAdapters.Image = ((System.Drawing.Image)(resources.GetObject("cmdRefreshAdapters.Image")));
            this.cmdRefreshAdapters.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRefreshAdapters.Name = "cmdRefreshAdapters";
            this.cmdRefreshAdapters.Size = new System.Drawing.Size(50, 22);
            this.cmdRefreshAdapters.Text = "Refresh";
            this.cmdRefreshAdapters.Click += new System.EventHandler(this.cmdRefreshAdapters_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdRemoveAdapter
            // 
            this.cmdRemoveAdapter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdRemoveAdapter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdRemoveAdapter.Image = ((System.Drawing.Image)(resources.GetObject("cmdRemoveAdapter.Image")));
            this.cmdRemoveAdapter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRemoveAdapter.Name = "cmdRemoveAdapter";
            this.cmdRemoveAdapter.Size = new System.Drawing.Size(66, 22);
            this.cmdRemoveAdapter.Text = "  Remove  ";
            this.cmdRemoveAdapter.Click += new System.EventHandler(this.cmdRemoveAdapter_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdAddAdapter
            // 
            this.cmdAddAdapter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdAddAdapter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdAddAdapter.Image = ((System.Drawing.Image)(resources.GetObject("cmdAddAdapter.Image")));
            this.cmdAddAdapter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdAddAdapter.Name = "cmdAddAdapter";
            this.cmdAddAdapter.Size = new System.Drawing.Size(66, 22);
            this.cmdAddAdapter.Text = "      Add     ";
            this.cmdAddAdapter.Click += new System.EventHandler(this.cmdAddAdapter_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // dlgOpenAdapterDll
            // 
            this.dlgOpenAdapterDll.Filter = "Device/Tools driver |*.dll;";
            this.dlgOpenAdapterDll.RestoreDirectory = true;
            this.dlgOpenAdapterDll.Title = "Add device / tools driver";
            // 
            // frmAdapterManagerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 527);
            this.Controls.Add(this.panelEditor);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAdapterManagerEditor";
            this.Text = "  Adapter Management";
            this.panelEditor.ResumeLayout(false);
            this.tabsAdapterManager.ResumeLayout(false);
            this.tabToolList.ResumeLayout(false);
            this.panelToolList.ResumeLayout(false);
            this.grbToolList.ResumeLayout(false);
            this.grbToolList.PerformLayout();
            this.toolbarAdapter.ResumeLayout(false);
            this.toolbarAdapter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.GroupBox grbToolList;
        private System.Windows.Forms.ListView lvAdapters;
        private System.Windows.Forms.ColumnHeader clhAdapterID;
        private System.Windows.Forms.ColumnHeader clhAdapterType;
        private System.Windows.Forms.ColumnHeader clhAdapterName;
        private System.Windows.Forms.ToolStrip toolbarAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton cmdRefreshAdapters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        internal System.Windows.Forms.Panel panelToolList;
        private System.Windows.Forms.TabControl tabsAdapterManager;
        internal System.Windows.Forms.TabPage tabToolList;
        private System.Windows.Forms.ToolStripButton cmdRemoveAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton cmdAddAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.OpenFileDialog dlgOpenAdapterDll;
        private System.Windows.Forms.ColumnHeader clhAdapterVersion;
    }
}