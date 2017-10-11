namespace ESnail.Device.Adapters.USB.HID
{
    partial class ESnailHIDAgent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (null != m_Adapter)
            {
                m_Adapter.Dispose();
            }
            base.Dispose(disposing);
        }

        #region ESnail.Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.SuspendLayout();
            // 
            // ATBatteryManageHIDAgent
            // 
            //this.Name = "ATBatteryManageHIDAgent";
            //this.Size = new System.Drawing.Size(195, 235);
            //this.ResumeLayout(false);

        }

        #endregion

    }
}
