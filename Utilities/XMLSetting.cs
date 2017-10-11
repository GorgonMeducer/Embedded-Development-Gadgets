using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ESnail.Utilities.XML
{
    //! \brief Battery manage io inteface
    //! @{
    public interface IXMLSetting
    {
        //! find all available object
        String[] Find(String strPath);
        //! find all available object in specified xml struture
        String[] Find(XmlDocument xmlDocument, XmlNode xmlRootNode);
        //! Import Battery Manage Object from somewhere                              
        Boolean  Import(String strPath, ref Object tObj);  
        //! Import Battery Manage Object from somewhere with a specified index number
        Boolean  Import(String strPath, ref Object bmObj, Int32 nIndex);
        //! Import Battery Manage Object from somewhere with a specified XmlNode
        Boolean  Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref Object tObj);
        //! Import Battery Manage Object from somewhere with a specified XmlNode and a index number
        Boolean Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref Object tObj, Int32 nIndex);
        //! Export Battery Manage Object to somewhere                                
        Boolean  Export(String strPath, Object tObj);
        //! Append Battery Manage Object to somewhere                                
        Boolean  Append(String strPath, Object tObj);
        //! Append Battery Manage Object to somewhere
        Boolean  Append(XmlDocument xmlDocument, XmlNode xmlRootNode, Object tObj);
        //! IO Type information                                                      
        String   Type                                                                    
        {
            get;
        }
        //! Get Last exception                          
        Exception       GetLastError();                                                         
    }
    //! @{

    //! \name abstract class for all battery manage io 
    //! @{
    public abstract class XmlSettingIO : ESDisposableClass , IXMLSetting
    {

        protected  Exception m_Exception = null;

        //! \brief default import method
        public virtual Boolean Import(string strPath, ref object bmObj, int nIndex)
        {
            bmObj = null;
            return false;
        }

        //! \brief default import method
        public virtual Boolean Import(System.String strPath,ref System.Object bmObj)
        {
            //! import first object
            return Import(strPath,ref bmObj,0);
        }

        //! \brief default import method
        public virtual Boolean Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref System.Object bmObj)
        {
            return Import(xmlDocument,xmlRootNode,ref bmObj,0);
        }

        //! \brief default import method
        public virtual Boolean Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref Object bmObj, System.Int32 nIndex)
        {
            return false;
        }

        //! \brief default import method
        public virtual Boolean Export(System.String strPath,System.Object bmObj)
        {
            return false;
        }

        //! \brief default append method
        public virtual Boolean Append(System.String strPath, System.Object bmObj)
        {
            return false;
        }

        //! \brief default append method
        public virtual Boolean Append(XmlDocument xmlDocument, XmlNode xmlRootNode, System.Object bmObj)
        {
            return false;
        }

        //! \brief default Find method
        public virtual String[] Find(String strPath)
        {
            return null;
        }

        //! \brief default find method
        public virtual String[] Find(XmlDocument xmlDocument, XmlNode xmlRootNode)
        {
            return null;
        }

        public abstract String Type
        {
            get;
        }

        //! \brief implement interface: IBatteryManageIO.GetLastError
        public virtual Exception GetLastError()
        {
            return m_Exception;
        }

    }
    //! @}

    //! \name generic TXMLSettingIO
    //! @{
    public abstract class TXMLSettingIO<TType> : XmlSettingIO
    {

        public abstract System.String XMLRootName
        {
            get;
        }

        public abstract System.String XMLObjectName
        {
            get;
        }

        //! \brief override method for finding objects' information from a specified xml file
        public override String[] Find(string strPath)
        {
            if (!File.Exists(strPath))
            {
                return null;
            }

            XmlDocument docWriter = new XmlDocument();
            try
            {
                //! try to load a xml file
                docWriter.Load(strPath);
            }
            catch (Exception Err)
            {
                m_Exception = Err;

                return null;
            }

            return Find(docWriter, docWriter.DocumentElement);
        }

        //! \brief override method for finding objects' information from a specified xml structure
        public override String[] Find(XmlDocument xmlDocument, XmlNode xmlRootNode)
        {
            if ((null == xmlDocument) || (null == xmlRootNode))
            {
                return null;
            }

            List<String> tResultList = new List<String>();

            try
            {
                //! get root node

                XmlNode TargetNode = null;
                //! find targets from root node
                if (xmlRootNode.Name != XMLRootName)
                {
                    TargetNode = xmlRootNode.SelectSingleNode(XMLRootName);
                }
                else
                {
                    TargetNode = xmlRootNode;
                }

                if (null == TargetNode)
                {
                    if (0 == xmlRootNode.ChildNodes.Count)
                    {
                        return null;
                    }

                    //! search for targets in each childnode
                    foreach (XmlNode xmlChildren in xmlRootNode.ChildNodes)
                    {
                        String[] tResult = Find(xmlDocument, xmlChildren);
                        if (null != tResult)
                        {
                            return tResult;
                        }
                    }

                    return null;
                }

                //! get parameter group set with index number
                XmlNodeList xmlParameterGroupSetList = TargetNode.SelectNodes(XMLObjectName);
                if (null == xmlParameterGroupSetList)
                {
                    return null;
                }
                if (0 == xmlParameterGroupSetList.Count)
                {
                    return null;
                }

                foreach (XmlNode tNodeItem in xmlParameterGroupSetList)
                {
                    XmlAttributeCollection tAttributes = tNodeItem.Attributes;
                    if (null == tAttributes)
                    {
                        continue;
                    }
                    if (0 == tAttributes.Count)
                    {
                        continue;
                    }

                    StringBuilder sbAttrib = new StringBuilder();
                    //! get all attribute string
                    foreach (XmlAttribute tAttribute in tAttributes)
                    {
                        if (null == tAttribute.Value)
                        {
                            continue;
                        }
                        else if ("" == tAttribute.Value.Trim())
                        {
                            continue;
                        }

                        sbAttrib.Append(tAttribute.Value);
                        sbAttrib.Append('\t');
                    }

                    tResultList.Add(sbAttrib.ToString());
                }

            }
            catch (Exception e)
            {
                m_Exception = e;
                return null;
            }

            if (tResultList.Count > 0)
            {
                return tResultList.ToArray();
            }

            return null;
        }


        //! \brief override method for exporting object to xml file
        public override Boolean Export(System.String strPath, System.Object bmObj)
        {
            if (bmObj is TType)
            {
                return Export(strPath, (TType)bmObj);
            }

            return false;
        }

        //! \brief method for exporting object to xml file
        public Boolean Export
                                (
                                    System.String strPath,
                                    TType tObject
                                )
        {
           
            if ((null == strPath) || (null == tObject))
            {
                return false;
            }
            else if ("" == strPath.Trim())
            {
                return false;
            }


            XmlWriterSettings XmlSetting = new XmlWriterSettings();

            XmlSetting.Indent = true;                                   //!< enable indent
            XmlSetting.IndentChars = "    ";                            //!< using whitespace as indent-chars
            XmlSetting.OmitXmlDeclaration = true;

            using (XmlWriter Writer = XmlWriter.Create(strPath, XmlSetting))
            {
                //! create root node
                Writer.WriteStartElement(XMLRootName);
                Writer.WriteEndElement();
                Writer.Close();
            }

            return Append(strPath, tObject);

        }

        //! \brief override method for appending object to xml file
        public override Boolean Append(System.String strPath, System.Object tObj)
        {
            if (tObj is TType)
            {
                return Append(strPath, (TType)tObj);
            }

            return false;
        }

        //! \brief method for appending object to xml file
        public Boolean Append
                                (
                                    System.String strPath,
                                    TType tObject
                                )
        {
#if false
            //! check parameter
            if ((null == strPath) || (null == tObject))
            {
                return false;
            }
            else if ("" == strPath.Trim())
            {
                return false;
            }

            //! create a XmlDocument element
            XmlDocument docWriter = new XmlDocument();

            try
            {
                //! try to load a xml file
                docWriter.Load(strPath);
            }
            catch (XmlException)
            {
                XmlWriterSettings XmlSetting = new XmlWriterSettings();

                XmlSetting.Indent = true;                                   //!< enable indent
                XmlSetting.IndentChars = "    ";                            //!< using whitespace as indent-chars
                XmlSetting.OmitXmlDeclaration = true;

                using (XmlWriter Writer = XmlWriter.Create(strPath, XmlSetting))
                {
                    //! create root node
                    Writer.WriteStartElement(XMLRootName);
                    Writer.WriteEndElement();
                    Writer.Close();
                }
                docWriter.Load(strPath);
            }
            catch (System.IO.FileNotFoundException )
            {
                XmlWriterSettings XmlSetting = new XmlWriterSettings();

                XmlSetting.Indent = true;                                   //!< enable indent
                XmlSetting.IndentChars = "    ";                            //!< using whitespace as indent-chars
                XmlSetting.OmitXmlDeclaration = true;

                using (XmlWriter Writer = XmlWriter.Create(strPath, XmlSetting))
                {
                    //! create root node
                    Writer.WriteStartElement(XMLRootName);
                    Writer.WriteEndElement();
                    Writer.Close();
                }
                docWriter.Load(strPath);
            }
            catch (Exception e)
            {
                m_Exception = e;

                return false;
            }
#else
            if (null == tObject)
            {
                return false;
            }
            XmlDocument docWriter = XMLHelper.Open(strPath, XMLRootName);
            if (null == docWriter)
            {
                return false;
            }
#endif
            //! append 
            if (Append(docWriter, docWriter.DocumentElement, tObject))
            {
                try
                {
                    //! save xml file
                    docWriter.Save(strPath);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }


            return false;
        }

        //! \brief override method for appending object to xml file
        public override Boolean Append(XmlDocument xmlDocument, XmlNode xmlRootNode, System.Object tObj)
        {
            if (tObj is TType)
            {
                return Append(xmlDocument, xmlRootNode, (TType)tObj);
            }

            return false;
        }

        //! \brief method for appending object to xml file
        public abstract Boolean Append
                                        (
                                            XmlDocument xmlDocument,
                                            XmlNode xmlRootNode,
                                            TType BMObject
                                        );



        //! \brief method for importing object from xml file
        public override Boolean Import(System.String strPath, ref Object tObj, int nIndex)
        {
            if (tObj is TType)
            {
                TType tObject = (TType)tObj;

                if (Import(strPath, ref tObject, nIndex))
                {
                    tObj = tObject;

                    return true;
                }
            }
            return false;
        }

        //! \brief method for importing object from xml file
        public Boolean Import
                                (
                                    System.String strPath,
                                    ref TType tObject,
                                    int nIndex
                                )
        {
            if ((!File.Exists(strPath)) || (nIndex < 0))
            {
                return false;
            }



            XmlDocument docWriter = new XmlDocument();
            try
            {
                //! try to load a xml file
                docWriter.Load(strPath);
            }
            catch (Exception e)
            {
                m_Exception = e;

                return false;
            }

            return Import(docWriter, docWriter.DocumentElement, ref tObject, nIndex);
        }

        //! \brief method for importing object from xml file
        public override Boolean Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref Object tObj, int nIndex)
        {
            if (tObj is TType)
            {
                TType tObject = (TType)tObj;

                if (Import(xmlDocument, xmlRootNode, ref tObject, nIndex))
                {
                    tObj = tObject;
                    return true;
                }
            }
            return false;
        }


        //! \brief method for import TalkSet from xml file
        public abstract Boolean Import
                                (
                                    XmlDocument xmlDocument,
                                    XmlNode xmlRootNode,
                                    ref TType BMObject,
                                    int nIndex
                                );




    }
    //! @}


    //! \name generic TXMLSettingIO
    //! @{
    public abstract class TXMLSettingIOEx<TType> : TXMLSettingIO<TType>
        where TType : ISafeID
    {

        protected abstract Boolean CheckAppendObject(TType tBMObject);
        protected abstract Boolean _AppendObject(XmlDocument xmlDocument, XmlElement xmlNode, TType BMObject);

        public override Boolean Append(XmlDocument xmlDocument, XmlNode xmlRootNode, TType tBMObject)
        {
            //! check parameter
            if ((null == xmlDocument) || (null == xmlRootNode))
            {
                return false;
            }

            if (!CheckAppendObject(tBMObject))
            {
                return false;
            }

            try
            {
                //! get root node               

                XmlNode xmlObjectRootNode = xmlRootNode;
                do
                {
                    if (null == XMLRootName)
                    {
                        break;
                    }
                    if ("" == XMLRootName)
                    {
                        break;
                    }
                    if (xmlRootNode.Name == XMLRootName)
                    {
                        break;

                    }

                    xmlObjectRootNode = XMLHelper.Find(xmlRootNode, XMLRootName, 0);
                    
                } while (false);

                if (null == xmlObjectRootNode)
                {
                    //! no parameter group set, so create a function set node
                    xmlObjectRootNode = xmlDocument.CreateNode(XmlNodeType.Element, XMLRootName, null);
                    xmlRootNode.AppendChild(xmlObjectRootNode);
                }
                else
                {
                    //! try to find parameter group set
                    foreach (XmlNode enumNode in xmlObjectRootNode.ChildNodes)
                    {
                        //! find a specified parameter group set
                        if (
                                (enumNode.Name == XMLObjectName)
                            && (enumNode.Attributes["ID"] != null)
                           )
                        {
                            if (enumNode.Attributes["ID"].Value == tBMObject.ID)
                            {
                                //! we find the parameter group set, remove this child
                                xmlRootNode.RemoveChild(enumNode);
                                break;
                            }
                        }
                    }
                }

                //! create a new parameter group set
                XmlElement xmlObjectNode = xmlDocument.CreateElement(XMLObjectName);

                //! set id
                xmlObjectNode.SetAttribute("ID", tBMObject.ID);

                if (!_AppendObject(xmlDocument, xmlObjectNode, tBMObject))
                {
                    return false;
                }

                //! add new parameter group set to groups
                xmlObjectRootNode.AppendChild(xmlObjectNode);
            }
            catch (Exception e)
            {
                m_Exception = e;
                return false;
            }

            return true;
        }


        protected abstract Boolean CheckImportObject(ref TType tBMObject);
        protected abstract Boolean _ImportObject(XmlDocument xmlDocument, XmlNode xmlRootNode, ref TType tBMObject);
        public override Boolean Import(XmlDocument xmlDocument, XmlNode xmlRootNode, ref TType tBMObject, int nIndex)
        {
            if ((null == xmlDocument) || (null == xmlRootNode))
            {
                return false;
            }

            if (!CheckImportObject(ref tBMObject))
            {
                return false;
            }

            try
            {
                //! get root node

                XmlNode xmlObjectNode = xmlRootNode;
                do
                {
                    if (null == XMLRootName)
                    {
                        break;
                    }
                    if ("" == XMLRootName)
                    {
                        break;
                    }
                    if (xmlRootNode.Name == XMLRootName)
                    {
                        break;

                    }


                   xmlObjectNode = XMLHelper.Find(xmlRootNode, XMLRootName, 0);

                } while (false);

                if (null == xmlObjectNode)
                {
                    if (0 == xmlRootNode.ChildNodes.Count)
                    {
                        return false;
                    }

                    //! search for node "Groups" in each childnode
                    foreach (XmlNode xmlChildren in xmlRootNode.ChildNodes)
                    {
                        if (Import(xmlDocument, xmlChildren, ref tBMObject, nIndex))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                //! get parameter group set with index number
                XmlNodeList xmlObjectSetList = xmlObjectNode.SelectNodes(XMLObjectName);
                if (null == xmlObjectSetList)
                {
                    return false;
                }

                if (xmlObjectSetList.Count <= nIndex)
                {
                    return false;
                }

                if (!_ImportObject(xmlDocument, xmlObjectSetList[nIndex], ref tBMObject))
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                m_Exception = e;
                return false;
            }

            return true;
        }

    }
    //! @}


    public interface IOBJXMLSettingIO
    {
        //! import setting
        Boolean ImportSetting(XmlDocument xmlDoc, XmlNode xmlRoot);
        //! export setting
        Boolean ExportSetting(XmlDocument xmlDoc, XmlNode xmlRoot);

    }

    public interface IOBJXMLSettingIOEx : IOBJXMLSettingIO
    {
        //! import setting from specified file
        Boolean ImportSetting(String strFilePath);
        //! export setting to specified file
        Boolean ExportSetting(String strFilePath);
    }



    public static class XMLHelper
    {
        public static XmlDocument New(String strRootName)
        {
            return Open(null, strRootName);
        }

        public static XmlDocument Parse(String tXMLContent)
        {
            XmlDocument docWriter = new XmlDocument();

            if (null == tXMLContent)
            {
                return null;
            }
            if ("" == tXMLContent.Trim())
            {
                return null;
            }

            try
            {
                docWriter.LoadXml(tXMLContent);
            }
            catch (Exception)
            {
                return null;
            }

            return docWriter;
        }

        public static XmlNodeList Find(XmlNode tStartPoint, String tNodeName)
        {
            do
            {
                if (null == tStartPoint)
                {
                    break;
                }
                if (null == tStartPoint.ParentNode) 
                {
                    break;
                }
                if (null == tNodeName)
                {
                    return tStartPoint.ParentNode.ChildNodes;
                }
                tNodeName = tNodeName.Trim();
                if ("" == tNodeName)
                {
                    return tStartPoint.ParentNode.ChildNodes;
                }

                //! check current node
                if (tStartPoint.Name == tNodeName)
                {
                    return tStartPoint.ParentNode.SelectNodes(tNodeName);
                }

                XmlNodeList tNodeList = tStartPoint.SelectNodes(tNodeName);
                if (null != tNodeList)
                {
                    if (tNodeList.Count > 0)
                    {
                        return tNodeList;
                    }
                }

                if (null == tStartPoint.ChildNodes)
                {
                    break;
                }

                //! recursion search
                foreach (XmlNode tNode in tStartPoint.ChildNodes)
                {
                    XmlNodeList tResult = Find(tNode, tNodeName);
                    if (null != tResult)
                    {
                        return tResult;
                    }
                }

            } while (false);

            return null;
        }

        public static XmlNode Find(XmlNode tStartPoint, String tNodeName, Int32 tIndex)
        {
            do
            {
                
                if (null == tStartPoint || tIndex < 0)
                {
                    break;
                }
                if (null == tNodeName)
                {
                    return tStartPoint;
                }
                tNodeName = tNodeName.Trim();
                if ("" == tNodeName)
                {
                    return tStartPoint;
                }

                //! check current node
                if (tStartPoint.Name == tNodeName)
                {
                    return tStartPoint;
                }
                
                XmlNodeList tNodeList = tStartPoint.SelectNodes(tNodeName);
                if (null != tNodeList)
                {
                    if (tNodeList.Count > tIndex)
                    {
                        return tNodeList.Item(tIndex);
                    }
                }

                if (null == tStartPoint.ChildNodes)
                {
                    break;
                }

                //! recursion search
                foreach (XmlNode tNode in tStartPoint.ChildNodes)
                {
                    XmlNode tResult = Find(tNode, tNodeName, tIndex);
                    if (null != tResult)
                    {
                        return tResult;
                    }
                }

            } while (false);

            return null;
        }

        private static XmlDocument __Create(String strPath, String strRootName)
        {
            //! create a XmlDocument element
            XmlDocument docWriter = new XmlDocument();

            XmlWriterSettings XmlSetting = new XmlWriterSettings();

            XmlSetting.Indent = true;                                   //!< enable indent
            XmlSetting.IndentChars = "    ";                            //!< using whitespace as indent-chars
            XmlSetting.OmitXmlDeclaration = true;

            try
            {
                using (XmlWriter Writer = XmlWriter.Create(strPath, XmlSetting))
                {
                    //! create root node
                    Writer.WriteStartElement(strRootName);
                    Writer.WriteEndElement();
                    Writer.Close();
                }
            }
            catch (Exception)
            {
                return null;
            }
            docWriter.Load(strPath);

            return docWriter;
        }

        public static XmlDocument Create(String strPath, String strRootName)
        {
            if (null == strPath)
            {
                return null;
            }
            if ("" == strPath.Trim())
            {
                return null;
            }
            if (null == strRootName)
            {
                return null;
            }
            if ("" == strRootName.Trim())
            {
                return null;
            }

            return __Create(strPath, strRootName);
        }

        public static XmlDocument Open(String strPath, String strRootName)
        {
            if (null == strRootName)
            {
                return null;
            }
            if ("" == strRootName.Trim())
            {
                return null;
            }

            //! create a XmlDocument element
            XmlDocument docWriter = new XmlDocument();

            if ((null == strPath) || ("" == strPath))
            {
                StringBuilder sbRootXML = new StringBuilder();

                sbRootXML.Append('<');
                sbRootXML.Append(strRootName);
                sbRootXML.Append("/>");

                docWriter.LoadXml(sbRootXML.ToString());

                return docWriter;
            }

            if (!File.Exists(strPath))
            {
                return __Create(strPath, strRootName);
            }
            else
            {
                try
                {
                    //! try to load a xml file
                    docWriter.Load(strPath);
                    if (null != Find(docWriter, strRootName, 0))
                    {
                        return docWriter;
                    }
                }
                catch (XmlException) { }
                catch (Exception)
                {
                    return null;
                }
                return __Create(strPath, strRootName);
            }
        }
    
    }
}
