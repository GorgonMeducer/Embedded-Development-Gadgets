﻿namespace ESnail.CommunicationSet.Commands
{
    partial class frmCommandWizardStepA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommandWizardStepA));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdNext = new System.Windows.Forms.Button();
            this.labStep = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.combCommandType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(22, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 5);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(22, 432);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 5);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(387, 443);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(84, 29);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdNext
            // 
            this.cmdNext.Location = new System.Drawing.Point(297, 443);
            this.cmdNext.Name = "cmdNext";
            this.cmdNext.Size = new System.Drawing.Size(84, 29);
            this.cmdNext.TabIndex = 2;
            this.cmdNext.Text = "Next >>";
            this.cmdNext.UseVisualStyleBackColor = true;
            this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
            // 
            // labStep
            // 
            this.labStep.AutoSize = true;
            this.labStep.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labStep.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.labStep.Location = new System.Drawing.Point(24, 37);
            this.labStep.Name = "labStep";
            this.labStep.Size = new System.Drawing.Size(176, 14);
            this.labStep.TabIndex = 4;
            this.labStep.Text = "Step A:  Select Command Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Command Type";
            // 
            // textBox2
            // 
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.textBox2.Location = new System.Drawing.Point(27, 73);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(453, 42);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label3.Location = new System.Drawing.Point(441, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "1 / 8";
            // 
            // combCommandType
            // 
            this.combCommandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combCommandType.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.combCommandType.FormattingEnabled = true;
            this.combCommandType.Items.AddRange(new object[] {
            "Just Command",
            "Write Word",
            "Read Word",
            "Write Block",
            "Read Block"});
            this.combCommandType.Location = new System.Drawing.Point(111, 140);
            this.combCommandType.Name = "combCommandType";
            this.combCommandType.Size = new System.Drawing.Size(118, 21);
            this.combCommandType.TabIndex = 12;
            // 
            // frmCommandWizardStepA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 492);
            this.Controls.Add(this.combCommandType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labStep);
            this.Controls.Add(this.cmdNext);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmCommandWizardStepA";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Command Wizard";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCommandWizardStepA_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdNext;
        private System.Windows.Forms.Label labStep;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox combCommandType;
    }
}