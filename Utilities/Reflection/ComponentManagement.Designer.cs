namespace ESnail.Utilities.Reflection
{

    partial class ComponentManagement<TType>
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
            this.components = new System.ComponentModel.Container();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.tabsComoponents = new System.Windows.Forms.TabControl();
            this.tabComponents = new System.Windows.Forms.TabPage();
            this.panelComponents = new System.Windows.Forms.Panel();
            this.grbComponentList = new System.Windows.Forms.GroupBox();
            this.lvComponents = new System.Windows.Forms.ListView();
            this.clhAdapterID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhAdapterType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhAdapterVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhCompany = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clhDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolbarAdapter = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdRefreshComponent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdRemoveComponent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdAddComponent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.dlgOpenAdapterDll = new System.Windows.Forms.OpenFileDialog();
            this.toolTipError = new System.Windows.Forms.ToolTip(this.components);
            this.panelEditor.SuspendLayout();
            this.tabsComoponents.SuspendLayout();
            this.tabComponents.SuspendLayout();
            this.panelComponents.SuspendLayout();
            this.grbComponentList.SuspendLayout();
            this.toolbarAdapter.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 484);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(658, 22);
            this.StatusBar.TabIndex = 0;
            this.StatusBar.Text = "statusStrip1";
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.tabsComoponents);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Margin = new System.Windows.Forms.Padding(0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(5);
            this.panelEditor.Size = new System.Drawing.Size(658, 484);
            this.panelEditor.TabIndex = 1;
            // 
            // tabsComoponents
            // 
            this.tabsComoponents.Controls.Add(this.tabComponents);
            this.tabsComoponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsComoponents.Location = new System.Drawing.Point(5, 5);
            this.tabsComoponents.Name = "tabsComoponents";
            this.tabsComoponents.SelectedIndex = 0;
            this.tabsComoponents.Size = new System.Drawing.Size(648, 474);
            this.tabsComoponents.TabIndex = 0;
            // 
            // tabComponents
            // 
            this.tabComponents.Controls.Add(this.panelComponents);
            this.tabComponents.Location = new System.Drawing.Point(4, 22);
            this.tabComponents.Name = "tabComponents";
            this.tabComponents.Padding = new System.Windows.Forms.Padding(3);
            this.tabComponents.Size = new System.Drawing.Size(640, 448);
            this.tabComponents.TabIndex = 0;
            this.tabComponents.Text = "Components Management";
            this.tabComponents.UseVisualStyleBackColor = true;
            // 
            // panelComponents
            // 
            this.panelComponents.Controls.Add(this.grbComponentList);
            this.panelComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelComponents.Location = new System.Drawing.Point(3, 3);
            this.panelComponents.Margin = new System.Windows.Forms.Padding(0);
            this.panelComponents.Name = "panelComponents";
            this.panelComponents.Padding = new System.Windows.Forms.Padding(5);
            this.panelComponents.Size = new System.Drawing.Size(634, 442);
            this.panelComponents.TabIndex = 1;
            // 
            // grbComponentList
            // 
            this.grbComponentList.BackColor = System.Drawing.Color.Transparent;
            this.grbComponentList.Controls.Add(this.lvComponents);
            this.grbComponentList.Controls.Add(this.toolbarAdapter);
            this.grbComponentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbComponentList.Location = new System.Drawing.Point(5, 5);
            this.grbComponentList.Name = "grbComponentList";
            this.grbComponentList.Padding = new System.Windows.Forms.Padding(8, 3, 8, 7);
            this.grbComponentList.Size = new System.Drawing.Size(624, 432);
            this.grbComponentList.TabIndex = 3;
            this.grbComponentList.TabStop = false;
            this.grbComponentList.Text = "Component List";
            // 
            // lvComponents
            // 
            this.lvComponents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clhAdapterID,
            this.clhAdapterType,
            this.clhAdapterVersion,
            this.clhCompany,
            this.clhDescription});
            this.lvComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvComponents.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lvComponents.FullRowSelect = true;
            this.lvComponents.GridLines = true;
            this.lvComponents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvComponents.Location = new System.Drawing.Point(8, 42);
            this.lvComponents.MultiSelect = false;
            this.lvComponents.Name = "lvComponents";
            this.lvComponents.Size = new System.Drawing.Size(608, 383);
            this.lvComponents.TabIndex = 4;
            this.lvComponents.UseCompatibleStateImageBehavior = false;
            this.lvComponents.View = System.Windows.Forms.View.Details;
            // 
            // clhAdapterID
            // 
            this.clhAdapterID.Text = "ID";
            this.clhAdapterID.Width = 40;
            // 
            // clhAdapterType
            // 
            this.clhAdapterType.Text = "Type";
            this.clhAdapterType.Width = 200;
            // 
            // clhAdapterVersion
            // 
            this.clhAdapterVersion.Text = "Version";
            this.clhAdapterVersion.Width = 80;
            // 
            // clhCompany
            // 
            this.clhCompany.Text = "Company";
            this.clhCompany.Width = 100;
            // 
            // clhDescription
            // 
            this.clhDescription.Text = "Description";
            this.clhDescription.Width = 300;
            // 
            // toolbarAdapter
            // 
            this.toolbarAdapter.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarAdapter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.cmdRefreshComponent,
            this.toolStripSeparator2,
            this.cmdRemoveComponent,
            this.toolStripSeparator4,
            this.cmdAddComponent,
            this.toolStripSeparator5});
            this.toolbarAdapter.Location = new System.Drawing.Point(8, 17);
            this.toolbarAdapter.Name = "toolbarAdapter";
            this.toolbarAdapter.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarAdapter.Size = new System.Drawing.Size(608, 25);
            this.toolbarAdapter.TabIndex = 3;
            this.toolbarAdapter.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdRefreshComponent
            // 
            this.cmdRefreshComponent.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdRefreshComponent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            
            this.cmdRefreshComponent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRefreshComponent.Name = "cmdRefreshComponent";
            this.cmdRefreshComponent.Size = new System.Drawing.Size(50, 22);
            this.cmdRefreshComponent.Text = "Refresh";
            this.cmdRefreshComponent.Click += new System.EventHandler(this.cmdRefreshComponent_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdRemoveComponent
            // 
            this.cmdRemoveComponent.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdRemoveComponent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            
            this.cmdRemoveComponent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRemoveComponent.Name = "cmdRemoveComponent";
            this.cmdRemoveComponent.Size = new System.Drawing.Size(66, 22);
            this.cmdRemoveComponent.Text = "  Remove  ";
            this.cmdRemoveComponent.Click += new System.EventHandler(this.cmdRemoveComponent_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdAddComponent
            // 
            this.cmdAddComponent.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdAddComponent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            
            this.cmdAddComponent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdAddComponent.Name = "cmdAddComponent";
            this.cmdAddComponent.Size = new System.Drawing.Size(66, 22);
            this.cmdAddComponent.Text = "      Add     ";
            this.cmdAddComponent.Click += new System.EventHandler(this.cmdAddComponent_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // dlgOpenAdapterDll
            // 
            this.dlgOpenAdapterDll.Filter = "Component Library |*.dll;";
            this.dlgOpenAdapterDll.RestoreDirectory = true;
            this.dlgOpenAdapterDll.SupportMultiDottedExtensions = true;
            this.dlgOpenAdapterDll.Title = "Add Component";
            // 
            // toolTipError
            // 
            this.toolTipError.AutoPopDelay = 5000;
            this.toolTipError.InitialDelay = 1500;
            this.toolTipError.IsBalloon = true;
            this.toolTipError.ReshowDelay = 1000;
            this.toolTipError.ShowAlways = true;
            this.toolTipError.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTipError.ToolTipTitle = "Command";
            // 
            // frmComponentManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 506);
            this.Controls.Add(this.panelEditor);
            this.Controls.Add(this.StatusBar);
            
            this.Name = "frmComponentManagement";
            this.Text = "Component Management";
            this.panelEditor.ResumeLayout(false);
            this.tabsComoponents.ResumeLayout(false);
            this.tabComponents.ResumeLayout(false);
            this.panelComponents.ResumeLayout(false);
            this.grbComponentList.ResumeLayout(false);
            this.grbComponentList.PerformLayout();
            this.toolbarAdapter.ResumeLayout(false);
            this.toolbarAdapter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.TabControl tabsComoponents;
        private System.Windows.Forms.GroupBox grbComponentList;
        protected System.Windows.Forms.ListView lvComponents;
        private System.Windows.Forms.ColumnHeader clhAdapterID;
        private System.Windows.Forms.ColumnHeader clhAdapterType;
        private System.Windows.Forms.ColumnHeader clhAdapterVersion;
        private System.Windows.Forms.ColumnHeader clhCompany;
        private System.Windows.Forms.ColumnHeader clhDescription;
        private System.Windows.Forms.ToolStrip toolbarAdapter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton cmdRefreshComponent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton cmdRemoveComponent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton cmdAddComponent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.OpenFileDialog dlgOpenAdapterDll;
        private System.Windows.Forms.ToolTip toolTipError;
        protected System.Windows.Forms.TabPage tabComponents;
        protected System.Windows.Forms.Panel panelComponents;
    }
}