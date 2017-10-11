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
    internal partial class frmCommandWizardStepGWB : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepGWB()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtCommandBrief.Enabled = false;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepGWB(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtCommandBrief.Enabled = false;
            }
            else
            {
                txtCommandBrief.Text = HEXBuilder.ByteArrayToHEXString(m_Command.Data);
            }
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {

            if (null != m_Command)
            {
                if (checkWriteBlockShowHEX.Checked)
                {
                    //! hex string
                    System.Byte[] cResult = null;

                    //! hex string model
                    if (HEXBuilder.HEXStringToByteArray(txtCommandBrief.Text, ref cResult))
                    {
                        m_Command.Data = cResult;
                    }
                    else
                    {
                        MessageBox.Show
                        (
                            "Please Enter a legal HEX string.",
                            "Wizard Warnning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        return;
                    }
                }
                else
                {
                    //! normal string
                    System.Byte[] BlockWriteBuffer = null;
                    if (null != txtCommandBrief.Text)
                    {
                        Char[] CharBuffer = txtCommandBrief.Text.ToCharArray();
                        BlockWriteBuffer = new System.Byte[CharBuffer.Length];

                        for (System.Int32 n = 0; n < BlockWriteBuffer.Length; n++)
                        {
                            BlockWriteBuffer[n] = (System.Byte)CharBuffer[n];
                        }

                        m_Command.Data = BlockWriteBuffer;
                    }
                    else
                    {
                        m_Command.Data = new Byte[0];
                    }
                }
            }

            frmCommandWizardStepH CommandWizard = new frmCommandWizardStepH(m_Command);

            this.Hide();
            CommandWizard.Show();
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
            if (null != m_Command)
            {
                if (checkWriteBlockShowHEX.Checked)
                {
                    //! hex string
                    System.Byte[] cResult = null;

                    //! hex string model
                    if (HEXBuilder.HEXStringToByteArray(txtCommandBrief.Text, ref cResult))
                    {
                        m_Command.Data = cResult;
                    }
                }
                else
                {
                    //! normal string
                    System.Byte[] BlockWriteBuffer = null;

                    if (null != txtCommandBrief.Text)
                    {

                        Char[] CharBuffer = txtCommandBrief.Text.ToCharArray();
                        BlockWriteBuffer = new System.Byte[CharBuffer.Length];

                        for (System.Int32 n = 0; n < BlockWriteBuffer.Length; n++)
                        {
                            BlockWriteBuffer[n] = (System.Byte)CharBuffer[n];
                        }

                        m_Command.Data = BlockWriteBuffer;
                    }
                    else
                    {
                        m_Command.Data = new Byte[0];
                    }
                }
            }

            frmCommandWizardStepF CommandWizard = new frmCommandWizardStepF(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }

        private void txtCommandBrief_TextChanged(object sender, EventArgs e)
        {
            if (checkWriteBlockShowHEX.Checked)
            {
                System.Byte[] cResult = null;

                if (null != txtCommandBrief.Text)
                {
                    //! hex string model
                    if (!HEXBuilder.HEXStringToByteArray(txtCommandBrief.Text, ref cResult))
                    {
                        MessageBox.Show
                            (
                                "Please Enter a legal HEX string.",
                                "Wizard Warnning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        txtCommandBrief.Text = txtCommandBrief.Text.Substring(0, txtCommandBrief.Text.Length - 1);
                        //cmdNext.Enabled = false;
                    }
                    else
                    {
                        cmdNext.Enabled = true;
                    }
                }
            }
            else
            {
                cmdNext.Enabled = true;
            }
        }

        private void checkWriteBlockShowHEX_CheckedChanged(object sender, EventArgs e)
        {
            if (checkWriteBlockShowHEX.Checked)
            {
                //! HEX string
                //! normal string
                System.Byte[] BlockWriteBuffer = null;

                Char[] CharBuffer = txtCommandBrief.Text.ToCharArray();
                BlockWriteBuffer = new System.Byte[CharBuffer.Length];

                for (System.Int32 n = 0; n < BlockWriteBuffer.Length; n++)
                {
                    BlockWriteBuffer[n] = (System.Byte)CharBuffer[n];
                }

                txtCommandBrief.Text = HEXBuilder.ByteArrayToHEXString(BlockWriteBuffer);

            }
            else
            { 
                //! normal string
                System.Byte[] cResult = null;

                //! hex string model
                if (HEXBuilder.HEXStringToByteArray(txtCommandBrief.Text, ref cResult))
                {
                    StringBuilder sbTempString = new StringBuilder();

                    for (System.Int32 n = 0; n < cResult.Length; n++)
                    {
                        sbTempString.Append((Char)cResult[n]);

                        txtCommandBrief.Text = sbTempString.ToString();
                    }

                }
            }
        }

        private void frmCommandWizardStepGWB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
