using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESnail.Utilities;

namespace ESnail.Utilities.Windows.Forms.Dialogs
{
    public enum WIZARD_REPORT
    {
        WIZARD_CANCELLED,
        WIZARD_ERROR,
        WIZARD_OK
    }

    public partial class TWizard<TType> : Form
    {
        protected TType m_Item = default(TType);
        private Boolean m_OnRefresh = false;
        private Boolean m_Available = false;
        public TWizard()
        {
            Initialize();
        }

        public TWizard(TType tItem)
        {
            m_Item = tItem;
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();

            if (null == m_Item)
            {
                panelEditor.Enabled = false;
            }
            else
            {
                m_Available = true;
            }

            //Refresh();
        }

        public Boolean Available
        {
            get { return m_Available; }
        }

        protected Boolean Refreshing
        {
            get { return m_OnRefresh; }
        }

        public override void Refresh()
        {
            if (null == m_Item)
            {
                base.Refresh();
                return;
            }
            else if (m_OnRefresh)
            {
                base.Refresh();
                return;
            }
            m_OnRefresh = true;
            
            //! refresh body
            do
            {
                _Refresh();
            }
            while (false);

            m_OnRefresh = false;

            base.Refresh();
        }

        protected virtual void _Refresh()
        { 
            
        }


        public delegate void WizardReport(TType tItem, WIZARD_REPORT tResult);

        public event WizardReport WizardReportEvent;
        private WizardReport m_WizardEvent = null;

        protected void OnWizardReport(WIZARD_REPORT tResult)
        {
            if (null != WizardReportEvent)
            {
                try
                {
                    WizardReportEvent.Invoke(m_Item, tResult);
                }
                catch (Exception) { }
            }
            else if (null != m_WizardEvent)
            {
                try
                {
                    m_WizardEvent.Invoke(m_Item, tResult);
                }
                catch (Exception) { }
            }
        }

        protected void RegisterEvent(WizardReport tDelegates)
        {
            if (null != tDelegates)
            {
                m_WizardEvent += tDelegates;
            }
        }

        protected Boolean TransferTo(TWizard<TType> tWizard)
        {
            if (null == tWizard)
            {
                return false;
            }

            if (null != WizardReportEvent)
            {
                tWizard.RegisterEvent(WizardReportEvent);
                this.WizardReportEvent -= WizardReportEvent;
            }
            else if (null != m_WizardEvent)
            {
                tWizard.RegisterEvent(m_WizardEvent);
                this.m_WizardEvent -= m_WizardEvent;
            }

            this.Hide();
            tWizard.Show();
            tWizard.Refresh();
            this.Dispose();

            return true;
        }

        private void TWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnWizardReport(WIZARD_REPORT.WIZARD_CANCELLED);
        }

        protected void Cancel()
        {
            OnWizardReport(WIZARD_REPORT.WIZARD_CANCELLED);
            Dispose();
        }

        protected void Finish()
        {
            OnWizardReport(WIZARD_REPORT.WIZARD_OK);
            Dispose();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }



        protected void ShowErrorMessage(Control tControl, String tMessage)
        {
            toolTipError.Active = false;
            toolTipError.ToolTipIcon = ToolTipIcon.Warning;
            toolTipError.Show("", tControl);
            toolTipError.Active = true;
            toolTipError.Show(tMessage, tControl, 5000);
        }
    }
}
