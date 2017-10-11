using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Utilities;
using System.Threading;

namespace ESnail.Device.Adapters
{
    public abstract class SerialPortDeviceAdapter : SingleDeviceAdapter
    {

        protected String m_strSerialPortSetting = null;

        //! override adapter type property
        public override String Type
        {
            get { return "Serial Port Device Adapter"; }
        }

        //! constructor
        public SerialPortDeviceAdapter(SafeID tID, IDevice DeviceInterface)
            : base(tID, DeviceInterface)
        {
            
        }

        //! settings for single endpoint usb device
        public override System.String Settings
        {
            get { return m_strSerialPortSetting; }
            set
            {
                if (!Open)
                {
                    //! setting could only be changed when device was disconnected.
                    m_strSerialPortSetting = value;
                    WriteLogLine("Change Setting succeeded.");
                }
                else
                {
                    WriteLogLine("Change Setting failed. Setting could only be changed when adapter was closed.");
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
                        lock (m_Signal)
                        {
                            try
                            {
                                m_Device.OpenDevice(m_strSerialPortSetting);
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
                        lock(m_Signal) 
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
