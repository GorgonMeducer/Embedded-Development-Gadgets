using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ESnail.Device.Adapters;

namespace ESnail.Device.Telegraphs.Engines
{
    public class SinglePhaseTelegraphEngine : TelegraphEngine, ISPTelegraph
    {
        private Queue<SinglePhaseTelegraph> m_qTransmit = new Queue<SinglePhaseTelegraph>();
        private Queue<System.Byte> m_qReceive = new Queue<byte>();
        private SingleDeviceAdapter m_Adapter = null;
        private ManualResetEvent m_CompleteingSignal = new ManualResetEvent(false);
        private ManualResetEvent m_StartSignal = new ManualResetEvent(false);

        //! constructor
        public SinglePhaseTelegraphEngine(SingleDeviceAdapter DeviceInterface)
        {
            m_Adapter = DeviceInterface;
        }

        //! get type
        public override System.String Type
        {
            get { return "Single Phase Telegraph Engine"; }
        }

        //! propery parentadatper
        public override Adapter ParentAdapter
        {
            get
            {
                return m_Adapter;
            }
            set
            {
                m_Adapter = value as SingleDeviceAdapter;
            }
        }

        public ManualResetEvent CompleteSignal
        {
            get { return m_CompleteingSignal; }
        }

        public ManualResetEvent StartSignal
        {
            get { return m_StartSignal; }
        }

        //! communication thread
        protected override void DoCommunication()
        {
            m_StartSignal.Set();

            while (null == m_Adapter)
            {
                m_StopRequest.WaitOne();

                m_CompleteingSignal.Set();
                return;
            }

            SinglePhaseTelegraph telTemp = null;
            Byte[] temBuffer = null;
            List<SinglePhaseTelegraph> WaitReplyList = new List<SinglePhaseTelegraph>();
            //List<SinglePhaseTelegraph>.Enumerator ListenerEnum = WaitReplyList.GetEnumerator();
            Int32 tWaitListIndex = 0;
            Queue<Byte> qReceiveBuffer = new Queue<byte>();
            Byte[] tempReceiveBuffer = new Byte[256];

            //! internal task send telegraph
            Boolean FSM_GET_QUEUE_ITEM = true;
            Boolean FSM_ENCODE = false;
            Boolean FSM_TRANSMIT_DATA = false;
            Boolean FSM_RECEIVE_DATA = false;

            //! interanl task receive telegraph
            Boolean FSM_GET_LIST_ENUM = true;
            Boolean FSM_GET_LIST_ITEM = false;
            Boolean FSM_GET_NEXT_ITEM = false;
            Boolean FSM_DECODE = false;

            Boolean m_StopMessageSent = false;

            SinglePhaseTelegraph teleReceive = null;

            Boolean IsNoPendingTranmitItem = false;
            Boolean IsNoPendingReceiveItem = true;
            Int32 ReadingFailedCounter = 0;
            
            if (m_Adapter.Open)
            {
                //! clean receive buffer
                while (m_Adapter.ReadDevice(ref tempReceiveBuffer, "All Dropped for flushing buffer")) ;
            }
            
            while (true)
            {
                if (m_StopRequest.WaitOne(0, true))
                {
                    m_CompleteingSignal.Set();
                    OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_STOPED);
                    return;                    
                }

                while (!IsNoPendingTranmitItem)
                {
                    //! get item from telegraph queue
                    if (FSM_GET_QUEUE_ITEM)
                    {
                        //! get telegraph from queue
                        lock (((ICollection)m_qTransmit).SyncRoot)
                        {
                            if (0 != m_qTransmit.Count)
                            {
                                //! do next loop

                                telTemp = m_qTransmit.Dequeue();
                                FSM_GET_QUEUE_ITEM = false;
                                FSM_ENCODE = true;
                                IsNoPendingTranmitItem = false;
                                m_StopMessageSent = false;
                            }
                            else
                            {
                                IsNoPendingTranmitItem = true;
                            }
                        }
                    }

                    //! after get queue item, try to decoding
                    if (FSM_ENCODE)
                    {
                        lock (telTemp)
                        {
                            
                            //! try to encode
                            temBuffer = telTemp.Encode();

                            if (null == temBuffer)
                            {
                                /*! nothing to transmit, directly check if this 
                                 *  telegraph was cancelled and receive data
                                 */
                                FSM_RECEIVE_DATA = true;
                            }
                            else
                            {
                                //! try to transmit
                                FSM_TRANSMIT_DATA = true;
                            }

                            FSM_ENCODE = false;
                        }
                    }

                    //! transmit data to device
                    if (FSM_TRANSMIT_DATA)
                    {
                        if (m_Adapter.Open)
                        {
                            if (m_Adapter.WriteDeviceNoDebug(temBuffer))
                            {
                                //! data transmitted
                                FSM_RECEIVE_DATA = true;
                                //! rasing on communication event
                                m_Adapter.OnCommunication(MSG_DIRECTION.OUTPUT_MSG, temBuffer, telTemp.Description);
                            }
                            else
                            {
                                /*! failed to transmit data, rasing event and this telegraph
                                 *  will be dropped directly.
                                 */
                                telTemp.OnError(BM_TELEGRAPH_STATE.BM_TELE_RT_ERROR_FAILD_TO_WRITE_DEVICE);
                                FSM_GET_QUEUE_ITEM = true;
                            }
                            FSM_TRANSMIT_DATA = false;  //!< reset state
                        }
                        else if (telTemp.isCancelled)
                        {
                            //! raising cancel event
                            telTemp.OnCancel();

                            FSM_TRANSMIT_DATA = false;
                            FSM_GET_QUEUE_ITEM = true;
                        }
                    }

                    if (FSM_RECEIVE_DATA)
                    {
                        //! check whether this telegraph was already cancelled.
                        if (telTemp.isCancelled)
                        {
                            //! raising cancel event
                            telTemp.OnCancel();
                        }
                        else
                        {                            
                            //! add this telegraph to wait reply list
                            WaitReplyList.Add(telTemp);
                        }

                        FSM_RECEIVE_DATA = false;
                        FSM_GET_QUEUE_ITEM = true;      //!< reset state
                    }
                }

                Boolean bDropFlag = true;
                

                //! get a enum from list
                if (FSM_GET_LIST_ENUM)
                {
                    if (WaitReplyList.Count > 0)
                    {
                        //! create a copy of list
#if false
                        ListenerEnum = WaitReplyList.GetEnumerator();

                        tWaitListIndex = 0;

                        if (ListenerEnum.MoveNext())
                        {
                            IsNoPendingReceiveItem = false;
                            m_StopMessageSent = false;

                            FSM_GET_LIST_ENUM = false;
                            FSM_GET_LIST_ITEM = true;
                        }
                        else
                        {
                            ListenerEnum.Dispose();
                            IsNoPendingReceiveItem = true;
                        }
#else
                        tWaitListIndex = 0;

                        IsNoPendingReceiveItem = false;
                        m_StopMessageSent = false;

                        FSM_GET_LIST_ENUM = false;
                        FSM_GET_LIST_ITEM = true;
#endif

                    }
                    else
                    {
                        IsNoPendingReceiveItem = true;
                    }
                }

                

                //! get a item from list
                if (FSM_GET_LIST_ITEM)
                {
#if false
                    if (ListenerEnum.Current.isCancelled)
                    {
                        //! illegal list item

                        //! raising cancel event
                        ListenerEnum.Current.OnCancel();

                        //! remove current item
                        WaitReplyList.Remove(ListenerEnum.Current);

                        FSM_GET_NEXT_ITEM = true;
                    }
                    else
                    {
                        //! get a item
                        teleReceive = ListenerEnum.Current;

                        //! try to decode
                        FSM_DECODE = true;
                    }
#else
                    if (WaitReplyList[tWaitListIndex].isCancelled)
                    {
                        //! illegal list item

                        //! raising cancel event
                        WaitReplyList[tWaitListIndex].OnCancel();

                        //! remove current item
                        WaitReplyList.RemoveAt(tWaitListIndex);
                        tWaitListIndex--;
                        FSM_GET_NEXT_ITEM = true;
                    }
                    else
                    {
                        //! get a item
                        teleReceive = WaitReplyList[tWaitListIndex];

                        //! try to decode
                        FSM_DECODE = true;
                    }
#endif
                    FSM_GET_LIST_ITEM = false;
                }

                //! check if there any input data
                if (!IsNoPendingReceiveItem)
                {
                    if (m_Adapter.Open)
                    {
                        if (m_Adapter.ReadDeviceNoDebug(ref tempReceiveBuffer))
                        {
                            ReadingFailedCounter = 0;
                            //! add data to input buffer
                            foreach (Byte tValue in tempReceiveBuffer)
                            {
                                qReceiveBuffer.Enqueue(tValue);
                            }
                            //Thread.Sleep(1);
                        }
                        else
                        {
                            ReadingFailedCounter++;
                            if (ReadingFailedCounter > 10)
                            {
                                Thread.Sleep(50);
                            }
                        }
                        
                    }
                }

                //! decode
                if (FSM_DECODE)
                {
                    Byte[] DropData = new Byte[1];

                    //! drop all zeros
                    while (qReceiveBuffer.Count > 0)
                    {
                        if (0 != qReceiveBuffer.Peek())
                        {
                            break;
                        }

                        //! drop one data
                        DropData[0] = qReceiveBuffer.Dequeue();

                        //! raising debug event
                        m_Adapter.OnCommunication(MSG_DIRECTION.INPUT_MSG, DropData, "Dropping Zero");
                    }
                    Boolean tRequestDrop = false;
                    System.Int32 nDequeueSize = teleReceive.Decode(ref qReceiveBuffer, ref tRequestDrop);
                    if (0 < nDequeueSize)
                    {
                        //! decode success
                        System.Byte[] tempReceiveMessage = new System.Byte[nDequeueSize];
                        for (System.Int32 n = 0; n < nDequeueSize; n++)
                        {
                            tempReceiveMessage[n] = qReceiveBuffer.Dequeue();
                        }

                        //! raising debug event
                        m_Adapter.OnCommunication(MSG_DIRECTION.INPUT_MSG, tempReceiveMessage, teleReceive.Description);

                        WaitReplyList.Remove(teleReceive);      //!< remove telegraph from listener list

                        tRequestDrop = false;
                        FSM_GET_LIST_ENUM = true;               //!< reset state machine
                    }
                    else
                    {
                        //! mismatch
                        if (!tRequestDrop)
                        {
                            bDropFlag = false;
                        }

                        FSM_GET_NEXT_ITEM = true;
                    }
                    FSM_DECODE = false;
                }


                //! get next available item
                if (FSM_GET_NEXT_ITEM)
                {
                    /*
                    Boolean result = false;

                    do
                    {
                        try
                        {
                            result = ListenerEnum.MoveNext();
                            break;
                        }
                        catch (InvalidOperationException )
                        {
                            try
                            {
                                ListenerEnum.Dispose();
                            }
                            catch (Exception e2)
                            {
                                e2.ToString();
                            }

                            ListenerEnum = WaitReplyList.GetEnumerator();
                        }
                    }
                    while (true);
                    */
                    tWaitListIndex++;

                    if (tWaitListIndex < WaitReplyList.Count)
                    {
                        //! get next item
                        FSM_GET_LIST_ITEM = true;
                        
                    }
                    else
                    {
                        List<Byte> tDropDataList = new List<Byte>();

                        if (bDropFlag)
                        {
                            if (qReceiveBuffer.Count > 0)
                            {
                                tDropDataList.Add(qReceiveBuffer.Dequeue());
                            }
                        }
                        bDropFlag = true;

                        while (qReceiveBuffer.Count > 0)
                        {
                            if (0 != qReceiveBuffer.Peek())
                            {
                                break;
                            }
                            //! drop one data
                            tDropDataList.Add(qReceiveBuffer.Dequeue());
                        }

                        if (tDropDataList.Count > 0)
                        {
                            //! raising debug event
                            m_Adapter.OnCommunication(MSG_DIRECTION.INPUT_MSG, tDropDataList.ToArray(), "Dropping byte(s)");
                        }
                        /*
                        try
                        {
                            ListenerEnum.Dispose();
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        */
                        FSM_GET_LIST_ENUM = true;
                    }
                    FSM_GET_NEXT_ITEM = false;
                }
                
                if (IsNoPendingTranmitItem && IsNoPendingReceiveItem) 
                {
                    //m_StartSignal.Reset();
                    IsNoPendingTranmitItem = false;
                    if (!m_StopMessageSent)
                    {
                        m_CompleteingSignal.Set();
                        m_StopMessageSent = true;
                        OnEngineStateReport(TELEGRAPH_ENGINE_STATE.ENGINE_STOPED);
                    }

                    if (qReceiveBuffer.Count > 0)
                    {
                        qReceiveBuffer.Clear();
                    }
                }
            }            
        }


        //! try to send single phase telegraph
        public Boolean TryToSendTelegraph(SinglePhaseTelegraph telTarget)
        {
            lock (((ICollection)m_qTransmit).SyncRoot)
            {
                try
                {
                    //! add BatteryManageTelegraph item to queue
                    m_qTransmit.Enqueue(telTarget);
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    return false;
                }
            }

            if (!IsWorking)
            {
                //! re-start engine
                IsWorking = true;
            }

            return true;
        }


        //! try to send single phase telegraph
        public Boolean TryToSendTelegraphs(SinglePhaseTelegraph[] telTargets)
        {
            if (null == telTargets)
            {
                return false;
            }

            if (0 == telTargets.Length)
            { 
                return true;
            }

            lock (((ICollection)m_qTransmit).SyncRoot)
            {                
                try
                {
                    foreach (SinglePhaseTelegraph telItem in telTargets)
                    {
                        //! add BatteryManageTelegraph item to queue
                        m_qTransmit.Enqueue(telItem);
                    }
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.ToString());
                    return false;
                }
                
            }

            if (!IsWorking)
            {
                //! re-start engine
                IsWorking = true;
            }

            return true;
        }

        public override System.Boolean TryToSendTelegraphs(Telegraph[] telTagets)
        {
            
            List<SinglePhaseTelegraph> tList = new List<SinglePhaseTelegraph>();
            if (null == telTagets)
            {
                return false;
            }

            foreach (Telegraph tTelegraph in telTagets)
            {
                SinglePhaseTelegraph tTelegraphItem = tTelegraph as SinglePhaseTelegraph;
                if (null != tTelegraphItem)
                {
                    tList.Add(tTelegraphItem);
                }
                else
                {
                    tTelegraph.OnCancel();
                }
            }

            return TryToSendTelegraphs(tList.ToArray());
        }

        //! try to send telegraph(single phase)
        public override System.Boolean TryToSendTelegraph(Telegraph telTarget)
        {
            SinglePhaseTelegraph tempTelegraph = telTarget as SinglePhaseTelegraph;

            return TryToSendTelegraph(tempTelegraph);
        }

        
    }


    public class SinglePhaseTelegraphAdapter : ISPTelegraph, IDisposable
    {
        private TelegraphEngine m_Engine = null;
        private Queue<Telegraph[]> m_TelegrapGroupQueue = new Queue<Telegraph[]>();
        private System.Boolean m_IsWorking = false;
        private ManualResetEvent m_WaitTelegraphGroup = new ManualResetEvent(false);
        private AutoResetEvent m_RequestStop = new AutoResetEvent(false);
        private WaitHandle[] m_WaitHandlers = new WaitHandle[2];
        private Adapter m_Adapter = null;
        private Boolean m_Available = false;

        public SinglePhaseTelegraphAdapter(Adapter tAdapter)
        {
            m_Adapter = tAdapter;

            if (null == m_Adapter)
            {
                return;
            }

            m_WaitHandlers[0] = m_WaitTelegraphGroup;
            m_WaitHandlers[1] = m_RequestStop;

            m_Available = true;
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        public System.Boolean IsWorking
        {
            get { return m_IsWorking; }
            set
            {
                if (!m_Available)
                {
                    return;
                }
                if (value)
                {
                    if (m_IsWorking)
                    {
                        return;
                    }

                    FetchNextTelegraphGroup();
                }
                else
                {
                    //m_IsWorking = false;
                    m_RequestStop.Set();
                }
            }
        }

        private void FetchNextTelegraphGroup()
        {
            Object tObj = new Object();

            lock (tObj)
            {
                Telegraph[] tTelegraphGroup = null;
                TelegraphEngine tEngine = null;

                do
                {

                    lock (((ICollection)m_TelegrapGroupQueue).SyncRoot)
                    {
                        if (m_TelegrapGroupQueue.Count > 0)
                        {
                            tTelegraphGroup = m_TelegrapGroupQueue.Dequeue();
                            if (null == tTelegraphGroup)
                            {
                                continue;
                            }
                            if (0 == tTelegraphGroup.Length)
                            {
                                continue;
                            }

                            tEngine = tTelegraphGroup[0].CreateTelegraphEngine();
                            if (null == tEngine)
                            {
                                continue;
                            }
                            tEngine.ParentAdapter = m_Adapter;
                            tEngine.Priority = ThreadPriority.Normal;

                            m_WaitTelegraphGroup.Reset();
                            break;
                        }
                        else
                        {
                            if (null != m_Adapter)
                            {
                                m_Adapter.WriteLogLine("Adapter enter idle state.");
                            }
                        }
                    }

                    switch (WaitHandle.WaitAny(m_WaitHandlers))
                    {
                        case 0:
                            break;
                        case 1:
                            m_IsWorking = false;
                            m_Engine.IsWorking = false;
                            m_Engine.EngineStateReportEvent -= new EngineStateReport(EngineStateReportEventHandler);
                            m_Engine = null;
                            return;
                    }

                }
                while (true);


                if (null == m_Engine)
                {
                    m_Engine = tEngine;
                    m_Engine.EngineStateReportEvent += new EngineStateReport(EngineStateReportEventHandler);
                }
                else
                {
                    if (m_Engine.Type != tEngine.Type)
                    {
                        m_Engine.EngineStateReportEvent -= new EngineStateReport(EngineStateReportEventHandler);
                        m_Engine.Dispose();
                        if (null != m_Adapter)
                        {
                            m_Adapter.WriteLogLine("Telegraph engine changed.");
                        }
                        m_Engine = tEngine;
                    }
                }

                m_Engine.TryToSendTelegraphs(tTelegraphGroup);
                m_IsWorking = true;
                if (null != m_Adapter)
                {
                    m_Adapter.WriteLogLine("Adapter enter working state.");
                }
                m_Engine.IsWorking = true;

            }
        }

        private void EngineStateReportEventHandler(TELEGRAPH_ENGINE_STATE State, TelegraphEngine EngineItem)
        {
            switch (State)
            {
                case TELEGRAPH_ENGINE_STATE.ENGINE_DISPOSED:
                    if (null != m_Adapter)
                    {
                        m_Adapter.WriteLogLine("Telegraph engine disposed.");
                    }
                    if (EngineItem == m_Engine)
                    {
                        m_Engine.EngineStateReportEvent -= new EngineStateReport(EngineStateReportEventHandler);
                        m_Engine = null;
                    }
                    break;

                case TELEGRAPH_ENGINE_STATE.ENGINE_STOPED:
                    if (null != m_Adapter)
                    {
                        m_Adapter.WriteLogLine("Telegrapp engine stopped. Try to fetch next telegraph group.");
                    }
                    //Thread.Sleep(5);
                    FetchNextTelegraphGroup();
                    break;
                case TELEGRAPH_ENGINE_STATE.ENGINE_START:
                    if (null != m_Adapter)
                    {
                        m_Adapter.WriteLogLine("Telegraph engine get start.");
                    }
                    break;
                case TELEGRAPH_ENGINE_STATE.ENGINE_STOPPING:
                    if (null != m_Adapter)
                    {
                        m_Adapter.WriteLogLine("Stopping Telegraph engine...");
                    }
                    break;
                case TELEGRAPH_ENGINE_STATE.ENGINE_WORKING:
                    break;
            }
        }

        //! implement interface IBMTelegraph : TryToSendTelegraph
        public System.Boolean TryToSendTelegraph(SinglePhaseTelegraph telTarget)
        {
            if (null == telTarget)
            {
                return false;
            }

            if (!m_Available)
            {
                return false;
            }

            if (null != m_Adapter)
            {
                m_Adapter.WriteLogLine("Try to send telegraph.");
            }

            /*
            SinglePhaseTelegraphService tService = new SinglePhaseTelegraphService(telTarget, m_PipelServiceTimeOut, this);

            if (m_Pipeline.AddService(tService))
            {
                //tService.StartSignal.WaitOne();
                return true;
            }
            */
            lock (((ICollection)m_TelegrapGroupQueue).SyncRoot)
            {
                m_TelegrapGroupQueue.Enqueue(new SinglePhaseTelegraph[1] { telTarget });
                m_WaitTelegraphGroup.Set();
            }

            this.IsWorking = true;

            return true;
        }


        public System.Boolean TryToSendTelegraphs(Telegraph[] telTargets)
        {
            if (null == telTargets)
            {
                return false;
            }
            if (0 == telTargets.Length)
            {
                return true;
            }

            if (!m_Available)
            {
                return false;
            }

            if (null != m_Adapter)
            {
                m_Adapter.WriteLogLine("Try to send multi-telegraphs.");
            }


            /*
            SinglePhaseTelegraphService tService = null;
            do
            {
                List<SinglePhaseTelegraph> tList = new List<SinglePhaseTelegraph>();

                foreach (Telegraph tTelegraph in telTargets)
                {
                    if (tTelegraph is SinglePhaseTelegraph)
                    {
                        tList.Add(tTelegraph as SinglePhaseTelegraph);
                    }
                }

                tService = new SinglePhaseTelegraphService(tList.ToArray(), m_PipelServiceTimeOut, this);
            }
            while (false);
            
            if (m_Pipeline.AddService(tService))
            {
                //tService.StartSignal.WaitOne();
                return true;
            }
            */


            lock (((ICollection)m_TelegrapGroupQueue).SyncRoot)
            {
                m_TelegrapGroupQueue.Enqueue(telTargets);
                m_WaitTelegraphGroup.Set();
            }

            this.IsWorking = true;


            return true;
        }


        //! implement interface ITelegraph : TryToSendTelegraph
        public Boolean TryToSendTelegraph(Telegraph telTarget)
        {
            return TryToSendTelegraph(telTarget as SinglePhaseTelegraph);
        }


        #region dispose
        ~SinglePhaseTelegraphAdapter()
        {
            Dispose();
        }

        
        private Boolean m_bDisposed = false;
        
        public Boolean Disposed
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
                    if (null != m_Engine)
                    {
                        m_Engine.EngineStateReportEvent -= new EngineStateReport(EngineStateReportEventHandler);
                        m_Engine.Dispose();
                        m_Engine = null;
                    }
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }

                GC.SuppressFinalize(this);
            }
        }
        #endregion
    }
}
