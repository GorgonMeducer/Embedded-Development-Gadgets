using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.IO
{

    public class MemoryDescription : IMemoryRangeChecker
    {
        private UInt32 m_wAddress = 0;
        private UInt32 m_wSize = 0;
        private String m_Description = null;
        private Object m_Tag = null;

        #region Constructors
        public MemoryDescription(UInt32 wAddress, UInt32 wSize, String tDescription, Object tObj)
        {
            m_wAddress = wAddress;
            m_wSize = wSize;            //! could be zero
            m_Description = tDescription;
            m_Tag = tObj;
        }

        public MemoryDescription(UInt32 wAddress, String tDescription, Object tObj)
        {
            m_wAddress = wAddress;
            m_Description = tDescription;
            m_Tag = tObj;
        }

        public MemoryDescription(UInt32 wAddress, UInt32 wSize, String tDescription)
        {
            m_wAddress = wAddress;
            m_wSize = wSize;            //! could be zero
            m_Description = tDescription;
        }

        //! initialise a label for specified address
        public MemoryDescription(UInt32 wAddress, String tDescription)
        {
            m_wAddress = wAddress;
            m_Description = tDescription;
        }

        //! initialise a label for specified address
        public MemoryDescription(UInt32 wAddress, UInt32 wSize, Object tObj)
        {
            m_wAddress = wAddress;
            m_wSize = wSize;            //! could be zero
            m_Tag = tObj;
            m_Description = tObj.ToString();
        }

        //! initialise a label for specified address
        public MemoryDescription(UInt32 wAddress, Object tObj)
        {
            m_wAddress = wAddress;
            m_Tag = tObj;
            m_Description = tObj.ToString();
        }

        #endregion

        public Boolean IsInRange(UInt32 wAddress)
        {
            if ((wAddress >= m_wAddress) && (wAddress <= m_wAddress + m_wSize))
            {
                return true;
            }

            return false;
        }

        #region Properties
        public Object Tag
        {
            get { return m_Tag; }
        }

        public String Description
        {
            get { return m_Description; }
        }

        public UInt32 Address
        {
            get { return m_wAddress; }
        }

        public UInt32 Size
        {
            get => m_wSize;
        }

        public override String ToString()
        {
            StringBuilder sbOutput = new StringBuilder();

            sbOutput.Append(m_wAddress.ToString("X8"));
            sbOutput.Append('\t');
            sbOutput.Append(m_Description);
            sbOutput.Append('\t');
            if (null != m_Tag)
            {
                sbOutput.Append(m_Tag.ToString());
            }

            return sbOutput.ToString();
        }

        #endregion
    }


    //! \name memory heap
    public partial class VirtualMemorySpaceImage : IMemoryReader
    {
        protected Byte m_FillByte = 0xFF;
        private Int32 m_MaxBufferSize = -1;//256 * 1024;             //!< default buffer size
        private UInt32 m_SpaceSize = 0;
        protected UInt32 m_MaxSpaceSize = 0;

        protected SortedList<UInt32, MemoryControlBlock> m_Blocks = new SortedList<uint, MemoryControlBlock>();

        #region Events and delegates
        public delegate void BeginUpdateMemorySpace();

        public event BeginUpdateMemorySpace BeginUpdateMemorySpaceEvent;
        
        //! \brief method for clear other media
        protected Boolean OnBeginUpdateMemorySpace()
        {
            if (null != BeginUpdateMemorySpaceEvent)
            {
                try
                {
                    BeginUpdateMemorySpaceEvent.Invoke();
                }
                catch (Exception )
                {
                    return false;
                }
            }

            return true;
        }

        public delegate void UpdateMemorySpace(UInt32 tAddress, Byte[] tData);

        public event UpdateMemorySpace UpdateMemorySpaceEvent;

        //! \brief method for writing memory block to other media
        protected Boolean OnUpdateMemorySpace(UInt32 tAddress, Byte[] tData)
        {
            if (null != UpdateMemorySpaceEvent)
            {
                try
                {
                    UpdateMemorySpaceEvent.Invoke(tAddress, tData);
                }
                catch (Exception )
                {
                    return false;
                }
            }

            return true;
        }

        public delegate void EndUpdateMemorySpace();

        public event EndUpdateMemorySpace EndUpdateMemorySpaceEvent;

        protected Boolean OnEndUpdateMemorySpace()
        {
            if (null != EndUpdateMemorySpaceEvent)
            {
                try
                {
                    EndUpdateMemorySpaceEvent.Invoke();
                }
                catch (Exception )
                {
                    return false;
                }
            }

            return true;
        }

        public delegate Boolean LoadMemoryBlock(UInt32 tAddress, ref Byte[] tData, Int32 tSize);

        public event LoadMemoryBlock LoadMemoryBlockEvent;

        public Boolean OnLoadMemoryBlock(UInt32 tAddress, ref Byte[] tData, Int32 tSize)
        {
            if (null != LoadMemoryBlockEvent)
            {
                try
                {
                    return LoadMemoryBlockEvent.Invoke(tAddress,ref tData, tSize);
                }
                catch (Exception )
                {
                }
            }

            return false;
        }

        #endregion


        protected void ManageBuffer()
        {
            UInt32 tTotalSize = 0;
            Int32 tBuffedBlockCount = 0;
            UInt32 tMaxAddress = 0;
            foreach (KeyValuePair<UInt32, MemoryControlBlock> tBlock in m_Blocks)
            {
                if (null == tBlock.Value)
                {
                    continue;
                }
                if (tBlock.Value.Buffed)
                {
                    tTotalSize += (UInt32)tBlock.Value.Size;
                    tBuffedBlockCount++;
                }
                if (((tBlock.Value.StartAddress + tBlock.Value.Size) - 1) > tMaxAddress)
                {
                    tMaxAddress = (tBlock.Value.StartAddress + (UInt32)tBlock.Value.Size) - 1;
                }
            }

            m_SpaceSize = tMaxAddress + 1;


            if ((tTotalSize <= m_MaxBufferSize) || m_MaxBufferSize < 0)
            {
                return;
            }

            UInt32 tLeftSize = tTotalSize - (UInt32)m_MaxBufferSize;

            foreach (KeyValuePair<UInt32, MemoryControlBlock> tBlock in m_Blocks)
            {
                if (null == tBlock.Value)
                {
                    continue;
                }
                if ((tBlock.Value.Buffed) && (tBuffedBlockCount > 1))
                {
                    tBlock.Value.Unload();
                    tLeftSize -= (UInt32)tBlock.Value.Size;
                    tBuffedBlockCount--;
                    if (tLeftSize <= 0)
                    {
                        break;
                    }
                }
            }


        }

        public void Refresh()
        {
            CleanUp(0);
            ManageBuffer();
        }

        public UInt32 StartAddress
        {
            get 
            {
                UInt32 tResult = 0;
                do
                {
                    if (null == m_Blocks)
                    {
                        break;
                    }
                    else if (0 == m_Blocks.Count)
                    {
                        break;
                    }

                    tResult = m_Blocks.Keys[0];
                }
                while (false);
                return tResult;
            }
        }

        protected void CleanUp(UInt32 tStartKey)
        {
            MemoryControlBlock tLastBlock = null;
            Boolean bFirstFind = true;
            if (0 >= m_Blocks.Count)
            {
                return;
            }

            MemoryControlBlock tItem = m_Blocks.Values[0];

            for (Int32 tIndex = 0; tIndex < m_Blocks.Values.Count; tIndex++)
            {

                MemoryControlBlock tBlock = m_Blocks.Values[tIndex];

                //_Blocks.Values[tIndex];
                //foreach (KeyValuePair<UInt32, MemoryBlockControlBlock> tBlock in m_Blocks)
                {
                    if (null == tBlock)
                    {
                        m_Blocks.Remove(tBlock.StartAddress);
                        continue;
                    }

                    if (0 != m_MaxSpaceSize)
                    {
                        if (tBlock.StartAddress >= m_MaxSpaceSize)
                        {
                            m_Blocks.Remove(tBlock.StartAddress);
                            tBlock.UpdateMemoryBlockEvent -= new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                            tBlock.LoadMemoryBlockEvent -= new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);
                            tIndex--;
                            continue;
                        }
                    }

                    if (tBlock.StartAddress < tStartKey)
                    {
                        continue;
                    }
                    else if (bFirstFind)
                    {
                        if (null == tBlock.Memory)
                        {
                            continue;
                        }

                        bFirstFind = false;
                        tLastBlock = tBlock;
                        continue;
                    }


                    tBlock.Refresh();
                    if ((tLastBlock.StartAddress + tLastBlock.Size) >= tBlock.StartAddress && tLastBlock != tBlock)
                    {
                        if (null == tBlock.Memory)
                        {
                            m_Blocks.Remove(tBlock.StartAddress);
                            tBlock.UpdateMemoryBlockEvent -= new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                            tBlock.LoadMemoryBlockEvent -= new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);
                            tIndex--;
                            continue;
                        }

                        //! we should merge these memory block
                        MemoryBlock tNewMemoryBlock = Merge(tBlock.Memory, tLastBlock.Memory);
                        if (null == tNewMemoryBlock)
                        {
                            continue;
                        }

                        //! creat new memory block control block
                        MemoryControlBlock tNewMBCB = new MemoryControlBlock(tNewMemoryBlock);
                        tNewMBCB.UpdateMemoryBlockEvent += new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                        tNewMBCB.LoadMemoryBlockEvent += new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);

                        //! remove old mbcb
                        m_Blocks.Remove(tLastBlock.StartAddress);
                        tLastBlock.UpdateMemoryBlockEvent -= new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                        tLastBlock.LoadMemoryBlockEvent -= new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);
                        tIndex--;

                        m_Blocks.Remove(tBlock.StartAddress);
                        tBlock.UpdateMemoryBlockEvent -= new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                        tBlock.LoadMemoryBlockEvent -= new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);
                        tIndex--;

                        //! add this new block to list
                        m_Blocks.Add(tNewMBCB.StartAddress, tNewMBCB);

                        //! update last block 
                        tLastBlock = tNewMBCB;
                    }
                }
            }
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

            return Read(tAddress, ref tData, tData.Length);
        }

        public Boolean Verify(MemoryBlock tBlock, ref UInt32 tAddress)
        {
            if (null == tBlock)
            {
                return true;
            }
            else if (0 == tBlock.Size)
            {
                return true;
            }
            else if (null == tBlock.Data)
            {
                return true;
            }

            Byte[] tBuffer = new Byte[tBlock.Size];
            if (!this.Read(tBlock.Address, tBuffer))
            {
                return false;
            }

            for (UInt32 n = 0; n < tBlock.Size; n++)
            {
                if (tBlock.Data[n] != tBuffer[n])
                {
                    tAddress = tBlock.Address + n;
                    return false;
                }
            }

            return true;
        }

        //! \brief read memory block from virtual memory space
        public Boolean Read(UInt32 tAddress, ref Byte[] tData, Int32 tLenght)
        {
            
            if (0 == tLenght) 
            {
                return true;
            }
            if (null == tData)
            {
                tData = new Byte[tLenght];
            }
            else if (tData.Length < tLenght)
            {
                Array.Resize(ref tData, tLenght);
            }

            Int32 tReadIndex = 0;

            foreach (MemoryControlBlock tBlock in m_Blocks.Values)
            {
                if ((tBlock.StartAddress + tBlock.Size)  <= tAddress)
                {
                    continue;
                }
                else if (tBlock.StartAddress > tAddress)
                {
                    while (tBlock.StartAddress > tAddress)
                    {
                        tData[tReadIndex] = m_FillByte;
                        tReadIndex++;
                        tAddress++;

                        if (tReadIndex >= tLenght)
                        {
                            //! read all data
                            return true;
                        }
                    }

                    Int32 tReadCount = tBlock.Read(tAddress, tData, tReadIndex);
                    if (0 == tReadCount)
                    {
                        return false;
                    }
                    tReadIndex += tReadCount;
                    tAddress += (UInt32)tReadCount;

                    if (tReadIndex >= tLenght)
                    {
                        //! read all data
                        break;
                    }
                }
                else
                { 
                    Int32 tReadCount = tBlock.Read(tAddress,tData,tReadIndex);
                    if (0 == tReadCount)
                    {
                        return false;
                    }
                    tReadIndex += tReadCount;
                    tAddress += (UInt32)tReadCount;

                    if (tReadIndex >= tLenght)
                    {
                        //! read all data
                        break;
                    }
                }
            }

            while (tReadIndex < tLenght)
            {
                tData[tReadIndex] = m_FillByte;
                tReadIndex++;
                tAddress++;
            }

            return true;
        }

        //! \brief update virtual memory space
        public Boolean Update()
        {
            //! raising begin update memory space event
            if (!OnBeginUpdateMemorySpace())
            {
                return false;
            }

            //! add code here
            foreach (MemoryControlBlock tBlock in m_Blocks.Values)
            {
                if (tBlock.Update())
                {
                    OnEndUpdateMemorySpace();
                    return false;
                }
            }

            //! raising end update memory space event
            if (!OnEndUpdateMemorySpace())
            {
                return false;
            }

            return true;
        }

        //! \brief virtual memory space size
        public UInt32 Size
        {
            get 
            {
                if (m_MaxSpaceSize > 0)
                {
                    return Math.Min((UInt32)m_SpaceSize, m_MaxSpaceSize);
                }

                return m_SpaceSize;
            }
        }

        public UInt32 SpaceLength
        {
            get
            {
                if (0 != m_MaxSpaceSize)
                {
                    return m_MaxSpaceSize;
                }

                return (UInt32)m_SpaceSize;
            }
            set
            {
                m_MaxSpaceSize = value;
                if (0 != m_MaxSpaceSize)
                {
                    CleanUp(0);
                }
            }
        }

        //! \brief get / set fill byte
        public Byte FillByte
        {
            get { return m_FillByte; }
            set { m_FillByte = value; }
        }

        //! \brief max buffer size
        public Int32 MaxBufferSize
        {
            get { return m_MaxBufferSize; }
            set 
            {
                if (value < 0)
                {
                    m_MaxBufferSize = -1;
                }

                m_MaxBufferSize = value;
            }
        }

        public MemoryBlock[] MemoryBlocks
        { 
            get 
            {
                List<MemoryBlock> tResultList = new List<MemoryBlock>();

                foreach (MemoryControlBlock tItem in m_Blocks.Values)
                {
                    if (tItem.Buffed)
                    {
                        tResultList.Add(tItem.Memory);
                    }
                    else
                    {
                        if (tItem.Update())
                        {
                            tResultList.Add(tItem.Memory);
                        }
                    }
                }

                CleanUp(0);

                return tResultList.ToArray();
            }
        }

        private UInt16 m_Alignment = 1;

        public UInt16 Alignment
        {
            get { return m_Alignment; }
            set
            {
                if (value < 1)
                {
                    m_Alignment = 1;
                }
                else
                {
                    m_Alignment = value;
                }
            }
        }

        public MemoryBlock[] FetchMemoryBlocks(UInt32 tAddress, UInt32 tLength)
        {
            List<MemoryBlock> tResultList = new List<MemoryBlock>();
            if (0 != m_MaxSpaceSize)
            {
                if ((tAddress >= m_MaxSpaceSize) || ((tAddress + tLength) > m_MaxSpaceSize))
                {
                    return tResultList.ToArray();
                }
            }

            do
            {
                MemoryBlock[] tBlocks = this.MemoryBlocks;
                if (null == tBlocks)
                {
                    break;
                }
                foreach (MemoryBlock tBlock in tBlocks)
                {
                    if (tAddress <= tBlock.Address)
                    {
                        if (0 == tLength)
                        { 
                            //! get all blocks whose addresses are larger than tAddress
                            tResultList.Add(tBlock);
                        }
                        else if ((tAddress + tLength) >= (tBlock.Address + tBlock.Size))
                        { 
                            //! in target range
                            tResultList.Add(tBlock);
                        }
                        else if ((tAddress + tLength) > tBlock.Address)
                        { 
                            //! get intersection
                            Byte[] tData = new Byte[tAddress + tLength - tBlock.Address];
                            if (Read(tBlock.Address, tData))
                            {
                                tResultList.Add(new MemoryBlock(tBlock.Address, tData));
                            }
                        }
                    }
                    else if (tAddress < (tBlock.Address + tBlock.Size))
                    {
                        Byte[] tData = null;
                        if ((0 == tLength) || ((tAddress + tLength) >= (tBlock.Address + tBlock.Size)))
                        {
                            //! get right part of this memory block
                            tData = new Byte[tBlock.Address + tBlock.Size - tAddress];
                        }
                        else if ((tAddress + tLength) < (tBlock.Address + tBlock.Size))
                        { 
                            //! target range in this memory block
                            tData = new Byte[tLength];
                        }
                        if (Read(tAddress, tData))
                        {
                            tResultList.Add(new MemoryBlock(tAddress, tData));
                        }
                    }
                }
            }
            while (false);

            if ((1 == m_Alignment) || (0 == tResultList.Count))
            {
                return tResultList.ToArray();
            }

            //! check blocks
            List<MemoryBlock> tMergedBlockList = new List<MemoryBlock>();
            Boolean tMerged = false;
            Int32 tIndex = 0;
            if (0 == tResultList[0].Address % m_Alignment)
            {
                tMergedBlockList.Add(tResultList[0]);
                tResultList.RemoveAt(0);
            }
            else 
            {
                MemoryBlock tBlock = tResultList[0];
                UInt32 tBufferSize = (UInt32)(tBlock.Address % m_Alignment) + (UInt32)tBlock.Size;

                Byte[] tBuffer = new Byte[tBufferSize];
                for (Int32 n = 0;n < tBuffer.Length;n++)
                {
                    tBuffer[n] = m_FillByte;
                }

                Array.Copy(tBlock.Data,0,tBuffer,tBlock.Address % m_Alignment,tBlock.Size);

                tMergedBlockList.Add(new MemoryBlock((tBlock.Address - (tBlock.Address % m_Alignment)),tBuffer));
                tResultList.RemoveAt(0);
            }

            //do
            {
                //tMerged = false;
                do
                {
                    if (0 == tResultList.Count)
                    {
                        break;
                    }

                    MemoryBlock tBlock = tMergedBlockList[tIndex];
                    UInt32 tBlockAddress = tBlock.Address + (UInt32)tBlock.Size + m_Alignment;
                    tBlockAddress -= tBlockAddress % m_Alignment;
                    UInt32 tTargetAddress = tResultList[0].Address;
                    tTargetAddress -= tTargetAddress % m_Alignment;

                    if (tBlockAddress >= tTargetAddress)
                    {
                        //! we should merge this two block
                        tBlock.Append(tResultList[0]);
                        tResultList.RemoveAt(0);
                        //tMerged = true;
                    }
                    else
                    {
                        MemoryBlock tTargetBlock = tResultList[0];
                        UInt32 tBufferSize = (UInt32)(tTargetBlock.Address % m_Alignment) + (UInt32)tTargetBlock.Size;

                        Byte[] tBuffer = new Byte[tBufferSize];
                        for (Int32 n = 0; n < tBuffer.Length; n++)
                        {
                            tBuffer[n] = m_FillByte;
                        }

                        Array.Copy(tTargetBlock.Data, 0, tBuffer, tTargetBlock.Address % m_Alignment, tTargetBlock.Size);

                        tMergedBlockList.Add(new MemoryBlock((tTargetBlock.Address - (tTargetBlock.Address % m_Alignment)), tBuffer));
                        tResultList.RemoveAt(0);
                        tIndex++;
                    }
                } while (true);
            }
            //while (tMerged);

            do
            {
                MemoryBlock tTargetBlock = tMergedBlockList[tIndex];
                if ((tTargetBlock.Address % m_Alignment) == 0)
                {
                    break;
                }
                UInt32 tBufferSize = (UInt32)(tTargetBlock.Address % m_Alignment) + (UInt32)tTargetBlock.Size + m_Alignment;
                tBufferSize -= tBufferSize % m_Alignment;
                Byte[] tBuffer = new Byte[tBufferSize];
                for (Int32 n = 0; n < tBuffer.Length; n++)
                {
                    tBuffer[n] = m_FillByte;
                }

                Array.Copy(tTargetBlock.Data, 0, tBuffer, tTargetBlock.Address % m_Alignment, tTargetBlock.Size);

                tMergedBlockList.Remove(tTargetBlock);
                tMergedBlockList.Add(new MemoryBlock(tTargetBlock.Address - (tTargetBlock.Address % m_Alignment), tBuffer));
            }
            while (false);

            return tMergedBlockList.ToArray();
        }

        //! \brief 
        public static MemoryBlock Merge(params MemoryBlock[] tBlocks)
        {
            if (null == tBlocks)
            {
                return null;
            }
            else if (0 == tBlocks.Length)
            {
                return null;
            }
            else if (1 == tBlocks.Length)
            {
                return tBlocks[0];
            }

            //! find a range of new block
            MemoryBlock tStartMemoryBlock = tBlocks[0];
            MemoryBlock tEndMemoryBlock = tBlocks[0];

            foreach (MemoryBlock tItem in tBlocks)
            {
                if (tItem.Address < tStartMemoryBlock.Address)
                {
                    tStartMemoryBlock = tItem;
                }
                if (tItem.Address > tEndMemoryBlock.Address)
                {
                    tEndMemoryBlock = tItem;
                }
            }

            //! create a new blockk
            MemoryBlock tNewBlock = new MemoryBlock
                                        (
                                            tStartMemoryBlock.Address,
                                            (int)(tEndMemoryBlock.Address - tStartMemoryBlock.Address + tEndMemoryBlock.Size)
                                        );

            //! append each block into new block
            foreach (MemoryBlock tItem in tBlocks)
            {
                if (!tNewBlock.Append(tItem))
                {
                    return null;
                }
            }

            return tNewBlock;
        }

        //! \brief write memory block to virtual memory space
        protected Boolean LoadMemory(UInt32 tAddress, Byte[] tData)
        {
            if (null == tData)
            {
                return false;
            }

            Boolean bMemoryAdded = false;

            if (0 != m_MaxSpaceSize)
            {
                if (tAddress >= m_MaxSpaceSize)
                {
                    return false;
                }
                /*
                else if (tAddress + tData.Length >= m_MaxSpaceSize)
                {
                    return false;
                }
                */
            }

            foreach (KeyValuePair<UInt32, MemoryControlBlock> tBlock in m_Blocks)
            {
                if (null == tBlock.Value)
                {
                    continue;
                }


                if (tAddress < tBlock.Key)
                {

                    //! invade into (or connect to ) a exitting one , so we try to merge this block
                    MemoryControlBlock tNewMBCB = new MemoryControlBlock
                                                            (
                                                                new MemoryBlock(tAddress, tData.Length, tData)
                                                            );
                    tNewMBCB.UpdateMemoryBlockEvent += new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                    tNewMBCB.LoadMemoryBlockEvent += new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);

                    m_Blocks.Add(tAddress, tNewMBCB);

                    bMemoryAdded = true;

                    //! before a exist block
                    if ((tAddress + tData.Length) >= tBlock.Key)
                    {
                        CleanUp(tAddress);
                        //break;
                    }
                    break;
                }
                else if (tAddress > tBlock.Key)
                {
                    if (tAddress <= (tBlock.Value.StartAddress + tBlock.Value.Size))
                    {
                        if (null == tBlock.Value.Memory)
                        {
                            continue;
                        }

                        tBlock.Value.Memory.Append(tAddress, tData);
                        tBlock.Value.Refresh();
                        CleanUp(tAddress);

                        bMemoryAdded = true;

                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    if (null == tBlock.Value.Memory)
                    {
                        continue;
                    }

                    //! append memory block to a exist memory block
                    tBlock.Value.Memory.Append(tAddress, tData);
                    tBlock.Value.Refresh();
                    CleanUp(tAddress);

                    bMemoryAdded = true;

                    break;
                }
            }

            if (!bMemoryAdded)
            {
                //! a individual memory block
                MemoryControlBlock tNewMBCB = new MemoryControlBlock
                                                            (
                                                                new MemoryBlock(tAddress, tData.Length, tData)
                                                            );
                tNewMBCB.UpdateMemoryBlockEvent += new MemoryControlBlock.UpdateMemoryBlock(OnUpdateMemorySpace);
                tNewMBCB.LoadMemoryBlockEvent += new MemoryControlBlock.LoadMemoryBlock(OnLoadMemoryBlock);

                m_Blocks.Add(tAddress, tNewMBCB);
            }

            ManageBuffer();

            return true;
        }

        #region constructors
        public VirtualMemorySpaceImage()
        { }

        public VirtualMemorySpaceImage(UInt32 tAddress, Byte[] tData)
        {
            LoadMemory(tAddress, tData);
        }

        

        public VirtualMemorySpaceImage(MemoryBlock tBlock)
        {
            LoadMemory(tBlock.Address, tBlock.Data);
        }

        public VirtualMemorySpaceImage(MemoryBlock[] tBlocks)
        {
            if (null == tBlocks)
            {
                return;
            }

            foreach (MemoryBlock tBlock in tBlocks)
            {
                LoadMemory(tBlock.Address, tBlock.Data);
            }
        }


        public VirtualMemorySpaceImage(MemoryBlock[] tBlocks, MemoryDescription[] tDescriptions)
        {
            if (null == tBlocks)
            {
                return;
            }

            foreach (MemoryBlock tBlock in tBlocks)
            {
                LoadMemory(tBlock.Address, tBlock.Data);
            }

            if (null == tDescriptions)
            {
                this.LoadDescription(tDescriptions);
            }
        }

        protected Boolean LoadDescription(MemoryDescription[] tDescriptions)
        {
            if (null == tDescriptions)
            {
                return false;
            }

            foreach (MemoryDescription tItem in tDescriptions)
            {
                if (null == tItem)
                {
                    return false;
                }
                else if ((tItem.Address < this.StartAddress) || tItem.Address > (this.StartAddress + this.Size))
                {
                    return false;
                }
            }

            foreach (MemoryDescription tItem in tDescriptions)
            {
                m_DescriptionList.Add(tItem.Address, tItem);
            }

            return true;
        }

        #endregion



        #region Memory Description
        protected SortedList<UInt32, MemoryDescription> m_DescriptionList = new SortedList<UInt32, MemoryDescription>();

        public MemoryDescription[] Descriptions
        {
            get
            {
                List<MemoryDescription> tList = new List<MemoryDescription>(m_DescriptionList.Values);
                return tList.ToArray();
            }
        }

        #region Read description
        public MemoryDescription ReadDescription(UInt32 wAddress)
        {
            Int32 nIndex = m_DescriptionList.IndexOfKey(wAddress);
            if (nIndex >= 0)
            {
                return m_DescriptionList.Values[nIndex];
            }

            foreach (MemoryDescription tItem in m_DescriptionList.Values)
            {
                if (tItem.IsInRange(wAddress))
                {
                    return tItem;
                }
            }

            return null;
        }
        #endregion

        #endregion

    }

    public sealed class VirtualMemorySpace : VirtualMemorySpaceImage, IMemoryWriter
    {
        #region constructors
        public VirtualMemorySpace(UInt32 tAddress, Byte[] tData)
            :base (tAddress,tData)
        { }

        public VirtualMemorySpace(MemoryBlock tBlock)
            :base(tBlock)
        { }

        public VirtualMemorySpace(MemoryBlock[] tBlocks)
            :base(tBlocks)
        { }

        public VirtualMemorySpace(MemoryBlock[] tBlocks, MemoryDescription[] tDescriptions) 
            : base(tBlocks, tDescriptions)
        { }

        public VirtualMemorySpace() 
            : base ()
        { }
        #endregion

        public Boolean Write(MemoryBlock tBlock)
        {
            if (null == tBlock)
            {
                return false;
            }
            else if (null == tBlock.Data)
            {
                return false;
            }

            return LoadMemory(tBlock.Address, tBlock.Data);
        }

        

        public Boolean Write(MemoryBlock[] tBlocks)
        {
            bool tResult = true;
            if (null == tBlocks)
            {
                return false;
            }

            foreach (MemoryBlock tBlock in tBlocks)
            {
                tResult &= Write(tBlock);
            }

            return tResult;
        }

        public Boolean Write(UInt32 tAddress, Byte[] tData)
        {
            return LoadMemory(tAddress, tData);
        }

        public Boolean Write(UInt32 tAddress, Byte tData)
        {
            return LoadMemory(tAddress, new Byte[] { tData });
        }

        public Boolean Write(UInt32 tAddress, UInt16 tData)
        {
            return LoadMemory(tAddress, BitConverter.GetBytes(tData));
        }

        public Boolean Write(UInt32 tAddress, UInt32 tData)
        {
            return LoadMemory(tAddress, BitConverter.GetBytes(tData));
        }

        public Boolean Write(UInt32 tAddress, UInt64 tData)
        {
            return LoadMemory(tAddress, BitConverter.GetBytes(tData));
        }

        //! \brief erase all memory space
        public Boolean EraseAll()
        {
            m_Blocks.Clear();
            Update();
            return true;
        }

        //! \brief fill a block of memory with specified byte
        public Boolean Fill(UInt32 tStartAddress, Int32 tLength, Byte tFillByte)
        {
            if (tLength < 0)
            {
                return false;
            }
            else if (0 == tLength)
            {
                return true;
            }

            Byte[] tFillBuffer = new Byte[tLength];
            for (Int32 n = 0; n < tLength; n++)
            {
                tFillBuffer[n] = tFillByte;
            }

            return Write(tStartAddress, tFillBuffer);
        }

        //! \brief fill a block of memory with default fillByte
        public Boolean Fill(UInt32 tStartAddress, Int32 tLength)
        {
            return Fill(tStartAddress, tLength, m_FillByte);
        }


        #region write description

        public Boolean WriteDescription(MemoryDescription[] tDescriptions)
        {
            return LoadDescription(tDescriptions);
        }

        public Boolean WriteDescription(MemoryDescription tDescription)
        {
            if (null == tDescription)
            {
                return false;
            }
            else if ((tDescription.Address < this.StartAddress) || tDescription.Address > (this.StartAddress + this.Size))
            {
                return false;
            }

            m_DescriptionList.Add(tDescription.Address, tDescription);

            return true;
        }


        public Boolean WriteDescription(UInt32 wAddress, UInt32 wSize, String tDescription, Object tObj)
        {
            return WriteDescription(new MemoryDescription(wAddress, wSize, tDescription, tObj));
        }

        public Boolean WriteDescription(UInt32 wAddress, String tDescription, Object tObj)
        {
            return WriteDescription(new MemoryDescription(wAddress, tDescription, tObj));
        }

        public Boolean WriteDescription(UInt32 wAddress, UInt32 wSize, String tDescription)
        {
            return WriteDescription(new MemoryDescription(wAddress, wSize, tDescription));
        }

        public Boolean WriteDescription(UInt32 wAddress, String tDescription)
        {
            return WriteDescription(new MemoryDescription(wAddress, tDescription));
        }

        public Boolean WriteDescription(UInt32 wAddress, UInt32 wSize, Object tObj)
        {
            return WriteDescription(new MemoryDescription(wAddress, wSize, tObj));
        }

        public Boolean WriteDescription(UInt32 wAddress, Object tObj)
        {
            return WriteDescription(new MemoryDescription(wAddress, tObj));
        }
        #endregion

    }
}
