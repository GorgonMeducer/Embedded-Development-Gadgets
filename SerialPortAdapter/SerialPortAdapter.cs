using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using ESnail.Device.Adapters;
using ESnail.Utilities;

namespace ESnail.Device.Adapters.SerialPort
{
    public abstract class SerialPortAdapter : SerialPortDeviceAdapter
    {
        protected SerialPortDriver m_SerialPort = null;

        //! \brief default constructor
        public SerialPortAdapter()
            : base(null, new SerialPortDriver())
        {
            m_SerialPort = m_Device as SerialPortDriver;
        }

        //! \brief constructor with ID
        public SerialPortAdapter(SafeID tID)
            : base(tID,new SerialPortDriver())
        {
            m_SerialPort = m_Device as SerialPortDriver;
        }

        //! \brief dispose this object
        protected override void _Dispose()
        {
            if (null != m_SerialPort)
            {
                m_SerialPort.Dispose();
                m_SerialPort = null;
            }
            base._Dispose();
        }


        //! find HID devices
        public static System.String[] FindDevice()
        {
            SerialPortDriver tDriver = new SerialPortDriver();

            return tDriver.FindDevice(tDriver.DeviceType());
        }

        //! \brief system busy state
        public override Boolean IsBusy
        {
            get { return false; }
        }

        #region serial port setting
        //! \brief baudrate
        public Int32 Baudrate
        {
            get
            {
                if (null == m_SerialPort)
                {
                    return -1;
                }

                return m_SerialPort.Baudrate;
            }
            set
            {
                if (null != m_SerialPort)
                {
                    m_SerialPort.Baudrate = value;
                }
            }
        }

        //! \brief Stopbits
        public StopBits StopBits
        {
            get
            {
                if (null == m_SerialPort)
                {
                    return StopBits.None;
                }

                return m_SerialPort.StopBits;
            }
            set
            {
                if (null != m_SerialPort)
                {
                    m_SerialPort.StopBits = value;
                }
            }
        }

        //! \brief parity
        public Parity Parity
        {
            get
            {
                if (null == m_SerialPort)
                {
                    return Parity.None;
                }

                return m_SerialPort.Parity;
            }
            set
            {
                if (null != m_SerialPort)
                {
                    m_SerialPort.Parity = value;
                }
            }
        }

        //! \brief data bits
        public Int32 DataBits
        {
            get
            {
                if (null == m_SerialPort)
                {
                    return 0;
                }

                return m_SerialPort.DataBits;
            }
            set
            {
                if (null != m_SerialPort)
                {
                    m_SerialPort.DataBits = value;
                }
            }
        }

        #endregion
    }
}
