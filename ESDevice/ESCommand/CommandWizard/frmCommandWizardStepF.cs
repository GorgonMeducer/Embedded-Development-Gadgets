using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.CommunicationSet.Commands;
using ESnail.Utilities.DEC;

namespace ESnail.CommunicationSet.Commands
{
    internal partial class frmCommandWizardStepF : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepF()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                
                rbNoResponse.Enabled = false;
                rbNoTimeout.Enabled = false;
                rbTimeout.Enabled = false;
                txtTimeout.Enabled = false;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepF(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                rbNoResponse.Enabled = false;
                rbNoTimeout.Enabled = false;
                rbTimeout.Enabled = false;
                txtTimeout.Enabled = false;
            }
            else
            {
                
                switch (m_Command.ResponseMode)
                { 
                    case BM_CMD_RT.BM_CMD_RT_NO_RESPONSE:
                        rbNoResponse.Checked = true;
                        break;
                    case BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT:
                        rbNoTimeout.Checked = true;
                        break;
                    default:
                        rbTimeout.Checked = true;
                        txtTimeout.Text = m_Command.TimeOut.ToString();
                        txtTimeout.Visible = true;
                        break;
                }
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
                if (rbNoResponse.Checked)
                {
                    m_Command.ResponseMode = BM_CMD_RT.BM_CMD_RT_NO_RESPONSE;
                }
                else if (rbNoTimeout.Checked)
                {
                    m_Command.ResponseMode = BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT;
                }
                else
                {
                    System.UInt16 hwResult = 0;
                    if (DECBuilder.DECStringToWord(txtTimeout.Text, ref hwResult))
                    {
                        m_Command.TimeOut = hwResult;
                    }
                }

            }


            frmCommandWizardStepE CommandWizard = new frmCommandWizardStepE(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }
        
        private void cmdNext_Click(object sender, EventArgs e)
        {

            if (null != m_Command)
            {
                if (rbNoResponse.Checked)
                {
                    m_Command.ResponseMode = BM_CMD_RT.BM_CMD_RT_NO_RESPONSE;
                }
                else if (rbNoTimeout.Checked)
                {
                    m_Command.ResponseMode = BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT;
                }
                else
                {
                    System.UInt16 hwResult = 0;
                    if (DECBuilder.DECStringToWord(txtTimeout.Text, ref hwResult))
                    {
                        m_Command.TimeOut = hwResult;
                    }
                    else
                    {
                        MessageBox.Show
                            (
                                "Please Enter a legal timeout period or select a response mode.",
                                "Wizard Warnning",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        return;
                    }
                }
                
                this.Hide();
                switch (m_Command.Type)
                {
                    case BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER:
                    case BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ:
                    case BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ:
                        {
                            frmCommandWizardStepH CommandWizard = new frmCommandWizardStepH(m_Command);
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
                this.Dispose();

            }

            
            
            
        }


        private void combTimeoutMode_TextUpdate(object sender, EventArgs e)
        {
            
        }

        private void combTimeoutMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdNext.Enabled = true;
        }

        private void txtTimeout_TextChanged(object sender, EventArgs e)
        {
            System.UInt16 hwResult = 0;
            if (false == DECBuilder.DECStringToWord(txtTimeout.Text, ref hwResult))
            {
                //! illegal input
                txtTimeout.Text = "";
                cmdNext.Enabled = false;
            }
            else
            {
                cmdNext.Enabled = true;
            }

            if ((txtTimeout.Text.StartsWith("-")) || (txtTimeout.Text.StartsWith("+")))
            {

                if ((txtTimeout.Text.StartsWith("-")))
                {
                    txtTimeout.Text = "";
                    cmdNext.Enabled = false;
                }
                else
                {
                    txtTimeout.MaxLength = 6;
                }
            }
            else
            {
                txtTimeout.MaxLength = 5;
            }
        }

        private void rbNoResponse_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoResponse.Checked)
            {
                txtTimeout.Visible = false;
            }
        }

        private void rbNoTimeout_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNoTimeout.Checked)
            {
                txtTimeout.Visible = false;
            }
        }

        private void rbTimeout_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTimeout.Checked)
            {
                txtTimeout.Visible = true;
            }
        }

        private void frmCommandWizardStepF_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
