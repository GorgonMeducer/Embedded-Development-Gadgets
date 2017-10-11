namespace ESnail.Utilities.Windows.Forms.Controls
{
    partial class MemorySpaceViewer
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.columnAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuMemory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.displayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.byteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.halfword16bitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.word32BitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.doubleWord64BitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyASCIIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMemory.SuspendLayout();
            this.SuspendLayout();

            this.dgMemory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.columnAddress,
                this.columnData,
                this.columnContent
            });

            // 
            // columnAddress
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Blue;
            this.columnAddress.DefaultCellStyle = dataGridViewCellStyle1;
            this.columnAddress.Frozen = true;
            this.columnAddress.HeaderText = "Address";
            this.columnAddress.Name = "columnAddress";
            this.columnAddress.ReadOnly = true;
            this.columnAddress.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // columnData
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.columnData.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnData.HeaderText = "Data";
            this.columnData.Name = "columnData";
            this.columnData.ReadOnly = true;
            this.columnData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnData.Width = 400;
            // 
            // columnContent
            // 
            this.columnContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.columnContent.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnContent.HeaderText = "Content";
            this.columnContent.Name = "columnContent";
            this.columnContent.ReadOnly = true;
            this.columnContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // menuMemory
            // 
            this.menuMemory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayToolStripMenuItem,
            this.toolStripSeparator1,
            this.copyDataToolStripMenuItem,
            this.copyASCIIToolStripMenuItem});
            this.menuMemory.Name = "menuMemory";
            this.menuMemory.Size = new System.Drawing.Size(153, 98);
            // 
            // displayToolStripMenuItem
            // 
            this.displayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.byteToolStripMenuItem,
            this.halfword16bitsToolStripMenuItem,
            this.word32BitsToolStripMenuItem,
            this.doubleWord64BitsToolStripMenuItem});
            this.displayToolStripMenuItem.Name = "displayToolStripMenuItem";
            this.displayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.displayToolStripMenuItem.Text = "Display";
            // 
            // byteToolStripMenuItem
            // 
            this.byteToolStripMenuItem.Name = "byteToolStripMenuItem";
            this.byteToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.byteToolStripMenuItem.Text = "Byte ( 8bits )";
            this.byteToolStripMenuItem.Click += new System.EventHandler(this.byteToolStripMenuItem_Click);
            // 
            // halfword16bitsToolStripMenuItem
            // 
            this.halfword16bitsToolStripMenuItem.Name = "halfword16bitsToolStripMenuItem";
            this.halfword16bitsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.halfword16bitsToolStripMenuItem.Text = "Half-word ( 16bits )";
            this.halfword16bitsToolStripMenuItem.Click += new System.EventHandler(this.halfword16bitsToolStripMenuItem_Click);
            // 
            // word32BitsToolStripMenuItem
            // 
            this.word32BitsToolStripMenuItem.Name = "word32BitsToolStripMenuItem";
            this.word32BitsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.word32BitsToolStripMenuItem.Text = "Word ( 32 bits )";
            this.word32BitsToolStripMenuItem.Click += new System.EventHandler(this.word32BitsToolStripMenuItem_Click);
            // 
            // doubleWord64BitsToolStripMenuItem
            // 
            this.doubleWord64BitsToolStripMenuItem.Name = "doubleWord64BitsToolStripMenuItem";
            this.doubleWord64BitsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.doubleWord64BitsToolStripMenuItem.Text = "Double Word ( 64 bits )";
            this.doubleWord64BitsToolStripMenuItem.Click += new System.EventHandler(this.doubleWord64BitsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // copyDataToolStripMenuItem
            // 
            this.copyDataToolStripMenuItem.Name = "copyDataToolStripMenuItem";
            this.copyDataToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyDataToolStripMenuItem.Text = "Copy Data";
            this.copyDataToolStripMenuItem.Click += new System.EventHandler(this.copyDataToolStripMenuItem_Click);
            // 
            // copyASCIIToolStripMenuItem
            // 
            this.copyASCIIToolStripMenuItem.Name = "copyASCIIToolStripMenuItem";
            this.copyASCIIToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyASCIIToolStripMenuItem.Text = "Copy ASCII";
            this.copyASCIIToolStripMenuItem.Click += new System.EventHandler(this.copyASCIIToolStripMenuItem_Click);
            // 
            // MemorySpaceViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ContextMenuStrip = this.menuMemory;
            this.Name = "MemorySpaceViewer";
            this.menuMemory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        //protected System.Windows.Forms.DataGridView dgMemory;
        //private System.Windows.Forms.VScrollBar vbItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnData;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnContent;
        private System.Windows.Forms.ContextMenuStrip menuMemory;
        private System.Windows.Forms.ToolStripMenuItem displayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem byteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem halfword16bitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem word32BitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem doubleWord64BitsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem copyDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyASCIIToolStripMenuItem;
    }
}
