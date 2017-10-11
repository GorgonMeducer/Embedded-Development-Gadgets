using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using ESnail.Utilities;
using ESnail.Utilities.IO;
using ESnail.Utilities.Windows.Forms.Interfaces;
using System.Xml;
using System.Windows.Forms;

namespace ESnail.Component
{
    partial class BMComponentManager
    {
        //! \brief get all components
        public IBMComponentDesign[] Components
        {
            get
            {
                List<IBMComponentDesign> tResultList = new List<IBMComponentDesign>();

                foreach (BMComponentItem tItem in m_ComponentSet)
                {
                    if (null == tItem)
                    {
                        continue;
                    }
                    if (!tItem.Available)
                    {
                        continue;
                    }
                    if (null == tItem.Component)
                    {
                        continue;
                    }
                    tResultList.Add(tItem.Component);
                }

                return tResultList.ToArray();
            }
        }

        //! \brief get parent of this component manager
        public Object Parent
        {
            get { return m_Parent; }
        }



        #region editors
        public TabPage EditorPage
        {
            get 
            {
                frmComponentManagement tEditor = new frmComponentManagement(this);
                return tEditor.ComponentsTabPage;
            }
        }

        public Panel EditorPanel
        {
            get
            {
                frmComponentManagement tEditor = new frmComponentManagement(this);
                return tEditor.ComponentsPanel;
            }
        }

        private frmComponentManagement m_Editor = null;

        public Form Editor
        {
            get 
            { 
                if (null == m_Editor) 
                {
                    m_Editor = new frmComponentManagement(this);
                    m_Editor.Disposed += new EventHandler(m_Editor_Disposed);
                }

                return m_Editor;
            }
        }

        private void m_Editor_Disposed(object sender, EventArgs e)
        {
            Form tEditor = sender as Form;
            if (null != tEditor)
            {
                tEditor.Disposed -= new EventHandler(m_Editor_Disposed);
            }
            m_Editor = null;
        }

        public Form CreateEditor()
        {
            return new frmComponentManagement(this);
        }
        #endregion


    }
}