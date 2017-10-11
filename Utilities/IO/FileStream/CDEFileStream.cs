using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace ESnail.Utilities.IO
{

    public partial class CDEFileStream : ESMemoryFileStream
    {
        private class Record
        {
            UInt32 m_Address = 0;
            UInt32 m_Data = 0;
            Boolean m_Available = false;

            public Record(String tLine)
            {
                Parse(tLine);
            }

            public Record(UInt32 tAddress, UInt32 tData)
            {
                m_Address = tAddress;
                m_Data = tData;
                m_Available = true;
            }

            public override string ToString()
            {
                StringBuilder tString = new StringBuilder();
                tString.Append('@');
                UInt32 tTemp = m_Address / 4;
                tString.Append(tTemp.ToString("X8"));
                tString.Append(' ');
                tString.Append(m_Data.ToString("X8"));
                tString.Append('\n');
                return tString.ToString();
            }

            private void Parse(String tLine)
            {
                do
                {
                    if (null == tLine)
                    {
                        break;
                    }
                    tLine = tLine.Trim();
                    if ("" == tLine)
                    {
                        break;
                    }
                    else if (tLine.StartsWith("@"))
                    {

                    }
                    String[] tDatas = PathEx.Separate(tLine.Substring(1), ' ');
                    if (null == tDatas)
                    {
                        break;
                    }
                    else if (tDatas.Length < 2)
                    {
                        break;
                    }

                    {
                        UInt32 tTemp;
                        if (!UInt32.TryParse(tDatas[0], NumberStyles.HexNumber, null, out tTemp))
                        {
                            break;
                        }
                        m_Address = tTemp * 4;
                    }

                    {
                        UInt32 tTemp;
                        if (!UInt32.TryParse(tDatas[1], NumberStyles.HexNumber, null, out tTemp))
                        {
                            break;
                        }
                        m_Data = tTemp;
                    }

                    m_Available = true;
                } while (false);
            }

            public Boolean Available
            {
                get { return m_Available; }
            }

            public UInt32 Address
            {
                get { return m_Address; }
            }

            public UInt32 Data
            {
                get { return m_Data; }
            }
        }
    }


    public partial class CDEFileStream : ESMemoryFileStream
    {
        
        //! constructor
        public CDEFileStream(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare)
            :base(tPath,tMode,tAccess,tFileShare)
        {
            m_MemorySpace.Alignment = 4;
        }


        protected override void FillMemorySpace()
        {
            m_MemorySpace.Alignment = 4;
            FileStream tFileStream = null;
            if (m_FileAccess == FileAccess.Write)
            {
                return;
            }
            tFileStream = m_File;

            StreamReader tStreamReader = new StreamReader(tFileStream);

            String tRecordStr = null;
            try
            {
                while (!tStreamReader.EndOfStream)
                {
                    tRecordStr = tStreamReader.ReadLine();
                    if (null != tRecordStr)
                    {
                        Record tRecord = new Record(tRecordStr);
                        if (null == tRecord)
                        {
                            continue;
                        }
                        else if (!tRecord.Available)
                        {
                            continue;
                        }

                        UInt32 tLoadAddress = tRecord.Address;
                        if (this.Offset < 0)
                        {
                            tLoadAddress -= (UInt32)Math.Abs(this.Offset);
                        }
                        else
                        {
                            tLoadAddress += (UInt32)this.Offset;
                        }
                        if (!m_MemorySpace.Write(tLoadAddress, BitConverter.GetBytes(tRecord.Data)))
                        {
                            throw new IOException("Failed in loading target cde file");
                        }
                    }                    
                }
                
            }
            catch (Exception Err)
            {
                throw Err;
            }
            finally
            {
                tStreamReader.Close();
            }
        }

        #region Epityphlon
        protected override void OnUpdateMemorySpaceEvent(uint tAddress, byte[] tData)
        {
            
        }

        protected override void OnEndUpdateMemorySpaceEvent()
        {
            
        }

        protected override void OnBeginUpdateMemorySpaceEvent()
        {
            
        }

        protected override bool LoadMemoryBlockFromTargetFile(uint tTargetAddress, ref byte[] tData, int tSize)
        {
            return false;
        }
        #endregion

        protected override void OnWriteMemoryToFile()
        {
            FileStream tFileStream = null;
            if (m_FileAccess == FileAccess.Read)
            {
                return;
            }
            tFileStream = m_File;

            StreamWriter tStreamWriter = new StreamWriter(tFileStream);

            try
            {
                StringBuilder tResult = new StringBuilder();
                do
                {
                    MemoryBlock[] tBlocks = m_MemorySpace.FetchMemoryBlocks(0,m_MemorySpace.Size);
                    if (null == tBlocks)
                    {
                        break;
                    }
                    foreach (MemoryBlock tBlock in tBlocks)
                    {
                        UInt32 tAddress = tBlock.Address;
                        for (UInt32 tSize = 0; tSize < tBlock.Size; tSize += 4)
                        { 
                            Byte[] tBuffer = new Byte[4];
                            if (!tBlock.Read(tAddress,ref tBuffer, 4))
                            {
                                break;
                            }
                            UInt32 tWriteAddress = tAddress;
                            if (this.Offset < 0)
                            {
                                tWriteAddress += (UInt32)Math.Abs(this.Offset);
                            }
                            else
                            {
                                tWriteAddress -= (UInt32)this.Offset;
                            }
                            Record tRecord = new Record(tWriteAddress, BitConverter.ToUInt32(tBuffer, 0));
                            String tRecordString = tRecord.ToString();
                            tStreamWriter.Write(tRecordString);
                            if (this.AutoCopyToClipBoard)
                            {
                                tResult.Append(tRecordString);
                            }
                            tAddress += 4;
                        }
                    }
                } while (false);
                if (this.AutoCopyToClipBoard)
                {
                    System.Windows.Forms.Clipboard.SetText(tResult.ToString());
                }
            }
            catch (Exception Err)
            {
                throw Err;
            }
            finally
            {
                tStreamWriter.Close();
            }
        }

        
    }

}
