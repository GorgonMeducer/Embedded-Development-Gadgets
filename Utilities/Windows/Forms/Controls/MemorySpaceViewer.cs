using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities;
using ESnail.Utilities.IO;

namespace ESnail.Utilities.Windows.Forms.Controls
{
    public partial class MemorySpaceViewer : LargeDBViewer
    {
        public MemorySpaceViewer()
        {
            InitializeComponent();
        }

        public MemorySpaceViewer(VirtualMemorySpaceImage tImage)
        {
            InitializeComponent();

            this.Image = tImage;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RefreshMenu();
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
                this.RefreshListViewItemNumber();
            }
        }

        protected override bool IsDataAvailable
        {
            get { return this.Image != null; }
        }


        protected override UInt32 DataRecordStart
        {
            get
            {
                if (null == this.Image)
                {
                    return 0;
                }

                return this.Image.StartAddress;
            }
        }

        protected override UInt32 DataRecordMax
        {
            get {
                if (null == this.Image)
                {
                    return 0;
                }

                return this.Image.SpaceLength;
            }
        }



#if false
        #region LineSize
        private UInt32 m_LineSize = 16;

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

        private Int32 m_DisplayedWindowStart = 0;
        private Int32 m_DisplayedWindowLength = 1000;

        private Int32 DisplayedWindowStart
        {
            get { return m_DisplayedWindowStart; }
        }

        public Int32 DisplayedWindowLength
        {
            get { return m_DisplayedWindowLength; }
            set { m_DisplayedWindowLength = value; }
        }

        

        private void UpdateDisplayWindow()
        {
            if (null == this.Image)
            {
                return;
            }

            Boolean tWindowChanged = false;
            dgMemory.RowCount = this.DisplayedWindowLength;
            vbItems.Maximum = this.MaxRows;// - dgMemory.DisplayedRowCount(false);

            vbItems.Minimum = (int)(this.Image.StartAddress / this.LineSize);
            if (m_DisplayedWindowStart < vbItems.Minimum)
            {
                m_DisplayedWindowStart = vbItems.Minimum;
            }


            vbItems.SmallChange = 1;
            vbItems.LargeChange = dgMemory.DisplayedRowCount(false);

            if (vbItems.Value < DisplayedWindowStart)
            {
                m_DisplayedWindowStart = vbItems.Value;
                tWindowChanged = true;
            }
            else if ((vbItems.Value + dgMemory.DisplayedRowCount(false)) > (this.DisplayedWindowStart + this.DisplayedWindowLength))
            {
                tWindowChanged = true;
                m_DisplayedWindowStart = vbItems.Value + dgMemory.DisplayedRowCount(false) - DisplayedWindowLength;
            }

            if (tWindowChanged)
            {
                dgMemory.Refresh();
            }

        }

        public Int32 MaxRows
        {
            get
            {
                if (null == this.Image)
                {
                    return 0;
                }
            
                return (Int32)((this.Image.SpaceLength + this.LineSize - 1) / this.LineSize);
            }
        }

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
                    this.dgMemory.RowCount = 0;
                    break;
                }

                this.UpdateDisplayWindow();
                dgMemory.Refresh();
            }
            while (false);
        }

        


        

        

        private void dgMemory_Resize(object sender, EventArgs e)
        {
            this.UpdateDisplayWindow();
        }

        private void vbItems_ValueChanged(object sender, EventArgs e)
        {
            do
            {
                try
                {
                    if (vbItems.Value < vbItems.Minimum)
                    {
                        break;
                    }
                    else if (vbItems.Value > vbItems.Maximum)
                    {
                        break;
                    }

                    this.UpdateDisplayWindow();

                    dgMemory.FirstDisplayedScrollingRowIndex = vbItems.Value - this.DisplayedWindowStart;

                }
                catch (Exception) { }
            } while (false);
        }
#endif

        public UInt32 Address
        {
            get { return this.Postion; }
            set
            {
                this.Postion = value;
            }
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
                    if ((Char.IsControl((char)tByte))
                        || (tByte > 0x7F))
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

        protected override void dgMemory_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            do
            {
                if (null == this.Image)
                {
                    break;
                }

                UInt32 tStartAddress = (UInt32)(e.RowIndex + this.DisplayedWindowStart) * this.LineSize;

                switch (e.ColumnIndex)
                {
                    case 0:

                        e.Value = tStartAddress.ToString("X8");
                        break;

                    case 1:
                        do
                        {
                            Byte[] tBuffer = new Byte[this.LineSize];
                            this.Image.Read(tStartAddress, tBuffer);

                            e.Value = HEX.HEXBuilder.ByteArrayToHEXString(tBuffer, this.DisplayDataSize);

                        } while (false);

                        break;
                    case 2:
                        do
                        {
                            Byte[] tBuffer = new Byte[this.LineSize];
                            this.Image.Read(tStartAddress, tBuffer);

                            e.Value = BuildDisplayString(tBuffer);
                        } while (false);
                        break;
                }
            }
            while (false);

        }

        private DATA_SIZE m_DisplayDataSize = DATA_SIZE.DATA_SIZE_BYTE;

        public DATA_SIZE DisplayDataSize
        {
            get { return m_DisplayDataSize; }
            set
            {
                m_DisplayDataSize = value;
                dgMemory.Refresh();

                RefreshMenu();
            }

        }

        private void RefreshMenu()
        {
            byteToolStripMenuItem.Checked = false;
            halfword16bitsToolStripMenuItem.Checked = false;
            word32BitsToolStripMenuItem.Checked = false;
            doubleWord64BitsToolStripMenuItem.Checked = false;

            switch (m_DisplayDataSize)
            {
                case DATA_SIZE.DATA_SIZE_BYTE:
                    byteToolStripMenuItem.Checked = true;
                    break;
                case DATA_SIZE.DATA_SIZE_HALF_WORD:
                    halfword16bitsToolStripMenuItem.Checked = true;
                    break;
                case DATA_SIZE.DATA_SIZE_WORD:
                    word32BitsToolStripMenuItem.Checked = true;
                    break;
                case DATA_SIZE.DATA_SIZE_DOUBLE_WORD:
                    doubleWord64BitsToolStripMenuItem.Checked = true;
                    break;
            }
        }


        public override void Refresh()
        {
            RefreshMenu();

            base.Refresh();

        }

        private void byteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DisplayDataSize = DATA_SIZE.DATA_SIZE_BYTE;
        }

        private void halfword16bitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DisplayDataSize = DATA_SIZE.DATA_SIZE_HALF_WORD;
        }

        private void word32BitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DisplayDataSize = DATA_SIZE.DATA_SIZE_WORD;
        }

        private void doubleWord64BitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DisplayDataSize = DATA_SIZE.DATA_SIZE_DOUBLE_WORD;
        }

        private void copyDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == dgMemory.SelectedRows)
            {
                return;
            }
            else if (0 == dgMemory.SelectedRows.Count)
            {
                return;
            }
            StringBuilder sbOutput = new StringBuilder();
            Stack<String> tStack = new Stack<string>();
            Int32 tCount = 0;
            foreach (DataGridViewRow tView in dgMemory.SelectedRows)
            {
                tStack.Push(tView.Cells[1].FormattedValue as String);
                tCount++;
            }

            do
            {
                sbOutput.Append(tStack.Pop());
            } while (--tCount > 0);

            Clipboard.Clear();
            Clipboard.SetText(sbOutput.ToString());

        }

        private void copyASCIIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null == dgMemory.SelectedRows)
            {
                return;
            }
            else if (0 == dgMemory.SelectedRows.Count)
            {
                return;
            }
            StringBuilder sbOutput = new StringBuilder();
            Stack<String> tStack = new Stack<string>();
            Int32 tCount = 0;
            foreach (DataGridViewRow tView in dgMemory.SelectedRows)
            {
                tStack.Push(tView.Cells[2].FormattedValue as String);
                tCount++;
            }

            do
            {
                sbOutput.Append(tStack.Pop());
            } while (--tCount > 0);

            Clipboard.Clear();
            Clipboard.SetText(sbOutput.ToString());
        }
    }
}
