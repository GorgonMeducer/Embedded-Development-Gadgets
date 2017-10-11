using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities;
using ESnail.Device;

namespace ESnail.Device.Adapters.USB.HID
{
    public partial class ESnailHIDAgent : AdapterAgent
    {
        private SafeID m_AdapterID = null;
        private frmTelegraphHIDAdapterEditor m_Form = null;
        //private TelegraphHIDAdapter m_Adatper = null;

        //! default constructor
        public ESnailHIDAgent()
            :base()
        {
            Initialize();
        }


        //! constructor with adapter and parent
        public ESnailHIDAgent(TelegraphHIDAdapter AdapterItem)
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
                m_Adapter = new TelegraphHIDAdapter(m_AdapterID);
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

        public TelegraphHIDAdapter Adapter
        {
            get { return m_Adapter as TelegraphHIDAdapter; }
        }

        //! propery for get test form
        public override Form Editor
        {
            get 
            {               
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphHIDAdapterEditor(m_Adapter as TelegraphHIDAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                }

                return m_Form;
            }
        }

        private void FormDisposedEventHandler(object sender, EventArgs e)
        {
            m_Form.Disposed -= new EventHandler(FormDisposedEventHandler);
            m_Form.tabDebug.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form.tabCommunication.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form.tabAdatperInfo.Disposed -= new EventHandler(TablePageDisposedEventHandler);
            m_Form.tabDevices.Disposed -= new EventHandler(TablePageDisposedEventHandler);

            m_Form = null;
        }

        //! propery for get DeviceManagerTabPage
        public override TabPage DeviceManagerPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphHIDAdapterEditor(m_Adapter as TelegraphHIDAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.tabDevices.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.tabDevices;
            }
        }

        private void TablePageDisposedEventHandler(object sender, EventArgs e)
        {
            if (null != m_Form)
            {
                m_Form.Disposed -= new EventHandler(FormDisposedEventHandler);
                m_Form.tabDebug.Disposed -= new EventHandler(TablePageDisposedEventHandler);
                m_Form.tabCommunication.Disposed -= new EventHandler(TablePageDisposedEventHandler);
                m_Form.tabAdatperInfo.Disposed -= new EventHandler(TablePageDisposedEventHandler);
                m_Form.tabDevices.Disposed -= new EventHandler(TablePageDisposedEventHandler);

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
                    m_Form = new frmTelegraphHIDAdapterEditor(m_Adapter as TelegraphHIDAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.tabDebug.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.tabDebug;
            }
        }

        //! propery for get DebugTabPage
        public override TabPage CommunicationPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphHIDAdapterEditor(m_Adapter as TelegraphHIDAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.tabCommunication.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.tabCommunication;
            }
        }

        //! propery for get DebugTabPage
        public override TabPage InformationPage
        {
            get
            {
                if (null == m_Form)
                {
                    m_Form = new frmTelegraphHIDAdapterEditor(m_Adapter as TelegraphHIDAdapter);
                    m_Form.Disposed += new EventHandler(FormDisposedEventHandler);
                    m_Form.tabAdatperInfo.Disposed += new EventHandler(TablePageDisposedEventHandler);
                }

                return m_Form.tabAdatperInfo;
            }
        }




    }
}
