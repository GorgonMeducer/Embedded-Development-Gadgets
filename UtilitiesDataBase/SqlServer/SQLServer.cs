using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using ESnail.Component;
using System.IO;
using ESnail.Utilities.IO;
using System.Windows.Forms;

namespace ESnail.Documents.Database
{
    public partial class BMDBSQLServer : BMDataLog
    {
        private DataTable m_DataTable = null;
        private Boolean m_bAvailable = false;
        private Boolean m_bConnected = false;

        /* @"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;" */

        private String c_DBConnectionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename=|DataDirectory|ATBMDB.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
        private String m_DBQueryString = null;
        private DataColumnMapping[] m_ColumnMappings = null;
        //private DataRow m_RowTemplate = null;
        private DataTable m_OriginalDataTable = null;
        private SqlConnection m_Connection = null;
        private SqlDataAdapter m_DataAdapter = null;
        private String m_SampleName = null;
        private String[] m_ColumnStrings = null;
        private String m_Path = ".\\";

        //! \brief dispose this object
        protected override void _Dispose()
        {
            Close();

            if (null != m_OriginalDataTable)
            {
                m_OriginalDataTable.Dispose();
                m_OriginalDataTable = null;
            }

            if (null != m_ExporterSet)
            {
                foreach (DatabaseExporter tExporter in m_ExporterSet.Items)
                {
                    tExporter.Dispose();
                    m_ExporterSet.Remove(tExporter.ID);
                }
            }
        }        
        
        //! \brief get available state
        public override Boolean Available
        {
            get { return m_bAvailable; }
        }

        //! \brief property for checking whether this interface is initialized
        public override Boolean Connected
        {
            get { return m_bConnected; }
        }

        //! \brief initialize this object
        public override Boolean Connect(DataTable tDataTable, String tSampleName)
        {
            if (m_bConnected)
            {
                return true;
            }
            m_bConnected = true;

            if (null == tDataTable)
            {
                m_bAvailable = false;
                return false;
            }

            m_DataTable = new DataTable(tDataTable.TableName, tDataTable.Namespace);
            m_OriginalDataTable = tDataTable.Clone();

            m_SampleName = tSampleName;
            if (null == m_SampleName)
            {
                m_SampleName = "Temporary";
            }
            else if ("" == m_SampleName.Trim())
            {
                m_SampleName = "Temporary";
            }

            List<String> tColumnStringList = new List<string>();
            do
            {
                DataColumn tNewColumn = new DataColumn("Sampling Name");
                
                tNewColumn.DataType = typeof(String);
                tNewColumn.AllowDBNull = true;
                tNewColumn.ReadOnly = true;

                tColumnStringList.Add("Sampling Name");
                m_DataTable.Columns.Add(tNewColumn);
            }
            while (false);

            do
            {
                DataColumn tNewColumn = new DataColumn("Number");
                tNewColumn.DataType = typeof(Int32);
                tNewColumn.AllowDBNull = false;
                tNewColumn.ReadOnly = true;
                tNewColumn.AutoIncrement = true;
                tNewColumn.AutoIncrementSeed = 0;
                tNewColumn.AutoIncrementStep = 1;

                tColumnStringList.Add("Number");
                m_DataTable.Columns.Add(tNewColumn);
            }
            while (false);

            do
            {
                DataColumn tNewColumn = new DataColumn("Time Stamp");
                tNewColumn.DataType = typeof(DateTime);
                tNewColumn.AllowDBNull = false;
                tNewColumn.ReadOnly = true;

                tColumnStringList.Add("Time Stamp");
                m_DataTable.Columns.Add(tNewColumn);
            }
            while (false);

            /*
            do
            {
                DataColumn tNewColumn = new DataColumn("Time Elapsed");
                tNewColumn.DataType = typeof(Int32);
                tNewColumn.AllowDBNull = false;
                tNewColumn.ReadOnly = true;

                tColumnStringList.Add("Time Elapsed");
                m_DataTable.Columns.Add(tNewColumn);
            }
            while (false);*/
            

            StringBuilder sbColumns = new StringBuilder();
            

            sbColumns.Append("SELECT ");
            sbColumns.Append("[Sampling Name], ");
            sbColumns.Append("Number, ");
            sbColumns.Append("[Time Stamp]");

            

            //! create columns mapping
            do
            {
                //! get columns
                DataColumnCollection tColumns = tDataTable.Columns;
                if (null == tColumns)
                {
                    break;
                }

                List<DataColumnMapping> tColumnMappingList = new List<DataColumnMapping>();

                for (Int32 n = 0;n < tColumns.Count;n++)
                {
                    DataColumnMapping tMapping = new DataColumnMapping((n + 1).ToString(), tColumns[n].ColumnName);
                    tColumnMappingList.Add(tMapping);

                    DataColumn tNewColumn = new DataColumn(tColumns[n].ColumnName);
                    tNewColumn.AllowDBNull = true;
                    tNewColumn.DataType = typeof(Object);
                    m_DataTable.Columns.Add(tNewColumn);

                    sbColumns.Append(", [");
                    sbColumns.Append((n+1).ToString().Trim());
                    sbColumns.Append("]");

                    do
                    {
                        StringBuilder sbColumnsString = new StringBuilder();
                        //sbColumnsString.Append("[");
                        sbColumnsString.Append((n + 1).ToString().Trim());
                        //sbColumnsString.Append("]");

                        tColumnStringList.Add(sbColumnsString.ToString());
                    }
                    while (false);
                }

                //! get mapping array
                m_ColumnMappings = tColumnMappingList.ToArray();
            }
            while (false);

            m_ColumnStrings = tColumnStringList.ToArray();

            sbColumns.Append(" FROM Sampling");

            if (null != m_SampleName)
            {
                m_SampleName = m_SampleName.Trim();
                if ("" != m_SampleName)
                {
                    sbColumns.Append(" WHERE [Sampling Name] = '");
                    sbColumns.Append(m_SampleName);
                    sbColumns.Append("'");
                }
            }

            //! get query string
            m_DBQueryString = sbColumns.ToString();


            try
            {
                //! create connection
                m_Connection = new SqlConnection(c_DBConnectionString);

            }
            catch (Exception )
            {
                return false;
            }


            //! create data adapter
            m_DataAdapter = new SqlDataAdapter(m_DBQueryString, m_Connection);

            ITableMapping Mapping = m_DataAdapter.TableMappings.Add("Table","Sampling");
            foreach (DataColumnMapping tColumnMapping in m_ColumnMappings)
            {
                Mapping.ColumnMappings.Add(tColumnMapping);
            }

            //! create insert command
            do
            {
                StringBuilder sbSQLInsertString = new StringBuilder();
                sbSQLInsertString.Append("INSERT INTO Sampling ( [");
                sbSQLInsertString.Append(m_ColumnStrings[0]);
                sbSQLInsertString.Append("]");
                for (Int32 n = 1; n < m_ColumnStrings.Length; n++)
                {
                    sbSQLInsertString.Append(", [");

                    sbSQLInsertString.Append(m_ColumnStrings[n]);
                    sbSQLInsertString.Append("]");
                }
                sbSQLInsertString.Append(" ) VALUES ( @");
                sbSQLInsertString.Append(m_ColumnStrings[0].Replace(' ', '_'));
                for (Int32 n = 1; n < m_ColumnStrings.Length; n++)
                {
                    sbSQLInsertString.Append(", @");
                    sbSQLInsertString.Append(m_ColumnStrings[n].Replace(' ', '_'));
                }
                sbSQLInsertString.Append(" )");

                //! initialize insert command
                SqlCommand tCommand = new SqlCommand(sbSQLInsertString.ToString(), m_Connection);

                tCommand.Parameters.Add(new SqlParameter("@Sampling_Name", SqlDbType.VarChar, 0, ParameterDirection.Input, 0, 0, "Sampling Name", DataRowVersion.Current, false, null, "", "", ""));

                tCommand.Parameters.Add(new SqlParameter("@Number", SqlDbType.Int, 0, ParameterDirection.Input, 0, 0, "Number", DataRowVersion.Current, false, null, "", "", ""));

                tCommand.Parameters.Add(new SqlParameter("@Time_Stamp", SqlDbType.DateTime, 0, ParameterDirection.Input, 0, 0, "Time Stamp", DataRowVersion.Current, false, null, "", "", ""));

                for (Int32 n = 3; n < m_ColumnStrings.Length; n++)
                {
                    tCommand.Parameters.Add("@" + m_ColumnStrings[n], SqlDbType.Variant, 0, m_DataTable.Columns[n].ColumnName);
                }

                m_DataAdapter.InsertCommand = tCommand;
                m_DataAdapter.InsertCommand.CommandType = CommandType.Text;
            }
            while (false);

            

            //! try to connect to database
            do
            {
                
                //! try to record sampling plan
                do
                {
                    if (FindSampling(m_SampleName) < 0)
                    {

                        if (!InsertSampling(m_SampleName))
                        {
                            m_bAvailable = false;
                            return false;
                        }
                    }

                }
                while (false);

                Int32 tRecordNumber = GetDatabaseRecordCount(m_SampleName);
                if (tRecordNumber < 0)
                {
                    m_bAvailable = false;
                    return false;
                }
                else if (0 == tRecordNumber)
                {
                    m_DataTable.Columns["Number"].AutoIncrementSeed = tRecordNumber;
                }
                else
                {
                    m_DataTable.Columns["Number"].AutoIncrementSeed = tRecordNumber + 1;
                }
                m_RowCount = tRecordNumber;
            }
            while (false);

            m_bAvailable = true;

            //m_LastTime = DateTime.Now;

            return true;
        }

        private Int32 GetDatabaseRecordCount(String tSampling)
        {
            if (null == tSampling)
            {
                tSampling = "Temporary";
            }
            else if ("" == tSampling.Trim())
            {
                tSampling = "Temporary";
            }

            Int32 tResult = -1;
            String queryString = "SELECT [Sampling ID], [RecordCount] FROM SamplingRecord WHERE [Sampling Name] = '" + tSampling + "'";

            try
            {
                using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(queryString, tConnection))
                    {
                        try
                        {
                            tConnection.Open();

                            using (SqlDataReader reader = tCommand.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        tResult = reader.GetInt32(1);
                                    }
                                }

                                reader.Close();
                            }
                        }
                        catch (Exception) { }
                        finally
                        {
                            tConnection.Close();
                        }
                    }
                }
            }
            catch (Exception )
            {
                return tResult;
            }

            return tResult;
        }


        public override void Close()
        {
            

            if (null != m_Connection)
            {
                try
                {
                    m_Connection.Close();
                    m_Connection.Dispose();
                    m_Connection = null;
                }
                catch (Exception) { }
            }

            if (null != m_DataAdapter)
            {
                m_DataAdapter.Dispose();
                m_DataAdapter = null;
                GC.Collect();
            }

            if (null != m_DataTable)
            {
                m_DataTable.Dispose();
                m_DataTable = null;
            }

            m_bConnected = false;
            m_bAvailable = false;
        }

        //! \brief get new row
        public override DataRow NewRow()
        {
            if (!m_bAvailable)
            {
                return null;
            }
            if (null == m_OriginalDataTable)
            {
                return null;
            }

            DataRow tRow = m_OriginalDataTable.NewRow();

            return tRow;

        }

        private Int32 m_RowCount = 0;
        //private DateTime m_LastTime = DateTime.Now;

        public Boolean AddRange(DataRow[] tRows, DateTime tTime)
        {
            if (null == tRows)
            {
                return false;
            }
            else if (0 == tRows.Length)
            {
                return false;
            }
            if (!m_bAvailable)
            {
                return false;
            }
            if (!m_bConnected)
            {
                return false;
            }
            if (null == m_DataTable)
            {
                return false;
            }

            List<DataRow> tRowList = new List<DataRow>();
            
            foreach (DataRow tRow in tRows)
            {
                if (m_OriginalDataTable.Columns.Count != tRow.ItemArray.Length)
                {
                    return false;
                }

                DataRow tInternalRow = m_DataTable.NewRow();
                tInternalRow["Sampling Name"] = m_SampleName;
                tInternalRow["Time Stamp"] = tTime;
                //tInternalRow["Time Elapsed"] = (tTime - m_LastTime).Seconds;
                //m_LastTime = tTime;

                for (Int32 n = 0; n < tRow.ItemArray.Length; n++)
                {
                    if (null == tRow[n])
                    {
                        continue;
                    }
                    DataColumn tSourceColumn = m_OriginalDataTable.Columns[n];
                    if (null == tSourceColumn)
                    {
                        return false;
                    }

                    DataColumn tTargetColumn = m_DataTable.Columns[tSourceColumn.ColumnName];
                    if (null == tTargetColumn)
                    {
                        return false;
                    }

                    //! check data type
                    if (tRow[n].GetType() != tSourceColumn.DataType)
                    {
                        continue;
                    }

                    if (tRow[n].GetType() == typeof(UInt16))
                    {
                        tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt16(BitConverter.GetBytes((UInt16)tRow[n]), 0);
                    }
                    else if (tRow[n].GetType() == typeof(UInt32))
                    {
                        tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt32(BitConverter.GetBytes((UInt32)tRow[n]), 0);
                    }
                    else if (tRow[n].GetType() == typeof(UInt64))
                    {
                        tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt64(BitConverter.GetBytes((UInt64)tRow[n]), 0);
                    }
                    else
                    {
                        tInternalRow[tTargetColumn.ColumnName] = tRow[n];
                    }
                }


                m_DataTable.Rows.Add(tInternalRow);
                tRowList.Add(tInternalRow);
            }

            try
            {
                m_DataAdapter.Update(tRowList.ToArray());
                m_RowCount += tRowList.Count ;

                UpdateSampling(m_SampleName, m_RowCount);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                m_DataTable.Rows.Clear();
                m_Connection.Close();
            }

            return true;
        }

        //! \brief add new row to database
        public override Boolean Add(DataRow tRow, DateTime tTime)
        {
            if (null == tRow)
            {
                return false;
            }
            if (!m_bAvailable)
            {
                return false;
            }
            if (!m_bConnected)
            {
                return false;
            }
            if (null == m_DataTable)
            {
                return false;
            }
            if (m_OriginalDataTable.Columns.Count != tRow.ItemArray.Length)
            {
                return false;
            }

            DataRow tInternalRow = m_DataTable.NewRow();
            tInternalRow["Sampling Name"] = m_SampleName;
            tInternalRow["Time Stamp"] = tTime;
            //tInternalRow["Time Elapsed"] = (tTime - m_LastTime).Seconds;
            //m_LastTime = tTime;
            
            for (Int32 n = 0; n < tRow.ItemArray.Length; n++)
            { 
                if (null == tRow[n])
                {
                    continue;
                }
                DataColumn tSourceColumn = m_OriginalDataTable.Columns[n];
                if (null == tSourceColumn)
                {
                    return false;
                }
                
                DataColumn tTargetColumn = m_DataTable.Columns[tSourceColumn.ColumnName];
                if (null == tTargetColumn)
                {
                    return false;
                }

                //! check data type
                if (tRow[n].GetType() != tSourceColumn.DataType)
                {
                    continue;
                }

                if (tRow[n].GetType() == typeof(UInt16))
                {
                    tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt16(BitConverter.GetBytes((UInt16)tRow[n]),0);
                }
                else if (tRow[n].GetType() == typeof(UInt32))
                {
                    tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt32(BitConverter.GetBytes((UInt32)tRow[n]), 0);
                }
                else if (tRow[n].GetType() == typeof(UInt64))
                {
                    tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt64(BitConverter.GetBytes((UInt64)tRow[n]), 0);
                }
                else
                {
                    tInternalRow[tTargetColumn.ColumnName] = tRow[n];
                }
            }

            
            m_DataTable.Rows.Add(tInternalRow);

            try
            {
                m_DataAdapter.Update(new DataRow[]{tInternalRow} );
                m_RowCount++;

                UpdateSampling(m_SampleName, m_RowCount);
            }
            catch (Exception )
            {
                return false;
            }
            finally
            {
                m_DataTable.Rows.Clear();
                m_Connection.Close();
            }

            return true;
        }


        public override Object Get(Int32 tRowIndex, Int32 tColumnIndex, String tSampleName)
        {
            Object tResult = null;
            
            if (/*(!Available) ||*/ (tRowIndex < 0) || (tColumnIndex < 0) ||(null == m_DBQueryString))
            {
                return null;
            }
            if ("" == m_DBQueryString.Trim())
            {
                return null;
            }
            

            if (null == tSampleName)
            {
                tSampleName = "Temporary";
            }
            else if ("" == tSampleName.Trim())
            {
                tSampleName = "Temporary";
            }

            if (null == m_OriginalDataTable)
            {
                return null;
            }
            tColumnIndex++;
            if (tColumnIndex >= (m_OriginalDataTable.Columns.Count+3))
            {
                return null;
            }

            Object[] tObjects = FindBuffer(tRowIndex, tSampleName);

            if (null != tObjects)
            { 
                if (tColumnIndex < 3)
                {
                    return tObjects[tColumnIndex];
                }
                else
                {
                    Object tData = tObjects[tColumnIndex];
                    Type tType = m_OriginalDataTable.Columns[tColumnIndex - 3].DataType;

                    tResult = tData;
                    return tResult;
                }
            }

            /*
            //! check record count
            Int32 tRecordCount = GetDatabaseRecordCount(tSampleName);
            if (tRowIndex >= tRecordCount)
            {
                return null;
            }
            */

            try
            {
                //! creat a connection
                using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                {
                    String tQueryString = m_DBQueryString.Substring(0, m_DBQueryString.IndexOf("WHERE"));

                    tQueryString += " WHERE ";

                    if ("" != tSampleName)
                    {
                        tQueryString += "[Sampling Name] = '";
                        tQueryString += tSampleName;
                        tQueryString += "' and ";
                    }
                    tQueryString += "Number = " + tRowIndex.ToString();

                    try
                    {
                        using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                        {
                            tConnection.Open();
                            using (System.Data.SqlClient.SqlDataReader tReader = tCommand.ExecuteReader())
                            {
                                if (!tReader.HasRows)
                                {
                                    tReader.Close();
                                    return null;
                                }

                                if (tReader.Read())
                                {
                                    List<Object> tObjectList = new List<object>();

                                    for (Int32 n = 0; n < (m_OriginalDataTable.Columns.Count + 3); n++)
                                    {
                                        Object tObj = tReader.GetValue(n);

                                        if (n < 3)
                                        {
                                            tObjectList.Add(tObj);
                                            continue;
                                        }

                                        Type tType = m_OriginalDataTable.Columns[n - 3].DataType;
                                        if (tType == typeof(UInt16))
                                        {

                                            if (tObj is Int16)
                                            {
                                                tObj = BitConverter.ToUInt16(BitConverter.GetBytes((Int16)tObj), 0);
                                            }

                                        }
                                        else if (tType == typeof(UInt32))
                                        {
                                            if (tObj is Int32)
                                            {
                                                tObj = BitConverter.ToUInt32(BitConverter.GetBytes((Int32)tObj), 0);
                                            }
                                        }
                                        else if (tType == typeof(UInt64))
                                        {
                                            if (tObj is Int64)
                                            {
                                                tObj = BitConverter.ToUInt64(BitConverter.GetBytes((Int64)tObj), 0);
                                            }
                                        }

                                        tObjectList.Add(tObj);
                                    }
                                    tReader.Close();


                                    Object[] tReaderObjects = tObjectList.ToArray();

                                    AddBufferList(new RecordBufferItem(tRowIndex, tReaderObjects, tSampleName));


                                    if (tColumnIndex < 3)
                                    {
                                        tResult = tReaderObjects[tColumnIndex];
                                    }
                                    else
                                    {
                                        return tReaderObjects[tColumnIndex];
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        tConnection.Close();
                    }
                }
            }
            catch (Exception )
            {
                return tResult;
            }

            return tResult;
        }

        public override Object[] Get(int tRowIndex)
        {
            return Get(tRowIndex, m_SampleName);
        }

        public List<Object[]> GetRange(int tRowIndex, int tCount, String tSampleName)
        {
            do
            {
                if (/*(!Available) ||*/ (tRowIndex < 0) || (null == m_DBQueryString) || (tCount <= 0))
                {
                    break;
                }
                if ("" == m_DBQueryString.Trim())
                {
                    break;
                }


                if (null == tSampleName)
                {
                    tSampleName = "Temporary";
                }
                else if ("" == tSampleName.Trim())
                {
                    tSampleName = "Temporary";
                }

                if (null == m_OriginalDataTable)
                {
                    break;
                }

                List<Object[]> tResultObjectList = new List<object[]>();

                //! find buffer first
                
                for (Int32 n = tCount; n > 0; n--)
                {
                    Object[] tObjects = FindBuffer(tRowIndex, tSampleName);

                    if (null == tObjects)
                    {
                        break;
                    }

                    tResultObjectList.Add(tObjects);

                    tRowIndex++;
                    tCount--;
                }

                do {

                    if (tCount == 0)
                    {
                        break;
                    }

                    SqlConnection tConnection = null;

                    //! read data base
                    try
                    {

                        //! creat a connection
                        using ( tConnection = new SqlConnection(c_DBConnectionString))
                        {
                            String tQueryStringTemplate = m_DBQueryString.Substring(0, m_DBQueryString.IndexOf("WHERE"));

                            tQueryStringTemplate += " WHERE ";

                            if ("" != tSampleName)
                            {
                                tQueryStringTemplate += "[Sampling Name] = '";
                                tQueryStringTemplate += tSampleName;
                                tQueryStringTemplate += "' and ";
                            }
                            tConnection.Open();
                            do
                            {
                                String tQueryString = tQueryStringTemplate + "Number = " + tRowIndex.ToString();

                                try
                                {
                                    

                                    using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                                    {
                                        
                                        using (System.Data.SqlClient.SqlDataReader tReader = tCommand.ExecuteReader())
                                        {
                                            if (!tReader.HasRows)
                                            {
                                                tReader.Close();
                                                break;
                                            }

                                            if (tReader.Read())
                                            {
                                                List<Object> tObjectList = new List<object>();

                                                for (Int32 n = 0; n < (m_OriginalDataTable.Columns.Count + 3); n++)
                                                {
                                                    Object tObj = tReader.GetValue(n);

                                                    if (n < 3)
                                                    {
                                                        tObjectList.Add(tObj);
                                                        continue;
                                                    }

                                                    Type tType = m_OriginalDataTable.Columns[n - 3].DataType;
                                                    if (tType == typeof(UInt16))
                                                    {

                                                        if (tObj is Int16)
                                                        {
                                                            tObj = BitConverter.ToUInt16(BitConverter.GetBytes((Int16)tObj), 0);
                                                        }

                                                    }
                                                    else if (tType == typeof(UInt32))
                                                    {
                                                        if (tObj is Int32)
                                                        {
                                                            tObj = BitConverter.ToUInt32(BitConverter.GetBytes((Int32)tObj), 0);
                                                        }
                                                    }
                                                    else if (tType == typeof(UInt64))
                                                    {
                                                        if (tObj is Int64)
                                                        {
                                                            tObj = BitConverter.ToUInt64(BitConverter.GetBytes((Int64)tObj), 0);
                                                        }
                                                    }

                                                    tObjectList.Add(tObj);
                                                }
                                                tReader.Close();


                                                Object[] tReaderObjects = tObjectList.ToArray();

                                                //AddBufferList(new RecordBufferItem(tRowIndex, tReaderObjects, tSampleName));

                                                tResultObjectList.Add(tReaderObjects);
                                            }

                                        }
                                    }
                                }
                                catch (Exception) { }

                                tRowIndex++;
                            } while (--tCount > 0);
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        if (null != tConnection)
                        {
                            tConnection.Close();
                        }
                    }



                } while (false);


                return tResultObjectList;
            }
            while (false);

            return null;
        }


        public override Object[] Get(int tRowIndex, string tSampleName)
        {
            if (/*(!Available) ||*/ (tRowIndex < 0)  || (null == m_DBQueryString))
            {
                return null;
            }
            if ("" == m_DBQueryString.Trim())
            {
                return null;
            }


            if (null == tSampleName)
            {
                tSampleName = "Temporary";
            }
            else if ("" == tSampleName.Trim())
            {
                tSampleName = "Temporary";
            }

            if (null == m_OriginalDataTable)
            {
                return null;
            }


            Object[] tObjects = FindBuffer(tRowIndex, tSampleName);

            if (null != tObjects)
            {
                return tObjects;
            }

            /*
            //! check record count
            Int32 tRecordCount = GetDatabaseRecordCount(tSampleName);
            if (tRowIndex >= tRecordCount)
            {
                return null;
            }
            */
            try
            {

                //! creat a connection
                using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                {
                    String tQueryString = m_DBQueryString.Substring(0, m_DBQueryString.IndexOf("WHERE"));

                    tQueryString += " WHERE ";

                    if ("" != tSampleName)
                    {
                        tQueryString += "[Sampling Name] = '";
                        tQueryString += tSampleName;
                        tQueryString += "' and ";
                    }
                    tQueryString += "Number = " + tRowIndex.ToString();

                    try
                    {
                        using (SqlCommand tCommand = new SqlCommand(tQueryString, tConnection))
                        {
                            tConnection.Open();
                            using (System.Data.SqlClient.SqlDataReader tReader = tCommand.ExecuteReader())
                            {
                                if (!tReader.HasRows)
                                {
                                    tReader.Close();
                                    return null;
                                }

                                if (tReader.Read())
                                {
                                    List<Object> tObjectList = new List<object>();

                                    for (Int32 n = 0; n < (m_OriginalDataTable.Columns.Count + 3); n++)
                                    {
                                        Object tObj = tReader.GetValue(n);

                                        if (n < 3)
                                        {
                                            tObjectList.Add(tObj);
                                            continue;
                                        }

                                        Type tType = m_OriginalDataTable.Columns[n - 3].DataType;
                                        if (tType == typeof(UInt16))
                                        {

                                            if (tObj is Int16)
                                            {
                                                tObj = BitConverter.ToUInt16(BitConverter.GetBytes((Int16)tObj), 0);
                                            }

                                        }
                                        else if (tType == typeof(UInt32))
                                        {
                                            if (tObj is Int32)
                                            {
                                                tObj = BitConverter.ToUInt32(BitConverter.GetBytes((Int32)tObj), 0);
                                            }
                                        }
                                        else if (tType == typeof(UInt64))
                                        {
                                            if (tObj is Int64)
                                            {
                                                tObj = BitConverter.ToUInt64(BitConverter.GetBytes((Int64)tObj), 0);
                                            }
                                        }

                                        tObjectList.Add(tObj);
                                    }
                                    tReader.Close();


                                    Object[] tReaderObjects = tObjectList.ToArray();

                                    AddBufferList(new RecordBufferItem(tRowIndex, tReaderObjects, tSampleName));

                                    return tReaderObjects;
                                }

                            }
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        tConnection.Close();
                    }
                }
            }
            catch (Exception) { }
                
            return null;
        }

        public override Object Get(int tRowIndex, int tColumnIndex)
        {
            return Get(tRowIndex, tColumnIndex, m_SampleName);
        }

        public override Int32 RowCount
        {
            get 
            {
                if (!m_bAvailable)
                {
                    return -1;
                }

                return m_RowCount;
                //return GetDatabaseRecordCount(m_SampleName);
            }
        }

        public override Int32 GetRowCount(String tSampleName)
        {
            if (null == tSampleName)
            {
                tSampleName = "Temporary";
            }
            else if ("" == tSampleName.Trim())
            {
                tSampleName = "Temporary";
            }

            return GetDatabaseRecordCount(tSampleName);
        }

        public override DataColumnCollection Columns
        {
            get 
            {
                if (null == m_OriginalDataTable)
                {
                    return null;
                }
                return m_OriginalDataTable.Columns; 
            }
        }

        private Int32 FindSampling(String tSampling)
        {
            if (null == tSampling)
            {
                tSampling = "Temporary";
            }
            else if ("" == tSampling.Trim())
            {
                tSampling = "Temporary";
            }

            Int32 tResult = -1;
            String queryString = "SELECT [Sampling ID] FROM SamplingRecord WHERE [Sampling Name] = '" + tSampling + "'";

            using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
            {
                using (SqlCommand tCommand = new SqlCommand(queryString, tConnection))
                {
                    try
                    {
                        tConnection.Open();

                        using (SqlDataReader reader = tCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    tResult = reader.GetInt32(0);
                                }
                            }

                            reader.Close();
                        }
                    }
                    catch (Exception) { }
                    finally
                    {
                        tConnection.Close();
                    }
                }
            }

            return tResult;
        }

        private Boolean InsertSampling(String tSamplingName)
        {
            if (null == tSamplingName)
            {
                tSamplingName = "Temporary";
            }
            else if ("" == tSamplingName.Trim())
            {
                tSamplingName = "Temporary";
            }

            Int32 tNewID = SamplingCount();
            tNewID = (tNewID == 0) ? 0 : tNewID;

            String queryString = @"
                    INSERT INTO SamplingRecord
                    (
                        [Sampling ID],
                        [Sampling Name],
                        [StartTime],
                        [LastUpdate],
                        [RecordCount]
                    )
                    VALUES
                    (
                        @ID,
                        @Name,
                        @StartTime,
                        @LastUpdate,
                        @RecordCount
                    )
                    ";
            try
            {
                using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(queryString, tConnection))
                    {
                        //! set ID
                        tCommand.Parameters.Add("@ID", SqlDbType.Int);
                        tCommand.Parameters["@ID"].Value = tNewID;

                        //! set ID
                        tCommand.Parameters.Add("@Name", SqlDbType.Text);
                        tCommand.Parameters["@Name"].Value = tSamplingName;

                        //! set ID
                        tCommand.Parameters.Add("@StartTime", SqlDbType.DateTime);
                        tCommand.Parameters["@StartTime"].Value = DateTime.Now;

                        //! set ID
                        tCommand.Parameters.Add("@LastUpdate", SqlDbType.DateTime);
                        tCommand.Parameters["@LastUpdate"].Value = DateTime.Now;

                        //! set ID
                        tCommand.Parameters.Add("@RecordCount", SqlDbType.Int);
                        tCommand.Parameters["@RecordCount"].Value = 0;

                        try
                        {
                            tConnection.Open();
                            tCommand.ExecuteNonQuery();
                        }
                        catch (Exception )
                        {
                        
                        }
                        finally
                        {
                            tConnection.Close();
                        }
                    }
                }
            }
            catch (Exception) { }

            return true;
        }

        private Boolean UpdateSampling(String tSamplingName, Int32 tRecordCount)
        {
            if ((null == tSamplingName)|| (tRecordCount < 0))
            {
                tSamplingName = "Temporary";
            }
            else if ("" == tSamplingName.Trim())
            {
                tSamplingName = "Temporary";
            }
            Int32 tID = FindSampling(tSamplingName);
            if (-1 == tID)
            {
                return false;
            }

            String queryString = @"
                    UPDATE SamplingRecord
                    SET
                        [LastUpdate] = @LastUpdate,
                        [RecordCount] = @RecordCount                    
                    WHERE
                        [Sampling ID] = @ID
                    ";
            try
            {
                using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(queryString, tConnection))
                    {
                        //! set ID
                        tCommand.Parameters.Add("@ID", SqlDbType.Int);
                        tCommand.Parameters["@ID"].Value = tID;

                        //! set last update
                        tCommand.Parameters.Add("@LastUpdate", SqlDbType.DateTime);
                        tCommand.Parameters["@LastUpdate"].Value = DateTime.Now;

                        //! set recordcount
                        tCommand.Parameters.Add("@RecordCount", SqlDbType.Int);
                        tCommand.Parameters["@RecordCount"].Value = tRecordCount;

                        try
                        {
                            tConnection.Open();
                            tCommand.ExecuteNonQuery();
                        }
                        catch (Exception )
                        {
                        }
                        finally
                        {
                            tConnection.Close();
                        }
                    }
                }
            }
            catch (Exception )
            {
                return false;
            }

            return true;
        }

        private Int32 SamplingCount()
        {
            String queryString = "SELECT COUNT(*) AS [Count] FROM SamplingRecord";

            Int32 tResult = -1;

            try
            {
                using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                {
                    using (SqlCommand tCommand = new SqlCommand(queryString, tConnection))
                    {
                        try
                        {
                            tConnection.Open();

                            using (SqlDataReader reader = tCommand.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        tResult = reader.GetInt32(0);
                                    }
                                }

                                reader.Close();
                            }
                        }
                        catch (Exception ) {
                        }
                        finally
                        {
                            tConnection.Close();
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return tResult;
        }

        public Boolean ConnectionTest()
        {
            return SamplingCount() >= 0;
        }

        public override String[] Samples
        {
            get 
            {
                String queryString = "SELECT [Sampling Name] FROM SamplingRecord";
                List<String> tResultStringList = new List<string>();

                try
                {
                    using (SqlConnection tConnection = new SqlConnection(c_DBConnectionString))
                    {
                        using (SqlCommand tCommand = new SqlCommand(queryString, tConnection))
                        {
                            try
                            {
                                tConnection.Open();

                                using (SqlDataReader reader = tCommand.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            tResultStringList.Add(reader.GetString(0));
                                        }
                                    }

                                    reader.Close();
                                }
                            }
                            catch (Exception) { }
                            finally
                            {
                                tConnection.Close();
                            }
                        }
                    }
                }
                catch (Exception) { }

                if (0 == tResultStringList.Count)
                {
                    return null;
                }

                return tResultStringList.ToArray();
            }
        }

        public override String CurrentSamplingName
        {
            get 
            {
                /*
                if (!m_bAvailable)
                {
                    return null;
                }*/

                return m_SampleName; 
            }
        }

        public override DataTable DataTable
        {
            get 
            {
                if (null != m_OriginalDataTable)
                {
                    return m_OriginalDataTable.Clone();
                }

                return null;
            }
        }

        #region IBMDataExpoter Members

        public override String DefaultFilePath
        {
            get
            {
                return m_Path;
            }
            set
            {
                if (m_bConnected)
                {
                    return;
                }

                if (Directory.Exists(value))
                {
                    m_Path = Path.GetFullPath(value);
                }
                //String tRelativePath = PathEx.RelativePath(System.Windows.Forms.Application.StartupPath, m_Path);
                //String tRelativePath = m_Path;

                /* AttachDbFilename=|DataDirectory|*/
                c_DBConnectionString = "Data Source=.\\SQLEXPRESS;AttachDBFileName=" + m_Path + "\\ATBMDB.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            }
        }

        public override Boolean Add(Object[] tRowItems)
        {
            //! need debug
            if (null == tRowItems)
            {
                return false;
            }
            else if (tRowItems.Length < 1)
            {
                return false;
            }
            if (!m_bAvailable)
            {
                return false;
            }
            if (!m_bConnected)
            {
                return false;
            }
            if (null == m_DataTable)
            {
                return false;
            }
            if (m_OriginalDataTable.Columns.Count != tRowItems.Length)
            {
                return false;
            }

            DataRow tInternalRow = m_DataTable.NewRow();
            tInternalRow["Sampling Name"] = m_SampleName;

            for (Int32 n = 1; n < tRowItems.Length; n++)
            {
                if (null == tRowItems[n])
                {
                    continue;
                }
                DataColumn tSourceColumn = m_OriginalDataTable.Columns[n];
                if (null == tSourceColumn)
                {
                    return false;
                }

                DataColumn tTargetColumn = m_DataTable.Columns[tSourceColumn.ColumnName];
                if (null == tTargetColumn)
                {
                    return false;
                }

                //! check data type
                if (tRowItems[n].GetType() != tSourceColumn.DataType)
                {
                    continue;
                }

                if (tRowItems[n].GetType() == typeof(UInt16))
                {
                    tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt16(BitConverter.GetBytes((UInt16)tRowItems[n]), 0);
                }
                else if (tRowItems[n].GetType() == typeof(UInt32))
                {
                    tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt32(BitConverter.GetBytes((UInt32)tRowItems[n]), 0);
                }
                else if (tRowItems[n].GetType() == typeof(UInt64))
                {
                    tInternalRow[tTargetColumn.ColumnName] = BitConverter.ToInt64(BitConverter.GetBytes((UInt64)tRowItems[n]), 0);
                }
                else
                {
                    tInternalRow[tTargetColumn.ColumnName] = tRowItems[n];
                }
            }


            m_DataTable.Rows.Add(tInternalRow);

            try
            {
                m_DataAdapter.Update(new DataRow[] { tInternalRow });
                m_RowCount++;

                UpdateSampling(m_SampleName, m_RowCount);
            }
            catch (Exception )
            {
                return false;
            }
            finally
            {
                m_DataTable.Rows.Clear();
                m_Connection.Close();
            }

            return true;
        }

#endregion
    }


    static public class SQLHelper
    {

        static public Boolean DeployDB(String tPath)
        {
            return DeployDB(tPath, "ATBMDB");
        }

        static public Boolean DeployDB(String tPath, String tDBName)
        {
            do
            {
                if (!Directory.Exists(tPath))
                {
                    break;
                }
                if (null == tDBName)
                {
                    break;
                }
                if (tDBName.Trim() == "")
                {
                    break;
                }

                try
                {
                    if (File.Exists(Path.Combine(tPath, tDBName + ".mdf")))
                    {
                        File.Delete(Path.Combine(tPath, tDBName + ".mdf"));
                    }
                }
                catch (Exception)
                { }

                try
                {
                    if (File.Exists(Path.Combine(tPath, tDBName + "_log.ldf")))
                    {
                        File.Delete(Path.Combine(tPath, tDBName + "_log.ldf"));
                    }
                }
                catch (Exception)
                { }


                try
                {
                    if (!File.Exists(Path.Combine(tPath, tDBName + ".mdf")))
                    {
                        File.WriteAllBytes(Path.Combine(tPath, tDBName + ".mdf"), UtilitiesDatabase.Properties.Resources.ATBMDB);
                    }
                    if (!File.Exists(Path.Combine(tPath, tDBName + "_log.ldf")))
                    {
                        File.WriteAllBytes(Path.Combine(tPath, tDBName + "_log.ldf"), UtilitiesDatabase.Properties.Resources.ATBMDB_log);
                    }
                }
                catch (Exception )
                {
                    break;
                }

                return true;

            } while (false);

            return false;
        }
    }
}
