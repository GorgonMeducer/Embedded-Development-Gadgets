using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities;
using ESnail.Device;

namespace ESnail.Device.Adapters.USB
{
    //! single endpoint usb device adapter
    public abstract class SingleEndPointUSBDeviceAdapter : SingleDeviceAdapter
    {
        protected System.String m_strUSBDevicePathName = null;

        //! override adapter type property
        public override string Type
        {
            get { return "Single EndPoint USB ESnail.Device Adapter"; }
        }

        //! constructor
        public SingleEndPointUSBDeviceAdapter(SafeID tID, IDevice DeviceInterface)
            : base(tID, DeviceInterface)
        {
            
        }

        //! settings for single endpoint usb device
        public override String Settings
        {
            get { return m_strUSBDevicePathName; }
            set 
            {
                if (!Open)
                {
                    //! setting could only be changed when device was disconnected.
                    m_strUSBDevicePathName = value;
                    WriteLogLine("Change Setting succeeded.");
                }
                else
                {
                    WriteLogLine("Change Setting failed. Setting could only be changed when adapter is closed.");
                }
            }
        }

        //! open adapter
        public override Boolean Open
        {
            set
            {
                if (value == true)
                {
                    if (!Open)
                    {
                        lock(m_Signal) 
                        {
                            try
                            {
                                m_Device.OpenDevice(m_strUSBDevicePathName);
                            }
                            catch (Exception) { }
                        }
                        if (Open)
                        {
                            WriteLogLine("Open adapter succeeded.");
                            //! raising event
                            OnDeviceOpened();
                        }
                        else
                        {
                            WriteLogLine("Open adapter failed.");
                        }
                    }
                }
                else
                {
                    if (Open)
                    {
                        this.IsWorking = false;
                        lock (m_Signal)
                        {
                            m_Device.CloseDevice();
                        }
                        WriteLogLine("Adapter closed.");
                        //! raising event
                        OnDeviceClosed();
                    }                    
                }
            }
        }


    }
}