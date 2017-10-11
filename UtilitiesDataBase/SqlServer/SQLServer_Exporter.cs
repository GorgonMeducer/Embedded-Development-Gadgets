using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ESnail.Component;
using ESnail.Utilities.Windows.Forms.Interfaces;
using ESnail.Utilities.Threading;
using ESnail.Utilities;
using System.Data;
using System.IO;

namespace ESnail.Documents.Database
{
    
    public class BMDatabaseExporter : IDisposable
    {
        private BMDBSQLServer.IDataBaseExporter m_DBExpoter = null;
        private IBMDataLog m_DBServerAgent = null;
        private BMDBSQLServer m_DBServer = null;
        private Boolean m_Available = false;
        private SafeInvoker m_Invoker = new SafeInvoker();
        private Boolean m_Working = false;

        //! \brief default constructor
        public BMDatabaseExporter()
        {
            Initialize();
        }

        //! \brief constructor with source database and exporter interface
        public BMDatabaseExporter(BMDBSQLServer tDBServer, IBMDataLog tExportAgent)
        {
            m_DBServer = tDBServer;
            m_DBServerAgent = tExportAgent;

            Initialize();
        }


        private void Initialize()
        {
            //! initialize exporter
            if (null == m_DBServer)
            {
                return;
            }
            if ((null == m_DBServerAgent) && !(m_DBServerAgent is IBMDataExpoter))
            {
                return;
            }

            BMDBSQLServer.IDataBaseExporter tExpoter = m_DBServer.NewExporter();
            if (null == tExpoter)
            {
                return;    
            }

            m_DBServer.RegisterExporter(tExpoter);
            tExpoter.EndExportEvent += new BMDBSQLServer.EndExport(EndExportEvent);
            tExpoter.BeginExportEvent += new BMDBSQLServer.BeginExport(BeginExportEvent);
            tExpoter.ExportDatabaseReportEvent += new BMDBSQLServer.ExportDatabaseReport(ExportDatabaseReportEvent);
            tExpoter.RequestWriteRowEvent += new BMDBSQLServer.RequestWriteRow(RequestWriteRowEvent);

            m_DBExpoter = tExpoter;

            m_Working = false;

            m_Available = true;
        }

        //! \brief get exporter
        public BMDBSQLServer.IDataBaseExporter Exporter
        {
            get 
            {
                if (m_Available)
                {
                    return m_DBExpoter;
                }

                return null;
            }
        }

        //! \brief available
        public Boolean Available
        {
            get { return m_Available; }
        }

        //! \brief get busy state
        public Boolean IsBusy
        {
            get { return m_Working; }
            set
            {
                if ((value) || !m_Working)
                {
                    return;
                }

                if (null != m_DBExpoter)
                {
                    m_DBExpoter.IsWorking = value;
                }
            }
            
        }

        //! \brief get/set data source
        public BMDBSQLServer Source
        {
            get 
            {
                if (m_Available)
                {
                    return m_DBServer;
                }

                return null;
            }
            set
            {
                if ((null == value) || m_Working)
                {
                    return;
                }
                if (!value.Available)
                {
                    return;
                }
                m_DBServer = value;

                Initialize();
            }
        }

        //! \brief get/set exporting agent
        public IBMDataLog ExportAgent
        {
            get 
            {
                if (m_Available)
                {
                    return m_DBServerAgent;
                }

                return null;
            }
            set
            {
                if ((null == value) || m_Working)
                {
                    return;
                }
                if (!(value is IBMDataExpoter))
                {
                    return;
                }

                m_DBServerAgent = value;

                Initialize();
            }

        }

        //! \brief request exporting specified data base
        public Boolean RequestExportDataBase(String tPath, String tSamplingPlan)
        {
            if (!m_Available)
            {
                return false;
            }
            if (null == m_DBExpoter)
            {
                return false;
            }

            m_Working = m_DBExpoter.TryToExportDatabase(tPath, tSamplingPlan);

            return m_Working;
        }

        public event BMDBSQLServer.ExportDatabaseReport ExportDatabaseReport;

        private void OnExportDatabaseReport(BMDBSQLServer.EXPORT_DATABASE_REPORT tResult, Int32 tPercent, TimeSpan tTimeLeft)
        {
            try
            {
                if (null != ExportDatabaseReport)
                {
                    m_Invoker.BeginInvoke(ExportDatabaseReport, tResult, tPercent,tTimeLeft);
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        private void ExportDatabaseReportEvent(BMDBSQLServer.EXPORT_DATABASE_REPORT tResult, Int32 tPercent, TimeSpan tTimeLeft)
        {
            switch (tResult)
            {
                case BMDBSQLServer.EXPORT_DATABASE_REPORT.DB_FAILED:
                case BMDBSQLServer.EXPORT_DATABASE_REPORT.DB_CANCELLED:
                case BMDBSQLServer.EXPORT_DATABASE_REPORT.DB_COMPLETE:
                    m_Working = false;
                    break;
                case BMDBSQLServer.EXPORT_DATABASE_REPORT.DB_EXPORTING:
                    m_Working = true;
                    break;
            }

            OnExportDatabaseReport(tResult, tPercent, tTimeLeft);
        }

        private Boolean BeginExportEvent(String tPath, String tSamplingName)
        {
            IBMDataExpoter tExporter = m_DBServerAgent as IBMDataExpoter;
            if (null != tExporter)
            {
                String tFilePath = tPath.Substring(0, tPath.IndexOf(System.IO.Path.GetFileName(tPath)));
                if (!System.IO.Directory.Exists(tFilePath))
                {
                    return false;
                }
                tExporter.DefaultFilePath = tFilePath;

                return m_DBServerAgent.Connect(m_DBServer.DataTable, System.IO.Path.GetFileNameWithoutExtension(tPath));
            }

            return m_DBServerAgent.Connect(m_DBServer.DataTable, tSamplingName);
        }

        private Boolean EndExportEvent(String tPath, String tSamplingName)
        {
            m_DBServerAgent.Close();

            return true;
        }

        private Boolean RequestWriteRowEvent(Object[] tRowItems, DataColumnCollection tColumns)
        {
            IBMDataExpoter tExporter = m_DBServerAgent as IBMDataExpoter;
            if (null != tExporter)
            {
                return tExporter.Add(tRowItems);
            }

            return false;
        }

        ~BMDatabaseExporter()
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
                m_Available = false;
                m_Working = false;

                try
                {
                    //! unregister exporter
                    if (null != m_DBServer)
                    {
                        if (null != m_DBExpoter)
                        {
                            m_DBServer.UnregisterExporter(m_DBExpoter);
                        }
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
