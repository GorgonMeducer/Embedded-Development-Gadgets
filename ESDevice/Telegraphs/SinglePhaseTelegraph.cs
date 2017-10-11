
//#define __ASYN_TELEGRAPH_REPORT__ 

using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device;
using ESnail.CommunicationSet.Commands;
using System.Timers;
using System.Threading;
using ESnail.Device.Telegraphs.Engines;
using ESnail.Utilities.Log;



namespace ESnail.Device.Telegraphs
{

    
    //! enum for show the state of a receiving BatteryManageTelegraph item
    public enum BM_TELEGRAPH_STATE : ushort
    {
        BM_TELE_RT_SUCCESS,
        BM_TELE_RT_TIME_OUT,
        BM_TELE_RT_CANCELLED,
        BM_TELE_RT_ERROR,
        BM_TELE_RT_ERROR_DATA_SIZE_TOO_LARGE,
        BM_TELE_RT_ERROR_ILLEGAL_FRAME,
        BM_TELE_RT_ERROR_FAILD_TO_WRITE_DEVICE
    }

    //! a delegate for handling event coming from BatteryManageTelegraph
    public delegate void SinglePhaseTelegraphEventHandler
                            (
                                SinglePhaseTelegraph tTelegraph,
                                BM_TELEGRAPH_STATE State,
                                ESCommand ReceivedCommand
                            );
    
    //! \name BatteryManageTelegraph
    // @{
    public abstract class SinglePhaseTelegraph : Telegraph
    {
        protected ESCommand m_Command = null;
        protected System.Timers.Timer m_Timer = new System.Timers.Timer();
        private Boolean m_IfDisposed = false;

        //! constructor
        public SinglePhaseTelegraph(ESCommand Command)
            : base()
        {
            if (null != Command)
            {
                m_Command = Command;

                lock (m_Timer)
                {
                    //! handlinig time out feature
                    switch ((BM_CMD_RT)m_Command.TimeOut)
                    {
                        case BM_CMD_RT.BM_CMD_RT_NO_RESPONSE:           //!< command need no response
                            //! set telegraph cancel flag
                            CancelTelegraph();
                            break;
                        case BM_CMD_RT.BM_CMD_RT_NO_TIME_OUT:           //!< Wait forever
                            m_Timer.Enabled = false;
                            break;
                        default:
                            //! time out feature enabled
                            m_Timer.Interval = m_Command.TimeOut;
                            m_Timer.Enabled = false;
                            m_Timer.Elapsed += new ElapsedEventHandler(TimeOutHandler);
                            break;
                    }
                }
            }
            else
            {
                //! mark this as an invalid telegraph
                CancelTelegraph();
            }
        }

        //! timeout handle routine
        private void TimeOutHandler(System.Object sender, ElapsedEventArgs e)
        {
            if (null != m_Timer)
            {
                //! Disable timer
                lock (m_Timer)
                {
                    try
                    {
                        if (null != m_Timer)
                        {
                            m_Timer.Stop();
                            m_Timer.Enabled = false;
                            m_Timer.Dispose();
                            m_Timer = null;
                        }
                    }
                    catch (Exception) { }
                }
            }
            //! raising time out event
            OnTimeOut();
        }

        //! a event for all kinds of events
        public event SinglePhaseTelegraphEventHandler SinglePhaseTelegraphEvent;

        //! method for raising time out event
        protected virtual void OnTimeOut()
        {
            //LogWriter.WriteLine("Timeout!");

            if (m_IfDisposed)
            {
                return;
            }
            //Dispose();
            base.OnCancel();

            /*
            if (null == m_Command)
            {
                if (null != SinglePhaseTelegraphEvent)
                {
                    SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR, m_Command);
                }
                //BeginInvoke(SinglePhaseTelegraphEvent, this, BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR, m_Command);
                return;
            }
            */
            //! raising timeout event
            //BeginInvoke(SinglePhaseTelegraphEvent,this, BM_TELEGRAPH_STATE.BM_TELE_RT_TIME_OUT, m_Command);
            
            if (null != SinglePhaseTelegraphEvent)
            {
                SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_TIME_OUT, m_Command);
            }
        }

        //! method for raising cancelled event
        public override void OnCancel()
        {
            //! raising event
            OnTelegrahAccessed();

            if ((m_IfDisposed) || (this.isCancelled))
            {
                return;
            }
            //Dispose();


            if (null == m_Command)
            {
                /*
                BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR,
                    m_Command
                );*/
                if (null != SinglePhaseTelegraphEvent)
                {
#if __ASYN_TELEGRAPH_REPORT__
                    BeginInvoke
                    (
                        SinglePhaseTelegraphEvent,
                        this,
                        BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR,
                        m_Command
                    );
#else
                    SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR, m_Command);
#endif
                }
                base.OnCancel();
                return;
            }
            else if ((UInt16)BM_CMD_RT.BM_CMD_RT_NO_RESPONSE != m_Command.TimeOut)
            {
                /*
                //! raising timeout event
                BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_CANCELLED,
                    m_Command
                );
                */
                if (null != SinglePhaseTelegraphEvent)
                {
#if __ASYN_TELEGRAPH_REPORT__ 
                    BeginInvoke
                    (
                        SinglePhaseTelegraphEvent,
                        this,
                        BM_TELEGRAPH_STATE.BM_TELE_RT_CANCELLED,
                        m_Command
                    );
#else
                    SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_CANCELLED, m_Command);
#endif
                }
            }
            else
            {
                /*
                //! raising success event
                BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS,
                    m_Command
                );
                */
                if (null != SinglePhaseTelegraphEvent)
                {
#if __ASYN_TELEGRAPH_REPORT__
                    BeginInvoke
                    (
                        SinglePhaseTelegraphEvent,
                        this,
                        BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS,
                        m_Command
                    );
#else
                    SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS, m_Command);
#endif
                }
            }
            

            base.OnCancel();
        }


        //! serious system error
        public virtual void OnError()
        {
            if (m_IfDisposed)
            {
                return;
            }

            Dispose();
            /*
            //BatteryManageTelegraphEvent.Invoke(BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR, m_Command);
            BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR,
                    m_Command
                );
            */
            if (null != SinglePhaseTelegraphEvent)
            {
#if __ASYN_TELEGRAPH_REPORT__ 
                BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR,
                    m_Command
                );
#else
                SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR, m_Command);
#endif
            }
            base.OnCancel();
        }

        //! serious system error
        public virtual void OnError(BM_TELEGRAPH_STATE ErrorState)
        {
            if (m_IfDisposed)
            {
                return;
            }

            Dispose();
            //BatteryManageTelegraphEvent.Invoke(ErrorState, m_Command);
            /*
            BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    ErrorState,
                    m_Command
                );
            */
            if (null != SinglePhaseTelegraphEvent)
            {
#if __ASYN_TELEGRAPH_REPORT__ 
                BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    ErrorState,
                    m_Command
                );
#else
                SinglePhaseTelegraphEvent.Invoke(this, ErrorState, m_Command);
#endif
            }
            base.OnCancel();
        }

        //! method for raising decoding success event
        protected virtual void OnDecoderSuccess(ESCommand ReceivedCommand)
        {
            //! raising event
            OnTelegrahAccessed();

            if (m_IfDisposed)
            {
                return;
            }

            
            //BatteryManageTelegraphEvent.Invoke(BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS, ReceivedCommand);
            /*
            BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS,
                    ReceivedCommand
                );
            */
            if (null != SinglePhaseTelegraphEvent)
            {
#if __ASYN_TELEGRAPH_REPORT__ 
                BeginInvoke
                (
                    SinglePhaseTelegraphEvent,
                    this,
                    BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS,
                    ReceivedCommand
                );
#else
                SinglePhaseTelegraphEvent.Invoke(this, BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS, ReceivedCommand);
#endif
            }
            
            Dispose();
        }



        //! property for get telegraph description
        public override String Description
        {
            get
            {
                return m_Command.Description;
            }
            set { ;}
        }

        //! create a telegraph engine for this telegraph
        public override TelegraphEngine CreateTelegraphEngine()
        {
            return new SinglePhaseTelegraphEngine(null);
        }

        public override String EngineType
        {
            get { return "Single Phase Telegraph Engine"; }
        }


        public override object Target
        {
            get { return m_Command; }
        }

        protected override void _Dispose()
        {
            if (null != m_Timer)
            {
                try
                {
                    m_Timer.Elapsed -= new ElapsedEventHandler(TimeOutHandler);
                    m_Timer.Dispose();
                    m_Timer = null;
                }
                catch (Exception) { }
            }

            if (null != m_Command)
            {
                m_Command.Dispose();
            }
        }
    }
    // @}
    
}
