using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ESnail.Utilities;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Forms;
using ESnail.Utilities.Log;

namespace ESnail.Utilities.Threading
{

    //! \name pipelline state
    //! @{
    public enum PIPELINE_STATE
    { 
        PIPELINE_START,
        PIPELINE_STOPPED
    }
    //! @}
    
    // Summary:
    //     Describes the priorities at which operations can be invoked by way of the
    //     System.Windows.Threading.Dispatcher.
    public enum DispatcherPrioritys
    {
        // Summary:
        //     The enumeration value is -1. This is an invalid priority.
        Invalid = -1,
        //
        // Summary:
        //     The enumeration value is 0. Operations are not processed.
        Inactive = 0,
        //
        // Summary:
        //     The enumeration value is 1. Operations are processed when the system is idle.
        SystemIdle = 1,
        //
        // Summary:
        //     The enumeration value is 2. Operations are processed when the application
        //     is idle.
        ApplicationIdle = 2,
        //
        // Summary:
        //     The enumeration value is 3. Operations are processed after background operations
        //     have completed.
        ContextIdle = 3,
        //
        // Summary:
        //     The enumeration value is 4. Operations are processed after all other non-idle
        //     operations are completed.
        Background = 4,
        //
        // Summary:
        //     The enumeration value is 5. Operations are processed at the same priority
        //     as input.
        Input = 5,
        //
        // Summary:
        //     The enumeration value is 6. Operations are processed when layout and render
        //     has finished but just before items at input priority are serviced. Specifically
        //     this is used when raising the Loaded event.
        Loaded = 6,
        //
        // Summary:
        //     The enumeration value is 7. Operations processed at the same priority as
        //     rendering.
        Render = 7,
        //
        // Summary:
        //     The enumeration value is 8. Operations are processed at the same priority
        //     as data binding.
        DataBind = 8,
        //
        // Summary:
        //     The enumeration value is 9. Operations are processed at normal priority.
        //     This is the typical application priority.
        Normal = 9,
        //
        // Summary:
        //     The enumeration value is 10. Operations are processed before other asynchronous
        //     operations. This is the highest priority.
        Send = 10,
    }
    
    public class DispatcherContainer : DispatcherObject
    {
        
    }


    public class SafeInvoker
    {
        private DispatcherPriority m_Priority = DispatcherPriority.Normal;
        static private DispatcherContainer m_Dispatcher = new DispatcherContainer();

        public DispatcherPrioritys Priority
        {
            get 
            {
                return (DispatcherPrioritys)m_Priority;
            }
            set
            {
                m_Priority = ((DispatcherPriority)value);
            }
        }

        public Boolean BeginInvoke(Delegate Method, params Object[] Args)
        {
            
            if (null == Method)
            {
                return false;
            }

            if (null != Args)
            {
                if (Args.Length > 0)
                {
                    Object tObj = Args[0];
                    Object[] tTargetArgs = new Object[Args.Length - 1];
                    Array.Copy(Args, 1, tTargetArgs, 0, Args.Length - 1);

                    m_Dispatcher.Dispatcher.BeginInvoke(m_Priority, Method, tObj, tTargetArgs);
                }
                else
                {
                    m_Dispatcher.Dispatcher.BeginInvoke(m_Priority, Method, null);
                }
            }
            else
            {
                m_Dispatcher.Dispatcher.BeginInvoke(m_Priority, Method);
            }

            return true;
        }
    }


    //! \name Command set pipe line
    //! @{
    public  abstract partial class PipelineCore : ESDisposableClass
    {
        //! \name field
        //! @{
        //private Boolean m_bDisposed = false;                    //!< disposing flag
        private SafeID m_ID = null;                             //!< ID
        private Thread m_PipelineCoreThread = null;             //!< main thread
        private Boolean m_RequestStop = false;                  //!< request stop flag
        private ThreadPriority m_ThreadPriority = ThreadPriority.BelowNormal;
        private Int32 m_wPipelineTimeoutCounter = 0;            //!< timeout counter
        private System.Timers.Timer m_TimeOutTimer = null;
        private SafeInvoker m_Invoker = new SafeInvoker();
        private Queue<PipelineCoreService> m_ServicesQueue = new Queue<PipelineCoreService>();
        private AutoResetEvent m_WaitService = new AutoResetEvent(false);

        //! @}

        //! \brief default constructor
        public PipelineCore()
        {
            Initialize();            
        }

        

        public delegate void PipelineCoreDiposing(PipelineCore Pipeline);
        public delegate void PipelineCoreStateReport(PIPELINE_STATE State, PipelineCore PipelineCore);

        //! \brief initialize this pipeline
        private void Initialize()
        {
            m_TimeOutTimer = new System.Timers.Timer();
            m_TimeOutTimer.Stop();
            m_TimeOutTimer.AutoReset = true;
            m_TimeOutTimer.Interval = 100;
            m_TimeOutTimer.Elapsed += new ElapsedEventHandler(PipelineTimeoutTimer);

        }

        //! \brief timer
        private void PipelineTimeoutTimer(object sender, ElapsedEventArgs e)
        {
            if (m_wPipelineTimeoutCounter > 0)
            {
                m_wPipelineTimeoutCounter--;
                if (0 == m_wPipelineTimeoutCounter)
                {
                    //! service no response for quite a long time
                    m_TimeOutTimer.Stop();

                    try
                    {
                        //! time out
                        m_PipelineCoreThread.Abort();
                    }
                    catch (Exception) { }

                    //! drop service
                    lock (((ICollection)m_ServicesQueue).SyncRoot)
                    {
                        if (m_ServicesQueue.Count > 0)
                        {
                            PipelineCoreService ServiceItem = m_ServicesQueue.Dequeue();
                            if (null != ServiceItem)
                            {
                                //! raising event
                                ServiceItem.OnServiceCancelled();
                            }
                        }
                    }

                    try
                    {
                        m_PipelineCoreThread.Join();

                    }
                    catch (Exception) { }
                    finally
                    {
                        m_PipelineCoreThread = null;
                    }
                    if (m_RequestStop)
                    {
                        m_RequestStop = false;

                        //! raising event
                        OnPipelineCoreStateReport(PIPELINE_STATE.PIPELINE_STOPPED);
                    }
                    else
                    {
                        //! restart pipeline
                        PipelineOpen = true;
                    }
                    
                }
            }
        }

#if false
        //! \brief property for getting disposing state
        public System.Boolean Disposed
        {
            get { return m_bDisposed; }
        }
#endif

        public event PipelineCoreStateReport PipelineCoreStateReportEvent;

        //! \brief method for raising pipeline report event
        protected void OnPipelineCoreStateReport(PIPELINE_STATE State)
        {
            if (null != PipelineCoreStateReportEvent)
            {
                PipelineCoreStateReportEvent.Invoke(State, this);
                //PipelineReportEvent.Invoke(State,this);
                //BeginInvoke(PipelineCoreStateReportEvent, State, this);
            }
        }

        public void BeginInvoke(Delegate Method, params Object[] Args)
        {
            if (null != Method)
            {
                try
                {
                    m_Invoker.BeginInvoke(Method, Args);
                }
                catch (Exception) { }
            }
        }

        public DispatcherPrioritys InvokePriority
        {
            get { return m_Invoker.Priority; }
            set { m_Invoker.Priority = value; }
        }

        public event PipelineCoreDiposing PipelineDiposingEvent;

        //! \brief method for raising pipeline disposed event
        private void OnPipelineDiposing()
        {
            if (null != PipelineDiposingEvent)
            {
                PipelineDiposingEvent.Invoke(this);
                //m_Invoker.BeginInvoke(PipelineDiposingEvent, this);
            }
        }

        //! \brief property for getting/setting pipeline thread priority
        public ThreadPriority Priority
        {
            get { return m_ThreadPriority; }
            set
            {
                if (PipelineOpen)
                {
                    return;
                }
                
                m_ThreadPriority = value;
            }
        }

        //! \brief pipeline background task
        private void PipelLineCoreBackgroundTask()
        {
            if (null == m_PipelineCoreThread)
            {
                return;
            }
            Boolean tWaitService = false;
            //LogWriter.Write("V");
            do
            {
                PipelineCoreService ServiceItem = null;

                if (tWaitService)
                {
                    m_WaitService.WaitOne();
                    tWaitService = false;
                }
                
                //! get queue item
                lock (((ICollection)m_ServicesQueue).SyncRoot)
                {
                    if (0 == m_ServicesQueue.Count)
                    {
                        if (AutoStart)
                        {
                            break;
                        }
                        else
                        { 
                            tWaitService = true;
                            //Thread.Sleep(20);
                            continue;
                        }
                    }

                    //! get a new service item
                    ServiceItem = m_ServicesQueue.Peek();
                    if (null == ServiceItem)
                    {
                        m_ServicesQueue.Dequeue();
                        continue;
                    }
                    if (!ServiceItem.Available)
                    {
                        m_ServicesQueue.Dequeue();
                        continue;
                    }

                    //! raising event
                    ServiceItem.OnLine();
                }


                if (ServiceItem.Timeout > 0)
                {
                    m_wPipelineTimeoutCounter = ServiceItem.Timeout;
                    m_TimeOutTimer.Start();

                    do
                    {
                        if (!ServiceItem.DoService())
                        {
                            try
                            {
                                m_TimeOutTimer.Stop();
                                m_wPipelineTimeoutCounter = ServiceItem.Timeout;

                                if (ServiceItem.Cancelled)
                                {
                                    //! raising event
                                    ServiceItem.OnServiceCancelled();
                                }
                                else
                                {
                                    //! raising event
                                    ServiceItem.OnServiceComplete();

                                }
                                break;
                            }
                            catch (Exception )
                            {
                                break;
                            }
                        }

                        //! after stop request flag set to true, you have only m_wPipelineTimeout * 100ms to die...
                        if ((!m_RequestStop) && (!ServiceItem.Cancelled))
                        {
                            m_wPipelineTimeoutCounter = ServiceItem.Timeout;
                        }
                        
                        //Thread.Sleep(1);
                    }
                    while (true);

                    lock (((ICollection)m_ServicesQueue).SyncRoot)
                    {
                        try
                        {
                            m_ServicesQueue.Dequeue();
                        }
                        catch (Exception) { }
                    }

                }
                else
                {
                    do
                    {
                        if (!ServiceItem.DoService())
                        {
                            //! raising event
                            if (ServiceItem.Cancelled)
                            {
                                //! raising event
                                ServiceItem.OnServiceCancelled();
                            }
                            else
                            {
                                //! raising event
                                ServiceItem.OnServiceComplete();
                            }
                            break;
                        }

                        if ((m_RequestStop) || (ServiceItem.Cancelled))
                        {
                            //! after stop request flag set to true, it's your last chance...
                            ServiceItem.DoService();

                            //! raising event
                            ServiceItem.OnServiceCancelled();
                            break;
                        }
                    }
                    while (true);

                    lock (((ICollection)m_ServicesQueue).SyncRoot)
                    {
                        if (m_ServicesQueue.Count > 0)
                        {
                            m_ServicesQueue.Dequeue();
                        }
                    }
                }

                m_wPipelineTimeoutCounter = 0;

                if (m_RequestStop)
                {
                    m_RequestStop = false;
                    break;
                }
            }
            while (true);

            //! raising event
            OnPipelineCoreStateReport(PIPELINE_STATE.PIPELINE_STOPPED);

            m_PipelineCoreThread = null;
        }


        //! \brief property for openning/closing pipeline
        public System.Boolean PipelineOpen
        {
            get 
            {
                if (!Available)
                {
                    return false;
                }

                if (null == m_PipelineCoreThread)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            set 
            {
                if (!Available)
                {
                    return;
                }

                if (value)
                {
                    try
                    {
                        if (null == m_PipelineCoreThread)
                        {
                            m_PipelineCoreThread = new Thread(PipelLineCoreBackgroundTask);
                            m_PipelineCoreThread.Priority = m_ThreadPriority;
                            m_PipelineCoreThread.IsBackground = true;
                            m_PipelineCoreThread.Name = "PipelineCore";
                            m_PipelineCoreThread.Start();

                            //! raising event
                            OnPipelineCoreStateReport(PIPELINE_STATE.PIPELINE_START);
                        }
                    }
                    catch (Exception) { }
                        /*
                        else if (!m_PipelineCoreThread.IsAlive)
                        {
                            lock (m_PipelineCoreThread)
                            {
                                if (m_PipelineCoreThread.ThreadState != ThreadState.WaitSleepJoin)
                                {
                                    m_PipelineCoreThread = new Thread(PipelLineCoreBackgroundTask);
                                    m_PipelineCoreThread.Priority = m_ThreadPriority;
                                    m_PipelineCoreThread.IsBackground = true;
                        
                                    m_PipelineCoreThread.Start();
                                }
                            }

                            //! raising event
                            OnPipelineCoreStateReport(PIPELINE_STATE.PIPELINE_START);
                        }
                        */

                        m_RequestStop = false;
                    
                }
                else
                {
                    if (null != m_PipelineCoreThread)
                    {
                        m_RequestStop = true;
                    }
                    
                }
            }
        }

        //! \brief property for checking whether this pipeline is available
        public abstract System.Boolean Available
        {
            get;
        }

#if false
        //! \brief distructor
        ~PipelineCore()
        {
            Dispose();
        }
#endif
        

        //! \brief method for disposing this object
        public override void Dispose()
        {
            if (!m_bDisposed)
            {
                m_bDisposed = true;

                try
                {
                    _Dispose();
                }
                catch (Exception) { }


                try
                {
                    if (null != m_PipelineCoreThread)
                    {
                        if (m_PipelineCoreThread.IsAlive)
                        {
                            m_RequestStop = true;
                            if (!m_PipelineCoreThread.Join(2000))
                            {
                                m_PipelineCoreThread.Abort();
                            }
                            //! raising event
                            OnPipelineCoreStateReport(PIPELINE_STATE.PIPELINE_STOPPED);
                        }
                        m_RequestStop = false;
                        m_PipelineCoreThread = null;
                    }
                }
                catch (Exception) { }

                try
                {
                    if (null != m_TimeOutTimer)
                    {
                        m_TimeOutTimer.Stop();
                        m_TimeOutTimer.Elapsed -= new ElapsedEventHandler(PipelineTimeoutTimer);

                        m_TimeOutTimer.Close();
                        m_TimeOutTimer = null;
                    }
                }
                catch (Exception) { }

                //! raising event
                OnPipelineDiposing();
                
                foreach (PipelineCoreService tService in m_ServicesQueue)
                {
                    try
                    {
                        tService.Dispose();
                    }
                    catch (Exception) { }
                }

                GC.SuppressFinalize(this);
            }
        }

        private System.Boolean m_AutoStart = true;

        //! \brief property for getting/setting whether an pipeline start work automaticly
        public System.Boolean AutoStart
        {
            get { return m_AutoStart; }
            set { m_AutoStart = value; }
        }



        //! \brief adding a pipeline core service
        public virtual Boolean AddService(PipelineCoreService ServiceItem)
        {
            if (!Available)
            {
                return false;
            }

            if (null == ServiceItem)
            {
                return false;
            }

            lock (((ICollection)m_ServicesQueue).SyncRoot)
            {
                try
                {
                    //! add service to queue
                    m_ServicesQueue.Enqueue(ServiceItem);
                    m_WaitService.Set();
                }
                catch (System.Exception)
                {
                    //System.Console.WriteLine(e.ToString());
                    return false;
                }
            }


            if (AutoStart)
            {
                //! auto start pipeline
                PipelineOpen = true;
            }

            return true;
        }

        //! \brief adding a pipeline core service
        public virtual System.Boolean AddServices(PipelineCoreService[] ServiceItems)
        {
            if (!Available)
            {
                return false;
            }

            if (null == ServiceItems)
            {
                return false;
            }
            if (0 == ServiceItems.Length)
            {
                return true;
            }

            lock (((ICollection)m_ServicesQueue).SyncRoot)
            {
                foreach (PipelineCoreService tService in ServiceItems)
                {
                    try
                    {
                        //! add service to queue
                        m_ServicesQueue.Enqueue(tService);
                        m_WaitService.Set();
                    }
                    catch (System.Exception)
                    {
                        //System.Console.WriteLine(e.ToString());
                        return false;
                    }
                }
            }

            if (AutoStart)
            {
                //! auto start pipeline
                PipelineOpen = true;
            }

            return true;
        }
    }
    //! @{


    //! \name pipeline core service 
    //! @{
    public abstract class PipelineCoreService : ESDisposableClass
    {
        volatile private System.Boolean m_Working = false;
        private Object m_Target = null;
        private System.Boolean m_Cancelled = false;
        protected System.Boolean m_Available = true;
        private System.Boolean m_Complete = false;
        private ManualResetEvent m_CompleteSignal = new ManualResetEvent(false);
        private ManualResetEvent m_StartSignal = new ManualResetEvent(false);
        private System.Int32 m_SafeTimeOut = 0;
        protected  SafeInvoker m_Invoker = new SafeInvoker();
        private SafeID m_tID = null;

        //! \brief constructor
        public PipelineCoreService(Object TargetItem)
        {
            m_Target = TargetItem;
            if (null == m_Target)
            {
                m_Available = false;
            }
        }

        //! \brief constructor
        public PipelineCoreService(Object TargetItem, System.Int32 SafeTimeOutSetting)
        {
            m_SafeTimeOut = SafeTimeOutSetting;
            m_Target = TargetItem;
            if (null == m_Target)
            {
                m_Available = false;
            }
        }

        public Int32 Timeout
        {
            get { return m_SafeTimeOut; }
        }

        public abstract Boolean DoService();

        

        //! \brief raising service complete event
        internal void OnServiceComplete()
        {
            m_Complete = true;
            m_Working = false;
            
            OnInternalPipelineCoreServiceComplete();

            m_CompleteSignal.Set();
        }

        public delegate void PipelineCoreServiceComplete(PipelineCoreService Item);
        public delegate void PipelineServiceCancelled(PipelineCoreService Item);

        public event PipelineCoreServiceComplete PipelineCoreServiceCompleteEvent;
        public event PipelineServiceCancelled PipelineServiceCancelledEvent;

        protected virtual void OnInternalPipelineCoreServiceComplete()
        {

            if (null != PipelineCoreServiceCompleteEvent)
            {
                m_Invoker.BeginInvoke(PipelineCoreServiceCompleteEvent, this);
            }
        }

        internal void OnServiceCancelled()
        {
            OnPipelineServiceCancelled();
            m_CompleteSignal.Set();
        }

        protected internal virtual void OnPipelineServiceCancelled()
        {
            if (null != PipelineServiceCancelledEvent)
            {
                m_Invoker.BeginInvoke(PipelineServiceCancelledEvent,this);
            }
        }

        //! \brief property for mark this object online
        internal void OnLine()
        {
            
            m_Complete = false;
            m_Working = true;
            m_StartSignal.Set();
        }

        //! \brief available state
        public Boolean Available
        {
            get { return m_Available; }
        }
        
        
        //! \brief property for check whether work completed
        public System.Boolean Complete
        {
            get 
            {
                return m_Complete; 
            }
        }
        
        public ManualResetEvent CompleteSignal
        {
            get { return m_CompleteSignal; }
        }

        public ManualResetEvent StartSignal
        {
            get { return m_StartSignal; }
        }

        //! \brief property for getting target
        public Object Tag
        {
            get { return m_Target; }
        }

        //! \brief property for checking whether it's this item's turn or not
        public Boolean Working
        {
            get { return m_Working; }
        }

        //! \brief property for checking whether this item was requested to be cancelled
        public Boolean Cancelled
        {
            get { return m_Cancelled; }
        }

        //! \brief cancel this work
        public void Cancel()
        {
            //m_StartSignal.Reset();
            //m_CompleteSignal.Set();
            m_Cancelled = true;
        }

#if false
        ~PipelineCoreService()
        {
            Dispose();
        }
        
#region IDisposable Members

        private System.Boolean m_bDisposed = false;

        public System.Boolean Disposed
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
                    //! dipose managed objects
                    _Dispose();
                }
                catch (Exception) { }

                GC.SuppressFinalize(this);
            }
        }

        protected abstract void _Dispose();

#endregion
#endif
    }
    //! @}

    public abstract class TPipelineCoreService<TObject> : PipelineCoreService
    {
        //! \brief constructor
        public TPipelineCoreService(TObject Item)
            : base(Item)
        {
            
        }

        //! \brief constructor
        public TPipelineCoreService(TObject Item, Int32 SafeTimeOutSetting)
            : base(Item, SafeTimeOutSetting)
        { 
        
        }


        //! \brief get target
        public TObject Target
        {
            get { return (TObject)base.Tag; }
        }

    }

}
