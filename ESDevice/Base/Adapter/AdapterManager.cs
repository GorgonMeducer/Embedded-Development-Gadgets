using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities;
using ESnail.Utilities.Windows.Forms.Interfaces;
using System.Windows.Forms;
using ESnail.Utilities.Log;
using ESnail.Utilities.Windows;
using System.Xml;
using ESnail.Utilities.XML;
using ESnail.Utilities.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ESnail.Device
{
    public static partial class AdapterManager
    {
        private static List<Adapter> m_UnRegiesterAdapterList = new List<Adapter>();

        public class AdapterType : ISafeID
        {
            private SafeID m_ID = null;
            private Boolean m_Available = false;
            private Adapter m_Adapter = null;
            private String m_Assembly = null;
            

            public AdapterType(Adapter tAdapter)
            {
                if (null == tAdapter)
                {
                    return;
                }

                m_ID = GetTypeID(tAdapter);

                m_Adapter = tAdapter.CreateAdapter(m_ID);
                m_Adapter.Name = tAdapter.Name;

                

                if (null == m_Adapter)
                {
                    return;
                }

                m_Available = true;
            }

            public AdapterType(Adapter tAdapter, String tAssembly)
            {
                if (null == tAdapter)
                {
                    return;
                }

                m_ID = GetTypeID(tAdapter);

                m_Adapter = tAdapter.CreateAdapter(m_ID);
                m_Adapter.Name = tAdapter.Name;

                if (null == m_Adapter)
                {
                    return;
                }

                m_Assembly = System.IO.Path.GetFullPath(tAssembly);

                m_Available = true;
            }

            static public SafeID GetTypeID(Adapter tAdapter)
            {
                String tID = null;
                if (null == tAdapter)
                {
                    return "Unknown Adapter";
                }

                if (null == tAdapter.Type)
                {
                    tID = "Unknown Adapter";
                }
                else
                {
                    tID = tAdapter.Type;
                }
                if (null != tAdapter.Version)
                {
                    if ("" != tAdapter.Version.Trim())
                    {
                        tID += " Ver:" + tAdapter.Version;
                    }
                }
                return tID;
            }

            //! \brief ID
            public SafeID ID
            {
                get
                {
                    return m_ID;
                }
                set { }
            }

            //! \brief object available state
            public Boolean Available
            {
                get { return m_Available; }
            }

            //! \brief adatper type
            public String Type
            {
                get 
                {
                    if (null != m_Adapter)
                    {
                        return m_Adapter.Type;
                    }
                    return null;
                }
            }

            //! \brief adatper version
            public String Version
            {
                get
                {
                    if (null != m_Adapter)
                    {
                        return m_Adapter.Version;
                    }
                    return null;
                }
            }

            public String Name
            {
                get
                {
                    if (null != m_Adapter)
                    {
                        return m_Adapter.Name;
                    }
                    return null;
                }
            }

            public Boolean IsTypeOf(Type tType)
            {
                if ((null == m_Adapter) || (null == tType))
                {
                    return false;
                }

                return tType.IsInstanceOfType(m_Adapter);
            }

            //! \brief create a adapter
            public Adapter CreateAdapter(SafeID tID)
            {
                if (null == m_Adapter)
                {
                    return null;
                }

                Adapter tAdapter = m_Adapter.CreateAdapter(tID);

                if ((null != tAdapter) && (null != Adapter.m_WindowsMessageHandler))
                {
                    tAdapter.RegisterDeviceMessageHandler(ref Adapter.m_WindowsMessageHandler, m_Handle);
                }
                else
                {
                    m_UnRegiesterAdapterList.Add(tAdapter);
                }

                return tAdapter;
            }

            internal Adapter Adapter
            {
                get { return m_Adapter; }
            }

            internal String Assembly
            {
                get { return m_Assembly; }
            }
        }
        

        internal class AdapterList : ISafeID
        {
            private AdapterType[] m_List = null;
            private SafeID m_ID = null;
            public AdapterList(AdapterType[] tList)
            {
                m_List = tList;
            }

            public AdapterType[] AdapterTypes
            {
                get { return m_List; }
            }

            public SafeID ID
            {
                get { return m_ID; }
                set {}
            }
        }

        private class XMLAdapterSettingIO : TXMLSettingIO<AdapterList>
        {

            public override String XMLRootName
            {
                get { return "General"; }
            }

            public override String XMLObjectName
            {
                get { return "AdapterManagment"; }
            }

            public override Boolean Append(XmlDocument xmlDocument, XmlNode xmlRootNode, AdapterList BMObject)
            {
                //! check parameter
                if ((null == xmlDocument) || (null == xmlRootNode) || (null == BMObject))
                {
                    return false;
                }

                if (null == BMObject.AdapterTypes)
                {
                    return false;
                }

                try
                {

                    //! get root node               
                    XmlNode AdapterManagmentNode = null;

                    if (xmlRootNode.Name != XMLRootName)
                    {
                        AdapterManagmentNode = xmlRootNode.SelectSingleNode(XMLRootName);
                    }
                    else
                    {
                        AdapterManagmentNode = xmlRootNode;
                    }

                    if (null == AdapterManagmentNode)
                    {
                        //! no parameter group set, so create a function set node
                        AdapterManagmentNode = xmlDocument.CreateNode(XmlNodeType.Element, XMLRootName, null);
                        xmlRootNode.AppendChild(AdapterManagmentNode);
                    }
                    else
                    {
                        //! try to find parameter group set
                        foreach (XmlNode enumNode in AdapterManagmentNode.ChildNodes)
                        {
                            //! find a specified parameter group set
                            if (
                                    (enumNode.Name == XMLObjectName)
                                //&& (enumNode.Attributes.GetNamedItem("ID").Value == BMObject.ID)
                               )
                            {
                                //! we find the parameter group set, remove this child
                                xmlRootNode.RemoveChild(enumNode);
                                break;
                            }
                        }
                    }

                    //! create a new parameter group set
                    XmlElement newAdapterManagmentSettings = xmlDocument.CreateElement(XMLObjectName);

                    /*
                    //! ID
                    newAdapterManagmentSettings.SetAttribute("ID", BMObject.ID);
                     */

                    //! adapter count                    
                    newAdapterManagmentSettings.SetAttribute("AdapterCount", BMObject.AdapterTypes.Length.ToString());
                    

                    //! save each item
                    foreach (AdapterType tItem in BMObject.AdapterTypes)
                    {
                        XmlElement xmlAdapterType = xmlDocument.CreateElement("AdapterType");
                        xmlAdapterType.SetAttribute("ID", tItem.ID);

                        //! dll
                        do
                        {
                            XmlElement xmlDll = xmlDocument.CreateElement("Assembly");
                            xmlDll.InnerText = ESnail.Utilities.IO.PathEx.RelativePath(Application.StartupPath, tItem.Assembly);
                            xmlAdapterType.AppendChild(xmlDll);
                        }
                        while (false);

                        //! adapter type
                        do
                        {
                            XmlElement xmlType = xmlDocument.CreateElement("Type");
                            xmlType.InnerText = tItem.Type;
                            xmlAdapterType.AppendChild(xmlType);
                        }
                        while (false);

                        //! adapter version
                        do
                        {
                            XmlElement xmlVersion = xmlDocument.CreateElement("Version");
                            xmlVersion.InnerText = tItem.Version;
                            xmlAdapterType.AppendChild(xmlVersion);
                        }
                        while (false);

                        //! adapter name
                        do
                        {
                            XmlElement xmlName = xmlDocument.CreateElement("Name");
                            xmlName.InnerText = tItem.Name;
                            xmlAdapterType.AppendChild(xmlName);
                        }
                        while (false);

                        

                        newAdapterManagmentSettings.AppendChild(xmlAdapterType);
                    }
                    //! add settings to xml root
                    AdapterManagmentNode.AppendChild(newAdapterManagmentSettings);

                    
                }
                catch (Exception Err)
                {
                    m_Exception = Err;
                    return false;
                }
                return true;
            }

            protected override void _Dispose()
            {
                //! doing nothing at all
            }

            //! \brief XML I/O type
            public override String Type
            {
                get { return "Adapters managment Input/Output Adapter for XML"; }
            }

            public override Boolean Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref AdapterList BMObject, int nIndex)
            {
                if ((null == xmlDocument) || (null == xmlRootNode) )
                {
                    return false;
                }

                try
                {
                    //! get root node

                    XmlNode AdapterTypesNode = null;
                    //! find groups from root node
                    if (xmlRootNode.Name != XMLRootName)
                    {
                        AdapterTypesNode = xmlRootNode.SelectSingleNode(XMLRootName);
                    }
                    else
                    {
                        AdapterTypesNode = xmlRootNode;
                    }

                    if (null == AdapterTypesNode)
                    {
                        if (0 == xmlRootNode.ChildNodes.Count)
                        {
                            return false;
                        }

                        //! search for node "Groups" in each childnode
                        foreach (XmlNode xmlChildren in xmlRootNode.ChildNodes)
                        {
                            if (Import(xmlDocument, xmlChildren, ref BMObject, nIndex))
                            {
                                return true;
                            }
                        }

                        return false;
                    }

                    //! get parameter group set with index number
                    XmlNodeList xmlParameterAgentList = AdapterTypesNode.SelectNodes(XMLObjectName);
                    if (null == xmlParameterAgentList)
                    {
                        return false;
                    }

                    if (xmlParameterAgentList.Count <= nIndex)
                    {
                        return false;
                    }

                    //! get specified setting entrance
                    XmlNodeList xmlAdapterTypes = xmlParameterAgentList[nIndex].SelectNodes("AdapterType");
                    if (null == xmlAdapterTypes)
                    {
                        //! nothing to import 
                        return true;
                    }

                    //! import each node
                    foreach (XmlNode tNode in xmlAdapterTypes)
                    {
                        
                        //! get assmebly string
                        if (null == tNode.SelectSingleNode("Assembly"))
                        {
                            continue;
                        }
                        String tAssembly = tNode.SelectSingleNode("Assembly").InnerText;
                        if (null == tAssembly)
                        {
                            continue;
                        }
                        if ("" == tAssembly.Trim())
                        {
                            continue;
                        }


                        //! get type
                        if (null == tNode.SelectSingleNode("Type"))
                        {
                            continue;
                        }
                        String tAdapterType = tNode.SelectSingleNode("Type").InnerText;
                        if (null == tAdapterType)
                        {
                            continue;
                        }
                        if ("" == tAdapterType.Trim())
                        {
                            continue;
                        }

                        //! read version
                        String tVersion = tNode.SelectSingleNode("Version").InnerText;

                        try
                        {
                            //! load assmebly
                            Assembly ass = Assembly.LoadFrom(System.IO.Path.Combine(Application.StartupPath, tAssembly));
                            if (null == ass)
                            {
                                continue;
                            }

                            //! load class
                            Object newAdapter = ass.CreateInstance("ESnail.Device.Adapters.AdapterLoader");
                            if (null == newAdapter)
                            {
                                continue;
                            }

                            //! load method
                            MethodInfo tMethod = ass.GetType("ESnail.Device.Adapters.AdapterLoader").GetMethod("Create");
                            if (tMethod == null)
                            {
                                continue;
                            }

                            //! load adapter 
                            Adapter tAdapter = tMethod.Invoke(newAdapter, new Object[]{null,null}) as Adapter;
                            if (null == tAdapter)
                            {
                                continue;
                            }

                            if (null == tAdapter.Type)
                            {
                                continue;
                            }
                            else if ("" == tAdapter.Type.Trim())
                            {
                                continue;
                            }
                            else if (tAdapter.Type.Trim().ToUpper() != tAdapterType.Trim().ToUpper())
                            {
                                continue;
                            }
                            if (tAdapter.Version != tVersion)
                            {
                                continue;
                            }

                            //! register adapter
                            AdapterManager.RegisterAdapterDatatype(tAdapter,tAssembly);
                        }
                        catch (Exception Err)
                        {
                            Err.ToString();
                            continue;
                        }
                        
                    }

                    BMObject = new AdapterList(AdapterManager.AdapterTypes);
                }
                catch (Exception Err)
                {
                    m_Exception = Err;
                    return false;
                }
                return true;
            }
        }
    
    }



    public static partial class AdapterManager 
    {
        private static TSet<AdapterType> s_AdapterSet = new TSet<AdapterType>();
        private static IntPtr m_Handle = IntPtr.Zero;

        public static void RegisterWindowmMessageHandler(WindowsMessageHandler tHandler, IntPtr tHandle)
        {
            if (null == tHandler)
            {
                return;
            }
            if (null == Adapter.m_WindowsMessageHandler)
            {
                Adapter.m_WindowsMessageHandler = tHandler;
            }

            if (IntPtr.Zero != tHandle)
            {
                m_Handle = tHandle;
            }
            
            foreach (Adapter tItem in m_UnRegiesterAdapterList)
            {
                tItem.RegisterDeviceMessageHandler(ref Adapter.m_WindowsMessageHandler, tHandle);
            }

            m_UnRegiesterAdapterList.Clear();
        }

        //! \brief static method for registering a new adatper datatype
        public static Boolean RegisterAdapterDatatype(Adapter tAdapter)
        {
            if (null == tAdapter)
            {
                return false;
            }

            

            AdapterType tNewType = new AdapterType(tAdapter);
            if (null == tNewType)
            {
                return false;
            }
            else if (!tNewType.Available)
            {
                return false;
            }

            s_AdapterSet.Add(tNewType);

            tAdapter.RegisterDeviceMessageHandler(ref Adapter.m_WindowsMessageHandler, m_Handle);

            return true;
        }

        //! \brief static method for registering a new adatper datatype
        public static Boolean RegisterAdapterDatatype(Adapter tAdapter, String tAssembly)
        {
            if (null == tAdapter)
            {
                return false;
            }

            AdapterType tNewType = new AdapterType(tAdapter,tAssembly);
            if (null == tNewType)
            {
                return false;
            }
            else if (!tNewType.Available)
            {
                return false;
            }

            s_AdapterSet.Add(tNewType);
            tAdapter.RegisterDeviceMessageHandler(ref Adapter.m_WindowsMessageHandler, m_Handle);
            return true;
        }

        //! \brief static method for unregistering a adapter datatype
        public static Boolean UnregisterAdapterDatatype(Adapter tAdapter)
        {
            if (null == tAdapter)
            {
                return false;
            }

            String tIDStr = null;
            if (null == tAdapter.Type)
            {
                tIDStr = "Unknown Adapter";
            }
            else
            {
                tIDStr = tAdapter.Type;
            }
            if (null != tAdapter.Version)
            {
                if ("" != tAdapter.Version.Trim())
                {
                    tIDStr += " Ver:" + tAdapter.Version;
                }
            }

            if (null != s_AdapterSet.Find(tIDStr))
            {
                s_AdapterSet.Remove(tIDStr);
            }


            
            return true;
        }

        //! \brief static method for unregistering a adapter datatype
        public static Boolean UnregisterAdapterDatatype(AdapterType tAdapterType)
        {
            if (null == tAdapterType)
            {
                return false;
            }

            return s_AdapterSet.Remove(tAdapterType.ID);           
        }

        public static AdapterType[] AdapterTypes
        {
            get 
            {
                return s_AdapterSet.Items;
            }
        }

        //! \brief create a specified adapter
        public static Adapter CreateAdapter(SafeID tType, SafeID tID)
        { 
            AdapterType tAdapterType = s_AdapterSet.Find(tType);
            if (null == tAdapterType)
            {
                return null;
            }

            return tAdapterType.CreateAdapter(tID);
        }

        //! \brief adapter type count
        public static Int32 Count
        {
            get { return s_AdapterSet.Count; }
        }

        //! \brief find a adapter with a specified ID
        public static AdapterType Find(SafeID tID)
        {
            return s_AdapterSet.Find(tID);
        }

        public static TabPage CreateToolManagerPage(frmAdapterManagerEditor.ToolItemReport tMethodA, frmAdapterManagerEditor.ToolItemReport tMethodB)
        {
            frmAdapterManagerEditor tEditor = new frmAdapterManagerEditor();

            if (null != tMethodA)
            {
                tEditor.ToolItemActivatedEvent += tMethodA;
            }
            if (null != tMethodB)
            {
                tEditor.ToolItemSelectedtEvent += tMethodB;
            }

            return tEditor.tabToolList;
        }

        public static Panel CreateToolManagePanel(frmAdapterManagerEditor.ToolItemReport tMethodA, frmAdapterManagerEditor.ToolItemReport tMethodB)
        {
            frmAdapterManagerEditor tEditor = new frmAdapterManagerEditor();

            if (null != tMethodA)
            {
                tEditor.ToolItemActivatedEvent += tMethodA;
            }
            if (null != tMethodB)
            {
                tEditor.ToolItemSelectedtEvent += tMethodB;
            }

            return tEditor.panelToolList;
        }

        public static frmAdapterManagerEditor CreateToolManageEditor()
        {
            return new frmAdapterManagerEditor();
        }

        public static Boolean LoadSetting(String tPath)
        {
            if (null == tPath)
            {
                return false;
            }
            else if ("" == tPath.Trim())
            {
                return false;
            }
            AdapterList tList = null;
            using (XMLAdapterSettingIO tReadIO = new XMLAdapterSettingIO())
            {
                if (!tReadIO.Import(tPath, ref tList, 0))
                {
                    return false;
                }
            }

            return true;
        }


        public static Boolean SaveSetting(String tPath)
        {
            if (null == tPath)
            {
                return false;
            }
            else if ("" == tPath.Trim())
            {
                return false;
            }

            using (XMLAdapterSettingIO tReadIO = new XMLAdapterSettingIO())
            {
                if (!tReadIO.Append(tPath, new AdapterList(AdapterManager.AdapterTypes)))
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean LoadSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            AdapterList tList = null;
            using (XMLAdapterSettingIO tReadIO = new XMLAdapterSettingIO())
            {
                if (!tReadIO.Import(xmlDoc,xmlRoot,ref tList,0))
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean SaveSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            using (XMLAdapterSettingIO tReadIO = new XMLAdapterSettingIO())
            {
                if (!tReadIO.Append(xmlDoc,xmlRoot, new AdapterList(AdapterManager.AdapterTypes)))
                {
                    return false;
                }
            }

            return true;
        }

        public static Boolean TryToAdd(String tPath)
        {
            try
            {
                tPath = System.IO.Path.GetFullPath(tPath);
            }
            catch (Exception Err)
            {
                Err.ToString();
                return false;
            }

            if (null == tPath)
            {
                return false;
            }
            else if ("" == tPath.Trim())
            {
                return false;
            }

            if (!System.IO.File.Exists(tPath))
            {
                return false;
            }

            try
            {
                //! load assmebly
                Assembly ass = Assembly.LoadFrom(tPath);
                if (null == ass)
                {
                    return false;
                }

                //! load class
                Object newAdapter = ass.CreateInstance("ESnail.Device.Adapters.AdapterLoader");
                if (null == newAdapter)
                {
                    return false;
                }

                //! load method
                MethodInfo tMethod = ass.GetType("ESnail.Device.Adapters.AdapterLoader").GetMethod("Create");
                if (tMethod == null)
                {
                    return false;
                }

                

                //! load adapter 
                Adapter tAdapter = tMethod.Invoke(newAdapter, new Object[] {null,null}) as Adapter;
                if (null == tAdapter)
                {
                    return false;
                }

                //! register adapter
                AdapterManager.RegisterAdapterDatatype(tAdapter, tPath);
            }
            catch (Exception Err)
            {
                Err.ToString();
                return false;
            }

            return true;
        }

    }
}
