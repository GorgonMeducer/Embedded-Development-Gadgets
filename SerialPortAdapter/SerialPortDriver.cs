using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;
using Microsoft.Win32;
using System.IO.Ports;
namespace ESnail.Device.Adapters.SerialPort
{
    public class SerialPortDriver : IDevice, IDisposable
    {
        private System.IO.Ports.SerialPort m_SerialPort = new System.IO.Ports.SerialPort();
        private String m_ErrorInfo = "";
        

        //! \brief device info
        public String DeviceInfo()
        {
            if (null == m_SerialPort)
            {
                return "No device information.";
            }

            StringBuilder sbDeviceInfo = new StringBuilder();

            //! serial port name
            //sbDeviceInfo.Append(m_SerialPort.PortName);
            //sbDeviceInfo.Append(':');

            //! baudrate
            sbDeviceInfo.Append(m_SerialPort.BaudRate.ToString());
            sbDeviceInfo.Append(',');

            //! data bits
            sbDeviceInfo.Append(m_SerialPort.DataBits.ToString());
            sbDeviceInfo.Append(',');

            //! parity
            sbDeviceInfo.Append(m_SerialPort.Parity.ToString());
            sbDeviceInfo.Append(',');

            //! stopbits
            switch (m_SerialPort.StopBits)
            { 
                case StopBits.One:
                    sbDeviceInfo.Append('1');
                    break;
                case StopBits.Two:
                    sbDeviceInfo.Append('2');
                    break;
                case StopBits.OnePointFive:
                    sbDeviceInfo.Append("1.5");
                    break;
            }
            
           

            return sbDeviceInfo.ToString();
        }

        //! \brief version info
        public String Version()
        {
            return "ESnail Studio, Serial Port Driver, Ver1.00";
        }

        //! \brief device type info
        public String DeviceType()
        {
            return "ESNAIL_SERIAL_PORT";
        }

        //! \brief register device notification
        public Boolean RegisterDeviceNotification(IntPtr hwndHandle)
        {
            //! doing nothing at all
            return true;
        }

        //! \brief unregister device notification
        public Boolean UnregisterDeviceNotification()
        {
            //! doing nothing at all
            return false;
        }

        //! \brief get last error info
        public String GetErrorInfo()
        {
            String tError = m_ErrorInfo;
            m_ErrorInfo = "";
            return tError;
        }

        //! \brief check whether a message is belong to this device
        public Boolean IsMyDevice(IntPtr pMessage)
        {
            return false;
        }

        //! \brief find all available devices
        public String[] FindDevice(String tDeviceType, params object[] Parameter)
        {
            if (this.DeviceType() != tDeviceType)
            {
                return null;
            }
            
            return System.IO.Ports.SerialPort.GetPortNames();
            /*
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                List<String> tComList = new List<String>();
                String[] sSubKeys = keyCom.GetValueNames();
                foreach (String sName in sSubKeys)
                {
                    tComList.Add((String)keyCom.GetValue(sName));
                }

                if (tComList.Count > 0)
                {
                    return tComList.ToArray();
                }
            }
            return null;*/
        }

        //! \brife open serial port
        public Boolean OpenDevice(String tComName)
        {
            if (null == m_SerialPort)
            {
                return false;
            }

            try
            {
                m_SerialPort.PortName = tComName;
                m_SerialPort.Open();
            }
            catch (Exception Err)
            {
                m_ErrorInfo = Err.ToString();
                return false;
            }

            return true;
        }

        //! \brief close serial port
        public Boolean CloseDevice()
        {
            if (null == m_SerialPort)
            {
                return true;
            }
            try
            {
                m_SerialPort.Close();
                System.IO.Ports.SerialPort tSerialPort = new System.IO.Ports.SerialPort();
                tSerialPort.BreakState = m_SerialPort.BreakState;
                tSerialPort.DataBits = m_SerialPort.DataBits;
                tSerialPort.DiscardNull = m_SerialPort.DiscardNull;
                tSerialPort.DtrEnable = m_SerialPort.DtrEnable;
                tSerialPort.Handshake = m_SerialPort.Handshake;
                tSerialPort.NewLine = m_SerialPort.NewLine;
                tSerialPort.BaudRate = m_SerialPort.BaudRate;
                tSerialPort.Parity = m_SerialPort.Parity;
                tSerialPort.ParityReplace = m_SerialPort.ParityReplace;
                tSerialPort.PortName = m_SerialPort.PortName;
                tSerialPort.ReadBufferSize = m_SerialPort.ReadBufferSize;
                tSerialPort.ReadTimeout = m_SerialPort.ReadTimeout;
                tSerialPort.ReceivedBytesThreshold = m_SerialPort.ReceivedBytesThreshold;
                tSerialPort.RtsEnable = m_SerialPort.RtsEnable;
                tSerialPort.Site = m_SerialPort.Site;
                tSerialPort.StopBits = m_SerialPort.StopBits;
                tSerialPort.WriteBufferSize = m_SerialPort.WriteBufferSize;
                tSerialPort.WriteTimeout = m_SerialPort.WriteTimeout;
            }
            catch (Exception Err)
            {
                m_ErrorInfo = Err.ToString();
                return false;
            }

            return true;
        }

        //! \brief check whether this device is opened
        public Boolean isOpen
        {
            get 
            {
                if (null == m_SerialPort)
                {
                    return false;
                }

                return m_SerialPort.IsOpen; 
            }
        }

        public String PortName
        {
            get { return m_SerialPort.PortName; }
        }

        //! \brief write data to serial port
        public Boolean WriteDevice(Byte[] tData)
        {
            if (null == m_SerialPort)
            {
                return false;
            }
            else if (null == tData)
            {
                return true;
            }
            else if (0 == tData.Length)
            {
                return true;
            }


            try
            {
                if (!m_SerialPort.IsOpen)
                {
                    return false;
                }

                m_SerialPort.Write(tData, 0, tData.Length);

                return true;
            }
            catch (Exception Err)
            {
                m_ErrorInfo = Err.ToString();
                return false;
            }
        }

        //! \brief read data from serial port
        public Boolean ReadDevice(ref Byte[] tData)
        {
            if (null == m_SerialPort)
            {
                return false;
            }

            try
            {
                if (!m_SerialPort.IsOpen)
                {
                    return false;
                }
                else if (m_SerialPort.BytesToRead <= 0)
                {
                    return false ;
                }

                tData = new Byte[m_SerialPort.BytesToRead];

                if (m_SerialPort.Read(tData, 0, tData.Length) > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception Err)
            {
                m_ErrorInfo = Err.ToString();
                return false;
            }
        }

        //! \brief baudrate
        public Int32 Baudrate
        {
            get 
            {
                if (null == m_SerialPort)
                {
                    return -1;
                }

                return m_SerialPort.BaudRate; 
            }
            set
            {
                if (null == m_SerialPort)
                {
                    return;
                }
                else if (m_SerialPort.IsOpen)
                {
                    return;
                }
                if (value <= 0)
                {
                    return;
                }

                m_SerialPort.BaudRate = value;
            }
        }

        //! \brief set/get stopbit(s)
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
                if (null == m_SerialPort)
                {
                    return;
                }
                else if (m_SerialPort.IsOpen)
                {
                    return;
                }

                m_SerialPort.StopBits = value;
            }
        }

        //! \brief set/get parity
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
                if (null == m_SerialPort)
                {
                    return;
                }
                else if (m_SerialPort.IsOpen)
                {
                    return;
                }

                m_SerialPort.Parity = value;
            }
        }

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
                if (null == m_SerialPort)
                {
                    return;
                }
                else if (m_SerialPort.IsOpen)
                {
                    return;
                }

                if ((m_SerialPort.DataBits >= 5) && (m_SerialPort.DataBits <= 8))
                {
                    m_SerialPort.DataBits = value;
                }
            }
        }

       

        #region dispose this object
        private Boolean m_bDisposed = false;
        
        public Boolean Disposed
        {
            get { return m_bDisposed; }
        }

        public void Dispose()
        {
            if (!m_bDisposed)
            {
                m_bDisposed = true;

                try
                {
                    if (null != m_SerialPort)
                    {
                        m_SerialPort.Dispose();
                    }
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
                finally
                {
                    m_SerialPort = null;
                    GC.SuppressFinalize(this);
                }
            }
        }
        #endregion

    }
}
