using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;

namespace ESnail.Device
{
    public enum TELEGRAPH_ENGINE_STATE
    {
        ENGINE_START,
        ENGINE_STOPPING,
        ENGINE_STOPED,
        ENGINE_WORKING,
        ENGINE_DISPOSED
    }

    public delegate void EngineStateReport(TELEGRAPH_ENGINE_STATE State, TelegraphEngine EngineItem);

    public abstract class TelegraphEngine : IDisposable, ITelegraph
    {
        private System.Boolean m_Disposed = false;
        protected System.Threading.Thread m_threadCommunication = null;
        protected ThreadPriority m_Priority = ThreadPriority.Normal;
        protected ManualResetEvent m_StopRequest = new ManualResetEvent(false);

        //! distructor
        ~TelegraphEngine()
        {
            Dispose();
        }

        //! set thread priority
        public virtual ThreadPriority Priority
        {
            get { return m_Priority; }
            set { m_Priority = value; }
        }

        public event EngineStateReport EngineStateReportEvent;

        protected void OnEngineStateReport(TELEGRAPH_ENGINE_STATE State)
        {
            if (null != EngineStateReportEvent)
            {
                EngineStateReportEvent.Invoke(State,this);
            }
        }

        //! propery to set working state
        public virtual System.Boolean IsWorking
        {
            get
            {
                if (null == m_threadCommunication)
                {
                    return false;
                }
                
                return m_threadCommunication.IsAlive;
            }
            set 
            {
                if (value)
                {
                    m_StopRequest.Reset();
                    //! request start working
                    if (null == m_threadCommunication)                    
                    {                        
                        //! initialize communication thread
                        m_threadCommunication = new Thread(this.DoCommunication);
                        m_threadCommunication.Priority = m_Priority;
                        m_threadCommunication.IsBackground = true;
                        m_threadCommunication.Start();                                  //!< start communication thread
                        m_threadCommunication.Name = "Telegraph [" + DateTime.Now.ToLongDateString() + "]";
                        
                        OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_START);
                    }
                    else if (!m_threadCommunication.IsAlive)
                    {
                        //! initialize communication thread
                        m_threadCommunication = new Thread(this.DoCommunication);
                        m_threadCommunication.Priority = m_Priority;
                        m_threadCommunication.IsBackground = true;
                        m_threadCommunication.Name = "Telegraph [" + DateTime.Now.ToLongDateString() + "]";
                        m_threadCommunication.Start();                                  //!< start communication thread
                        OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_START);
                    }
                }
                else
                { 
                    //! request stop working
                    if (null != m_threadCommunication)
                    {
                        if (m_threadCommunication.IsAlive)
                        {
                            m_StopRequest.Set();
                            OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_STOPPING);
                        }
                    }
                }

                
            }
        }


        //! this method should be override
        protected virtual void DoCommunication()
        {
            m_StopRequest.WaitOne();
        }

        public virtual void Dispose()
        {
            //! request the communication thread stop
            m_StopRequest.Set();
            //m_RequestStop = true;
            //! wait for communication thread complete
            if (!m_Disposed)
            {
                m_Disposed = true;
                try
                {
                    if (null != m_threadCommunication)
                    {
                        if (m_threadCommunication.IsAlive)
                        {
                            switch (m_threadCommunication.ThreadState)
                            { 
                                case ThreadState.Aborted:                                
                                case ThreadState.Stopped:                                
                                case ThreadState.Suspended:                                
                                case ThreadState.Unstarted:
                                case ThreadState.Background:
                                    m_threadCommunication.Join();
                                    break;
                                case ThreadState.SuspendRequested:
                                case ThreadState.AbortRequested:                                
                                case ThreadState.Running:
                                case ThreadState.StopRequested:
                                case ThreadState.WaitSleepJoin:
                                    m_threadCommunication.Join();
                                    OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_STOPED);
                                    break;
                            }                            
                        }                        
                    }
                }
                catch (ThreadStateException e)
                {
                    System.Console.WriteLine(e.ToString());
                }
                finally
                {
                    //m_RequestStop = false;
                    GC.SuppressFinalize(this);
                }
                OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_DISPOSED);
            }
        }

        //! \brief property check whether this object was disposed
        public System.Boolean Disposed
        {
            get { return m_Disposed; }
        }


        public virtual System.Boolean TryToSendTelegraph(Telegraph telTarget)
        {
            return false;
        }


        public virtual System.Boolean TryToSendTelegraphs(Telegraph[] telTagets)
        {
            return false;
        }


        public virtual System.String Type
        {
            get { return "Telegraph Engine"; }
        }

        public abstract Adapter ParentAdapter
        {
            get;
            set;
        }


        
    }
}
