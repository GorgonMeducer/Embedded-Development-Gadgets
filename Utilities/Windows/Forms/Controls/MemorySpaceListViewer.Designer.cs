namespace ESnail.Utilities.Windows.Forms.Controls
{
    partial class MemorySpaceListViewer
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
            this.lvMemory = new System.Windows.Forms.ListView();
            this.columnAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnContent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // lvMemory
            // 
            this.lvMemory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnAddress,
            this.columnData,
            this.columnContent});
            this.lvMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMemory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvMemory.FullRowSelect = true;
            this.lvMemory.Location = new System.Drawing.Point(0, 0);
            this.lvMemory.Name = "lvMemory";
            this.lvMemory.Size = new System.Drawing.Size(621, 599);
            this.lvMemory.TabIndex = 0;
            this.lvMemory.UseCompatibleStateImageBehavior = false;
            this.lvMemory.View = System.Windows.Forms.View.Details;
            this.lvMemory.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.lvMemory_CacheVirtualItems);
            this.lvMemory.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvMemory_RetrieveVirtualItem);
            this.lvMemory.SearchForVirtualItem += new System.Windows.Forms.SearchForVirtualItemEventHandler(this.lvMemory_SearchForVirtualItem);
            // 
            // columnAddress
            // 
            this.columnAddress.Text = "Address";
            this.columnAddress.Width = 80;
            // 
            // columnData
            // 
            this.columnData.Text = "Data";
            this.columnData.Width = 300;
            // 
            // columnContent
            // 
            this.columnContent.Text = "Content";
            this.columnContent.Width = 200;
            // 
            // MemorySpaceListViewer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.lvMemory);
            this.DoubleBuffered = true;
            this.Name = "MemorySpaceListViewer";
            this.Size = new System.Drawing.Size(621, 599);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader columnAddress;
        private System.Windows.Forms.ColumnHeader columnData;
        private System.Windows.Forms.ColumnHeader columnContent;
        protected System.Windows.Forms.ListView lvMemory;
    }
}
