using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

using ESnail.Component;
using ESnail.Utilities;
using ESnail.Utilities.Generic;


namespace ESnail.Documents.Database
{

    partial class BMDBSQLServer
    {
        public delegate Boolean BeginExport(String tPath, String tSamplingName);
        public delegate Boolean EndExport(String tPath, String tSamplingName);
        public delegate Boolean RequestWriteRow(Object[] tRowItems, DataColumnCollection tColumns);
        public delegate void ExportDatabaseReport(EXPORT_DATABASE_REPORT tResult, Int32 tPercent, TimeSpan tTimeLeft);

        public interface IDataBaseExporter
        {
            event BeginExport BeginExportEvent;

            event EndExport EndExportEvent;

            event RequestWriteRow RequestWriteRowEvent;

            event ExportDatabaseReport ExportDatabaseReportEvent;

            Boolean TryToExportDatabase(String tPath, String tSamplingPlan);

            Boolean IsWorking
            {
                get;
                set;
            }
        }

        public IDataBaseExporter NewExporter()
        {
            return new DatabaseExporter(this);
        }

        private TSet<DatabaseExporter> m_ExporterSet = new TSet<DatabaseExporter>();

        //! \brief register exporter
        public Boolean RegisterExporter(IDataBaseExporter tExporter)
        {
            DatabaseExporter tDBExporter = tExporter as DatabaseExporter;
            if (null == tDBExporter)
            {
                return false;
            }
            if (!tDBExporter.Available)
            {
                return false;
            }

            m_ExporterSet.Add(tDBExporter);

            return true;
        }

        //! \brief unregister exporter
        public void UnregisterExporter(IDataBaseExporter tExporter)
        {
            DatabaseExporter tDBExporter = tExporter as DatabaseExporter;
            if (null == tDBExporter)
            {
                return;
            }
            if (!tDBExporter.Available)
            {
                return;
            }

            DatabaseExporter tTarget = m_ExporterSet.Find(tDBExporter.ID);
            m_ExporterSet.Remove(tDBExporter.ID);

            tTarget.Dispose();
            tDBExporter.Dispose();            
        }


        //! \name export database report
        //! @{
        public enum EXPORT_DATABASE_REPORT
        {
            DB_EXPORTING,                       //!< exporting
            DB_COMPLETE,                        //!< completed
            DB_CANCELLED,                       //!< cancelled
            DB_FAILED                           //!< failed
        }
        //! @}

        protected class DatabaseExporter : IDataBaseExporter, ISafeID, IDisposable 
        {
            private BMDBSQLServer m_Parent = null;
            private Boolean m_bAvailable = false;
            private Boolean m_Busy = false;
            private Thread m_ExportingThread = null;
            private ManualResetEvent m_RequestStop = new ManualResetEvent(false);

            //! \brief constructor
            public DatabaseExporter(BMDBSQLServer tParent)
            {
                m_Parent = tParent;

                if (null == m_Parent)
                {
                    return;
                }
                //! try to connect to database
                if (m_Parent.SamplingCount() < 0)
                {
                    return;
                }

                m_bAvailable = true;
            }

            //! \brief available
            public Boolean Available
            {
                get { return m_bAvailable; }
            }

            public event BeginExport BeginExportEvent;
            private Boolean OnBeginExport(String tPath, String tSamplingName)
            {
                if (null != BeginExportEvent)
                {
                    try
                    {
                        return BeginExportEvent.Invoke(tPath, tSamplingName);
                    }
                    catch (Exception )
                    {
                        return false;
                    }
                }

                return true;
            }

            public event EndExport EndExportEvent;
            private Boolean OnEndExport(String tPath, String tSamplingName)
            {
                if (null != EndExportEvent)
                {
                    try
                    {
                        return EndExportEvent.Invoke(tPath, tSamplingName);
                    }
                    catch (Exception )
                    {
                        return false;
                    }
                }

                return true;
            }

            public event RequestWriteRow RequestWriteRowEvent;
            private Boolean OnRequestWriteRow(Object[] tItems, DataColumnCollection tColumns)
            {
                if (null != EndExportEvent)
                {
                    try
                    {
                        return RequestWriteRowEvent.Invoke(tItems, tColumns);
                    }
                    catch (Exception )
                    {
                        return false;
                    }
                }

                return true;
            }

            public event ExportDatabaseReport ExportDatabaseReportEvent;
            private void OnExportDatabaseReport(EXPORT_DATABASE_REPORT tResult, Int32 tPercent, TimeSpan tTimeLeft)
            {
                if (null == ExportDatabaseReportEvent)
                {
                    return;
                }

                try
                {
                    ExportDatabaseReportEvent.Invoke(tResult, tPercent, tTimeLeft);
                }
                catch (Exception )
                {
                }
            }


            #region ISafeID Members

            private SafeID m_tID = null;

            public SafeID ID
            {
                get
                {
                    return m_tID;
                }
                set { }
            }

            #endregion

            private String m_Path = null;
            private String m_SamplingPlan = null;

            public Boolean TryToExportDatabase(String tPath, String tSamplingPlan)
            {
                //! check exporter
                if (!m_bAvailable)
                {
                    return false;
                }
                if (null == m_Parent)
                {
                    return false;
                }
                if (m_Busy)
                {
                    return false;
                }

                //! find sampling plan
                if (-1 == m_Parent.FindSampling(tSamplingPlan))
                {
                    return false;
                }

                m_Path = tPath;
                m_SamplingPlan = tSamplingPlan;

                //! set busy flag
                m_Busy = true;
                m_ExportingThread = new Thread(DoWork);
                m_ExportingThread.Priority = ThreadPriority.BelowNormal;
                m_ExportingThread.IsBackground = true;
                //! set default culture
                m_ExportingThread.CurrentCulture = new System.Globalization.CultureInfo("en-us");


                m_RequestStop.Reset();
                m_ExportingThread.Start();

                
                return true;
            }

            public Boolean IsWorking
            {
                get { return m_Busy; }
                set
                {
                    if ((value) || !m_Busy) 
                    {
                        return;
                    }

                    m_RequestStop.Set();
                }
            }

            private TimeSpan m_TimeLeft = TimeSpan.Zero;

            public TimeSpan TimeLeft
            {
                get { return m_TimeLeft; }
            }

            private void DoWork()
            {
                Int32 tLastPercent = 0;
                DateTime tStartTime = DateTime.Now;
                do
                {
                    //! begin 
                    if (!OnBeginExport(m_Path, m_SamplingPlan))
                    {
                        break;
                    }

                    System.Int32 tRowCount = m_Parent.GetRowCount(m_SamplingPlan);
                    if (-1 == tRowCount)
                    {
                        break;
                    }

                    //! output each row
                    for (Int32 n = 0; n < tRowCount; n++)
                    {
                        if (m_RequestStop.WaitOne(0, true))
                        {
                            //! raising report evetn
                            OnExportDatabaseReport(EXPORT_DATABASE_REPORT.DB_CANCELLED, tLastPercent, TimeSpan.Zero);
                            
                            m_Busy = false;
                            return;
                        }

                        //! get row
                        Object[] tItems = m_Parent.Get(n, m_SamplingPlan);
                        if (null == tItems)
                        {
                            continue;
                        }
                        if (tItems.Length != m_Parent.Columns.Count + 3)
                        {
                            continue;
                        }
                        //! raising evet
                        if (!OnRequestWriteRow(tItems, m_Parent.Columns))
                        {
                            //! failed to write row
                            continue;
                        }

                        if (null != ExportDatabaseReportEvent)
                        {
                            Int32 tCurrentPercent = n * 100 / tRowCount;

                            if (tCurrentPercent > tLastPercent)
                            {
                                tLastPercent = tCurrentPercent;

                                m_TimeLeft = TimeSpan.FromSeconds(((DateTime.Now - tStartTime).TotalSeconds / (double)tCurrentPercent) * (100 - tCurrentPercent));

                                //! raising report evetn
                                OnExportDatabaseReport(EXPORT_DATABASE_REPORT.DB_EXPORTING, tCurrentPercent, m_TimeLeft);
                            }
                        }
                    }

                    //! end
                    if (!OnEndExport(m_Path, m_SamplingPlan))
                    {
                        break;
                    }
                    else
                    {

                        //! raising event
                        OnExportDatabaseReport(EXPORT_DATABASE_REPORT.DB_COMPLETE, 100, TimeSpan.Zero);

                        m_Busy = false;
                        return;
                    }
                }
                while (false);

                //! raising event
                OnExportDatabaseReport(EXPORT_DATABASE_REPORT.DB_FAILED, -1, TimeSpan.Zero);

                //! release busy flag
                m_Busy = false;
            }


            ~DatabaseExporter()
            {
                Dispose();
            }

            #region IDisposable Members

            private Boolean m_bDisposed = false;

            public Boolean Disposed
            {
                get { return m_bDisposed; }
            }

            public void Dispose()
            {
                if (!m_bDisposed)
                {
                    m_bDisposed = true;

                    try
                    {
                        if (null != m_ExportingThread)
                        {
                            if (m_ExportingThread.IsAlive)
                            {
                                m_RequestStop.Set();
                                m_ExportingThread.Join();
                            }
                            m_ExportingThread = null;
                        }
                    }
                    catch (Exception Err)
                    {
                        Err.ToString();
                    }

                    GC.SuppressFinalize(this);
                }
            }

            #endregion
        }
    }
}
