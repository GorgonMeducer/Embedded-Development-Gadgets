using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.CommunicationSet.Commands;

namespace ESnail.CommunicationSet.Commands
{
    internal partial class frmCommandWizardStepB : Form
    {
        private ESCommand m_Command = null;

        //! \brief constructor
        public frmCommandWizardStepB()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtCommandID.Enabled = false;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepB(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                cmdNext.Enabled = false;
                txtCommandID.Enabled = false;
            }
            else
            {
                txtCommandID.Text = m_Command.ID;
            }
        }

        
        private void cmdNext_Click(object sender, EventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.ID = txtCommandID.Text;
            }

            frmCommandWizardStepC CommandWizard = new frmCommandWizardStepC(m_Command);

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
                m_Command.ID = txtCommandID.Text;
            }

            frmCommandWizardStepA CommandWizard = new frmCommandWizardStepA(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }

        private void frmCommandWizardStepB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
