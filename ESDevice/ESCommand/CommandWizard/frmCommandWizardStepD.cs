using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.CommunicationSet.Commands;

namespace ESnail.CommunicationSet.Commands
{
    internal partial class frmCommandWizardStepD : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepD()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                txtCommandBrief.Enabled = false;
                cmdNext.Enabled = false;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepD(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                txtCommandBrief.Enabled = false;
                cmdNext.Enabled = false;
            }
            else
            {
                txtCommandBrief.Text = m_Command.Description;
            }
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {

            if (null != m_Command)
            {
                if ("" != txtCommandBrief.Text.Trim())
                {
                    m_Command.Description = txtCommandBrief.Text;
                }
                else
                {
                    m_Command.Description = m_Command.ID;
                }

            }

            frmCommandWizardStepE CommandWizard = new frmCommandWizardStepE(m_Command);

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
                if ("" != txtCommandBrief.Text.Trim())
                {
                    m_Command.Description = txtCommandBrief.Text;
                }
                else
                {
                    m_Command.Description = m_Command.ID;
                }

            }

            frmCommandWizardStepC CommandWizard = new frmCommandWizardStepC(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }

        private void frmCommandWizardStepD_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
