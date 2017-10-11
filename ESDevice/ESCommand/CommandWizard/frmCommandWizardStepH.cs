using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.CommunicationSet.Commands;
using ESnail.Utilities.HEX;

namespace ESnail.CommunicationSet.Commands
{
    internal partial class frmCommandWizardStepH : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepH()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdFinish.Enabled = false;
                txtCommandDetail.Text = "";
            }
        }

        //! \brief default constructor
        public frmCommandWizardStepH(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdFinish.Enabled = false;
                txtCommandDetail.Text = "";
            }
            else
            {
                StringBuilder sbCommandDetail = new StringBuilder();

                //! ID
                sbCommandDetail.Append("[Command ID]\r\n");
                sbCommandDetail.Append("    = " + m_Command.ID+"\r\n\r\n");

                //! Command Type
                sbCommandDetail.Append("[Command Type]\r\n");
                switch (m_Command.Type)
                { 
                    case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                        sbCommandDetail.Append("    = NO_PARAMETER\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                        sbCommandDetail.Append("    = READ_BLOCK\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                        sbCommandDetail.Append("    = WRITE_BLOCK\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                        sbCommandDetail.Append("    = READ_WORD\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                        sbCommandDetail.Append("    = WRITE_WORD\r\n\r\n");
                        break;
                }

                //! Command
                sbCommandDetail.Append("[Command]\r\n");
                sbCommandDetail.Append("    = " + m_Command.Command.ToString("X2") + "\r\n\r\n");

                //! Brief
                sbCommandDetail.Append("[Command Brief Information]\r\n");
                sbCommandDetail.Append("    = " + m_Command.Description + "\r\n\r\n");

                //! Response type
                sbCommandDetail.Append("[Timeout Mode]\r\n");
                switch (m_Command.ResponseMode)
                { 
                    case BM_CMD_RT.BM_CMD_RT_NO_RESPONSE:
                        sbCommandDetail.Append("    = NO_RESPONSE\r\n\r\n");
                        break;
                    case BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT:
                        sbCommandDetail.Append("    = NO_TIMEOUT\r\n\r\n");
                        break;
                    default:
                        sbCommandDetail.Append("    = TIME OUT SETTING:");
                        sbCommandDetail.Append(m_Command.TimeOut);
                        sbCommandDetail.Append("\r\n\r\n");
                        break;
                }

                //! Address
                sbCommandDetail.Append("[Destination Address]\r\n");
                sbCommandDetail.Append("    = ");            
                switch (m_Command.Address)
                { 
                    case BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER:
                        sbCommandDetail.Append("Adapter\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_ALL:
                        sbCommandDetail.Append("Broadcast\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_CHARGER:
                        sbCommandDetail.Append("Charger\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_LCD:
                        sbCommandDetail.Append("Display ESnail.Device\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_LOADER:
                        sbCommandDetail.Append("Electronic Loader\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_PC:
                        sbCommandDetail.Append("PC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_PRN:
                        sbCommandDetail.Append("Printer\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                        sbCommandDetail.Append("Single-wire UART\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                        sbCommandDetail.Append("Single-wire UART with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                        sbCommandDetail.Append("SMBus\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                        sbCommandDetail.Append("SMBus with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                        sbCommandDetail.Append("SPI\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                        sbCommandDetail.Append("SPI with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                        sbCommandDetail.Append("UART\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                        sbCommandDetail.Append("UART with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C:
                        sbCommandDetail.Append("I2C\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                        sbCommandDetail.Append("I2C with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:
                        sbCommandDetail.Append("Extend I2C\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                        sbCommandDetail.Append("Extend I2C with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:
                        sbCommandDetail.Append("Extend Single-wire UART\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                        sbCommandDetail.Append("Extend Single-wire UART with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                        sbCommandDetail.Append("Extend SMBus\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                        sbCommandDetail.Append("Extend SMBus with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                        sbCommandDetail.Append("Extend SPI\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                        sbCommandDetail.Append("Extend SPI with PEC\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                        sbCommandDetail.Append("Extend UART\r\n\r\n");
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                        sbCommandDetail.Append("Extend UART with PEC\r\n\r\n");
                        break;
                    default:
                        sbCommandDetail.Append("[");
                        sbCommandDetail.Append(m_Command.AddressValue.ToString("X2"));
                        sbCommandDetail.Append("]\r\n\r\n");    
                        break;
                }

                //! optional sub address
                sbCommandDetail.Append("[Optional Sub-Address]\r\n");
                sbCommandDetail.Append("    = [");
                sbCommandDetail.Append(m_Command.SubAddress.ToString("X2"));
                sbCommandDetail.Append("]\r\n\r\n");

                //! Parameter
                sbCommandDetail.Append("[Parameter]\r\n");
                switch (m_Command.Type)
                {
                    case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                        sbCommandDetail.Append("    = NO_PARAMETER\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                        sbCommandDetail.Append("    = NO_INITIAL_PARAMETER\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                        sbCommandDetail.Append("    = ");
                        sbCommandDetail.Append(((ESCommandWriteWord)m_Command).DataValue.ToString("X4"));
                        sbCommandDetail.Append("\r\n\r\n");
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                        sbCommandDetail.Append("    DATA LENGHT :");
                        sbCommandDetail.Append(m_Command.Data.Length.ToString());
                        sbCommandDetail.Append("\r\n");

                        System.Byte[] chDataBuffer = m_Command.Data;
                        System.Int32 nCounter = 0;

                        while (nCounter < chDataBuffer.Length)
                        {
                            sbCommandDetail.Append("    ");
                            for (System.Int32 n = 0; n < 8; n++)
                            {
                                if (n + nCounter >= chDataBuffer.Length)
                                {
                                    break;
                                }

                                sbCommandDetail.Append(chDataBuffer[n + nCounter].ToString("X2"));
                                sbCommandDetail.Append(" ");
                                
                            }

                            sbCommandDetail.Append("         ");

                            for (System.Int32 n = 0; n < 8; n++)
                            {
                                if (n + nCounter >= chDataBuffer.Length)
                                {
                                    break;
                                }

                                if ((chDataBuffer[n + nCounter] >= 0x20) && (chDataBuffer[n + nCounter] <= 0x7F))
                                {
                                    sbCommandDetail.Append((char)chDataBuffer[n + nCounter]);
                                }
                                else
                                {
                                    sbCommandDetail.Append('.');
                                }

                                
                            }
                            sbCommandDetail.Append("\r\n");

                            nCounter += 8;
                        }
                        break;
                    
                }

                txtCommandDetail.Text = sbCommandDetail.ToString();
            }
        }


        private void cmdNext_Click(object sender, EventArgs e)
        {
            this.Hide();
            //! wizard complete
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_FINISH);
            }

            this.Dispose(); 
             
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
            
            Dispose();
        }

        private void cmdPrevious_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (null != m_Command)
            { 

                switch (m_Command.Type)
                {
                    case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                        {
                            frmCommandWizardStepF CommandWizard = new frmCommandWizardStepF(m_Command);
                            CommandWizard.Show();
                        }
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                        {
                            frmCommandWizardStepGWB CommandWizard = new frmCommandWizardStepGWB(m_Command);
                            CommandWizard.Show();
                        }
                        break;
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                        {
                            frmCommandWizardStepGWW CommandWizard = new frmCommandWizardStepGWW(m_Command);
                            CommandWizard.Show();
                        }
                        break;
                }
            }
            else 
            {
                frmCommandWizardStepA CommandWizard = new frmCommandWizardStepA(m_Command);
                CommandWizard.Show();
            }
            this.Dispose();
        }

        private void frmCommandWizardStepH_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
