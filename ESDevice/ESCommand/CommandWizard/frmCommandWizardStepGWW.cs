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
    internal partial class frmCommandWizardStepGWW : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepGWW()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtWriteWord.Enabled = false;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepGWW(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtWriteWord.Enabled = false;
            }
            else
            {
                txtWriteWord.Text = ((ESCommandWriteWord)m_Command).DataValue.ToString("X4");
            }
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            if (null != m_Command)
            {
                System.UInt16[] hwResult = null;
                if (HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwResult))
                {
                    //! legal input
                    ((ESCommandWriteWord)m_Command).DataValue = hwResult[0];
                }
                else
                {
                    MessageBox.Show
                                (
                                    "Please Enter Word in legal HEX string format.",
                                    "Wizard Warnning",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning
                                );
                    return;
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
                System.UInt16[] hwResult = null;
                if (HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwResult))
                {
                    //! legal input
                    ((ESCommandWriteWord)m_Command).DataValue = hwResult[0];
                }
            }

            frmCommandWizardStepF CommandWizard = new frmCommandWizardStepF(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }

        private void txtWriteWord_TextChanged(object sender, EventArgs e)
        {
            System.UInt16[] hwResult = null;
            if (false == HEXBuilder.HEXStringToU16Array(txtWriteWord.Text, ref hwResult))
            {
                //! illegal input
                txtWriteWord.Text = "";
                cmdNext.Enabled = false;
            }
            else
            {
                cmdNext.Enabled = true;
            }
        }

        private void frmCommandWizardStepGWW_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
