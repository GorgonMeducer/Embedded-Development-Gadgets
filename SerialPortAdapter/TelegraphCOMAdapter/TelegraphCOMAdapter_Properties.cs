using System;
using System.Collections.Generic;
using System.Text;
using ESnail.Device.Adapters;
using ESnail.Device.Telegraphs;
using ESnail.Utilities;
using System.Windows.Forms;


namespace ESnail.Device.Adapters.SerialPort
{
    partial class TelegraphCOMAdapter
    {
        public Boolean TryToSendTelegraph(SinglePhaseTelegraph telTarget)
        {
            return m_TelegraphAdapter.TryToSendTelegraph(telTarget);
        }

        public Boolean TryToSendTelegraph(Telegraph telTarget)
        {
            return m_TelegraphAdapter.TryToSendTelegraph(telTarget);
        }

        public Boolean TryToSendTelegraphs(Telegraph[] telTagets)
        {
            return m_TelegraphAdapter.TryToSendTelegraphs(telTagets);
        }

        #region editor and pages
        //! \brief get default editor
        public override Form Editor
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphCOMAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(m_Editor_Disposed);
                }

                return m_Editor;
            }
        }

        private void m_Editor_Disposed(object sender, EventArgs e)
        {
            m_Editor = null;
        }

        public override TabPage CommunicationPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphCOMAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(m_Editor_Disposed);
                }

                return m_Editor.CommunicationPage;
            }
        }

        public override TabPage DeviceManagerPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphCOMAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(m_Editor_Disposed);
                }

                return m_Editor.DeviceManagerPage;
            }
        }

        public override TabPage InformationPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphCOMAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(m_Editor_Disposed);
                }

                return m_Editor.InformationPage;
            }
        }

        public override TabPage DebugPage
        {
            get
            {
                if (null == m_Editor)
                {
                    m_Editor = new frmTelegraphCOMAdapterEditor(this);
                    m_Editor.Disposed += new EventHandler(m_Editor_Disposed);
                }

                return m_Editor.DebugPage;
            }
        }

        //! \brief create new editor
        public override Form CreateEditor()
        {
            return new frmTelegraphCOMAdapterEditor(this);
        }

        //! \brief create control
        public override Control CreateControl()
        {
            return null;
        }

        public override System.ComponentModel.Component CreateComponent()
        {
            return new ESnailSerialPortAgent(this); ;
        }

        #endregion
    }
}
