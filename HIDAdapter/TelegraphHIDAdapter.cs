using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ESnail.Device;
using ESnail.Device.Adapters;
using ESnail.Device.Telegraphs;
using ESnail.Device.Telegraphs.Engines;
using ESnail.Device.Telegraphs.Pipeline;
using ESnail.Utilities;
using System.Windows.Forms;
using System.Threading;
using ESnail.CommunicationSet.Commands;
using System.ComponentModel;

namespace ESnail.Device.Adapters.USB.HID
{
    //! \name telegraph based HID adapter
    //! @{
    public partial class TelegraphHIDAdapter : USBHIDAdapter, ISPTelegraph 
    {
        //private SinglePhaseTelegraphEngine m_TelegraphEngine = null;    //!< engine
        private ESnailHIDAgent m_Control = null;               //!< default control
        private frmTelegraphHIDAdapterEditor m_Editor = null;           //!< default editor
        //private TelegraphPipeline m_Pipeline = new TelegraphPipeline();        
        //private System.Int32 m_PipelServiceTimeOut = 100;
        private SinglePhaseTelegraphAdapter m_TelegraphAdapter = null;

        //! constructor
        public TelegraphHIDAdapter(SafeID tID)
            : base(tID)
        {
            Initiliaze();
        }

        private void Initiliaze()
        {
            
            //RegisterSupportTelegraph(new XBatteryTelegraph(null));
            //
            //RegisterSupportTelegraph(new SmartBatteryTelegraph(null));
            RegisterSupportTelegraph(new UserTelegraph(null));
            RegisterSupportTelegraph(new BatteryManagementTelegraph(null));
            m_TelegraphAdapter = new SinglePhaseTelegraphAdapter(this);
        }

        //! distructor
        ~TelegraphHIDAdapter()
        {
            //! dispose
            Dispose();
        }

        //! Adapter type
        public override String Type
        {
            get { return "Telegraph-based USB HID-Compliant Adapter"; }
        }


        public override Boolean IsWorking
        {
            get 
            {
                if (null == m_TelegraphAdapter)
                {
                    return false;
                }
                return m_TelegraphAdapter.IsWorking; 
            }
            set
            {
                if (null != m_TelegraphAdapter)
                {
                    m_TelegraphAdapter.IsWorking = value;
                }
            }
        }

        protected override void _Dispose()
        {
            /*
            //! dispose telegraph engine
            if (null != m_Pipeline)
            {
                m_Pipeline.Dispose();
                m_Pipeline = null;
            }
            */
            //! dipose engine
            if (null != m_TelegraphAdapter)
            {
                m_TelegraphAdapter.Dispose();
                m_TelegraphAdapter = null;
            }

            //! dispose default control
            if (null != m_Control)
            {
                m_Control.Disposed -= new EventHandler(DefaultControlDisposedEventHandler);
                m_Control.Dispose();
                m_Control = null;
            }

            //! dispose default editor
            if (null != m_Editor)
            {
                m_Editor.Disposed -= new EventHandler(DefaultEditorDisposedEventHandler);
                m_Editor.Dispose();
                m_Editor = null;
            }
            
            base._Dispose();
        }

        //! implement interface IBMTelegraph : TryToSendTelegraph
        public Boolean TryToSendTelegraph(SinglePhaseTelegraph telTarget)
        {
            return m_TelegraphAdapter.TryToSendTelegraph(telTarget);
        }


        public Boolean TryToSendTelegraphs(Telegraph[] telTargets)
        {
            return m_TelegraphAdapter.TryToSendTelegraphs(telTargets);
        }


        //! implement interface ITelegraph : TryToSendTelegraph
        public Boolean TryToSendTelegraph(Telegraph telTarget)
        {
            return TryToSendTelegraph(telTarget as SinglePhaseTelegraph);
        }


        //! property for checking whether system is busy
        public override Boolean IsBusy
        {
            get { return false; }
        }

        //! \brief property for getting adapter information page
        public override TabPage InformationPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphHIDAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(DefaultEditorDisposedEventHandler);
                    m_Editor.InformationPage.Disposed += new EventHandler(PagesDisposedEventHandler);
                }
                return m_Editor.InformationPage;
            }
        }

        void PagesDisposedEventHandler(object sender, EventArgs e)
        {
            if (null != m_Editor)
            {
                m_Editor.Disposed -= new EventHandler(DefaultEditorDisposedEventHandler);
                m_Editor.InformationPage.Disposed -= new EventHandler(PagesDisposedEventHandler);
                m_Editor.CommunicationPage.Disposed -= new EventHandler(PagesDisposedEventHandler);
                m_Editor.DebugPage.Disposed -= new EventHandler(PagesDisposedEventHandler);
                m_Editor.DeviceManagerPage.Disposed -= new EventHandler(PagesDisposedEventHandler);

                m_Editor.Dispose();
                m_Editor = null;
            }
        }

        //! \brief property for getting adapter communication page
        public override TabPage CommunicationPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphHIDAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(DefaultEditorDisposedEventHandler);
                    m_Editor.CommunicationPage.Disposed += new EventHandler(PagesDisposedEventHandler);
                }
                return m_Editor.CommunicationPage;
            }
        }

        //! \brief property for getting adapter debug page
        public override TabPage DebugPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphHIDAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(DefaultEditorDisposedEventHandler);
                    m_Editor.DebugPage.Disposed += new EventHandler(PagesDisposedEventHandler);
                }
                return m_Editor.DebugPage;
            }
        }

        //! \brief property for getting adapter device manager page
        public override TabPage DeviceManagerPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphHIDAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(DefaultEditorDisposedEventHandler);
                    m_Editor.DeviceManagerPage.Disposed += new EventHandler(PagesDisposedEventHandler);
                }
                return m_Editor.DeviceManagerPage;
            }
        }

        
        //! brief create a new ATBatteryManageHIDAgent object
        public override System.ComponentModel.Component CreateComponent()
        {
            return new ESnailHIDAgent(this);
        }

        //! \brief get default ATBatteryManageHIDAgent control
        public override System.ComponentModel.Component DefaultComponent
        {
            get
            {
                if (null == m_Control)
                {
                    m_Control = new ESnailHIDAgent(this);
                    m_Control.Disposed += new EventHandler(DefaultControlDisposedEventHandler);
                }

                return m_Control;
            }
        }

        //! \brief default control disposed event handler
        private void DefaultControlDisposedEventHandler(object sender, EventArgs e)
        {
            m_Control.Disposed -= new EventHandler(DefaultControlDisposedEventHandler);
            m_Control = null;
        }

        //! \brief create a new editor for this adapter
        public override Form CreateEditor()
        {
            return new frmTelegraphHIDAdapterEditor(this);
        }


        //! \brief property for getting adapter editor
        public override Form Editor
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphHIDAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(DefaultEditorDisposedEventHandler);
                }
                return m_Editor;
            }
        }

        //! \brief default editor disposed event handler
        private void DefaultEditorDisposedEventHandler(object sender, EventArgs e)
        {
            m_Editor.Disposed -= new EventHandler(DefaultEditorDisposedEventHandler);
            m_Editor.InformationPage.Disposed -= new EventHandler(PagesDisposedEventHandler);
            m_Editor.CommunicationPage.Disposed -= new EventHandler(PagesDisposedEventHandler);
            m_Editor.DebugPage.Disposed -= new EventHandler(PagesDisposedEventHandler);
            m_Editor.DeviceManagerPage.Disposed -= new EventHandler(PagesDisposedEventHandler);

            m_Editor = null;
        }


        public override Adapter CreateAdapter(SafeID tID)
        {
            return new TelegraphHIDAdapter(tID);
        }

        
        public override Boolean AutoDetectDeviceTelegraph()
        {
            List<SinglePhaseTelegraph> tTestingTelegraphsList = new List<SinglePhaseTelegraph>();

            if (!Open)
            {
                return false;
            }
            if (this.IsBusy)
            {
                return false;
            }

            foreach (Telegraph tTelegraph in m_SupportTelegraphList)
            { 
                if (null == tTelegraph)
                {
                    continue;
                }
                SinglePhaseTelegraph tTelegraphItem = tTelegraph.GetTestTelegraph() as SinglePhaseTelegraph;
                if (null != tTelegraphItem)
                {
                    tTestingTelegraphsList.Add(tTelegraphItem);
                }
            }

            if (0 == tTestingTelegraphsList.Count)
            {
                return false;
            }

            foreach (SinglePhaseTelegraph tTelegraph in tTestingTelegraphsList)
            {
                tTelegraph.SinglePhaseTelegraphEvent += new SinglePhaseTelegraphEventHandler(TestingSinglePhaseTelegraphEvent);
            }

            this.TryToSendTelegraphs(tTestingTelegraphsList.ToArray());

            return true;
        }

        private void TestingSinglePhaseTelegraphEvent(SinglePhaseTelegraph tTelegraph, BM_TELEGRAPH_STATE State, ESCommand ReceivedCommand)
        {
            tTelegraph.SinglePhaseTelegraphEvent -= new SinglePhaseTelegraphEventHandler(TestingSinglePhaseTelegraphEvent);

            if (State == BM_TELEGRAPH_STATE.BM_TELE_RT_SUCCESS)
            {
                //! raising event
                OnAdapterAvailableTelegraphAutoDetectionReport
                    (TELEGRAPH_AUTO_DETECT_RESULT.ONE_TELEGRAPH_MATCHED, new String[1] { tTelegraph.Type });
            }
            else
            { 
                //! raising event
                OnAdapterAvailableTelegraphAutoDetectionReport
                    (TELEGRAPH_AUTO_DETECT_RESULT.NO_TELEGRAPH_MATCHED, null);
            }
        }

        //! \brief adapter version
        public override String Version
        {
            get { return "1.0.3.0"; }
        }

        private UInt16 m_PID = 0x2722;
        private UInt16 m_VID = 0xC251;

        internal UInt16 PID
        {
            get { return m_PID; }
            set { m_PID = value; }
        }

        internal UInt16 VID
        {
            get { return m_VID; }
            set { m_VID = value; }
        }
    }
    //! @}

    internal class SinglePhaseTelegraphService : TelegraphService
    {
        SingleDeviceAdapter m_Adapter = null;

        //! \brief constructor with single phase telegraph
        public SinglePhaseTelegraphService(SinglePhaseTelegraph tTelegraph, SingleDeviceAdapter tAdapter)
            : base(tTelegraph)
        {
            m_Adapter = tAdapter;

            Initialize();
        }

        //! \brief constructor with a set of single phase telegraphs
        public SinglePhaseTelegraphService(SinglePhaseTelegraph[] tTelegraphs, SingleDeviceAdapter tAdapter)
            : base(tTelegraphs)
        {
            m_Adapter = tAdapter;

            Initialize();
        }

        
        //! \brief constructor with single phase telegraph
        public SinglePhaseTelegraphService(SinglePhaseTelegraph tTelegraph, System.Int32 tTimeOut, SingleDeviceAdapter tAdapter)
            : base(tTelegraph, tTimeOut)
        {
            m_Adapter = tAdapter;

            Initialize();
        }

        //! \brief constructor with a set of single phase telegraphs
        public SinglePhaseTelegraphService(SinglePhaseTelegraph[] tTelegraphs, System.Int32 tTimeOut, SingleDeviceAdapter tAdapter)
            : base(tTelegraphs, tTimeOut)
        {
            m_Adapter = tAdapter;

            Initialize();
        }
        

        //! \brief initialize this object
        private void Initialize()
        {
            if (null == m_Adapter)
            {
                m_Available = false;
            }
        }


        private System.Boolean FSM_SEND_TELEGRAPHS = true;
        private System.Boolean FSM_WAIT_RESULT = false;
        private SinglePhaseTelegraphEngine m_tEngine = null;
        //private AutoResetEvent m_EngineCompleteSignal = null;

        public override System.Boolean DoService()
        {
            
            if (Cancelled)
            {
                //! recieve cancel request
                OnCancelAllPendingTelegraphs();
                return false;
            }

            if (FSM_SEND_TELEGRAPHS)
            {
                if (null == m_TelegraphList)
                {
                    return false;
                }
                if (0 == m_TelegraphList.Count)
                {
                    return false;
                }

                foreach (Telegraph tTelegraph in m_TelegraphList)
                {
                    SinglePhaseTelegraph tTelegraphItem = tTelegraph as SinglePhaseTelegraph;
                    if (null == tTelegraphItem)
                    {
                        //! cancel this telegraph
                        m_TelegraphList.Remove(tTelegraph);
                        tTelegraph.OnCancel();
                        continue;
                    }
                }

                
                if (null == m_tEngine)
                {
                    m_tEngine = m_TelegraphList[0].CreateTelegraphEngine() as SinglePhaseTelegraphEngine;
                    m_tEngine.ParentAdapter = m_Adapter;
                    m_tEngine.Priority = ThreadPriority.AboveNormal;
                    m_tEngine.EngineStateReportEvent += new EngineStateReport(EngineStateReportEventHandler);
                    m_Adapter.WriteLogLine("New telegraph engine is created.");
                }
                if (null != m_tEngine.CompleteSignal)
                {
                    m_tEngine.CompleteSignal.Reset();
                }

                m_tEngine.TryToSendTelegraphs(m_TelegraphList.ToArray());
 
                if (null != m_tEngine.CompleteSignal)
                {
                    m_tEngine.CompleteSignal.WaitOne();
                }
                else
                {
                    Thread.Sleep(10);
                }

                FSM_SEND_TELEGRAPHS = false;
                FSM_WAIT_RESULT = true;
            }

            if (FSM_WAIT_RESULT)
            {
                do
                {
                    lock (((ICollection)m_TelegraphList).SyncRoot)
                    {
                        if (0 == m_TelegraphList.Count)
                        {
                            if (null != m_tEngine)
                            {
                                try
                                {
                                    m_tEngine.Dispose();
                                    m_tEngine = null;
                                }
                                catch (Exception )
                                {
                                }
                            }
                            
                            //m_tEngine = null;
                            FSM_WAIT_RESULT = false;
                            FSM_SEND_TELEGRAPHS = true;
                            return false;
                        }
                    }

                    if (Cancelled)
                    {
                        FSM_WAIT_RESULT = false;
                        FSM_SEND_TELEGRAPHS = true;

                        //! recieve cancel request
                        OnCancelAllPendingTelegraphs();
                        return false;
                    }

                    Thread.Sleep(1);

                } while (true);
            }
            
            return true;
        }

        private void EngineStateReportEventHandler(TELEGRAPH_ENGINE_STATE State, TelegraphEngine EngineItem)
        {
            switch (State)
            {
                case TELEGRAPH_ENGINE_STATE.ENGINE_START:
                    m_Adapter.WriteLogLine("Telegraph engine starts working..");
                    break;
                case TELEGRAPH_ENGINE_STATE.ENGINE_DISPOSED:
                    EngineItem.EngineStateReportEvent -= new EngineStateReport(EngineStateReportEventHandler);
                    m_Adapter.WriteLogLine("Telegraph engine is disposed.");                    
                    if (m_tEngine == EngineItem)
                    {
                        m_tEngine = null;
                    }
                    
                    break;
                case TELEGRAPH_ENGINE_STATE.ENGINE_STOPED:
                    //EngineItem.EngineStateReportEvent -= new EngineStateReport(EngineStateReportEventHandler);
                    m_Adapter.WriteLogLine("Telegraph engine stopped.");
                   
                    break;
                case TELEGRAPH_ENGINE_STATE.ENGINE_STOPPING:
                    m_Adapter.WriteLogLine("Requesting stopping telegraph engine...");
                    break;
            }
        }

        protected override void OnPipelineServiceCancelled()
        {
            base.OnPipelineServiceCancelled();

            OnCancelAllPendingTelegraphs();

            if (null != m_tEngine)
            {
                try
                {
                    m_tEngine.Dispose();
                    m_tEngine = null;
                }
                catch (Exception Err)
                {
                    Err.ToString();
                }
            }
        }

        protected override void _Dispose()
        {
            //! cancell all pending telegraphs
            OnCancelAllPendingTelegraphs();

            try
            {
                if (null != m_tEngine)
                {
                    m_tEngine.Dispose();
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }
        }

        
    }
}

//! class for dynamic load
namespace ESnail.Device.Adapters
{
    public class AdapterLoader
    {
        public USB.HID.TelegraphHIDAdapter Create(SafeID tID, params Object[] tArgs)
        {
            USB.HID.TelegraphHIDAdapter tAdapter = new ESnail.Device.Adapters.USB.HID.TelegraphHIDAdapter(tID);
            if (null != tAdapter)
            {
                tAdapter.Name = "AT SB200/BM300";
            }
            return tAdapter;
        }
    }
}
