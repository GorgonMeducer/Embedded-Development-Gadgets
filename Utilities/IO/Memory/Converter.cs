using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Utilities.IO
{
    public class MemorySpaceConverter 
    {
        public interface IConverter
        {
            MemoryBlock Convert(MemoryBlock tBlock, UInt32 wAddress);
            MemoryBlock InverseConvert(MemoryBlock tBlock, UInt32 wAddress);
            UInt16 Alignment
            {
                get;
            }
        }


        private VirtualMemorySpace m_tMemorySpace = null;
        private IConverter m_tConverter = null;
        private Boolean m_bAvailable = false;
        private VirtualMemorySpace m_tConvertedMemorySpace = new VirtualMemorySpace();

        #region constructor
        public MemorySpaceConverter(VirtualMemorySpace tSpace, IConverter tConverter)
        {
            m_tMemorySpace = tSpace;
            m_tConverter = tConverter;

            Initialization();
        }

        private void Initialization()
        {
            if (null == m_tMemorySpace)
            {
                return;
            }
            else if (null == m_tConverter)
            {
                return;
            }

            m_bAvailable = true;
            
            Refresh();
        }
        #endregion


        public IConverter Convertor
        {
            get { return m_tConverter; }
            set 
            {
                m_tConverter = value;
                if (null == m_tConverter)
                {
                    m_bAvailable = false;
                }
            }
        }

        public void Refresh()
        {
            if (null == m_tMemorySpace || null == m_tConverter)
            {
                return;
            }

            VirtualMemorySpace tMemorySpace = new VirtualMemorySpace();
            //! update alignment
            tMemorySpace.Alignment = m_tConverter.Alignment;

            //! get all blocks
            MemoryBlock[] tBlocks = m_tMemorySpace.MemoryBlocks;

            //! erase all
            if (null == tBlocks)
            {
                return;
            }
            else if (0 == tBlocks.Length)
            {
                return;
            }

            //! convert blocks
            foreach (MemoryBlock tBlock in tBlocks)
            {
                UInt32 wAlignment = m_tConverter.Alignment;
                UInt32 wAddress = tBlock.Address;
                wAddress -= wAddress % wAlignment;
                for (UInt32 n = 0; n < tBlock.Size; n += wAlignment)
                {
                    MemoryBlock tConvertedBlock = m_tConverter.Convert(tBlock, n + wAddress);
                    tMemorySpace.Write(tConvertedBlock);
                }
            }

            m_tConvertedMemorySpace = tMemorySpace;
        }

        public VirtualMemorySpaceImage ConvertedMemorySpace
        {
            get { return m_tConvertedMemorySpace; }
        }

        public Boolean Available
        {
            get 
            {
                return m_bAvailable;
            }
        }

        public UInt32 Alignment
        {
            get
            {
                if (null == m_tConverter)
                {
                    return 0;
                }

                return m_tConverter.Alignment;
            }
        }

        public Boolean Load(UInt32 wAddress, Byte[] tBuffer)
        {
            if (null == tBuffer)
            {
                return false;
            }
            else if (0 == tBuffer.Length)
            {
                return true;
            }
            else if (null == m_tMemorySpace)
            {
                return false;
            }

            do
            {
                UInt32 wAlignment = m_tConverter.Alignment;
                MemoryBlock tBlock = new MemoryBlock(wAddress, tBuffer);
                wAddress -= wAddress % wAlignment;
                for (UInt32 n = 0; n < tBuffer.Length; n += wAlignment)
                {
                    MemoryBlock tConvertedBlock = m_tConverter.InverseConvert(tBlock, n + wAddress);
                    m_tMemorySpace.Write(tConvertedBlock);
                }

            } while (false);

            Refresh();

            return true;
        }

    }


}