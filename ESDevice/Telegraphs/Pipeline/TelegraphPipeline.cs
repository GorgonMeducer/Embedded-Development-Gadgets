using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ESnail.Utilities.Threading;
using ESnail.Device;
using ESnail.Utilities;

namespace ESnail.Device.Telegraphs.Pipeline
{
    //! \name Telegraph pipeline
    //! @{
    public class TelegraphPipeline : PipelineCore
    {
        //! \brief always return true
        public override Boolean Available
        {
            get {  return true; }
        }

        //! \brief method for releasing managed objects
        protected override void _Dispose()
        {
            //! add code here
        }

        public override Boolean AddService(PipelineCoreService ServiceItem)
        {
            return AddService(ServiceItem as TelegraphService);
        }

        //! \brief method for adding telegraph serivces to pipeline core
        public Boolean AddService(TelegraphService tService)
        {
            if (null == tService)
            {
                return false;
            }

           
            return base.AddService(tService as TelegraphService);
        }
    }
    //! @}

    //! \name telegraph service
    //! @{
    public abstract class TelegraphService : PipelineCoreService
    {
        protected List<Telegraph> m_TelegraphList = new List<Telegraph>();
        //private SafeInvoker m_Invoker = new SafeInvoker();

        //! \brief constructor with single telegraph
        public TelegraphService(Telegraph tTelegraph)
            : base(new Telegraph[1] { tTelegraph })
        {
            if (null == tTelegraph)
            {
                m_Available = false;
            }

            Initialize();
        }

        //! \brief constructor with multiple telegraphs
        public TelegraphService(Telegraph[] tTelegraphs)
            : base(tTelegraphs)
        {
            if (null != tTelegraphs)
            {
                if (0 == tTelegraphs.Length)
                {
                    m_Available = false;
                }
            }

            Initialize();
        }

        //! \brief constructor with single telegraph
        public TelegraphService(Telegraph tTelegraph, System.Int32 tTimeOut)
            : base(new Telegraph[1] { tTelegraph },tTimeOut)
        {
            if (null == tTelegraph)
            {
                m_Available = false;
            }

            Initialize();
        }

        //! \brief constructor with multiple telegraphs
        public TelegraphService(Telegraph[] tTelegraphs, System.Int32 tTimeOut)
            : base(tTelegraphs, tTimeOut)
        {
            if (null != tTelegraphs)
            {
                if (0 == tTelegraphs.Length)
                {
                    m_Available = false;
                }
            }

            Initialize();
        }


        private TelegraphCanceller m_TelegraphServiceCanceller = new TelegraphCanceller();

        //! \brief initialize this object
        private void Initialize()
        {
            Telegraph[] tTelegraphs = Tag as Telegraph[];
            if (null == tTelegraphs)
            {
                return;
            }

            if (!Available)
            {
                return;
            }

            foreach (Telegraph tTelegraphItem in tTelegraphs)
            {
                //! register canceller
                tTelegraphItem.RegisterCancel(ref m_TelegraphServiceCanceller);
                tTelegraphItem.TelegrahAccessedEvent += new TelegrahAccessed(tTelegraphItem_TelegrahAccessedEvent);
                m_TelegraphList.Add(tTelegraphItem);
            }
        }

        private void tTelegraphItem_TelegrahAccessedEvent(Telegraph tTelegraph)
        {
            tTelegraph.TelegrahAccessedEvent -= new TelegrahAccessed(tTelegraphItem_TelegrahAccessedEvent);
            lock (((ICollection)m_TelegraphList).SyncRoot)
            {
                m_TelegraphList.Remove(tTelegraph);
            }
        }

        protected void OnCancelAllPendingTelegraphs()
        {
            lock (m_TelegraphServiceCanceller)
            {
                m_TelegraphServiceCanceller.OnCancelTelegraph();
            }
        }



    }
    //! @}
}
