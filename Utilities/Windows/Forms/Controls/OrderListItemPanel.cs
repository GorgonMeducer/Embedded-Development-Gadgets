using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows.Forms.Controls
{
    internal partial class OrderListItemPanel : UserControl
    {
        private OrderListItem m_Tag = null;

        public OrderListItemPanel()
        {
            Initialize();
        }

        public OrderListItemPanel(OrderListItem tParent)
        {
            m_Tag = tParent;
            this.ResizeRedraw = true;
            Initialize();
        }


        private void Initialize()
        {
            InitializeComponent();

            Refresh();
        }

        public OrderListItem Target
        {
            get { return m_Tag; }
        }

        #region buttons' events
        private void cmdUp_Click(object sender, EventArgs e)
        {
            if (null == m_Tag)
            {
                return;
            }

            m_Tag.MoveUp();
        }

        private void cmdDown_Click(object sender, EventArgs e)
        {
            if (null == m_Tag)
            {
                return;
            }

            m_Tag.MoveDown();
        }

        private void cmdTop_Click(object sender, EventArgs e)
        {
            if (null == m_Tag)
            {
                return;
            }

            m_Tag.BringToTop();
        }

        private void cmdBottom_Click(object sender, EventArgs e)
        {
            if (null == m_Tag)
            {
                return;
            }

            m_Tag.SendToBottom();
        }

        private void cmdRemove_Click(object sender, EventArgs e)
        {
            if (null == m_Tag)
            {
                return;
            }

            m_Tag.Remove();
        }

        #endregion

        #region buttons' properties
        [Description("flowlayoutpanel for holding controls")]
        [Category("Data")]
        public FlowLayoutPanel Panel
        {
            get { return flpPanel; }
        }

        [Description("Show Up button")]
        [DefaultValue(true)]
        [Category("Buttons")]
        public Boolean UpButton
        {
            get { return cmdUp.Visible; }
            set 
            {
                cmdUp.Visible = value;
                this.Refresh();
            }
        }

        [Description("Show Down button")]
        [DefaultValue(true)]
        [Category("Buttons")]
        public Boolean DownButton
        {
            get { return cmdDown.Visible; }
            set { cmdDown.Visible = value; }
        }

        [Description("Show Top button")]
        [DefaultValue(true)]
        [Category("Buttons")]
        public Boolean TopButton
        {
            get { return cmdTop.Visible; }
            set { cmdTop.Visible = value; }
        }

        [Description("Show Bottom button")]
        [DefaultValue(true)]
        [Category("Buttons")]
        public Boolean BottomButton
        {
            get { return cmdBottom.Visible; }
            set { cmdBottom.Visible = value; }
        }

        [Description("Show Remove button")]
        [DefaultValue(true)]
        [Category("Buttons")]
        public Boolean RemoveButton
        {
            get { return cmdRemove.Visible; }
            set { cmdRemove.Visible = value; }
        }
        #endregion

    }
}
