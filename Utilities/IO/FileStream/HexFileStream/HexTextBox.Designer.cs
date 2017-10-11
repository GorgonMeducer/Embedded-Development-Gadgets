namespace ESnail.Utilities.Windows.Forms.Controls
{
    partial class HexTextBox
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.hexView = new System.Windows.Forms.DataGridView();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte00 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte01 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte02 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte03 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Byte04 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.hexView)).BeginInit();
            this.SuspendLayout();
            // 
            // hexView
            // 
            this.hexView.AllowUserToAddRows = false;
            this.hexView.AllowUserToDeleteRows = false;
            this.hexView.AllowUserToResizeColumns = false;
            this.hexView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.NullValue = "FF";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.hexView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.hexView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.hexView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.hexView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.hexView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.hexView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.hexView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Address,
            this.Byte00,
            this.Byte01,
            this.Byte02,
            this.Byte03,
            this.Byte04});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.NullValue = "FF";
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.hexView.DefaultCellStyle = dataGridViewCellStyle3;
            this.hexView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hexView.Location = new System.Drawing.Point(0, 0);
            this.hexView.MultiSelect = false;
            this.hexView.Name = "hexView";
            this.hexView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.hexView.RowHeadersVisible = false;
            this.hexView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.hexView.RowTemplate.Height = 16;
            this.hexView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.hexView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.hexView.Size = new System.Drawing.Size(525, 439);
            this.hexView.TabIndex = 0;
            this.hexView.VirtualMode = true;
            // 
            // Address
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.NullValue = "FFFFFFFF";
            this.Address.DefaultCellStyle = dataGridViewCellStyle2;
            this.Address.Frozen = true;
            this.Address.HeaderText = "Address";
            this.Address.MaxInputLength = 50;
            this.Address.MinimumWidth = 64;
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            this.Address.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Address.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Address.Width = 64;
            // 
            // Byte00
            // 
            this.Byte00.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Byte00.HeaderText = "00";
            this.Byte00.MaxInputLength = 2;
            this.Byte00.MinimumWidth = 24;
            this.Byte00.Name = "Byte00";
            this.Byte00.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte00.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Byte00.Width = 24;
            // 
            // Byte01
            // 
            this.Byte01.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Byte01.HeaderText = "01";
            this.Byte01.MaxInputLength = 2;
            this.Byte01.MinimumWidth = 24;
            this.Byte01.Name = "Byte01";
            this.Byte01.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte01.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Byte01.Width = 24;
            // 
            // Byte02
            // 
            this.Byte02.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Byte02.HeaderText = "02";
            this.Byte02.MaxInputLength = 2;
            this.Byte02.MinimumWidth = 24;
            this.Byte02.Name = "Byte02";
            this.Byte02.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte02.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Byte02.Width = 24;
            // 
            // Byte03
            // 
            this.Byte03.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Byte03.HeaderText = "03";
            this.Byte03.MaxInputLength = 2;
            this.Byte03.MinimumWidth = 24;
            this.Byte03.Name = "Byte03";
            this.Byte03.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte03.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Byte03.Width = 24;
            // 
            // Byte04
            // 
            this.Byte04.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Byte04.HeaderText = "04";
            this.Byte04.MaxInputLength = 2;
            this.Byte04.MinimumWidth = 24;
            this.Byte04.Name = "Byte04";
            this.Byte04.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Byte04.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Byte04.Width = 24;
            // 
            // HexTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hexView);
            this.DoubleBuffered = true;
            this.Name = "HexTextBox";
            this.Size = new System.Drawing.Size(525, 439);
            ((System.ComponentModel.ISupportInitialize)(this.hexView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView hexView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte00;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte01;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte02;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte03;
        private System.Windows.Forms.DataGridViewTextBoxColumn Byte04;
    }
}
