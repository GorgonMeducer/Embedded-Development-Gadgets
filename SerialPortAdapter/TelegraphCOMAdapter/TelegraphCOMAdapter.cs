using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device.Adapters;
using ESnail.Device.Telegraphs;
using ESnail.Device.Telegraphs.Engines;
using ESnail.Device;
using ESnail.CommunicationSet.Commands;
using ESnail.Utilities;

namespace ESnail.Device.Adapters.SerialPort
{
    public partial class TelegraphCOMAdapter : SerialPortAdapter, ISPTelegraph
    {
        private SinglePhaseTelegraphAdapter m_TelegraphAdapter = null;
        private frmTelegraphCOMAdapterEditor m_Editor = null;

        public TelegraphCOMAdapter()
        {
            Initialize();
        }

        public TelegraphCOMAdapter(SafeID tID)
            : base(tID)
        {
            Initialize();
        }

        private void Initialize()
        {
            RegisterSupportTelegraph(new UserTelegraph(null));
            RegisterSupportTelegraph(new GSFrameTelegraph(null));

            m_TelegraphAdapter = new SinglePhaseTelegraphAdapter(this);
        }

        public override Adapter CreateAdapter(SafeID tID)
        {
            return new TelegraphCOMAdapter(tID);
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

        public override Boolean IsWorking
        {
            get
            {
                return m_TelegraphAdapter.IsWorking;
            }
            set
            {
                m_TelegraphAdapter.IsWorking = value;
            }
        }

        public override String Type
        {
            get {  return "Telegraph-based Serialport Adapter";}
        }

        //! \brief adapter version
        public override String Version
        {
            get { return "1.0.0.0"; }
        }
    }
}

//! class for dynamic load
namespace ESnail.Device.Adapters
{
    public class AdapterLoader
    {
        public SerialPort.TelegraphCOMAdapter Create(SafeID tID, params Object[] tArgs)
        {
            SerialPort.TelegraphCOMAdapter tAdapter = new SerialPort.TelegraphCOMAdapter(tID);
            if (null != tAdapter)
            {
                tAdapter.Name = "Serial Port";
            }
            return tAdapter;
        }
    }
}