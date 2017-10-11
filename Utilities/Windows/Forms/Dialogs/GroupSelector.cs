using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ESnail.Utilities.Windows.Forms.Dialogs
{
    public delegate void ListItemSelected(Int32 nIndex,Object[] tArgs);


    public partial class frmGroupSelector : Form
    {
        private Object[] m_Args = null;

        //! \brief default constructor
        public frmGroupSelector()
        {
            InitializeComponent();

            lstGroups.Enabled = false;
        }

        

        //! \brief constructor with grouplist
        public frmGroupSelector(String[] strsGroupList)
        {
            InitializeComponent();

            if (null == strsGroupList)
            {
                lstGroups.Enabled = false;
            }
            else
            {
                lstGroups.Items.AddRange(strsGroupList);
                lstGroups.SelectedIndex = 0;
            }
        }

        //! \brief constructor with grouplist
        public frmGroupSelector(String strTitle,String[] strsGroupList)
        {
            InitializeComponent();

            if (null == strsGroupList)
            {
                lstGroups.Enabled = false;
            }
            else
            {
                lstGroups.Items.AddRange(strsGroupList);
                lstGroups.SelectedIndex = 0;
            }

            if (null != strTitle)
            {
                this.Text = strTitle;
            }
        }

        //! \brief constructor with grouplist
        public frmGroupSelector(String[] strsGroupList, params Object[] tArgs)
        {
            InitializeComponent();

            m_Args = tArgs;

            if (null == strsGroupList)
            {
                lstGroups.Enabled = false;
            }
            else
            {
                lstGroups.Items.AddRange(strsGroupList);
                lstGroups.SelectedIndex = 0;
            }
        }

        //! \brief constructor with grouplist
        public frmGroupSelector(String strTitle, String[] strsGroupList, params Object[] tArgs)
        {
            InitializeComponent();

            m_Args = tArgs;

            if (null == strsGroupList)
            {
                lstGroups.Enabled = false;
            }
            else
            {
                lstGroups.Items.AddRange(strsGroupList);
                lstGroups.SelectedIndex = 0;
            }

            if (null != strTitle)
            {
                this.Text = strTitle;
            }
        }

        public event ListItemSelected ListItemSelectedEvent;

        //! private method for raising ListItemSelectedEvent
        private void OnListItemSelected(System.Int32 nIndex)
        {
            if (null != ListItemSelectedEvent)
            {
                //! raising event
                ListItemSelectedEvent.Invoke(nIndex, m_Args);
            }
        }

        private void cmdSelect_Click(object sender, EventArgs e)
        {
            OnListItemSelected(lstGroups.SelectedIndex);
            this.Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            OnListItemSelected(-1);
            this.Dispose();
        }

        private void frmGroupSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnListItemSelected(-1);
            this.Dispose();
        }

        private void lstGroups_DoubleClick(object sender, EventArgs e)
        {
            OnListItemSelected(lstGroups.SelectedIndex);
            this.Dispose();
        }
    }
}
