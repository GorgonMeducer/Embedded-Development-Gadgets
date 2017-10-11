using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ESnail.Utilities.Log;
namespace ESnail.Utilities.Threading
{
    //! \name dynamic multiple-stage pipeline
    //! @{
    public abstract class Pipeline : PipelineCore
    {
        private Queue<PipelineService> m_WaitingServiceQueue = new Queue<PipelineService>();
        private Thread m_PipelineThread = null;
        private Boolean m_RequestStop = false;
        private List<Queue<PipelineService>> m_DynamicStageList = new List<Queue<PipelineService>>();
        private Boolean m_StageServiceCancelled = false;

        //! \brief default constructor
        public Pipeline() 
            : base()
        { 
        
        }

        //! \brief pipeline background task
        private void PipelineBackgroundTask()
        {
            PipelineService ServiceItem = null;
            Queue<PipelineService> queueFirstStageQueue = null;

            //! \name finit state machine A
            //! @{
            System.Boolean FSM_GET_SERVICE_FROM_WAITING_QUEUE = true;
            System.Boolean FSM_INITIALIZE_NEW_SERVICE = false;
            //! @}

            System.Int32 nCurrentStageIndex = 0;
            //! \name finit state machine B
            //! @{
            Boolean FSM_CHECK_CURRENT_STAGE_QUEUE = true;
            Boolean FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE = false;
            Boolean FSM_WAIT_SERVICE_RESULT = false;
            //! @}

            System.Object tObject = new object();

            lock (tObject)
            {
                //! small super loop
                do
                {
                    //! try to peek a available service
                    if (FSM_GET_SERVICE_FROM_WAITING_QUEUE)
                    {
                        lock (((ICollection)m_WaitingServiceQueue).SyncRoot)
                        {
                            if (m_WaitingServiceQueue.Count != 0)
                            {
                                try
                                {
                                    //! peek a new service
                                    ServiceItem = m_WaitingServiceQueue.Peek();
                                }
                                catch (Exception) { }

                                if (null == ServiceItem)
                                {
                                    //! drop this service
                                    m_WaitingServiceQueue.Dequeue();
                                }
                                else if (!ServiceItem.Available)
                                {
                                    //! drop this service
                                    m_WaitingServiceQueue.Dequeue();
                                }
                                else if (ServiceItem.Stage > 0)
                                {
                                    //! drop this service
                                    m_WaitingServiceQueue.Dequeue();

                                    //! raising event
                                    ServiceItem.OnPipelineServiceException();
                                }
                                else
                                {
                                    FSM_GET_SERVICE_FROM_WAITING_QUEUE = false;
                                    FSM_INITIALIZE_NEW_SERVICE = true;
                                }
                            }
                        }
                    }
                    //! initialize this service item and add it to first stage queue
                    if (FSM_INITIALIZE_NEW_SERVICE)
                    {
                        if (0 == m_DynamicStageList.Count)
                        {
                            queueFirstStageQueue = new Queue<PipelineService>();
                            //! create first stage
                            m_DynamicStageList.Add(queueFirstStageQueue);
                        }

                        //! add this service to first stage
                        queueFirstStageQueue.Enqueue(ServiceItem);

                        lock (((ICollection)m_WaitingServiceQueue).SyncRoot)
                        {
                            //! drop this service
                            m_WaitingServiceQueue.Dequeue();
                        }

                        //! reset this stage machine
                        FSM_INITIALIZE_NEW_SERVICE = false;
                        FSM_GET_SERVICE_FROM_WAITING_QUEUE = true;
                    }

                    //! check current stage queue
                    if (FSM_CHECK_CURRENT_STAGE_QUEUE)
                    {
                        if (m_RequestStop)
                        {
                            //! request stop
                            m_RequestStop = false;
                            break;
                        }

                        if (0 != m_DynamicStageList.Count)
                        {
                            if (m_DynamicStageList.Count > nCurrentStageIndex)
                            {
                                if (m_DynamicStageList[nCurrentStageIndex].Count > 0)
                                {
                                    //! current stage queue is not empty
                                    FSM_CHECK_CURRENT_STAGE_QUEUE = false;
                                    FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE = true;
                                }
                                else
                                {
                                    //! current stage queue is empty
                                    if (m_DynamicStageList.Count == nCurrentStageIndex + 1)
                                    {
                                        //! current stage is the top stage, release it
                                        m_DynamicStageList.Remove(m_DynamicStageList[nCurrentStageIndex]);
                                    }
                                    if (nCurrentStageIndex > 0)
                                    {
                                        nCurrentStageIndex--;
                                    }
                                    else if (m_DynamicStageList.Count > 0)
                                    {
                                        nCurrentStageIndex = m_DynamicStageList.Count - 1;
                                    }
                                }
                            }
                            else if (0 != nCurrentStageIndex)
                            {
                                nCurrentStageIndex--;
                            }
                        }
                    }
                    //! peek service item from current stage queue
                    if (FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE)
                    {
                        PipelineService tService = m_DynamicStageList[nCurrentStageIndex].Peek();
                        if (null == tService)
                        {
                            //! illegal service
                            m_DynamicStageList[nCurrentStageIndex].Dequeue();

                            //! try to get next service
                            FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE = false;
                            FSM_CHECK_CURRENT_STAGE_QUEUE = true;
                        }
                        else if (!tService.Available)
                        {
                            //! illegal service
                            m_DynamicStageList[nCurrentStageIndex].Dequeue();

                            //! try to get next service
                            FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE = false;
                            FSM_CHECK_CURRENT_STAGE_QUEUE = true;
                        }
                        else
                        {
                            m_StageServiceCancelled = false;
                            //tService.InitializeCurrentStage();
                            tService.PipelineServiceCancelledEvent += new PipelineCoreService.PipelineServiceCancelled(PipelineServiceCancelledEvent);
                            //! get an available service,add this service to pipelinecore

                            if (base.AddService(tService))
                            {
                                //! add service to pipeline core success
                                FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE = false;
                                FSM_WAIT_SERVICE_RESULT = true;
                                
                                tService.CompleteSignal.WaitOne();
                            }
                            else
                            {
                                tService.PipelineServiceCancelledEvent -= new PipelineCoreService.PipelineServiceCancelled(PipelineServiceCancelledEvent);

                                //! failed to add service
                                m_DynamicStageList[nCurrentStageIndex].Dequeue();

                                tService.Cancel();
                                tService.OnServiceCancelled();

                                //! raising event
                                ServiceItem.OnPipelineServiceException();


                                //! try to get next service
                                FSM_PEEK_SERVICE_FROM_CURRENT_STAGE_QUEUE = false;
                                FSM_CHECK_CURRENT_STAGE_QUEUE = true;
                            }
                        }
                    }
                    if (FSM_WAIT_SERVICE_RESULT)
                    {
                        //base.PipelineOpen = true;
                        PipelineService tService = m_DynamicStageList[nCurrentStageIndex].Peek();
                        if (m_StageServiceCancelled)
                        {
                            //! service cancelled, drop it
                            m_DynamicStageList[nCurrentStageIndex].Dequeue();

                            if (m_DynamicStageList.Count > 0)
                            {
                                nCurrentStageIndex = m_DynamicStageList.Count - 1;
                            }

                            //! reset state machine
                            FSM_WAIT_SERVICE_RESULT = false;
                            FSM_CHECK_CURRENT_STAGE_QUEUE = true;
                        }
                        else if (tService.Complete)
                        {
                            //! complete
                            switch (tService.StageState)
                            {
                                case PIPELINE_STAGE_STATE.STATE_BLOCKED:
                                    //! blocked, try previouse stage
                                    if (nCurrentStageIndex > 0)
                                    {
                                        nCurrentStageIndex--;
                                    }
                                    else
                                    {
                                        if (m_DynamicStageList.Count > 0)
                                        {
                                            nCurrentStageIndex = m_DynamicStageList.Count - 1;
                                        }
                                    }
                                    break;
                                case PIPELINE_STAGE_STATE.STATE_COMPLETE:
                                    //! complete, raising event
                                    m_DynamicStageList[nCurrentStageIndex].Dequeue();

                                    if (m_DynamicStageList.Count > 0)
                                    {
                                        nCurrentStageIndex = m_DynamicStageList.Count - 1;
                                    }

                                    break;

                                case PIPELINE_STAGE_STATE.STATE_READY:
                                //! ready for what ? consider it as "next state"
                                case PIPELINE_STAGE_STATE.STATE_NEXT_STATE:
                                    m_DynamicStageList[nCurrentStageIndex].Dequeue();
                                    if (tService.Stage > nCurrentStageIndex)
                                    {
                                        nCurrentStageIndex++;
                                        if (m_DynamicStageList.Count <= nCurrentStageIndex)
                                        {
                                            m_DynamicStageList.Add(new Queue<PipelineService>());
                                        }
                                        //! add current service to next stage
                                        m_DynamicStageList[nCurrentStageIndex].Enqueue(tService);
                                    }
                                    else if (tService.Stage == nCurrentStageIndex)
                                    {
                                        //! consider as complete

                                        if (m_DynamicStageList.Count > 0)
                                        {
                                            nCurrentStageIndex = m_DynamicStageList.Count - 1;
                                        }
                                    }
                                    else
                                    {
                                        //! exception, raising event
                                        tService.OnPipelineServiceException();

                                        if (m_DynamicStageList.Count > 0)
                                        {
                                            nCurrentStageIndex = m_DynamicStageList.Count - 1;
                                        }
                                    }
                                    break;
                                case PIPELINE_STAGE_STATE.STATE_WORKING:
                                    m_DynamicStageList[nCurrentStageIndex].Dequeue();
                                    //! should not meet this situation, raising event
                                    tService.OnPipelineServiceException();

                                    if (m_DynamicStageList.Count > 0)
                                    {
                                        nCurrentStageIndex = m_DynamicStageList.Count - 1;
                                    }
                                    break;
                            }

                            //! reset state machine
                            FSM_WAIT_SERVICE_RESULT = false;
                            FSM_CHECK_CURRENT_STAGE_QUEUE = true;

                        }
                    }

                    if (0 == m_DynamicStageList.Count)
                    {
                        lock (((ICollection)m_WaitingServiceQueue).SyncRoot)
                        {
                            if (0 == m_WaitingServiceQueue.Count)
                            {
                                //! all service is complete
                                break;
                            }
                        }
                    }

                    //Thread.Sleep(1);
                }
                while (true);
            }

            //! raising event
            OnPipelineStateReport(PIPELINE_STATE.PIPELINE_STOPPED);

            m_PipelineThread = null;
        }


        private void PipelineServiceCancelledEvent(PipelineCoreService Item)
        {
            Item.PipelineServiceCancelledEvent -= new PipelineCoreService.PipelineServiceCancelled(PipelineServiceCancelledEvent);
            //! set flag
            m_StageServiceCancelled = true;
        }


        public delegate void PipelinStateReport(PIPELINE_STATE State, Pipeline Pipeline);

        //! pipeline state report event
        public event PipelinStateReport PipelinStateReportEvent;

        //! \brief method for raising event
        private void OnPipelineStateReport(PIPELINE_STATE State)
        {
            if (null != PipelinStateReportEvent)
            {
                PipelinStateReportEvent.Invoke(State, this);
                //BeginInvoke(PipelinStateReportEvent, this);
            }
        }

        //! \brief property for getting/setting pipelinestate
        public new Boolean PipelineOpen
        {
            get 
            {
                if (!Available)
                {
                    return false;
                }

                if (null == m_PipelineThread)
                {
                    return false;
                }

                return true;//m_PipelineThread.IsAlive;
            }

            set 
            {
                if (!Available)
                {
                    return;
                }

                if (value)
                {
                    if (null == m_PipelineThread)
                    {
                        m_PipelineThread = new Thread(PipelineBackgroundTask);
                        m_PipelineThread.IsBackground = false;
                        m_PipelineThread.Priority = Priority;
                        m_PipelineThread.Name = "Pipeline";
                        m_PipelineThread.Start();

                        //! raising event 
                        OnPipelineStateReport(PIPELINE_STATE.PIPELINE_START);
                    }

                    m_RequestStop = false;
                }
                else
                {
                    if (null != m_PipelineThread)
                    {
                        m_RequestStop = true;
                    }
                }

                //! set base pipeline open property
                //base.PipelineOpen = value;
            }
        }

        //! \brief method for add a pipeline service
        public override System.Boolean AddService(PipelineCoreService ServiceItem)
        {
            return AddService(ServiceItem as PipelineService);
        }

        public override bool AddServices(PipelineCoreService[] ServiceItems)
        {
            List<PipelineService> tServiceList = new List<PipelineService>();

            foreach (PipelineCoreService tService in ServiceItems)
            {
                if (tService is PipelineCoreService)
                {
                    tServiceList.Add(tService as PipelineService);
                }
            }

            return AddServices(tServiceList.ToArray());
        }

        //! \brief method for add a pipeline service
        public System.Boolean AddService(PipelineService ServiceItem)
        {
            if (!Available)
            {
                return false;
            }

            if (null == ServiceItem)
            {
                return false;
            }

            lock (((ICollection)m_WaitingServiceQueue).SyncRoot)
            {
                try
                {
                    //! add service to queue
                    m_WaitingServiceQueue.Enqueue(ServiceItem);
                }
                catch (Exception )
                {
                    //System.Console.WriteLine(e.ToString());
                    return false;
                }
            }

            //! auto star
            if (AutoStart)
            {
                //! auto start pipeline
                PipelineOpen = true;
            }

            return true;
        }

        

        //! \brief method for add a pipeline service
        public Boolean AddServices(PipelineService[] ServiceItems)
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

            lock (((ICollection)m_WaitingServiceQueue).SyncRoot)
            {
                foreach (PipelineService tService in ServiceItems)
                {
                    try
                    {
                        //! add service to queue
                        m_WaitingServiceQueue.Enqueue(tService);
                    }
                    catch (Exception)
                    {
                        //System.Console.WriteLine(e.ToString());
                        return false;
                    }
                }
            }

            //! auto star
            if (AutoStart)
            {
                //! auto start pipeline
                PipelineOpen = true;
            }

            return true;
        }


        protected override void _Dispose()
        {
            try
            {
                if (null != m_PipelineThread)
                {
                    if (m_PipelineThread.IsAlive)
                    {
                        m_RequestStop = true;
                        if (!m_PipelineThread.Join(3000))
                        {
                            m_PipelineThread.Abort();
                        }
                        //! raising event
                        OnPipelineStateReport(PIPELINE_STATE.PIPELINE_STOPPED);
                    }
                    m_RequestStop = false;
                    m_PipelineThread = null;
                }
            }
            catch (Exception) { }
        }
    }
    //! @}

    //! \name pipeline state state
    //! @{
    public enum PIPELINE_STAGE_STATE
    { 
        STATE_WORKING,                      //!< working at current stage
        STATE_BLOCKED,                      //!< current state is blocked
        STATE_READY,                        //!< current state is ready and is waiting to running
        STATE_COMPLETE,                     //!< all stage clear
        STATE_NEXT_STATE                    //!< apply for next state
    }
    //! @}

    //! \name pipeline service
    //! @{
    public abstract class PipelineService : PipelineCoreService
    {
        private List<StageService> m_Stages = new List<StageService>();
        private System.Int32 m_nStageIndex = 0;
        private PIPELINE_STAGE_STATE m_StageState = PIPELINE_STAGE_STATE.STATE_READY;

        //! \brief constructor
        public PipelineService(Object TargetItem)
            : base(TargetItem)
        {
            Initialize();
        }

        //! \brief constructor
        public PipelineService(Object TargetItem, System.Int32 SafeTimeOutSetting)
            : base(TargetItem, SafeTimeOutSetting)
        {
            Initialize();
        }

        //! \brief initialize this object
        private void Initialize()
        {
            //! \brief register all stages
            RegisterStages();
        }

        //! \brief inherited class could using this method to registering stage functions
        protected void AddStage(StageService Method)
        {
            if (!Available)
            {
                return;
            }

            if (null != Method)
            {
                //! add method to stage list
                m_Stages.Add(Method);
            }
        }

        //! \brief property for getting current stage index
        internal Int32 Stage
        {
            get 
            {
                if (!Available)
                {
                    return -1;
                }

                return m_nStageIndex; 
            }
        }

        //! \brief property for getting the stage count of current service.
        internal Int32 StageCount
        {
            get 
            {
                if (!Available)
                {
                    return 0;
                }

                return m_Stages.Count; 
            }
        }

        //! \brief property for getting current stage state
        internal PIPELINE_STAGE_STATE StageState
        {
            get 
            {
                if (!Available)
                {
                    return PIPELINE_STAGE_STATE.STATE_COMPLETE;
                }

                return m_StageState; 
            }
        }

        public delegate void PipelineServiceException(PipelineService tService);

        public event PipelineServiceException PipelineServiceExceptionEvent;

        //! \brief method for raising service failed event
        internal void OnPipelineServiceException()
        {
            if (null != PipelineServiceExceptionEvent)
            {
                m_Invoker.BeginInvoke(PipelineServiceExceptionEvent,this);
                //BeginInvoke(PipelineServiceExceptionEvent, this);
            }
        }

        //! \brief method for registering stages
        protected abstract void RegisterStages();

        //! \brief delegate for pipeline stage function
        protected delegate PIPELINE_STAGE_STATE StageService();

        //! \brief pipeline background service function
        public override sealed System.Boolean DoService()
        {
            if (!Available)
            {
                return false;
            }
            

            do
            {
                //! check stage index
                if (m_Stages.Count <= m_nStageIndex)
                {
                    //! all stage clear
                    m_StageState = PIPELINE_STAGE_STATE.STATE_COMPLETE;
                    return false;
                }

                //! this condition should not happened.
                if (null == m_Stages[m_nStageIndex])
                {
                    m_nStageIndex++;
                    continue;
                }

                break;

            }
            while (true);

            //! update current stage state
            m_StageState = m_Stages[m_nStageIndex].Invoke();

            switch (m_StageState)
            { 
                case PIPELINE_STAGE_STATE.STATE_BLOCKED:
                    //! current stage is blocked, we need wait...
                    return false;

                case PIPELINE_STAGE_STATE.STATE_COMPLETE:
                    //! maybe we should raising a pipeline service complete event
                    if (!Cancelled)
                    {
                        OnPipelineServiceComplete();
                    }
                    return false;
                    
                case PIPELINE_STAGE_STATE.STATE_NEXT_STATE:
                    //! next stage is applied
                    m_nStageIndex++;
                    if (m_Stages.Count <= m_nStageIndex)
                    {
                        //! all stage clear
                        m_StageState = PIPELINE_STAGE_STATE.STATE_COMPLETE;
                        OnPipelineServiceComplete();
                        return false;
                    }
                    break;

                case PIPELINE_STAGE_STATE.STATE_READY:
                case PIPELINE_STAGE_STATE.STATE_WORKING:
                    break;
            }


            return true;
        }

        public delegate void PipelineServiceCompleted(PipelineService tService);

        public event PipelineServiceCompleted PipelineServiceCompletedEvent;

        protected virtual void OnPipelineServiceComplete()
        {
            if (null != PipelineServiceCompletedEvent)
            {
                m_Invoker.BeginInvoke(PipelineServiceCompletedEvent,this);
            }
        }

        protected override sealed void OnInternalPipelineCoreServiceComplete()
        {
 	        //! when one stage is cleared, doing nothing at all
        }
    }
    //! @}

    public abstract class TPipelineService<TObject> : PipelineService
    {
        //! \brief constructor
        public TPipelineService(TObject Item)
            : base(Item)
        {

        }

        //! \brief constructor
        public TPipelineService(TObject Item, System.Int32 SafeTimeOutSetting)
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
