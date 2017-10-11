namespace ESnail.CommunicationSet.Commands
{
    partial class frmCommandEditor
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
            if (null != CommandEditEvent)
            {
                //! raising event
                CommandEditEvent(BM_CMD_EDIT_RESULT.BM_CMD_EDIT_CANCELLED, m_Command);
            }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommandEditor));
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.tabsCommandEditor = new System.Windows.Forms.TabControl();
            this.tabCommandEditor = new System.Windows.Forms.TabPage();
            this.panelCommandEditor = new System.Windows.Forms.Panel();
            this.grbCMDEditor = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolbarCommand = new System.Windows.Forms.ToolStrip();
            this.labCommand = new System.Windows.Forms.ToolStripLabel();
            this.txtCommand = new System.Windows.Forms.ToolStripTextBox();
            this.labAddress = new System.Windows.Forms.ToolStripLabel();
            this.combAddress = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtSubAddress = new System.Windows.Forms.ToolStripTextBox();
            this.labResponseType = new System.Windows.Forms.ToolStripLabel();
            this.combResponseType = new System.Windows.Forms.ToolStripComboBox();
            this.textBrief = new System.Windows.Forms.TextBox();
            this.labBrief = new System.Windows.Forms.Label();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.toolbarWriteWord = new System.Windows.Forms.ToolStrip();
            this.labWriteWord = new System.Windows.Forms.ToolStripLabel();
            this.txtWriteWord = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.checkWriteBlockShowHEX = new System.Windows.Forms.CheckBox();
            this.labWriteBlock = new System.Windows.Forms.Label();
            this.txtWriteBlock = new System.Windows.Forms.TextBox();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.toolbarCommandType = new System.Windows.Forms.ToolStrip();
            this.labCommandType = new System.Windows.Forms.ToolStripLabel();
            this.combCommandType = new System.Windows.Forms.ToolStripComboBox();
            this.labFrameType = new System.Windows.Forms.ToolStripLabel();
            this.txtCommandID = new System.Windows.Forms.ToolStripTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.panelEditor.SuspendLayout();
            this.tabsCommandEditor.SuspendLayout();
            this.tabCommandEditor.SuspendLayout();
            this.panelCommandEditor.SuspendLayout();
            this.grbCMDEditor.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.toolbarCommand.SuspendLayout();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.toolbarWriteWord.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.toolbarCommandType.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 541);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(598, 22);
            this.StatusBar.TabIndex = 0;
            this.StatusBar.Text = "statusStrip1";
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.tabsCommandEditor);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Padding = new System.Windows.Forms.Padding(5);
            this.panelEditor.Size = new System.Drawing.Size(598, 541);
            this.panelEditor.TabIndex = 1;
            // 
            // tabsCommandEditor
            // 
            this.tabsCommandEditor.Controls.Add(this.tabCommandEditor);
            this.tabsCommandEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsCommandEditor.Location = new System.Drawing.Point(5, 5);
            this.tabsCommandEditor.Name = "tabsCommandEditor";
            this.tabsCommandEditor.SelectedIndex = 0;
            this.tabsCommandEditor.Size = new System.Drawing.Size(588, 531);
            this.tabsCommandEditor.TabIndex = 2;
            // 
            // tabCommandEditor
            // 
            this.tabCommandEditor.Controls.Add(this.panelCommandEditor);
            this.tabCommandEditor.Location = new System.Drawing.Point(4, 22);
            this.tabCommandEditor.Name = "tabCommandEditor";
            this.tabCommandEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommandEditor.Size = new System.Drawing.Size(580, 505);
            this.tabCommandEditor.TabIndex = 0;
            this.tabCommandEditor.Text = "Command";
            this.tabCommandEditor.UseVisualStyleBackColor = true;
            // 
            // panelCommandEditor
            // 
            this.panelCommandEditor.Controls.Add(this.grbCMDEditor);
            this.panelCommandEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCommandEditor.Location = new System.Drawing.Point(3, 3);
            this.panelCommandEditor.Name = "panelCommandEditor";
            this.panelCommandEditor.Padding = new System.Windows.Forms.Padding(3);
            this.panelCommandEditor.Size = new System.Drawing.Size(574, 499);
            this.panelCommandEditor.TabIndex = 0;
            // 
            // grbCMDEditor
            // 
            this.grbCMDEditor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grbCMDEditor.Controls.Add(this.splitContainer2);
            this.grbCMDEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbCMDEditor.Location = new System.Drawing.Point(3, 3);
            this.grbCMDEditor.Name = "grbCMDEditor";
            this.grbCMDEditor.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.grbCMDEditor.Size = new System.Drawing.Size(568, 493);
            this.grbCMDEditor.TabIndex = 4;
            this.grbCMDEditor.TabStop = false;
            this.grbCMDEditor.Text = "Command Editor";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(10, 23);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer5);
            this.splitContainer2.Size = new System.Drawing.Size(548, 461);
            this.splitContainer2.SplitterDistance = 56;
            this.splitContainer2.SplitterWidth = 9;
            this.splitContainer2.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.toolbarCommand);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.textBrief);
            this.splitContainer3.Panel2.Controls.Add(this.labBrief);
            this.splitContainer3.Size = new System.Drawing.Size(548, 56);
            this.splitContainer3.SplitterDistance = 25;
            this.splitContainer3.TabIndex = 0;
            // 
            // toolbarCommand
            // 
            this.toolbarCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarCommand.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarCommand.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labCommand,
            this.txtCommand,
            this.labAddress,
            this.combAddress,
            this.toolStripLabel1,
            this.txtSubAddress,
            this.labResponseType,
            this.combResponseType});
            this.toolbarCommand.Location = new System.Drawing.Point(0, 0);
            this.toolbarCommand.Name = "toolbarCommand";
            this.toolbarCommand.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCommand.Size = new System.Drawing.Size(548, 25);
            this.toolbarCommand.TabIndex = 2;
            this.toolbarCommand.Text = "toolStrip2";
            // 
            // labCommand
            // 
            this.labCommand.Name = "labCommand";
            this.labCommand.Size = new System.Drawing.Size(70, 22);
            this.labCommand.Text = "  Command";
            // 
            // txtCommand
            // 
            this.txtCommand.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCommand.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtCommand.MaxLength = 2;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(40, 25);
            this.txtCommand.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCommand.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
            // 
            // labAddress
            // 
            this.labAddress.Name = "labAddress";
            this.labAddress.Size = new System.Drawing.Size(59, 22);
            this.labAddress.Text = "   Send To";
            // 
            // combAddress
            // 
            this.combAddress.AutoSize = false;
            this.combAddress.AutoToolTip = true;
            this.combAddress.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combAddress.Items.AddRange(new object[] {
            "Adapter",
            "SMBus",
            "SMBus with PEC",
            "UART",
            "UART with PEC",
            "Single-wire UART",
            "Single-wire UART with PEC",
            "SPI",
            "SPI with PEC",
            "I2C",
            "I2C with PEC",
            "Loader",
            "Charger",
            "Printer",
            "LCD",
            "Extend SMBus",
            "Extend SMBus with PEC",
            "Extend UART",
            "Extend UART with PEC",
            "Extend Single-wire UART",
            "Extend Single-wire UART with PEC",
            "Extend SPI",
            "Extend SPI with PEC",
            "Extend I2C",
            "Extend I2C with PEC",
            "All"});
            this.combAddress.MaxLength = 2;
            this.combAddress.Name = "combAddress";
            this.combAddress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.combAddress.Size = new System.Drawing.Size(100, 23);
            this.combAddress.TextUpdate += new System.EventHandler(this.combAddress_TextUpdate);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(62, 22);
            this.toolStripLabel1.Text = "  Sub.Addr";
            // 
            // txtSubAddress
            // 
            this.txtSubAddress.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtSubAddress.Name = "txtSubAddress";
            this.txtSubAddress.Size = new System.Drawing.Size(40, 25);
            this.txtSubAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSubAddress.TextChanged += new System.EventHandler(this.txtSubAddress_TextChanged);
            // 
            // labResponseType
            // 
            this.labResponseType.Name = "labResponseType";
            this.labResponseType.Size = new System.Drawing.Size(61, 22);
            this.labResponseType.Text = "    Wait for";
            // 
            // combResponseType
            // 
            this.combResponseType.AutoSize = false;
            this.combResponseType.AutoToolTip = true;
            this.combResponseType.ForeColor = System.Drawing.Color.Red;
            this.combResponseType.Items.AddRange(new object[] {
            "No Response",
            "Wait forever"});
            this.combResponseType.MaxLength = 5;
            this.combResponseType.Name = "combResponseType";
            this.combResponseType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.combResponseType.Size = new System.Drawing.Size(100, 21);
            this.combResponseType.TextChanged += new System.EventHandler(this.combResponseType_TextChanged);
            // 
            // textBrief
            // 
            this.textBrief.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBrief.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.textBrief.Location = new System.Drawing.Point(43, 2);
            this.textBrief.Name = "textBrief";
            this.textBrief.Size = new System.Drawing.Size(502, 21);
            this.textBrief.TabIndex = 1;
            // 
            // labBrief
            // 
            this.labBrief.Location = new System.Drawing.Point(3, 4);
            this.labBrief.Name = "labBrief";
            this.labBrief.Size = new System.Drawing.Size(37, 13);
            this.labBrief.TabIndex = 0;
            this.labBrief.Text = "  Brief";
            this.labBrief.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            this.splitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer6);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.splitContainer5.Panel2.Controls.Add(this.splitContainer9);
            this.splitContainer5.Size = new System.Drawing.Size(548, 396);
            this.splitContainer5.SplitterDistance = 237;
            this.splitContainer5.SplitterWidth = 9;
            this.splitContainer5.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer6.IsSplitterFixed = true;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.splitContainer8);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer6.Size = new System.Drawing.Size(548, 237);
            this.splitContainer6.SplitterDistance = 25;
            this.splitContainer6.TabIndex = 0;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.toolbarWriteWord);
            this.splitContainer8.Size = new System.Drawing.Size(548, 25);
            this.splitContainer8.SplitterDistance = 262;
            this.splitContainer8.TabIndex = 0;
            // 
            // toolbarWriteWord
            // 
            this.toolbarWriteWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarWriteWord.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarWriteWord.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labWriteWord,
            this.txtWriteWord});
            this.toolbarWriteWord.Location = new System.Drawing.Point(0, 0);
            this.toolbarWriteWord.Name = "toolbarWriteWord";
            this.toolbarWriteWord.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarWriteWord.Size = new System.Drawing.Size(262, 25);
            this.toolbarWriteWord.TabIndex = 2;
            this.toolbarWriteWord.Text = "toolbarWriteWord";
            // 
            // labWriteWord
            // 
            this.labWriteWord.Name = "labWriteWord";
            this.labWriteWord.Size = new System.Drawing.Size(73, 22);
            this.labWriteWord.Text = "  Write Word";
            // 
            // txtWriteWord
            // 
            this.txtWriteWord.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtWriteWord.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtWriteWord.MaxLength = 4;
            this.txtWriteWord.Name = "txtWriteWord";
            this.txtWriteWord.Size = new System.Drawing.Size(50, 25);
            this.txtWriteWord.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWriteWord.TextChanged += new System.EventHandler(this.txtWriteWord_TextChanged);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.checkWriteBlockShowHEX);
            this.splitContainer4.Panel1.Controls.Add(this.labWriteBlock);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.txtWriteBlock);
            this.splitContainer4.Size = new System.Drawing.Size(548, 208);
            this.splitContainer4.SplitterDistance = 25;
            this.splitContainer4.TabIndex = 0;
            // 
            // checkWriteBlockShowHEX
            // 
            this.checkWriteBlockShowHEX.AutoSize = true;
            this.checkWriteBlockShowHEX.Checked = true;
            this.checkWriteBlockShowHEX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkWriteBlockShowHEX.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkWriteBlockShowHEX.Location = new System.Drawing.Point(506, 0);
            this.checkWriteBlockShowHEX.Name = "checkWriteBlockShowHEX";
            this.checkWriteBlockShowHEX.Size = new System.Drawing.Size(42, 25);
            this.checkWriteBlockShowHEX.TabIndex = 11;
            this.checkWriteBlockShowHEX.Text = "HEX";
            this.checkWriteBlockShowHEX.UseVisualStyleBackColor = true;
            this.checkWriteBlockShowHEX.CheckedChanged += new System.EventHandler(this.checkWriteBlockShowHEX_CheckedChanged);
            // 
            // labWriteBlock
            // 
            this.labWriteBlock.Dock = System.Windows.Forms.DockStyle.Left;
            this.labWriteBlock.Location = new System.Drawing.Point(0, 0);
            this.labWriteBlock.Name = "labWriteBlock";
            this.labWriteBlock.Size = new System.Drawing.Size(123, 25);
            this.labWriteBlock.TabIndex = 10;
            this.labWriteBlock.Text = "  Write Block";
            this.labWriteBlock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtWriteBlock
            // 
            this.txtWriteBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWriteBlock.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtWriteBlock.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.txtWriteBlock.Location = new System.Drawing.Point(0, 0);
            this.txtWriteBlock.Multiline = true;
            this.txtWriteBlock.Name = "txtWriteBlock";
            this.txtWriteBlock.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWriteBlock.Size = new System.Drawing.Size(548, 179);
            this.txtWriteBlock.TabIndex = 8;
            this.txtWriteBlock.TextChanged += new System.EventHandler(this.txtWriteBlock_TextChanged);
            // 
            // splitContainer9
            // 
            this.splitContainer9.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer9.IsSplitterFixed = true;
            this.splitContainer9.Location = new System.Drawing.Point(0, 0);
            this.splitContainer9.Name = "splitContainer9";
            this.splitContainer9.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.toolbarCommandType);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer9.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer9.Size = new System.Drawing.Size(548, 150);
            this.splitContainer9.SplitterDistance = 28;
            this.splitContainer9.TabIndex = 0;
            // 
            // toolbarCommandType
            // 
            this.toolbarCommandType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarCommandType.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarCommandType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labCommandType,
            this.combCommandType,
            this.labFrameType,
            this.txtCommandID});
            this.toolbarCommandType.Location = new System.Drawing.Point(0, 0);
            this.toolbarCommandType.Name = "toolbarCommandType";
            this.toolbarCommandType.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCommandType.Size = new System.Drawing.Size(548, 28);
            this.toolbarCommandType.TabIndex = 3;
            this.toolbarCommandType.Text = "toolStrip6";
            // 
            // labCommandType
            // 
            this.labCommandType.Name = "labCommandType";
            this.labCommandType.Size = new System.Drawing.Size(75, 25);
            this.labCommandType.Text = "  CMD Type  ";
            // 
            // combCommandType
            // 
            this.combCommandType.AutoSize = false;
            this.combCommandType.AutoToolTip = true;
            this.combCommandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combCommandType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combCommandType.Items.AddRange(new object[] {
            "Just Command",
            "Write Word",
            "Read Word",
            "Write Block",
            "Read Block"});
            this.combCommandType.Name = "combCommandType";
            this.combCommandType.Size = new System.Drawing.Size(100, 23);
            this.combCommandType.SelectedIndexChanged += new System.EventHandler(this.combCommandType_SelectedIndexChanged);
            // 
            // labFrameType
            // 
            this.labFrameType.Name = "labFrameType";
            this.labFrameType.Size = new System.Drawing.Size(30, 25);
            this.labFrameType.Text = "    ID";
            // 
            // txtCommandID
            // 
            this.txtCommandID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCommandID.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtCommandID.MaxLength = 255;
            this.txtCommandID.Name = "txtCommandID";
            this.txtCommandID.Size = new System.Drawing.Size(200, 28);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtDescription);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer1.Size = new System.Drawing.Size(548, 118);
            this.splitContainer1.SplitterDistance = 68;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDescription.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtDescription.Location = new System.Drawing.Point(0, 0);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(548, 68);
            this.txtDescription.TabIndex = 2;
            this.txtDescription.Text = "Put help information here...";
            this.txtDescription.Visible = false;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer7.IsSplitterFixed = true;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.cmdCancel);
            this.splitContainer7.Panel2.Controls.Add(this.cmdOK);
            this.splitContainer7.Size = new System.Drawing.Size(548, 46);
            this.splitContainer7.SplitterDistance = 367;
            this.splitContainer7.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(100, 18);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(77, 21);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(17, 18);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(77, 21);
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // frmCommandEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 563);
            this.Controls.Add(this.panelEditor);
            this.Controls.Add(this.StatusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCommandEditor";
            this.ShowInTaskbar = false;
            this.Text = "  Command Editor";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCommandEditor_FormClosing);
            this.panelEditor.ResumeLayout(false);
            this.tabsCommandEditor.ResumeLayout(false);
            this.tabCommandEditor.ResumeLayout(false);
            this.panelCommandEditor.ResumeLayout(false);
            this.grbCMDEditor.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            this.splitContainer3.ResumeLayout(false);
            this.toolbarCommand.ResumeLayout(false);
            this.toolbarCommand.PerformLayout();
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel2.ResumeLayout(false);
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel1.PerformLayout();
            this.splitContainer8.ResumeLayout(false);
            this.toolbarWriteWord.ResumeLayout(false);
            this.toolbarWriteWord.PerformLayout();
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel1.PerformLayout();
            this.splitContainer9.Panel2.ResumeLayout(false);
            this.splitContainer9.ResumeLayout(false);
            this.toolbarCommandType.ResumeLayout(false);
            this.toolbarCommandType.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.Panel panelEditor;
        private System.Windows.Forms.GroupBox grbCMDEditor;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStrip toolbarCommand;
        private System.Windows.Forms.ToolStripLabel labCommand;
        private System.Windows.Forms.ToolStripTextBox txtCommand;
        private System.Windows.Forms.ToolStripLabel labAddress;
        private System.Windows.Forms.ToolStripComboBox combAddress;
        private System.Windows.Forms.ToolStripLabel labResponseType;
        private System.Windows.Forms.ToolStripComboBox combResponseType;
        private System.Windows.Forms.TextBox textBrief;
        private System.Windows.Forms.Label labBrief;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.ToolStrip toolbarWriteWord;
        private System.Windows.Forms.ToolStripLabel labWriteWord;
        private System.Windows.Forms.ToolStripTextBox txtWriteWord;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.CheckBox checkWriteBlockShowHEX;
        private System.Windows.Forms.Label labWriteBlock;
        private System.Windows.Forms.TextBox txtWriteBlock;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.ToolStrip toolbarCommandType;
        private System.Windows.Forms.ToolStripLabel labCommandType;
        private System.Windows.Forms.ToolStripComboBox combCommandType;
        private System.Windows.Forms.ToolStripLabel labFrameType;
        private System.Windows.Forms.ToolStripTextBox txtCommandID;
        private System.Windows.Forms.TabControl tabsCommandEditor;
        internal System.Windows.Forms.TabPage tabCommandEditor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        public System.Windows.Forms.Panel panelCommandEditor;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtSubAddress;
    }
}