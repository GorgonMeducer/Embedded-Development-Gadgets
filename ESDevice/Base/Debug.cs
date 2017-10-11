using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ESnail.Utilities.Threading;

namespace ESnail.Device
{
    public enum MSG_DIRECTION
    {
        INPUT_MSG,
        OUTPUT_MSG
    }

    public delegate void MessageListener(MSG_DIRECTION Direction,Byte[] Data, System.String strDescription);

    
    public interface IDebugListener
    {
        event MessageListener MessageHooker;

        Boolean DebugEnabled
        {
            get;
            set;
        }
    }

    public abstract class Debug : IDebugListener
    {
        public event MessageListener MessageHooker;

        private Boolean m_DebugEnabled = false;
        private SafeInvoker m_Invoker = new SafeInvoker();

        //! property for debug enable
        public virtual Boolean DebugEnabled
        {
            get { return m_DebugEnabled; }
            set { m_DebugEnabled = value; }
        }

        //! rasing event
        internal virtual void OnCommunication(MSG_DIRECTION Direction, Byte[] Data, System.String strDescription)
        {
            if (m_DebugEnabled)
            {
                if (null != MessageHooker)
                {
                    m_Invoker.BeginInvoke(MessageHooker, Direction, Data, strDescription);
                }
            }
        }


        protected void BeginInvoke(Delegate Method, params Object[] Args)
        {
            if (null != Method)
            {
                m_Invoker.BeginInvoke(Method, Args);
            }
        }
    }
}
