using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms; 
using ESnail.Utilities;
using System.Threading;
using ESnail.Device;
using ESnail.Device.Telegraphs;
using ESnail.Utilities.Windows;

namespace ESnail.Device.Adapters
{
    public delegate void DeviceDisconnected();
    public delegate void DeviceConnected();
    public delegate void DeviceOpened(SingleDeviceAdapter tAdapter);
    public delegate void DeviceClosed(SingleDeviceAdapter tAdapter);

    //! single device adapter
    public abstract class SingleDeviceAdapter : Adapter
    {
        protected IDevice m_Device = null;
        protected Object m_Signal = new Object();

        private const Int32 WM_DEVICECHANGE             = 0x0219;
        private const Int32 DBT_DEVICEARRIVAL           = 0x8000;
        private const Int32 DBT_DEVICEREMOVECOMPLETE    = 0x8004;

        //! override adapter type property
        public override String Type
        {
            get { return "Single Device Adapter"; }
        }

        //! constructor 
        public SingleDeviceAdapter(SafeID tID, IDevice DeviceInterface)
            : base(tID)            
        {
            m_Device = DeviceInterface;
        }

        //! open adapter
        public override Boolean Open
        {
            get
            {
                
                lock(m_Signal)
                {
                    if (null != m_Device)
                    {
                        return m_Device.isOpen;
                    }
                }

                return false;
            }
        }

        public abstract System.String Settings
        {
            get;
            set;
        }


        public event DeviceOpened DeviceOpenedEvent;
        public event DeviceClosed DeviceClosedEvent;

        protected void OnDeviceOpened()
        {
            BeginInvoke(DeviceOpenedEvent,this);
        }

        protected void OnDeviceClosed()
        {
            BeginInvoke(DeviceClosedEvent, this);
        }

        //! get device error information
        public virtual String ErrorMessage
        {
            get 
            {
                lock (m_Signal)
                {
                    if (null == m_Device)
                    {
                        return "No defined device!";
                    }

                    return m_Device.GetErrorInfo();
                }
            }
        }

        //! get device information
        public virtual String DeviceInfo
        {
            get
            {
                lock (m_Signal)
                {
                    if (null == m_Device)
                    {
                        return "No defined device!";
                    }

                    return m_Device.DeviceInfo();
                }
            }
        }

        //! get device version information
        public virtual String DeviceVersion
        {
            get
            {
                lock (m_Signal)
                {
                    if (null == m_Device)
                    {
                        return "No defined device!";
                    }

                    return m_Device.Version();
                }
            }
        }

        //! get device type information
        public virtual System.String DeviceType
        {
            get
            {
                lock (m_Signal)
                {
                    if (null == m_Device)
                    {
                        return "No defined device!";
                    }

                    return m_Device.DeviceType();
                }
            }
        }

        //! disconnect event
        public event DeviceDisconnected DisconnectNotice;

        //! raising event: Disconnected
        protected virtual void OnDisconnect()
        {
            try
            {
                //! try to close device
                this.Open = false;
                
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

            WriteLogLine("Adapter disconnect.");

            if (null != DisconnectNotice)
            {
                DisconnectNotice.Invoke();
            }            
        }

        //! connnect event
        public event DeviceConnected ConnectNotice;

        //! raising event: Connected
        protected virtual void OnConnect()
        {
            if (null != ConnectNotice)
            {
                ConnectNotice.Invoke();
            }

            WriteLogLine("Adapter connect.");
        }

        //! ESnail.Device message proccess routine
        protected override void DeviceMessageProcess(System.Windows.Forms.Message m)
        {
            Monitor.Enter(m_Signal);
            if (null != m_Device)
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    //! check if my device

                    switch (m.WParam.ToInt32())
                    {
                        case DBT_DEVICEARRIVAL:
                            //! device connected

                            try
                            {
                                if (m_Device.IsMyDevice(m.LParam))
                                {
                                    Monitor.Exit(m_Signal);
                                    OnConnect();
                                    break;
                                }
                            }
                            catch (Exception) { }
                            Monitor.Exit(m_Signal);
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:

                            try
                            {
                                //! device removed
                                if (m_Device.IsMyDevice(m.LParam))
                                {
                                    Monitor.Exit(m_Signal);
                                    //! raising disconnect event
                                    OnDisconnect();
                                    break;
                                }
                            }
                            catch (Exception) { }
                            Monitor.Exit(m_Signal);
                            break;
                        default :
                            Monitor.Exit(m_Signal);
                            break;
                    }
                    
                }
                else
                {
                    Monitor.Exit(m_Signal);
                }
            }
            else
            {
                Monitor.Exit(m_Signal);
            }
        }

        

        

        //! register device message proccessor
        public override void RegisterDeviceMessageHandler(ref WindowsMessageHandler tHandler, IntPtr hwndParent)
        {
            if ((null == hwndParent) || (null == tHandler) || (null == m_Device))
            {
                //! illegal parameter
                return;
            }

            //! register only once
            if (null != m_WindowsMessageHandler)
            {
                UnregisterDeviceMessageHandler();
            }
            
            
            m_WindowsMessageHandler = tHandler;
            m_WindowsMessageHandler.WindowsMessageArrived += new WindowsMessageProcessor(this.DeviceMessageProcess);               

            m_Device.RegisterDeviceNotification(hwndParent);
            
        }


        //! unregister device message handler
        protected override void UnregisterDeviceMessageHandler()
        {
            if (null != m_WindowsMessageHandler)
            {
                m_WindowsMessageHandler.WindowsMessageArrived -= new WindowsMessageProcessor(this.DeviceMessageProcess);
                //m_WindowsMessageHandler = null;
                /*
                if (m_Device != null)
                {
                    m_Device.UnregisterDeviceNotification();
                }
                */
            }
        }
        
        protected override void _Dispose()
        {
            //! unregister device message handler
            UnregisterDeviceMessageHandler();

            //! raising disconnect event
            OnDisconnect();
        }

        public Boolean WriteDevice(System.Byte[] DataBuffer)
        {
            return WriteDevice(DataBuffer, "");
        }

        public virtual Boolean WriteDevice(System.Byte[] DataBuffer, String tDescription)
        {
            Boolean tResult = false;
            lock (m_Signal)
            {
                tResult = m_Device.WriteDevice(DataBuffer);
            }

            if (tResult)
            {
                this.OnCommunication(MSG_DIRECTION.OUTPUT_MSG, DataBuffer, tDescription);
            }

            return tResult;
        }

        internal Boolean WriteDeviceNoDebug(Byte[] tBuffer)
        {
            lock (m_Signal)
            {
                return m_Device.WriteDevice(tBuffer);
            }
        }

        public Boolean ReadDevice(ref Byte[] tBuffer)
        {
            return ReadDevice(ref tBuffer, "");
        }

        public virtual Boolean ReadDevice(ref System.Byte[] tBuffer, String tDescription)
        {
            Boolean tResult = false;
            lock (m_Signal)
            {
                tResult = m_Device.ReadDevice(ref tBuffer);
            }

            if (tResult)
            {
                this.OnCommunication(MSG_DIRECTION.INPUT_MSG, tBuffer, tDescription);
            }

            return tResult;
        }

        internal Boolean ReadDeviceNoDebug(ref Byte[] tBuffer)
        {
            lock (m_Signal)
            {
                return m_Device.ReadDevice(ref tBuffer);
            }
        }


        public override Boolean RegisterSupportTelegraph(Telegraph tTelegraph)
        {
            if (!(tTelegraph is SinglePhaseTelegraph))
            {
                return false;
            }

            foreach (Telegraph tTelegraphItem in m_SupportTelegraphList)
            {
                if (tTelegraphItem.Type == tTelegraph.Type)
                {
                    return false;
                }
            }

            m_SupportTelegraphList.Add(tTelegraph);

            return true;
        }

        
    }
    
    
    
}