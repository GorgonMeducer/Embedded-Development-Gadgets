using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities
{

    

    public abstract class ESOneTimeInitialisedObject
    {
        private Boolean m_bInitialised = false;

        public Boolean Initialised
        {
            get { return m_bInitialised; }
            set
            {
                if (value)
                {
                    m_bInitialised = true;
                }
            }
        }

    }

    public enum DATA_SIZE
    {
        DATA_SIZE_BYTE = 0,
        DATA_SIZE_HALF_WORD, 
        DATA_SIZE_WORD,
        DATA_SIZE_DOUBLE_WORD
    }

    public abstract class ESDisposableClass : IDisposable
    {
        protected Boolean m_bDisposed = false;                    //!< disposing flag

        ~ESDisposableClass()
        {
            Dispose();
        }

        //! \brief property for getting disposing state
        public Boolean Disposed
        {
            get { return m_bDisposed; }
        }

        public virtual void Dispose()
        {
            if (!m_bDisposed)
            {
                m_bDisposed = true;

                try
                {
                    _Dispose();
                }
                catch (Exception) { }

                GC.SuppressFinalize(this);
            }
        }

        protected abstract void _Dispose();
    }


    public abstract class ESDisposableOneTimeInitialisedObject : ESDisposableClass
    {
        private Boolean m_bInitialised = false;

        public Boolean Initialised
        {
            get { return m_bInitialised; }
            set
            {
                if (value)
                {
                    m_bInitialised = true;
                }
            }
        }
    }

    public interface IStatusReporter
    {
        Boolean IsError
        {
            get;
        }

        String ErrorInfo
        {
            get;
        }

        event ESStatusReporter.ReporterHandler StatusReport;
    }

    public class ESStatusReporter : IStatusReporter
    {

        #region Status Report

        public enum WorkingStatus : int
        {
            ERROR = -1,
            PROGRESS_REPORT,
            WARNING,
            COMPLETE
        };

        public delegate Boolean ReporterHandler(WorkingStatus tStatus, String tInfo);
        public event ReporterHandler StatusReport;

        private Boolean m_IsError = false;

        private String m_ErrorInfo = "";

        public Boolean IsError
        {
            get { return m_IsError; }
        }

        public String ErrorInfo
        {
            get { return m_ErrorInfo; }
        }



        /*! \brief Report error info and get decision about whether retry or not
         *  \retval true retry
         *  \retval false terminate the process
         */
        public virtual Boolean ReportError(String tErrorInfo)
        {
            Boolean tResult = false;
            m_ErrorInfo = tErrorInfo;
            m_IsError = (tErrorInfo == null);

            if (StatusReport != null)
            {
                try
                {
                    tResult = StatusReport.Invoke(WorkingStatus.ERROR, tErrorInfo);
                }
                catch (Exception)
                {
                }
            }

            return tResult;
        }

        public virtual Boolean ReportStatus(WorkingStatus tStatus, String tInfo)
        {
            if (tStatus == WorkingStatus.ERROR)
            {
                return ReportError(tInfo);
                
            }

            if (StatusReport != null)
            {
                try
                {
                    return StatusReport.Invoke(tStatus, tInfo);
                }
                catch (Exception)
                {
                }
            }

            return true;
        }


        #endregion

    }
}
