using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Security.Policy;
using ESnail.Utilities.Generic;
using ESnail.Utilities.XML;
using System.Xml;
using ESnail.Utilities.IO;
using System.Windows.Forms;

namespace ESnail.Utilities.Reflection
{
    internal class AssemblyFileLoader : MarshalByRefObject
    {
        private Assembly m_Assembly = null;
        private MarshalByRefObject m_Object = null;
        private Boolean m_Available = false;
        private String m_ClassName = null;

        public Boolean Available
        {
            get { return m_Available; }
        }

        public MarshalByRefObject LoadObject(String tFilePath, String tClassName)
        {
            
            MarshalByRefObject tObject = null;
            if (!File.Exists(tFilePath))
            {
                return null;
            }
            m_Object = null;

            try
            {
                m_Assembly = Assembly.LoadFrom(tFilePath);

                tObject = m_Assembly.CreateInstance(tClassName) as MarshalByRefObject;
            }
            catch (Exception )
            {
                m_Assembly = null;
            }

            if (null != tObject)
            {
                m_Available = true;
            }

            m_ClassName = tClassName;
            m_Object = tObject;

            return tObject;
        }

        public MarshalByRefObject CallMethod(String tMethod, params Object[] Object)
        {
            if (!m_Available)
            {
                return null;
            }

            MethodInfo tMethodInfo = m_Assembly.GetType(m_ClassName).GetMethod(tMethod);
            if (null == tMethodInfo)
            {
                return null ;
            }

            return tMethodInfo.Invoke(m_Object, new Object[] { null }) as MarshalByRefObject;
        }

    }

    public class AssemblyLoader : IDisposable
    {
        private AppDomain m_TempDomain = null;
        private Boolean m_Available = false;
        private AssemblyFileLoader m_Loader = null;

        public AssemblyLoader()
        {
            try
            {
                AppDomainSetup tAPPSetup = new AppDomainSetup();
                tAPPSetup.ApplicationName = "ESUtilities";
                tAPPSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                tAPPSetup.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Private");
                tAPPSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                tAPPSetup.CachePath = tAPPSetup.ApplicationBase;
                tAPPSetup.ShadowCopyFiles = "false";
                tAPPSetup.ShadowCopyDirectories = Path.GetTempPath();

                Evidence baseEvidence = AppDomain.CurrentDomain.Evidence;
                Evidence evidence = new Evidence(baseEvidence);

                m_TempDomain = AppDomain.CreateDomain("ESUtilitiesDomain", evidence, tAPPSetup);

                String tName = Assembly.GetExecutingAssembly().GetName().FullName;
                m_Loader = m_TempDomain.CreateInstanceAndUnwrap(tName, typeof(AssemblyFileLoader).FullName) as AssemblyFileLoader;
                if (null == m_Loader)
                {
                    return;
                }
            }
            catch (Exception )
            {
                return;
            }
            m_Available = true;
        }

        public MarshalByRefObject LoadObject(String tPath, String tClassName)
        {
            if (!m_Available)
            {
                return null;
            }

            return m_Loader.LoadObject(tPath, tClassName);
        }


        public MarshalByRefObject CallMethod(String tMethod, params Object[] tArgs)
        {
            if (!m_Available)
            {
                return null;
            }

            return m_Loader.CallMethod(tMethod, tArgs);
        }

        //! unload
        public void Unload()
        {
            Dispose();
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        #region IDisposable Members

        private Boolean m_Disposed = false;

        ~AssemblyLoader()
        {
            Dispose();
        }
        
        public Boolean Disposed
        {
            get { return m_Disposed; }
        }

        public void Dispose()
        {
            if (!m_Disposed)
            {
                m_Disposed = true;

                try
                {
                    //! unload application domain
                    AppDomain.Unload(m_TempDomain);
                    m_TempDomain = null;
                }
                catch (Exception) { }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }
        }

        #endregion
    }

    public interface IComponentAgent : IAvailable, ISafeID
    {
        Object Target
        {
            get;
        }

        String Path
        {
            get;
        }
    }

    public interface IAssembly
    {
        String Path
        {
            get;
        }

        String Component
        {
            get;
        }

        String Entry
        {
            get;
        }
    }



    public class ComponentLoader<TType> : IComponentAgent
        where TType : class, ISafeID 
    {
        private Boolean m_Available = false;
        private String m_Path = null;
        protected TType m_Component = null;

        public ComponentLoader(String tPath, String tComponentName, String tMethodName, params Object[] tArguments)
        {
            __Loader(tPath, tComponentName, tMethodName, tArguments);
        }

        private void __Loader(String tPath, String tComponentName, String tMethodName, params Object[] tArguments)
        {
            Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);
            if (!File.Exists(tPath))
            {
                return;
            }

            try
            {
                Object tObj = null;
                //! load assembly
                Assembly tAss = Assembly.LoadFrom(tPath);
                if (null == tAss)
                {
                    return;
                }
                //! load instance
                tObj = tAss.CreateInstance(tComponentName);
                if (null == tObj)
                {
                    return;
                }

                MethodInfo tMethod = tAss.GetType(tComponentName).GetMethod(tMethodName);
                if (tMethod == null)
                {
                    return;
                }
                m_Component = tMethod.Invoke(tObj, tArguments) as TType;
                if (null == m_Component)
                {
                    return;
                }

            }
            catch (Exception)
            {
                return;
            }

            m_Path = System.IO.Path.GetFullPath(tPath);

            m_Available = true;
        }

        public ComponentLoader(IAssembly tTarget, params Object[] tArgument)
        {
            if (null == tTarget)
            {
                return;
            }

            __Loader(tTarget.Path, tTarget.Component, tTarget.Entry, tArgument);
        }

        public TType Component
        {
            get { return m_Component; }
        }

        public Object Target
        {
            get { return m_Component; }
        }

        public String Path
        {
            get { return m_Path; }
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        public SafeID ID
        {
            get
            {
                return m_Component.ID;
            }
            set { }
        }
    }

    public interface IComponentManager
    {
        Boolean Add(String tPath);
        Boolean Remove(SafeID tID);
    }

    public abstract partial class ComponentManager<TType> : IComponentManager, IOBJXMLSettingIO
    where TType : class, IComponentAgent
    {
        protected TSet<TType> m_ComponentSet = new TSet<TType>();

        protected abstract TType CreateItem(String tPath);

        #region XML Import/Export Settings
        public Boolean ImportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            try
            {
                //! get root node
                XmlNode tComponentsNode = null;
                //! find groups from root node
                if (xmlRoot.Name != "Components")
                {
                    tComponentsNode = xmlRoot.SelectSingleNode("Components");
                }
                else
                {
                    tComponentsNode = xmlRoot;
                }

                if (null == tComponentsNode)
                {
                    if (0 == xmlRoot.ChildNodes.Count)
                    {
                        return false;
                    }

                    //! search for node "Groups" in each childnode
                    foreach (XmlNode xmlChildren in xmlRoot.ChildNodes)
                    {
                        if (ImportSetting(xmlDoc, xmlChildren))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                //! get components list
                XmlNodeList xmlComponentList = tComponentsNode.SelectNodes("Component");
                if (null == xmlComponentList)
                {
                    return true;
                }

                ComponentManager<TType> tGeneralComponentManager = null;
                do
                {
                    //! get general name space
                    Blackboard.Namespace tGeneral = Blackboard.Find("General");
                    if (null == tGeneral)
                    {
                        break;
                    }
                    Blackboard.Slip tSlip = tGeneral.Find("Components");
                    if (null == tSlip)
                    {
                        break;
                    }
                    if (null == tSlip.Contents)
                    {
                        break;
                    }
                    if (0 == tSlip.Contents.Length)
                    {
                        break;
                    }
                    tGeneralComponentManager = tSlip.Contents[0] as ComponentManager<TType>;
                }
                while (false);

                //! \brief import each node
                foreach (XmlNode tComponent in xmlComponentList)
                {
                    if (null != tGeneralComponentManager)
                    {
                        //! load from general first
                        if (null != tComponent.Attributes["ID"])
                        {
                            SafeID tID = tComponent.Attributes["ID"].Value;

                            //! add item
                            if (Add(tGeneralComponentManager.Find(tID)))
                            {
                                continue;
                            }
                        }
                    }

                    if (null == tComponent.Attributes["Assembly"])
                    {
                        continue;
                    }
                    String tPath = tComponent.Attributes["Assembly"].Value;
                    if (this.Add(tPath))
                    {
                        continue;
                    }

                    //! add code here for future
                }
            }
            catch (Exception) { }

            return true;
        }

        public Boolean ExportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            //! check parameter
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            try
            {

                //! get root node

                XmlNode tComponentsNode = null;
                //! find parameter group set from root node
                if (xmlRoot.Name != "Components")
                {
                    tComponentsNode = xmlRoot.SelectSingleNode("Components");
                }
                else
                {
                    tComponentsNode = xmlRoot;
                }

                if (null == tComponentsNode)
                {
                    //! no parameter group set, so create a function set node
                    tComponentsNode = xmlDoc.CreateNode(XmlNodeType.Element, "Components", null);
                    xmlRoot.AppendChild(tComponentsNode);
                }
                else
                {
                    //! try to find parameter group set
                    foreach (XmlNode enumNode in tComponentsNode.ChildNodes)
                    {
                        //! find a specified parameter group set
                        if (enumNode.Name == "Component")
                        {
                            //! we find the parameter group set, remove this child
                            xmlRoot.RemoveChild(enumNode);
                        }
                    }
                }

                foreach (TType tItem in m_ComponentSet)
                {
                    if (null == tItem)
                    {
                        continue;
                    }
                    if (!tItem.Available)
                    {
                        continue;
                    }
                    if (null == tItem.Target)
                    {
                        continue;
                    }

                    XmlElement xmlComponent = xmlDoc.CreateElement("Component");

                    xmlComponent.SetAttribute("ID", tItem.ID);
                    xmlComponent.SetAttribute("Assembly", PathEx.RelativePath(Application.StartupPath, tItem.Path));
#if false
                    //! name
                    do
                    {
                        XmlElement xmlName = xmlDoc.CreateElement("Name");
                        xmlName.InnerText = tItem.Component.ComponentName;
                        xmlComponent.AppendChild(xmlName);
                    }
                    while (false);

                    //! version
                    do
                    {
                        XmlElement xmlVersion = xmlDoc.CreateElement("version");
                        xmlVersion.InnerText = tItem.Component.Version;
                        xmlComponent.AppendChild(xmlVersion);
                    }
                    while (false);

                    //! company
                    do
                    {
                        XmlElement xmlCompany = xmlDoc.CreateElement("Company");
                        xmlCompany.InnerText = tItem.Component.Version;
                        xmlComponent.AppendChild(xmlCompany);
                    }
                    while (false);
#endif

                    tComponentsNode.AppendChild(xmlComponent);
                }

            }
            catch (Exception) { }

            return true;
        }
        #endregion


        #region Item Management

        public TType[] Items
        {
            get { return m_ComponentSet.Items; }
        }

        //! \brief the number of components
        public Int32 Count
        {
            get { return m_ComponentSet.Count; }
        }


        //! \brief add component to list
        public Boolean Add(String tPath)
        {
            TType tItem = CreateItem(tPath);

            if (!tItem.Available)
            {
                return false;
            }
            else if (null == tItem.Target)
            {
                return false;
            }

            if (m_ComponentSet.Add(tItem) == SET_ADD_RESULT.SET_FAILED)
            {
                return false;
            }

            return true;
        }

        public Boolean Add(TType tItem)
        {
            if (null == tItem)
            {
                return false;
            }
            if (!tItem.Available)
            {
                return false;
            }
            else if (null == tItem.Target)
            {
                return false;
            }

            if (m_ComponentSet.Add(tItem) == SET_ADD_RESULT.SET_FAILED)
            {
                return false;
            }

            return true;
        }

        public TType Find(SafeID tID)
        {
            return m_ComponentSet.Find(tID);
        }

        //! \brief remove component from list
        public Boolean Remove(SafeID tID)
        {
            return m_ComponentSet.Remove(tID);
        }

        public Boolean AddRange(ICollection<TType> tItems)
        {
            if (null == tItems)
            {
                return false;
            }

            foreach (TType tItem in tItems)
            {
                if (null == tItem)
                {
                    continue;
                }
                if (!tItem.Available)
                {
                    continue;
                }
                if (null == tItem.Target)
                {
                    continue;
                }
                m_ComponentSet.Add(tItem);
            }

            return true;
        }
        #endregion

    }


}
