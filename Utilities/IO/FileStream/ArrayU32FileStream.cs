using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace ESnail.Utilities.IO
{
    public partial class ArrayU32FileStream : ESMemoryFileStream
    {

        //! constructor
        public ArrayU32FileStream(String tPath, FileMode tMode, FileAccess tAccess, FileShare tFileShare)
            :base(tPath,tMode,tAccess,tFileShare)
        {
            m_MemorySpace.Alignment = 4;
        }

       
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
                    MemoryBlock[] tBlocks = m_MemorySpace.FetchMemoryBlocks(0, m_MemorySpace.Size);
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
                            if (!tBlock.Read(tAddress, ref tBuffer, 4))
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
                            UInt32 tWord = BitConverter.ToUInt32(tBuffer, 0);


                            if (0 == (tSize & 0x1F))
                            {
                                tStreamWriter.Write(tStreamWriter.NewLine);
                                if (this.AutoCopyToClipBoard)
                                {
                                    tResult.Append(tStreamWriter.NewLine);
                                }
                            }


                            String tRecordString = tWord.ToString("X8");
                            tStreamWriter.Write("0x");
                            tStreamWriter.Write(tRecordString);
                            tStreamWriter.Write(", ");

                            if (this.AutoCopyToClipBoard)
                            {
                                tResult.Append("0x");
                                tResult.Append(tRecordString);
                                tResult.Append(", ");

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

        protected override void FillMemorySpace()
        {
            throw new NotImplementedException();
        }

        #region Epityphlon
        protected override bool LoadMemoryBlockFromTargetFile(uint tTargetAddress, ref byte[] tData, int tSize)
        {
            return false;
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
        #endregion
    }
}