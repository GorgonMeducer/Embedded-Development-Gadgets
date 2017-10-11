using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ESnail.Utilities.IO
{
    public partial class HexFileStream : ESMemoryFileStream
    {
        
        
        //! \brief constructor
        public HexFileStream(String tFilePath,FileMode tMode, FileAccess tAccess, FileShare tFileShare)
            : base(tFilePath,tMode,tAccess,tFileShare)
        {
            
        }

        public HexFileStream(String tFilePath, FileMode tMode, FileAccess tAccess)
            : base(tFilePath, tMode, tAccess) { 
        
        }

        protected override void FillMemorySpace()
        {
            FileStream tFileStream = null;
            if (m_FileAccess == FileAccess.Write)
            {
                return;
            }
            else if (m_FileAccess == FileAccess.Read) 
            {
                tFileStream = new FileStream(m_Path, m_FileMode, m_FileAccess, FileShare.Read);
            }
            else
            {
                tFileStream = new FileStream(m_Path, m_FileMode, m_FileAccess, FileShare.ReadWrite);
            }

            using (StreamReader tStreamReader = new StreamReader(tFileStream))
            {

                String tRecordStr = null;
                try
                {
                    Boolean bSeeEOF = true;
                    UInt32 tAddress = 0;
                    do
                    {
                        tRecordStr = tStreamReader.ReadLine();
                        if (null != tRecordStr)
                        {
                            HEXRecord tRecord = HEXRecord.Parse(tRecordStr);
                            bSeeEOF = false;
                            if (null == tRecord)
                            {
                                throw new IOException("Illegal Hexadecimal Object File.");
                            }
                            switch (tRecord.RecordType)
                            {
                                case HEXRecord.Type.DATA_RECORD:
                                    UInt32 tTargetAddress = tAddress + tRecord.LoadOffset;
                                    if (this.Offset < 0)
                                    {
                                        tTargetAddress -= (UInt32)Math.Abs(this.Offset);
                                    }
                                    else
                                    {
                                        tTargetAddress += (UInt32)this.Offset;
                                    }
                                    m_MemorySpace.Write(tTargetAddress, tRecord.Data);
                                    break;
                                case HEXRecord.Type.END_OF_FILE_RECORD:
                                    bSeeEOF = true;
                                    break;
                                case HEXRecord.Type.EXTEND_SEGMENT_ADDRESS_RECORD:
                                    tAddress = ((ExtendSegmentAddressRecord)tRecord).ExtendSegmentBaseAddress;
                                    break;
                                case HEXRecord.Type.EXTEND_LINEAR_ADDRESS_RECORD:
                                    tAddress = ((ExtendLinearAddressRecord)tRecord).UpperLinearBaseAddress;
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            break;
                        }
                    }
                    while (!bSeeEOF);
                }
                catch (Exception Err)
                {
                    throw Err;
                }
                
            }
        }

        //! \brief load memory block from hex file
        protected override Boolean LoadMemoryBlockFromTargetFile(UInt32 tTargetAddress, ref Byte[] tData, Int32 tSize)
        {
            if (m_FileAccess == FileAccess.Write)
            {
                return false;
            }
            else if (null == m_File)
            {
                return false;
            }
            else if (!m_File.CanRead)
            {
                return false;
            }

            VirtualMemorySpace tMemorySpace = new VirtualMemorySpace();
            tMemorySpace.SpaceLength = UInt32.MaxValue;

            StreamReader tStreamReader = null;
            try
            {

                FileStream tFileStream = null;
                if (m_FileAccess == FileAccess.Read)
                {
                    tFileStream = new FileStream(m_Path, m_FileMode, m_FileAccess, FileShare.Read);
                }
                else
                {
                    tFileStream = new FileStream(m_Path, m_FileMode, m_FileAccess, FileShare.ReadWrite);
                }

                tStreamReader = new StreamReader(tFileStream);
                if (null == tStreamReader)
                {
                    return false;
                }
            }
            catch (Exception )
            {
                tStreamReader.Close();
                return false;
            }

            String tRecordStr = null;
            try
            {
                Boolean bSeeEOF = false;
                UInt32 tAddress = 0;
                do
                {
                    tRecordStr = tStreamReader.ReadLine();
                    if (null != tRecordStr)
                    {
                        HEXRecord tRecord = HEXRecord.Parse(tRecordStr);
                        if (null == tRecord)
                        {
                            break;
                        }
                        switch (tRecord.RecordType)
                        {
                            case HEXRecord.Type.DATA_RECORD:
                                if (
                                        ((tRecord.LoadOffset + tAddress) < tTargetAddress)
                                    && ((tRecord.LoadOffset + tAddress + tRecord.Data.Length) <= tTargetAddress)
                                   )
                                {
                                    continue;
                                }
                                else if (
                                            ((tRecord.LoadOffset + tAddress) > tTargetAddress)
                                        && ((tTargetAddress + tSize) <= (tRecord.LoadOffset + tAddress))
                                        )
                                {
                                    break;
                                }
                                UInt32 tLoadAddress = tAddress + tRecord.LoadOffset;
                                if (this.Offset < 0)
                                {
                                    tLoadAddress -= (UInt32)Math.Abs(this.Offset);
                                }
                                else
                                {
                                    tLoadAddress += (UInt32)this.Offset;
                                }
                                tMemorySpace.Write(tLoadAddress, tRecord.Data);
                                break;
                            case HEXRecord.Type.END_OF_FILE_RECORD:
                                bSeeEOF = true;
                                break;
                            case HEXRecord.Type.EXTEND_SEGMENT_ADDRESS_RECORD:
                                tAddress = ((ExtendSegmentAddressRecord)tRecord).ExtendSegmentBaseAddress;
                                break;
                            case HEXRecord.Type.EXTEND_LINEAR_ADDRESS_RECORD:
                                tAddress = ((ExtendLinearAddressRecord)tRecord).UpperLinearBaseAddress;
                                break;
                            default:
                                break;
                        }

                    }
                }
                while (!bSeeEOF);
            }
            catch (Exception )
            {
            }
            finally
            {
                tStreamReader.Dispose();
            }

            return tMemorySpace.Read(tTargetAddress, ref tData, tSize);
        }

        protected override void OnUpdateMemorySpaceEvent(uint tAddress, byte[] tData)
        {
        }

        protected override void OnEndUpdateMemorySpaceEvent()
        {
        }

        protected override void OnBeginUpdateMemorySpaceEvent()
        {
        }

        protected override void OnWriteMemoryToFile()
        {
            
        }
    }
}
