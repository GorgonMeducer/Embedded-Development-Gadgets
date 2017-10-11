using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using ESnail.Utilities;
using ESnail.Utilities.XML;
using ESnail.Utilities.Generic;
using ESnail.Utilities.Windows.Forms.Interfaces;
using ESnail.Utilities.Reflection;
using ESnail.Utilities.IO;
using System.Windows.Forms;

namespace ESnail.Component
{

    

    public class BMComponentItem : ComponentLoader<IBMComponentDesign>, ISafeID
    {
        internal BMComponentItem(String tPath)
            : base(tPath, "ESnail.Component.ComponentLoader", "Create", new Object[] { null })
        {
        }

        public SafeID ID
        {
            get
            {
                if (null == m_Component)
                {
                    return null;
                }

                return m_Component.ID;
            }
            set { }
        }

        public override String ToString()
        {
            return ID;
        }
    }




    public partial class BMComponentManager : ComponentManager<BMComponentItem>, IEditorEx
    {

        private Object m_Parent = null;
        
        //! \brief default constructor
        public BMComponentManager()
        { 
            
        }

        //! \brief constructor with parent object
        public BMComponentManager(Object tParent)
        {
            m_Parent = tParent;
        }

        //! \brief find component
        public IBMComponentDesign FindComponent(SafeID tID)
        {
            BMComponentItem tItem = m_ComponentSet.Find(tID);
            if (null == tItem)
            {
                return null;
            }
            else if (!tItem.Available)
            {
                return null;
            }
            return tItem.Component;
        }


        protected override BMComponentItem CreateItem(string tPath)
        {
            return new BMComponentItem(tPath);
        }
    }
}
