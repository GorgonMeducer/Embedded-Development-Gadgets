using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Diagnostics;
using ESnail.Utilities.Generic;


namespace ESnail.Utilities.Automata.FSM
{
    public delegate void FSMReport(IFSM tFSM, params Object[] tArgs);
    

    //!　\name finite state machine state
    //! @{
    public enum Status
    { 
        FSM_IDLE            = 0x00,         //!< idle state
        FSM_STARTING        = 0x01,         //!< initializing
        FSM_WORKING         = 0x02,         //!< normal working
        FSM_PENDING         = 0x03,         //!< calling other finite state machine
        FSM_CLOSING         = 0x04,         //!< stop-request received
        FSM_CLOSED          = 0x05,         //!< finite state machine closed
        FSM_CANCELLED       = 0x06,         //!< finite state machine cancelled
        FSM_ERROR           = 0xE0,         //!< state machine error
        FSM_INVALID         = 0xFF          //!< invalid
    }
    //! @}

    public interface IState
    {
        Boolean IsFSM
        {
            get;
        }

        Status Status
        {
            get;
        }

        Status Task(params Object[] tArgs);

        Boolean IsActive
        {
            get;
        }

        Boolean Available
        {
            get;
        }

        
    }

    public interface IFSM : IState
    {
        Boolean Start(params Object[] tArgs);
        Boolean Start();
        Boolean Reset();

        Object[] Arguments
        {
            get;
            set;
        }
        
        Status RequestStop();
        Status RequestStop(Int32 tMilionsecond);

        Boolean StateChanged
        {
            get;
        }

        Boolean TriggerState(SafeID tID);
        Boolean TriggerFSM(SafeID tID, params Object[] tArgs);
        Boolean CallFSM(SafeID tID, String tReturnID, params Object[] tArgs);
        Boolean Deactive(params SafeID[] tIDs);

        event FSMReport FSMClosing;
        event FSMReport FSMClosed;
        event FSMReport FSMStarted;
    }

    public delegate void FSMStateReport(State tState);
    public delegate Status StateRoutine(IFSM tParent, params Object[] tArgs);
    public delegate Boolean StateErrorHandler(IFSM tParent, params Object[] tArgs);

    public class State :IState, ISafeID
    {
        private SafeID m_ID = null;
        protected Status m_Status = Status.FSM_INVALID;
        private StateRoutine m_Routine = null;
        private StateErrorHandler m_ErrorHandler = null;
        protected AutoResetEvent m_Event = null;
        private State m_Parent = null;
        protected Boolean m_Available = false;

        public State(String tID, StateRoutine tRoutine, AutoResetEvent tEvent, State tParent)
        {
            m_Event = tEvent;
            m_Parent = tParent;
            m_ID = tID;

            if (null == m_Event)
            {
                m_Event = new AutoResetEvent(false);
            }

            if (null == m_Parent)
            {
                return;
            }

            if (null == tID)
            {
                return;
            }
            else if ("" == tID.Trim())
            {
                return;
            }
            else if (null == tRoutine)
            {
                return;
            }
            m_Routine = tRoutine;
            

            m_Status = Status.FSM_IDLE;
            m_Available = true;
        }

        public State(String tID, StateRoutine tRoutine, StateErrorHandler tErrorHandler, AutoResetEvent tEvent, State tParent)
        {
            m_Event = tEvent;
            m_Parent = tParent;
            m_ID = tID;
            m_ErrorHandler = tErrorHandler;

            if (null == m_Event)
            {
                m_Event = new AutoResetEvent(false);
            }

            if (null == m_Parent)
            {
                return;
            }

            if (null == tID)
            {
                return;
            }
            else if ("" == tID.Trim())
            {
                return;
            }
            else if (null == tRoutine)
            {
                return;
            }
            m_Routine = tRoutine;


            m_Status = Status.FSM_IDLE;
            m_Available = true;
        }

        internal AutoResetEvent StopSignal
        {
            get { return m_Event; }
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        public Boolean IsActive
        {
            get
            {
                switch (m_Status)
                {
                    
                    case Status.FSM_INVALID:
                    case Status.FSM_CANCELLED:
                    case Status.FSM_CLOSED:
                    case Status.FSM_IDLE:
                        return false;
                    case Status.FSM_ERROR:
                    case Status.FSM_CLOSING:
                    case Status.FSM_PENDING:
                    case Status.FSM_STARTING:
                    case Status.FSM_WORKING:
                    default:
                        return true;
                }
            }
        }

        

        #region state events
        
        public event FSMStateReport EnterState;

        private void OnEnterState()
        { 
            if (null != EnterState)
            {
                try
                {
                    EnterState(this);
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }

        public event FSMStateReport LeaveState;

        private void OnLeaveState()
        {
            if (null != LeaveState)
            {
                try
                {
                    LeaveState(this);
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }

        public event FSMStateReport StateError;

        private void OnStateError()
        {
            if (null != StateError)
            {
                try
                {
                    StateError(this);
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }

        #endregion

        protected virtual Boolean ErrorHandler(params Object[] tArgs)
        {
            if (null == m_ErrorHandler)
            {
                return false;
            }

            Boolean tResult = false;

            try
            {
                tResult = m_ErrorHandler.Invoke(m_Parent as IFSM, tArgs);
            }
            catch (Exception Err)
            {
                Err.ToString();
            }

            return tResult;
        }

        public virtual Status Task(params Object[] tArgs)
        {
            if (m_Event.WaitOne(0))
            {
                m_Status = Status.FSM_CANCELLED;
                return m_Status;
            }

            if (m_Status == Status.FSM_INVALID)
            {
                return m_Status;
            }
            else if (m_Status != Status.FSM_WORKING)
            {
                m_Status = Status.FSM_WORKING;
                m_Event.Reset();
                OnEnterState();                     //!< raising event
            }

            switch (m_Routine.Invoke(m_Parent as IFSM, tArgs))
            {
                case Status.FSM_INVALID:
                    //m_Status = Status.FSM_INVALID;
                    //break;
                case Status.FSM_ERROR:
                    if (ErrorHandler(tArgs))
                    {
                        //! error handled
                        break;
                    }
                    else
                    {
                        //! failed to handle error
                        m_Status = Status.FSM_ERROR;
                    }
                    break;
                case Status.FSM_CANCELLED:
                case Status.FSM_CLOSED:
                case Status.FSM_IDLE:
                    m_Status = Status.FSM_CLOSED;
                    OnLeaveState();                 //!< raising event
                    break;
                
                case Status.FSM_PENDING:
                    m_Status = Status.FSM_PENDING;
                    break;
                case Status.FSM_CLOSING:
                case Status.FSM_STARTING:
                case Status.FSM_WORKING:
                    m_Status = Status.FSM_WORKING;
                    break;
            }
            /*
            if (m_Routine.Invoke(m_Parent as IFSM, tArgs))
            {
                m_Status = Status.FSM_WORKING;
            }
            else
            {
                m_Status = Status.FSM_CLOSED;
                OnLeaveState();                     //!< raising event
            }
            */
            return m_Status;
        }

        public Status Status
        {
            get { return m_Status; }
        }

        public State Parent
        {
            get { return m_Parent; }
        }

        public SafeID ID
        {
            get
            {
                return m_ID;
            }
            set { }
        }

        public virtual Boolean IsFSM
        {
            get { return false; }
        }
    }

    public class miniFSM :State, IFSM, IDisposable
    {
        private Int32 m_Timeout = 100;
        private Object[] m_Args = null;
        private TSet<State> m_ActiveStateSet = new TSet<State>();
        private TSet<State> m_FreeStateSet = new TSet<State>();
        private TSet<State> m_ActiveFSMSet = new TSet<State>();
        private Object m_Locker = new object();
        private Stopwatch m_StopWatch = new Stopwatch();
        private Boolean m_DynamicMode = false;
        private String m_ReturnStateID = null;
        private Boolean m_StateChanged = false;

        #region constructions
        public miniFSM(String tID)
            : base(tID, null, null, null)
        {
            Initialize();
        }



        public miniFSM(String tID, AutoResetEvent tEvent)
            : base(tID, null, tEvent, null)
        {
            Initialize();
        }

        public miniFSM(String tID, miniFSM tParent)
            : base(tID, null, null, tParent)
        {
            Initialize();
        }

        public miniFSM(String tID, AutoResetEvent tEvent, miniFSM tParent)
            : base(tID, null, tEvent, tParent)
        {
            Initialize();
        }

        public miniFSM(String tID, params Object[] tArgs)
            : base(tID, null, null, null)
        {
            m_Args = tArgs;
            Initialize();
        }

        public miniFSM(String tID, AutoResetEvent tEvent, params Object[] tArgs)
            : base(tID, null, tEvent, null)
        {
            m_Args = tArgs;
            Initialize();
        }

        public miniFSM(String tID, miniFSM tParent, params Object[] tArgs)
            : base(tID, null, null, tParent)
        {
            m_Args = tArgs;
            Initialize();
        }

        public miniFSM(String tID, AutoResetEvent tEvent, miniFSM tParent, params Object[] tArgs)
            : base(tID, null, tEvent, tParent)
        {
            m_Args = tArgs;
            Initialize();
        }        

        #endregion

        private void Initialize()
        {
            m_Available = true;
            m_ReturnStateID = null;
        }

        private SafeID ReturnStateID
        {
            get { return m_ReturnStateID; }
        }

        public Boolean StateChanged
        {
            get { return m_StateChanged; }
        }

        public Object[] Arguments
        {
            get { return m_Args; }
            set
            {
                if (this.IsActive)
                {
                    return;
                }
                m_Args = value;
            }
        }

        public Boolean DynamicMode
        {
            get { return m_DynamicMode; }
            set 
            {
                if (this.IsActive)
                {
                    return;
                }

                m_DynamicMode = value;
            }
        }

        public Int32 Timeout
        {
            get { return m_Timeout; }
            set
            {
                if (Status == Status.FSM_IDLE)
                {
                    m_Timeout = (value < 0) ? 0 : value;
                }
            }
        }

        public override Status Task(params object[] tArgs)
        {
            if (!this.Available)
            {
                return m_Status;
            }


            do
            {
                switch (m_Status)
                {
                    case Status.FSM_STARTING:
                        m_Status = Status.FSM_WORKING;
                        break;
                    case Status.FSM_CLOSING:
                    case Status.FSM_PENDING:
                    case Status.FSM_WORKING:
                        break;
                    case Status.FSM_ERROR:
                    case Status.FSM_INVALID:
                    case Status.FSM_CANCELLED:
                    case Status.FSM_CLOSED:
                    case Status.FSM_IDLE:
                    default:
                        return m_Status;
                }

                //! stop event
                if (m_Event.WaitOne(0))
                {
                    lock (m_Locker)
                    {
                        foreach (State tItem in m_ActiveStateSet)
                        {
                            if (null == tItem)
                            {
                                continue;
                            }
                            else if (!tItem.Available)
                            {
                                continue;
                            }
                            if ((tItem.IsActive) || (tItem.Status == Status.FSM_IDLE))
                            {
                                tItem.StopSignal.Set();
                            }
                        }

                        foreach (State tItem in m_ActiveFSMSet)
                        {
                            if (null == tItem)
                            {
                                continue;
                            }
                            else if (!tItem.Available)
                            {
                                continue;
                            }
                            IFSM tFSM = tItem as IFSM;
                            if (null == tFSM)
                            {
                                continue;
                            }
                            if ((tFSM.IsActive) || (tFSM.Status == Status.FSM_IDLE))
                            {
                                tFSM.RequestStop();
                            }
                        }
                        m_Event.Reset();
                    }
                }

                m_StateChanged = false;                     //!< reset state change flag


                lock (m_Locker)
                {
                    State[] tActiveStates = m_ActiveStateSet.ToArray();
                    foreach (State tItem in tActiveStates)
                    {
                        if (null == tItem)
                        {
                            continue;
                        }
                        else if (!tItem.Available)
                        {
                            continue;
                        }

                        switch (tItem.Task(m_Args))
                        {
                            case Status.FSM_PENDING:
                            case Status.FSM_CLOSING:
                            case Status.FSM_STARTING:
                            case Status.FSM_WORKING:

                                break;
                            case Status.FSM_ERROR:
                                //! encountered an unhandled error
                                if (ErrorHandler(m_Args))
                                {
                                    continue;
                                }
                                m_Status = Status.FSM_ERROR;
                                return m_Status;
                            case Status.FSM_INVALID:
                            case Status.FSM_CANCELLED:
                            case Status.FSM_CLOSED:
                            case Status.FSM_IDLE:
                            default:
                                m_FreeStateSet.Add(tItem);
                                m_ActiveStateSet.Remove(tItem.ID);
                                m_StateChanged = true;
                                break;
                        }
                    }

                    tActiveStates = m_ActiveFSMSet.ToArray();
                    foreach (State tItem in tActiveStates)
                    {
                        if (null == tItem)
                        {
                            continue;
                        }
                        else if (!tItem.Available)
                        {
                            continue;
                        }
                        switch (tItem.Task(m_Args))
                        {
                            case Status.FSM_ERROR:
                                m_Status = Status.FSM_ERROR;
                                return m_Status;
                            case Status.FSM_CLOSING:
                            case Status.FSM_PENDING:
                            case Status.FSM_STARTING:
                            case Status.FSM_WORKING:
                                break;
                            case Status.FSM_INVALID:
                            case Status.FSM_CANCELLED:
                            case Status.FSM_CLOSED:
                            case Status.FSM_IDLE:
                            default:
                                m_FreeStateSet.Add(tItem);
                                m_ActiveFSMSet.Remove(tItem.ID);

                                m_StateChanged = true;

                                IFSM tFSM = tItem as IFSM;
                                if (null != tFSM)
                                {
                                    tFSM.Reset();
                                }

                                do
                                {
                                    miniFSM tminiFSM = tItem as miniFSM;
                                    if (null == tminiFSM)
                                    {
                                        break;
                                    }
                                    else if (null == tminiFSM.ReturnStateID)
                                    {
                                        break;
                                    }
                                    this.TriggerFSM(tminiFSM.ReturnStateID, m_Args);
                                }
                                while (false);
                                break;
                        }
                    }
                }

                RefreshState();
            }
            while (m_StateChanged);

            return m_Status;
        }

        public new miniFSM Parent
        {
            get { return base.Parent as miniFSM; }
        }

        #region states management
        public Boolean AddState(State tState)
        {
            if ((!this.Available) || (!m_DynamicMode && IsActive))
            {
                return false;
            } 
            else if (null == tState)
            {
                return false;
            }
            else if (!tState.Available)
            {
                return false;
            }
            

            lock (m_Locker)
            {
                if (null != m_ActiveFSMSet.Find(tState.ID))
                {
                    return true;
                }
                if (null != m_ActiveStateSet.Find(tState.ID))
                {
                    return true;
                }
                m_FreeStateSet.Add(tState);
            }

            RefreshState();

            return true;
        }

        public Boolean RemoveState(SafeID tID)
        {
            if ((!this.Available) || ((!m_DynamicMode && IsActive)))
            {
                return false;
            }

            lock (m_Locker)
            {
                State tTarget = m_FreeStateSet.Find(tID);
                if (null == tTarget)
                {
                    return false;
                }

                m_FreeStateSet.Remove(tID);
            }

            RefreshState();

            return true;
        }

        public Boolean Clear()
        {
            if (!this.Available)
            {
                return false;
            }
            else if (this.IsActive)
            {
                return false;
            }
            
            lock (m_Locker)
            {
                m_ActiveStateSet.Clear();
                m_FreeStateSet.Clear();
                m_ActiveFSMSet.Clear();
            }

            RefreshState();

            return true;
        }

        public Boolean TriggerState(SafeID tID)
        {
            if (!this.Available)
            {
                return false;
            }
            else if (!this.IsActive)
            {
                return false;
            }

            lock (m_Locker)
            {
                State tTarget = m_FreeStateSet.Find(tID);
                if (null != tTarget)
                {
                    if (!tTarget.Available)
                    {
                        return false;
                    }

                    if (tTarget.IsFSM)
                    {
                        IFSM tFSM = tTarget as IFSM;
                        if (null == tFSM)
                        {
                            return false;
                        }
                        m_FreeStateSet.Remove(tID);
                        m_ActiveFSMSet.Add(tTarget);
                        tFSM.Reset();
                        tFSM.Start();
                    }
                    else
                    {
                        m_FreeStateSet.Remove(tID);
                        m_ActiveStateSet.Add(tTarget);
                    }
                }
            }

            m_StateChanged = true;

            RefreshState();

            return true;
        }

        public Boolean CallFSM(SafeID tID, String tReturnID, params Object[] tArgs)
        {
            if (!this.Available)
            {
                return false;
            }
            else if (!this.IsActive)
            {
                return false;
            }

            lock (m_Locker)
            {
                State tTarget = m_FreeStateSet.Find(tID);
                if (null != tTarget)
                {
                    if (!tTarget.Available)
                    {
                        return false;
                    }


                    if (tTarget.IsFSM)
                    {
                        IFSM tFSM = tTarget as IFSM;
                        if (null == tFSM)
                        {
                            return false;
                        }
                        m_FreeStateSet.Remove(tID);
                        m_ActiveFSMSet.Add(tTarget);
                        tFSM.Reset();
                        do
                        {
                            miniFSM tminiFSM = tFSM as miniFSM;
                            if (null == tminiFSM)
                            {
                                break;
                            }
                            tminiFSM.m_ReturnStateID = tReturnID;
                        }
                        while (false);

                        tFSM.Start(tArgs);
                    }
                    else
                    {
                        m_FreeStateSet.Remove(tID);
                        m_ActiveStateSet.Add(tTarget);
                    }
                }
            }

            m_StateChanged = true;

            RefreshState();

            return true;
        }

        public Boolean TriggerFSM(SafeID tID, params Object[] tArgs)
        {
            if (!this.Available)
            {
                return false;
            }
            else if (!this.IsActive)
            {
                return false;
            }

            lock (m_Locker)
            {
                State tTarget = m_FreeStateSet.Find(tID);
                if (null != tTarget)
                {
                    if (!tTarget.Available)
                    {
                        return false;
                    }


                    if (tTarget.IsFSM)
                    {
                        IFSM tFSM = tTarget as IFSM;
                        if (null == tFSM)
                        {
                            return false;
                        }
                        m_FreeStateSet.Remove(tID);
                        m_ActiveFSMSet.Add(tTarget);
                        tFSM.Reset();
                        tFSM.Start(tArgs);
                    }
                    else
                    {
                        m_FreeStateSet.Remove(tID);
                        m_ActiveStateSet.Add(tTarget);
                    }
                }
            }

            m_StateChanged = true;

            RefreshState();

            return true;
        }

        public Boolean Deactive(params SafeID[] tIDs)
        {
            if (!this.Available)
            {
                return false;
            }
            else if (!this.IsActive)
            {
                return false;
            }
            else if (null == tIDs)
            {
                return false;
            }
            else if (0 == tIDs.Length)
            {
                return false;
            }

            lock(m_Locker)
            {
                foreach (SafeID tID in tIDs)
                { 
                    State tTarget = m_ActiveFSMSet.Find(tID);
                    if (null != tTarget)
                    {
                        m_ActiveFSMSet.Remove(tID);
                        IFSM tFSM = tTarget as IFSM;
                        if (null != tFSM)
                        {
                            do
                            {
                                tFSM.RequestStop();
                            }
                            while (tFSM.IsActive);
                            tFSM.Reset();
                        }
                        m_FreeStateSet.Add(tTarget);
                    }

                    tTarget = m_ActiveStateSet.Find(tID);
                    if (null != tTarget)
                    {
                        tTarget.StopSignal.Set();

                        m_ActiveStateSet.Remove(tID);
                        m_FreeStateSet.Add(tTarget);
                    }
                }
            }

            RefreshState();

            return true;
        }

        private void RefreshState()
        {
            lock (m_Locker)
            {
                switch (m_Status)
                {
                    case Status.FSM_INVALID:
                    case Status.FSM_IDLE:
                    case Status.FSM_PENDING:
                    case Status.FSM_STARTING:
                    case Status.FSM_WORKING:
                        break;
                    case Status.FSM_ERROR:
                    case Status.FSM_CLOSING:
                    case Status.FSM_CANCELLED:
                    case Status.FSM_CLOSED:
                    default:
                        return;
                    
                }

                if (0 == m_ActiveFSMSet.Count && 0 == m_ActiveStateSet.Count)
                {
                    if (0 == m_FreeStateSet.Count)
                    {
                        m_Status = Status.FSM_INVALID;
                    }
                    else
                    {
                        m_Status = Status.FSM_IDLE;
                    }
                }
                else
                {
                    Status tStatus = Status.FSM_PENDING;

                    do
                    {
                        if (0 != m_ActiveStateSet.Count)
                        {
                            foreach (State tState in m_ActiveStateSet)
                            {
                                if (tState.Status == Status.FSM_WORKING)
                                {
                                    tStatus = Status.FSM_WORKING;
                                    break;
                                }
                            }
                            if (tStatus == Status.FSM_WORKING)
                            {
                                break;
                            }

                        }

                        if (0 != m_ActiveFSMSet.Count)
                        {
                            foreach (State tState in m_ActiveFSMSet)
                            {
                                if (tState.Status == Status.FSM_WORKING)
                                {
                                    tStatus = Status.FSM_WORKING;
                                    break;
                                }
                            }
                            if (tStatus == Status.FSM_WORKING)
                            {
                                break;
                            }
                        }
                    }
                    while (false);

                    m_Status = tStatus;
                }
            }
        }

        #endregion

        public Boolean Start()
        {
            if ((m_Status != Status.FSM_IDLE) || (0 == m_FreeStateSet.Count))
            {
                return false;
            }

            lock (m_Locker)
            {
                State tFirstState = m_FreeStateSet.Items[0];
                if (!tFirstState.Available)
                {
                    return false;
                }
                if (tFirstState.IsFSM)
                {
                    IFSM tFSM = tFirstState as IFSM;
                    if (null == tFSM)
                    {
                        return false;
                    }
                    else if (tFSM.Reset())
                    {
                        return false;
                    }
                    else if (!tFSM.Start())
                    {
                        return false;
                    }
                    m_ActiveFSMSet.Add(tFirstState);
                }
                else
                {
                    m_ActiveStateSet.Add(tFirstState);
                }

                m_Status = Status.FSM_STARTING;

                if (null != FSMStarted)
                {
                    try
                    {
                        FSMStarted.Invoke(this, m_Args);
                    }
                    catch (Exception )
                    {
                    }
                }
            }

            return true;
        }

        public Boolean Start(params Object[] tArgs)
        {
            if ((m_Status != Status.FSM_IDLE) || (0 == m_FreeStateSet.Count))
            {
                return false;
            }

            lock (m_Locker)
            {
                State tFirstState = m_FreeStateSet.Items[0];
                if (!tFirstState.Available)
                {
                    return false;
                }
                if (tFirstState.IsFSM)
                {
                    IFSM tFSM = tFirstState as IFSM;
                    if (null == tFSM)
                    {
                        return false;
                    }
                    else if (tFSM.Reset())
                    {
                        return false;
                    }
                    else if (!tFSM.Start(tArgs))
                    {
                        return false;
                    }
                    m_ActiveFSMSet.Add(tFirstState);
                }
                else
                {
                    
                    m_ActiveStateSet.Add(tFirstState);
                }
                m_Args = tArgs;
                m_Status = Status.FSM_STARTING;

                if (null != FSMStarted)
                {
                    try
                    {
                        FSMStarted.Invoke(this, m_Args);
                    }
                    catch (Exception )
                    {
                    }
                }
            }

            return true;
        }


        public Boolean Reset()
        {
            switch (m_Status)
            {
                case Status.FSM_ERROR:
                case Status.FSM_CANCELLED:
                case Status.FSM_CLOSED:
                    
                    //! reset stop watch
                    if (m_StopWatch.IsRunning)
                    {
                        m_StopWatch.Stop();
                    }
                    m_StopWatch.Reset();

                    m_Event.Reset();                        //!< reset event
                    m_Status = Status.FSM_IDLE;
                    break;
                case Status.FSM_IDLE:
                    return true;
                
                case Status.FSM_CLOSING:
                case Status.FSM_PENDING:
                case Status.FSM_STARTING:
                case Status.FSM_WORKING:
                case Status.FSM_INVALID:
                default:
                    return false;
            }

            return false;
        }

        public Status RequestStop()
        {
            RequestStop(m_Timeout);

            return m_Status;
        }

        public Status RequestStop(int tMilionsecond)
        {
            lock (m_Event)
            {
                switch (m_Status)
                {
                    case Status.FSM_CLOSING:

                        lock (m_Locker)
                        {
                            if ((m_ActiveFSMSet.Count == 0) && (m_ActiveStateSet.Count == 0))
                            {
                                if (0 == m_FreeStateSet.Count)
                                {
                                    m_Status = Status.FSM_INVALID;
                                }
                                else
                                {
                                    m_Status = Status.FSM_CLOSED;
                                }
                                break;
                            }
                        }

                        //! force to stop
                        if (m_StopWatch.ElapsedMilliseconds >= tMilionsecond)
                        {
                            lock (m_Locker)
                            {
                                if (m_ActiveStateSet.Count > 0)
                                {
                                    foreach (State tItem in m_ActiveStateSet)
                                    {
                                        if (null == tItem)
                                        {
                                            continue;
                                        }
                                        else if (!tItem.Available)
                                        {
                                            continue;
                                        }
                                        m_FreeStateSet.Add(tItem);
                                    }
                                }
                                m_ActiveStateSet.Clear();

                                if (m_ActiveFSMSet.Count > 0)
                                {
                                    foreach (State tItem in m_ActiveFSMSet)
                                    {
                                        if (null == tItem)
                                        {
                                            continue;
                                        }
                                        else if (!tItem.Available)
                                        {
                                            continue;
                                        }

                                        IFSM tFSMItem = tItem as IFSM;
                                        if (null == tFSMItem)
                                        {
                                            continue;
                                        }

                                        do
                                        {
                                            tFSMItem.RequestStop(tMilionsecond);
                                        }
                                        while (tFSMItem.IsActive);
                                        tFSMItem.Reset();

                                        m_FreeStateSet.Add(tItem);
                                    }
                                }
                                m_ActiveFSMSet.Clear();
                                m_StopWatch.Stop();
                                m_Status = Status.FSM_CLOSED;
                            }
                        }

                        break;

                    case Status.FSM_CANCELLED:
                    case Status.FSM_CLOSED:
                    case Status.FSM_IDLE:
                    case Status.FSM_INVALID:
                        break;

                    case Status.FSM_ERROR:
                    case Status.FSM_PENDING:
                    case Status.FSM_STARTING:
                    case Status.FSM_WORKING:
                        m_Status = Status.FSM_CLOSING;
                        m_Event.Set();
                        m_StopWatch.Reset();
                        m_StopWatch.Start();
                        //! raising closing event
                        if (null != FSMClosing)
                        {
                            try
                            {
                                FSMClosing.Invoke(this, m_Args);
                            }
                            catch (Exception )
                            {
                            }
                        }

                        break;
                    default:
                        break;
                }
            }

            return m_Status;
        }

        public override Boolean IsFSM
        {
            get { return true;}
        }

        public event FSMReport FSMClosing;

        public event FSMReport FSMClosed;

        public event FSMReport FSMStarted;


        #region IDisposable Members

        ~miniFSM()
        {
            Dispose();
        }

        private Boolean m_Disposed = false;

        public Boolean Disposed
        {
            get { return m_Disposed; }
        }

        public void Dispose()
        {
            if (m_Disposed)
            {
                return;
            }
            m_Disposed = true;

            try
            {
                _Dispose();
            }
            catch (Exception Err)
            {
                Err.ToString();
            }

            try
            {
                Boolean m_Waiting = true;
                do
                {
                    switch (RequestStop(m_Timeout))
                    {
                        case Status.FSM_ERROR:
                        case Status.FSM_CLOSING:
                        case Status.FSM_PENDING:
                        case Status.FSM_STARTING:
                        case Status.FSM_WORKING:
                            break;
                        case Status.FSM_INVALID:
                        case Status.FSM_CLOSED:
                        case Status.FSM_IDLE:
                        default:
                            m_Waiting = false;
                            break;
                    }
                }
                while (m_Waiting);
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
            finally
            {
                m_Status = Status.FSM_INVALID;
            }

            GC.SuppressFinalize(this);
        }

        protected virtual void _Dispose()
        { 
        
        }

        #endregion


        
    }

}
