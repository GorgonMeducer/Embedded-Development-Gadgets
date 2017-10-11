using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities;
using ESnail.Device;


namespace ESnail.Device.Adapters.SerialPort
{
    public partial class ESnailSerialPortAgent : AdapterAgent
    {
        private SafeID m_AdapterID = null;
        private frmTelegraphCOMAdapterEditor m_Form = null;
        //private TelegraphHIDAdapter m_Adatper = null;

        //! default constructor
        public ESnailSerialPortAgent()
            :base()
        {
            Initialize();
        }


        //! constructor with adapter and parent
        public ESnailSerialPortAgent(TelegraphCOMAdapter AdapterItem)
            : base(AdapterItem)
        {
            Initialize();
        }

        //! \brief initialize this object
        private void Initialize()
        {
            InitializeComponent();

            if (null == m_Adapter)
            {
                m_Adapter = new TelegraphCOMAdapter(m_AdapterID);
            }
            else
            {
                m_AdapterID = m_Adapter.ID;
            }

            //this.Visible = false;
        }

        
        //! propery for get Adapter
        public override Adapter Adatper
        {
            get { return m_Adapter; }
        }

        public TelegraphCOMAdapter Adapter
        {
            get { return m_Adapter as TelegraphCOMAdapter; }
        }

        //! propery for get test form
        public override Form Editor
        {
            get 
            {               
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphCOMAdapterEditor(m_Adapter as TelegraphCOMAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                }

                return m_Form;
            }
        }

        private void FormDisposedEventHandler(object sender, EventArgs e)
        {
            m_Form.Disposed -= new EventHandler(FormDisposedEventHandler);
            m_Form.DebugPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form.CommunicationPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form.InformationPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form.DeviceManagerPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form = null;
        }

        //! propery for get DeviceManagerTabPage
        public override TabPage DeviceManagerPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphCOMAdapterEditor(m_Adapter as TelegraphCOMAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.DeviceManagerPage.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.DeviceManagerPage;
            }
        }

        private void TablePageDisposedEventHandler(object sender, EventArgs e)
        {
            if (null != m_Form)
            {
                m_Form.Disposed -= new EventHandler(FormDisposedEventHandler);
                m_Form.DebugPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
                m_Form.CommunicationPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
                m_Form.InformationPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);
                m_Form.DeviceManagerPage.Disposed -= new EventHandler(TablePageDisposedEventHandler);

                m_Form.Dispose();
                m_Form = null;
            }
        }

        //! propery for get DebugTabPage
        public override TabPage DebugPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphCOMAdapterEditor(m_Adapter as TelegraphCOMAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.DebugPage.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.DebugPage;
            }
        }

        //! propery for get DebugTabPage
        public override TabPage CommunicationPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphCOMAdapterEditor(m_Adapter as TelegraphCOMAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.CommunicationPage.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.CommunicationPage;
            }
        }

        //! propery for get DebugTabPage
        public override TabPage InformationPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphCOMAdapterEditor(m_Adapter as TelegraphCOMAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.InformationPage.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.InformationPage;
            }
        }
    }
}
