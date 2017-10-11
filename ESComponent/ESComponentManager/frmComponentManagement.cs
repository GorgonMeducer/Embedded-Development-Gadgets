using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities.Reflection;
using ESnail.Utilities;

namespace ESnail.Component
{

    public partial class frmComponentManagement : ComponentManagement<BMComponentManager>
    {
        public frmComponentManagement()
        { }

        public frmComponentManagement(BMComponentManager tManager)
            : base(tManager)
        { }

        protected override ListViewItem[] __RefreshList()
        {
            List<ListViewItem> m_List = new List<ListViewItem>();
            IBMComponent[] tComponents = m_Manager.Components;
            if (null == tComponents)
            {
                return null;
            }

            foreach (IBMComponent tComponent in tComponents)
            {
                ListViewItem tItem = new ListViewItem(tComponent.ID);

                tItem.SubItems.Add(tComponent.ComponentName);
                tItem.SubItems.Add(tComponent.Version);
                tItem.SubItems.Add(tComponent.Company);
                tItem.SubItems.Add(tComponent.ComponentDescription);
                tItem.Tag = tComponent;

                m_List.Add(tItem);
            }

            return m_List.ToArray();
        }

        protected override ISafeID ObjectValidation(Object tObj)
        {
            return tObj as IBMComponent;
        }

        internal Panel ComponentsPanel
        {
            get { return this.panelComponents; }
        }

        internal TabPage ComponentsTabPage
        {
            get { return this.tabComponents; }
        }

    }
    




      
}
