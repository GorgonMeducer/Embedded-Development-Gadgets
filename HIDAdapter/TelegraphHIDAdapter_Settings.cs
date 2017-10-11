using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using ESnail.Device;
using ESnail.Device.Adapters;
using ESnail.Device.Telegraphs;
using ESnail.Device.Telegraphs.Engines;
using ESnail.Device.Telegraphs.Pipeline;
using ESnail.Utilities;
using System.Windows.Forms;
using System.Threading;
using ESnail.CommunicationSet.Commands;
using System.ComponentModel;
using ESnail.Utilities.HEX;

namespace ESnail.Device.Adapters.USB.HID
{
    //! \name telegraph based HID adapter
    //! @{
    partial class TelegraphHIDAdapter
    {
        public override Boolean ExportSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {

            //! check parameter
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            

            try
            {

                //! get root node

                XmlNode AdapterNode = null;
                //! find parameter group set from root node
                if (xmlRoot.Name != "AdapterSetting")
                {
                    AdapterNode = xmlRoot.SelectSingleNode("AdapterSetting");
                }
                else
                {
                    AdapterNode = xmlRoot;
                }

                if (null == AdapterNode)
                {
                    //! no parameter group set, so create a function set node
                    AdapterNode = xmlDoc.CreateNode(XmlNodeType.Element, "AdapterSetting", null);
                    xmlRoot.AppendChild(AdapterNode);
                }
                else
                {
                    //! try to find parameter group set
                    foreach (XmlNode enumNode in AdapterNode.ChildNodes)
                    {
                        //! find a specified parameter group set
                        if (enumNode.Name == "TelegraphHIDAdapter")
                        {
                            //! we find the parameter group set, remove this child
                            xmlRoot.RemoveChild(enumNode);
                            break;
                        }
                    }
                }

                //! create a new setting 
                XmlElement newAdapterSetting = xmlDoc.CreateElement("TelegraphHIDAdapter");

                //! adapter setting
                do
                {
                    XmlElement xmlAdapterSetting = xmlDoc.CreateElement("Setting");
                    xmlAdapterSetting.InnerText = Settings;
                    newAdapterSetting.AppendChild(xmlAdapterSetting);
                }
                while (false);

                //! USB
                do
                {
                    XmlElement xmlUSBSetting = xmlDoc.CreateElement("USB");

                    xmlUSBSetting.SetAttribute("VID", m_VID.ToString("X4"));
                    xmlUSBSetting.SetAttribute("PID", m_PID.ToString("X4"));

                    newAdapterSetting.AppendChild(xmlUSBSetting);
                }
                while (false);


                AdapterNode.AppendChild(newAdapterSetting);
            }
            catch (Exception Err)
            {
                Err.ToString();
            }

            return true;
        }


        public override Boolean ImportSetting(System.Xml.XmlDocument xmlDoc, System.Xml.XmlNode xmlRoot)
        {
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            try
            {
                //! get root node
                XmlNode BMCBatteryNode = null;

                if (xmlRoot.Name != "AdapterSetting")
                {
                    BMCBatteryNode = xmlRoot.SelectSingleNode("AdapterSetting");
                }
                else
                {
                    BMCBatteryNode = xmlRoot;
                }

                if (null == BMCBatteryNode)
                {
                    if (0 == xmlRoot.ChildNodes.Count)
                    {
                        return false;
                    }

                    foreach (XmlNode xmlChildren in xmlRoot.ChildNodes)
                    {
                        if (ImportSetting(xmlDoc, xmlChildren))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                //! get parameter group set with index number

                XmlNode xmlAdapter = BMCBatteryNode.SelectSingleNode("TelegraphHIDAdapter");
                if (null == xmlAdapter)
                {
                    return false;
                }

                //! read setting
                do
                {
                    XmlNode xmlAdapterSetting = xmlAdapter.SelectSingleNode("Setting");
                    if (null == xmlAdapterSetting)
                    {
                        break;
                    }
                    String tSetting = xmlAdapterSetting.InnerText;
                    if (null == tSetting)
                    {
                        break;
                    }
                    else if ("" == tSetting.Trim())
                    {
                        break;
                    }

                    m_strUSBDevicePathName = tSetting;
                }
                while (false);

                //! read USB setting
                do
                {
                    XmlNode xmlUSBSetting = xmlAdapter.SelectSingleNode("USB");
                    if (null == xmlUSBSetting)
                    {
                        break;
                    }

                    do
                    {
                        //! VID
                        if (null == xmlUSBSetting.Attributes["VID"])
                        {
                            break;
                        }
                        String strVID = xmlUSBSetting.Attributes["VID"].Value;
                        UInt16[] hwResult = null;
                        if (!HEXBuilder.HEXStringToU16Array(strVID, ref hwResult))
                        {
                            break;
                        }
                        m_VID = hwResult[0];
                    }
                    while (false);

                    do
                    {
                        //! VID
                        if (null == xmlUSBSetting.Attributes["PID"])
                        {
                            break;
                        }
                        String strPID = xmlUSBSetting.Attributes["PID"].Value;
                        UInt16[] hwResult = null;
                        if (!HEXBuilder.HEXStringToU16Array(strPID, ref hwResult))
                        {
                            break;
                        }
                        m_PID = hwResult[0];
                    }
                    while (false);
                }
                while (false);

            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }

            return true;
        }

    }
}