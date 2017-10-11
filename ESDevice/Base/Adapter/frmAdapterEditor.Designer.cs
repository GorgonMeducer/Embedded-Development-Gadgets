namespace ESnail.Device
{
    partial class frmAdapterEditor
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
                _Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdapterEditor));
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.tabsAdapterEditor = new System.Windows.Forms.TabControl();
            this.tabDeviceManager = new System.Windows.Forms.TabPage();
            this.panelDeviceManager = new System.Windows.Forms.Panel();
            this.tabCommunication = new System.Windows.Forms.TabPage();
            this.panelCommunication = new System.Windows.Forms.Panel();
            this.grbCommand = new System.Windows.Forms.GroupBox();
            this.cmdDoIt = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtWriteBlock = new System.Windows.Forms.TextBox();
            this.labWriteBlock = new System.Windows.Forms.Label();
            this.chkWriteHex = new System.Windows.Forms.CheckBox();
            this.txtReadBlock = new System.Windows.Forms.TextBox();
            this.labReadBlock = new System.Windows.Forms.Label();
            this.chkHexDisplay = new System.Windows.Forms.CheckBox();
            this.toolbarCMDB = new System.Windows.Forms.ToolStrip();
            this.labWriteWord = new System.Windows.Forms.ToolStripLabel();
            this.txtWriteWord = new System.Windows.Forms.ToolStripTextBox();
            this.txtReadWord = new System.Windows.Forms.ToolStripTextBox();
            this.labReadWord = new System.Windows.Forms.ToolStripLabel();
            this.toolbarCMDC = new System.Windows.Forms.ToolStrip();
            this.labCommandType = new System.Windows.Forms.ToolStripLabel();
            this.comboCMDType = new System.Windows.Forms.ToolStripComboBox();
            this.labTelegraph = new System.Windows.Forms.ToolStripLabel();
            this.comboTelegraph = new System.Windows.Forms.ToolStripComboBox();
            this.txtHelp = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labBrief = new System.Windows.Forms.Label();
            this.txtBrief = new System.Windows.Forms.TextBox();
            this.toolbarCMDA = new System.Windows.Forms.ToolStrip();
            this.labCommand = new System.Windows.Forms.ToolStripLabel();
            this.txtCommand = new System.Windows.Forms.ToolStripTextBox();
            this.labAddress = new System.Windows.Forms.ToolStripLabel();
            this.comboAddress = new System.Windows.Forms.ToolStripComboBox();
            this.labSubAddress = new System.Windows.Forms.ToolStripLabel();
            this.txtSubAddress = new System.Windows.Forms.ToolStripTextBox();
            this.labTimeoutSetting = new System.Windows.Forms.ToolStripLabel();
            this.comboTimeout = new System.Windows.Forms.ToolStripComboBox();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.lvDebugMessage = new System.Windows.Forms.ListView();
            this.chMSGDirection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDisplayData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBrief = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolbarDebug = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdEnableDebug = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tabInformation = new System.Windows.Forms.TabPage();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipError = new System.Windows.Forms.ToolTip(this.components);
            this.timerRefreshDebugMessage = new System.Windows.Forms.Timer(this.components);
            this.panelEditor.SuspendLayout();
            this.tabsAdapterEditor.SuspendLayout();
            this.tabDeviceManager.SuspendLayout();
            this.tabCommunication.SuspendLayout();
            this.panelCommunication.SuspendLayout();
            this.grbCommand.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolbarCMDB.SuspendLayout();
            this.toolbarCMDC.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolbarCMDA.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.grpDebug.SuspendLayout();
            this.toolbarDebug.SuspendLayout();
            this.tabInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 529);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(648, 22);
            this.StatusBar.TabIndex = 0;
            this.StatusBar.Text = "statusStrip1";
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.tabsAdapterEditor);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(5);
            this.panelEditor.Size = new System.Drawing.Size(648, 529);
            this.panelEditor.TabIndex = 1;
            // 
            // tabsAdapterEditor
            // 
            this.tabsAdapterEditor.Controls.Add(this.tabDeviceManager);
            this.tabsAdapterEditor.Controls.Add(this.tabCommunication);
            this.tabsAdapterEditor.Controls.Add(this.tabDebug);
            this.tabsAdapterEditor.Controls.Add(this.tabInformation);
            this.tabsAdapterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsAdapterEditor.Location = new System.Drawing.Point(5, 5);
            this.tabsAdapterEditor.Name = "tabsAdapterEditor";
            this.tabsAdapterEditor.SelectedIndex = 0;
            this.tabsAdapterEditor.Size = new System.Drawing.Size(638, 519);
            this.tabsAdapterEditor.TabIndex = 2;
            // 
            // tabDeviceManager
            // 
            this.tabDeviceManager.Controls.Add(this.panelDeviceManager);
            this.tabDeviceManager.Location = new System.Drawing.Point(4, 22);
            this.tabDeviceManager.Name = "tabDeviceManager";
            this.tabDeviceManager.Padding = new System.Windows.Forms.Padding(5);
            this.tabDeviceManager.Size = new System.Drawing.Size(630, 493);
            this.tabDeviceManager.TabIndex = 0;
            this.tabDeviceManager.Text = "Device Manager";
            this.tabDeviceManager.UseVisualStyleBackColor = true;
            // 
            // panelDeviceManager
            // 
            this.panelDeviceManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDeviceManager.Location = new System.Drawing.Point(5, 5);
            this.panelDeviceManager.Name = "panelDeviceManager";
            this.panelDeviceManager.Padding = new System.Windows.Forms.Padding(5);
            this.panelDeviceManager.Size = new System.Drawing.Size(620, 483);
            this.panelDeviceManager.TabIndex = 0;
            // 
            // tabCommunication
            // 
            this.tabCommunication.Controls.Add(this.panelCommunication);
            this.tabCommunication.Location = new System.Drawing.Point(4, 22);
            this.tabCommunication.Name = "tabCommunication";
            this.tabCommunication.Padding = new System.Windows.Forms.Padding(5);
            this.tabCommunication.Size = new System.Drawing.Size(630, 493);
            this.tabCommunication.TabIndex = 1;
            this.tabCommunication.Text = "Communication";
            this.tabCommunication.UseVisualStyleBackColor = true;
            // 
            // panelCommunication
            // 
            this.panelCommunication.Controls.Add(this.grbCommand);
            this.panelCommunication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCommunication.Location = new System.Drawing.Point(5, 5);
            this.panelCommunication.Name = "panelCommunication";
            this.panelCommunication.Padding = new System.Windows.Forms.Padding(5);
            this.panelCommunication.Size = new System.Drawing.Size(620, 483);
            this.panelCommunication.TabIndex = 1;
            // 
            // grbCommand
            // 
            this.grbCommand.Controls.Add(this.cmdDoIt);
            this.grbCommand.Controls.Add(this.splitContainer1);
            this.grbCommand.Controls.Add(this.toolbarCMDB);
            this.grbCommand.Controls.Add(this.toolbarCMDC);
            this.grbCommand.Controls.Add(this.txtHelp);
            this.grbCommand.Controls.Add(this.panel1);
            this.grbCommand.Controls.Add(this.toolbarCMDA);
            this.grbCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbCommand.Location = new System.Drawing.Point(5, 5);
            this.grbCommand.Name = "grbCommand";
            this.grbCommand.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.grbCommand.Size = new System.Drawing.Size(610, 473);
            this.grbCommand.TabIndex = 0;
            this.grbCommand.TabStop = false;
            this.grbCommand.Text = "Command";
            // 
            // cmdDoIt
            // 
            this.cmdDoIt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDoIt.Location = new System.Drawing.Point(513, 352);
            this.cmdDoIt.Name = "cmdDoIt";
            this.cmdDoIt.Size = new System.Drawing.Size(87, 22);
            this.cmdDoIt.TabIndex = 6;
            this.cmdDoIt.Text = "Execute";
            this.cmdDoIt.UseVisualStyleBackColor = true;
            this.cmdDoIt.Click += new System.EventHandler(this.cmdDoIt_Click);
            this.cmdDoIt.MouseEnter += new System.EventHandler(this.cmdDoIt_MouseEnter);
            this.cmdDoIt.MouseLeave += new System.EventHandler(this.cmdDoIt_MouseLeave);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Location = new System.Drawing.Point(10, 91);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtWriteBlock);
            this.splitContainer1.Panel1.Controls.Add(this.labWriteBlock);
            this.splitContainer1.Panel1.Controls.Add(this.chkWriteHex);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtReadBlock);
            this.splitContainer1.Panel2.Controls.Add(this.labReadBlock);
            this.splitContainer1.Panel2.Controls.Add(this.chkHexDisplay);
            this.splitContainer1.Size = new System.Drawing.Size(590, 258);
            this.splitContainer1.SplitterDistance = 129;
            this.splitContainer1.TabIndex = 5;
            // 
            // txtWriteBlock
            // 
            this.txtWriteBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWriteBlock.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtWriteBlock.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.txtWriteBlock.Location = new System.Drawing.Point(0, 27);
            this.txtWriteBlock.Margin = new System.Windows.Forms.Padding(0);
            this.txtWriteBlock.Multiline = true;
            this.txtWriteBlock.Name = "txtWriteBlock";
            this.txtWriteBlock.Size = new System.Drawing.Size(590, 102);
            this.txtWriteBlock.TabIndex = 2;
            this.toolTip.SetToolTip(this.txtWriteBlock, "ABC");
            this.txtWriteBlock.TextChanged += new System.EventHandler(this.txtWriteBlock_TextChanged);
            this.txtWriteBlock.MouseEnter += new System.EventHandler(this.txtWriteBlock_MouseEnter);
            this.txtWriteBlock.MouseLeave += new System.EventHandler(this.txtWriteBlock_MouseLeave);
            // 
            // labWriteBlock
            // 
            this.labWriteBlock.AutoSize = true;
            this.labWriteBlock.Location = new System.Drawing.Point(8, 8);
            this.labWriteBlock.Name = "labWriteBlock";
            this.labWriteBlock.Size = new System.Drawing.Size(71, 12);
            this.labWriteBlock.TabIndex = 1;
            this.labWriteBlock.Text = "Write Block";
            this.labWriteBlock.Click += new System.EventHandler(this.labWriteBlock_Click);
            // 
            // chkWriteHex
            // 
            this.chkWriteHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkWriteHex.AutoSize = true;
            this.chkWriteHex.Location = new System.Drawing.Point(545, 8);
            this.chkWriteHex.Name = "chkWriteHex";
            this.chkWriteHex.Size = new System.Drawing.Size(42, 16);
            this.chkWriteHex.TabIndex = 0;
            this.chkWriteHex.Text = "HEX";
            this.chkWriteHex.UseVisualStyleBackColor = true;
            this.chkWriteHex.CheckedChanged += new System.EventHandler(this.chkWriteHex_CheckedChanged);
            this.chkWriteHex.MouseEnter += new System.EventHandler(this.chkWriteHex_MouseEnter);
            this.chkWriteHex.MouseLeave += new System.EventHandler(this.chkWriteHex_MouseLeave);
            // 
            // txtReadBlock
            // 
            this.txtReadBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReadBlock.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtReadBlock.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtReadBlock.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.txtReadBlock.Location = new System.Drawing.Point(0, 24);
            this.txtReadBlock.Margin = new System.Windows.Forms.Padding(0);
            this.txtReadBlock.Multiline = true;
            this.txtReadBlock.Name = "txtReadBlock";
            this.txtReadBlock.ReadOnly = true;
            this.txtReadBlock.Size = new System.Drawing.Size(590, 102);
            this.txtReadBlock.TabIndex = 5;
            this.toolTip.SetToolTip(this.txtReadBlock, "ABC");
            this.txtReadBlock.MouseEnter += new System.EventHandler(this.txtReadBlock_MouseEnter);
            this.txtReadBlock.MouseLeave += new System.EventHandler(this.txtReadBlock_MouseLeave);
            // 
            // labReadBlock
            // 
            this.labReadBlock.AutoSize = true;
            this.labReadBlock.Location = new System.Drawing.Point(8, 6);
            this.labReadBlock.Name = "labReadBlock";
            this.labReadBlock.Size = new System.Drawing.Size(65, 12);
            this.labReadBlock.TabIndex = 4;
            this.labReadBlock.Text = "Read Block";
            this.labReadBlock.Click += new System.EventHandler(this.labReadBlock_Click);
            // 
            // chkHexDisplay
            // 
            this.chkHexDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkHexDisplay.AutoSize = true;
            this.chkHexDisplay.Location = new System.Drawing.Point(544, 6);
            this.chkHexDisplay.Name = "chkHexDisplay";
            this.chkHexDisplay.Size = new System.Drawing.Size(42, 16);
            this.chkHexDisplay.TabIndex = 3;
            this.chkHexDisplay.Text = "HEX";
            this.chkHexDisplay.UseVisualStyleBackColor = true;
            this.chkHexDisplay.CheckedChanged += new System.EventHandler(this.chkHexDisplay_CheckedChanged);
            this.chkHexDisplay.MouseEnter += new System.EventHandler(this.chkHexDisplay_MouseEnter);
            this.chkHexDisplay.MouseLeave += new System.EventHandler(this.chkHexDisplay_MouseLeave);
            // 
            // toolbarCMDB
            // 
            this.toolbarCMDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolbarCMDB.AutoSize = false;
            this.toolbarCMDB.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbarCMDB.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarCMDB.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labWriteWord,
            this.txtWriteWord,
            this.txtReadWord,
            this.labReadWord});
            this.toolbarCMDB.Location = new System.Drawing.Point(10, 68);
            this.toolbarCMDB.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.toolbarCMDB.Name = "toolbarCMDB";
            this.toolbarCMDB.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCMDB.Size = new System.Drawing.Size(590, 23);
            this.toolbarCMDB.TabIndex = 4;
            this.toolbarCMDB.Text = "toolStrip1";
            // 
            // labWriteWord
            // 
            this.labWriteWord.Name = "labWriteWord";
            this.labWriteWord.Size = new System.Drawing.Size(73, 20);
            this.labWriteWord.Text = "  Write Word";
            this.labWriteWord.Click += new System.EventHandler(this.labWriteWord_Click);
            // 
            // txtWriteWord
            // 
            this.txtWriteWord.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtWriteWord.MaxLength = 4;
            this.txtWriteWord.Name = "txtWriteWord";
            this.txtWriteWord.Size = new System.Drawing.Size(50, 23);
            this.txtWriteWord.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWriteWord.MouseEnter += new System.EventHandler(this.txtWriteWord_MouseEnter);
            this.txtWriteWord.MouseLeave += new System.EventHandler(this.txtWriteWord_MouseLeave);
            this.txtWriteWord.TextChanged += new System.EventHandler(this.txtWriteWord_TextChanged);
            // 
            // txtReadWord
            // 
            this.txtReadWord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.txtReadWord.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtReadWord.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtReadWord.Margin = new System.Windows.Forms.Padding(1, 0, 3, 0);
            this.txtReadWord.MaxLength = 4;
            this.txtReadWord.Name = "txtReadWord";
            this.txtReadWord.ReadOnly = true;
            this.txtReadWord.Size = new System.Drawing.Size(50, 23);
            this.txtReadWord.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtReadWord.MouseEnter += new System.EventHandler(this.txtReadWord_MouseEnter);
            this.txtReadWord.MouseLeave += new System.EventHandler(this.txtReadWord_MouseLeave);
            // 
            // labReadWord
            // 
            this.labReadWord.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.labReadWord.Name = "labReadWord";
            this.labReadWord.Size = new System.Drawing.Size(62, 20);
            this.labReadWord.Text = "ReadWord";
            this.labReadWord.Click += new System.EventHandler(this.labReadWord_Click);
            // 
            // toolbarCMDC
            // 
            this.toolbarCMDC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolbarCMDC.AutoSize = false;
            this.toolbarCMDC.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbarCMDC.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarCMDC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labCommandType,
            this.comboCMDType,
            this.labTelegraph,
            this.comboTelegraph});
            this.toolbarCMDC.Location = new System.Drawing.Point(10, 352);
            this.toolbarCMDC.Name = "toolbarCMDC";
            this.toolbarCMDC.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCMDC.Size = new System.Drawing.Size(500, 22);
            this.toolbarCMDC.Stretch = true;
            this.toolbarCMDC.TabIndex = 3;
            this.toolbarCMDC.Text = "toolStrip1";
            // 
            // labCommandType
            // 
            this.labCommandType.Name = "labCommandType";
            this.labCommandType.Size = new System.Drawing.Size(102, 19);
            this.labCommandType.Text = "    CommandType";
            // 
            // comboCMDType
            // 
            this.comboCMDType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCMDType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.comboCMDType.Items.AddRange(new object[] {
            "Just Command",
            "Write Word",
            "Read Word",
            "Write Block",
            "Read Block"});
            this.comboCMDType.Name = "comboCMDType";
            this.comboCMDType.Size = new System.Drawing.Size(121, 22);
            this.comboCMDType.SelectedIndexChanged += new System.EventHandler(this.comboCMDType_SelectedIndexChanged);
            this.comboCMDType.MouseEnter += new System.EventHandler(this.comboCMDType_MouseEnter);
            this.comboCMDType.MouseLeave += new System.EventHandler(this.comboCMDType_MouseLeave);
            // 
            // labTelegraph
            // 
            this.labTelegraph.Name = "labTelegraph";
            this.labTelegraph.Size = new System.Drawing.Size(98, 19);
            this.labTelegraph.Text = "    TelegraphType";
            // 
            // comboTelegraph
            // 
            this.comboTelegraph.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTelegraph.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.comboTelegraph.Name = "comboTelegraph";
            this.comboTelegraph.Size = new System.Drawing.Size(121, 22);
            this.comboTelegraph.MouseEnter += new System.EventHandler(this.comboTelegraph_MouseEnter);
            this.comboTelegraph.MouseLeave += new System.EventHandler(this.comboTelegraph_MouseLeave);
            // 
            // txtHelp
            // 
            this.txtHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHelp.BackColor = System.Drawing.SystemColors.Control;
            this.txtHelp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHelp.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtHelp.Location = new System.Drawing.Point(10, 374);
            this.txtHelp.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.txtHelp.Multiline = true;
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.ReadOnly = true;
            this.txtHelp.Size = new System.Drawing.Size(590, 87);
            this.txtHelp.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labBrief);
            this.panel1.Controls.Add(this.txtBrief);
            this.panel1.Location = new System.Drawing.Point(10, 44);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(590, 21);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "Brief";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labBrief
            // 
            this.labBrief.AutoSize = true;
            this.labBrief.BackColor = System.Drawing.SystemColors.Control;
            this.labBrief.Location = new System.Drawing.Point(-85, 21);
            this.labBrief.Name = "labBrief";
            this.labBrief.Size = new System.Drawing.Size(35, 12);
            this.labBrief.TabIndex = 2;
            this.labBrief.Text = "Brief";
            this.labBrief.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtBrief
            // 
            this.txtBrief.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrief.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtBrief.Location = new System.Drawing.Point(61, 1);
            this.txtBrief.Name = "txtBrief";
            this.txtBrief.Size = new System.Drawing.Size(525, 21);
            this.txtBrief.TabIndex = 3;
            this.txtBrief.MouseEnter += new System.EventHandler(this.txtBrief_MouseEnter);
            this.txtBrief.MouseLeave += new System.EventHandler(this.txtBrief_MouseLeave);
            // 
            // toolbarCMDA
            // 
            this.toolbarCMDA.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarCMDA.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labCommand,
            this.txtCommand,
            this.labAddress,
            this.comboAddress,
            this.labSubAddress,
            this.txtSubAddress,
            this.labTimeoutSetting,
            this.comboTimeout});
            this.toolbarCMDA.Location = new System.Drawing.Point(10, 23);
            this.toolbarCMDA.Name = "toolbarCMDA";
            this.toolbarCMDA.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCMDA.Size = new System.Drawing.Size(590, 25);
            this.toolbarCMDA.TabIndex = 0;
            this.toolbarCMDA.Text = "toolStrip1";
            // 
            // labCommand
            // 
            this.labCommand.Name = "labCommand";
            this.labCommand.Size = new System.Drawing.Size(70, 22);
            this.labCommand.Text = "  Command";
            this.labCommand.Click += new System.EventHandler(this.labCommand_Click);
            // 
            // txtCommand
            // 
            this.txtCommand.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtCommand.MaxLength = 2;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(40, 25);
            this.txtCommand.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCommand.MouseEnter += new System.EventHandler(this.txtCommand_MouseEnter);
            this.txtCommand.MouseLeave += new System.EventHandler(this.txtCommand_MouseLeave);
            this.txtCommand.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
            // 
            // labAddress
            // 
            this.labAddress.Name = "labAddress";
            this.labAddress.Size = new System.Drawing.Size(62, 22);
            this.labAddress.Text = "    Send To";
            // 
            // comboAddress
            // 
            this.comboAddress.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.comboAddress.MaxLength = 2;
            this.comboAddress.Name = "comboAddress";
            this.comboAddress.Size = new System.Drawing.Size(121, 25);
            this.comboAddress.TextUpdate += new System.EventHandler(this.comboAddress_TextUpdate);
            this.comboAddress.MouseEnter += new System.EventHandler(this.comboAddress_MouseEnter);
            this.comboAddress.MouseLeave += new System.EventHandler(this.comboAddress_MouseLeave);
            // 
            // labSubAddress
            // 
            this.labSubAddress.Name = "labSubAddress";
            this.labSubAddress.Size = new System.Drawing.Size(62, 22);
            this.labSubAddress.Text = "  Sub.Addr";
            // 
            // txtSubAddress
            // 
            this.txtSubAddress.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtSubAddress.MaxLength = 2;
            this.txtSubAddress.Name = "txtSubAddress";
            this.txtSubAddress.Size = new System.Drawing.Size(40, 25);
            this.txtSubAddress.Text = "00";
            this.txtSubAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSubAddress.MouseEnter += new System.EventHandler(this.txtSubAddress_MouseEnter);
            this.txtSubAddress.MouseLeave += new System.EventHandler(this.txtSubAddress_MouseLeave);
            this.txtSubAddress.TextChanged += new System.EventHandler(this.txtSubAddress_TextChanged);
            // 
            // labTimeoutSetting
            // 
            this.labTimeoutSetting.Name = "labTimeoutSetting";
            this.labTimeoutSetting.Size = new System.Drawing.Size(61, 22);
            this.labTimeoutSetting.Text = "    Wait for";
            // 
            // comboTimeout
            // 
            this.comboTimeout.ForeColor = System.Drawing.Color.Red;
            this.comboTimeout.Name = "comboTimeout";
            this.comboTimeout.Size = new System.Drawing.Size(100, 25);
            this.comboTimeout.TextUpdate += new System.EventHandler(this.comboTimeout_TextUpdate);
            this.comboTimeout.MouseEnter += new System.EventHandler(this.comboTimeout_MouseEnter);
            this.comboTimeout.MouseLeave += new System.EventHandler(this.comboTimeout_MouseLeave);
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.grpDebug);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebug.Size = new System.Drawing.Size(630, 493);
            this.tabDebug.TabIndex = 2;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // grpDebug
            // 
            this.grpDebug.Controls.Add(this.lvDebugMessage);
            this.grpDebug.Controls.Add(this.toolbarDebug);
            this.grpDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDebug.Location = new System.Drawing.Point(3, 3);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Padding = new System.Windows.Forms.Padding(5);
            this.grpDebug.Size = new System.Drawing.Size(624, 487);
            this.grpDebug.TabIndex = 1;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Communication Monitor";
            // 
            // lvDebugMessage
            // 
            this.lvDebugMessage.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.lvDebugMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvDebugMessage.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chMSGDirection,
            this.chLength,
            this.chDisplayData,
            this.chDescription,
            this.chBrief});
            this.lvDebugMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDebugMessage.Enabled = false;
            this.lvDebugMessage.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDebugMessage.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lvDebugMessage.FullRowSelect = true;
            this.lvDebugMessage.Location = new System.Drawing.Point(5, 44);
            this.lvDebugMessage.Name = "lvDebugMessage";
            this.lvDebugMessage.Size = new System.Drawing.Size(614, 438);
            this.lvDebugMessage.TabIndex = 3;
            this.lvDebugMessage.UseCompatibleStateImageBehavior = false;
            this.lvDebugMessage.View = System.Windows.Forms.View.Details;
            this.lvDebugMessage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvDebugMessage_MouseDoubleClick);
            // 
            // chMSGDirection
            // 
            this.chMSGDirection.Text = "I/O";
            this.chMSGDirection.Width = 40;
            // 
            // chLength
            // 
            this.chLength.Text = "LEN";
            this.chLength.Width = 39;
            // 
            // chDisplayData
            // 
            this.chDisplayData.Text = "Data";
            this.chDisplayData.Width = 289;
            // 
            // chDescription
            // 
            this.chDescription.Text = "Discription";
            this.chDescription.Width = 126;
            // 
            // chBrief
            // 
            this.chBrief.Text = "Brief";
            this.chBrief.Width = 134;
            // 
            // toolbarDebug
            // 
            this.toolbarDebug.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarDebug.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.cmdClear,
            this.toolStripSeparator2,
            this.cmdEnableDebug,
            this.toolStripSeparator3});
            this.toolbarDebug.Location = new System.Drawing.Point(5, 19);
            this.toolbarDebug.Name = "toolbarDebug";
            this.toolbarDebug.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarDebug.Size = new System.Drawing.Size(614, 25);
            this.toolbarDebug.TabIndex = 2;
            this.toolbarDebug.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdClear
            // 
            this.cmdClear.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdClear.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.cmdClear.Image = ((System.Drawing.Image)(resources.GetObject("cmdClear.Image")));
            this.cmdClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(59, 22);
            this.cmdClear.Text = "    Clear   ";
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdEnableDebug
            // 
            this.cmdEnableDebug.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdEnableDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdEnableDebug.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.cmdEnableDebug.Image = ((System.Drawing.Image)(resources.GetObject("cmdEnableDebug.Image")));
            this.cmdEnableDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdEnableDebug.Name = "cmdEnableDebug";
            this.cmdEnableDebug.Size = new System.Drawing.Size(67, 22);
            this.cmdEnableDebug.Text = "    Debug   ";
            this.cmdEnableDebug.Click += new System.EventHandler(this.cmdEnableDebug_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tabInformation
            // 
            this.tabInformation.Controls.Add(this.panelProperties);
            this.tabInformation.Location = new System.Drawing.Point(4, 22);
            this.tabInformation.Name = "tabInformation";
            this.tabInformation.Padding = new System.Windows.Forms.Padding(3);
            this.tabInformation.Size = new System.Drawing.Size(630, 493);
            this.tabInformation.TabIndex = 3;
            this.tabInformation.Text = "Properties";
            this.tabInformation.UseVisualStyleBackColor = true;
            // 
            // panelProperties
            // 
            this.panelProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProperties.Location = new System.Drawing.Point(3, 3);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Padding = new System.Windows.Forms.Padding(5);
            this.panelProperties.Size = new System.Drawing.Size(624, 487);
            this.panelProperties.TabIndex = 2;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 1500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 1000;
            this.toolTip.ShowAlways = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.ToolTipTitle = "Command";
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
            // timerRefreshDebugMessage
            // 
            this.timerRefreshDebugMessage.Tick += new System.EventHandler(this.timerRefreshDebugMessage_Tick);
            // 
            // frmAdapterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 551);
            this.Controls.Add(this.panelEditor);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAdapterEditor";
            this.ShowInTaskbar = false;
            this.Text = "Adapter Editor";
            this.panelEditor.ResumeLayout(false);
            this.tabsAdapterEditor.ResumeLayout(false);
            this.tabDeviceManager.ResumeLayout(false);
            this.tabCommunication.ResumeLayout(false);
            this.panelCommunication.ResumeLayout(false);
            this.grbCommand.ResumeLayout(false);
            this.grbCommand.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.toolbarCMDB.ResumeLayout(false);
            this.toolbarCMDB.PerformLayout();
            this.toolbarCMDC.ResumeLayout(false);
            this.toolbarCMDC.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolbarCMDA.ResumeLayout(false);
            this.toolbarCMDA.PerformLayout();
            this.tabDebug.ResumeLayout(false);
            this.grpDebug.ResumeLayout(false);
            this.grpDebug.PerformLayout();
            this.toolbarDebug.ResumeLayout(false);
            this.toolbarDebug.PerformLayout();
            this.tabInformation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelEditor;
        protected System.Windows.Forms.TabPage tabDeviceManager;
        private System.Windows.Forms.TabPage tabCommunication;
        protected System.Windows.Forms.TabPage tabDebug;
        protected System.Windows.Forms.TabPage tabInformation;
        protected System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ListView lvDebugMessage;
        private System.Windows.Forms.ColumnHeader chMSGDirection;
        private System.Windows.Forms.ColumnHeader chLength;
        private System.Windows.Forms.ColumnHeader chDisplayData;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.ColumnHeader chBrief;
        private System.Windows.Forms.ToolStrip toolbarDebug;
        private System.Windows.Forms.ToolStripButton cmdEnableDebug;
        private System.Windows.Forms.ToolStripButton cmdClear;
        private System.Windows.Forms.TabControl tabsAdapterEditor;
        private System.Windows.Forms.GroupBox grpDebug;
        protected System.Windows.Forms.Panel panelDeviceManager;
        protected System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.GroupBox grbCommand;
        private System.Windows.Forms.Panel panelCommunication;
        private System.Windows.Forms.ToolStrip toolbarCMDA;
        private System.Windows.Forms.ToolStripLabel labCommand;
        private System.Windows.Forms.ToolStripTextBox txtCommand;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStripLabel labAddress;
        private System.Windows.Forms.ToolStripComboBox comboAddress;
        private System.Windows.Forms.ToolStripLabel labTimeoutSetting;
        private System.Windows.Forms.ToolStripComboBox comboTimeout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labBrief;
        private System.Windows.Forms.TextBox txtBrief;
        private System.Windows.Forms.TextBox txtHelp;
        private System.Windows.Forms.ToolStrip toolbarCMDC;
        private System.Windows.Forms.ToolStripLabel labCommandType;
        private System.Windows.Forms.ToolStripComboBox comboCMDType;
        private System.Windows.Forms.ToolStripLabel labTelegraph;
        private System.Windows.Forms.ToolStrip toolbarCMDB;
        private System.Windows.Forms.ToolStripLabel labWriteWord;
        private System.Windows.Forms.ToolStripTextBox txtWriteWord;
        private System.Windows.Forms.ToolStripTextBox txtReadWord;
        private System.Windows.Forms.ToolStripLabel labReadWord;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkWriteHex;
        private System.Windows.Forms.Label labWriteBlock;
        private System.Windows.Forms.TextBox txtWriteBlock;
        private System.Windows.Forms.TextBox txtReadBlock;
        private System.Windows.Forms.Label labReadBlock;
        private System.Windows.Forms.CheckBox chkHexDisplay;
        protected System.Windows.Forms.ToolStripComboBox comboTelegraph;
        private System.Windows.Forms.Button cmdDoIt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolTip toolTipError;
        private System.Windows.Forms.ToolStripLabel labSubAddress;
        private System.Windows.Forms.ToolStripTextBox txtSubAddress;
        private System.Windows.Forms.Timer timerRefreshDebugMessage;

    }
}