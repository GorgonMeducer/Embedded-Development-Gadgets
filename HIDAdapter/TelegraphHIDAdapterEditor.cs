using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;


using ESnail.CommunicationSet.Commands;
using ESnail.Device;
using ESnail.Device.Adapters.USB.HID;
using ESnail.Utilities.HEX;
using ESnail.Utilities.DEC;
using ESnail.Utilities.Log;
using ESnail.Utilities;
using ESnail.Device.Telegraphs;
using ESnail.Component;

namespace ESnail.Device.Adapters.USB.HID
{
    using ESnail.Device.Adapters.USB.HID;

    internal partial class frmTelegraphHIDAdapterEditor : Form, IAdapterEditorComponent, IBMCommand
    {
        private TelegraphHIDAdapter m_Adapter = null;
        private KeyboardIncantationMonitor m_KeyBackDoor = new KeyboardIncantationMonitor(); 

        //! default constructor
        public frmTelegraphHIDAdapterEditor()
        {
            Initialize();
        }


        //! constructor 
        public frmTelegraphHIDAdapterEditor(TelegraphHIDAdapter AdapterItem)
        {
            m_Adapter = AdapterItem;
            Initialize();
        }
         
        //! initialization
        private void Initialize()
        {
            InitializeComponent();

            AddBackDoor();
            /*
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            */

            //! initilize command type
            combCommandType.SelectedIndex = 1;
            ChangeCommandType((BM_CMD_TYPE)combCommandType.SelectedIndex);

            //! initialize frame 
            combFrameType.SelectedIndex = 1;

            //! initialize address
            combAddress.Items.Add("Adapter");
            combAddress.Items.Add("SMBus");
            combAddress.Items.Add("SMBus with PEC");
            combAddress.Items.Add("UART");
            combAddress.Items.Add("UART with PEC");
            combAddress.Items.Add("Single-wire UART");
            combAddress.Items.Add("Single-wire UART with PEC");
            combAddress.Items.Add("SPI");
            combAddress.Items.Add("SPI with PEC");
            combAddress.Items.Add("I2C");
            combAddress.Items.Add("I2C with PEC");
            combAddress.Items.Add("Loader");
            combAddress.Items.Add("Charger");
            combAddress.Items.Add("Printer");
            combAddress.Items.Add("LCD");
            combAddress.Items.Add("Extend SMBus");
            combAddress.Items.Add("Extend SMBus with PEC");
            combAddress.Items.Add("Extend UART");
            combAddress.Items.Add("Extend UART with PEC");
            combAddress.Items.Add("Extend Single-wire UART");
            combAddress.Items.Add("Extend Single-wire UART with PEC");
            combAddress.Items.Add("Extend SPI");
            combAddress.Items.Add("Extend SPI with PEC");
            combAddress.Items.Add("Extend I2C");
            combAddress.Items.Add("Extend I2C with PEC");
            combAddress.Items.Add("All");
            combAddress.SelectedIndex = 1;
            

            //! initialize timeout
            combResponseType.Items.Add("No Response");
            combResponseType.Items.Add("Wait forever");
            combResponseType.Text = "300";

            

            if (false == m_Adapter.Open)
            {
                grbSMBus.Enabled = false;
                grpDebug.Enabled = false;
            }
            else
            {
                grbSMBus.Enabled = true;
                grpDebug.Enabled = true;
            }

            //! initilize message hooker
            if (null != m_Adapter)
            {
                m_Adapter.MessageHooker += new MessageListener(CommunicationHookerSync);

                txtPID.Text = m_Adapter.PID.ToString("X4");
                txtVID.Text = m_Adapter.VID.ToString("X4");
            }
            else
            {
                grbSMBus.Enabled = false;
                grpDebug.Enabled = false;
            }

            //! properties
            txtAdapterType.Text = m_Adapter.Type;
            txtDeviceType.Text = m_Adapter.DeviceType;
            txtDriverVersion.Text = m_Adapter.DeviceVersion;
            txtDeviceInfo.Text = m_Adapter.DeviceInfo;
            txtAdapterKey.Text = m_Adapter.ID;
            txtSetting.Text = m_Adapter.Settings;
            txtConnectionState.Text = m_Adapter.Open ?　"Open" : "Close";

            if (null != m_Adapter)
            {
                m_Adapter.ConnectNotice += new ESnail.Device.Adapters.DeviceConnected(AdapterConnectEventHandler);
                m_Adapter.DisconnectNotice += new ESnail.Device.Adapters.DeviceDisconnected(AdapterDisconnectEventHandler);
                m_Adapter.DeviceClosedEvent += new DeviceClosed(m_Adapter_DeviceClosedEvent);
                m_Adapter.DeviceOpenedEvent += new DeviceOpened(m_Adapter_DeviceOpenedEvent);
            }

            if (null != m_Adapter)
            {
                combFrameType.Items.Clear();
                combFrameType.Items.AddRange(m_Adapter.SupportedTelegraph);
                if (combFrameType.Items.Count > 0)
                {
                    combFrameType.SelectedIndex = 0;
                }
                else
                {
                    cmdDoIt.Enabled = false;
                }
            }

            cmdRefreshDevice_Click(null, null);
        }

        private void AddBackDoor()
        {
            //! 第一个后门 
            do
            {
                KeyboardIncantationMonitor.KeysIncantation tInc = m_KeyBackDoor.NewIncantation() as KeyboardIncantationMonitor.KeysIncantation;

                //! 初始化这个暗号为：依次按下 <Esc>HELLO<Enter> 
                tInc.AddKey(Keys.Escape);
                tInc.AddKey(Keys.V);
                tInc.AddKey(Keys.I);
                tInc.AddKey(Keys.D);
                tInc.AddKey(Keys.P);
                tInc.AddKey(Keys.I);
                tInc.AddKey(Keys.D);
                tInc.AddKey(Keys.Enter);

                //! 对上暗号以后的处理程序 
                tInc.IncantationCantillatedReport += new IncantationReport(tInc_IncantationCantillatedReport);

                //! 将这个暗号添加到后门监视器里面 
                m_KeyBackDoor.AddIncantation(tInc);
            }
            while (false); 


        }

        private void tInc_IncantationCantillatedReport(IIncantation tInc)
        {
            txtVID.Visible = true;
            labVenderID.Visible = true;
            txtPID.Visible = true;
            labProductID.Visible = true;
        }

        void m_Adapter_DeviceOpenedEvent(SingleDeviceAdapter tAdapter)
        {
            cmdRefreshDevice_Click(null, null);
        }

        private void m_Adapter_DeviceClosedEvent(SingleDeviceAdapter tAdapter)
        {
            cmdRefreshDevice_Click(null, null);
        }

        

        private void _Dispose()
        {
            m_Adapter.MessageHooker -= new MessageListener(CommunicationHookerSync);
            m_Adapter.ConnectNotice -= new ESnail.Device.Adapters.DeviceConnected(AdapterConnectEventHandler);
            m_Adapter.DeviceClosedEvent -= new DeviceClosed(m_Adapter_DeviceClosedEvent);
            m_Adapter.DeviceOpenedEvent -= new DeviceOpened(m_Adapter_DeviceOpenedEvent);

            m_Adapter.DisconnectNotice -= new ESnail.Device.Adapters.DeviceDisconnected(AdapterDisconnectEventHandler);

            tabsAdatper.TabPages.Clear();
        }

        //! device disconnected
        private void AdapterDisconnectEventHandler()
        {
            //! refresh system state
            cmdRefreshDevice_Click(null, null);
        }

        //! device connected
        private void AdapterConnectEventHandler()
        {
            //! refresh system state
            cmdRefreshDevice_Click(null, null);
        }

        /*
        //! disctructor
        ~frmTelegraphHIDAdapterEditor()
        { 
            if (null != m_Adapter)
            {
                m_Adapter.MessageHooker -= new MessageListener(CommunicationHookerSync);
                m_Adapter.ConnectNotice -= new ESnail.Device.Adapters.DeviceConnected(AdapterConnectEventHandler);
                m_Adapter.DisconnectNotice -= new ESnail.Device.Adapters.DeviceDisconnected(AdapterDisconnectEventHandler);
            }
        }
        */

        private void ChangeCommandType(BM_CMD_TYPE dwCommandType)
        {
            /*
            switch (dwCommandType)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:  
                    //! Just command
                    checkWriteBlockShowHEX.Visible = false;
                    checkReadBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = false;          //!< write word
                    toolbarReadWord.Visible = false;           //!< read word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    //! write word
                    checkWriteBlockShowHEX.Visible = false;
                    checkReadBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = true;           //!< write word
                    toolbarReadWord.Visible = false;           //!< read word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    //! read word
                    checkWriteBlockShowHEX.Visible = false;
                    checkReadBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = false;          //!< write word
                    toolbarReadWord.Visible = true;            //!< read word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    //! write block
                    checkWriteBlockShowHEX.Visible = true;
                    checkReadBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = false;          //!< write word
                    toolbarReadWord.Visible = false;           //!< read word
                    labWriteBlock.Visible = true;              //! write block
                    txtWriteBlock.Visible = true;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    //! read block
                    checkWriteBlockShowHEX.Visible = false;
                    checkReadBlockShowHEX.Visible = true;
                    toolbarWriteWord.Visible = false;          //!< write word
                    toolbarReadWord.Visible = false;           //!< read word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = true;               //! read block
                    txtReadBlock.Visible = true;
                    break;
                default:
                    break;
            }*/
            checkWriteBlockShowHEX.Visible = true;
            checkReadBlockShowHEX.Visible = true;
            toolbarWriteWord.Visible = true;            //!< write word
            toolbarReadWord.Visible = true;             //!< read word
            labWriteBlock.Visible = true;               //! write block
            txtWriteBlock.Visible = true;
            labReadBlock.Visible = true;                //! read block
            txtReadBlock.Visible = true;
        }

        private void combCommandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCommandType((BM_CMD_TYPE)combCommandType.SelectedIndex);
        }

        private void WriteLog(String strLog)
        {
            if (null != m_Adapter)
            {
                m_Adapter.WriteLog(strLog);
            }
            Console.Write(strLog);
        }

        private void WriteLogLine(String strLog)
        {
            if (null != m_Adapter)
            {
                m_Adapter.WriteLogLine(strLog);
            }
            Console.WriteLine(strLog);
        }

        private void BeginLog()
        {
            if (null != m_Adapter)
            {
                m_Adapter.BeginLog();
            }
        }

        private void EndLog()
        {
            if (null != m_Adapter)
            {
                m_Adapter.EndLog();
            }
        }

        //* ----------------------------------------------------------------------------- *
        // * Adapter Event Receiver                                                       *
        // * --------------------------------------------------------------------------- */

        private Int32 m_RefreshDownCounter = 0;
        private List<ListViewItem> m_ListViewItems = new List<ListViewItem>();

        private void ResetRefreshDownCounter(ListViewItem tItem)
        {
            //if (0 == m_RefreshDownCounter)
            //{
                //lvDebugMessage.BeginUpdate();
            //}

                m_RefreshDownCounter = 2;

                m_ListViewItems.Add(tItem);
                if (m_ListViewItems.Count >= 100)
                {

                    ListViewItem[] tItems = m_ListViewItems.ToArray();
                    m_ListViewItems.Clear();
                    lvDebugMessage.BeginUpdate();

                    lvDebugMessage.Items.AddRange(tItems);
                    lvDebugMessage.EndUpdate();
                    lvDebugMessage.Items[lvDebugMessage.Items.Count - 1].EnsureVisible();
                }
        }

        private void CommunicationHooker(MSG_DIRECTION Direction, Byte[] Data, System.String strDescription)
        {
            ListViewItem newItem = null;

            //! initialize new item
            if (Direction == MSG_DIRECTION.INPUT_MSG)
            {
                newItem = new ListViewItem("IN");
            }
            else
            {
                newItem = new ListViewItem("OUT");
            }

            

            //! add data 
            if (null != Data)
            {
                

                newItem.SubItems.Add(Data.Length.ToString());
                
                Int32 nOutputCount = Data.Length;
                Int32 nCounter = 0;

                //if (nOutputCount >= 16)
                {
                    Byte[] OutputBuffer = null;

                    if (nOutputCount >= 16)
                    {
                        OutputBuffer = new Byte[16];
                    }
                    else
                    {
                        OutputBuffer = new Byte[nOutputCount];
                    }

                    StringBuilder sb = new StringBuilder();

                    for (Int32 n = 0; n < 16; n++)
                    {
                        nOutputCount--;
                        OutputBuffer[n] = Data[nCounter++];

                        if ((OutputBuffer[n] < 0x20) || (OutputBuffer[n] > 0x7F))
                        {
                            sb.Append('.');
                        }
                        else
                        {
                            sb.Append((Char)OutputBuffer[n]);
                        }

                        if (nOutputCount == 0)
                        {
                            
                            break;
                        }
                    }

                    newItem.SubItems.Add(HEXBuilder.ByteArrayToHEXString(OutputBuffer));
                    newItem.SubItems.Add(sb.ToString());
                    newItem.SubItems.Add(strDescription);
                    //lvDebugMessage.Items.Add(newItem);

                    ResetRefreshDownCounter(newItem);
                }

                while (nOutputCount > 0)
                {
                    newItem = new ListViewItem("");
                    newItem.SubItems.Add("");

                    Byte[] OutputBuffer = new Byte[16];
                    StringBuilder sb = new StringBuilder();
                    for (System.Int32 n = 0; n < 16; n++)
                    {
                        nOutputCount--;
                        OutputBuffer[n] = Data[nCounter++];

                        if ((OutputBuffer[n] < 0x20) || (OutputBuffer[n] > 0x7F))
                        {
                            sb.Append('.');
                        }
                        else
                        {
                            sb.Append((Char)OutputBuffer[n]);
                        }

                        if (nOutputCount == 0)
                        {
                            Array.Resize(ref OutputBuffer, n + 1);
                            break;
                        }
                    }

                    newItem.SubItems.Add(HEXBuilder.ByteArrayToHEXString(OutputBuffer));
                    newItem.SubItems.Add(sb.ToString());
                    newItem.SubItems.Add("");
                    //lvDebugMessage.Items.Add(newItem);
                    ResetRefreshDownCounter(newItem);
                }
            }
            else
            {
                newItem.SubItems.Add(Data.Length.ToString());
                newItem.SubItems.Add(" ");
                newItem.SubItems.Add(" ");
                newItem.SubItems.Add(strDescription);
                //lvDebugMessage.Items.Add(newItem);
                ResetRefreshDownCounter(newItem);
            }

            
            //! ensure this item is visible
            //newItem.EnsureVisible();
            //lvDebugMessage.EndUpdate();
        }

        //! communication hook event sync receiver 
        private void CommunicationHookerSync(MSG_DIRECTION Direction, Byte[] Data, System.String strDescription)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    lvDebugMessage.BeginInvoke(new MessageListener(CommunicationHooker), Direction, Data, strDescription);
                }
                else
                {
                    CommunicationHooker(Direction, Data, strDescription);
                }
            }
            catch (Exception )
            {
            }

            //m_Adapter.
            BeginLog();
            if (Direction == MSG_DIRECTION.INPUT_MSG)
            {
                WriteLog("IN :[");
            }
            else
            {
                WriteLog("OUT:[");
            }
            WriteLog(Data.Length.ToString("D3"));
            WriteLog("]");
            if (null != strDescription)
            {
                WriteLog("    " + strDescription);
            }
            WriteLog("    ");
            WriteLog(HEXBuilder.ByteArrayToHEXString(Data));
            EndLog();

        }

        //* ----------------------------------------------------------------------------- *
        // * Telegraph Event Receiver                                                     *
        // * --------------------------------------------------------------------------- */
        private void TelegraphReportEventHandler(SinglePhaseTelegraph tTelegraph, BM_TELEGRAPH_STATE State, ESCommand ReceivedCommand)
        {
            tTelegraph.SinglePhaseTelegraphEvent -= new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);

            try
            {
                this.txtReadBlock.BeginInvoke(new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandlerAsyn), tTelegraph, State, ReceivedCommand);
            }
            catch (Exception )
            {
            }
        }

        //* ----------------------------------------------------------------------------- *
        // * Telegraph Event Asyn Receiver                                                *
        // * --------------------------------------------------------------------------- */
        private void TelegraphReportEventHandlerAsyn(SinglePhaseTelegraph tTelegraph, BM_TELEGRAPH_STATE State, ESCommand ReceivedCommand)
        {
            switch (State)
            {
                case BM_TELEGRAPH_STATE.BM_TELE_RT_CANCELLED:
                    MessageBox.Show
                    (
                        "Telegraph has been cancelled!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    tTelegraph.Dispose();
                    return;
                //break;

                case BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_DATA_SIZE_TOO_LARGE:
                    MessageBox.Show
                    (
                        "Telegraph data size is too large!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    tTelegraph.Dispose();
                    return;
                //break;

                case BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_ILLEGAL_FRAME:
                    MessageBox.Show
                    (
                        "Illegal telegraph frame!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    tTelegraph.Dispose();
                    return;
                // break;

                case BM_TELEGRAPH_STATE.BM_TELE_RT_TIME_OUT:
                    MessageBox.Show
                    (
                        "Telegraph Listening Timeout.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    tTelegraph.Dispose();
                    return;
                //break;
                case BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_FAILD_TO_WRITE_DEVICE:
                    MessageBox.Show
                    (
                        "Failed to write telegraph to device!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    tTelegraph.Dispose();
                    return;
                case BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS:
                    //MessageBox.Show("Telegraph transmittance is successful!");
                    break;

                case BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR:
                default:
                    MessageBox.Show("Unknow Error happened during encoding/decoding telegraph");
                    tTelegraph.Dispose();
                    return;
                //break;
            }

            if (null == ReceivedCommand)
            {
                MessageBox.Show
                    (
                        "No available response command!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
            }

            switch (ReceivedCommand.Type)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    if (null == ReceivedCommand.Data)
                    {
                        txtReadBlock.Text = "[SYSTEM_INFO]:NO DATA RECEIVED";
                    }
                    else
                    {
                        if (checkReadBlockShowHEX.Checked)
                        {
                            //! show hex
                            txtReadBlock.Text = HEXBuilder.ByteArrayToHEXString(ReceivedCommand.Data);
                        }
                        else
                        {
                            //! show string
                            StringBuilder strbTempResult = new StringBuilder();
                            System.Byte[] ReceiveBuffer = ReceivedCommand.Data;

                            for (Int32 n = 0; n < ReceiveBuffer.Length; n++)
                            {
                                strbTempResult.Append((char)ReceiveBuffer[n]);
                            }

                            txtReadBlock.Text = strbTempResult.ToString();
                        }
                    }
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    txtReadWord.Text = ((ESCommandReadWord)ReceivedCommand).DataValue.ToString("X4");
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    //MessageBox.Show("Telegraph transmittance is successful!","Success",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    break;
            }

            tTelegraph.Dispose();

        }

        //* ----------------------------------------------------------------------------- *
        // * Form Event Receiver                                                          *
        // * --------------------------------------------------------------------------- */

        private void cmdEnableDebug_Click(object sender, EventArgs e)
        {
            if (null != m_Adapter)
            {
                cmdEnableDebug.Checked = !m_Adapter.DebugEnabled;

                m_Adapter.DebugEnabled = cmdEnableDebug.Checked;
                if (m_Adapter.DebugEnabled)
                {
                    lvDebugMessage.Enabled = true;
                    lvDebugMessage.BackColor = System.Drawing.Color.FromArgb(0, 32, 64);
                    timerRefreshDebugMessage.Enabled = true;
                }
                else
                {
                    lvDebugMessage.Enabled = false;
                    lvDebugMessage.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
                    timerRefreshDebugMessage.Enabled = false;
                }
            }
            else
            {
                cmdEnableDebug.Checked = false;
            }
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            lvDebugMessage.Items.Clear();
        }

        private void cmdDoIt_Click(object sender, EventArgs e)
        {
            System.Byte[] cCommand = null;
            //! get command
            txtCommand.Text = txtCommand.Text.Trim().ToUpper();
            if (false == HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cCommand))
            {
                //! illegal command
                MessageBox.Show
                    (
                        "Please enter a legal command byte!",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                return;
            }

            //! get target address
            System.Byte[] cAddress = new Byte[1];
            System.Byte[] cSubAddress = new Byte[1];
            if (-1 == combAddress.SelectedIndex)
            {
                //! text
                combAddress.Text = combAddress.Text.Trim().ToUpper();
                if (false == HEXBuilder.HEXStringToByteArray(combAddress.Text, ref cAddress))
                {
                    //! illegal command
                    MessageBox.Show
                        (
                            "Please enter a legal target address!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    return;
                }
            }
            else
            {
                switch (combAddress.SelectedIndex)
                {
                    case 0:                     //!< Adapter
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
                        break;
                    case 1:                     //!< SMBus
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                        break;
                    case 2:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC;
                        break;
                    case 3:                     //!< UART
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART;
                        break;
                    case 4:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC;
                        break;
                    case 5:                     //!< Single wire UART
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART;
                        break;
                    case 6:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC;
                        break;
                    case 7:                     //!< SPI
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI;
                        break;
                    case 8:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC;
                        break;
                    case 9:                     //!< I2C
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C;
                        break;
                    case 10:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC;
                        break;
                    case 11:                    //!< Loader
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LOADER;
                        break;
                    case 12:                    //!< Charger
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_CHARGER;
                        break;
                    case 13:                    //!< printer
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_PRN;
                        break;
                    case 14:                    //!< LCD
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LCD;
                        break;
                    case 15:                     //!< SMBus Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX;
                        break;
                    case 16:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX;
                        break;
                    case 17:                     //!< UART Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_EX;
                        break;
                    case 18:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX;
                        break;
                    case 19:                     //!< Single wire UART Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX;
                        break;
                    case 20:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX;
                        break;
                    case 21:                     //!< SPI Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX;
                        break;
                    case 22:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX;
                        break;
                    case 23:                     //!< I2C Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX;
                        break;
                    case 24:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX;
                        break;

                    case 25:                    //!< All
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ALL;
                        break;
                }
            }

            do
            {

                if (false == HEXBuilder.HEXStringToByteArray(txtSubAddress.Text, ref cSubAddress))
                {
                    //! illegal command
                    MessageBox.Show
                        (
                            "Please enter a legal optional sub-address!",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    return;
                }
            }
            while (false);


            //! get response type
            System.UInt16 hwTimeOut = 0;
            if (-1 == combResponseType.SelectedIndex)
            {
                if (false == DECBuilder.DECStringToWord(combResponseType.Text, ref hwTimeOut))
                {
                    combResponseType.Text = "300";
                    hwTimeOut = 300;
                }
            }
            else
            {
                switch (combResponseType.SelectedIndex)
                { 
                    case 0:
                        hwTimeOut = (UInt16)BM_CMD_RT.BM_CMD_RT_NO_RESPONSE;
                        break;
                    case 1:
                        hwTimeOut = (UInt16)BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT;
                        break;
                }
                
            }

            //! handle command type
            ESCommand bmcTarget = null;
            switch (combCommandType.SelectedIndex)
            {
                case 1:                 //!< write word
                    System.UInt16[] hwWriteWord = null;
                    if (false == HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwWriteWord))
                    {
                        //! illegal command
                        MessageBox.Show
                            (
                                "Please enter a legal word value!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        return;
                    }
                    bmcTarget = new ESCommandWriteWord(hwWriteWord[0]);
                    break;
                case 2:                 //!< read word
                    bmcTarget = new ESCommandReadWord();
                    break;
                case 3:                 //!< write block
                    System.Byte[] BlockWriteBuffer = null;

                    if (checkWriteBlockShowHEX.Checked)
                    {
                        //! hex string
                        if (false == HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref BlockWriteBuffer, false))
                        {
                            //! illegal command
                            MessageBox.Show
                                (
                                    "Please enter a legal HEX String",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                            return;
                        }

                        txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(BlockWriteBuffer);
                    }
                    else
                    {
                        Char[] CharBuffer = txtWriteBlock.Text.ToCharArray();
                        BlockWriteBuffer = new System.Byte[CharBuffer.Length];
                        //! Just string

                        for (System.Int32 n = 0; n < BlockWriteBuffer.Length; n++)
                        {
                            BlockWriteBuffer[n] = (System.Byte)CharBuffer[n];
                        }
                    }
                    bmcTarget = new ESCommandWriteBlock(BlockWriteBuffer);
                    break;
                case 4:                 //!< read block
                    bmcTarget = new ESCommandReadBlock();
                    break;
                case 0:                 //!< just command
                default:
                    bmcTarget = new ESCommand();
                    break;
            }

            bmcTarget.Command = cCommand[0];
            bmcTarget.AddressValue = cAddress[0];
            bmcTarget.SubAddress = cSubAddress[0];
            bmcTarget.TimeOut = hwTimeOut;
            bmcTarget.Description = txtBrief.Text;


            //! handle telegraph type
            SinglePhaseTelegraph telTarget = m_Adapter.CreateTelegraph(combFrameType.Text, bmcTarget) as SinglePhaseTelegraph;
            if (null == telTarget)
            {
                MessageBox.Show
                            (
                                "Failed to create telegraph!",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                return ;
            }

            //! register a result handler
            telTarget.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);

            m_Adapter.TryToSendTelegraph(telTarget);

        }

        

        private void lvDebugMessage_Resize(object sender, EventArgs e)
        {
            /*
            lvDebugMessage.Columns[0].Width = 40;
            lvDebugMessage.Columns[1].Width = 40;
            lvDebugMessage.Columns[2].Width = (int)((double)lvDebugMessage.Width * 0.56);
            lvDebugMessage.Columns[3].Width = (int)((double)lvDebugMessage.Width * 0.3);
            */
        }

        private void cmdRefreshInformation_Click(object sender, EventArgs e)
        {
            txtAdapterType.Text = m_Adapter.Type;
            txtDeviceType.Text = m_Adapter.DeviceType;
            txtDriverVersion.Text = m_Adapter.DeviceVersion;
            txtDeviceInfo.Text = m_Adapter.DeviceInfo;
            txtAdapterKey.Text = m_Adapter.ID;
            txtSetting.Text = m_Adapter.Settings;
            txtConnectionState.Text = m_Adapter.Open ? "Open" : "Close";

            if (false == m_Adapter.Open)
            {
                grbSMBus.Enabled = false;
                grpDebug.Enabled = false;
            }
            else
            {
                grbSMBus.Enabled = true;
                grpDebug.Enabled = true;
            }

        }

        private void lvHIDDevice_Resize(object sender, EventArgs e)
        {
            lvHIDDevice.Columns[1].Width = (Int32)((double)lvHIDDevice.Width * 0.8);
        }

        private void cmdRefreshDevice_Click(object sender, EventArgs e)
        {
            //! clear all devices
            lvHIDDevice.Items.Clear();

            System.UInt16[] hwProductID = null;
            System.UInt16[] hwVenderID = null;

            if (
                    (false == HEXBuilder.HEXStringToU16Array(txtVID.Text, ref hwVenderID))
                || (false == HEXBuilder.HEXStringToU16Array(txtPID.Text, ref hwProductID))
                )
            {
                //! illegal product id and vender id
                return;
            }

            //! find all devices
            String[] strDeviceList = TelegraphHIDAdapter.FindHIDDevice(hwVenderID[0], hwProductID[0]);

            if (null == strDeviceList)
            { 
                //! no devices found
                cmdRefreshInformation_Click(null, null);
                return;
            }

            for (Int32 n = 0; n < strDeviceList.Length; n++)
            { 
                //! add command
                ListViewItem temItem = new ListViewItem(n.ToString("D2"));
                String tName = null;

                try
                {
                    tName = strDeviceList[n].Substring(28, 8);
                }
                catch (Exception) 
                {
                    tName = "Unknown";
                }

                temItem.SubItems.Add("HID Device          [" + tName.ToUpper() + "]");
                temItem.SubItems[1].Tag = strDeviceList[n];
                if (m_Adapter.Settings == strDeviceList[n])
                {
                    if (m_Adapter.Open)
                    {
                        temItem.SubItems.Add("Open");
                    }
                    else
                    {
                        temItem.SubItems.Add("Close");
                    }
                }
                else
                {
                    temItem.SubItems.Add("Close");
                }

                lvHIDDevice.Items.Add(temItem);
            }
            cmdRefreshInformation_Click(null,null);
        }

        private void txtVID_TextChanged(object sender, EventArgs e)
        {
            System.UInt16[] hwResult = null;
            if (false == HEXBuilder.HEXStringToU16Array(txtVID.Text, ref hwResult))
            { 
                //! illegal input
                txtVID.Text = "";
            }

            if (null != m_Adapter)
            {
                m_Adapter.VID = hwResult[0];
            }
            //txtVID.Text = txtVID.Text.ToUpper();
        }

        private void txtPID_TextChanged(object sender, EventArgs e)
        {
            System.UInt16[] hwResult = null;
            if (false == HEXBuilder.HEXStringToU16Array(txtPID.Text, ref hwResult))
            {
                //! illegal input
                txtPID.Text = "";
            }

            if (null != m_Adapter)
            {
                m_Adapter.PID = hwResult[0];
            }
            //txtPID.Text = txtPID.Text.ToUpper();
        }

        private void lvHIDDevice_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lviSelectedItem = lvHIDDevice.SelectedItems[0];

            if ((lviSelectedItem.SubItems[1].Tag as String) != m_Adapter.Settings)
            {
                //! disconnect current connection first
                m_Adapter.Open = false;

                //! modify previouse device connection display
                if ((null != m_Adapter.Settings) && ("" != m_Adapter.Settings.Trim()))
                {
                    for (System.Int32 n = 0; n < lvHIDDevice.Items.Count; n++)
                    {
                        if ((lvHIDDevice.Items[n].SubItems[1].Tag as String) == m_Adapter.Settings)
                        {
                            lvHIDDevice.Items[n].SubItems[2].Text = "Close";
                            break; 
                        }
                    }
                }
                
                //! change seetingss
                m_Adapter.Settings = lviSelectedItem.SubItems[1].Tag as String;
                m_Adapter.Open = true;                
            }
            else
            {
                m_Adapter.Open = !m_Adapter.Open;

            }
            
            lviSelectedItem.SubItems[2].Text = m_Adapter.Open ? "Open" : "Close";
            cmdRefreshDevice_Click(null, null);
            //cmdRefreshInformation_Click(sender, e);
        }

        private void combAddress_TextUpdate(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(combAddress.Text, ref cResult))
            {
                //! illegal input
                combAddress.Text = "";
            }

            //combAddress.Text = combAddress.Text.ToUpper();
        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cResult))
            {
                //! illegal input
                txtCommand.Text = "";
            }

            //txtCommand.Text = txtCommand.Text.ToUpper();
        }

        private void txtWriteWord_TextChanged(object sender, EventArgs e)
        {
            System.UInt16[] hwResult = null;
            if (false == HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwResult))
            {
                //! illegal input
                txtWriteWord.Text = "";
            }
        }

        private void combResponseType_TextChanged(object sender, EventArgs e)
        {
            System.UInt16 hwResult = 0;
            if (false == DECBuilder.DECStringToWord(combResponseType.Text, ref hwResult))
            {
                //! illegal input
                combResponseType.Text = "";
            }

            if ((combResponseType.Text.StartsWith("-")) || (combResponseType.Text.StartsWith("+")))
            {

                if ((combResponseType.Text.StartsWith("-")))
                {
                    combResponseType.Text = "";
                }
                else
                {
                    combResponseType.MaxLength = 6;
                }
            }
            else
            { 
                combResponseType.MaxLength = 5;
            }
        }

        private void txtWriteBlock_TextChanged(object sender, EventArgs e)
        {
            if (checkWriteBlockShowHEX.Checked)
            {
                System.Byte[] cResult = null;

                //! hex string model
                if (!HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref cResult))
                {
                    MessageBox.Show
                        (
                            "Please Enter a legal HEX string.",
                            "Wizard Warnning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref cResult, false);

                    if (null != cResult)
                    {
                        txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(cResult);
                    }
                    else
                    {
                        txtWriteBlock.Text = "";
                    }
                    //cmdNext.Enabled = false;
                }
                else
                {
                    //cmdNext.Enabled = true;
                }
            }
            else
            {
                //cmdNext.Enabled = true;
            }
        }

        private void checkWriteBlockShowHEX_CheckedChanged(object sender, EventArgs e)
        {
            if (checkWriteBlockShowHEX.Checked)
            {
                //! HEX string
                //! normal string
                System.Byte[] BlockWriteBuffer = null;

                Char[] CharBuffer = txtWriteBlock.Text.ToCharArray();
                BlockWriteBuffer = new System.Byte[CharBuffer.Length];

                for (System.Int32 n = 0; n < BlockWriteBuffer.Length; n++)
                {
                    BlockWriteBuffer[n] = (System.Byte)CharBuffer[n];
                }

                txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(BlockWriteBuffer);

            }
            else
            {
                //! normal string
                System.Byte[] cResult = null;

                //! hex string model
                if (HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref cResult))
                {
                    StringBuilder sbTempString = new StringBuilder();

                    for (System.Int32 n = 0; n < cResult.Length; n++)
                    {
                        sbTempString.Append((char)cResult[n]);

                        txtWriteBlock.Text = sbTempString.ToString();
                    }

                }
            }
        }


        public TabPage DeviceManagerPage
        {
            get { return this.tabDevices; }
        }

        public TabPage DebugPage
        {
            get { return this.tabDebug; }
        }

        public TabPage CommunicationPage
        {
            get { return this.tabCommunication; }
        }

        public TabPage InformationPage
        {
            get { return this.tabAdatperInfo; }
        }

        public Adapter Adatper
        {
            get { return m_Adapter; }
        }




        public Boolean SendCommand(ESCommand tCommand)
        {
            if (null == tCommand)
            {
                return false;
            }

            if (null == m_Adapter)
            {
                return false;
            }

            if (!m_Adapter.Open)
            {
                return false;
            }

            txtCommand.Text = tCommand.Command.ToString("X2");
             
            txtBrief.Text = tCommand.Description;
            txtSubAddress.Text = tCommand.SubAddress.ToString("X2");

            switch (tCommand.Address)
            {
                case BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER:
                    combAddress.SelectedIndex = 0;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                    combAddress.SelectedIndex = 1;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                    combAddress.SelectedIndex = 2;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                    combAddress.SelectedIndex = 3;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                    combAddress.SelectedIndex = 4;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                    combAddress.SelectedIndex = 5;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                    combAddress.SelectedIndex = 6;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                    combAddress.SelectedIndex = 7;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                    combAddress.SelectedIndex = 8;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C:
                    combAddress.SelectedIndex = 9;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                    combAddress.SelectedIndex = 10;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LOADER:
                    combAddress.SelectedIndex = 11;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_CHARGER:
                    combAddress.SelectedIndex = 12;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LCD:
                    combAddress.SelectedIndex = 13;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_PRN:
                    combAddress.SelectedIndex = 14;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                    combAddress.SelectedIndex = 15;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                    combAddress.SelectedIndex = 16;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                    combAddress.SelectedIndex = 17;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                    combAddress.SelectedIndex = 18;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:
                    combAddress.SelectedIndex = 19;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                    combAddress.SelectedIndex = 20;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                    combAddress.SelectedIndex = 21;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                    combAddress.SelectedIndex = 22;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:
                    combAddress.SelectedIndex = 23;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                    combAddress.SelectedIndex = 24;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_ALL:
                    combAddress.SelectedIndex = 25;
                    break;
                default:
                    combAddress.Text = tCommand.AddressValue.ToString("X2");
                    break;
            }

            switch (tCommand.ResponseMode)
            {
                case BM_CMD_RT.BM_CMD_RT_NO_RESPONSE:
                    combResponseType.SelectedIndex = 0;
                    break;
                case BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT:
                    combResponseType.SelectedIndex = 1;
                    break;
                default:
                    combResponseType.Text = tCommand.TimeOut.ToString();
                    break;
            }

            switch (tCommand.Type)
            {                 
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    combCommandType.SelectedIndex = 1;
                    try
                    {
                        txtWriteWord.Text = BitConverter.ToUInt16(tCommand.Data, 0).ToString("X4");
                    }
                    catch (Exception Err)
                    {
                        Err.ToString();
                    }
                    return true;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    combCommandType.SelectedIndex = 2;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    combCommandType.SelectedIndex = 3;
                    checkWriteBlockShowHEX.Checked = true;
                    txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(tCommand.Data);
                    return true;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    combCommandType.SelectedIndex = 4;
                    break;
                default:
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    combCommandType.SelectedIndex = 0;
                    break;
            }

            SinglePhaseTelegraph telTarget = m_Adapter.CreateTelegraph(combFrameType.Text, tCommand) as SinglePhaseTelegraph;
            if (null == telTarget)
            {
                return false;
            }

            telTarget.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);

            return m_Adapter.TryToSendTelegraph(telTarget);

        }

        private void checkReadBlockShowHEX_CheckedChanged(object sender, EventArgs e)
        {
            if (checkReadBlockShowHEX.Checked)
            {
                //! HEX string
                //! normal string
                System.Byte[] BlockReadBuffer = null;

                Char[] CharBuffer = txtReadBlock.Text.ToCharArray();
                BlockReadBuffer = new System.Byte[CharBuffer.Length];

                for (System.Int32 n = 0; n < BlockReadBuffer.Length; n++)
                {
                    BlockReadBuffer[n] = (System.Byte)CharBuffer[n];
                }

                txtReadBlock.Text = HEXBuilder.ByteArrayToHEXString(BlockReadBuffer);

            }
            else
            {
                //! normal string
                System.Byte[] cResult = null;

                //! hex string model
                if (HEXBuilder.HEXStringToByteArray(txtReadBlock.Text, ref cResult))
                {
                    StringBuilder sbTempString = new StringBuilder();

                    for (System.Int32 n = 0; n < cResult.Length; n++)
                    {
                        sbTempString.Append((Char)cResult[n]);

                        txtReadBlock.Text = sbTempString.ToString();
                    }

                }
            }
        }


        private void txtSubAddress_TextChanged(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(txtSubAddress.Text, ref cResult))
            {
                //! illegal input
                txtSubAddress.Text = "00";
            }
        }

        private void timerRefreshDebugMessage_Tick(object sender, EventArgs e)
        {
            if (m_RefreshDownCounter > 0)
            {
                if (m_RefreshDownCounter-- == 1)
                {
                    ListViewItem[] tItems = m_ListViewItems.ToArray();
                    m_ListViewItems.Clear();
                    lvDebugMessage.BeginUpdate();
                    lvDebugMessage.Items.AddRange(tItems);
                    lvDebugMessage.EndUpdate();

                    lvDebugMessage.Items[lvDebugMessage.Items.Count - 1].EnsureVisible();
                }
            }
        }

        private void labWriteWord_Click(object sender, EventArgs e)
        {
            combCommandType.SelectedIndex = 1;

            cmdDoIt_Click(labWriteWord, e);
        }

        private void labReadWord_Click(object sender, EventArgs e)
        {
            combCommandType.SelectedIndex = 2;

            cmdDoIt_Click(labReadWord, e);
        }

        private void labWriteBlock_Click(object sender, EventArgs e)
        {
            combCommandType.SelectedIndex = 3;

            cmdDoIt_Click(labWriteBlock, e);
        }

        private void labReadBlock_Click(object sender, EventArgs e)
        {
            combCommandType.SelectedIndex = 4;

            cmdDoIt_Click(labReadBlock, e);
        }

        private void lvHIDDevice_KeyDown(object sender, KeyEventArgs e)
        {
            m_KeyBackDoor.Append(e.KeyCode);
        }

        private void lvDebugMessage_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView.SelectedListViewItemCollection tItems = lvDebugMessage.SelectedItems;
            StringBuilder sbOutputString = new StringBuilder();
            foreach (ListViewItem tItem in tItems)
            {
                String tTemp = tItem.SubItems[2].Text;
                if (null == tTemp)
                {
                    continue;
                }

                sbOutputString.Append(tTemp.Trim());
                sbOutputString.Append(' ');
            }

            Clipboard.SetText(sbOutputString.ToString());
        }


    }
}
