namespace ESnail.Utilities.Windows.Forms.Controls
{
    public partial class LargeDBViewer
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
            this.vbItems = new System.Windows.Forms.VScrollBar();
            this.dgMemory = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // vbItems
            // 
            this.vbItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vbItems.Location = new System.Drawing.Point(746, 2);
            this.vbItems.Name = "vbItems";
            this.vbItems.Size = new System.Drawing.Size(18, 594);
            this.vbItems.TabIndex = 4;
            this.vbItems.ValueChanged += new System.EventHandler(this.vbItems_ValueChanged);
            // 
            // dgMemory
            // 
            this.dgMemory.AllowUserToAddRows = false;
            this.dgMemory.AllowUserToDeleteRows = false;
            this.dgMemory.AllowUserToResizeRows = false;
            this.dgMemory.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgMemory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgMemory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgMemory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgMemory.GridColor = System.Drawing.SystemColors.Window;
            this.dgMemory.Location = new System.Drawing.Point(0, 0);
            this.dgMemory.Name = "dgMemory";
            this.dgMemory.ReadOnly = true;
            this.dgMemory.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgMemory.RowHeadersVisible = false;
            this.dgMemory.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgMemory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMemory.Size = new System.Drawing.Size(765, 598);
            this.dgMemory.TabIndex = 3;
            this.dgMemory.VirtualMode = true;
            this.dgMemory.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgMemory_CellValueNeeded);
            this.dgMemory.Resize += new System.EventHandler(this.dgMemory_Resize);
            // 
            // LargeDBViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vbItems);
            this.Controls.Add(this.dgMemory);
            this.DoubleBuffered = true;
            this.Name = "LargeDBViewer";
            this.Size = new System.Drawing.Size(765, 598);
            ((System.ComponentModel.ISupportInitialize)(this.dgMemory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vbItems;
        protected System.Windows.Forms.DataGridView dgMemory;
    }
}
