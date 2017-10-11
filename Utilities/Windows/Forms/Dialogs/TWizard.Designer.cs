namespace ESnail.Utilities.Windows.Forms.Dialogs
{
    partial class TWizard<TType>
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
            this.components = new System.ComponentModel.Container();
            this.panelEditor = new System.Windows.Forms.Panel();
            this.panelWizard = new System.Windows.Forms.Panel();
            this.cmdPrevious = new System.Windows.Forms.Button();
            this.labStepNumber = new System.Windows.Forms.Label();
            this.labStepName = new System.Windows.Forms.Label();
            this.cmdNext = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolTipError = new System.Windows.Forms.ToolTip(this.components);
            this.panelEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEditor
            // 
            this.panelEditor.Controls.Add(this.panelWizard);
            this.panelEditor.Controls.Add(this.cmdPrevious);
            this.panelEditor.Controls.Add(this.labStepNumber);
            this.panelEditor.Controls.Add(this.labStepName);
            this.panelEditor.Controls.Add(this.cmdNext);
            this.panelEditor.Controls.Add(this.cmdCancel);
            this.panelEditor.Controls.Add(this.groupBox2);
            this.panelEditor.Controls.Add(this.groupBox1);
            this.panelEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEditor.Location = new System.Drawing.Point(0, 0);
            this.panelEditor.Name = "panelEditor";
            this.panelEditor.Size = new System.Drawing.Size(452, 498);
            this.panelEditor.TabIndex = 0;
            // 
            // panelWizard
            // 
            this.panelWizard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizard.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelWizard.Location = new System.Drawing.Point(20, 53);
            this.panelWizard.Name = "panelWizard";
            this.panelWizard.Size = new System.Drawing.Size(413, 373);
            this.panelWizard.TabIndex = 35;
            // 
            // cmdPrevious
            // 
            this.cmdPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrevious.Location = new System.Drawing.Point(167, 445);
            this.cmdPrevious.Name = "cmdPrevious";
            this.cmdPrevious.Size = new System.Drawing.Size(84, 29);
            this.cmdPrevious.TabIndex = 34;
            this.cmdPrevious.Text = "<< Previous";
            this.cmdPrevious.UseVisualStyleBackColor = true;
            // 
            // labStepNumber
            // 
            this.labStepNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labStepNumber.AutoSize = true;
            this.labStepNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labStepNumber.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labStepNumber.Location = new System.Drawing.Point(398, 25);
            this.labStepNumber.Name = "labStepNumber";
            this.labStepNumber.Size = new System.Drawing.Size(35, 13);
            this.labStepNumber.TabIndex = 33;
            this.labStepNumber.Text = "1 / 2";
            // 
            // labStepName
            // 
            this.labStepName.AutoSize = true;
            this.labStepName.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labStepName.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labStepName.Location = new System.Drawing.Point(24, 25);
            this.labStepName.Name = "labStepName";
            this.labStepName.Size = new System.Drawing.Size(87, 14);
            this.labStepName.TabIndex = 32;
            this.labStepName.Text = "Step A:  XXXXX";
            // 
            // cmdNext
            // 
            this.cmdNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdNext.Location = new System.Drawing.Point(257, 445);
            this.cmdNext.Name = "cmdNext";
            this.cmdNext.Size = new System.Drawing.Size(84, 29);
            this.cmdNext.TabIndex = 31;
            this.cmdNext.Text = "Next >>";
            this.cmdNext.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(347, 445);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(84, 29);
            this.cmdCancel.TabIndex = 30;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(20, 432);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(413, 5);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(20, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(413, 5);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // toolTipError
            // 
            this.toolTipError.AutoPopDelay = 5000;
            this.toolTipError.InitialDelay = 1500;
            this.toolTipError.IsBalloon = true;
            this.toolTipError.ReshowDelay = 1000;
            this.toolTipError.ShowAlways = true;
            this.toolTipError.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.toolTipError.ToolTipTitle = "Command";
            // 
            // TWizard
            // 
            this.AcceptButton = this.cmdNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(452, 498);
            this.Controls.Add(this.panelEditor);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "TWizard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wizard";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TWizard_FormClosing);
            this.panelEditor.ResumeLayout(false);
            this.panelEditor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEditor;
        protected System.Windows.Forms.Panel panelWizard;
        protected System.Windows.Forms.Button cmdPrevious;
        protected System.Windows.Forms.Label labStepNumber;
        protected System.Windows.Forms.Label labStepName;
        protected System.Windows.Forms.Button cmdNext;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ToolTip toolTipError;

    }
}