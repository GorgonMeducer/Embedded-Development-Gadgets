using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;
using ESnail.Device.Telegraphs;
using ESnail.CommunicationSet.Commands;

namespace ESnail.Device.Adapters.USB.HID
{
    //! \name usb hid tools
    //! @{
    public abstract class USBHIDTools : Tool
    {
        private TelegraphHIDAdapter m_Adapter = null;

        //! constructor
        public USBHIDTools(TelegraphHIDAdapter Adapter)
        {
            m_Adapter = Adapter;

            m_Adapter.ConnectNotice += new DeviceConnected(AdapterConnectNotice);
            m_Adapter.DisconnectNotice += new DeviceDisconnected(AdapterDisconnectNotice);

            m_State = BM_TOOL_STATE.BM_TS_CONNECTED;
        }

        //! \brief disconnection event receiver
        void AdapterDisconnectNotice()
        {
            m_State = BM_TOOL_STATE.BM_TS_DISCONNECTED;
        }

        //! \brief connection event receiver
        private void AdapterConnectNotice()
        {
            m_State = BM_TOOL_STATE.BM_TS_CONNECTED;
        }
        
        //! \brief remove a tool
        public override Boolean Remove()
        {
            if (null != m_Adapter)
            {
                m_Adapter.Dispose();
                m_Adapter = null;
            }

            m_State = BM_TOOL_STATE.BM_TS_REMOVED;

            return true;
        }

        //! \brief property for checking whether system is busy
        public override Boolean IsBusy
        {
            get 
            {
                if (null == m_Adapter)
                {
                    return false;
                }

                return m_Adapter.IsBusy;
            }
        }

        //! \brief property for checking whether system is working
        public override Boolean IsWorking
        {
            get
            {
                if (null == m_Adapter)
                {
                    return false;
                }

                return m_Adapter.IsWorking;
            }
        }

        //! \brief property for get adatper
        public override Adapter Adapter
        {
            get { return m_Adapter; }
        }

        //! \brief method for creating a SinglePhaseTelegrah object
        public abstract SinglePhaseTelegraph CreateSinglePhaseTelegraph(ESCommand Command);

    }
    //! @}

#if false
    //! \name SB200
    //! @{
    public class ToolsSB200 : USBHIDTools
    {
        //! \brief constructor
        public ToolsSB200(TelegraphHIDAdapter Adapter)
            : base(Adapter)
        { 
        
        }

        //! \brief implement method for get a SmartBatteryTelegraph
        public override SinglePhaseTelegraph CreateSinglePhaseTelegraph(ESCommand Command)
        {
            SmartBatteryTelegraph teleSB200 = new SmartBatteryTelegraph(Command);

            return teleSB200;
        }

        //! \brief property : tool type
        public override string Type
        {
            get { return "GSPHIDCompliantTools"; }
        }

        
    }
    //! @}

    //! \name SB200
    //! @{
    public class ToolsBM300 : USBHIDTools
    {
        //! \brief constructor
        public ToolsBM300(TelegraphHIDAdapter Adapter)
            : base(Adapter)
        {

        }

        //! \brief implement method for get a SmartBatteryTelegraph
        public override SinglePhaseTelegraph CreateSinglePhaseTelegraph(ESCommand Command)
        {
            XBatteryTelegraph teleSB200 = new XBatteryTelegraph(Command);

            return teleSB200;
        }

        //! \brief property : tool type
        public override string Type
        {
            get { return "BM300"; }
        }
    }
    //! @}
#endif
}
