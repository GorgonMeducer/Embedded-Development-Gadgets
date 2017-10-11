using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities;
using ESnail.Utilities.Windows.Forms.Interfaces;
using System.Windows.Forms;
using ESnail.Utilities.Log;
using ESnail.Utilities.Windows;
using System.ComponentModel;
using ESnail.Device.Adapters;

namespace ESnail.Device
{
    public partial class frmAdapterManagerEditor : Form
    {
        public frmAdapterManagerEditor()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();

            panelToolList.Disposed += new EventHandler(panelToolList_Disposed);
            tabToolList.Disposed += new EventHandler(tabToolList_Disposed);

            RefreshAdapters();
        }

        private void tabToolList_Disposed(object sender, EventArgs e)
        {
            tabToolList.Disposed -= new EventHandler(tabToolList_Disposed);
            panelToolList.Disposed -= new EventHandler(panelToolList_Disposed);
            this.Dispose();
        }

        private void panelToolList_Disposed(object sender, EventArgs e)
        {
            tabToolList.Disposed -= new EventHandler(tabToolList_Disposed);
            panelToolList.Disposed -= new EventHandler(panelToolList_Disposed);
            this.Dispose();
        }

        private void cmdRefreshAdapters_Click(object sender, EventArgs e)
        {
            RefreshAdapters();
        }

        //! \brief method for refreshing all adapters
        private void RefreshAdapters()
        {
            //! clear list view
            lvAdapters.Items.Clear();

            AdapterManager.AdapterType[] AdapterTypes = AdapterManager.AdapterTypes;
            if (null == AdapterTypes)
            {
                return;
            }

            foreach (AdapterManager.AdapterType tAdapter in AdapterTypes)
            {
                if (!(tAdapter.IsTypeOf(typeof(SingleDeviceAdapter))))
                {
                    continue;
                }
                if (!(tAdapter.IsTypeOf(typeof(ITelegraph))))
                {
                    continue;
                }

                ListViewItem newItem = new ListViewItem(tAdapter.ID);
                newItem.SubItems.Add(tAdapter.Name);
                newItem.SubItems.Add(tAdapter.Type);
                newItem.SubItems.Add(tAdapter.Version);
                newItem.Tag = tAdapter;
                //! add this item to listview
                lvAdapters.Items.Add(newItem);
            }

        }


        public delegate void ToolItemReport(AdapterManager.AdapterType tType);

        public event ToolItemReport ToolItemSelectedtEvent;
        public event ToolItemReport ToolItemActivatedEvent;

        private void OnToolItemSelected(AdapterManager.AdapterType tType)
        {
            if (null != ToolItemSelectedtEvent)
            {
                try
                {
                    ToolItemSelectedtEvent.Invoke(tType);
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }

        private void OnToolItemActivated(AdapterManager.AdapterType tType)
        {
            if (null != ToolItemActivatedEvent)
            {
                try
                {
                    ToolItemActivatedEvent.Invoke(tType);
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }

        private void lvAdapters_Click(object sender, EventArgs e)
        {
            if (0 == lvAdapters.SelectedItems.Count)
            {
                return;
            }

            AdapterManager.AdapterType tAdapterType = lvAdapters.SelectedItems[0].Tag as AdapterManager.AdapterType;
            if (null == tAdapterType)
            {
                return;
            }
            else if (!tAdapterType.Available)
            {
                return;
            }

            //! \brief raising event
            OnToolItemSelected(tAdapterType);
        }

        private void lvAdapters_ItemActivate(object sender, EventArgs e)
        {
            if (0 == lvAdapters.SelectedItems.Count)
            {
                return;
            }

            AdapterManager.AdapterType tAdapterType = lvAdapters.SelectedItems[0].Tag as AdapterManager.AdapterType;
            if (null == tAdapterType)
            {
                return;
            }
            else if (!tAdapterType.Available)
            {
                return;
            }

            //! \brief raising event
            OnToolItemActivated(tAdapterType);
        }

        private void cmdAddAdapter_Click(object sender, EventArgs e)
        {
            cmdAddAdapter.Enabled = false;
            dlgOpenAdapterDll.InitialDirectory = Application.StartupPath;

            if (DialogResult.OK == dlgOpenAdapterDll.ShowDialog())
            {

                if (AdapterManager.TryToAdd(dlgOpenAdapterDll.FileName))
                {
                    RefreshAdapters();
                }

            }

            cmdAddAdapter.Enabled = true;
        }

        private void cmdRemoveAdapter_Click(object sender, EventArgs e)
        {
            if (0 == lvAdapters.SelectedItems.Count)
            {
                return;
            }

            AdapterManager.AdapterType tAdapterType = lvAdapters.SelectedItems[0].Tag as AdapterManager.AdapterType;
            if (null == tAdapterType)
            {
                return;
            }

            if (AdapterManager.UnregisterAdapterDatatype(tAdapterType))
            {
                RefreshAdapters();
            }
        }
    }
}
