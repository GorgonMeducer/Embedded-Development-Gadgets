using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Device.Adapters;
using ESnail.CommunicationSet.Commands;
using System.IO.Ports;

namespace ESnail.Device.Adapters.SerialPort
{
    internal partial class frmTelegraphCOMAdapterEditor : frmAdapterEditor
    {
        private new TelegraphCOMAdapter m_Adapter = null;
        private Boolean m_bOnRefreshing = false;
        private Boolean m_Initialized = false;

        public frmTelegraphCOMAdapterEditor() 
            :base()
        {
            Initialize();
        }

        public frmTelegraphCOMAdapterEditor(TelegraphCOMAdapter tAdapter)
            : base(tAdapter)
        {
            m_Adapter = tAdapter;
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();
            m_Initialized = true;

            if (null == m_Adapter)
            {
                return;
            }
            m_Adapter.DeviceOpenedEvent += new DeviceOpened(m_Adapter_DeviceOpenedEvent);
            m_Adapter.DeviceClosedEvent += new DeviceClosed(m_Adapter_DeviceClosedEvent);
            RefreshAdapterInformation();
            
            Refresh();
        }

        private void _Dispose()
        {
            if (null != m_Adapter)
            {
                m_Adapter.DeviceOpenedEvent -= new DeviceOpened(m_Adapter_DeviceOpenedEvent);
                m_Adapter.DeviceClosedEvent -= new DeviceClosed(m_Adapter_DeviceClosedEvent);
            }
        }

        private void m_Adapter_DeviceClosedEvent(SingleDeviceAdapter tAdapter)
        {
            Refresh();
        }

        private void m_Adapter_DeviceOpenedEvent(SingleDeviceAdapter tAdapter)
        {
            Refresh();
        }

        public override void Refresh()
        {
            base.Refresh();
            if ((m_bOnRefreshing) || (!m_Initialized))
            {
                return;
            }
            m_bOnRefreshing = true;


            RefreshDeviceList();
            RefreshAdapterInformation();
            

            m_bOnRefreshing = false;
        }

        private void cmdRefreshInfo_Click(object sender, EventArgs e)
        {
            RefreshAdapterInformation();
        }

        private void RefreshAdapterInformation()
        {
            if (null == m_Adapter)
            {
                grbInfo.Enabled = false;
                grbSerialPortSettings.Enabled = false;
                return;
            }

            //! avoid change settings if the serial port is open.
            if (m_Adapter.Open)
            {
                grbSerialPortSettings.Enabled = false;
            }
            else
            {
                grbSerialPortSettings.Enabled = true;
            }

            //! information
            txtAdapterKey.Text = m_Adapter.ID;
            txtAdapterType.Text = m_Adapter.Type;
            txtDeviceType.Text = m_Adapter.DeviceType;
            txtDriverVersion.Text = m_Adapter.DeviceVersion;
            txtDeviceInfo.Text = m_Adapter.DeviceInfo;
            txtSetting.Text = m_Adapter.Settings;
            txtConnectionState.Text = m_Adapter.Open ? "Open" : "Closed";

            //! settings
            do
            {
                //! baudrate
                Int32 tIndex = combBaudrate.Items.IndexOf(m_Adapter.Baudrate.ToString());
                if (tIndex >= 0)
                {
                    combBaudrate.SelectedIndex = tIndex;
                }
                else
                {
                    combBaudrate.SelectedIndex = -1;
                    combBaudrate.Text = m_Adapter.Baudrate.ToString();
                }
            }
            while (false);

            do
            {
                 
                //! data bits
                Int32 tIndex = combDatabits.Items.IndexOf(m_Adapter.DataBits.ToString());
                if (tIndex >= 0)
                {
                    combDatabits.SelectedIndex = tIndex;
                }
                else
                {
                    combDatabits.SelectedIndex = -1;
                    combDatabits.Text = m_Adapter.DataBits.ToString();
                }
            }
            while (false);

            do
            {
                
                //! parity
                Int32 tIndex = comboParity.Items.IndexOf(m_Adapter.Parity.ToString());
                if (tIndex >= 0)
                {
                    comboParity.SelectedIndex = tIndex;
                }
                else
                {
                    comboParity.SelectedIndex = -1;
                    comboParity.Text = m_Adapter.Parity.ToString();
                }
            }
            while (false);

            do
            {
                
                //! stop bit
                switch (m_Adapter.StopBits)
                { 
                    case StopBits.None:
                        comboStopBits.SelectedIndex = -1;
                        break;
                    case StopBits.One:
                        comboStopBits.SelectedIndex = 0;
                        break;
                    case StopBits.OnePointFive:
                        comboStopBits.SelectedIndex = 1;
                        break;
                    case StopBits.Two:
                        comboStopBits.SelectedIndex = 2;
                        break;
                }
            }
            while (false);
        }

        private void combBaudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == m_Adapter)
            {
                return;
            }

            try
            {
                m_Adapter.Baudrate = Convert.ToInt32(combBaudrate.Text);
            }
            catch (Exception )
            {
                
            }

            if (!m_bOnRefreshing)
            {
                RefreshAdapterInformation();
            }
        }

        private void combDatabits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == m_Adapter)
            {
                return;
            }

            try
            {
                m_Adapter.DataBits = Convert.ToInt32(combDatabits.Text);
            }
            catch (Exception )
            {
                
            }

            if (!m_bOnRefreshing)
            {
                RefreshAdapterInformation();
            }
        }

        private void comboParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == m_Adapter)
            {
                return;
            }

            try
            {
                switch (comboParity.Text)
                { 
                    case "Even":
                        m_Adapter.Parity = Parity.Even;
                        break;
                    case "Odd":
                        m_Adapter.Parity = Parity.Odd;
                        break;
                    case "Space":
                        m_Adapter.Parity = Parity.Space;
                        break;
                    case "Mark":
                        m_Adapter.Parity = Parity.Mark;
                        break;
                    default:
                    case "None":
                        m_Adapter.Parity = Parity.None;
                        break;
                }
            }
            catch (Exception Err)
            {
                Err.ToString();
            }

            if (!m_bOnRefreshing)
            {
                RefreshAdapterInformation();
            }
        }

        private void comboStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == m_Adapter)
            {
                return;
            }

            try
            {
                switch (comboStopBits.Text)
                { 
                    case "1.5":
                        m_Adapter.StopBits = StopBits.OnePointFive;
                        break;
                    case "2":
                        m_Adapter.StopBits = StopBits.Two;
                        break;
                    default:
                    case "1":
                        m_Adapter.StopBits = StopBits.One;
                        break;
                }

            }
            catch (Exception )
            {
                
            }

            if (!m_bOnRefreshing)
            {
                RefreshAdapterInformation();
            }
        }

        private void cmdRefreshDevice_Click(object sender, EventArgs e)
        {
            RefreshDeviceList();
        }

        private void RefreshDeviceList()
        {
            lvHIDDevice.Items.Clear();


            //! find all devices
            String[] strDeviceList = TelegraphCOMAdapter.FindDevice();

            if (null == strDeviceList)
            {
                if (!m_bOnRefreshing)
                {
                    RefreshAdapterInformation();
                }
                return;
            }

            for (System.Int32 n = 0; n < strDeviceList.Length; n++)
            {
                //! add command
                ListViewItem temItem = new ListViewItem(n.ToString("D2"));
                temItem.SubItems.Add(strDeviceList[n]);

                if (m_Adapter.Settings == strDeviceList[n])
                {
                    temItem.SubItems.Add("Connected");
                    if (m_Adapter.Open)
                    {
                        temItem.SubItems.Add("Open");
                    }
                    else
                    {
                        temItem.SubItems.Add("Close");
                    }
                }
                else
                {
                    temItem.SubItems.Add("-");
                    temItem.SubItems.Add("-");
                }

                lvHIDDevice.Items.Add(temItem);
            }

            if (!m_bOnRefreshing)
            {
                RefreshAdapterInformation();
            }
        }

        private void lvHIDDevice_DoubleClick(object sender, EventArgs e)
        {
            if (lvHIDDevice.SelectedItems.Count <= 0)
            {
                return;
            }
            ListViewItem lviSelectedItem = lvHIDDevice.SelectedItems[0];

            if (lviSelectedItem.SubItems[1].Text != m_Adapter.Settings)
            {
                //! disconnect current connection first
                m_Adapter.Open = false;

                //! modify previouse device connection display
                if ((null != m_Adapter.Settings) && ("" != m_Adapter.Settings.Trim()))
                {
                    for (System.Int32 n = 0; n < lvHIDDevice.Items.Count; n++)
                    {
                        if (lvHIDDevice.Items[n].SubItems[1].Text == m_Adapter.Settings)
                        {
                            lvHIDDevice.Items[n].SubItems[3].Text = "Close";
                            break;
                        }
                    }
                }

                //! change seetingss
                m_Adapter.Settings = lviSelectedItem.SubItems[1].Text;
                m_Adapter.Open = true;
            }
            else
            {
                m_Adapter.Open = !m_Adapter.Open;

            }

            lviSelectedItem.SubItems[3].Text = m_Adapter.Open ? "Open" : "Close";
            RefreshDeviceList();
        }
    }
}
