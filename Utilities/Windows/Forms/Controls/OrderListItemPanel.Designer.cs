namespace ESnail.Utilities.Windows.Forms.Controls
{
    partial class OrderListItemPanel
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
            this.flpButton = new System.Windows.Forms.FlowLayoutPanel();
            this.cmdUp = new System.Windows.Forms.Button();
            this.cmdDown = new System.Windows.Forms.Button();
            this.cmdTop = new System.Windows.Forms.Button();
            this.cmdBottom = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.flpPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.flpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // flpButton
            // 
            this.flpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpButton.AutoSize = true;
            this.flpButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpButton.Controls.Add(this.cmdUp);
            this.flpButton.Controls.Add(this.cmdDown);
            this.flpButton.Controls.Add(this.cmdTop);
            this.flpButton.Controls.Add(this.cmdBottom);
            this.flpButton.Controls.Add(this.cmdRemove);
            this.flpButton.Location = new System.Drawing.Point(86, 0);
            this.flpButton.Margin = new System.Windows.Forms.Padding(0);
            this.flpButton.Name = "flpButton";
            this.flpButton.Size = new System.Drawing.Size(115, 23);
            this.flpButton.TabIndex = 16;
            this.flpButton.WrapContents = false;
            // 
            // cmdUp
            // 
            this.cmdUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdUp.BackgroundImage = global::ESnail.Utilities.Properties.Resources.taskup;
            this.cmdUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmdUp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdUp.Location = new System.Drawing.Point(0, 0);
            this.cmdUp.Margin = new System.Windows.Forms.Padding(0);
            this.cmdUp.Name = "cmdUp";
            this.cmdUp.Size = new System.Drawing.Size(23, 23);
            this.cmdUp.TabIndex = 17;
            this.cmdUp.UseVisualStyleBackColor = true;
            this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
            // 
            // cmdDown
            // 
            this.cmdDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDown.BackgroundImage = global::ESnail.Utilities.Properties.Resources.taskdown;
            this.cmdDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmdDown.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdDown.Location = new System.Drawing.Point(23, 0);
            this.cmdDown.Margin = new System.Windows.Forms.Padding(0);
            this.cmdDown.Name = "cmdDown";
            this.cmdDown.Size = new System.Drawing.Size(23, 23);
            this.cmdDown.TabIndex = 16;
            this.cmdDown.UseVisualStyleBackColor = true;
            this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
            // 
            // cmdTop
            // 
            this.cmdTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTop.BackgroundImage = global::ESnail.Utilities.Properties.Resources.taskTop;
            this.cmdTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmdTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdTop.Location = new System.Drawing.Point(46, 0);
            this.cmdTop.Margin = new System.Windows.Forms.Padding(0);
            this.cmdTop.Name = "cmdTop";
            this.cmdTop.Size = new System.Drawing.Size(23, 23);
            this.cmdTop.TabIndex = 18;
            this.cmdTop.UseVisualStyleBackColor = true;
            this.cmdTop.Click += new System.EventHandler(this.cmdTop_Click);
            // 
            // cmdBottom
            // 
            this.cmdBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBottom.BackgroundImage = global::ESnail.Utilities.Properties.Resources.taskButtom;
            this.cmdBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmdBottom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdBottom.Location = new System.Drawing.Point(69, 0);
            this.cmdBottom.Margin = new System.Windows.Forms.Padding(0);
            this.cmdBottom.Name = "cmdBottom";
            this.cmdBottom.Size = new System.Drawing.Size(23, 23);
            this.cmdBottom.TabIndex = 19;
            this.cmdBottom.UseVisualStyleBackColor = true;
            this.cmdBottom.Click += new System.EventHandler(this.cmdBottom_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRemove.BackgroundImage = global::ESnail.Utilities.Properties.Resources.Delete_1;
            this.cmdRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmdRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdRemove.Location = new System.Drawing.Point(92, 0);
            this.cmdRemove.Margin = new System.Windows.Forms.Padding(0);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(23, 23);
            this.cmdRemove.TabIndex = 20;
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // flpPanel
            // 
            this.flpPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flpPanel.AutoSize = true;
            this.flpPanel.Location = new System.Drawing.Point(0, 0);
            this.flpPanel.Margin = new System.Windows.Forms.Padding(0);
            this.flpPanel.Name = "flpPanel";
            this.flpPanel.Size = new System.Drawing.Size(86, 23);
            this.flpPanel.TabIndex = 17;
            this.flpPanel.WrapContents = false;
            // 
            // OrderListItemPanel
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.flpPanel);
            this.Controls.Add(this.flpButton);
            this.DoubleBuffered = true;
            this.Name = "OrderListItemPanel";
            this.Size = new System.Drawing.Size(201, 23);
            this.flpButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpButton;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.Button cmdBottom;
        private System.Windows.Forms.Button cmdTop;
        private System.Windows.Forms.Button cmdUp;
        private System.Windows.Forms.Button cmdDown;
        private System.Windows.Forms.FlowLayoutPanel flpPanel;

    }
}
