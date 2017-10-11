using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace ESnail.Device
{
    public partial class AdapterAgent : System.ComponentModel.Component, IAdapter 
    {
        protected Adapter m_Adapter = null;

        public AdapterAgent()
        {
            Initialize();
        }

        public AdapterAgent(Adapter AdapterItem)
        {
            m_Adapter = AdapterItem;
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();
        }


        public virtual Form Editor
        {
            get { return null; }
        }

        public virtual TabPage DeviceManagerPage
        {
            get { return null; }
        }

        public virtual TabPage DebugPage
        {
            get { return null; }
        }

        public virtual TabPage CommunicationPage
        {
            get { return null; }
        }

        public virtual TabPage InformationPage
        {
            get { return null; }
        }


        public virtual Control DefaultControl
        {
            get { return null; }
        }

        public virtual Control CreateControl()
        {
            return null;
        }

        public virtual System.ComponentModel.Component DefaultComponent
        {
            get { return this; }
        }

        public virtual System.ComponentModel.Component CreateComponent()
        {
            return null;
        }

        public virtual Adapter Adatper
        {
            get { return m_Adapter; }
        }

        public Form CreateEditor()
        {
            return null;
        }

        public Boolean ImportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if (null == m_Adapter)
            {
                return false;
            }

            return m_Adapter.ImportDefaultSetting(xmlDoc,xmlRoot);
        }

        public Boolean ExportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if (null == m_Adapter)
            {
                return false;
            }
            return m_Adapter.ExportDefaultSetting(xmlDoc,xmlRoot);
        }

        public Boolean ImportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if (null == m_Adapter)
            {
                return false;
            }
            return m_Adapter.ImportSetting(xmlDoc, xmlRoot);
        }

        public Boolean ExportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if (null == m_Adapter)
            {
                return false;
            }
            return m_Adapter.ExportSetting(xmlDoc, xmlRoot);
        }

    }
}
