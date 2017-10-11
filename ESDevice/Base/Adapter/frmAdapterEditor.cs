using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities.HEX;
using ESnail.Utilities.DEC;
using ESnail.Utilities.Log;
using ESnail.CommunicationSet.Commands;
using ESnail.Device.Telegraphs;
using ESnail.Utilities;

namespace ESnail.Device
{
    public partial class frmAdapterEditor : Form, IAdapterEditorComponent, IBMCommand
    {
        protected Form m_RootParent = null;
        protected Adapter m_Adapter = null;

        //! \brief default constructor
        public frmAdapterEditor()
        {
            Initialize();
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

        //! \brief constructor with rootform
        public frmAdapterEditor(Adapter AdapterItem)
        {
            m_Adapter = AdapterItem;

            Initialize();
        }

        //! \brief initialize this object
        private void Initialize()
        {
            InitializeComponent();

            if (null == m_Adapter)
            {
                //! debug page
                grpDebug.Enabled = false;

                //! device manager page
                panelDeviceManager.Enabled = false;

                //! communication page
                panelCommunication.Enabled = false;

                //! properties page
                panelProperties.Enabled = false;
            }
            else
            {
                //! communication hooker
                m_Adapter.MessageHooker += new MessageListener(CommunicationHookerSync);
                
                cmdEnableDebug.Checked = m_Adapter.DebugEnabled;

                //! communication page
                if (m_Adapter is ITelegraph)
                {
                    InitializeCommunicationPage();
                }
                else
                {
                    tabsAdapterEditor.TabPages.Remove(tabCommunication);
                }
            }

            tabInformation.Disposed += new EventHandler(TabpagesDisposedEventHandler);
            tabDebug.Disposed += new EventHandler(TabpagesDisposedEventHandler);
            tabDeviceManager.Disposed += new EventHandler(TabpagesDisposedEventHandler);
            tabCommunication.Disposed += new EventHandler(TabpagesDisposedEventHandler);

            Refresh();
        }


        private void TabpagesDisposedEventHandler(object sender, EventArgs e)
        {
            tabInformation.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
            tabDebug.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
            tabDeviceManager.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
            tabCommunication.Disposed -= new EventHandler(TabpagesDisposedEventHandler);

            try
            {
                this.Dispose();
            }
            catch (Exception) { }
        }

        private void _Dispose()
        {
            try
            {
                tabInformation.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
                tabDebug.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
                tabDeviceManager.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
                tabCommunication.Disposed -= new EventHandler(TabpagesDisposedEventHandler);
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        private Boolean m_bOnRefreshing = false;

        public override void Refresh()
        {
            base.Refresh();
            if (null == m_Adapter)
            {
                return ;
            }

            if (m_bOnRefreshing)
            {
                return;
            }
            m_bOnRefreshing = true;


            if (m_Adapter.Open)
            {
                //! debug toolbar
                toolbarDebug.Enabled = true;

                //! communication page
                grbCommand.Enabled = true;
            }
            else
            {
                //! debug toolbar
                toolbarDebug.Enabled = false;

                //! communication page
                grbCommand.Enabled = false;
            }

            cmdEnableDebug.Checked = m_Adapter.DebugEnabled;

            m_bOnRefreshing = false;
        }

        private void InitializeCommunicationPage()
        {
            if (null == m_Adapter)
            {
                return;
            }

            comboTelegraph.Items.Clear();
            String[] tSupportedTelegraph = m_Adapter.SupportedTelegraph;
            if (null == tSupportedTelegraph)
            {
                tabsAdapterEditor.TabPages.Remove(tabCommunication);
                return;
            }
            else if (tSupportedTelegraph.Length > 0)
            {
                comboTelegraph.Items.AddRange(tSupportedTelegraph);
                comboTelegraph.SelectedIndex = 0;
            }
            else
            {
                tabsAdapterEditor.TabPages.Remove(tabCommunication);
                return;
            }

            //! initilize command type
            comboCMDType.SelectedIndex = 2;

            //! initialize address
            comboAddress.Items.Add("Adapter");
            comboAddress.Items.Add("SMBus");
            comboAddress.Items.Add("SMBus with PEC");
            comboAddress.Items.Add("UART");
            comboAddress.Items.Add("UART with PEC");
            comboAddress.Items.Add("Single-wire UART");
            comboAddress.Items.Add("Single-wire UART with PEC");
            comboAddress.Items.Add("SPI");
            comboAddress.Items.Add("SPI with PEC");
            comboAddress.Items.Add("I2C");
            comboAddress.Items.Add("I2C with PEC");
            comboAddress.Items.Add("Loader");
            comboAddress.Items.Add("Charger");
            comboAddress.Items.Add("Printer");
            comboAddress.Items.Add("LCD");
            comboAddress.Items.Add("Extend SMBus");
            comboAddress.Items.Add("Extend SMBus with PEC");
            comboAddress.Items.Add("Extend UART");
            comboAddress.Items.Add("Extend UART with PEC");
            comboAddress.Items.Add("Extend Single-wire UART");
            comboAddress.Items.Add("Extend Single-wire UART with PEC");
            comboAddress.Items.Add("Extend SPI");
            comboAddress.Items.Add("Extend SPI with PEC");
            comboAddress.Items.Add("Extend I2C");
            comboAddress.Items.Add("Extend I2C with PEC");
            comboAddress.Items.Add("All");
            comboAddress.SelectedIndex = 1;

            
            //! initialize timeout
            comboTimeout.Items.Add("No Response");
            comboTimeout.Items.Add("Wait forever");
            comboTimeout.Text = "300";

            
        }

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

        #region Communication Hooker
        //! \brief method for showing debug information
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
                    MessageListener Hooker = new MessageListener(this.CommunicationHooker);
                    lvDebugMessage.BeginInvoke(Hooker, Direction, Data, strDescription);
                }
                else
                {
                    CommunicationHooker(Direction, Data, strDescription);
                }
            }
            catch (System.InvalidOperationException e)
            {
                WriteLogLine(e.ToString());
            }
            finally
            {
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
        }

        #endregion

        #region tool tips
        private void txtCommand_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtCommand_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtCommand.Control, "Please Enter a command byte here \nin hexadecimal string");
            toolTip.Active = true;
        }

        private void comboAddress_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(comboAddress.Control, "Please Enter a address byte here \nin hexadecimal string or select a destination.");
            toolTip.Active = true;
        }

        private void comboAddress_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void comboTimeout_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(comboTimeout.Control, "Please Enter a timeout value here (ms) \n or select a timeout setting.");
            toolTip.Active = true;
        }

        private void comboTimeout_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtBrief_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtBrief, "Please Enter a brief description about this command.");
            toolTip.Active = true;
        }

        private void txtBrief_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void comboCMDType_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(comboCMDType.Control, "Please select a command type here.");
            toolTip.Active = true;
        }

        private void comboCMDType_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void comboTelegraph_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(comboTelegraph.Control, "Please select a telegraph type here.");
            toolTip.Active = true;
        }

        private void comboTelegraph_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtWriteWord_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtWriteWord.Control, "Please enter a word(2bytes) here \nin hexadecimal string.");
            toolTip.Active = true;
        }

        private void txtWriteWord_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtWriteBlock_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtWriteBlock, "Please enter literal string or hexadecimal string here \nwhich will be send to target device.");
            toolTip.Active = true;
        }

        private void txtWriteBlock_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void chkWriteHex_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(chkWriteHex, "Select this check box will only receive input content writen in hexadecimal \nstring,otherwise the input content will be considered as literal string\n(invisible symbles will be replaced by dots).");
            toolTip.Active = true;
        }

        private void chkWriteHex_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtReadBlock_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtReadBlock, "You can read received literal string or hexadecimal string here \nwhich will be send to target device.");
            toolTip.Active = true;
        }

        private void txtReadBlock_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void chkHexDisplay_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(chkHexDisplay, "Select this check box will force all received content shown in\n hexadecimal string, otherwise the content will be shown as \nliteral string(invisible symbles will be replaced by dots).");
            toolTip.Active = true;
        }

        private void chkHexDisplay_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtReadWord_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtReadWord.Control, "You can read received word(2bytes) \nshown as hexadecimal string here.");
            toolTip.Active = true;
        }

        private void txtReadWord_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void cmdDoIt_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(cmdDoIt, "Send command to target device.");
            toolTip.Active = true;
        }

        private void cmdDoIt_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        private void txtSubAddress_MouseEnter(object sender, EventArgs e)
        {
            toolTip.SetToolTip(txtSubAddress.Control, "Please Enter an optional sub-address here \nin hexadecimal string");
            toolTip.Active = true;
        }

        private void txtSubAddress_MouseLeave(object sender, EventArgs e)
        {
            toolTip.Active = false;
        }

        #endregion

        #region command type selection
        private void comboCMDType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCommandType((BM_CMD_TYPE)comboCMDType.SelectedIndex);
        }

        

        private void ChangeCommandType(BM_CMD_TYPE dwCommandType)
        {/*
            switch (dwCommandType)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    
                    //! Just command
                    chkWriteHex.Visible = false;
                    chkHexDisplay.Visible = false;
                    labWriteWord.Visible = false;              //!< write word
                    txtWriteWord.Visible = false;
                    labReadWord.Visible = false;              //!< read word
                    txtReadWord.Visible = false;
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    //! write word
                    chkWriteHex.Visible = false;
                    chkHexDisplay.Visible = false;
                    labWriteWord.Visible = true;              //!< write word
                    txtWriteWord.Visible = true;
                    labReadWord.Visible = false;              //!< read word
                    txtReadWord.Visible = false;
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    //! read word
                    chkWriteHex.Visible = false;
                    chkHexDisplay.Visible = false;
                    labWriteWord.Visible = false;             //!< write word
                    txtWriteWord.Visible = false;
                    labReadWord.Visible = true;               //!< read word
                    txtReadWord.Visible = true;
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    //! write block
                    chkWriteHex.Visible = true;
                    chkHexDisplay.Visible = false;
                    labWriteWord.Visible = false;             //!< write word
                    txtWriteWord.Visible = false;
                    labReadWord.Visible = false;              //!< read word
                    txtReadWord.Visible = false;
                    labWriteBlock.Visible = true;              //! write block
                    txtWriteBlock.Visible = true;
                    labReadBlock.Visible = false;              //! read block
                    txtReadBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    //! read block
                    chkWriteHex.Visible = false;
                    chkHexDisplay.Visible = true;
                    labWriteWord.Visible = false;             //!< write word
                    txtWriteWord.Visible = false;
                    labReadWord.Visible = false;              //!< read word
                    txtReadWord.Visible = false;
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    labReadBlock.Visible = true;               //! read block
                    txtReadBlock.Visible = true;
                    break;
                default:
                    break;
            }
          * */
            chkWriteHex.Visible = true;
            chkHexDisplay.Visible = true;
            labWriteWord.Visible = true;             //!< write word
            txtWriteWord.Visible = true;
            labReadWord.Visible = true;              //!< read word
            txtReadWord.Visible = true;
            labWriteBlock.Visible = true;             //! write block
            txtWriteBlock.Visible = true;
            labReadBlock.Visible = true;               //! read block
            txtReadBlock.Visible = true;
        }
        #endregion

        #region debug page
        private void cmdEnableDebug_Click(object sender, EventArgs e)
        {
            if (null == m_Adapter)
            {
                return;
            }

            m_Adapter.DebugEnabled = !m_Adapter.DebugEnabled;
            cmdEnableDebug.Checked = m_Adapter.DebugEnabled;

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

        private void cmdClear_Click(object sender, EventArgs e)
        {
            lvDebugMessage.Items.Clear();
        }
        #endregion

        #region data validation check
        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            if ("" == txtCommand.Text)
            {
                return;
            }

            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cResult))
            {
                ShowErrorMessage(txtCommand.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                txtCommand.Text = "";
            }
        }

        private void comboAddress_TextUpdate(object sender, EventArgs e)
        {
            if ("" == comboAddress.Text)
            {
                return;
            }

            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(comboAddress.Text, ref cResult))
            {
                //! illegal input
                ShowErrorMessage(comboAddress.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                comboAddress.Text = "";
            }
        }

        private void ShowErrorMessage(Control tControl, String tMessage)
        {
            toolTipError.Active = false;
            toolTipError.ToolTipIcon = ToolTipIcon.Warning;
            toolTipError.Show("", tControl);
            toolTipError.Active = true;
            toolTipError.Show(tMessage, tControl, 5000);
        }

        private void txtWriteWord_TextChanged(object sender, EventArgs e)
        {
            if ("" == txtWriteWord.Text)
            {
                return;
            }

            System.UInt16[] hwResult = null;
            if (false == HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwResult))
            {
                //! illegal input
                ShowErrorMessage(txtWriteWord.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                txtWriteWord.Text = "";
            }
        }

        private void txtWriteBlock_TextChanged(object sender, EventArgs e)
        {
            if ("" == txtWriteBlock.Text)
            {
                return;
            }

            if (chkWriteHex.Checked)
            {                
                System.Byte[] cResult = null;

                //! hex string model
                if (!HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref cResult))
                {
                    ShowErrorMessage(txtWriteBlock, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");

                    HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref cResult, false);

                    if (null != cResult)
                    {
                        txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(cResult);
                    }
                    else
                    {
                        txtWriteBlock.Text = "";
                    }
                }
            }
        }

        private void chkWriteHex_CheckedChanged(object sender, EventArgs e)
        {

            if (chkWriteHex.Checked)
            {
                //! HEX string
                //! normal string
                System.Byte[] BlockWriteBuffer = null;

                Char[] CharBuffer = txtWriteBlock.Text.ToCharArray();
                BlockWriteBuffer = new System.Byte[CharBuffer.Length];

                for (System.Int32 n = 0; n < BlockWriteBuffer.Length; n++)
                {
                    BlockWriteBuffer[n] = (Byte)CharBuffer[n];
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
                        sbTempString.Append((Char)cResult[n]);
                         txtWriteBlock.Text = sbTempString.ToString();
                    }

                }
            }
        }

        private void chkHexDisplay_CheckedChanged(object sender, EventArgs e)
        {

            if (chkHexDisplay.Checked)
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

        private void comboTimeout_TextUpdate(object sender, EventArgs e)
        {
            if ("" == comboTimeout.Text)
            {
                return;
            }

            System.UInt16 hwResult = 0;
            if (false == DECBuilder.DECStringToWord(comboTimeout.Text, ref hwResult))
            {
                //! illegal input
                ShowErrorMessage(comboTimeout.Control, "A legal positive integer decimal string could only \nconsist of symbles 0~9 and sign symble \'+\'");
                comboTimeout.Text = "";
            }

            if ((comboTimeout.Text.StartsWith("-")) || (comboTimeout.Text.StartsWith("+")))
            {

                if ((comboTimeout.Text.StartsWith("-")))
                {
                    ShowErrorMessage(comboTimeout.Control, "A legal positive integer decimal string could only \nconsist of symbles 0~9 and sign symble \'+\'");
                    comboTimeout.Text = "";
                }
                else
                {
                    comboTimeout.MaxLength = 6;
                    //toolTipError.Hide(comboTimeout.Control);
                }
            }
            else
            {
                comboTimeout.MaxLength = 5;
                //toolTipError.Hide(comboTimeout.Control);
            }
        }

        #endregion

        private void cmdDoIt_Click(object sender, EventArgs e)
        {
            if (null == m_Adapter)
            {
                return;
            }
            ITelegraph tTelegraphAdapter = m_Adapter as ITelegraph;
            if (null == tTelegraphAdapter)
            {
                return;
            }

            Byte[] cCommand = null;
            txtCommand.Text = txtCommand.Text.Trim().ToUpper();
            if (false == HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cCommand))
            {
                //! illegal command
                ShowErrorMessage(txtCommand.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                return;
            }

            //! get target address
            System.Byte[] cAddress = null ;
            System.Byte[] cSubAddress = null;
            if (-1 == comboAddress.SelectedIndex)
            {
                //! text
                comboAddress.Text = comboAddress.Text.Trim();
                if (false == HEXBuilder.HEXStringToByteArray(comboAddress.Text, ref cAddress))
                {
                    //! illegal command
                    ShowErrorMessage(comboAddress.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                    return;
                }
            }
            else
            {
                cAddress = new Byte[1];
                switch (comboAddress.SelectedIndex)
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

                    case 15:                    //!< SMBus Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX;
                        break;
                    case 16:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX;
                        break;
                    case 17:                    //!< UART Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_EX;
                        break;
                    case 18:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX;
                        break;
                    case 19:                    //!< Single wire UART Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX;
                        break;
                    case 20:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX;
                        break;
                    case 21:                    //!< SPI Extend
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX;
                        break;
                    case 22:
                        cAddress[0] = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX;
                        break;
                    case 23:                    //!< I2C Extend
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
                    ShowErrorMessage(comboAddress.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                    return;
                }
            }
            while (false);

            
            //! get response type
            System.UInt16 hwTimeOut = 0;
            if (-1 == comboTimeout.SelectedIndex)
            {
                if (false == DECBuilder.DECStringToWord(comboTimeout.Text, ref hwTimeOut))
                {
                    comboTimeout.Text = "";
                    ShowErrorMessage(comboTimeout.Control, "A legal positive integer decimal string could only \nconsist of symbles 0~9 and sign symble \'+\'");
                    
                    return;
                }
            }
            else
            {
                switch (comboTimeout.SelectedIndex)
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
            switch (comboCMDType.SelectedIndex)
            {
                case 1:                 //!< write word
                    System.UInt16[] hwWriteWord = null;
                    if (false == HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwWriteWord))
                    {
                        //! illegal command
                        ShowErrorMessage(txtWriteWord.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                        return;
                    }
                    bmcTarget = new ESCommandWriteWord(hwWriteWord[0]);
                    break;
                case 2:                 //!< read word
                    bmcTarget = new ESCommandReadWord();
                    break;
                case 3:                 //!< write block
                    System.Byte[] BlockWriteBuffer = null;

                    if (chkWriteHex.Checked)
                    {
                        //! hex string
                        if (false == HEXBuilder.HEXStringToByteArray(txtWriteBlock.Text, ref BlockWriteBuffer, false))
                        {
                            //! illegal command
                            ShowErrorMessage(txtWriteBlock, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
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

            SinglePhaseTelegraph telTarget = m_Adapter.CreateTelegraph(comboTelegraph.Text, bmcTarget) as SinglePhaseTelegraph;
            if (null == telTarget)
            {
                MessageBox.Show
                    (
                        "Failed to create a telegraph, please contact the vender of " + comboTelegraph.Text + " for more details.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                return;
            }

            //! register a result handler
            //telTarget.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(telTarget_SinglePhaseTelegraphEvent);
            telTarget.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);
            if (!tTelegraphAdapter.TryToSendTelegraph(telTarget))
            {
                telTarget.SinglePhaseTelegraphEvent -= new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);
                MessageBox.Show
                    (
                        "Failed to send telegraph, please check the connection with the target device.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                telTarget.Dispose();

                return;
            }            
        }

        //* ----------------------------------------------------------------------------- *
        // * Telegraph Event Receiver                                                     *
        // * --------------------------------------------------------------------------- */
        private void TelegraphReportEventHandler(SinglePhaseTelegraph tTelegraph, BM_TELEGRAPH_STATE State, ESCommand ReceivedCommand)
        {
            tTelegraph.SinglePhaseTelegraphEvent -= new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);

            try
            {
                this.txtReadBlock.BeginInvoke(new SinglePhaseTelegraphEventHandler(SinglePhaseTelegraphEventHandlerAysn), tTelegraph, State, ReceivedCommand);
            }
            catch (Exception)
            {
            }
        }

        private void SinglePhaseTelegraphEventHandlerAysn(SinglePhaseTelegraph tTelegraph, BM_TELEGRAPH_STATE State, ESCommand ReceivedCommand)
        {
            if (null == tTelegraph)
            {
                return;
            }

            //tTelegraph.SinglePhaseTelegraphEvent -= new SinglePhaseTelegraphEventHandler(SinglePhaseTelegraphEventHandlerAysn);

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
                        "Telegraph data size too large!",
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
                        "Illegal telegraph frame format!",
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
                        "Telegraph Listenning Timeout.",
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

                        if (chkHexDisplay.Checked)
                        {
                            //! show hex
                            txtReadBlock.Text = HEXBuilder.ByteArrayToHEXString(ReceivedCommand.Data);
                        }
                        else
                        {
                            //! show string
                            StringBuilder strbTempResult = new StringBuilder();
                            System.Byte[] ReceiveBuffer = ReceivedCommand.Data;

                            for (System.Int32 n = 0; n < ReceiveBuffer.Length; n++)
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
                    break;
            }

            tTelegraph.Dispose();
        }


        #region IAdapterEditorComponent Members

        public TabPage DeviceManagerPage
        {
            get { return tabDeviceManager; }
        }

        public TabPage DebugPage
        {
            get { return tabDebug; }
        }

        public TabPage CommunicationPage
        {
            get { return tabCommunication; }
        }

        public TabPage InformationPage
        {
            get { return tabInformation; }
        }

        public Adapter Adatper
        {
            get { return m_Adapter; }
        }

        #endregion

        private void txtSubAddress_TextChanged(object sender, EventArgs e)
        {
            if ("" == txtSubAddress.Text)
            {
                return;
            }

            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(txtSubAddress.Text, ref cResult))
            {
                ShowErrorMessage(txtSubAddress.Control, "A legal hexadecimal string could only \nconsist of symbles 0~9, a~f and A~F");
                txtSubAddress.Text = "00";
            }
        }


        public Boolean SendCommand(ESCommand tCommand)
        {
            if (null == m_Adapter)
            {
                return false;
            }
            ITelegraph tTelegraphAdapter = m_Adapter as ITelegraph;
            if (null == tTelegraphAdapter)
            {
                return false;
            }

            if (null == tCommand)
            {
                return false;
            }

            txtCommand.Text = tCommand.Command.ToString("X2");
            txtBrief.Text = tCommand.Description;
            txtSubAddress.Text = tCommand.SubAddress.ToString("X2");
            switch (tCommand.Address)
            {
                case BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER:
                    comboAddress.SelectedIndex = 0;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                    comboAddress.SelectedIndex = 1;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                    comboAddress.SelectedIndex = 2;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                    comboAddress.SelectedIndex = 3;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                    comboAddress.SelectedIndex = 4;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                    comboAddress.SelectedIndex = 5;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                    comboAddress.SelectedIndex = 6;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                    comboAddress.SelectedIndex = 7;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                    comboAddress.SelectedIndex = 8;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C:
                    comboAddress.SelectedIndex = 9;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                    comboAddress.SelectedIndex = 10;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LOADER:
                    comboAddress.SelectedIndex = 11;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_CHARGER:
                    comboAddress.SelectedIndex = 12;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_LCD:
                    comboAddress.SelectedIndex = 13;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_PRN:
                    comboAddress.SelectedIndex = 14;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                    comboAddress.SelectedIndex = 15;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                    comboAddress.SelectedIndex = 16;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                    comboAddress.SelectedIndex = 17;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                    comboAddress.SelectedIndex = 18;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:
                    comboAddress.SelectedIndex = 19;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                    comboAddress.SelectedIndex = 20;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                    comboAddress.SelectedIndex = 21;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                    comboAddress.SelectedIndex = 22;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:
                    comboAddress.SelectedIndex = 23;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                    comboAddress.SelectedIndex = 24;
                    break;
                case BM_CMD_ADDR.BM_CMD_ADDR_ALL:
                    comboAddress.SelectedIndex = 25;
                    break;
                default:
                    comboAddress.Text = tCommand.AddressValue.ToString("X2");
                    break;
            }

            switch (tCommand.Type)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    comboCMDType.SelectedIndex = 0;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    comboCMDType.SelectedIndex = 2;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    txtWriteWord.Text = HEXBuilder.ByteArrayToHEXString(tCommand.Data);
                    comboCMDType.SelectedIndex = 1;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    chkHexDisplay.Checked = true;
                    comboCMDType.SelectedIndex = 4;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    chkWriteHex.Checked = true;
                    txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(tCommand.Data);
                    comboCMDType.SelectedIndex = 3;
                    break;
            }

            switch (tCommand.ResponseMode)
            { 
                case BM_CMD_RT.BM_CMD_RT_NO_RESPONSE:
                    comboTimeout.SelectedIndex = 0;
                    break;
                case BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT:
                    comboTimeout.SelectedIndex = 1;
                    break;
                default:
                    comboTimeout.Text = tCommand.TimeOut.ToString();
                    break;
            }


            SinglePhaseTelegraph telTarget = m_Adapter.CreateTelegraph(comboTelegraph.Text, tCommand) as SinglePhaseTelegraph;
            if (null == telTarget)
            {
                MessageBox.Show
                    (
                        "Failed to create a telegraph, please contact the vender of " + comboTelegraph.Text + " for more details.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                return false;
            }

            //! register a result handler
            //telTarget.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(telTarget_SinglePhaseTelegraphEvent);
            telTarget.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);
            if (!tTelegraphAdapter.TryToSendTelegraph(telTarget))
            {
                telTarget.SinglePhaseTelegraphEvent -= new SinglePhaseTelegraphEventHandler(TelegraphReportEventHandler);
                MessageBox.Show
                    (
                        "Failed to send telegraph, please check the connection with the target device.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                telTarget.Dispose();

                return false;
            }     


            return true;
        }

        /*
         * switch (tCommand.Type)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    comboCMDType.SelectedIndex = 0;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    comboCMDType.SelectedIndex = 2;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    txtWriteWord.Text = HEXBuilder.ByteArrayToHEXString(tCommand.Data);
                    comboCMDType.SelectedIndex = 1;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    chkHexDisplay.Checked = true;
                    comboCMDType.SelectedIndex = 4;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    chkWriteHex.Checked = true;
                    txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(tCommand.Data);
                    comboCMDType.SelectedIndex = 3;
                    break;
            }
         */

        private void labWriteBlock_Click(object sender, EventArgs e)
        {
            comboCMDType.SelectedIndex = 3;
            cmdDoIt_Click(sender, e);
        }

        private void labWriteWord_Click(object sender, EventArgs e)
        {
            comboCMDType.SelectedIndex = 1;
            cmdDoIt_Click(sender, e);
        }

        private void labReadWord_Click(object sender, EventArgs e)
        {
            comboCMDType.SelectedIndex = 2;
            cmdDoIt_Click(sender, e);
        }

        private void labReadBlock_Click(object sender, EventArgs e)
        {
            comboCMDType.SelectedIndex = 4;
            cmdDoIt_Click(sender, e);
        }

        private void labCommand_Click(object sender, EventArgs e)
        {
            comboCMDType.SelectedIndex = 0;
            cmdDoIt_Click(sender, e);
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
