using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities.HEX;
using ESnail.Utilities.DEC;

namespace ESnail.CommunicationSet.Commands
{

    //! \name command wizard report state
    //! @{
    public enum BM_CMD_EDIT_RESULT
    {
        BM_CMD_EDIT_CANCELLED,        //!< the wizard for creating a new command was cancelled
        BM_CMD_EDIT_FINISH            //!< normal finish
    }
    //! @}

    public delegate void CommandEdit(BM_CMD_EDIT_RESULT Resule, ESCommand Command);

    public partial class frmCommandEditor : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandEditor()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                grbCMDEditor.Enabled = false;
            }
        }

        private void FormInitialize(ESCommand Command)
        {
            //! initialize command type
            ChangeCommandType(Command.Type);

            //! commmand type
            switch (Command.Type)
            { 
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    combCommandType.SelectedIndex = 0;                    
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    combCommandType.SelectedIndex = 1;
                    //! command parameter
                    txtWriteWord.Text = ((ESCommandWriteWord)Command).DataValue.ToString("X4");
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    combCommandType.SelectedIndex = 2;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    combCommandType.SelectedIndex = 3;
                    //! command parameter
                    if (checkWriteBlockShowHEX.Checked)
                    {
                        txtWriteBlock.Text = HEXBuilder.ByteArrayToHEXString(Command.Data);
                    }
                    else
                    {                        
                        System.Byte[] chTempBuffer = Command.Data;

                        if (null != chTempBuffer)
                        {
                            //! normal string
                            StringBuilder sbTempString = new StringBuilder();

                            for (System.Int32 n = 0; n < chTempBuffer.Length; n++)
                            {
                                if ((chTempBuffer[n] >= 0x20) && (chTempBuffer[n] <= 0x7F))
                                {
                                    sbTempString.Append((char)chTempBuffer[n]);
                                }
                                else
                                {
                                    sbTempString.Append(".");
                                }

                                txtWriteBlock.Text = sbTempString.ToString();
                            }
                        }
                    }
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    combCommandType.SelectedIndex = 4;
                    break;            
            }

            //! command ID
            txtCommandID.Text  = Command.ID;

            //! command 
            txtCommand.Text = Command.Command.ToString("X2");

            //! brief
            textBrief.Text = Command.Description;

            //! timeout
            switch (Command.ResponseMode)
            { 
                case BM_CMD_RT.BM_CMD_RT_NO_RESPONSE:
                    combResponseType.SelectedIndex = 0;
                    break;
                case BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT:
                    combResponseType.SelectedIndex = 1;
                    break;
                default:
                    combResponseType.Text = Command.TimeOut.ToString("D");
                    break;
            }

            //! address
            switch (m_Command.Address)
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
                    combAddress.Text = m_Command.AddressValue.ToString("X2");
                    break;
            }

            //! optional address
            txtSubAddress.Text = m_Command.SubAddress.ToString("X2");

        }

        //! \brief event for notice outside the apply button is pressed
        public event CommandEdit CommandEditEvent;



        //! \brief constructor
        public frmCommandEditor(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                grbCMDEditor.Enabled = false;
            }
            else
            {
                FormInitialize(Command);
            }

            combCommandType.Enabled = false;
            txtCommandID.Enabled = false;
        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cResult))
            {
                //! illegal input
                txtCommand.Text = "";
            }

        }

        private void combAddress_TextUpdate(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(combAddress.Text, ref cResult))
            {
                //! illegal input
                combAddress.Text = "";
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

        private void txtWriteWord_TextChanged(object sender, EventArgs e)
        {
            System.UInt16[] hwResult = null;
            if (false == HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwResult))
            {
                //! illegal input
                txtWriteWord.Text = "";
            }
        }

        private void ChangeCommandType(BM_CMD_TYPE dwCommandType)
        {
            switch (dwCommandType)
            {
                case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    //! Just command
                    checkWriteBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = false;          //!< write word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE:
                    //! write word
                    checkWriteBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = true;           //!< write word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                    //! read word
                    checkWriteBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = false;          //!< write word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE:
                    //! write block
                    checkWriteBlockShowHEX.Visible = true;
                    toolbarWriteWord.Visible = false;          //!< write word
                    labWriteBlock.Visible = true;              //! write block
                    txtWriteBlock.Visible = true;
                    break;
                case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    //! read block
                    checkWriteBlockShowHEX.Visible = false;
                    toolbarWriteWord.Visible = false;          //!< write word
                    labWriteBlock.Visible = false;             //! write block
                    txtWriteBlock.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void combCommandType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCommandType((BM_CMD_TYPE)combCommandType.SelectedIndex);
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
                    if (txtWriteBlock.Text != "")
                    {
                        txtWriteBlock.Text = txtWriteBlock.Text.Substring(0, txtWriteBlock.Text.Length - 1);
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

        private void cmdApply_Click(object sender, EventArgs e)
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

            //! get sub address
            do
            {

                if (false == HEXBuilder.HEXStringToByteArray(txtSubAddress.Text, ref cSubAddress))
                {
                    //! illegal command
                    MessageBox.Show
                        (
                            "Please enter a legal optinal sub address!",
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

                    ((ESCommandWriteWord)m_Command).DataValue = hwWriteWord[0];
                    break;
                case 2:                 //!< read word
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
                    m_Command.Data = BlockWriteBuffer;
                    break;
                case 4:                 //!< read block
                    break;
                case 0:                 //!< just command
                    break;
                default:
                    break;
            }
            
            m_Command.Command = cCommand[0];
            m_Command.AddressValue = cAddress[0];
            m_Command.SubAddress = cSubAddress[0];
            m_Command.TimeOut = hwTimeOut;
            m_Command.Description = textBrief.Text;

            if (null != CommandEditEvent)
            {
                //! raising event
                CommandEditEvent(BM_CMD_EDIT_RESULT.BM_CMD_EDIT_FINISH, m_Command);
            }

            this.Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            //! raising event
            CommandEditEvent(BM_CMD_EDIT_RESULT.BM_CMD_EDIT_CANCELLED, m_Command);
            Dispose();
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

        private void frmCommandEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != CommandEditEvent)
            {
                //! raising event
                CommandEditEvent(BM_CMD_EDIT_RESULT.BM_CMD_EDIT_CANCELLED, m_Command);
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
