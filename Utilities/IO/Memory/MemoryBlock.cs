using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.IO
{
    public interface IMemoryWriter
    {
        Boolean Write(UInt32 tAddress, Byte[] tBuffer);
    }

    public interface IMemoryReader
    {
        Boolean Read(UInt32 tAddress, ref Byte[] tData, Int32 tLenght);
    }

    public interface IMemoryRangeChecker
    {
        Boolean IsInRange(UInt32 tAddress);
    }

    public partial class VirtualMemorySpaceImage
    {
        protected class MemoryControlBlock
        {
            private UInt32 m_StartAddress = 0;
            private Int32 m_Size = 0;
            private MemoryBlock m_tBlock = null;
            private Boolean m_Modified = false;
            private Boolean m_Buffed = false;


            internal delegate Boolean UpdateMemoryBlock(UInt32 tAddress, Byte[] tData);
            internal event UpdateMemoryBlock UpdateMemoryBlockEvent;

            internal delegate Boolean LoadMemoryBlock(UInt32 tAddress, ref Byte[] tData, Int32 tSize);
            internal event LoadMemoryBlock LoadMemoryBlockEvent;

            private Boolean OnLoadMemoryBlock(UInt32 tAddress, ref Byte[] tData, Int32 tSize)
            {
                if (null != LoadMemoryBlockEvent)
                {
                    try
                    {
                        return LoadMemoryBlockEvent(tAddress, ref tData, tSize);
                    }
                    catch (Exception )
                    {
                        return false;
                    }
                }

                return false;
            }


            //! \brief constructor with address and size
            public MemoryControlBlock(UInt32 tAddress, Int32 tSize)
            {
                m_StartAddress = tAddress;
                m_Size = tSize;
            }

            //! \brief constructor with memory block
            public MemoryControlBlock(MemoryBlock tBlock)
            {
                if (null != tBlock)
                {
                    m_StartAddress = tBlock.Address;
                    m_Size = tBlock.Size;

                    m_tBlock = tBlock;
                    m_Buffed = true;
                }
            }

            //! update this memory block
            public Boolean Update()
            {
                if (null == m_tBlock)
                {
                    Byte[] tDataBuffer = null;

                    if (!OnLoadMemoryBlock(m_StartAddress, ref tDataBuffer, m_Size))
                    {
                        return false;
                    }

                    m_tBlock = new MemoryBlock(m_StartAddress, m_Size, tDataBuffer);
                    m_Buffed = true;
                }

                if (null != UpdateMemoryBlockEvent)
                {
                    try
                    {
                        if (UpdateMemoryBlockEvent(m_StartAddress, m_tBlock.Data))
                        {
                            m_Modified = false;
                            return true;
                        }

                        return false;
                    }
                    catch (Exception )
                    {
                        return false;
                    }
                }

                return false;
            }

            public void Refresh()
            {
                if (null == m_tBlock)
                {
                    return;
                }

                m_StartAddress = m_tBlock.Address;
                m_Size = m_tBlock.Size;
            }

            public MemoryBlock Memory
            {
                get
                {
                    if (null == m_tBlock)
                    {
                        Byte[] tDataBuffer = null;

                        if (!OnLoadMemoryBlock(m_StartAddress, ref tDataBuffer, m_Size))
                        {
                            return null;
                        }

                        m_tBlock = new MemoryBlock(m_StartAddress, m_Size, tDataBuffer);
                        m_Buffed = true;

                        Refresh();
                    }
                    return m_tBlock;
                }
            }

            public UInt32 StartAddress
            {
                get { return m_StartAddress; }
            }

            public UInt32 EndAddress
            {
                get { return (UInt32)((UInt32)m_StartAddress + (UInt32)m_Size - 1); }
            }

            public Int32 Size
            {
                get { return m_Size; }
            }

            public Boolean Modified
            {
                get { return m_Modified; }
            }

            public Boolean Buffed
            {
                get { return m_Buffed; }
            }

            //! \brief write data safely
            public Int32 Write(UInt32 tAddress, Byte[] tData, Int32 tStartIndex)
            {
                if (
                        (tAddress >= (m_StartAddress + m_Size))
                    || (tAddress < m_StartAddress)
                    || (null == tData)
                   )
                {
                    return 0;
                }
                else if (tStartIndex >= tData.Length)
                {
                    return 0;
                }

                //! load buffer
                if (null == m_tBlock)
                {
                    Byte[] tDataBuffer = null;

                    if (!OnLoadMemoryBlock(m_StartAddress, ref tDataBuffer, m_Size))
                    {
                        return 0;
                    }

                    m_tBlock = new MemoryBlock(m_StartAddress, m_Size, tDataBuffer);
                    m_Buffed = true;
                }


                m_Modified = true;

                Int32 Count = 0;
                UInt32 tIndex = tAddress - m_StartAddress;
                do
                {
                    if ((tIndex + Count) >= m_tBlock.Data.Length)
                    {
                        break;
                    }
                    m_tBlock.Data[tIndex + Count] = tData[tStartIndex + Count];
                    Count++;
                }
                while ((Count + tStartIndex) < tData.Length);

                return Count;
            }

            //! \brief read data safely
            public Int32 Read(UInt32 tAddress, Byte[] tData, Int32 tStartIndex)
            {
                if (
                        (tAddress >= (m_StartAddress + m_Size))
                    || (tAddress < m_StartAddress)//((tAddress + tData.Length - tStartIndex) <= m_StartAddress)
                    || (null == tData)
                   )
                {
                    return 0;
                }

                //! load buffer
                if (null == m_tBlock)
                {
                    Byte[] tDataBuffer = null;

                    if (!OnLoadMemoryBlock(m_StartAddress, ref tDataBuffer, m_Size))
                    {
                        return 0;
                    }

                    m_tBlock = new MemoryBlock(m_StartAddress, m_Size, tDataBuffer);
                    m_Buffed = true;

                    Refresh();
                }

                Int32 Count = 0;
                UInt32 tIndex = tAddress - m_StartAddress;
                do
                {
                    if ((tIndex + Count) >= m_tBlock.Data.Length)
                    {
                        break;
                    }
                    tData[tStartIndex + Count] = m_tBlock.Data[tIndex + Count];
                    Count++;
                }
                while ((Count + tStartIndex) < tData.Length);

                return Count;
            }

            //! \unload buffer
            public void Unload()
            {
                m_Buffed = false;
                m_tBlock = null;
            }

            //! \brief load buffer
            public Boolean Load(MemoryBlock tBlock)
            {
                if (null == tBlock)
                {
                    return false;
                }

                if (tBlock.Address != m_StartAddress)
                {
                    return false;
                }
                else if (tBlock.Size != m_Size)
                {
                    return false;
                }

                m_tBlock = tBlock;

                return true;
            }
        }
    }


    //! \name memory block
    public class MemoryBlock : IMemoryReader, IMemoryWriter, IMemoryRangeChecker
    {
        private UInt32 m_Address = 0;
        private Int32 m_Length = 0;
        private Byte[] m_Data = null;

        //! \brief constructor with address and size of memeory block
        public MemoryBlock(UInt32 tAddress, Int32 tSize)
        {
            m_Address = tAddress;
            m_Length = tSize;

            m_Data = new Byte[tSize];
        }

        public MemoryBlock(UInt32 tAddress, Byte[] tData)
        {
            m_Address = tAddress;
            if (null == tData)
            {
                return;
            }
            m_Data = tData;
            m_Length = tData.Length;
        }

        //! \brief constructor with address and memory block
        public MemoryBlock(UInt32 tAddress, Int32 tSize, Byte[] tData)
        {
            m_Address = tAddress;

            if (null != tData)
            {
                if (tData.Length != tSize)
                {
                    List<Byte> tList = new List<Byte>();

                    tList.AddRange(tData);
                    m_Data = tList.ToArray();
                    Array.Resize(ref m_Data, tSize);
                }
                else
                {
                    m_Data = tData;
                }
            }
            else
            {
                m_Data = new Byte[tSize];
            }

            m_Length = tSize;
        }

        public Boolean IsInRange(UInt32 wAddress)
        {
            if ((wAddress >= m_Address) && (wAddress <= m_Address + m_Length))
            {
                return true;
            }

            return false;
        }

        //! \brief memory block address
        public UInt32 Address
        {
            get
            {
                return m_Address;
            }
        }

        //! \brief memory block size
        public Int32 Size
        {
            get
            {
                return m_Length;
            }
        }

        //! \brief data
        public Byte[] Data
        {
            get { return m_Data; }
        }

        //! \brief append memory block
        internal Boolean Append(MemoryBlock tBlock)
        {
            if (null == tBlock)
            {
                return false;
            }

            return Append(tBlock.m_Address, tBlock.m_Data);
        }

        //! \brief append memory byte stream
        internal Boolean Append(UInt32 tAddress, Byte[] tData)
        {
            if ((tAddress < m_Address) || (null == tData))
            {
                return false;
            }

            if ((tAddress + tData.Length) > (m_Address + m_Length))
            {
                Array.Resize(ref m_Data, (int)((tAddress + tData.Length) - m_Address));
            }

            tData.CopyTo(m_Data, tAddress - m_Address);
            m_Length = m_Data.Length;

            return true;
        }

        public MemoryBlock[] Seperate(Int32 tOffset, Int32 tBlockSize)
        {
            if ((tOffset < 0) || (tBlockSize <= 0) || (0 == m_Length) || (null == m_Data))
            {
                return null;
            }
            if ((0 == tOffset && tBlockSize >= m_Length && null != m_Data))
            {
                return new MemoryBlock[] { this };
            }
            if (tOffset >= m_Length)
            {
                return new MemoryBlock[] { null };
            }

            List<MemoryBlock> m_BlockList = new List<MemoryBlock>();

            do
            {
                Int32 tLength = tBlockSize;
                if ((m_Length - tOffset) < tBlockSize)
                {
                    tLength = m_Length - tOffset;
                }
                Byte[] tBuffer = new Byte[tLength];
                Array.Copy(m_Data, tOffset, tBuffer, tLength, 0);
                m_BlockList.Add(new MemoryBlock((m_Address+(UInt32)tOffset),tLength,tBuffer));
                tOffset += tLength;
            }
            while(tOffset<m_Length);

            return m_BlockList.ToArray();
        }

        //! \brief read data out
        public Boolean Read(UInt32 tAddress, ref Byte[] tData, Int32 tLength)
        {
            if (0 == tLength)
            {
                return false;
            }
            if (null == tData)
            {
                tData = new Byte[tLength];
            }
            do
            {
                if (tAddress + tLength < m_Address)
                {
                    break;
                }
                if (tAddress >= m_Address + m_Length)
                {
                    break;
                }

                if (tAddress >= m_Address)
                {
                    if (tAddress + tLength > m_Address + m_Length)
                    {
                        tLength = Math.Min(tLength, (Int32)(m_Address + (UInt32)m_Length - tAddress));
                    }
                    Array.Copy(m_Data, tAddress - m_Address, tData, 0, tLength);
                }
                else /* if (tAddress + tSize >= m_Address) */
                {
                    UInt32 tDelta = m_Address - tAddress;
                    tLength -= (Int32)tDelta;
                    Array.Copy(m_Data, m_Address, tData, tDelta, tLength);
                }
                return true;
            }
            while (false);

            return false;
        }

        public Boolean Read(UInt32 tAddress, Byte[] tData)
        {
            if (null == tData)
            {
                return false;
            }
            else if (0 == tData.Length)
            {
                return true;
            }

            return this.Read(tAddress, ref tData, tData.Length);
        }

        

        public Boolean Write(UInt32 tAddress, Byte[] tBuffer)
        {
            if (null == tBuffer)
            {
                return false;
            } else if (tAddress < this.Address || tAddress >= this.Address + m_Length) {
                return false;
            }

            for (UInt32 tIndex = 0; tIndex < tBuffer.Length; tIndex++)
            {
                m_Data[tAddress + tIndex] = tBuffer[tIndex];
            }
            return true;
        }

    }


    static public class MemoryConvert
    {
        static public Boolean ToUInt32(IMemoryReader tMemory, UInt32 tAddress, ref UInt32 tData)
        {
            Boolean tResult = false;
            if (null == tMemory)
            {
                return false;
            }

            Byte[] tBuffer = new Byte[4];
            tResult = tMemory.Read(tAddress,ref tBuffer, 4);
            tData = BitConverter.ToUInt32(tBuffer, 0);
            return tResult;
        }

        static public Boolean ToInt32(IMemoryReader tMemory, UInt32 tAddress, ref Int32 tData)
        {
            Boolean tResult = false;
            if (null == tMemory)
            {
                return false;
            }

            Byte[] tBuffer = new Byte[4];
            tResult = tMemory.Read(tAddress, ref tBuffer, 4);
            tData = BitConverter.ToInt32(tBuffer, 0);
            return tResult;
        }
        
        static public Boolean FromUInt32(IMemoryWriter tMemory, UInt32 tAddress, UInt32 tData)
        {
            if (null == tMemory)
            {
                return false;
            }

            return tMemory.Write(tAddress, BitConverter.GetBytes(tData));
        }

        static public Boolean FromInt32(IMemoryWriter tMemory, UInt32 tAddress, Int32 tData)
        {
            if (null == tMemory)
            {
                return false;
            }

            return tMemory.Write(tAddress, BitConverter.GetBytes(tData));
        }
    }

}
