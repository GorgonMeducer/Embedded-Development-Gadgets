using System;
using System.Collections.Generic;
using System.Text;
using HID;
using ESnail.Device;

namespace ESnail.Device.Adapters.USB.HID
{
    internal class ESnailHIDDriver : HidDevice, IDevice
    {
        private IntPtr m_pdeviceHandler = IntPtr.Zero;
        private String m_strDevicePathString = null;
        private String m_strLastErrorInfo = null;
        private Boolean m_bIfConnected = false;

        public String DeviceInfo()
        {
            if (null != m_strDevicePathString)
            {
                return m_strDevicePathString;
            }

            return "";
        }

        public string Version()
        {
            return "ESnail Studio, HID Device Driver, Ver1.0.2.0";
        }

        public String DeviceType()
        {
            return "ES_USB_HID";
        }

        public Boolean RegisterDeviceNotification(IntPtr hwndHandle)
        {
            if (false == DeviceManagement.RegisterForHIDDeviceNotifications(hwndHandle,ref m_pdeviceHandler))
            {
                m_strLastErrorInfo = "Failed to register for HID device notifications!";
                return false;
            }

            return true;
        }

        public Boolean UnregisterDeviceNotification()
        {
            if (false == DeviceManagement.StopReceivingDeviceNotifications(m_pdeviceHandler))
            {
                m_strLastErrorInfo = "Failed to stop receiving HID device notifications!";
                return false;
            }

            return true;
        }

        public String GetErrorInfo()
        {
            String tResultString = m_strLastErrorInfo;

            m_strLastErrorInfo = "No Error";

            return tResultString;
        }

        public Boolean IsMyDevice(IntPtr pMessage)
        {
            return isDevice(pMessage);
        }

        public String[] FindDevice(String tDeviceType, params Object[] tParameter)
        {
            /*! check device ID */
            if (tDeviceType != "ES_USB_HID")
            {
                m_strLastErrorInfo = "Unknown device type.";
                return null;
            }

            if (null == tParameter)
            {
                m_strLastErrorInfo = "Illegal Vender ID and Product ID";
                return null;
            }
            else if (tParameter.Length < 2)
            {
                m_strLastErrorInfo = "Illegal Vender ID and Product ID";
                return null;
            }


            UInt16 tVenderID = 0;
            UInt16 tProductID = 0;

            if ((tParameter[0] is UInt16) || (tParameter[0] is Int16) || (tParameter[0] is UInt32) || (tParameter[0] is Int32))
            {
                tVenderID = (UInt16)tParameter[0];
            }
            else
            {
                m_strLastErrorInfo = "Illegal Vender ID and Product ID:";
                return null;
            }

            if ((tParameter[1] is UInt16) || (tParameter[1] is Int16) || (tParameter[1] is UInt32) || (tParameter[1] is Int32))
            {
                tProductID = (UInt16)tParameter[1];
            }
            else
            {
                m_strLastErrorInfo = "Illegal Vender ID and Product ID:";
                return null;
            }

            String[] tResult = DeviceManagement.FindHIDDevices((Int16)tVenderID, (Int16)tProductID);
            if (null == tResult)
            {
                m_strLastErrorInfo = "Could not find specified USB HID device!";
            }

            return tResult;

        }

        public Boolean OpenDevice(String strDevicePathName)
        {
            //! check parameter
            if (null == strDevicePathName)
            {
                m_strLastErrorInfo = "Illegal parameter, failed to open HID device.";
                return false;
            }
            else if ("" == strDevicePathName.Trim())
            {
                m_strLastErrorInfo = "Illegal parameter, failed to open HID device.";
                return false;
            }

            String strTemp = strDevicePathName.Clone() as String;

            //! maybe connection has already been established
            if (null != m_strDevicePathString)
            {
                if (m_strDevicePathString != strDevicePathName)
                {
                    //! close device first
                    CloseDevice();
                }
                else
                {
                    if (this.isOpen)
                    {
                        return true;
                    }
                }
            }
            

            UInt16 iResult = openDevice(strTemp);
            //! try to open device
            if (HidLibConstants.RES_OK != iResult)
            {
                m_bIfConnected = false;
                switch (iResult)
                {
                    case HidLibConstants.ERROR_NO_DEVICE:                    
                        m_strLastErrorInfo = "HID device dose not exist!";
                        break;
                    default:
                        m_strLastErrorInfo = "Could not open specified HID device!";
                        break;
                }

                return false;
            }
            
            m_bIfConnected = true;
            m_strDevicePathString = strTemp;

            return true;
        }

        public Boolean CloseDevice()
        {
            //! try to close HID device
            m_bIfConnected = false;
            flushQueue();
            UInt16 iResult = closeDevice();
            
            

            if (HidLibConstants.RES_OK != iResult)
            {
                if (null == m_strDevicePathString)
                {
                    return true;
                }

                //! update error message
                m_strLastErrorInfo = "Failed to close HID device!";

                return false;
            }
            
            m_strDevicePathString = null;

            return true;
        }

        public Boolean isOpen
        {
            get { return (isConnected() && m_bIfConnected); }
        }

        public Boolean WriteDevice(Byte[] Data)
        {
            UInt16 iResult = writeToDevice(Data);

            if (!isOpen)
            {
                return false;
            }

            if (HidLibConstants.RES_OK != iResult)
            {
                switch (iResult)
                {
                    case HidLibConstants.ERROR_INVALID_BUFFERSIZE:
                        m_strLastErrorInfo = "Data package too large, please try a smaller one.";
                        break;
                    case HidLibConstants.ERROR_FAILED_TO_WRITE_TO_DEVICE:
                        m_strLastErrorInfo = "Failed to write data to HID device.";
                        break;
                    default:
                        m_strLastErrorInfo = "Unknown Error!";
                        break;
                }

                return false;
            }

            return true;
        }

        public Boolean ReadDevice(ref byte[] tData)
        {
            if (!isOpen)
            {
                return false;
            }
            Byte[] tBuffer = null;
            UInt16 iResult = readFromDevice(ref tBuffer);
            if (HidLibConstants.RES_OK != iResult)
            {
                switch (iResult)
                {
                    case HidLibConstants.ERROR_INVALID_INPUTREPORT_LENGTH:
                        m_strLastErrorInfo = "Could not get input data from HID device. Maybe it is a write-only device.";
                        break;
                    case HidLibConstants.RES_NO_DATA:
                        m_strLastErrorInfo = "No date income.";
                        break;
                    case HidLibConstants.ERROR_FAILED_TO_READ_FROM_DEVICE:
                        m_strLastErrorInfo = "Failed read data from HID device.";
                        break;
                    default:
                        m_strLastErrorInfo = "Unknown Error!";
                        break;
                }

                return false;
            }

            tData = new Byte[tBuffer.Length - 1];

            Array.Copy(tBuffer, 1, tData, 0, tBuffer.Length - 1);

            return true;
        }
    }
}
