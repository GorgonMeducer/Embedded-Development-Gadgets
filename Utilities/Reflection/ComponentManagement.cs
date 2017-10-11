using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities.Reflection;
using ESnail.Utilities;

namespace ESnail.Utilities.Reflection
{

    public abstract partial class ComponentManagement<TType> : Form
        where TType : class, IComponentManager
    {
        protected TType m_Manager = null;
        private Boolean m_Refreshing = false;

        public ComponentManagement(TType tManager)
        {
            m_Manager = tManager;
            Initialize();
        }

        public ComponentManagement()
        {
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();

            if (null == m_Manager)
            {
                panelEditor.Enabled = false;
            }


            Refresh();
        }

        protected abstract ListViewItem[] __RefreshList();

        public override void Refresh()
        {
            do
            {
                if (null == m_Manager)
                {
                    break;
                }
                else if (m_Refreshing)
                {
                    break;
                }
                m_Refreshing = true;

                try
                {
                    do
                    {
                        //! refresh components
                        lvComponents.BeginUpdate();
                        do
                        {
                            lvComponents.Items.Clear();
                            try
                            {
                                ListViewItem[] tItems = __RefreshList();
                                if (null != tItems)
                                {
                                    lvComponents.Items.AddRange(tItems);
                                }
                                
                            }
                            catch (Exception) { }
                        }
                        while (false);
                        lvComponents.EndUpdate();
                    }
                    while (false);
                }
                catch (Exception) { }

                m_Refreshing = false;
            }
            while (false);

            base.Refresh();
        }

        

        private void ShowErrorMessage(Control tControl, String tMessage)
        {
            toolTipError.Active = false;
            toolTipError.ToolTipIcon = ToolTipIcon.Warning;
            toolTipError.Show("", tControl);
            toolTipError.Active = true;
            toolTipError.Show(tMessage, tControl, 5000);
        }

        private void cmdRefreshComponent_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void cmdAddComponent_Click(object sender, EventArgs e)
        {
            if (null == m_Manager)
            {
                return;
            }

            if (DialogResult.OK == dlgOpenAdapterDll.ShowDialog())
            {
                if (!m_Manager.Add(dlgOpenAdapterDll.FileName))
                {
                    ShowErrorMessage(lvComponents, "Failed to import component from library " + dlgOpenAdapterDll.SafeFileName);
                }
            }

            Refresh();
        }

        protected abstract ISafeID ObjectValidation(Object tObj);

        private void cmdRemoveComponent_Click(object sender, EventArgs e)
        {
            if (null == m_Manager)
            {
                return;
            }

            if (null == lvComponents.SelectedItems)
            {
                return;
            }
            else if (0 == lvComponents.SelectedItems.Count)
            {
                return;
            }

            ISafeID tObj = ObjectValidation(lvComponents.SelectedItems[0].Tag);
            if (null != tObj)
            {
                return;
            }

            m_Manager.Remove(tObj.ID);

            Refresh();
        }
    }



      
}
