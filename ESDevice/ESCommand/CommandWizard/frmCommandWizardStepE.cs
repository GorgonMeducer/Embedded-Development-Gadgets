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
    internal partial class frmCommandWizardStepE : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepE()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                combCommandAddress.Enabled = false;
            }

        }

        //! \brief constructor
        public frmCommandWizardStepE(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                combCommandAddress.Enabled = false;
            }
            else
            {
                switch (m_Command.Address)
                {
                    case BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER:
                        combCommandAddress.SelectedIndex = 0;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS:
                        combCommandAddress.SelectedIndex = 1;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC:
                        combCommandAddress.SelectedIndex = 2;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART:
                        combCommandAddress.SelectedIndex = 3;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC:
                        combCommandAddress.SelectedIndex = 4;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART:
                        combCommandAddress.SelectedIndex = 5;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC:
                        combCommandAddress.SelectedIndex = 6;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI:
                        combCommandAddress.SelectedIndex = 7;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC:
                        combCommandAddress.SelectedIndex = 8;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C:
                        combCommandAddress.SelectedIndex = 9;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC:
                        combCommandAddress.SelectedIndex = 10;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_LOADER:
                        combCommandAddress.SelectedIndex = 11;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_CHARGER:
                        combCommandAddress.SelectedIndex = 12;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_LCD:
                        combCommandAddress.SelectedIndex = 13;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_PRN:
                        combCommandAddress.SelectedIndex = 14;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX:
                        combCommandAddress.SelectedIndex = 15;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX:
                        combCommandAddress.SelectedIndex = 16;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART_EX:
                        combCommandAddress.SelectedIndex = 17;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX:
                        combCommandAddress.SelectedIndex = 18;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX:
                        combCommandAddress.SelectedIndex = 19;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX:
                        combCommandAddress.SelectedIndex = 20;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX:
                        combCommandAddress.SelectedIndex = 21;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX:
                        combCommandAddress.SelectedIndex = 22;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX:
                        combCommandAddress.SelectedIndex = 23;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX:
                        combCommandAddress.SelectedIndex = 24;
                        break;
                    case BM_CMD_ADDR.BM_CMD_ADDR_ALL:
                        combCommandAddress.SelectedIndex = 25;
                        break;
                    default:
                        combCommandAddress.Text = m_Command.AddressValue.ToString("X2");
                        break;
                }

                txtSubAddress.Text = m_Command.SubAddress.ToString("X2");
            }


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
            if (null != m_Command)
            {
                if (-1 == combCommandAddress.SelectedIndex)
                {
                    System.Byte[] cResult = null;
                    if (HEXBuilder.HEXStringToByteArray(combCommandAddress.Text, ref cResult))
                    {
                        m_Command.AddressValue = cResult[0];
                    }
                }
                else
                {
                    switch (combCommandAddress.SelectedIndex)
                    {
                        case 0:                     //!< Adapter
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
                            break;
                        case 1:                     //!< SMBus
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                            break;
                        case 2:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC;
                            break;
                        case 3:                     //!< UART
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART;
                            break;
                        case 4:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC;
                            break;
                        case 5:                     //!< Single wire UART
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART;
                            break;
                        case 6:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC;
                            break;
                        case 7:                     //!< SPI
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI;
                            break;
                        case 8:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC;
                            break;
                        case 9:                     //!< I2C
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C;
                            break;
                        case 10:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC;
                            break;
                        case 11:                    //!< Loader
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LOADER;
                            break;
                        case 12:                    //!< Charger
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_CHARGER;
                            break;
                        case 13:                    //!< printer
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_PRN;
                            break;
                        case 14:                    //!< LCD
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LCD;
                            break;
                        case 15:                     //!< SMBus Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX;
                            break;
                        case 16:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX;
                            break;
                        case 17:                     //!< UART Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_EX;
                            break;
                        case 18:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX;
                            break;
                        case 19:                     //!< Single wire UART Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX;
                            break;
                        case 20:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX;
                            break;
                        case 21:                     //!< SPI Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX;
                            break;
                        case 22:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX;
                            break;
                        case 23:                     //!< I2C Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX;
                            break;
                        case 24:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX;
                            break;

                        case 25:                    //!< All
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ALL;
                            break;
                    }
                }
                
            }


            frmCommandWizardStepD CommandWizard = new frmCommandWizardStepD(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }
        
        private void cmdNext_Click(object sender, EventArgs e)
        {

            if (null != m_Command)
            {
                if (-1 == combCommandAddress.SelectedIndex)
                {
                    System.Byte[] cResult = null;
                    if (HEXBuilder.HEXStringToByteArray(combCommandAddress.Text, ref cResult))
                    {
                        m_Command.AddressValue = cResult[0];
                    }
                }
                else
                {
                    switch (combCommandAddress.SelectedIndex)
                    {
                        case 0:                     //!< Adapter
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ADAPTER;
                            break;
                        case 1:                     //!< SMBus
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                            break;
                        case 2:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC;
                            break;
                        case 3:                     //!< UART
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART;
                            break;
                        case 4:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC;
                            break;
                        case 5:                     //!< Single wire UART
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART;
                            break;
                        case 6:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC;
                            break;
                        case 7:                     //!< SPI
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI;
                            break;
                        case 8:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC;
                            break;
                        case 9:                     //!< I2C
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C;
                            break;
                        case 10:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC;
                            break;
                        case 11:                    //!< Loader
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LOADER;
                            break;
                        case 12:                    //!< Charger
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_CHARGER;
                            break;
                        case 13:                    //!< printer
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_PRN;
                            break;
                        case 14:                    //!< LCD
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_LCD;
                            break;
                        case 15:                     //!< SMBus Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_EX;
                            break;
                        case 16:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SMBUS_PEC_EX;
                            break;
                        case 17:                     //!< UART Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_EX;
                            break;
                        case 18:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_UART_PEC_EX;
                            break;
                        case 19:                     //!< Single wire UART Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_EX;
                            break;
                        case 20:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SINGLE_WIRE_UART_PEC_EX;
                            break;
                        case 21:                     //!< SPI Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_EX;
                            break;
                        case 22:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_SPI_PEC_EX;
                            break;
                        case 23:                     //!< I2C Extend
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_EX;
                            break;
                        case 24:
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_I2C_PEC_EX;
                            break;

                        case 25:                    //!< All
                            m_Command.AddressValue = (Byte)BM_CMD_ADDR.BM_CMD_ADDR_ALL;
                            break;
                    }
                }

                do
                {
                    
                    Byte[] cResult = null;
                    if (HEXBuilder.HEXStringToByteArray(txtSubAddress.Text, ref cResult))
                    {
                        m_Command.SubAddress = cResult[0];
                    }
                    else
                    {
                        m_Command.SubAddress = 0;
                    }
                }
                while (false);

            }

            frmCommandWizardStepF CommandWizard = new frmCommandWizardStepF(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
            
        }

        private void combCommandAddress_TextUpdate(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(combCommandAddress.Text, ref cResult))
            {
                //! illegal input
                combCommandAddress.Text = "";
                cmdNext.Enabled = false;
            }
            else
            {
                cmdNext.Enabled = true;
            }
        }

        private void combCommandAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdNext.Enabled = true;
        }

        private void frmCommandWizardStepE_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
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
    }
}
