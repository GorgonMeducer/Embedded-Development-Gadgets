using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows.Forms.Controls
{
    public enum ORDERLIST_ORDER
    { 
        ORDERLIST_TOP,
        ORDERLIST_BOTTOM
    }


    [ToolboxBitmap(typeof(OrderList), "OrderList.bmp")]
    public partial class OrderList : UserControl
    {
        private SortedList<Int32, OrderListItem> m_SortedList = new SortedList<Int32, OrderListItem>();

        private Boolean m_ShowUpButton = true;
        private Boolean m_ShowDownButton = true;
        private Boolean m_ShowTopButton = true;
        private Boolean m_ShowBottomButton = true;
        private Boolean m_ShowRemoveButton = true;


        public OrderList()
        {
            InitializeComponent();
            /*
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            */
        }

        [Description("Indicates whether contents are wrapped or clipped at the control boundary.")]
        [DefaultValue(false)]
        [Category("Layout")]
        public Boolean WrapContents
        {
            get { return flpOrderList.WrapContents; }
            set 
            {
                flpOrderList.WrapContents = value;
            }
        }

        [Browsable(false)]
        public OrderListItem[] Items
        {
            get 
            {
                List<OrderListItem> tResultList = new List<OrderListItem>();
                tResultList.AddRange(m_SortedList.Values);
                return tResultList.ToArray();
            }
        }

        #region Buttons' properties

        [Description("Show Up button when displaying list items")]
        [DefaultValue(true)]
        [Category("Button")]
        public Boolean UpButton
        {
            get {return m_ShowUpButton;}
            set
            {
                if (value != m_ShowUpButton)
                {
                    m_ShowUpButton = value;

                    RefreshOrderList();
                }
            }
        }

        [Description("Show Down button when displaying list items")]
        [DefaultValue(true)]
        [Category("Button")]
        public Boolean DownButton
        {
            get { return m_ShowDownButton; }
            set
            {
                if (value != m_ShowDownButton)
                {
                    m_ShowDownButton = value;

                    RefreshOrderList();
                }
            }
        }

        [Description("Show Top button when displaying list items")]
        [DefaultValue(true)]
        [Category("Button")]
        public Boolean TopButton
        {
            get { return m_ShowTopButton; }
            set
            {
                if (value != m_ShowTopButton)
                {
                    m_ShowTopButton = value;

                    RefreshOrderList();
                }
            }
        }

        [Description("Show Bottom button when displaying list items")]
        [DefaultValue(true)]
        [Category("Button")]
        public Boolean BottomButton
        {
            get { return m_ShowBottomButton; }
            set
            {
                if (value != m_ShowBottomButton)
                {
                    m_ShowBottomButton = value;

                    RefreshOrderList();
                }
            }
        }

        [Description("Show Remove button when displaying list items")]
        [DefaultValue(true)]
        [Category("Button")]
        public Boolean RemoveButton
        {
            get { return m_ShowRemoveButton; }
            set
            {
                if (value != m_ShowRemoveButton)
                {
                    m_ShowRemoveButton = value;

                    RefreshOrderList();
                }
            }
        }

        #endregion


        public void RefreshOrderList()
        {
            flpOrderList.Visible = false;
            flpOrderList.Controls.Clear();
            foreach (OrderListItem tItem in m_SortedList.Values)
            {
                //! buttons state
                if (null != tItem.TargetPanel)
                {
                    tItem.TargetPanel.UpButton = m_ShowUpButton;
                    tItem.TargetPanel.DownButton = m_ShowDownButton;
                    tItem.TargetPanel.TopButton = m_ShowTopButton;
                    tItem.TargetPanel.BottomButton = m_ShowBottomButton;
                    tItem.TargetPanel.RemoveButton = m_ShowRemoveButton;

                    flpOrderList.Controls.Add(tItem.TargetPanel);
                }
            }
            flpOrderList.Visible = true;
        }

        public Boolean Add(Control[] tControls)
        {
            return Add(tControls, ORDERLIST_ORDER.ORDERLIST_BOTTOM);
        }

        public Boolean Add()
        {
            return Add(null, ORDERLIST_ORDER.ORDERLIST_BOTTOM);
        }

        public Boolean Add(Control[] tControls,ORDERLIST_ORDER tOrder)
        {
            OrderListItem tItem = new OrderListItem(tControls);
            

            if (0 == m_SortedList.Count)
            {
                m_SortedList.Add(0, tItem);                     //!< first item
                //flpOrderList.Controls.Add(tItem.TargetPanel);
            }
            else
            {
                Int32 tKey = 0;
                switch (tOrder)
                {
                    case ORDERLIST_ORDER.ORDERLIST_TOP:
                        tKey = m_SortedList.Keys[0] - 1;        //!< top item
                        //RefreshOrderList();
                        break;
                    case ORDERLIST_ORDER.ORDERLIST_BOTTOM:
                    default:
                        //! bottom item
                        tKey = m_SortedList.Keys[m_SortedList.Keys.Count - 1]+1;
                        //flpOrderList.Controls.Add(tItem.TargetPanel);
                        break;
                }
                m_SortedList.Add(tKey, tItem);
            }

            tItem.RemoveRequest += new OrderChangeReport(tItem_RemoveRequest);
            tItem.SendToBottomRequest += new OrderChangeReport(tItem_SendToBottomRequest);
            tItem.MoveUpRequest += new OrderChangeReport(tItem_MoveUpRequest);
            tItem.MoveDownRequest += new OrderChangeReport(tItem_MoveDownRequest);
            tItem.BringToTopRequest += new OrderChangeReport(tItem_BringToTopRequest);

            RefreshOrderList();

            return true;
        }

        private void tItem_BringToTopRequest(OrderListItem tItem)
        {
            if (null == tItem)
            {
                return;
            }
            Int32 tIndex = m_SortedList.Values.IndexOf(tItem);
            if (0 == tIndex)
            {
                //! already the top most
                return;
            }
            Int32 tTempKey = m_SortedList.Keys[0] - 1;

            m_SortedList.Remove(m_SortedList.Keys[tIndex]);     //!< remove target
            m_SortedList.Add(tTempKey, tItem);                  //!< add it to the top

            RefreshOrderList();
        }

        private void tItem_MoveDownRequest(OrderListItem tItem)
        {
            if (null == tItem)
            {
                return;
            }
            Int32 tIndex = m_SortedList.Values.IndexOf(tItem);
            if (tIndex == (m_SortedList.Keys.Count - 1))
            {
                //! already at the bottom
                return;
            }

            Int32 tKey = m_SortedList.Keys[tIndex];
            Int32 tTempKey = m_SortedList.Keys[tIndex + 1];
            OrderListItem tTempItemA = m_SortedList.Values[tIndex + 1];

            m_SortedList.Remove(tTempKey);          //!< remove lower item
            m_SortedList.Remove(tKey);              //!< remove target

            m_SortedList.Add(tKey, tTempItemA);
            m_SortedList.Add(tTempKey, tItem);

            RefreshOrderList();
        }

        private void tItem_MoveUpRequest(OrderListItem tItem)
        {
            if (null == tItem)
            {
                return;
            }
            Int32 tIndex = m_SortedList.Values.IndexOf(tItem);
            if (0 == tIndex)
            {
                //! already the top most
                return;
            }

            Int32 tKey = m_SortedList.Keys[tIndex];
            Int32 tTempKey = m_SortedList.Keys[tIndex - 1];
            OrderListItem tTempItemA = m_SortedList.Values[tIndex - 1];

            m_SortedList.Remove(tTempKey);          //!< remove lower item
            m_SortedList.Remove(tKey);              //!< remove target

            m_SortedList.Add(tKey, tTempItemA);
            m_SortedList.Add(tTempKey, tItem);

            RefreshOrderList();
        }

        private void tItem_SendToBottomRequest(OrderListItem tItem)
        {
            if (null == tItem)
            {
                return;
            }
            Int32 tIndex = m_SortedList.Values.IndexOf(tItem);
            if (tIndex == (m_SortedList.Keys.Count - 1))
            {
                //! already at the bottom
                return;
            }
            Int32 tTempKey = m_SortedList.Keys[(m_SortedList.Keys.Count - 1)] + 1;

            m_SortedList.Remove(m_SortedList.Keys[tIndex]);     //!< remove target
            m_SortedList.Add(tTempKey, tItem);                  //!< add it to the top

            //flpOrderList.Controls.Remove(tItem.TargetPanel);
            //flpOrderList.Controls.Add(tItem.TargetPanel);

            RefreshOrderList();
        }

        private void tItem_RemoveRequest(OrderListItem tItem)
        {
            if (null == tItem)
            {
                return;
            }
            Int32 tIndex = m_SortedList.Values.IndexOf(tItem);

            m_SortedList.Remove(m_SortedList.Keys[tIndex]);     //!< remove target

            tItem.RemoveRequest -= new OrderChangeReport(tItem_RemoveRequest);
            tItem.SendToBottomRequest -= new OrderChangeReport(tItem_SendToBottomRequest);
            tItem.MoveUpRequest -= new OrderChangeReport(tItem_MoveUpRequest);
            tItem.MoveDownRequest -= new OrderChangeReport(tItem_MoveDownRequest);
            tItem.BringToTopRequest -= new OrderChangeReport(tItem_BringToTopRequest);

            RefreshOrderList();
        }

    }

    internal delegate void OrderChangeReport(OrderListItem tItem);

    public class OrderListItem
    {
        private OrderListItemPanel m_Panel = null;

        internal OrderListItem()
        {
            m_Panel = new OrderListItemPanel(this);
            Initialize();
        }

        internal OrderListItem(Control[] tControls)
        {
            m_Panel = new OrderListItemPanel(this);
            if (null != tControls)
            {
                m_Panel.Panel.Controls.AddRange(tControls);
            }

            Initialize();
        }

        private void Initialize()
        {
            //! add code here
        }

        public FlowLayoutPanel Panel
        {
            get
            {
                if (null != m_Panel)
                {
                    return m_Panel.Panel;
                }

                return null;
            }
        }

        internal OrderListItemPanel TargetPanel
        {
            get { return m_Panel; }
        }
        
        #region Order-changing events
        
        internal event OrderChangeReport SendToBottomRequest;

        public void SendToBottom()
        {
            try
            {
                if (null != SendToBottomRequest)
                {
                    SendToBottomRequest(this);
                }
            }
            catch(Exception Err)
            {
                Err.ToString();
            }
        }

        internal event OrderChangeReport BringToTopRequest;

        public void BringToTop()
        {
            try
            {
                if (null != BringToTopRequest)
                {
                    BringToTopRequest(this);
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        internal event OrderChangeReport MoveUpRequest;

        public void MoveUp()
        {
            try
            {
                if (null != MoveUpRequest)
                {
                    MoveUpRequest(this);
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        internal event OrderChangeReport MoveDownRequest;

        public void MoveDown()
        {
            try
            {
                if (null != MoveDownRequest)
                {
                    MoveDownRequest(this);
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        internal event OrderChangeReport RemoveRequest;

        public void Remove()
        {
            try
            {
                if (null != RemoveRequest)
                {
                    RemoveRequest(this);
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        #endregion

    }
}
