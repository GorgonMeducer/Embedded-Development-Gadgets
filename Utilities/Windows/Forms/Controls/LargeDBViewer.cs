using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows.Forms.Controls
{
    public  partial  class LargeDBViewer : UserControl
    {
        public LargeDBViewer()
        {
            InitializeComponent();
        }

        #region LineSize
        private UInt32 m_LineSize = 16;

        public virtual UInt32 LineSize
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

        protected Int32 DisplayedWindowStart
        {
            get { return m_DisplayedWindowStart; }
        }

        public Int32 DisplayedWindowLength
        {
            get { return m_DisplayedWindowLength; }
            set { m_DisplayedWindowLength = value; }
        }

        public UInt32 Postion
        {
            get { return (UInt32)(vbItems.Value * this.LineSize); }
            set
            {
                Int32 tValue = (Int32)(value / this.LineSize);
                if (tValue < vbItems.Minimum)
                {
                    vbItems.Value = vbItems.Minimum;
                }
                else if (tValue > vbItems.Maximum)
                {
                    vbItems.Value = vbItems.Maximum;
                }
                else
                {
                    vbItems.Value = tValue;
                }
            }
        }

        protected virtual Boolean IsDataAvailable
        {
            get;
        }

        protected virtual UInt32 DataRecordStart
        {
            get;
        }

        protected virtual UInt32 DataRecordMax
        {
            get;
        }

        private void UpdateDisplayWindow()
        {
            if (!this.IsDataAvailable)
            {
                return;
            }

            Boolean tWindowChanged = false;
            dgMemory.RowCount = this.DisplayedWindowLength;
            vbItems.Maximum = this.MaxRows;// - dgMemory.DisplayedRowCount(false);

            vbItems.Minimum = (int)(this.DataRecordStart / this.LineSize);
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
                if (!this.IsDataAvailable)
                {
                    return 0;
                }

                return (Int32)((this.DataRecordMax + this.LineSize - 1) / this.LineSize);
            }
        }

        protected void RefreshListViewItemNumber()
        {
            do
            {
                if (!this.IsDataAvailable)
                {
                    break;
                }
                if (0 == this.DataRecordMax)
                {
                    this.dgMemory.RowCount = 0;
                    break;
                }

                this.UpdateDisplayWindow();
                dgMemory.Refresh();
            }
            while (false);
        }


        protected virtual void dgMemory_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e) { }

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
    }
}
