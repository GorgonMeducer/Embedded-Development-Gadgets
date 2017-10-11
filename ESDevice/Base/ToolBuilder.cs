using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Device
{
    //! \name toolbuilder refresh result
    //! @{
    public enum BM_TOOLS_REFRESH_RESULT     : ushort
    {
        BM_TRR_UNCHANGE,                        //!< nothing changed
        BM_TRR_FIND_NEW_TOOLS       = 0x0001,   //!< find new tools
        BM_TRR_TOOLS_REMOVED        = 0x0002,   //!< some tools were removed
        BM_TRR_TOOLS_LOST           = 0x0004,   //!< some tools were lost
        BM_TRR_TOOLS_AVAILABLE      = 0x0008,   //!< some tools became available
        BM_TRR_TOOLS_UNAVAILABLE    = 0x0010,   //!< some tools became unvaiable
        BM_TRR_TOOLS_CONNECTED      = 0x0020,   //!< some tools connected to devices
        BM_TRR_TOOLS_DISCONNECTED   = 0x0040,   //!< some tools disconnected from devices
        BM_TRR_TOOLS_BUSY           = 0x0080,   //!< some tools became busy
        BM_TRR_TOOLS_PENDING        = 0x0100,   //!< some tools became pending / has no response for quite a while
        BM_TRR_TOOLS_PAUSE_OR_SLEEP = 0x0200    //!< some tools have been paused or sleep
    }
    //! @}

    public delegate void RefreshComplete(BM_TOOLS_REFRESH_RESULT Result);

    //! \name tool builder
    //! \brief a tool builder is used for building a specified series of devices
    //! @{
    public abstract class ToolBuilder
    {
        public event RefreshComplete RefreshCompleteEvent;

        protected System.Boolean m_AutoScan = false;

        //! property : tool name
        public abstract System.String ToolName
        {
            get;
            set;
        }

        //! property: auto scan 
        public virtual System.Boolean AutoScanEnabled
        {
            get { return m_AutoScan; }
            set { m_AutoScan = value; }
        }

        //! a method for refresh all tools in a list and try to find new tools
        public abstract System.Boolean RefreshTools();

        //! raising refresh complete event
        protected virtual void OnRefreshComplete(BM_TOOLS_REFRESH_RESULT Result)
        {
            RefreshCompleteEvent.Invoke(Result);
        }

        //! raising  refresh complete event asynchronously
        protected virtual void OnRefreshCompleteAsyn(BM_TOOLS_REFRESH_RESULT Result)
        {
            IAsyncResult res = RefreshCompleteEvent.BeginInvoke(Result,null,null);
            RefreshCompleteEvent.EndInvoke(res);
        }

    }
    // @}
}
