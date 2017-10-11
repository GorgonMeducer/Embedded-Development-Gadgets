using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Windows.Threading;

namespace ESnail.Utilities.Log
{

    public enum LOG_LEVEL : UInt32
    {
        NONE = 0,
        PROCEDURE,
        PROGRESS,
        DETAILS
    }

    //! \brief a delegate for raising log message arrive event
    public delegate void LogMessage(String LogMessage);

    internal struct LogReceiver
    {
        public LogMessage LogMessageEvent;
        public LogMessage LogMessageSyncEvent;
        public String m_ID;
        

        LogReceiver(String strID)
        {
            m_ID = strID;
            LogMessageEvent = null;
            LogMessageSyncEvent = null;
        }

        public void OnLogMessageArrival(String LogMessage)
        {
            if (null != LogMessageSyncEvent)
            {
                try
                {
                    LogMessageSyncEvent.Invoke(LogMessage);
                }
                catch (Exception )
                { }
            }
            
        }
    }

    internal class SafeLogWriter : DispatcherObject
    { 
        
    }

    //! \name asynchronouse log writer
    //! @{
    public static class LogWriter
    {
        //! log message arrive event
        private static List<LogReceiver> m_LogReceiverList = new List<LogReceiver>();

        private static event LogMessage LogMessageEvent;
        private static event LogMessage LogMessageSyncEvent;

        //private static Form m_RootParentForm = null;
        private static SafeLogWriter m_RootParentForm = new SafeLogWriter();

        public static Boolean RegisterLogReceiver(String strID, LogMessage Handler)
        {
            return RegisterLogReceiver(strID, Handler, true);
        }

            //! register a log message receiver
        public static Boolean RegisterLogReceiver(String strID, LogMessage Handler, Boolean bIsAsync)
        {
            if (null == Handler)
            {
                return false;
            }

            if ((null == strID) || ("" == strID) || ("ALL" == strID.ToUpper().Trim()))
            {
                //! register a broadcast receiver
                if (bIsAsync)
                {
                    LogMessageEvent += new LogMessage(Handler);
                }
                else
                {
                    LogMessageSyncEvent += new LogMessage(Handler);
                }
                return true;
            }

            LogReceiver Result = new LogReceiver();
            if (FindReceiver(strID, ref Result))
            {
                //! find the receiver
                if (bIsAsync)
                {
                    Result.LogMessageEvent += new LogMessage(Handler);
                }
                else
                {
                    Result.LogMessageSyncEvent += new LogMessage(Handler);
                }
                return true;
            }
            else
            {
                //! a new receiver
                Result.m_ID = strID;
                if (bIsAsync)
                {
                    Result.LogMessageEvent += new LogMessage(Handler);
                }
                else
                {
                    Result.LogMessageSyncEvent += new LogMessage(Handler);
                }

                m_LogReceiverList.Add(Result);
                return true;
            }
        }

        //! register a log message receiver
        public static Boolean RegisterLogReceiver(LogMessage Handler)
        {
            return RegisterLogReceiver(null, Handler);
        }

        //! register a log message receiver
        public static Boolean UnregisterLogReceiver(String strID, LogMessage Handler)
        {
            if (null == Handler)
            {
                return false;
            }

            if ((null == strID) || ("" == strID) || ("ALL" == strID.ToUpper().Trim()))
            {
                //! register a broadcast receiver
                LogMessageEvent -= new LogMessage(Handler);
                LogMessageSyncEvent -= new LogMessage(Handler);
                return true;
            }

            LogReceiver Result = new LogReceiver();
            if (FindReceiver(strID, ref Result))
            {
                //! find the receiver
                Result.LogMessageEvent -= new LogMessage(Handler);
                Result.LogMessageSyncEvent -= new LogMessage(Handler);
                return true;
            }

            return true;
        }

        //! register a log message receiver
        public static Boolean UnregisterLogReceiver(LogMessage Handler)
        {
            return UnregisterLogReceiver(null, Handler);
        }

        //! \brief a method for write a log line
        public static void WriteLine(String strReceiverID, String strLogMessage)
        {
            if (null == strLogMessage)
            {
                return;
            }

            if ((null == strReceiverID) || ("" == strReceiverID) || ("ALL" == strReceiverID.ToUpper().Trim()))
            {
                strLogMessage += "\r\n";

                if (null != LogMessageEvent)
                {
                    try
                    {
                        m_RootParentForm.Dispatcher.BeginInvoke(DispatcherPriority.Normal, LogMessageEvent, strLogMessage);
                    }
                    catch (Exception) { }
                    
                }

                if (null != LogMessageSyncEvent)
                {
                    try
                    {
                        LogMessageEvent(strLogMessage);
                    }
                    catch (Exception) { }
                }
               
            }

            LogReceiver Result = new LogReceiver();
            if (FindReceiver(strReceiverID, ref Result))
            {
                //! find the receiver
                strLogMessage += "\r\n";

                try
                {
                    if (null != Result.LogMessageEvent)
                    {
                        m_RootParentForm.Dispatcher.BeginInvoke(DispatcherPriority.Normal, Result.LogMessageEvent, strLogMessage);
                    }
                }
                catch (Exception) { }

                Result.OnLogMessageArrival(strLogMessage);
            }
        }

        public static void WriteLine(String strLogMessage)
        {
            WriteLine(null, strLogMessage);
        }

        //! \brief a method for write a string
        public static void Write(String strReceiverID,String strLogMessage)
        {
            if (null == strLogMessage)
            {
                return;
            }

            if ((null == strReceiverID) || ("" == strReceiverID) || ("ALL" == strReceiverID.ToUpper().Trim()))
            {
                if (null != LogMessageEvent)
                {
                    try
                    {
                        m_RootParentForm.Dispatcher.BeginInvoke(DispatcherPriority.Normal, LogMessageEvent, strLogMessage);
                    }
                    catch (Exception) { }

                }

                if (null != LogMessageSyncEvent)
                {
                    try
                    {
                        LogMessageEvent(strLogMessage);
                    }
                    catch (Exception) { }
                }
            }

            LogReceiver Result = new LogReceiver();
            if (FindReceiver(strReceiverID, ref Result))
            {
                //! find the receiver
                try
                {
                    if (null != Result.LogMessageEvent)
                    {
                        m_RootParentForm.Dispatcher.BeginInvoke(DispatcherPriority.Normal, Result.LogMessageEvent, strLogMessage);
                    }
                }
                catch (Exception) { }

                Result.OnLogMessageArrival(strLogMessage);
            }
        }

        public static void Broadcast(System.String strLogMessage)
        {
            Write(null, strLogMessage);
        }


        private static Boolean FindReceiver(String strID,ref LogReceiver Result)
        {
            List<LogReceiver>.Enumerator tempListEnum = m_LogReceiverList.GetEnumerator();

            if (null == strID)
            {
                return false;
            }

            while (tempListEnum.MoveNext())
            {
                if (tempListEnum.Current.m_ID == strID)
                {
                    Result = tempListEnum.Current;
                    return true;
                }
            }

            return false;
        }

    }
    //! @{


    public interface ILogUser
    {
        SafeID ChannalID
        {
            get;
        }

        System.String ObjectName
        {
            get;
        }

        Boolean EnableLog
        {
            get;
            set;
        }

        Boolean XMLModeEnabled
        {
            get;
            set;
        }

        Boolean TimeStampEnabled
        {
            get;
            set;
        }
    }

    //! \brief interface  Log
    //! @{
    public interface ILog : ILogUser
    {
        
        void WriteLog(System.String strLog);

        void WriteLogLine(System.String strLog);

        void BeginLog();

        void EndLog();
 
    }
    //! @}

    public abstract class LogAgent: ILog
    {
        private StringBuilder m_sbWriter = null;
        protected ILogUser m_LogAgent = null;
        private Boolean m_EnableLog = false;
        private Boolean m_XMLModeEnabled = false;
        private Boolean m_TimestampEnabled = true;
        private Object m_Locker = new object();

        public LogAgent(ILogUser LogAgent)
        {
            m_LogAgent = LogAgent;
        }

        protected abstract String Name
        {
            get;
        }

        protected abstract String[] XMLChannals
        {
            get;
        }

        protected abstract String[] Channals
        {
            get;
        }

        public Boolean TimeStampEnabled
        {
            get
            {
                return m_TimestampEnabled;
            }
            set
            {
                m_TimestampEnabled = value;
            }
        }

        public Boolean EnableLog
        {
            get 
            {
                return m_EnableLog;
            }
            set
            {
                m_EnableLog = value;
            }
        }

        public Boolean XMLModeEnabled
        {
            get
            {
                return m_XMLModeEnabled;
            }
            set
            {
                m_XMLModeEnabled = value;
            }
        }

        public void WriteLog(String strLog)
        {
            lock (m_Locker)
            {
                _WriteLog(strLog);
            }
        }

        private void _WriteLog(String strLog)
        {
            if (!m_EnableLog)
            {
                return;
            }
            
            if (null == m_sbWriter)
            {
                _BeginLog();
            }
            
            m_sbWriter.Append(strLog);
        }

        public void WriteLogLine(String strLog)
        {
            if (!m_EnableLog)
            {
                return;
            }
            lock (m_Locker)
            {
                _BeginLog();
                _WriteLog(strLog);
                _EndLog();
            }
        }

        public void BeginLog()
        {
            lock (m_Locker)
            {
                _BeginLog();
            }
        }

        private void _BeginLog()
        {
            if (!m_EnableLog)
            {
                return;
            }
            _EndLog();
            do
            {
                try
                {
                    m_sbWriter = new StringBuilder();
                    break;
                }
                catch (Exception) { }
            } while (true);
        }

        public void EndLog()
        {
            lock (m_Locker)
            {
                _EndLog();
            }
        }

        private void _EndLog()
        {
            if (!m_EnableLog)
            {
                return;
            }

            if (null != m_sbWriter)
            {
                try
                {

                    if (m_XMLModeEnabled)
                    {

                        StringBuilder sbXMLWriter = new StringBuilder();

                        XmlWriterSettings XmlSetting = new XmlWriterSettings();

                        XmlSetting.Indent = true;                                   //!< enable indent
                        XmlSetting.IndentChars = "    ";                            //!< using whitespace as indent-chars
                        XmlSetting.OmitXmlDeclaration = true;

                        XmlWriter xmlWriter = XmlWriter.Create(sbXMLWriter, XmlSetting);
#if false


                        XmlDocument xmlDoc = new XmlDocument();

                        do
                        {
                            StringBuilder sbRootXML = new StringBuilder();

                            sbRootXML.Append('<');
                            sbRootXML.Append(Name);

                            if (null != m_LogAgent)
                            {
                                sbRootXML.Append(" ID=");
                                sbRootXML.Append('"');
                                sbRootXML.Append(ChannalID);
                                sbRootXML.Append('"');

                                //! name
                                sbRootXML.Append(" Name=");
                                sbRootXML.Append('"');
                                sbRootXML.Append(ObjectName);
                                sbRootXML.Append('"');
                            }
                            sbRootXML.Append("/>");

                            xmlDoc.LoadXml(sbRootXML.ToString());
                        }
                        while (false);
#else
                        XmlDocument xmlDoc = Utilities.XML.XMLHelper.New(Name);

#endif

                        //! create root
                        XmlNode xmlRoot = xmlDoc.DocumentElement;

                        //! write date
                        do
                        {
                            XmlElement xmlElmentItem = xmlDoc.CreateElement("Date");
                            xmlElmentItem.InnerText = DateTime.Now.ToLongDateString();

                            xmlRoot.AppendChild(xmlElmentItem);
                        }
                        while (false);

                        //! write time
                        do
                        {
                            XmlElement xmlElmentItem = xmlDoc.CreateElement("Time");
                            xmlElmentItem.InnerText = DateTime.Now.ToLongTimeString();

                            xmlRoot.AppendChild(xmlElmentItem);
                        }
                        while (false);


                        //! write time
                        do
                        {
                            XmlElement xmlElmentItem = xmlDoc.CreateElement("Message");
                            xmlElmentItem.InnerText = m_sbWriter.ToString();

                            xmlRoot.AppendChild(xmlElmentItem);
                        }
                        while (false);

                        xmlDoc.AppendChild(xmlRoot);

                        xmlDoc.Save(xmlWriter);

                        if (null != XMLChannals)
                        {
                            foreach (String strChannal in XMLChannals)
                            {
                                try
                                {
                                    LogWriter.WriteLine(strChannal, sbXMLWriter.ToString());
                                }
                                catch (Exception) { }
                            }

                            if (null != Channals)
                            {
                                foreach (String strChannal in Channals)
                                {
                                    try
                                    {
                                        if (m_TimestampEnabled)
                                        {
                                            LogWriter.Write(strChannal, "[" + DateTime.Now.ToString("MM-dd-yy hh:mm:ss") + "]  ");
                                        }
                                        LogWriter.WriteLine(strChannal, m_sbWriter.ToString());
                                    }
                                    catch (Exception) { }
                                }
                            }
                            else
                            {
                                try
                                {
                                    //! to open channal
                                    LogWriter.WriteLine(m_sbWriter.ToString());
                                }
                                catch (Exception) { }
                            }
                        }
                        else
                        {
                            try
                            {
                                //! to open channal
                                LogWriter.WriteLine(m_sbWriter.ToString());
                            }
                            catch (Exception) { }
                        }
                    }
                    else
                    {
                        if (null != Channals)
                        {
                            foreach (String strChannal in Channals)
                            {
                                try
                                {
                                    if (m_TimestampEnabled)
                                    {
                                        LogWriter.Write(strChannal, "[" + DateTime.Now.ToString("MM-dd-yy hh:mm:ss") + "]  ");
                                    }
                                    LogWriter.WriteLine(strChannal, m_sbWriter.ToString());
                                }
                                catch (Exception) { }
                            }
                        }
                        else
                        {
                            try
                            {
                                //! to open channal
                                LogWriter.WriteLine(m_sbWriter.ToString());
                            }
                            catch (Exception) { }
                        }
                    }
                }
                catch (Exception) { }
                finally
                {
                    m_sbWriter = null;
                }
            }
        }

        public SafeID ChannalID
        {
            get
            {
                if (null != m_LogAgent)
                {
                    return m_LogAgent.ChannalID;
                }

                throw new Exception("Null reference to ILog interface object.");
            }
        }


        public virtual String ObjectName
        {
            get
            {
                if (null != m_LogAgent)
                {
                    return m_LogAgent.ObjectName;
                }

                return null;
            }
        }

    }

}
