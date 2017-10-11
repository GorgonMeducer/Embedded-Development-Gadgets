using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO.IsolatedStorage;
using System.Data;

namespace ESnail.Component
{

    //! \name scan type
    //! @{
    public enum BM_PARAMETER_SCAN_TYPE
    {
        BM_SCAN_ONCE,                   //!< only scan one time when initialize
        BM_SCAN_MANUAL,                 //!< manaully scan
        BM_SCAN_AUTO                    //!< autoscan
    }
    //! @}

    //! \name interface for logging control
    //! @{
    public interface IBMDataLogControl
    {
        //! setting/getting log enable flag state
        System.Boolean LogEnabled
        {
            get;
            set;
        }

        //! setting/getting parameter scan-type
        BM_PARAMETER_SCAN_TYPE ScanType
        {
            get;
            set;
        }
    }
    //! @}

    public interface IBMDataExpoter
    {
        String DefaultFilePath
        {
            get;
            set;
        }

        Boolean Add(Object[] tRowItems);
    }

    //! \name interface for logging
    //! @{
    public interface IBMDataLog : IBMDataExpoter
    {
        //! \brief check whether this interface is available
        Boolean Available
        {
            get;
        }

        //! \brief initialize this interface
        Boolean Connect(DataTable tDataTable, String tSampleName);

        DataTable DataTable
        {
            get;
        }

        //! \brief check whether this interface has been initialized
        Boolean Connected
        {
            get;
        }

        //! \brief get new row
        DataRow NewRow();

        //! \brief add new row to database
        Boolean Add(DataRow tRow, DateTime tTime);

        //! \brief get cell
        Object Get(Int32 tRowIndex, Int32 tColumnIndex, String tSampleName);

        //! \brief get row
        Object[] Get(Int32 tRowIndex, String tSamplingName);

        //! \brief get current row
        Object[] Get(Int32 tRowIndex);

        //! \brief get row
        Object Get(Int32 tRowIndex, Int32 tColumnIndex);

        DataColumnCollection Columns
        {
            get;
        }

        //! \brief close 
        void Close();

        //! get current sampling record count
        Int32 RowCount
        {
            get;
        }

        //! get specified sampling record count
        Int32 GetRowCount(String tSampleName);

        //! \brief record buffer maxsize
        Int32 BufferCapacity
        {
            get;
            set;
        }

        //! \brief refresh database
        void Refresh();

        //! \brief get all exist samples
        String[] Samples
        {
            get;
        }

        String CurrentSamplingName
        {
            get;
        }
    }
    //! @}

    

    //public interface IBMDataLog

    //! \name battery manage data logging class
    //! @{
    public abstract class BMDataLog : IDisposable, IBMDataLog
    {
        protected class RecordBufferItem
        {
            private Int32 m_RowIndex = -1;
            private Object[] m_Objects = null;
            private Boolean m_Available = false;
            private String m_SamplingName = null;

            public RecordBufferItem(Int32 tRowIndex, Object[] tObjects, String tSamplingName)
            {
                if (tRowIndex < 0)
                {
                    return;
                }
                if (null == tObjects)
                {
                    return;
                }
                if (0 == tObjects.Length)
                {
                    return;
                }

                if (null == tSamplingName)
                {
                    m_SamplingName = "Temporary";
                }
                else if ("" == tSamplingName.Trim())
                {
                    m_SamplingName = "Temporary";
                }
                else
                {
                    m_SamplingName = tSamplingName;
                }

                m_RowIndex = tRowIndex;
                m_Objects = tObjects;

                m_Available = true;
            }

            public String SamplingName
            {
                get { return m_SamplingName; }
            }

            public Boolean Available
            {
                get { return m_Available; }
            }

            public Int32 RowIndex
            {
                get 
                {
                    if (!m_Available)
                    {
                        return -1;
                    }
                    return m_RowIndex; 
                }
            }

            public Object[] Items
            {
                get
                {
                    if (!m_Available)
                    {
                        return null;
                    } 
                    
                    return m_Objects;
                }
            }
        }

        private SortedList<Int32, RecordBufferItem> m_BufferList = new SortedList<int, RecordBufferItem>();
        private Int32 m_BufferCount = 30;
        private Int32 m_Ticket = 0;

        public Int32 BufferCapacity
        {
            get {return m_BufferCount;}
            set 
            {
                if (value <= 0)
                {
                    m_BufferCount = 1;
                }
                else
                {
                    m_BufferCount = value;
                }
            }
        }

        public virtual void Refresh()
        {
            lock (((ICollection)m_BufferList).SyncRoot)
            {
                m_Ticket = 0;
                m_BufferList.Clear();
            }
        }


        protected void AddBufferList(RecordBufferItem tNewItem)
        {
            if (!tNewItem.Available)
            {
                return;
            }

            if (m_Ticket == Int32.MaxValue)
            {
                Refresh();
            }

            lock (((ICollection)m_BufferList).SyncRoot)
            {
                if (0 == m_BufferList.Values.Count)
                {
                    m_BufferList.Add(m_Ticket++, tNewItem);
                }
                else
                {
                    if (tNewItem.SamplingName != m_BufferList.Values[0].SamplingName)
                    {
                        m_Ticket = 0;
                        m_BufferList.Clear();

                    }

                    m_BufferList.Add(m_Ticket++, tNewItem);

                    if (m_BufferList.Count >= m_BufferCount)
                    {
                        m_BufferList.RemoveAt(0);
                    }
                }
            }
        }

        private RecordBufferItem m_tLastRow = null;

        protected Object[] FindBuffer(Int32 tRowIndex, String tSamplingName)
        {
            if (-1 == tRowIndex)
            {
                return null;
            }

            if (null == tSamplingName)
            {
                tSamplingName = "Temporary";
            }
            else if ("" == tSamplingName.Trim())
            {
                tSamplingName = "Temporary";
            }

            lock (((ICollection)m_BufferList).SyncRoot)
            {
                if (0 == m_BufferList.Count)
                {
                    return null;
                }
                if (m_BufferList.Values[0].SamplingName != tSamplingName)
                {
                    return null;
                }

                if (null != m_tLastRow)
                {
                    if ((m_tLastRow.RowIndex == tRowIndex) && (m_tLastRow.SamplingName == tSamplingName))
                    {
                        return m_tLastRow.Items;
                    }
                }

                foreach (RecordBufferItem tItem in m_BufferList.Values)
                {
                    if ((tItem.RowIndex == tRowIndex) && (tItem.SamplingName == tSamplingName))
                    {
                        m_tLastRow = tItem;

                        return tItem.Items;
                    }
                }
            }

            return null;
        }

        #region IDisposable Members
        //! \brief destructor
        ~BMDataLog()
        {
            Dispose();
        }

        

        private System.Boolean m_bDisposed = false;

        //! \brief property for checking whether this object is disposed.
        public System.Boolean Disposed
        {
            get { return m_bDisposed; }
        }

        //! \brief method for disposing the object
        public void Dispose()
        {
            if (!m_bDisposed)
            {
                m_bDisposed = true;

                try
                {
                    _Dispose();
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }

                GC.SuppressFinalize(this);
            }
        }

        protected abstract void _Dispose();

        #endregion

        #region IBMDataLog Members

        public abstract Boolean Available
        {
            get;
        }

        public abstract Boolean Connect(DataTable tDataTable, String tSampleName);

        public abstract Boolean Connected
        {
            get;
        }
        public abstract void Close();

        public abstract DataRow NewRow();

        public abstract Boolean Add(DataRow tRow, DateTime tTime);


        public abstract Object Get(int tRowIndex, int tColumnIndex, string tSampleName);

        public abstract Object Get(int tRowIndex, int tColumnIndex);


        public abstract Int32 RowCount
        {
            get;
        }

        public abstract Int32 GetRowCount(string tSampleName);


        public abstract DataColumnCollection Columns
        {
            get;
        }

        public abstract String[] Samples
        {
            get;
        }

        public abstract String CurrentSamplingName
        {
            get;
        }


        public abstract DataTable DataTable
        {
            get;
        }

        public abstract Object[] Get(Int32 tRowIndex, String tSamplingName);

        public abstract Object[] Get(Int32 tRowIndex);

        #endregion

        #region IBMDataExpoter Members

        public abstract String DefaultFilePath
        {
            get;
            set;
        }

        public abstract Boolean Add(object[] tRowItems);

        #endregion
    }
    //! @}
}
