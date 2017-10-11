namespace ESnail.Device.Adapters.USB.HID
{
    internal partial class frmTelegraphHIDAdapterEditor
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
            

            if (disposing)
            {
                _Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTelegraphHIDAdapterEditor));
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.panelTest = new System.Windows.Forms.Panel();
            this.tabsAdatper = new System.Windows.Forms.TabControl();
            this.tabDevices = new System.Windows.Forms.TabPage();
            this.panelDevices = new System.Windows.Forms.Panel();
            this.grpDeviceManager = new System.Windows.Forms.GroupBox();
            this.lvHIDDevice = new System.Windows.Forms.ListView();
            this.colhNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhSerialNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdRefreshDevice = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.txtPID = new System.Windows.Forms.ToolStripTextBox();
            this.labProductID = new System.Windows.Forms.ToolStripLabel();
            this.txtVID = new System.Windows.Forms.ToolStripTextBox();
            this.labVenderID = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tabCommunication = new System.Windows.Forms.TabPage();
            this.panelCommunication = new System.Windows.Forms.Panel();
            this.grbSMBus = new System.Windows.Forms.GroupBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.toolbarCommand = new System.Windows.Forms.ToolStrip();
            this.labCommand = new System.Windows.Forms.ToolStripLabel();
            this.txtCommand = new System.Windows.Forms.ToolStripTextBox();
            this.labAddress = new System.Windows.Forms.ToolStripLabel();
            this.combAddress = new System.Windows.Forms.ToolStripComboBox();
            this.labSubAddress = new System.Windows.Forms.ToolStripLabel();
            this.txtSubAddress = new System.Windows.Forms.ToolStripTextBox();
            this.labResponseType = new System.Windows.Forms.ToolStripLabel();
            this.combResponseType = new System.Windows.Forms.ToolStripComboBox();
            this.txtBrief = new System.Windows.Forms.TextBox();
            this.labBrief = new System.Windows.Forms.Label();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.toolbarWriteWord = new System.Windows.Forms.ToolStrip();
            this.labWriteWord = new System.Windows.Forms.ToolStripLabel();
            this.txtWriteWord = new System.Windows.Forms.ToolStripTextBox();
            this.toolbarReadWord = new System.Windows.Forms.ToolStrip();
            this.txtReadWord = new System.Windows.Forms.ToolStripTextBox();
            this.labReadWord = new System.Windows.Forms.ToolStripLabel();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.checkWriteBlockShowHEX = new System.Windows.Forms.CheckBox();
            this.labWriteBlock = new System.Windows.Forms.Label();
            this.txtWriteBlock = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.checkReadBlockShowHEX = new System.Windows.Forms.CheckBox();
            this.labReadBlock = new System.Windows.Forms.Label();
            this.txtReadBlock = new System.Windows.Forms.TextBox();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.splitContainer10 = new System.Windows.Forms.SplitContainer();
            this.toolbarCommandType = new System.Windows.Forms.ToolStrip();
            this.labCommandType = new System.Windows.Forms.ToolStripLabel();
            this.combCommandType = new System.Windows.Forms.ToolStripComboBox();
            this.labFrameType = new System.Windows.Forms.ToolStripLabel();
            this.combFrameType = new System.Windows.Forms.ToolStripComboBox();
            this.cmdDoIt = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.panelDebug = new System.Windows.Forms.Panel();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.lvDebugMessage = new System.Windows.Forms.ListView();
            this.chMSGDirection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLength = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDisplayData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBrief = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolbarDebug = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdEnableDebug = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tabAdatperInfo = new System.Windows.Forms.TabPage();
            this.panelInformation = new System.Windows.Forms.Panel();
            this.grpInformation = new System.Windows.Forms.GroupBox();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtConnectionState = new System.Windows.Forms.TextBox();
            this.txtSetting = new System.Windows.Forms.TextBox();
            this.txtDeviceInfo = new System.Windows.Forms.TextBox();
            this.txtDriverVersion = new System.Windows.Forms.TextBox();
            this.txtDeviceType = new System.Windows.Forms.TextBox();
            this.txtAdapterKey = new System.Windows.Forms.TextBox();
            this.txtAdapterType = new System.Windows.Forms.TextBox();
            this.labConnectionState = new System.Windows.Forms.Label();
            this.labSetting = new System.Windows.Forms.Label();
            this.labDeviceInfo = new System.Windows.Forms.Label();
            this.labDriverVersion = new System.Windows.Forms.Label();
            this.labDeviceType = new System.Windows.Forms.Label();
            this.labAdapterKey = new System.Windows.Forms.Label();
            this.labAdatperType = new System.Windows.Forms.Label();
            this.timerRefreshDebugMessage = new System.Windows.Forms.Timer(this.components);
            this.panelTest.SuspendLayout();
            this.tabsAdatper.SuspendLayout();
            this.tabDevices.SuspendLayout();
            this.panelDevices.SuspendLayout();
            this.grpDeviceManager.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabCommunication.SuspendLayout();
            this.panelCommunication.SuspendLayout();
            this.grbSMBus.SuspendLayout();
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
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            this.toolbarWriteWord.SuspendLayout();
            this.toolbarReadWord.SuspendLayout();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            this.splitContainer11.Panel1.SuspendLayout();
            this.splitContainer11.Panel2.SuspendLayout();
            this.splitContainer11.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            this.splitContainer10.Panel1.SuspendLayout();
            this.splitContainer10.Panel2.SuspendLayout();
            this.splitContainer10.SuspendLayout();
            this.toolbarCommandType.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.panelDebug.SuspendLayout();
            this.grpDebug.SuspendLayout();
            this.toolbarDebug.SuspendLayout();
            this.tabAdatperInfo.SuspendLayout();
            this.panelInformation.SuspendLayout();
            this.grpInformation.SuspendLayout();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusBar
            // 
            this.StatusBar.Location = new System.Drawing.Point(0, 549);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(641, 22);
            this.StatusBar.TabIndex = 0;
            // 
            // panelTest
            // 
            this.panelTest.Controls.Add(this.tabsAdatper);
            this.panelTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTest.Location = new System.Drawing.Point(0, 0);
            this.panelTest.Name = "panelTest";
            this.panelTest.Padding = new System.Windows.Forms.Padding(5);
            this.panelTest.Size = new System.Drawing.Size(641, 549);
            this.panelTest.TabIndex = 1;
            // 
            // tabsAdatper
            // 
            this.tabsAdatper.Controls.Add(this.tabDevices);
            this.tabsAdatper.Controls.Add(this.tabCommunication);
            this.tabsAdatper.Controls.Add(this.tabDebug);
            this.tabsAdatper.Controls.Add(this.tabAdatperInfo);
            this.tabsAdatper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabsAdatper.Location = new System.Drawing.Point(5, 5);
            this.tabsAdatper.Name = "tabsAdatper";
            this.tabsAdatper.SelectedIndex = 0;
            this.tabsAdatper.Size = new System.Drawing.Size(631, 539);
            this.tabsAdatper.TabIndex = 0;
            // 
            // tabDevices
            // 
            this.tabDevices.Controls.Add(this.panelDevices);
            this.tabDevices.Location = new System.Drawing.Point(4, 22);
            this.tabDevices.Name = "tabDevices";
            this.tabDevices.Size = new System.Drawing.Size(623, 513);
            this.tabDevices.TabIndex = 3;
            this.tabDevices.Text = "Device Manager";
            this.tabDevices.UseVisualStyleBackColor = true;
            // 
            // panelDevices
            // 
            this.panelDevices.Controls.Add(this.grpDeviceManager);
            this.panelDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDevices.Location = new System.Drawing.Point(0, 0);
            this.panelDevices.Name = "panelDevices";
            this.panelDevices.Size = new System.Drawing.Size(623, 513);
            this.panelDevices.TabIndex = 0;
            // 
            // grpDeviceManager
            // 
            this.grpDeviceManager.Controls.Add(this.lvHIDDevice);
            this.grpDeviceManager.Controls.Add(this.toolStrip1);
            this.grpDeviceManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDeviceManager.Location = new System.Drawing.Point(0, 0);
            this.grpDeviceManager.Name = "grpDeviceManager";
            this.grpDeviceManager.Padding = new System.Windows.Forms.Padding(10, 5, 10, 9);
            this.grpDeviceManager.Size = new System.Drawing.Size(623, 513);
            this.grpDeviceManager.TabIndex = 0;
            this.grpDeviceManager.TabStop = false;
            this.grpDeviceManager.Text = "Device Manager";
            // 
            // lvHIDDevice
            // 
            this.lvHIDDevice.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvHIDDevice.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhNumber,
            this.colhSerialNumber,
            this.colhState});
            this.lvHIDDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHIDDevice.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lvHIDDevice.FullRowSelect = true;
            this.lvHIDDevice.GridLines = true;
            this.lvHIDDevice.Location = new System.Drawing.Point(10, 44);
            this.lvHIDDevice.MultiSelect = false;
            this.lvHIDDevice.Name = "lvHIDDevice";
            this.lvHIDDevice.Size = new System.Drawing.Size(603, 460);
            this.lvHIDDevice.TabIndex = 7;
            this.lvHIDDevice.UseCompatibleStateImageBehavior = false;
            this.lvHIDDevice.View = System.Windows.Forms.View.Details;
            this.lvHIDDevice.DoubleClick += new System.EventHandler(this.lvHIDDevice_DoubleClick);
            this.lvHIDDevice.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvHIDDevice_KeyDown);
            this.lvHIDDevice.Resize += new System.EventHandler(this.lvHIDDevice_Resize);
            // 
            // colhNumber
            // 
            this.colhNumber.Text = "No.";
            this.colhNumber.Width = 35;
            // 
            // colhSerialNumber
            // 
            this.colhSerialNumber.Text = "Information";
            this.colhSerialNumber.Width = 373;
            // 
            // colhState
            // 
            this.colhState.Text = "Statue";
            this.colhState.Width = 76;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.cmdRefreshDevice,
            this.toolStripSeparator1,
            this.txtPID,
            this.labProductID,
            this.txtVID,
            this.labVenderID,
            this.toolStripSeparator6});
            this.toolStrip1.Location = new System.Drawing.Point(10, 19);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(603, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdRefreshDevice
            // 
            this.cmdRefreshDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdRefreshDevice.Image = ((System.Drawing.Image)(resources.GetObject("cmdRefreshDevice.Image")));
            this.cmdRefreshDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRefreshDevice.Name = "cmdRefreshDevice";
            this.cmdRefreshDevice.Size = new System.Drawing.Size(121, 22);
            this.cmdRefreshDevice.Text = "Refresh Device List";
            this.cmdRefreshDevice.Click += new System.EventHandler(this.cmdRefreshDevice_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // txtPID
            // 
            this.txtPID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPID.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size(50, 25);
            this.txtPID.Text = "2724";
            this.txtPID.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPID.TextChanged += new System.EventHandler(this.txtPID_TextChanged);
            // 
            // labProductID
            // 
            this.labProductID.Name = "labProductID";
            this.labProductID.Size = new System.Drawing.Size(28, 22);
            this.labProductID.Text = "PID";
            // 
            // txtVID
            // 
            this.txtVID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtVID.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtVID.Name = "txtVID";
            this.txtVID.Size = new System.Drawing.Size(50, 25);
            this.txtVID.Text = "C252";
            this.txtVID.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtVID.TextChanged += new System.EventHandler(this.txtVID_TextChanged);
            // 
            // labVenderID
            // 
            this.labVenderID.Name = "labVenderID";
            this.labVenderID.Size = new System.Drawing.Size(29, 22);
            this.labVenderID.Text = "VID";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator6.Visible = false;
            // 
            // tabCommunication
            // 
            this.tabCommunication.Controls.Add(this.panelCommunication);
            this.tabCommunication.Location = new System.Drawing.Point(4, 22);
            this.tabCommunication.Name = "tabCommunication";
            this.tabCommunication.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommunication.Size = new System.Drawing.Size(623, 513);
            this.tabCommunication.TabIndex = 0;
            this.tabCommunication.Text = "Communication";
            this.tabCommunication.UseVisualStyleBackColor = true;
            // 
            // panelCommunication
            // 
            this.panelCommunication.Controls.Add(this.grbSMBus);
            this.panelCommunication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCommunication.Location = new System.Drawing.Point(3, 3);
            this.panelCommunication.Name = "panelCommunication";
            this.panelCommunication.Padding = new System.Windows.Forms.Padding(3);
            this.panelCommunication.Size = new System.Drawing.Size(617, 507);
            this.panelCommunication.TabIndex = 0;
            // 
            // grbSMBus
            // 
            this.grbSMBus.Controls.Add(this.splitContainer2);
            this.grbSMBus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbSMBus.Location = new System.Drawing.Point(3, 3);
            this.grbSMBus.Name = "grbSMBus";
            this.grbSMBus.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.grbSMBus.Size = new System.Drawing.Size(611, 501);
            this.grbSMBus.TabIndex = 1;
            this.grbSMBus.TabStop = false;
            this.grbSMBus.Text = "Command";
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
            this.splitContainer2.Size = new System.Drawing.Size(591, 469);
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
            this.splitContainer3.Panel2.Controls.Add(this.txtBrief);
            this.splitContainer3.Panel2.Controls.Add(this.labBrief);
            this.splitContainer3.Size = new System.Drawing.Size(591, 56);
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
            this.labSubAddress,
            this.txtSubAddress,
            this.labResponseType,
            this.combResponseType});
            this.toolbarCommand.Location = new System.Drawing.Point(0, 0);
            this.toolbarCommand.Name = "toolbarCommand";
            this.toolbarCommand.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCommand.Size = new System.Drawing.Size(591, 25);
            this.toolbarCommand.TabIndex = 2;
            this.toolbarCommand.Text = "toolStrip2";
            // 
            // labCommand
            // 
            this.labCommand.Name = "labCommand";
            this.labCommand.Size = new System.Drawing.Size(76, 22);
            this.labCommand.Text = "  Command";
            // 
            // txtCommand
            // 
            this.txtCommand.AutoToolTip = true;
            this.txtCommand.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCommand.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtCommand.MaxLength = 2;
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(40, 25);
            this.txtCommand.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCommand.ToolTipText = "Enter command byte here.";
            this.txtCommand.TextChanged += new System.EventHandler(this.txtCommand_TextChanged);
            // 
            // labAddress
            // 
            this.labAddress.Name = "labAddress";
            this.labAddress.Size = new System.Drawing.Size(68, 22);
            this.labAddress.Text = "   Send To";
            // 
            // combAddress
            // 
            this.combAddress.AutoSize = false;
            this.combAddress.AutoToolTip = true;
            this.combAddress.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combAddress.MaxLength = 2;
            this.combAddress.Name = "combAddress";
            this.combAddress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.combAddress.Size = new System.Drawing.Size(130, 25);
            this.combAddress.ToolTipText = "Select a destination or enter a 7-bit address (in a byte) directly.";
            this.combAddress.TextUpdate += new System.EventHandler(this.combAddress_TextUpdate);
            // 
            // labSubAddress
            // 
            this.labSubAddress.Name = "labSubAddress";
            this.labSubAddress.Size = new System.Drawing.Size(70, 22);
            this.labSubAddress.Text = "  Sub.Addr";
            // 
            // txtSubAddress
            // 
            this.txtSubAddress.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtSubAddress.MaxLength = 2;
            this.txtSubAddress.Name = "txtSubAddress";
            this.txtSubAddress.Size = new System.Drawing.Size(40, 25);
            this.txtSubAddress.Text = "16";
            this.txtSubAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSubAddress.TextChanged += new System.EventHandler(this.txtSubAddress_TextChanged);
            // 
            // labResponseType
            // 
            this.labResponseType.Name = "labResponseType";
            this.labResponseType.Size = new System.Drawing.Size(71, 22);
            this.labResponseType.Text = "    Wait for";
            // 
            // combResponseType
            // 
            this.combResponseType.AutoToolTip = true;
            this.combResponseType.ForeColor = System.Drawing.Color.Red;
            this.combResponseType.MaxLength = 5;
            this.combResponseType.Name = "combResponseType";
            this.combResponseType.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.combResponseType.Size = new System.Drawing.Size(100, 25);
            this.combResponseType.ToolTipText = "Enter a timeout value or select a response mode";
            this.combResponseType.TextChanged += new System.EventHandler(this.combResponseType_TextChanged);
            // 
            // txtBrief
            // 
            this.txtBrief.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrief.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtBrief.Location = new System.Drawing.Point(43, 2);
            this.txtBrief.Name = "txtBrief";
            this.txtBrief.Size = new System.Drawing.Size(545, 21);
            this.txtBrief.TabIndex = 1;
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
            this.splitContainer5.Size = new System.Drawing.Size(591, 404);
            this.splitContainer5.SplitterDistance = 281;
            this.splitContainer5.SplitterWidth = 9;
            this.splitContainer5.TabIndex = 0;
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(591, 281);
            this.splitContainer6.SplitterDistance = 31;
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
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.toolbarReadWord);
            this.splitContainer8.Size = new System.Drawing.Size(591, 31);
            this.splitContainer8.SplitterDistance = 288;
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
            this.toolbarWriteWord.Size = new System.Drawing.Size(288, 31);
            this.toolbarWriteWord.TabIndex = 2;
            this.toolbarWriteWord.Text = "toolbarWriteWord";
            // 
            // labWriteWord
            // 
            this.labWriteWord.Name = "labWriteWord";
            this.labWriteWord.Size = new System.Drawing.Size(84, 28);
            this.labWriteWord.Text = "  Write Word";
            this.labWriteWord.Click += new System.EventHandler(this.labWriteWord_Click);
            // 
            // txtWriteWord
            // 
            this.txtWriteWord.AutoToolTip = true;
            this.txtWriteWord.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtWriteWord.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtWriteWord.MaxLength = 4;
            this.txtWriteWord.Name = "txtWriteWord";
            this.txtWriteWord.Size = new System.Drawing.Size(50, 31);
            this.txtWriteWord.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtWriteWord.ToolTipText = "enter a word(HEX string) which will be sent to target";
            this.txtWriteWord.TextChanged += new System.EventHandler(this.txtWriteWord_TextChanged);
            // 
            // toolbarReadWord
            // 
            this.toolbarReadWord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarReadWord.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarReadWord.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtReadWord,
            this.labReadWord});
            this.toolbarReadWord.Location = new System.Drawing.Point(0, 0);
            this.toolbarReadWord.Name = "toolbarReadWord";
            this.toolbarReadWord.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarReadWord.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolbarReadWord.Size = new System.Drawing.Size(299, 31);
            this.toolbarReadWord.TabIndex = 2;
            this.toolbarReadWord.Text = "toolStrip5";
            // 
            // txtReadWord
            // 
            this.txtReadWord.AutoSize = false;
            this.txtReadWord.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtReadWord.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtReadWord.MaxLength = 4;
            this.txtReadWord.Name = "txtReadWord";
            this.txtReadWord.ReadOnly = true;
            this.txtReadWord.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtReadWord.Size = new System.Drawing.Size(50, 27);
            this.txtReadWord.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labReadWord
            // 
            this.labReadWord.Name = "labReadWord";
            this.labReadWord.Size = new System.Drawing.Size(83, 28);
            this.labReadWord.Text = "Read Word  ";
            this.labReadWord.Click += new System.EventHandler(this.labReadWord_Click);
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.splitContainer11);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer7.Size = new System.Drawing.Size(591, 246);
            this.splitContainer7.SplitterDistance = 128;
            this.splitContainer7.TabIndex = 0;
            // 
            // splitContainer11
            // 
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            this.splitContainer11.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer11.Panel1
            // 
            this.splitContainer11.Panel1.Controls.Add(this.checkWriteBlockShowHEX);
            this.splitContainer11.Panel1.Controls.Add(this.labWriteBlock);
            // 
            // splitContainer11.Panel2
            // 
            this.splitContainer11.Panel2.Controls.Add(this.txtWriteBlock);
            this.splitContainer11.Size = new System.Drawing.Size(591, 128);
            this.splitContainer11.SplitterDistance = 35;
            this.splitContainer11.TabIndex = 0;
            // 
            // checkWriteBlockShowHEX
            // 
            this.checkWriteBlockShowHEX.AutoSize = true;
            this.checkWriteBlockShowHEX.Checked = true;
            this.checkWriteBlockShowHEX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkWriteBlockShowHEX.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkWriteBlockShowHEX.Location = new System.Drawing.Point(549, 0);
            this.checkWriteBlockShowHEX.Name = "checkWriteBlockShowHEX";
            this.checkWriteBlockShowHEX.Size = new System.Drawing.Size(42, 35);
            this.checkWriteBlockShowHEX.TabIndex = 6;
            this.checkWriteBlockShowHEX.Text = "HEX";
            this.checkWriteBlockShowHEX.UseVisualStyleBackColor = true;
            this.checkWriteBlockShowHEX.CheckedChanged += new System.EventHandler(this.checkWriteBlockShowHEX_CheckedChanged);
            // 
            // labWriteBlock
            // 
            this.labWriteBlock.Dock = System.Windows.Forms.DockStyle.Left;
            this.labWriteBlock.Location = new System.Drawing.Point(0, 0);
            this.labWriteBlock.Name = "labWriteBlock";
            this.labWriteBlock.Size = new System.Drawing.Size(123, 35);
            this.labWriteBlock.TabIndex = 2;
            this.labWriteBlock.Text = "  Write Block";
            this.labWriteBlock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labWriteBlock.Click += new System.EventHandler(this.labWriteBlock_Click);
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
            this.txtWriteBlock.Size = new System.Drawing.Size(591, 89);
            this.txtWriteBlock.TabIndex = 4;
            this.txtWriteBlock.TextChanged += new System.EventHandler(this.txtWriteBlock_TextChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.checkReadBlockShowHEX);
            this.splitContainer1.Panel1.Controls.Add(this.labReadBlock);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtReadBlock);
            this.splitContainer1.Size = new System.Drawing.Size(591, 114);
            this.splitContainer1.SplitterDistance = 33;
            this.splitContainer1.TabIndex = 0;
            // 
            // checkReadBlockShowHEX
            // 
            this.checkReadBlockShowHEX.AutoSize = true;
            this.checkReadBlockShowHEX.Checked = true;
            this.checkReadBlockShowHEX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkReadBlockShowHEX.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkReadBlockShowHEX.Location = new System.Drawing.Point(549, 0);
            this.checkReadBlockShowHEX.Name = "checkReadBlockShowHEX";
            this.checkReadBlockShowHEX.Size = new System.Drawing.Size(42, 33);
            this.checkReadBlockShowHEX.TabIndex = 5;
            this.checkReadBlockShowHEX.Text = "HEX";
            this.checkReadBlockShowHEX.UseVisualStyleBackColor = true;
            this.checkReadBlockShowHEX.CheckedChanged += new System.EventHandler(this.checkReadBlockShowHEX_CheckedChanged);
            // 
            // labReadBlock
            // 
            this.labReadBlock.Dock = System.Windows.Forms.DockStyle.Left;
            this.labReadBlock.Location = new System.Drawing.Point(0, 0);
            this.labReadBlock.Name = "labReadBlock";
            this.labReadBlock.Size = new System.Drawing.Size(123, 33);
            this.labReadBlock.TabIndex = 4;
            this.labReadBlock.Text = "  Read Block";
            this.labReadBlock.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labReadBlock.Click += new System.EventHandler(this.labReadBlock_Click);
            // 
            // txtReadBlock
            // 
            this.txtReadBlock.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtReadBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReadBlock.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtReadBlock.Location = new System.Drawing.Point(0, 0);
            this.txtReadBlock.Multiline = true;
            this.txtReadBlock.Name = "txtReadBlock";
            this.txtReadBlock.ReadOnly = true;
            this.txtReadBlock.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReadBlock.Size = new System.Drawing.Size(591, 77);
            this.txtReadBlock.TabIndex = 6;
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
            this.splitContainer9.Panel1.Controls.Add(this.splitContainer10);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer9.Panel2.Controls.Add(this.txtDescription);
            this.splitContainer9.Size = new System.Drawing.Size(591, 114);
            this.splitContainer9.SplitterDistance = 25;
            this.splitContainer9.TabIndex = 0;
            // 
            // splitContainer10
            // 
            this.splitContainer10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer10.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer10.IsSplitterFixed = true;
            this.splitContainer10.Location = new System.Drawing.Point(0, 0);
            this.splitContainer10.Name = "splitContainer10";
            // 
            // splitContainer10.Panel1
            // 
            this.splitContainer10.Panel1.Controls.Add(this.toolbarCommandType);
            // 
            // splitContainer10.Panel2
            // 
            this.splitContainer10.Panel2.Controls.Add(this.cmdDoIt);
            this.splitContainer10.Size = new System.Drawing.Size(591, 25);
            this.splitContainer10.SplitterDistance = 473;
            this.splitContainer10.TabIndex = 0;
            // 
            // toolbarCommandType
            // 
            this.toolbarCommandType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarCommandType.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarCommandType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labCommandType,
            this.combCommandType,
            this.labFrameType,
            this.combFrameType});
            this.toolbarCommandType.Location = new System.Drawing.Point(0, 0);
            this.toolbarCommandType.Name = "toolbarCommandType";
            this.toolbarCommandType.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarCommandType.Size = new System.Drawing.Size(473, 25);
            this.toolbarCommandType.TabIndex = 3;
            this.toolbarCommandType.Text = "toolStrip6";
            // 
            // labCommandType
            // 
            this.labCommandType.Name = "labCommandType";
            this.labCommandType.Size = new System.Drawing.Size(116, 22);
            this.labCommandType.Text = "  Command Type  ";
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
            this.combCommandType.Size = new System.Drawing.Size(100, 25);
            this.combCommandType.ToolTipText = "Please select a command type here";
            this.combCommandType.SelectedIndexChanged += new System.EventHandler(this.combCommandType_SelectedIndexChanged);
            // 
            // labFrameType
            // 
            this.labFrameType.Name = "labFrameType";
            this.labFrameType.Size = new System.Drawing.Size(92, 22);
            this.labFrameType.Text = "    Frame Type";
            // 
            // combFrameType
            // 
            this.combFrameType.AutoSize = false;
            this.combFrameType.AutoToolTip = true;
            this.combFrameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combFrameType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combFrameType.Items.AddRange(new object[] {
            "ATXBattery compatiable",
            "ATSB200 compatiable",
            "ATBM300 compatiable"});
            this.combFrameType.Name = "combFrameType";
            this.combFrameType.Size = new System.Drawing.Size(120, 25);
            this.combFrameType.ToolTipText = "Telegraph type";
            // 
            // cmdDoIt
            // 
            this.cmdDoIt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmdDoIt.Location = new System.Drawing.Point(0, 0);
            this.cmdDoIt.Name = "cmdDoIt";
            this.cmdDoIt.Size = new System.Drawing.Size(114, 25);
            this.cmdDoIt.TabIndex = 0;
            this.cmdDoIt.Text = "Execute";
            this.cmdDoIt.UseVisualStyleBackColor = true;
            this.cmdDoIt.Click += new System.EventHandler(this.cmdDoIt_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Enabled = false;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDescription.ForeColor = System.Drawing.SystemColors.GrayText;
            this.txtDescription.Location = new System.Drawing.Point(0, 0);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(591, 85);
            this.txtDescription.TabIndex = 0;
            // 
            // tabDebug
            // 
            this.tabDebug.Controls.Add(this.panelDebug);
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(5);
            this.tabDebug.Size = new System.Drawing.Size(623, 513);
            this.tabDebug.TabIndex = 1;
            this.tabDebug.Text = "Debug";
            this.tabDebug.UseVisualStyleBackColor = true;
            // 
            // panelDebug
            // 
            this.panelDebug.Controls.Add(this.grpDebug);
            this.panelDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDebug.Location = new System.Drawing.Point(5, 5);
            this.panelDebug.Name = "panelDebug";
            this.panelDebug.Padding = new System.Windows.Forms.Padding(3);
            this.panelDebug.Size = new System.Drawing.Size(613, 503);
            this.panelDebug.TabIndex = 0;
            // 
            // grpDebug
            // 
            this.grpDebug.Controls.Add(this.lvDebugMessage);
            this.grpDebug.Controls.Add(this.toolbarDebug);
            this.grpDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDebug.Location = new System.Drawing.Point(3, 3);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Padding = new System.Windows.Forms.Padding(5);
            this.grpDebug.Size = new System.Drawing.Size(607, 497);
            this.grpDebug.TabIndex = 0;
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
            this.lvDebugMessage.Size = new System.Drawing.Size(597, 448);
            this.lvDebugMessage.TabIndex = 3;
            this.lvDebugMessage.UseCompatibleStateImageBehavior = false;
            this.lvDebugMessage.View = System.Windows.Forms.View.Details;
            this.lvDebugMessage.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvDebugMessage_MouseDoubleClick);
            this.lvDebugMessage.Resize += new System.EventHandler(this.lvDebugMessage_Resize);
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
            this.toolStripSeparator2,
            this.cmdClear,
            this.toolStripSeparator3,
            this.cmdEnableDebug,
            this.toolStripSeparator4});
            this.toolbarDebug.Location = new System.Drawing.Point(5, 19);
            this.toolbarDebug.Name = "toolbarDebug";
            this.toolbarDebug.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarDebug.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolbarDebug.Size = new System.Drawing.Size(597, 25);
            this.toolbarDebug.TabIndex = 2;
            this.toolbarDebug.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdClear
            // 
            this.cmdClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdClear.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.cmdClear.Image = ((System.Drawing.Image)(resources.GetObject("cmdClear.Image")));
            this.cmdClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(70, 22);
            this.cmdClear.Text = "   Clear    ";
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdEnableDebug
            // 
            this.cmdEnableDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdEnableDebug.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.cmdEnableDebug.Image = ((System.Drawing.Image)(resources.GetObject("cmdEnableDebug.Image")));
            this.cmdEnableDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdEnableDebug.Name = "cmdEnableDebug";
            this.cmdEnableDebug.Size = new System.Drawing.Size(79, 22);
            this.cmdEnableDebug.Text = "   Debug    ";
            this.cmdEnableDebug.Click += new System.EventHandler(this.cmdEnableDebug_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tabAdatperInfo
            // 
            this.tabAdatperInfo.Controls.Add(this.panelInformation);
            this.tabAdatperInfo.Location = new System.Drawing.Point(4, 22);
            this.tabAdatperInfo.Name = "tabAdatperInfo";
            this.tabAdatperInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabAdatperInfo.Size = new System.Drawing.Size(623, 513);
            this.tabAdatperInfo.TabIndex = 2;
            this.tabAdatperInfo.Text = "Properties";
            this.tabAdatperInfo.UseVisualStyleBackColor = true;
            // 
            // panelInformation
            // 
            this.panelInformation.Controls.Add(this.grpInformation);
            this.panelInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInformation.Location = new System.Drawing.Point(3, 3);
            this.panelInformation.Name = "panelInformation";
            this.panelInformation.Padding = new System.Windows.Forms.Padding(3);
            this.panelInformation.Size = new System.Drawing.Size(617, 507);
            this.panelInformation.TabIndex = 0;
            // 
            // grpInformation
            // 
            this.grpInformation.Controls.Add(this.splitContainer4);
            this.grpInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpInformation.Location = new System.Drawing.Point(3, 3);
            this.grpInformation.Name = "grpInformation";
            this.grpInformation.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.grpInformation.Size = new System.Drawing.Size(611, 501);
            this.grpInformation.TabIndex = 0;
            this.grpInformation.TabStop = false;
            this.grpInformation.Text = "Information";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(10, 23);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer4.Size = new System.Drawing.Size(591, 469);
            this.splitContainer4.SplitterDistance = 440;
            this.splitContainer4.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.20168F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.79832F));
            this.tableLayoutPanel1.Controls.Add(this.txtConnectionState, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtSetting, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtDeviceInfo, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtDriverVersion, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDeviceType, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtAdapterKey, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtAdapterType, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labConnectionState, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labSetting, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labDeviceInfo, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labDriverVersion, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labDeviceType, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labAdapterKey, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labAdatperType, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(591, 440);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // txtConnectionState
            // 
            this.txtConnectionState.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtConnectionState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConnectionState.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtConnectionState.Location = new System.Drawing.Point(146, 171);
            this.txtConnectionState.Multiline = true;
            this.txtConnectionState.Name = "txtConnectionState";
            this.txtConnectionState.ReadOnly = true;
            this.txtConnectionState.Size = new System.Drawing.Size(442, 22);
            this.txtConnectionState.TabIndex = 21;
            // 
            // txtSetting
            // 
            this.txtSetting.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSetting.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtSetting.Location = new System.Drawing.Point(146, 143);
            this.txtSetting.Multiline = true;
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(442, 22);
            this.txtSetting.TabIndex = 20;
            // 
            // txtDeviceInfo
            // 
            this.txtDeviceInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtDeviceInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeviceInfo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDeviceInfo.Location = new System.Drawing.Point(146, 115);
            this.txtDeviceInfo.Multiline = true;
            this.txtDeviceInfo.Name = "txtDeviceInfo";
            this.txtDeviceInfo.ReadOnly = true;
            this.txtDeviceInfo.Size = new System.Drawing.Size(442, 22);
            this.txtDeviceInfo.TabIndex = 19;
            // 
            // txtDriverVersion
            // 
            this.txtDriverVersion.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtDriverVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDriverVersion.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDriverVersion.Location = new System.Drawing.Point(146, 87);
            this.txtDriverVersion.Multiline = true;
            this.txtDriverVersion.Name = "txtDriverVersion";
            this.txtDriverVersion.ReadOnly = true;
            this.txtDriverVersion.Size = new System.Drawing.Size(442, 22);
            this.txtDriverVersion.TabIndex = 18;
            // 
            // txtDeviceType
            // 
            this.txtDeviceType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtDeviceType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDeviceType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDeviceType.Location = new System.Drawing.Point(146, 59);
            this.txtDeviceType.Multiline = true;
            this.txtDeviceType.Name = "txtDeviceType";
            this.txtDeviceType.ReadOnly = true;
            this.txtDeviceType.Size = new System.Drawing.Size(442, 22);
            this.txtDeviceType.TabIndex = 17;
            // 
            // txtAdapterKey
            // 
            this.txtAdapterKey.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtAdapterKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAdapterKey.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtAdapterKey.Location = new System.Drawing.Point(146, 31);
            this.txtAdapterKey.Multiline = true;
            this.txtAdapterKey.Name = "txtAdapterKey";
            this.txtAdapterKey.ReadOnly = true;
            this.txtAdapterKey.Size = new System.Drawing.Size(442, 22);
            this.txtAdapterKey.TabIndex = 16;
            this.txtAdapterKey.Visible = false;
            // 
            // txtAdapterType
            // 
            this.txtAdapterType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtAdapterType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAdapterType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtAdapterType.Location = new System.Drawing.Point(146, 3);
            this.txtAdapterType.Multiline = true;
            this.txtAdapterType.Name = "txtAdapterType";
            this.txtAdapterType.ReadOnly = true;
            this.txtAdapterType.Size = new System.Drawing.Size(442, 22);
            this.txtAdapterType.TabIndex = 15;
            // 
            // labConnectionState
            // 
            this.labConnectionState.Dock = System.Windows.Forms.DockStyle.Left;
            this.labConnectionState.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labConnectionState.Location = new System.Drawing.Point(3, 168);
            this.labConnectionState.Name = "labConnectionState";
            this.labConnectionState.Size = new System.Drawing.Size(109, 28);
            this.labConnectionState.TabIndex = 14;
            this.labConnectionState.Text = "Connection State";
            this.labConnectionState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labSetting
            // 
            this.labSetting.Dock = System.Windows.Forms.DockStyle.Left;
            this.labSetting.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labSetting.Location = new System.Drawing.Point(3, 140);
            this.labSetting.Name = "labSetting";
            this.labSetting.Size = new System.Drawing.Size(109, 28);
            this.labSetting.TabIndex = 13;
            this.labSetting.Text = "Setting";
            this.labSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDeviceInfo
            // 
            this.labDeviceInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.labDeviceInfo.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labDeviceInfo.Location = new System.Drawing.Point(3, 112);
            this.labDeviceInfo.Name = "labDeviceInfo";
            this.labDeviceInfo.Size = new System.Drawing.Size(109, 28);
            this.labDeviceInfo.TabIndex = 12;
            this.labDeviceInfo.Text = "Device Information";
            this.labDeviceInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDriverVersion
            // 
            this.labDriverVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.labDriverVersion.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labDriverVersion.Location = new System.Drawing.Point(3, 84);
            this.labDriverVersion.Name = "labDriverVersion";
            this.labDriverVersion.Size = new System.Drawing.Size(109, 28);
            this.labDriverVersion.TabIndex = 11;
            this.labDriverVersion.Text = "Driver Version";
            this.labDriverVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDeviceType
            // 
            this.labDeviceType.Dock = System.Windows.Forms.DockStyle.Left;
            this.labDeviceType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labDeviceType.Location = new System.Drawing.Point(3, 56);
            this.labDeviceType.Name = "labDeviceType";
            this.labDeviceType.Size = new System.Drawing.Size(109, 28);
            this.labDeviceType.TabIndex = 10;
            this.labDeviceType.Text = "Device Type";
            this.labDeviceType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labAdapterKey
            // 
            this.labAdapterKey.Dock = System.Windows.Forms.DockStyle.Left;
            this.labAdapterKey.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labAdapterKey.Location = new System.Drawing.Point(3, 28);
            this.labAdapterKey.Name = "labAdapterKey";
            this.labAdapterKey.Size = new System.Drawing.Size(109, 28);
            this.labAdapterKey.TabIndex = 9;
            this.labAdapterKey.Text = "Adapter Key";
            this.labAdapterKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labAdapterKey.Visible = false;
            // 
            // labAdatperType
            // 
            this.labAdatperType.Dock = System.Windows.Forms.DockStyle.Left;
            this.labAdatperType.ForeColor = System.Drawing.SystemColors.WindowText;
            this.labAdatperType.Location = new System.Drawing.Point(3, 0);
            this.labAdatperType.Name = "labAdatperType";
            this.labAdatperType.Size = new System.Drawing.Size(109, 28);
            this.labAdatperType.TabIndex = 8;
            this.labAdatperType.Text = "Adapter Type";
            this.labAdatperType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerRefreshDebugMessage
            // 
            this.timerRefreshDebugMessage.Tick += new System.EventHandler(this.timerRefreshDebugMessage_Tick);
            // 
            // frmTelegraphHIDAdapterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 571);
            this.Controls.Add(this.panelTest);
            this.Controls.Add(this.StatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTelegraphHIDAdapterEditor";
            this.Text = "Telegraph HID Adapter Editor";
            this.TopMost = true;
            this.panelTest.ResumeLayout(false);
            this.tabsAdatper.ResumeLayout(false);
            this.tabDevices.ResumeLayout(false);
            this.panelDevices.ResumeLayout(false);
            this.grpDeviceManager.ResumeLayout(false);
            this.grpDeviceManager.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabCommunication.ResumeLayout(false);
            this.panelCommunication.ResumeLayout(false);
            this.grbSMBus.ResumeLayout(false);
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
            this.splitContainer8.Panel2.ResumeLayout(false);
            this.splitContainer8.Panel2.PerformLayout();
            this.splitContainer8.ResumeLayout(false);
            this.toolbarWriteWord.ResumeLayout(false);
            this.toolbarWriteWord.PerformLayout();
            this.toolbarReadWord.ResumeLayout(false);
            this.toolbarReadWord.PerformLayout();
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            this.splitContainer7.ResumeLayout(false);
            this.splitContainer11.Panel1.ResumeLayout(false);
            this.splitContainer11.Panel1.PerformLayout();
            this.splitContainer11.Panel2.ResumeLayout(false);
            this.splitContainer11.Panel2.PerformLayout();
            this.splitContainer11.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel2.ResumeLayout(false);
            this.splitContainer9.Panel2.PerformLayout();
            this.splitContainer9.ResumeLayout(false);
            this.splitContainer10.Panel1.ResumeLayout(false);
            this.splitContainer10.Panel1.PerformLayout();
            this.splitContainer10.Panel2.ResumeLayout(false);
            this.splitContainer10.ResumeLayout(false);
            this.toolbarCommandType.ResumeLayout(false);
            this.toolbarCommandType.PerformLayout();
            this.tabDebug.ResumeLayout(false);
            this.panelDebug.ResumeLayout(false);
            this.grpDebug.ResumeLayout(false);
            this.grpDebug.PerformLayout();
            this.toolbarDebug.ResumeLayout(false);
            this.toolbarDebug.PerformLayout();
            this.tabAdatperInfo.ResumeLayout(false);
            this.panelInformation.ResumeLayout(false);
            this.grpInformation.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.Panel panelTest;
        private System.Windows.Forms.TabControl tabsAdatper;
        private System.Windows.Forms.Panel panelCommunication;
        private System.Windows.Forms.Panel panelDebug;
        private System.Windows.Forms.Panel panelInformation;
        private System.Windows.Forms.GroupBox grbSMBus;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox txtBrief;
        private System.Windows.Forms.Label labBrief;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.ToolStrip toolbarWriteWord;
        private System.Windows.Forms.ToolStripLabel labWriteWord;
        private System.Windows.Forms.ToolStripTextBox txtWriteWord;
        private System.Windows.Forms.ToolStrip toolbarReadWord;
        private System.Windows.Forms.ToolStripTextBox txtReadWord;
        private System.Windows.Forms.ToolStripLabel labReadWord;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.SplitContainer splitContainer10;
        private System.Windows.Forms.ToolStrip toolbarCommandType;
        private System.Windows.Forms.ToolStripLabel labCommandType;
        private System.Windows.Forms.ToolStripComboBox combCommandType;
        private System.Windows.Forms.Button cmdDoIt;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labReadBlock;
        private System.Windows.Forms.TextBox txtReadBlock;
        private System.Windows.Forms.CheckBox checkReadBlockShowHEX;
        private System.Windows.Forms.ListView lvDebugMessage;
        private System.Windows.Forms.ColumnHeader chMSGDirection;
        private System.Windows.Forms.ColumnHeader chDisplayData;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.ToolStrip toolbarDebug;
        private System.Windows.Forms.ToolStripButton cmdEnableDebug;
        private System.Windows.Forms.ToolStripButton cmdClear;
        private System.Windows.Forms.ColumnHeader chLength;
        private System.Windows.Forms.SplitContainer splitContainer11;
        private System.Windows.Forms.Label labWriteBlock;
        private System.Windows.Forms.TextBox txtWriteBlock;
        private System.Windows.Forms.CheckBox checkWriteBlockShowHEX;
        private System.Windows.Forms.ToolStrip toolbarCommand;
        private System.Windows.Forms.ToolStripLabel labCommand;
        private System.Windows.Forms.ToolStripLabel labAddress;
        private System.Windows.Forms.ToolStripComboBox combAddress;
        private System.Windows.Forms.ToolStripLabel labResponseType;
        private System.Windows.Forms.ToolStripComboBox combResponseType;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtConnectionState;
        private System.Windows.Forms.TextBox txtSetting;
        private System.Windows.Forms.TextBox txtDeviceInfo;
        private System.Windows.Forms.TextBox txtDriverVersion;
        private System.Windows.Forms.TextBox txtDeviceType;
        private System.Windows.Forms.TextBox txtAdapterKey;
        private System.Windows.Forms.TextBox txtAdapterType;
        private System.Windows.Forms.Label labConnectionState;
        private System.Windows.Forms.Label labSetting;
        private System.Windows.Forms.Label labDeviceInfo;
        private System.Windows.Forms.Label labDriverVersion;
        private System.Windows.Forms.Label labDeviceType;
        private System.Windows.Forms.Label labAdapterKey;
        private System.Windows.Forms.Label labAdatperType;
        private System.Windows.Forms.ToolStripLabel labFrameType;
        private System.Windows.Forms.ToolStripComboBox combFrameType;
        private System.Windows.Forms.Panel panelDevices;
        private System.Windows.Forms.GroupBox grpDeviceManager;
        private System.Windows.Forms.ListView lvHIDDevice;
        private System.Windows.Forms.ColumnHeader colhNumber;
        private System.Windows.Forms.ColumnHeader colhSerialNumber;
        private System.Windows.Forms.ColumnHeader colhState;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton cmdRefreshDevice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel labVenderID;
        private System.Windows.Forms.ToolStripTextBox txtVID;
        private System.Windows.Forms.ToolStripLabel labProductID;
        private System.Windows.Forms.ToolStripTextBox txtPID;
        private System.Windows.Forms.ToolStripTextBox txtCommand;
        public System.Windows.Forms.GroupBox grpDebug;
        public System.Windows.Forms.GroupBox grpInformation;
        private System.Windows.Forms.ColumnHeader chBrief;
        internal System.Windows.Forms.TabPage tabDebug;
        internal System.Windows.Forms.TabPage tabCommunication;
        internal System.Windows.Forms.TabPage tabAdatperInfo;
        internal System.Windows.Forms.TabPage tabDevices;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel labSubAddress;
        private System.Windows.Forms.ToolStripTextBox txtSubAddress;
        private System.Windows.Forms.Timer timerRefreshDebugMessage;
    }
}