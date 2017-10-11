using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Component;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace ESnail.Documents.Database
{
    public class BMDBTXTServer : BMDataLog
    {
        private Boolean m_bConnected = false;
        private DataTable m_OriginalDataTable = null;
        private Boolean m_bAvailable = false;
        private FileStream m_FileStream = null;
        private Int32 m_RowCount = 0;
        private String m_CurrentSampleName = null;
        private StreamWriter m_Writer = null;
        private DateTime m_ConnectTime = DateTime.Now;
        private Object m_Signal = new Object();
        //! \brief property for checking whether txtserver is connected or not
        public override Boolean Connected
        {
            get 
            {
                lock (m_Signal)
                {
                    return m_bConnected;
                }
            }
        }

        //! \brief dispose this object
        protected override void _Dispose()
        {
            Close();
        }

        //! \brief get new row
        public override DataRow NewRow()
        {
            lock (m_Signal)
            {
                if (null != m_OriginalDataTable)
                {
                    return m_OriginalDataTable.NewRow();
                }

                return null;
            }
        }

        //! \brief check whether this object is available
        public override Boolean Available
        {
            get 
            {
                lock (m_Signal)
                {
                    return m_bAvailable;
                }
            }
        }

        //! \brief reserved
        public override Object Get(int tRowIndex, int tColumnIndex, string tSampleName)
        {
            return null;
        }

        //! \brief reserved
        public override Object Get(int tRowIndex, int tColumnIndex)
        {
            return null;
        }

        //! \brief get current row count
        public override Int32 RowCount
        {
            get
            {
                lock (m_Signal)
                { return m_RowCount; }
            }
        }

        //! \brief reserved
        public override Int32 GetRowCount(string tSampleName)
        {
            return -1;
        }

        //! \brief get columns
        public override DataColumnCollection Columns
        {
            get
            {
                lock (m_Signal)
                {
                    if (null == m_OriginalDataTable)
                    {
                        return null;
                    }

                    return m_OriginalDataTable.Columns;
                }
            }
        }

        //! \brief reserved
        public override String[] Samples
        {
            get { return null; }
        }

        //! \brief get current sampling name 
        public override string CurrentSamplingName
        {
            get
            {
                lock (m_Signal)
                { return m_CurrentSampleName; }
            }
        }

        //! \brief get datatable
        public override DataTable DataTable
        {
            get
            {
                lock (m_Signal)
                { return m_OriginalDataTable; }
            }
        }

        //! \brief reserved
        public override Object[] Get(int tRowIndex, string tSamplingName)
        {
            return null;
        }

        //! \brief reserved
        public override Object[] Get(int tRowIndex)
        {
            return null;
        }

        private String m_DefaultPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "Documents");

        public override String DefaultFilePath
        {
            get
            {
                lock (m_Signal)
                { return m_DefaultPath; }
            }
            set
            {
                lock (m_Signal)
                {
                    if (m_bConnected)
                    {
                        return;
                    }

                    if (System.IO.Directory.Exists(value))
                    {
                        m_DefaultPath = value;
                    }
                }
            }
        }

        //! \brief connect
        public override Boolean Connect(DataTable tDataTable, String tSampleName)
        {
            lock (m_Signal)
            {
                if (m_bConnected)
                {
                    return true;
                }
                m_bConnected = true;

                if (null == tDataTable)
                {
                    m_bConnected = false;
                    m_bAvailable = false;
                    return false;
                }

                m_OriginalDataTable = tDataTable;
                m_CurrentSampleName = tSampleName;
                if (null == m_CurrentSampleName)
                {
                    m_CurrentSampleName = "Temporary";
                }
                else if ("" == m_CurrentSampleName.Trim())
                {
                    m_CurrentSampleName = "Temporary";
                }

                if (!Directory.Exists(m_DefaultPath))
                {
                    Directory.CreateDirectory(m_DefaultPath);
                }

                m_ConnectTime = DateTime.Now;

                if (!OpenFile())
                {
                    Close();
                    return false;
                }

                //! write columns
                try
                {
                    m_Writer.Write("Number\t");
                    m_Writer.Write("Time Stamp\t");

                    foreach (DataColumn tColumn in m_OriginalDataTable.Columns)
                    {
                        m_Writer.Write(tColumn.ColumnName);
                        m_Writer.Write('\t');
                    }

                }
                catch (Exception)
                {
                    Close();
                    return false;
                }

                m_bAvailable = true;

                return true;
            }
        }

        //! \brief close current connection
        public override void Close()
        {
            try
            {
                if (null != m_Writer)
                {
                    m_Writer.Close();
                    m_Writer.Dispose();
                }
            }
            catch (Exception) { }
            finally
            {
                m_Writer = null;
            }
            //! \brief close filestream
            if (null != m_FileStream)
            {
                try
                {
                    m_FileStream.Close();
                }
                catch (Exception) { }

                m_FileStream.Dispose();
                m_FileStream = null;
            }

            m_bConnected = false;
            m_bAvailable = false;
        }

        //! \brief write records to log file
        public override Boolean Add(DataRow tRow, DateTime tTime)
        {
            if ((!m_bAvailable) || (!m_bConnected) || (null == m_FileStream))
            {
                return false;
            }
            if (null == tRow)
            {
                return false;
            }

            try
            {
                //! start a new line
                m_Writer.Write(m_Writer.NewLine);

                //! row number
                m_Writer.Write(m_RowCount.ToString("D6"));
                m_Writer.Write('\t');

                //! time stamp
                m_Writer.Write(tTime.ToShortDateString());
                m_Writer.Write(' ');
                m_Writer.Write(tTime.ToShortTimeString());                    

                
                //! write each columns
                for (Int32 n = 0; n < tRow.ItemArray.Length;n++ )
                {
                    Object tObj = tRow[n];
                    m_Writer.Write('\t');
                    if (tObj is Byte)
                    {
                        m_Writer.Write(((Byte)tObj).ToString("X2"));
                    }
                    else if (tObj is UInt16)
                    {
                        m_Writer.Write(((UInt16)tObj).ToString("X4"));
                    }
                    else if (tObj is UInt32)
                    {
                        m_Writer.Write(((UInt32)tObj).ToString("X8"));
                    }
                    else if (tObj is UInt64)
                    {
                        m_Writer.Write(((UInt64)tObj).ToString("X16"));
                    }
                    else if (null == tObj)
                    {
                        m_Writer.Write("");
                    }
                    else
                    {
                        m_Writer.Write(tObj.ToString());
                    }
                }

                m_Writer.Close();
                m_Writer.Dispose();
                m_FileStream.Dispose();

                m_RowCount++;

                if (!OpenFile())
                {
                    Close();
                    return false;
                }

                if ((0 == (m_RowCount % 60000)) && (0 != m_RowCount))
                {
                    //! write columns
                    try
                    {
                        //! try to write columns
                        m_Writer.Write("Number\t");
                        m_Writer.Write("Time Stamp\t");

                        foreach (DataColumn tColumn in m_OriginalDataTable.Columns)
                        {
                            m_Writer.Write(tColumn.ColumnName);
                            m_Writer.Write('\t');
                        }

                    }
                    catch (Exception )
                    {
                        try
                        {
                            m_Writer.Close();
                            m_FileStream.Close();
                        }
                        catch (Exception) { }
                        finally
                        {
                            m_Writer.Dispose();
                            m_FileStream.Dispose();
                            m_FileStream = null;
                        }
                    }
                }
            }
            catch (Exception )
            {
                Close();
            }

            return true;
        }

        private Boolean OpenFile()
        {
            String tDirectory = Path.Combine(m_CurrentSampleName, m_ConnectTime.ToString("dd-MM-yyyy"));
            tDirectory = Path.Combine(tDirectory, m_ConnectTime.ToString("hh-mm-ss"));
            tDirectory = Path.Combine(m_DefaultPath, tDirectory);
            try
            {
                if (!Directory.Exists(tDirectory))
                {
                    Directory.CreateDirectory(tDirectory);
                }
            }
            catch (Exception )
            {
                return false;
            }

            //! try to open a log file
            try
            {
                String tPath = (m_RowCount / 60000).ToString("D2").Trim() + ".log";
                tPath = Path.Combine(tDirectory, tPath);

                m_FileStream = new FileStream(tPath, FileMode.Append, FileAccess.Write, FileShare.Read);
                m_Writer = new StreamWriter(m_FileStream);
            }
            catch (Exception )
            {
                m_FileStream = null;

                return false;
            }

            return true;
        }

        public override Boolean Add(Object[] tRowItems)
        {
            if ((!m_bAvailable) || (!m_bConnected) || (null == m_FileStream))
            {
                return false;
            }
            if (null == tRowItems)
            {
                return false;
            }
            if (tRowItems.Length < 3)
            {
                return false;
            }

            try
            {
                //! start a new line
                m_Writer.Write(m_Writer.NewLine);

                if (tRowItems[1] is Int32)
                {
                    //! row number
                    m_Writer.Write(((Int32)tRowItems[1]).ToString("D6"));
                    m_Writer.Write('\t');
                }
                else
                {
                    return false;
                }

                if (tRowItems[2] is DateTime)
                {
                    //! time stamp
                    m_Writer.Write(((DateTime)tRowItems[2]).ToShortDateString());
                    m_Writer.Write(' ');
                    m_Writer.Write(((DateTime)tRowItems[2]).ToShortTimeString());
                }
                else
                {
                    return false;
                }

                //! write each columns
                for (Int32 n = 3; n < tRowItems.Length; n++)
                {
                    Object tObj = tRowItems[n];
                    m_Writer.Write('\t');
                    if (tObj is Byte)
                    {
                        m_Writer.Write(((Byte)tObj).ToString("X2"));
                    }
                    else if (tObj is UInt16)
                    {
                        m_Writer.Write(((UInt16)tObj).ToString("X4"));
                    }
                    else if (tObj is UInt32)
                    {
                        m_Writer.Write(((UInt32)tObj).ToString("X8"));
                    }
                    else if (tObj is UInt64)
                    {
                        m_Writer.Write(((UInt64)tObj).ToString("X16"));
                    }
                    else if (null == tObj)
                    {
                        m_Writer.Write("");
                    }
                    else
                    {
                        m_Writer.Write(tObj.ToString());
                    }
                }

                m_RowCount++;
                if ((0 == (m_RowCount % 60000)) && (0 != m_RowCount))
                {
                    if (!OpenFile())
                    {
                        Close();
                        return false;
                    }

                    //! write columns
                    try
                    {
                        //! try to write columns
                        m_Writer.Write("Number\t");
                        m_Writer.Write("Time Stamp\t");

                        foreach (DataColumn tColumn in m_OriginalDataTable.Columns)
                        {
                            m_Writer.Write(tColumn.ColumnName);
                            m_Writer.Write('\t');
                        }
                    }
                    catch (Exception )
                    {
                        try
                        {
                            m_Writer.Close();
                            m_FileStream.Close();
                        }
                        catch (Exception) { }
                        finally
                        {
                            m_Writer.Dispose();
                            m_FileStream.Dispose();
                            m_FileStream = null;
                        }
                    }
                }
            }
            catch (Exception )
            {
                Close();
            }

            return true;
        }

    }
}
