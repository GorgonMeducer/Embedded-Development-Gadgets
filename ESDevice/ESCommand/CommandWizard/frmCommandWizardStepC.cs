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
    internal partial class frmCommandWizardStepC : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepC()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtCommand.Enabled = false;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepC(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtCommand.Enabled = false;
            }
            else
            {
                txtCommand.Text = m_Command.Command.ToString("X2");
            }
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {

            if (null != m_Command)
            {
                System.Byte[] cResult = null;
                if (HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cResult))
                {
                    //! legal input
                    m_Command.Command = cResult[0];
                }
                else
                {
                    MessageBox.Show
                                (
                                    "Please Enter a legal command byte in HEX string format.", 
                                    "Wizard Warnning", 
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Warning
                                );
                    return;
                }
            }

            frmCommandWizardStepD CommandWizard = new frmCommandWizardStepD(m_Command);

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
                System.Byte[] cResult = null;
                if (HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cResult))
                {
                    //! legal input
                    m_Command.Command = cResult[0];
                }
            }

            frmCommandWizardStepB CommandWizard = new frmCommandWizardStepB(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }

        private void txtCommand_TextChanged(object sender, EventArgs e)
        {
            System.Byte[] cResult = null;
            if (false == HEXBuilder.HEXStringToByteArray(txtCommand.Text, ref cResult))
            {
                //! illegal input
                txtCommand.Text = "";
                cmdNext.Enabled = false;
            }
            else
            {
                cmdNext.Enabled = true;
            }
        }

        private void frmCommandWizardStepC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
