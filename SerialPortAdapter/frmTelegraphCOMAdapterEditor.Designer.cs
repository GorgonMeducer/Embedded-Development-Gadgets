namespace ESnail.Device.Adapters.SerialPort
{
    partial class frmTelegraphCOMAdapterEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTelegraphCOMAdapterEditor));
            this.grbInfo = new System.Windows.Forms.GroupBox();
            this.txtConnectionState = new System.Windows.Forms.TextBox();
            this.labConnection = new System.Windows.Forms.Label();
            this.txtSetting = new System.Windows.Forms.TextBox();
            this.labSettings = new System.Windows.Forms.Label();
            this.txtDeviceInfo = new System.Windows.Forms.TextBox();
            this.labDeviceInfo = new System.Windows.Forms.Label();
            this.txtDriverVersion = new System.Windows.Forms.TextBox();
            this.labDriverVersion = new System.Windows.Forms.Label();
            this.txtDeviceType = new System.Windows.Forms.TextBox();
            this.labDeviceType = new System.Windows.Forms.Label();
            this.txtAdapterKey = new System.Windows.Forms.TextBox();
            this.labAdapterKey = new System.Windows.Forms.Label();
            this.txtAdapterType = new System.Windows.Forms.TextBox();
            this.labAdapterType = new System.Windows.Forms.Label();
            this.grbSerialPortSettings = new System.Windows.Forms.GroupBox();
            this.comboStopBits = new System.Windows.Forms.ComboBox();
            this.labStopBits = new System.Windows.Forms.Label();
            this.comboParity = new System.Windows.Forms.ComboBox();
            this.labParity = new System.Windows.Forms.Label();
            this.combDatabits = new System.Windows.Forms.ComboBox();
            this.labDatabits = new System.Windows.Forms.Label();
            this.combBaudrate = new System.Windows.Forms.ComboBox();
            this.labBaudrate = new System.Windows.Forms.Label();
            this.grpDeviceManager = new System.Windows.Forms.GroupBox();
            this.lvHIDDevice = new System.Windows.Forms.ListView();
            this.colhNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhSerialPortName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhConnection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colhState = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdRefreshDevice = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.tabDeviceManager.SuspendLayout();
            this.tabInformation.SuspendLayout();
            this.panelDeviceManager.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.grbInfo.SuspendLayout();
            this.grbSerialPortSettings.SuspendLayout();
            this.grpDeviceManager.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabDeviceManager
            // 
            this.tabDeviceManager.Location = new System.Drawing.Point(4, 22);
            this.tabDeviceManager.Size = new System.Drawing.Size(561, 570);
            // 
            // tabDebug
            // 
            this.tabDebug.Location = new System.Drawing.Point(4, 22);
            this.tabDebug.Size = new System.Drawing.Size(561, 570);
            // 
            // tabInformation
            // 
            this.tabInformation.Location = new System.Drawing.Point(4, 22);
            this.tabInformation.Size = new System.Drawing.Size(561, 570);
            // 
            // panelDeviceManager
            // 
            this.panelDeviceManager.Controls.Add(this.grpDeviceManager);
            this.panelDeviceManager.Size = new System.Drawing.Size(551, 560);
            // 
            // panelProperties
            // 
            this.panelProperties.AutoScroll = true;
            this.panelProperties.Controls.Add(this.grbSerialPortSettings);
            this.panelProperties.Controls.Add(this.grbInfo);
            this.panelProperties.Size = new System.Drawing.Size(555, 564);
            // 
            // grbInfo
            // 
            this.grbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbInfo.Controls.Add(this.txtConnectionState);
            this.grbInfo.Controls.Add(this.labConnection);
            this.grbInfo.Controls.Add(this.txtSetting);
            this.grbInfo.Controls.Add(this.labSettings);
            this.grbInfo.Controls.Add(this.txtDeviceInfo);
            this.grbInfo.Controls.Add(this.labDeviceInfo);
            this.grbInfo.Controls.Add(this.txtDriverVersion);
            this.grbInfo.Controls.Add(this.labDriverVersion);
            this.grbInfo.Controls.Add(this.txtDeviceType);
            this.grbInfo.Controls.Add(this.labDeviceType);
            this.grbInfo.Controls.Add(this.txtAdapterKey);
            this.grbInfo.Controls.Add(this.labAdapterKey);
            this.grbInfo.Controls.Add(this.txtAdapterType);
            this.grbInfo.Controls.Add(this.labAdapterType);
            this.grbInfo.Location = new System.Drawing.Point(8, 3);
            this.grbInfo.Name = "grbInfo";
            this.grbInfo.Padding = new System.Windows.Forms.Padding(5);
            this.grbInfo.Size = new System.Drawing.Size(539, 254);
            this.grbInfo.TabIndex = 1;
            this.grbInfo.TabStop = false;
            this.grbInfo.Text = "Information";
            // 
            // txtConnectionState
            // 
            this.txtConnectionState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionState.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtConnectionState.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtConnectionState.Location = new System.Drawing.Point(98, 209);
            this.txtConnectionState.Margin = new System.Windows.Forms.Padding(5);
            this.txtConnectionState.Name = "txtConnectionState";
            this.txtConnectionState.ReadOnly = true;
            this.txtConnectionState.Size = new System.Drawing.Size(428, 20);
            this.txtConnectionState.TabIndex = 14;
            // 
            // labConnection
            // 
            this.labConnection.AutoSize = true;
            this.labConnection.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labConnection.Location = new System.Drawing.Point(10, 212);
            this.labConnection.Name = "labConnection";
            this.labConnection.Size = new System.Drawing.Size(64, 13);
            this.labConnection.TabIndex = 13;
            this.labConnection.Text = "Connection ";
            // 
            // txtSetting
            // 
            this.txtSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSetting.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtSetting.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtSetting.Location = new System.Drawing.Point(98, 179);
            this.txtSetting.Margin = new System.Windows.Forms.Padding(5);
            this.txtSetting.Name = "txtSetting";
            this.txtSetting.ReadOnly = true;
            this.txtSetting.Size = new System.Drawing.Size(428, 20);
            this.txtSetting.TabIndex = 12;
            // 
            // labSettings
            // 
            this.labSettings.AutoSize = true;
            this.labSettings.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labSettings.Location = new System.Drawing.Point(10, 182);
            this.labSettings.Name = "labSettings";
            this.labSettings.Size = new System.Drawing.Size(54, 13);
            this.labSettings.TabIndex = 11;
            this.labSettings.Text = "PortName";
            // 
            // txtDeviceInfo
            // 
            this.txtDeviceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeviceInfo.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtDeviceInfo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDeviceInfo.Location = new System.Drawing.Point(98, 150);
            this.txtDeviceInfo.Margin = new System.Windows.Forms.Padding(5);
            this.txtDeviceInfo.Name = "txtDeviceInfo";
            this.txtDeviceInfo.ReadOnly = true;
            this.txtDeviceInfo.Size = new System.Drawing.Size(428, 20);
            this.txtDeviceInfo.TabIndex = 10;
            // 
            // labDeviceInfo
            // 
            this.labDeviceInfo.AutoSize = true;
            this.labDeviceInfo.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labDeviceInfo.Location = new System.Drawing.Point(10, 152);
            this.labDeviceInfo.Name = "labDeviceInfo";
            this.labDeviceInfo.Size = new System.Drawing.Size(40, 13);
            this.labDeviceInfo.TabIndex = 9;
            this.labDeviceInfo.Text = "Setting";
            // 
            // txtDriverVersion
            // 
            this.txtDriverVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDriverVersion.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtDriverVersion.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDriverVersion.Location = new System.Drawing.Point(98, 119);
            this.txtDriverVersion.Margin = new System.Windows.Forms.Padding(5);
            this.txtDriverVersion.Name = "txtDriverVersion";
            this.txtDriverVersion.ReadOnly = true;
            this.txtDriverVersion.Size = new System.Drawing.Size(428, 20);
            this.txtDriverVersion.TabIndex = 8;
            // 
            // labDriverVersion
            // 
            this.labDriverVersion.AutoSize = true;
            this.labDriverVersion.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labDriverVersion.Location = new System.Drawing.Point(10, 122);
            this.labDriverVersion.Name = "labDriverVersion";
            this.labDriverVersion.Size = new System.Drawing.Size(73, 13);
            this.labDriverVersion.TabIndex = 7;
            this.labDriverVersion.Text = "Driver Version";
            // 
            // txtDeviceType
            // 
            this.txtDeviceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeviceType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtDeviceType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtDeviceType.Location = new System.Drawing.Point(98, 89);
            this.txtDeviceType.Margin = new System.Windows.Forms.Padding(5);
            this.txtDeviceType.Name = "txtDeviceType";
            this.txtDeviceType.ReadOnly = true;
            this.txtDeviceType.Size = new System.Drawing.Size(428, 20);
            this.txtDeviceType.TabIndex = 6;
            // 
            // labDeviceType
            // 
            this.labDeviceType.AutoSize = true;
            this.labDeviceType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labDeviceType.Location = new System.Drawing.Point(10, 92);
            this.labDeviceType.Name = "labDeviceType";
            this.labDeviceType.Size = new System.Drawing.Size(68, 13);
            this.labDeviceType.TabIndex = 5;
            this.labDeviceType.Text = "Device Type";
            // 
            // txtAdapterKey
            // 
            this.txtAdapterKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdapterKey.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtAdapterKey.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtAdapterKey.Location = new System.Drawing.Point(98, 59);
            this.txtAdapterKey.Margin = new System.Windows.Forms.Padding(5);
            this.txtAdapterKey.Name = "txtAdapterKey";
            this.txtAdapterKey.ReadOnly = true;
            this.txtAdapterKey.Size = new System.Drawing.Size(428, 20);
            this.txtAdapterKey.TabIndex = 4;
            this.txtAdapterKey.Visible = false;
            // 
            // labAdapterKey
            // 
            this.labAdapterKey.AutoSize = true;
            this.labAdapterKey.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labAdapterKey.Location = new System.Drawing.Point(10, 62);
            this.labAdapterKey.Name = "labAdapterKey";
            this.labAdapterKey.Size = new System.Drawing.Size(65, 13);
            this.labAdapterKey.TabIndex = 3;
            this.labAdapterKey.Text = "Adapter Key";
            this.labAdapterKey.Visible = false;
            // 
            // txtAdapterType
            // 
            this.txtAdapterType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdapterType.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtAdapterType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.txtAdapterType.Location = new System.Drawing.Point(98, 29);
            this.txtAdapterType.Margin = new System.Windows.Forms.Padding(5);
            this.txtAdapterType.Name = "txtAdapterType";
            this.txtAdapterType.ReadOnly = true;
            this.txtAdapterType.Size = new System.Drawing.Size(428, 20);
            this.txtAdapterType.TabIndex = 2;
            // 
            // labAdapterType
            // 
            this.labAdapterType.AutoSize = true;
            this.labAdapterType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labAdapterType.Location = new System.Drawing.Point(10, 33);
            this.labAdapterType.Name = "labAdapterType";
            this.labAdapterType.Size = new System.Drawing.Size(71, 13);
            this.labAdapterType.TabIndex = 1;
            this.labAdapterType.Text = "Adapter Type";
            // 
            // grbSerialPortSettings
            // 
            this.grbSerialPortSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbSerialPortSettings.Controls.Add(this.comboStopBits);
            this.grbSerialPortSettings.Controls.Add(this.labStopBits);
            this.grbSerialPortSettings.Controls.Add(this.comboParity);
            this.grbSerialPortSettings.Controls.Add(this.labParity);
            this.grbSerialPortSettings.Controls.Add(this.combDatabits);
            this.grbSerialPortSettings.Controls.Add(this.labDatabits);
            this.grbSerialPortSettings.Controls.Add(this.combBaudrate);
            this.grbSerialPortSettings.Controls.Add(this.labBaudrate);
            this.grbSerialPortSettings.Location = new System.Drawing.Point(8, 263);
            this.grbSerialPortSettings.Name = "grbSerialPortSettings";
            this.grbSerialPortSettings.Size = new System.Drawing.Size(539, 167);
            this.grbSerialPortSettings.TabIndex = 2;
            this.grbSerialPortSettings.TabStop = false;
            this.grbSerialPortSettings.Text = "Serial Port Settings";
            // 
            // comboStopBits
            // 
            this.comboStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStopBits.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.comboStopBits.FormattingEnabled = true;
            this.comboStopBits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.comboStopBits.Location = new System.Drawing.Point(98, 124);
            this.comboStopBits.Margin = new System.Windows.Forms.Padding(5);
            this.comboStopBits.Name = "comboStopBits";
            this.comboStopBits.Size = new System.Drawing.Size(159, 21);
            this.comboStopBits.TabIndex = 7;
            this.comboStopBits.SelectedIndexChanged += new System.EventHandler(this.comboStopBits_SelectedIndexChanged);
            // 
            // labStopBits
            // 
            this.labStopBits.AutoSize = true;
            this.labStopBits.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labStopBits.Location = new System.Drawing.Point(10, 126);
            this.labStopBits.Name = "labStopBits";
            this.labStopBits.Size = new System.Drawing.Size(48, 13);
            this.labStopBits.TabIndex = 6;
            this.labStopBits.Text = "Stop bits";
            // 
            // comboParity
            // 
            this.comboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboParity.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.comboParity.FormattingEnabled = true;
            this.comboParity.Items.AddRange(new object[] {
            "Even",
            "Odd",
            "None",
            "Mark",
            "Space"});
            this.comboParity.Location = new System.Drawing.Point(98, 92);
            this.comboParity.Margin = new System.Windows.Forms.Padding(5);
            this.comboParity.Name = "comboParity";
            this.comboParity.Size = new System.Drawing.Size(159, 21);
            this.comboParity.TabIndex = 5;
            this.comboParity.SelectedIndexChanged += new System.EventHandler(this.comboParity_SelectedIndexChanged);
            // 
            // labParity
            // 
            this.labParity.AutoSize = true;
            this.labParity.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labParity.Location = new System.Drawing.Point(10, 95);
            this.labParity.Name = "labParity";
            this.labParity.Size = new System.Drawing.Size(33, 13);
            this.labParity.TabIndex = 4;
            this.labParity.Text = "Parity";
            // 
            // combDatabits
            // 
            this.combDatabits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combDatabits.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combDatabits.FormattingEnabled = true;
            this.combDatabits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.combDatabits.Location = new System.Drawing.Point(98, 61);
            this.combDatabits.Margin = new System.Windows.Forms.Padding(5);
            this.combDatabits.Name = "combDatabits";
            this.combDatabits.Size = new System.Drawing.Size(159, 21);
            this.combDatabits.TabIndex = 3;
            this.combDatabits.SelectedIndexChanged += new System.EventHandler(this.combDatabits_SelectedIndexChanged);
            // 
            // labDatabits
            // 
            this.labDatabits.AutoSize = true;
            this.labDatabits.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labDatabits.Location = new System.Drawing.Point(10, 64);
            this.labDatabits.Name = "labDatabits";
            this.labDatabits.Size = new System.Drawing.Size(49, 13);
            this.labDatabits.TabIndex = 2;
            this.labDatabits.Text = "Data bits";
            // 
            // combBaudrate
            // 
            this.combBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combBaudrate.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combBaudrate.FormattingEnabled = true;
            this.combBaudrate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "7200",
            "9600",
            "14400",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.combBaudrate.Location = new System.Drawing.Point(98, 30);
            this.combBaudrate.Margin = new System.Windows.Forms.Padding(5);
            this.combBaudrate.Name = "combBaudrate";
            this.combBaudrate.Size = new System.Drawing.Size(159, 21);
            this.combBaudrate.TabIndex = 1;
            this.combBaudrate.SelectedIndexChanged += new System.EventHandler(this.combBaudrate_SelectedIndexChanged);
            // 
            // labBaudrate
            // 
            this.labBaudrate.AutoSize = true;
            this.labBaudrate.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.labBaudrate.Location = new System.Drawing.Point(10, 33);
            this.labBaudrate.Name = "labBaudrate";
            this.labBaudrate.Size = new System.Drawing.Size(80, 13);
            this.labBaudrate.TabIndex = 0;
            this.labBaudrate.Text = "Bits per second";
            // 
            // grpDeviceManager
            // 
            this.grpDeviceManager.Controls.Add(this.lvHIDDevice);
            this.grpDeviceManager.Controls.Add(this.toolStrip1);
            this.grpDeviceManager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDeviceManager.Location = new System.Drawing.Point(5, 5);
            this.grpDeviceManager.Name = "grpDeviceManager";
            this.grpDeviceManager.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.grpDeviceManager.Size = new System.Drawing.Size(541, 550);
            this.grpDeviceManager.TabIndex = 1;
            this.grpDeviceManager.TabStop = false;
            this.grpDeviceManager.Text = "Device Manager";
            // 
            // lvHIDDevice
            // 
            this.lvHIDDevice.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvHIDDevice.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colhNumber,
            this.colhSerialPortName,
            this.colhConnection,
            this.colhState});
            this.lvHIDDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvHIDDevice.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lvHIDDevice.FullRowSelect = true;
            this.lvHIDDevice.GridLines = true;
            this.lvHIDDevice.Location = new System.Drawing.Point(10, 43);
            this.lvHIDDevice.MultiSelect = false;
            this.lvHIDDevice.Name = "lvHIDDevice";
            this.lvHIDDevice.Size = new System.Drawing.Size(521, 497);
            this.lvHIDDevice.TabIndex = 7;
            this.lvHIDDevice.UseCompatibleStateImageBehavior = false;
            this.lvHIDDevice.View = System.Windows.Forms.View.Details;
            this.lvHIDDevice.DoubleClick += new System.EventHandler(this.lvHIDDevice_DoubleClick);
            // 
            // colhNumber
            // 
            this.colhNumber.Text = "No.";
            this.colhNumber.Width = 35;
            // 
            // colhSerialPortName
            // 
            this.colhSerialPortName.Text = "Serial Port Name";
            this.colhSerialPortName.Width = 199;
            // 
            // colhConnection
            // 
            this.colhConnection.Text = "Connection";
            this.colhConnection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colhConnection.Width = 76;
            // 
            // colhState
            // 
            this.colhState.Text = "State";
            this.colhState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.cmdRefreshDevice,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(10, 18);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStrip1.Size = new System.Drawing.Size(521, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmdRefreshDevice
            // 
            this.cmdRefreshDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cmdRefreshDevice.Image = ((System.Drawing.Image)(resources.GetObject("cmdRefreshDevice.Image")));
            this.cmdRefreshDevice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdRefreshDevice.Name = "cmdRefreshDevice";
            this.cmdRefreshDevice.Size = new System.Drawing.Size(109, 22);
            this.cmdRefreshDevice.Text = "Refresh Device List";
            this.cmdRefreshDevice.Click += new System.EventHandler(this.cmdRefreshDevice_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // frmTelegraphCOMAdapterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 628);
            this.MinimumSize = new System.Drawing.Size(335, 585);
            this.Name = "frmTelegraphCOMAdapterEditor";
            this.Text = "Telegraph Serial Port Adapter Editor";
            this.tabDeviceManager.ResumeLayout(false);
            this.tabInformation.ResumeLayout(false);
            this.panelDeviceManager.ResumeLayout(false);
            this.panelProperties.ResumeLayout(false);
            this.grbInfo.ResumeLayout(false);
            this.grbInfo.PerformLayout();
            this.grbSerialPortSettings.ResumeLayout(false);
            this.grbSerialPortSettings.PerformLayout();
            this.grpDeviceManager.ResumeLayout(false);
            this.grpDeviceManager.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbInfo;
        private System.Windows.Forms.GroupBox grbSerialPortSettings;
        private System.Windows.Forms.ComboBox comboStopBits;
        private System.Windows.Forms.Label labStopBits;
        private System.Windows.Forms.ComboBox comboParity;
        private System.Windows.Forms.Label labParity;
        private System.Windows.Forms.ComboBox combDatabits;
        private System.Windows.Forms.Label labDatabits;
        private System.Windows.Forms.ComboBox combBaudrate;
        private System.Windows.Forms.Label labBaudrate;
        private System.Windows.Forms.TextBox txtAdapterType;
        private System.Windows.Forms.Label labAdapterType;
        private System.Windows.Forms.TextBox txtDriverVersion;
        private System.Windows.Forms.Label labDriverVersion;
        private System.Windows.Forms.TextBox txtDeviceType;
        private System.Windows.Forms.Label labDeviceType;
        private System.Windows.Forms.TextBox txtAdapterKey;
        private System.Windows.Forms.Label labAdapterKey;
        private System.Windows.Forms.TextBox txtDeviceInfo;
        private System.Windows.Forms.Label labDeviceInfo;
        private System.Windows.Forms.TextBox txtSetting;
        private System.Windows.Forms.Label labSettings;
        private System.Windows.Forms.TextBox txtConnectionState;
        private System.Windows.Forms.Label labConnection;
        private System.Windows.Forms.GroupBox grpDeviceManager;
        private System.Windows.Forms.ListView lvHIDDevice;
        private System.Windows.Forms.ColumnHeader colhNumber;
        private System.Windows.Forms.ColumnHeader colhSerialPortName;
        private System.Windows.Forms.ColumnHeader colhConnection;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton cmdRefreshDevice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ColumnHeader colhState;
        private System.IO.Ports.SerialPort serialPort1;
    }
}