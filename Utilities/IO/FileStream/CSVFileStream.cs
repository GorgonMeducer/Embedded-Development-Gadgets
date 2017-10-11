using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ESnail.Utilities.IO
{
    public class CSVFileStream : ESRecordFileStream
    {
        public CSVFileStream(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare) 
            :base(tPath,tMode,tAccess, tFileShare)
        {
        
        }

        public String[] Read()
        {
            do
            {
                if (!m_Available)
                {
                    break;
                }
                else if (null == m_File)
                {
                    break;
                }
                else if (!m_File.CanRead)
                {
                    break;
                }
                else if (null == m_StreamReader)
                {
                    break;
                }

                String tLine = null;
                try
                {
                    tLine = m_StreamReader.ReadLine();
                }
                catch (Exception)
                {
                    break;
                }

                if (null == tLine)
                {
                    break;
                }
                return PathEx.Separate(tLine, ',');
            }
            while (false);

            return null;
        }
        
        public Boolean Write(String[] tObjects)
        {
            do
            {
                if (!m_Available)
                {
                    break;
                }
                else if (null == m_File)
                {
                    break;
                }
                else if (!m_File.CanWrite)
                {
                    break;
                }
                else if (null == m_StreamWriter)
                {
                    break;
                }

                try
                {
                    m_StreamWriter.WriteLine(PathEx.CombineEx(',', tObjects));
                }
                catch (Exception)
                {
                    break;
                }

                return true;

            } while (false);

            return false;
        }

        public Boolean Write(String tObject)
        {
            do
            {
                if (!m_Available)
                {
                    break;
                }
                else if (null == m_File)
                {
                    break;
                }
                else if (!m_File.CanWrite)
                {
                    break;
                }
                else if (null == m_StreamWriter)
                {
                    break;
                }

                try
                {
                    m_StreamWriter.WriteLine(tObject);
                }
                catch (Exception)
                {
                    break;
                }

                return true;

            } while (false);

            return false;
        }

    }
}
