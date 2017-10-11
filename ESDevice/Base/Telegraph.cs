using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using ESnail.Utilities.Threading;
using System.Threading;

namespace ESnail.Device
{

    //! \name Interface: ITelegraph
    //! @{
    public interface ITelegraph
    {
        System.Boolean TryToSendTelegraph(Telegraph telTarget);             //!< try to send a single telegraph
        System.Boolean TryToSendTelegraphs(Telegraph[] telTagets);          //!< try to send a set of telegraphs
    }
    //! @}

    //! a delegate type for cancelling a pending telegraph
    internal delegate void _TelegraphCanceller();

    public delegate void TelegrahAccessed(Telegraph tTelegraph);

    public class TelegraphCanceller
    {
        internal event _TelegraphCanceller TelegraphCancellingEvent;

        public void OnCancelTelegraph()
        {
            if (null != TelegraphCancellingEvent)
            {
                try
                {
                    TelegraphCancellingEvent.Invoke();
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }
    }

    //! /name Abstruct class for telegraph
    public abstract class Telegraph : IDisposable
    {
        private Boolean m_Cancelled = false;
        private TelegraphCanceller m_Canceller = null;
        //private DispatcherContainer m_DispatherItem = new DispatcherContainer();
        private SafeInvoker m_Invoker = new SafeInvoker();
        private ManualResetEvent m_CompleteSignal = new ManualResetEvent(false);

        protected void CancelTelegraph()
        {
            m_Cancelled = true;
            if (null != m_Canceller)
            {
                lock (m_Canceller)
                {
                    m_Canceller.TelegraphCancellingEvent -= new _TelegraphCanceller(CancelTelegraph);
                }
            }
        }
        
        protected void BeginInvoke(Delegate Method, params Object[] Args)
        {
            m_Invoker.BeginInvoke(Method, Args);
        }
        
        //! a method for register a telegraph canceller
        public Boolean RegisterCancel(ref TelegraphCanceller tCanceller)
        {
            if ((null != tCanceller) && (!m_Cancelled))
            {
                if (null == m_Canceller)
                {
                    tCanceller.TelegraphCancellingEvent += new _TelegraphCanceller(CancelTelegraph);
                    m_Canceller = tCanceller;
                }
                else
                {
                    lock (m_Canceller)
                    {
                        tCanceller.TelegraphCancellingEvent += new _TelegraphCanceller(CancelTelegraph);
                        m_Canceller = tCanceller;
                    }
                }
                return true;
            }

            return false;
        }

        public abstract Telegraph CreateTelegraph(params Object[] Args);

        //! property for check if the Telegraph was cancelled
        public System.Boolean isCancelled
        {
            get { return m_Cancelled; }
        }

        //! encoder result
        public abstract Byte[] Encode();

        //! decoder method
        public abstract Int32 Decode(ref Queue<System.Byte> InputQueue, ref Boolean tRequestDrop);

        //! cancel these telegraph
        public virtual void OnCancel()
        {
            CancelTelegraph();
        }

        //! propery
        public abstract String Description
        {
            get;
            set;
        }

        public abstract String Type
        {
            get;
        }

        //! create a telegraph engine for this telegraph
        public virtual TelegraphEngine CreateTelegraphEngine()
        {
            return null;
        }

        public event TelegrahAccessed TelegrahAccessedEvent;

        protected void OnTelegrahAccessed()
        {
            //m_Invoker.BeginInvoke(TelegrahAccessedEvent, this);
            if (null != TelegrahAccessedEvent)
            {
                TelegrahAccessedEvent.Invoke(this);
            }
            m_CompleteSignal.Set();
        }

        public ManualResetEvent CompleteSignal
        {
            get { return m_CompleteSignal; }
        }

        public abstract System.String EngineType
        {
            get;
        }

        public abstract Telegraph GetTestTelegraph();

        public abstract Object Target
        {
            get;
        }

        public Object Tag
        {
            get;
            set;
        }

        #region IDisposable Members

        private System.Boolean m_Disposed = false;

        public System.Boolean Disposed
        {
            get { return m_Disposed; }
        }

        public void Dispose()
        {
            if (!m_Disposed)
            {
                m_Disposed = true;

                //CancelTelegraph();

                try
                {
                    _Dispose();
                }
                catch (Exception )
                {
                }
                

                GC.SuppressFinalize(this);
            }
        }

        protected abstract void _Dispose();

        #endregion
    }
}