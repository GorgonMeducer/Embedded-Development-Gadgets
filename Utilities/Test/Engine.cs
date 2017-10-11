using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities.Threading;
using ESnail.Utilities.Generic;
using ESnail.Utilities.IO;
using ESnail.Utilities;
using System.IO;
using System.Threading;
using ESnail.Utilities.Log;
using System.Reflection;

namespace ESnail.Utilities.Test
{

    public abstract partial class ReportService : TPipelineCoreService<TestReport>, ILogUser
    {

        private LOG_LEVEL m_LogLevel = LOG_LEVEL.NONE;

        public ReportService(TestReport tItem)
                : base(tItem)
        {
            m_Logger = new Logger(this);
        }

        public ReportService(TestReport tItem, Int32 SafeTimeOutSetting)
                : base(tItem, SafeTimeOutSetting)
        {
            m_Logger = new Logger(this);
        }



        public LOG_LEVEL LogLevel
        {
            get => m_LogLevel;
            set
            {
                m_LogLevel = value;

                if (m_LogLevel == LOG_LEVEL.NONE)
                {
                    this.EnableLog = false;
                }
                else
                {
                    this.EnableLog = true;
                }
            }
        }


        private Logger m_Logger = null;

        public abstract SafeID ChannalID
        {
            get;
        }

        public abstract string ObjectName
        {
            get;
        }

        public bool EnableLog
        {
            get { return m_Logger.EnableLog; }
            set { m_Logger.EnableLog = value; }
        }
        public bool XMLModeEnabled
        {
            get { return m_Logger.XMLModeEnabled; }
            set { m_Logger.XMLModeEnabled = value; }
        }

        public bool TimeStampEnabled
        {
            get { return m_Logger.TimeStampEnabled; }
            set { m_Logger.TimeStampEnabled = value; }
        }

        protected void __WriteLogLine(String tLog)
        {
            m_Logger.WriteLogLine(tLog);
        }

        protected void __WriteLog(String tLog)
        {
            m_Logger.WriteLog(tLog);
        }

        protected void __BeginLog()
        {
            m_Logger.BeginLog();
        }

        protected void __EndLog()
        {
            m_Logger.EndLog();
        }

        private class Logger : ESnail.Utilities.Log.LogAgent
        {
            public Logger(ILogUser LogAgent) : base(LogAgent)
            {
            }

            protected override String Name
            {
                get { return m_LogAgent.ObjectName; }
            }

            protected override String[] XMLChannals
            {
                get
                {
                    String[] c_ChannelList = { "XMLReportService", "XML[" + m_LogAgent.ChannalID + "]" };
                    return c_ChannelList;
                }
            }


            protected override String[] Channals
            {
                get
                {
                    String[] c_ChannelList = { "ReportService", m_LogAgent.ChannalID };
                    return c_ChannelList;
                }
            }
        }
    }

    public abstract class ReportEngine : ESDisposableClass
    {
        protected ReportReaderEngine m_Engine = new ReportReaderEngine();
        protected ManualResetEvent m_tComplete = new ManualResetEvent(false);
        protected List<TestReport> m_ReportList = new List<TestReport>();


        public ReportEngine()
        {
            m_Engine.PipelineCoreStateReportEvent += TReader_PipelineCoreStateReportEvent;
            m_Engine.AutoStart = true;
        }


        protected override void _Dispose()
        {
            m_Engine.Dispose();
        }

        private LOG_LEVEL m_LogLevel = LOG_LEVEL.PROGRESS;

        public LOG_LEVEL LogLevel
        {
            get
            {
                return m_LogLevel;
            }
            set
            {
                m_LogLevel = value;

            }
        }



        public delegate void CompleteHandler(TestReport[] tReports);

        public event CompleteHandler CompleteEvent;

        private SafeInvoker m_Invoker = new SafeInvoker();

        protected void RaiseCompleteEvent(TestReport[] tReports)
        {
            m_tComplete.Set();
            if (null == CompleteEvent)
            {
                return;
            }

            m_Invoker.BeginInvoke(CompleteEvent, tReports);
        }

        public TestReport[] Reports
        {
            get { return m_ReportList.ToArray(); }
        }

        private void TReader_PipelineCoreStateReportEvent(PIPELINE_STATE State, PipelineCore PipelineCore)
        {
            switch (State)
            {
                case PIPELINE_STATE.PIPELINE_START:
                    break;
                case PIPELINE_STATE.PIPELINE_STOPPED:
                    RaiseCompleteEvent(Reports);
                    break;
            }
        }

        public ManualResetEvent CompleteSignal
        {
            get { return m_tComplete; }
        }

        #region Internal Engine and Servies
        protected class ReportReaderEngine : PipelineCore
        {
            public override bool Available
            {
                get { return true; }
            }

            protected override void _Dispose()
            {

            }

        }
        #endregion
    }


    public class TestNode
    {
        private String m_NodeName = null;
        private Boolean m_Available = false;
        private String m_Value = null;

        public TestNode(String tName, String tValue)
        {
            if (null == tName)
            {
                return;
            }
            else if (tName.Trim() == "")
            {
                return;
            }
            else if (null == tValue)
            {
                return;
            }

            m_NodeName = tName.Trim();
            m_Value = tValue.Trim();

            m_Available = true;
        }


        public String Value
        {
            get { return m_Value; }
        }

        public String Name
        {
            get { return m_NodeName; }
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        public override String ToString()
        {
            return m_Value + "\t:  " + m_NodeName;
        }
    }

    public class TestReport
    {
        private String m_Source = null;
        private Boolean m_Available = false;
        private String m_ErrorInfo = "";

        public enum TaskStatus
        {
            INVALID,
            PENDING,
            VALID,
            ERROR
        };

        private TaskStatus m_Status = TaskStatus.INVALID;

        private List<TestNode> m_List = null;

        public TestReport(String tSource)
        {
            if (File.Exists(tSource))
            {
                m_Source = Path.GetFullPath(tSource);
            }
            else
            {
                m_Source = tSource;
            }

            m_List = new List<TestNode>();
            m_Status = TaskStatus.PENDING;

            m_Available = true;
        }

        public String Source
        {
            get { return m_Source; }
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        public TestNode[] Nodes
        {
            get { return m_List.ToArray(); }
        }

        public void AddNode(TestNode tNode)
        {
            m_Status = TaskStatus.VALID;
            m_List.Add(tNode);
        }

        public TaskStatus Status
        {
            get { return m_Status; }
        }

        public String ErrorInfo
        {
            get { return m_ErrorInfo; }
        }

        internal void ReportError(String tErr)
        {
            m_ErrorInfo = tErr;
        }
    }

}