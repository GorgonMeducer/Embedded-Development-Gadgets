namespace ESnail.Utilities.Windows.Forms.Dialogs
{
    partial class ProgressWheelDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labInfo = new System.Windows.Forms.Label();
            this.progressWheel = new ESnail.Utilities.Windows.Forms.Controls.ProgressWheel();
            this.SuspendLayout();
            // 
            // labInfo
            // 
            this.labInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labInfo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labInfo.Location = new System.Drawing.Point(0, 102);
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(472, 26);
            this.labInfo.TabIndex = 1;
            this.labInfo.Text = "     Loading...";
            this.labInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressWheel
            // 
            this.progressWheel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressWheel.BackColor = System.Drawing.Color.Transparent;
            this.progressWheel.Location = new System.Drawing.Point(0, 0);
            this.progressWheel.Name = "progressWheel";
            this.progressWheel.Size = new System.Drawing.Size(472, 102);
            this.progressWheel.Style = ESnail.Utilities.Windows.Forms.Controls.PROGRESS_WHEEL_STYLE.PW_STYLE_POINTS;
            this.progressWheel.TabIndex = 0;
            // 
            // ProgressWheelDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 131);
            this.Controls.Add(this.labInfo);
            this.Controls.Add(this.progressWheel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ProgressWheelDialog";
            this.Opacity = 0.6D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProgressWheelDialog";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.ResumeLayout(false);

        }

        #endregion

        private ESnail.Utilities.Windows.Forms.Controls.ProgressWheel progressWheel;
        private System.Windows.Forms.Label labInfo;
    }
}