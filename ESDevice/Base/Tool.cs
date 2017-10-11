using System;
using System.Collections.Generic;
using System.Text;

namespace ESnail.Device
{
    //! \name tool state
    //! @{
    public enum BM_TOOL_STATE : ushort
    { 
        BM_TS_RAW,                                          //!< uninitialized tool
        BM_TS_INITIALIZING,                                 //!< under initializing
        BM_TS_REMOVED,                                      //!< marked as a removed device
        BM_TS_AVAILABLE,                                    /*!< ready to use 
                                                             * (a device may have a state other than connect/disconnect) */
        BM_TS_UNAVAILABLE,                                  //!< unavailble
        BM_TS_CONNECTED,                                    //!< connect to the device
        BM_TS_DISCONNECTED,                                 //!< disconnected from the device
        BM_TS_PENDING,                                      //!< pending / having no response for a while
        BM_TS_SLEEPING_OR_PAUSED,                           //!< enter sleep model or pausing model
        BM_TS_WORKING,                                      //!< tool is working
        BM_TS_BUSY,                                         //!< tool is busy
        BM_TS_LOST,                                         //!< lost
        BM_TS_INVALID                                       //!< invalid tool
    }
    //! @}


    //! \name Tool
    //! @{
    public abstract class Tool
    {
        protected String m_strToolName = "Unknown";            //!< a tool name
        protected BM_TOOL_STATE m_State = BM_TOOL_STATE.BM_TS_RAW;

        //! property: tool name
        public virtual String Name
        {
            get { return m_strToolName; }
            set
            {
                if ((null != value) && ("" != value.Trim()))
                {
                    //! modify tool name
                    m_strToolName = value;
                }
            }
        }

        //! property: tool type
        public abstract String Type
        {
            get;
        }

        //! property: tool state
        public virtual BM_TOOL_STATE State
        {
            get { return m_State; }
        }

        //! property: adapter
        public abstract Adapter Adapter
        {
            get;
        }

        //! property: IsBusy
        public abstract Boolean IsBusy
        {
            get;
        }

        //! property: IsWorking
        public abstract Boolean IsWorking
        {
            get;
        }

        //! remove this tool
        public abstract Boolean Remove();
        
    }
    //! @}
}
