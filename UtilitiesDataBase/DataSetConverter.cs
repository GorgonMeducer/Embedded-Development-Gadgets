using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Office.Interop.Excel;
using ESnail.Utilities.Threading;

namespace ESnail.Utilities.DataBase
{
    //! \name data set converter
    //! @{
    public class DataSetConverter
    {
        public delegate void OutputProgressReport(Int32 tPercent);

        //! \brief static method for export dataset to excel file
        public static Boolean ToExcel(DataSet tDataSet, String tFilePath, Boolean ExcelVisible)
        {
            return ToExcel(tDataSet, tFilePath, ExcelVisible, null);
        }

        //! \brief static method for export dataset to excel file
        public static Boolean ToExcel(DataSet tDataSet, String tFilePath, Boolean ExcelVisible, OutputProgressReport ProgressReport)
        {
            if ((null == tDataSet) || (null == tFilePath))
            {
                return false;
            }
            if ((0 == tDataSet.Tables.Count) || ("" == Path.GetFileName(tFilePath).Trim()))
            {
                return false;
            }
            if (".xls" != Path.GetExtension(tFilePath).ToLower())
            {
                return false;
            }

            Application excelApp = null;

            using (CultureEnsure CultureKeeper = new CultureEnsure("en-us"))
            {
                //! try to get excel application instances
                try
                {
                    excelApp = new Application();
                    if (null == excelApp)
                    {
                        return false;
                    }
                    excelApp.Visible = ExcelVisible;
                    //! get all work books
                    Workbooks tWorkbooks = excelApp.Workbooks;
                    if (null == tWorkbooks)
                    {
                        return false;
                    }
                    
                    //! add new workbook
                    Workbook tNewWorkBook = tWorkbooks.Add(XlWBATemplate.xlWBATWorksheet);
                    if (null == tNewWorkBook)
                    {
                        return false;
                    }
                    
                    
                    //! output each table in each worksheet
                    for (Int32 n = 0; n < tDataSet.Tables.Count; n++)
                    {
                        Worksheet tWorksheet = null;
                        //! write all table to each sheets
                        if (n < tNewWorkBook.Worksheets.Count)
                        {
                            tWorksheet = tNewWorkBook.Worksheets[n + 1] as Worksheet;
                        }
                        else
                        {
                            tWorksheet = tNewWorkBook.Worksheets.Add(tNewWorkBook.Worksheets[n], null, 1, XlSheetType.xlWorksheet) as Worksheet;
                        }
                        if (null == tWorksheet)
                        {
                            return false;
                        }
                        

                        do
                        {
                            Int32 tLastPercent = 0;
                            DataColumnCollection tColumns = tDataSet.Tables[n].Columns;
                            if (null == tColumns)
                            {
                                return false;
                            }
                            //! add columns
                            for (Int32 nColumnCount = 0; nColumnCount < tColumns.Count; nColumnCount++)
                            {
                                tWorksheet.Cells[1, nColumnCount + 1] = tColumns[nColumnCount].ColumnName;
                            }


                            DataRowCollection tRows = tDataSet.Tables[n].Rows;
                            if (null == tRows)
                            {
                                return false;
                            }

                            //! add data
                            for (Int32 nRowCount = 0; nRowCount < tRows.Count; nRowCount++)
                            {
                                for (Int32 nColumnCount = 0; nColumnCount < tColumns.Count; nColumnCount++)
                                {

                                    if (typeof(Byte) == tColumns[nColumnCount].DataType)
                                    {
                                        if (tRows[nRowCount][nColumnCount] is Byte)
                                        {
                                            tWorksheet.Cells[nRowCount + 2, nColumnCount + 1] = ((Byte)tRows[nRowCount][nColumnCount]).ToString("X2");
                                        }
                                    }
                                    else if (typeof(UInt16) == tColumns[nColumnCount].DataType)
                                    {
                                        if (tRows[nRowCount][nColumnCount] is UInt16)
                                        {
                                            tWorksheet.Cells[nRowCount + 2, nColumnCount + 1] = ((UInt16)tRows[nRowCount][nColumnCount]).ToString("X4");
                                        }
                                    }
                                    else if (typeof(UInt32) == tColumns[nColumnCount].DataType)
                                    {
                                        if (tRows[nRowCount][nColumnCount] is UInt32)
                                        {
                                            tWorksheet.Cells[nRowCount + 2, nColumnCount + 1] = ((UInt32)tRows[nRowCount][nColumnCount]).ToString("X8");
                                        }
                                    }
                                    else if (typeof(UInt64) == tColumns[nColumnCount].DataType)
                                    {
                                        if (tRows[nRowCount][nColumnCount] is UInt64)
                                        {
                                            tWorksheet.Cells[nRowCount + 2, nColumnCount + 1] = ((UInt64)tRows[nRowCount][nColumnCount]).ToString("X16");
                                        }
                                    }
                                    else
                                    {
                                        tWorksheet.Cells[nRowCount + 2, nColumnCount + 1] = tRows[nRowCount][nColumnCount].ToString();
                                    }
                                }

                                if (0 == (nRowCount % 100))
                                {
                                    if (null != ProgressReport)
                                    {
                                        Int32 tCurrentProgress = nRowCount * 100 / tDataSet.Tables.Count;

                                        if (tCurrentProgress > tLastPercent)
                                        {
                                            tLastPercent = tCurrentProgress;

                                            try
                                            {
                                                ProgressReport.Invoke(tCurrentProgress);
                                            }
                                            catch (Exception )
                                            {
                                            }
                                        }
                                    }

                                    System.Windows.Forms.Application.DoEvents();
                                }
                            }

                            tWorksheet.Columns.EntireColumn.AutoFit();
                        }
                        while (false);

                        //! try to save file
                        tNewWorkBook.Saved = true;
                        tNewWorkBook.SaveCopyAs(tFilePath);


                    }
                }
                catch (Exception )
                {
                    return false;
                }
                finally
                {
                    if (null != excelApp)
                    {
                        try
                        {
                            excelApp.Quit();
                        }
                        catch (Exception )
                        {
                        }
                    }
                    GC.Collect();
                }
            }

            return true;
        }
    }
    //! @}
}
