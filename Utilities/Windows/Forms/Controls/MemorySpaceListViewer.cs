using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities;
using ESnail.Utilities.IO;
using ESnail.Utilities.Generic;

namespace ESnail.Utilities.Windows.Forms.Controls
{
    public partial class MemorySpaceListViewer : UserControl
    {
        public MemorySpaceListViewer()
        {
            InitializeComponent();
        }

        public MemorySpaceListViewer(VirtualMemorySpaceImage tImage)
        {
            InitializeComponent();

            this.Image = tImage;
        }

        private VirtualMemorySpaceImage m_Image = null;

        public VirtualMemorySpaceImage Image
        {
            get { return m_Image; }
            set
            {
                if (null == value)
                {
                    return;
                }

                m_Image = value;
                RefreshListViewItemNumber();
            }
        }

        #region LineSize
        private UInt32 m_LineSize = 32;

        public UInt32 LineSize
        {
            get
            {
                return m_LineSize;
            }
            set
            {
                m_LineSize = (value + 3) & 0xFFFFFFFC;
                if (m_LineSize == 0)
                {
                    m_LineSize = 4;
                }

                RefreshListViewItemNumber();
            }
        }
        #endregion
        private void RefreshListViewItemNumber()
        {
            do
            {
                if (null == this.Image)
                {
                    break;
                }
                if (0 == this.Image.SpaceLength)
                {
                    lvMemory.VirtualListSize = 0;
                    break;
                }

                lvMemory.VirtualMode = true;
                lvMemory.VirtualListSize = (int)(this.Image.SpaceLength / this.LineSize);
            }
            while (false);
        }

        #region list view cache

        #region cacheline
        private class ListViewCacheLine : ISafeID, IAvailable
        {
            private ListViewItem m_Item = null;
            private UInt32 m_wAddress = 0;

            private Boolean m_Available = false;

            public ListViewCacheLine(UInt32 tAddress, ListViewItem tItem)
            {
                do
                {
                    m_wAddress = tAddress;
                    if (null == tItem)
                    {
                        break;
                    }
                    m_Item = tItem;

                    m_Available = true;
                    return;
                }
                while (false);
            }

            public UInt32 Address
            {
                get { return m_wAddress; }
            }

            public SafeID ID
            {
                get { return this.Address.ToString("X8"); }
                set { }
            }

            public ListViewItem Tag
            {
                get { return m_Item; }
            }

            public bool Available
            {
                get { return m_Available; }
            }
        }
        #endregion

        private TSet<ListViewCacheLine> m_ListViewCache = new TSet<ListViewCacheLine>();


        #endregion


        private void lvMemory_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            do
            {
                if (null == this.Image)
                {
                    break;
                }
                UInt32 tStartAddress = (UInt32)e.ItemIndex * this.LineSize;

                ListViewCacheLine tCacheLineItem = m_ListViewCache.Find(tStartAddress.ToString("X8"));
                if (null == tCacheLineItem)
                {
                    //! a cache miss

                    Byte[] tBuffer = new Byte[this.LineSize];
                    this.Image.Read(tStartAddress, tBuffer);

                    
                    ListViewItem tItem = new ListViewItem(tStartAddress.ToString("X8"));
                    tItem.SubItems.Add(HEX.HEXBuilder.ByteArrayToHEXString(tBuffer));
                    tItem.SubItems.Add(BuildDisplayString(tBuffer));


                    tCacheLineItem = new ListViewCacheLine(tStartAddress, tItem);

                    m_ListViewCache.Add(tCacheLineItem);

                }


                e.Item = tCacheLineItem.Tag;


            } while (false);

        }

        private String BuildDisplayString(Byte[] tBuffer)
        {

            do
            {
                if (null == tBuffer)
                {
                    break;
                }
                else if (0 == tBuffer.Length)
                {
                    break;
                }

                StringBuilder sbLine = new StringBuilder();

                foreach (Byte tByte in tBuffer)
                {
                    if (    (Char.IsControl((char)tByte))
                        ||  (tByte > 0x7F))
                    {
                        sbLine.Append(".");
                    }
                    else 
                    {
                        sbLine.Append((Char)tByte);
                    }
                }

                return sbLine.ToString();
            }
            while (false);
            return "";
        }

        private void lvMemory_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            do
            {
                if (null == this.Image)
                {
                    break;
                }

                UInt32 tStartAddress = (UInt32)e.StartIndex * this.LineSize;

                Int32 tLength = (e.EndIndex - e.StartIndex + 1) * (Int32)this.LineSize;
                

                Byte[] tBuffer = null;
                if (!this.Image.Read(tStartAddress, ref tBuffer, tLength))
                {
                    break;
                }
                
                MemoryBlock tBlock = new MemoryBlock(tStartAddress, tBuffer);
                ListViewCacheLine tCacheLine = null;
                for (UInt32 tOffset = 0; tOffset < tBlock.Size; tOffset += this.LineSize)
                {
                    Byte[] tTempLine = null;
                    UInt32 wAddress = tStartAddress + tOffset;
                    if (!tBlock.Read(wAddress, ref tTempLine, (Int32)this.LineSize))
                    {
                        continue;
                    }

                    ListViewItem tItem = new ListViewItem(wAddress.ToString("X8"));
                    tItem.SubItems.Add(HEX.HEXBuilder.ByteArrayToHEXString(tTempLine));
                    tItem.SubItems.Add(BuildDisplayString(tTempLine));
                    tCacheLine = new ListViewCacheLine(wAddress, tItem);
                    m_ListViewCache.Add(tCacheLine);
                }

                m_ListViewCache.RemoveBefore(tStartAddress.ToString("X8"));

                m_ListViewCache.RemoveAfter(tCacheLine.ID);

            } while (false);
        }

        private void lvMemory_SearchForVirtualItem(object sender, SearchForVirtualItemEventArgs e)
        {
        }
    }
}
