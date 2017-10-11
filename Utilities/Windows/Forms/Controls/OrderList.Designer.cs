namespace ESnail.Utilities.Windows.Forms.Controls
{
    partial class OrderList
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flpOrderList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpOrderList
            // 
            this.flpOrderList.AutoScroll = true;
            this.flpOrderList.BackColor = System.Drawing.Color.Transparent;
            this.flpOrderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpOrderList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpOrderList.Location = new System.Drawing.Point(0, 0);
            this.flpOrderList.Name = "flpOrderList";
            this.flpOrderList.Padding = new System.Windows.Forms.Padding(10);
            this.flpOrderList.Size = new System.Drawing.Size(515, 423);
            this.flpOrderList.TabIndex = 0;
            this.flpOrderList.WrapContents = false;
            // 
            // OrderList
            // 
            this.Controls.Add(this.flpOrderList);
            this.DoubleBuffered = true;
            this.Name = "OrderList";
            this.Size = new System.Drawing.Size(515, 423);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpOrderList;
    }
}
