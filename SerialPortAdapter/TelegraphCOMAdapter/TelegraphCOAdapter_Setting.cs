using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device.Adapters;
using ESnail.Device.Telegraphs;
using ESnail.Device.Telegraphs.Engines;
using ESnail.Device;
using ESnail.CommunicationSet.Commands;
using System.Xml;
using ESnail.Utilities;

namespace ESnail.Device.Adapters.SerialPort
{
    partial class TelegraphCOMAdapter
    {
        public override Boolean ExportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
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
                if (xmlRoot.Name != "AdapterDefaultSetting")
                {
                    AdapterNode = xmlRoot.SelectSingleNode("AdapterDefaultSetting");
                }
                else
                {
                    AdapterNode = xmlRoot;
                }

                if (null == AdapterNode)
                {
                    //! no parameter group set, so create a function set node
                    AdapterNode = xmlDoc.CreateNode(XmlNodeType.Element, "AdapterDefaultSetting", null);
                    xmlRoot.AppendChild(AdapterNode);
                }
                else
                {
                    //! try to find parameter group set
                    foreach (XmlNode enumNode in AdapterNode.ChildNodes)
                    {
                        //! find a specified parameter group set
                        if (enumNode.Name == "TelegraphSerialPortAdapter")
                        {
                            //! we find the parameter group set, remove this child
                            xmlRoot.RemoveChild(enumNode);
                            break;
                        }
                    }
                }

                //! create a new setting 
                XmlElement newAdapterSetting = xmlDoc.CreateElement("TelegraphSerialPortAdapter");

                /*
                //! port
                do
                {
                    XmlElement xmlPort = xmlDoc.CreateElement("Port");
                    xmlPort.InnerText = m_strSerialPortSetting;
                    newAdapterSetting.AppendChild(xmlPort);
                }
                while (false);
                */
                //! baudrate
                do
                {
                    XmlElement xmlBaudrate = xmlDoc.CreateElement("Baudrate");
                    xmlBaudrate.InnerText = m_SerialPort.Baudrate.ToString();
                    newAdapterSetting.AppendChild(xmlBaudrate);
                }
                while (false);

                //! databit
                do
                {
                    XmlElement xmlDatabits = xmlDoc.CreateElement("DataBits");
                    xmlDatabits.InnerText = m_SerialPort.DataBits.ToString();
                    newAdapterSetting.AppendChild(xmlDatabits);
                }
                while (false);

                //! stopbits
                do
                {
                    XmlElement xmlStopbits = xmlDoc.CreateElement("StopBits");
                    xmlStopbits.InnerText = m_SerialPort.StopBits.ToString();
                    newAdapterSetting.AppendChild(xmlStopbits);
                }
                while (false);

                //! parity
                do
                {
                    XmlElement xmlParity = xmlDoc.CreateElement("Parity");
                    xmlParity.InnerText = m_SerialPort.Parity.ToString();
                    newAdapterSetting.AppendChild(xmlParity);
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

        public override Boolean ImportDefaultSetting(XmlDocument xmlDoc, XmlNode xmlRoot)
        {
            if ((null == xmlDoc) || (null == xmlRoot))
            {
                return false;
            }

            try
            {
                //! get root node

                XmlNode AdapterNode = null;
                //! find groups from root node
                if (xmlRoot.Name != "AdapterDefaultSetting")
                {
                    AdapterNode = xmlRoot.SelectSingleNode("AdapterDefaultSetting");
                }
                else
                {
                    AdapterNode = xmlRoot;
                }

                if (null == AdapterNode)
                {
                    if (0 == xmlRoot.ChildNodes.Count)
                    {
                        return false;
                    }

                    //! search for node "Groups" in each childnode
                    foreach (XmlNode xmlChildren in xmlRoot.ChildNodes)
                    {
                        if (ImportDefaultSetting(xmlDoc, xmlChildren))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                //! get parameter group set with index number
                XmlNode xmlAdapterSetting = AdapterNode.SelectSingleNode("TelegraphSerialPortAdapter");
                if (null == xmlAdapterSetting)
                {
                    return false;
                }
                /*
                //port
                do
                {
                    XmlNode xmlPort = xmlAdapterSetting.SelectSingleNode("Port");
                    if (null == xmlPort)
                    {
                        break;
                    }
                    String tPortSetting = xmlPort.InnerText;
                    if (null == tPortSetting)
                    {
                        break;
                    }
                    if ("" == tPortSetting.Trim())
                    {
                        break;
                    }
                    m_strSerialPortSetting = tPortSetting;
                }
                while (false);
                */
                //! baudrate
                do
                {
                    XmlNode xmlBaudrate = xmlAdapterSetting.SelectSingleNode("Baudrate");
                    if (null == xmlBaudrate)
                    {
                        break;
                    }
                    String tBaudrate = xmlBaudrate.InnerText;
                    if (null == tBaudrate)
                    {
                        break;
                    }
                    if ("" == tBaudrate.Trim().ToUpper())
                    {
                        break;
                    }
                    Int32 tResult;
                    if (Int32.TryParse(tBaudrate, out tResult))
                    {
                        m_SerialPort.Baudrate = tResult;
                    }
                }
                while (false);

                //! databit
                do
                {
                    XmlNode xmlDatabits = xmlAdapterSetting.SelectSingleNode("DataBits");
                    if (null == xmlDatabits)
                    {
                        break;
                    }
                    String tDatabits = xmlDatabits.InnerText;
                    if (null == tDatabits)
                    {
                        break;
                    }
                    if ("" == tDatabits.Trim().ToUpper())
                    {
                        break;
                    }
                    Int32 tResult;
                    if (Int32.TryParse(tDatabits, out tResult))
                    {
                        m_SerialPort.DataBits = tResult;
                    }

                }
                while (false);

                //! Stopbits
                do
                {
                    XmlNode xmlStopBits = xmlAdapterSetting.SelectSingleNode("StopBits");
                    if (null == xmlStopBits)
                    {
                        break;
                    }
                    String tStopBits = xmlStopBits.InnerText;
                    if (null == tStopBits)
                    {
                        break;
                    }
                    if ("" == tStopBits.Trim())
                    {
                        break;
                    }
                    tStopBits = tStopBits.ToUpper();
                    if (System.IO.Ports.StopBits.None.ToString().ToUpper() == tStopBits)
                    {
                        m_SerialPort.StopBits = System.IO.Ports.StopBits.None;
                    }
                    else if (System.IO.Ports.StopBits.OnePointFive.ToString().ToUpper() == tStopBits)
                    {
                        m_SerialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    }
                    else if (System.IO.Ports.StopBits.Two.ToString().ToUpper() == tStopBits)
                    {
                        m_SerialPort.StopBits = System.IO.Ports.StopBits.Two;
                    }
                    else
                    { 
                        m_SerialPort.StopBits = System.IO.Ports.StopBits.One;
                    }
                }
                while (false);

                //! parity
                do
                {
                    XmlNode xmlParity = xmlAdapterSetting.SelectSingleNode("Parity");
                    if (null == xmlParity)
                    {
                        break;
                    }
                    String tParity = xmlParity.InnerText;
                    if (null == tParity)
                    {
                        break;
                    }
                    if ("" == tParity.Trim())
                    {
                        break;
                    }
                    tParity = tParity.ToUpper();
                    if (System.IO.Ports.Parity.Even.ToString().ToUpper() == tParity)
                    {
                        m_SerialPort.Parity = System.IO.Ports.Parity.Even;
                    }
                    else if (System.IO.Ports.Parity.Mark.ToString().ToUpper() == tParity)
                    {
                        m_SerialPort.Parity = System.IO.Ports.Parity.Mark;
                    }
                    else if (System.IO.Ports.Parity.Odd.ToString().ToUpper() == tParity)
                    {
                        m_SerialPort.Parity = System.IO.Ports.Parity.Odd;
                    }
                    else if (System.IO.Ports.Parity.Space.ToString().ToUpper() == tParity)
                    {
                        m_SerialPort.Parity = System.IO.Ports.Parity.Space;
                    }
                    else
                    {
                        m_SerialPort.Parity = System.IO.Ports.Parity.None;
                    }
                }
                while (false);

            }
            catch (Exception Err)
            {
                Err.ToString();
            }
            
            return true;
        }
    }
}
