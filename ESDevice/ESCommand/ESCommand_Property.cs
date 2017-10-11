using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities;

namespace ESnail.CommunicationSet.Commands
{

    public delegate void CommandRemoved(ESCommand Command);
    public delegate void CommandWizardReport(BM_CMD_WIZARD_RESULT Result,ESCommand Command);

    //! \name command wizard report state
    //! @{
    public enum BM_CMD_WIZARD_RESULT
    { 
        BM_CMD_WIZARD_CANCELLED,        //!< the wizard for creating a new command was cancelled
        BM_CMD_WIZARD_FINISH            //!< normal finish
    }
    //! @}

    //! define properties
    public partial class ESCommand
    {
        //! event for get command wizard report
        public event CommandWizardReport CommandWizardReportEvent;

        //! \brief public method for raising WizardReport event
        public void OnWizardReport(BM_CMD_WIZARD_RESULT Result)
        {
            if (null != CommandWizardReportEvent)
            {
                CommandWizardReportEvent.Invoke(Result, this);
            }
        }

        public CommandWizardReport CommandWizardReportEventHandler
        {
            get { return CommandWizardReportEvent; }
            set { CommandWizardReportEvent = value; }
        }

        //! event for note owner that this command will be removed.
        public event CommandRemoved CommandRemovedEvent;

        //! \brief method for raising remove event
        public void OnRemove()
        {
            if (null != CommandRemovedEvent)
            {
                CommandRemovedEvent(this);
            }

            this.Dispose();
        }

        public CommandRemoved CommandRemovedEventHandler
        {
            get { return CommandRemovedEvent; }
            set { CommandRemovedEvent = value; }
        }

        //! \brief method for show a command wizard to initialize a specified command
        public void ShowWizard(ESCommand Command)
        {
            frmCommandWizardStepA CommandWizard = new frmCommandWizardStepA(Command);
            CommandWizard.Show();
        }

        //! \brief method for show a command wizard to initialize this command
        public void ShowWizard()
        {
            ShowWizard(this);
        }


        //! property ID
        public SafeID ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        //! property Command
        public virtual Byte Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        //! property Command Type Readonly
        public BM_CMD_TYPE Type
        {
            get { return m_CommandType; }
        }

        //! property Command Address
        public BM_CMD_ADDR Address
        {
            get { return m_Address; }
            set { m_Address = value; }
        }

        //! property Command AddressValue
        public Byte AddressValue
        {
            get { return (System.Byte)m_Address; }
            set { m_Address = (BM_CMD_ADDR)value; }
        }

        //! \brief sub address
        public Byte SubAddress
        {
            get { return m_SubAddress; }
            set { m_SubAddress = value; }
        }

        //! property Response Type
        public BM_CMD_RT ResponseMode
        {
            get { return m_ResponseType; }
            set { m_ResponseType = value; }
        }

        //! property TimeOut
        public UInt16 TimeOut
        {
            get { return (System.UInt16)m_ResponseType; }
            set { m_ResponseType = (BM_CMD_RT)value; }
        }

        //! property description
        public String Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        //! property data
        virtual public Byte[] Data
        {
            get { return null; }
            set { }
        }

        //! \brief property for get Command editor form
        public Form CommandEditor
        {
            get 
            {
                if (null == m_Form)
                {
                    m_Form = new frmCommandEditor(this);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                }

                return m_Form;
            }
        }

        void FormDisposedEventHandler(object sender, EventArgs e)
        {
            m_Form = null;
        }

        //! \brief property for get Command editor page
        public TabPage CommandEditorPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmCommandEditor(this);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                }

                return m_Form.tabCommandEditor;
            }
        }

        //! \brief property for get Command editor panel
        public Panel CommandEditorPanel
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmCommandEditor(this);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                }

                return m_Form.panelCommandEditor;
            }
        }

        //! \brief method for register a command edit appy event
        public Boolean RegisterCommandEditEvent(CommandEdit CommandEditEventHandler)
        {
            if (null == CommandEditEventHandler)
            {
                return false;
            }

            if (null == m_Form)
            {
                m_Form = new frmCommandEditor(this);
                m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
            }

            m_Form.CommandEditEvent += CommandEditEventHandler;

            return true;
        }

        //! \brief method for register a command edit appy event
        public Boolean UnregisterCommandEditEvent(CommandEdit CommandEditEventHandler)
        {
            if (null == CommandEditEventHandler)
            {
                return false;
            }

            if (null != m_Form)
            {
                m_Form.CommandEditEvent -= CommandEditEventHandler;
            }

            return true;
        }

        //! \brief property for cancel transmitter
        public Boolean IsPureListener
        {
            get { return m_CancelTransimtter; }
            set { m_CancelTransimtter = value; }
        }

        //! \brief public method for hide command editor form
        public void HideEditor()
        {
            if (null != m_Form)
            {
                m_Form.Hide();
                m_Form.Dispose();
                m_Form = null;
            }
        }

    }
}