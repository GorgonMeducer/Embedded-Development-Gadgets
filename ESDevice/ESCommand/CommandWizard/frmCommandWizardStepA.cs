using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.CommunicationSet.Commands;

namespace ESnail.CommunicationSet.Commands
{
    public partial class frmCommandWizardStepA : Form
    {
        private ESCommand m_Command = null;

        //! \brief default constructor
        public frmCommandWizardStepA()
        {
            InitializeComponent();

            if (null == m_Command)
            {
                m_Command = new ESCommand();
                m_Command.ID = null;
                m_Command.Address = BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                m_Command.TimeOut = 300;
                combCommandType.SelectedIndex = 0;
            }
        }

        //! \brief constructor
        public frmCommandWizardStepA(ESCommand Command)
        {
            InitializeComponent();

            m_Command = Command;

            if (null == m_Command)
            {
                m_Command = new ESCommand();
                m_Command.ID = null;
                m_Command.Address = BM_CMD_ADDR.BM_CMD_ADDR_SMBUS;
                m_Command.TimeOut = 300;
                combCommandType.SelectedIndex = 0;
            }
            else
            {
                combCommandType.SelectedIndex = (Int32)m_Command.Type;
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
        
        private void cmdNext_Click(object sender, EventArgs e)
        {
            ESCommand tempCommand = null;

            switch (combCommandType.SelectedIndex)
            { 
                case 0:             //!< Just command
                    if (m_Command.Type != BM_CMD_TYPE.BM_CMD_TYPE_NO_PARAMETER)
                    {
                        tempCommand = new ESCommand();
                        tempCommand.Address = m_Command.Address;
                        tempCommand.Command = m_Command.Command;
                        tempCommand.ID = m_Command.ID;
                        tempCommand.Description = m_Command.Description;
                        tempCommand.TimeOut = m_Command.TimeOut;
                        tempCommand.CommandWizardReportEventHandler = m_Command.CommandWizardReportEventHandler;
                        tempCommand.CommandRemovedEventHandler = m_Command.CommandRemovedEventHandler;
                        tempCommand.IsPureListener = m_Command.IsPureListener;
                        m_Command = tempCommand;

                    }
                    break;
                case 1:             //!< Write word
                    if (m_Command.Type != BM_CMD_TYPE.BM_CMD_TYPE_WORD_WRITE)
                    {
                        tempCommand = new ESCommandWriteWord();
                        tempCommand.Address = m_Command.Address;
                        tempCommand.Command = m_Command.Command;
                        tempCommand.Data = m_Command.Data;
                        tempCommand.ID = m_Command.ID;
                        tempCommand.Description = m_Command.Description;
                        tempCommand.TimeOut = m_Command.TimeOut;
                        tempCommand.CommandWizardReportEventHandler = m_Command.CommandWizardReportEventHandler;
                        tempCommand.CommandRemovedEventHandler = m_Command.CommandRemovedEventHandler;
                        tempCommand.IsPureListener = m_Command.IsPureListener;
                        m_Command = tempCommand;
                    }
                    break;
                case 2:             //!< Read word
                    if (m_Command.Type != BM_CMD_TYPE.BM_CMD_TYPE_WORD_READ)
                    {
                        tempCommand = new ESCommandReadWord(m_Command.Data);
                        tempCommand.Address = m_Command.Address;
                        tempCommand.Command = m_Command.Command;
                        tempCommand.ID = m_Command.ID;
                        tempCommand.Description = m_Command.Description;
                        tempCommand.TimeOut = m_Command.TimeOut;
                        tempCommand.CommandWizardReportEventHandler = m_Command.CommandWizardReportEventHandler;
                        tempCommand.CommandRemovedEventHandler = m_Command.CommandRemovedEventHandler;
                        tempCommand.IsPureListener = m_Command.IsPureListener;
                        m_Command = tempCommand;
                    }
                    break;
                case 3:             //!< Write block
                    if (m_Command.Type != BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_WRITE)
                    {
                        tempCommand = new ESCommandWriteBlock();
                        tempCommand.Address = m_Command.Address;
                        tempCommand.Command = m_Command.Command;
                        tempCommand.Data = m_Command.Data;
                        tempCommand.ID = m_Command.ID;
                        tempCommand.Description = m_Command.Description;
                        tempCommand.TimeOut = m_Command.TimeOut;
                        tempCommand.CommandWizardReportEventHandler = m_Command.CommandWizardReportEventHandler;
                        tempCommand.CommandRemovedEventHandler = m_Command.CommandRemovedEventHandler;
                        tempCommand.IsPureListener = m_Command.IsPureListener;
                        m_Command = tempCommand;
                    }
                    break;
                case 4:             //!< Read Block
                    if (m_Command.Type != BM_CMD_TYPE.BM_CMD_TYPE_BLOCK_READ)
                    {
                        tempCommand = new ESCommandReadBlock(m_Command.Data);
                        tempCommand.Address = m_Command.Address;
                        tempCommand.Command = m_Command.Command;
                        tempCommand.ID = m_Command.ID;
                        tempCommand.Description = m_Command.Description;
                        tempCommand.TimeOut = m_Command.TimeOut;
                        tempCommand.CommandWizardReportEventHandler = m_Command.CommandWizardReportEventHandler;
                        tempCommand.CommandRemovedEventHandler = m_Command.CommandRemovedEventHandler;
                        tempCommand.IsPureListener = m_Command.IsPureListener;
                        m_Command = tempCommand;
                    }
                    break;
            }


            frmCommandWizardStepB CommandWizard = new frmCommandWizardStepB(m_Command);

            this.Hide();
            CommandWizard.Show();
            this.Dispose();
        }

        private void frmCommandWizardStepA_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != m_Command)
            {
                m_Command.OnWizardReport(BM_CMD_WIZARD_RESULT.BM_CMD_WIZARD_CANCELLED);
            }
        }
    }
}
