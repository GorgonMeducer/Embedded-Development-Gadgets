using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ESnail.Utilities;
using ESnail.Utilities.Windows.Forms.Interfaces;
using System.Windows.Forms;
using ESnail.Utilities.Log;
using ESnail.Utilities.Windows;
using ESnail.Utilities.XML;
using System.ComponentModel;


namespace ESnail.Device
{
    //! \name adatper current telegraphs auto-detect result
    //! @{
    public enum TELEGRAPH_AUTO_DETECT_RESULT
    { 
        NO_TELEGRAPH_MATCHED,                       //!< no telegraph matched
        ONE_TELEGRAPH_MATCHED,                      //!< one telegraph matched
        MULTIPLE_TELEGRAPHS_MATCHED                 //!< multiple telegraphs matched
    }
    //! @}

    public delegate void AdapterAvailableTelegraphAutoDetectionReport(TELEGRAPH_AUTO_DETECT_RESULT Result, System.String[] tTypes);

    public interface IAdapterEditorComponent
    {
        TabPage DeviceManagerPage
        {
            get;
        }

        TabPage DebugPage
        {
            get;
        }

        TabPage CommunicationPage
        {
            get;
        }

        TabPage InformationPage
        {
            get;
        }

        Adapter Adatper
        {
            get;
        }
    }

    //! \name interfaces IAdapter
    //! @{
    public interface IAdapter : IControl, IAdapterEditorComponent, IEditor, ESnail.Utilities.Windows.Forms.Interfaces.IComponent, IOBJXMLSettingIO
    {
        Boolean ImportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot);

        Boolean ExportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot);
    }
    //! @}

    /*
    public class AdapterLoader
    {
        public virtual Adapter Create(SafeID tID, params Object[] tArgs)
        {
            return null;
        }
    }
    */

    //! /name Abstruct class Adapter
    abstract public class Adapter : Debug, IDisposable, IAdapter, ILog, ISafeID
    {
        protected List<Telegraph> m_SupportTelegraphList = new List<Telegraph>();
        private AdapterLog m_LogWriter = null;
        private SafeID m_ID = null;
        private System.Boolean m_bDisposed = false;  

        public abstract Adapter CreateAdapter(SafeID tID);

        //! constructor
        public Adapter(SafeID tID)
        {
            m_LogWriter = new AdapterLog(this);

            m_ID = tID;

            if (null != m_WindowsMessageHandler)
            {
                m_WindowsMessageHandler.WindowsMessageArrived += new WindowsMessageProcessor(this.DeviceMessageProcess);
            }

            WriteLogLine("Adapter Initialized.");
        }

        //! destructor
        ~Adapter()
        {
            //System.IDisposable.
            Dispose();            
        }

        //! get information of all adapter supported telegraphs
        public String[] SupportedTelegraph
        {
            get
            {
                List<System.String> tInfoList = new List<string>();
                foreach (Telegraph tTelegraph in m_SupportTelegraphList)
                {
                    tInfoList.Add(tTelegraph.Type);
                }

                if (0 == tInfoList.Count)
                {
                    return null;
                }

                return tInfoList.ToArray();
            }
        }

        //! \brief method for register supported telegraph
        public abstract Boolean RegisterSupportTelegraph(Telegraph tTelegraph);

        //! method for create telegraph with a telegraph type string
        public Telegraph CreateTelegraph(System.String strType, params Object[] Args)
        {            
            if (null == strType)
            {
                return null;
            }
            if ("" == strType.Trim().ToUpper())
            {
                return null;
            }

            foreach (Telegraph tTelegraph in m_SupportTelegraphList)
            {
                if (tTelegraph.Type.Trim().ToUpper() == strType.Trim().ToUpper())
                {
                    Telegraph tResultTelegraph = tTelegraph.CreateTelegraph(Args);
                    if (null != tResultTelegraph)
                    {
                        return tResultTelegraph;
                    }
                }
            }

            return null;
        }


        public event AdapterAvailableTelegraphAutoDetectionReport AdapterAvailableTelegraphAutoDetectionReportEvent;

        //! \brief raise auto detect report event
        protected void OnAdapterAvailableTelegraphAutoDetectionReport(TELEGRAPH_AUTO_DETECT_RESULT Result, System.String[] tTypes)
        {
            BeginInvoke(AdapterAvailableTelegraphAutoDetectionReportEvent, Result, tTypes);
        }

        public abstract System.Boolean AutoDetectDeviceTelegraph();


        //! property Adapter Type
        abstract public String Type
        {
            get;
        }

        abstract public String Version
        {
            get;
        }

        //! override ToString method
        public override String ToString()
        {
            return "Adapter:"+m_ID;
        }

        //! \brief get adapter ID
        public SafeID ID
        {
            get { return m_ID; }
            set { }
        }

        //! property for get adapter busy state
        public abstract Boolean IsBusy
        {
            get;
        }

        //! property for checking whether system is working
        public abstract Boolean IsWorking
        {
            get;
            set;
        }

        //! Dispose method
        public void Dispose()
        {
            if (!m_bDisposed)
            {
                m_bDisposed = true;
                try
                {
                    if (null != m_WindowsMessageHandler)
                    {
                        m_WindowsMessageHandler.WindowsMessageArrived -= new WindowsMessageProcessor(this.DeviceMessageProcess);
                    }

                    _Dispose();
                }
                catch (Exception) { }

                try
                {
                    WriteLogLine("Adapter Disposed!");
                }
                catch (Exception) { }

                GC.SuppressFinalize(this);
            }
        }

        protected abstract void _Dispose();

        //! Open method
        public abstract Boolean Open
        {
            get;
            set;
        }

        static internal WindowsMessageHandler m_WindowsMessageHandler = null;

        protected abstract void DeviceMessageProcess(System.Windows.Forms.Message m);
        public abstract void RegisterDeviceMessageHandler(ref WindowsMessageHandler tHandler, IntPtr hwndParent);
        protected abstract void UnregisterDeviceMessageHandler();

        public void WriteLogLine(System.String strLog)
        {
            if ((null != m_LogWriter))
            {
                m_LogWriter.WriteLogLine(strLog);
            }
        }

        public void BeginLog()
        {
            if ((null != m_LogWriter))
            {
                m_LogWriter.BeginLog();
            }
        }

        public void EndLog()
        {
            if ((null != m_LogWriter))
            {
                m_LogWriter.EndLog();
            }
        }

        public void WriteLog(System.String strLog)
        {
            if ((null != m_LogWriter))
            {
                m_LogWriter.WriteLog(strLog);
            }
        }

        public virtual  Form Editor
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

        public virtual Adapter Adatper
        {
            get { return this; }
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
            get { return null; }
        }

        public virtual System.ComponentModel.Component CreateComponent()
        {
            return null;
        }


        public virtual Form CreateEditor()
        {
            return null;
        }

        //! \brief property for getting/setting adapter name
        public virtual System.String Name
        {
            get;
            set;
        }

        public SafeID ChannalID
        {
            get { return this.ID; }
        }

        public System.String ObjectName
        {
            get { return this.Name; }
        }

        public override Boolean DebugEnabled
        {
            get
            {
                return base.DebugEnabled;
            }
            set
            {
                base.DebugEnabled = value;
                if (value)
                {
                    WriteLogLine("Debug interface enabled.");
                }
                else
                {
                    WriteLogLine("Debug interface disabled.");
                }
            }
        }


        public virtual Boolean ImportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            return true;
        }

        public virtual Boolean ExportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            return false;
        }


        public virtual Boolean ImportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            return true;
        }

        public virtual Boolean ExportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            return false;
        }


        public Boolean EnableLog
        {
            get { return m_LogWriter.EnableLog; }
            set { m_LogWriter.EnableLog = value; }
        }

        public Boolean XMLModeEnabled
        {
            get { return m_LogWriter.XMLModeEnabled; }
            set { m_LogWriter.XMLModeEnabled = value; }
        }

        public Boolean TimeStampEnabled
        {
            get { return m_LogWriter.TimeStampEnabled; }
            set { m_LogWriter.TimeStampEnabled = value; }
        }

    }

    internal class AdapterLog : LogAgent
    {
        

        public AdapterLog(Adapter AdapterItem)
            : base(AdapterItem)
        { 
            
        }

        protected override System.String Name
        {
            get { return "Adapter"; }
        }

        protected override String[] XMLChannals
        {
            get 
            {
                List<String> strXMLChannalList = new List<String>();

                strXMLChannalList.Add("XMLAdapters");
                if (null != m_LogAgent)
                {
                    strXMLChannalList.Add("XML["+m_LogAgent.ChannalID+"]");
                }

                return strXMLChannalList.ToArray();
            }
        }

        protected override String[] Channals
        {
            get
            {
                List<String> strXMLChannalList = new List<String>();

                strXMLChannalList.Add("Adapters");
                if (null != m_LogAgent)
                {
                    strXMLChannalList.Add("[" + m_LogAgent.ChannalID + "]");
                }

                return strXMLChannalList.ToArray();
            }
        }

        
    }
}
