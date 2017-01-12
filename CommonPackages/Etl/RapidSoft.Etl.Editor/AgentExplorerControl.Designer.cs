namespace RapidSoft.Etl.Editor
{
    partial class AgentExplorerControl
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
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.bndPackages = new System.Windows.Forms.BindingSource(this.components);
            this.tbcAgentItems = new System.Windows.Forms.TabControl();
            this.tabPackages = new System.Windows.Forms.TabPage();
            this.grdPackages = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageStepName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlsPackages = new System.Windows.Forms.ToolStrip();
            this.btnNewPackage = new System.Windows.Forms.ToolStripButton();
            this.btnOpenPackage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefreshPackages = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopyPackageTo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.panAgentItems = new System.Windows.Forms.Panel();
            this.bndEtlAgentTypes = new System.Windows.Forms.BindingSource(this.components);
            this.lblEtlAgentType = new System.Windows.Forms.Label();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.txtSchemaName = new System.Windows.Forms.TextBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.lblSchemaName = new System.Windows.Forms.Label();
            this.btnConnectAgent = new System.Windows.Forms.Button();
            this.lblEtlAgent = new System.Windows.Forms.Label();
            this.cmbEtlAgentType = new System.Windows.Forms.ComboBox();
            this.panAgentInfo = new System.Windows.Forms.Panel();
            this.sstMain = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bndPackages)).BeginInit();
            this.tbcAgentItems.SuspendLayout();
            this.tabPackages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPackages)).BeginInit();
            this.tlsPackages.SuspendLayout();
            this.panAgentItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndEtlAgentTypes)).BeginInit();
            this.panAgentInfo.SuspendLayout();
            this.sstMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.Location = new System.Drawing.Point(0, 0);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Size = new System.Drawing.Size(661, 25);
            this.miniToolStrip.TabIndex = 4;
            // 
            // bndPackages
            // 
            this.bndPackages.Sort = "";
            // 
            // tbcAgentItems
            // 
            this.tbcAgentItems.Controls.Add(this.tabPackages);
            this.tbcAgentItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcAgentItems.Location = new System.Drawing.Point(0, 0);
            this.tbcAgentItems.Name = "tbcAgentItems";
            this.tbcAgentItems.SelectedIndex = 0;
            this.tbcAgentItems.Size = new System.Drawing.Size(675, 459);
            this.tbcAgentItems.TabIndex = 0;
            // 
            // tabPackages
            // 
            this.tabPackages.Controls.Add(this.grdPackages);
            this.tabPackages.Controls.Add(this.tlsPackages);
            this.tabPackages.Location = new System.Drawing.Point(4, 22);
            this.tabPackages.Name = "tabPackages";
            this.tabPackages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPackages.Size = new System.Drawing.Size(667, 433);
            this.tabPackages.TabIndex = 0;
            this.tabPackages.Text = "Packages";
            this.tabPackages.UseVisualStyleBackColor = true;
            // 
            // grdPackages
            // 
            this.grdPackages.AllowUserToAddRows = false;
            this.grdPackages.AllowUserToDeleteRows = false;
            this.grdPackages.AllowUserToOrderColumns = true;
            this.grdPackages.AllowUserToResizeRows = false;
            this.grdPackages.AutoGenerateColumns = false;
            this.grdPackages.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdPackages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grdPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPackages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colPackageStepName});
            this.grdPackages.DataSource = this.bndPackages;
            this.grdPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPackages.Location = new System.Drawing.Point(3, 28);
            this.grdPackages.MultiSelect = false;
            this.grdPackages.Name = "grdPackages";
            this.grdPackages.ReadOnly = true;
            this.grdPackages.RowHeadersVisible = false;
            this.grdPackages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdPackages.Size = new System.Drawing.Size(661, 402);
            this.grdPackages.StandardTab = true;
            this.grdPackages.TabIndex = 3;
            this.grdPackages.SelectionChanged += new System.EventHandler(this.grdPackages_SelectionChanged);
            this.grdPackages.DoubleClick += new System.EventHandler(this.grdPackages_DoubleClick);
            this.grdPackages.KeyUp += new System.Windows.Forms.KeyEventHandler(this.grdPackages_KeyUp);
            // 
            // colId
            // 
            this.colId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 230;
            // 
            // colPackageStepName
            // 
            this.colPackageStepName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPackageStepName.DataPropertyName = "Name";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colPackageStepName.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPackageStepName.FillWeight = 132.4435F;
            this.colPackageStepName.HeaderText = "Name";
            this.colPackageStepName.Name = "colPackageStepName";
            this.colPackageStepName.ReadOnly = true;
            // 
            // tlsPackages
            // 
            this.tlsPackages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewPackage,
            this.btnOpenPackage,
            this.toolStripSeparator3,
            this.btnRefreshPackages,
            this.toolStripSeparator4,
            this.btnCopyPackageTo});
            this.tlsPackages.Location = new System.Drawing.Point(3, 3);
            this.tlsPackages.Name = "tlsPackages";
            this.tlsPackages.Size = new System.Drawing.Size(661, 25);
            this.tlsPackages.TabIndex = 4;
            this.tlsPackages.Text = "toolStrip1";
            // 
            // btnNewPackage
            // 
            this.btnNewPackage.Image = global::RapidSoft.Etl.Editor.Properties.Resources.NewDocument;
            this.btnNewPackage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewPackage.Name = "btnNewPackage";
            this.btnNewPackage.Size = new System.Drawing.Size(51, 22);
            this.btnNewPackage.Text = "&New";
            this.btnNewPackage.Click += new System.EventHandler(this.btnNewPackage_Click);
            // 
            // btnOpenPackage
            // 
            this.btnOpenPackage.Image = global::RapidSoft.Etl.Editor.Properties.Resources.OpenFile;
            this.btnOpenPackage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenPackage.Name = "btnOpenPackage";
            this.btnOpenPackage.Size = new System.Drawing.Size(56, 22);
            this.btnOpenPackage.Text = "&Open";
            this.btnOpenPackage.Click += new System.EventHandler(this.btnOpenPackage_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefreshPackages
            // 
            this.btnRefreshPackages.Image = global::RapidSoft.Etl.Editor.Properties.Resources.RefreshDocViewHS;
            this.btnRefreshPackages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshPackages.Name = "btnRefreshPackages";
            this.btnRefreshPackages.Size = new System.Drawing.Size(66, 22);
            this.btnRefreshPackages.Text = "&Refresh";
            this.btnRefreshPackages.Click += new System.EventHandler(this.btnRefreshPackages_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCopyPackageTo
            // 
            this.btnCopyPackageTo.Image = global::RapidSoft.Etl.Editor.Properties.Resources.CopyHS;
            this.btnCopyPackageTo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopyPackageTo.Name = "btnCopyPackageTo";
            this.btnCopyPackageTo.Size = new System.Drawing.Size(55, 22);
            this.btnCopyPackageTo.Text = "&Copy";
            this.btnCopyPackageTo.Click += new System.EventHandler(this.btnCopyPackageTo_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // panAgentItems
            // 
            this.panAgentItems.Controls.Add(this.tbcAgentItems);
            this.panAgentItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panAgentItems.Location = new System.Drawing.Point(0, 141);
            this.panAgentItems.Name = "panAgentItems";
            this.panAgentItems.Size = new System.Drawing.Size(675, 459);
            this.panAgentItems.TabIndex = 5;
            // 
            // lblEtlAgentType
            // 
            this.lblEtlAgentType.AutoSize = true;
            this.lblEtlAgentType.Location = new System.Drawing.Point(11, 35);
            this.lblEtlAgentType.Name = "lblEtlAgentType";
            this.lblEtlAgentType.Size = new System.Drawing.Size(34, 13);
            this.lblEtlAgentType.TabIndex = 6;
            this.lblEtlAgentType.Text = "Type:";
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConnectionString.Location = new System.Drawing.Point(112, 59);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(553, 20);
            this.txtConnectionString.TabIndex = 3;
            this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
            // 
            // txtSchemaName
            // 
            this.txtSchemaName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSchemaName.Location = new System.Drawing.Point(112, 86);
            this.txtSchemaName.Name = "txtSchemaName";
            this.txtSchemaName.Size = new System.Drawing.Size(553, 20);
            this.txtSchemaName.TabIndex = 4;
            // 
            // lblConnectionString
            // 
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(11, 62);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(92, 13);
            this.lblConnectionString.TabIndex = 10;
            this.lblConnectionString.Text = "Connection string:";
            // 
            // lblSchemaName
            // 
            this.lblSchemaName.AutoSize = true;
            this.lblSchemaName.Location = new System.Drawing.Point(11, 89);
            this.lblSchemaName.Name = "lblSchemaName";
            this.lblSchemaName.Size = new System.Drawing.Size(49, 13);
            this.lblSchemaName.TabIndex = 11;
            this.lblSchemaName.Text = "Schema:";
            // 
            // btnConnectAgent
            // 
            this.btnConnectAgent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnectAgent.Location = new System.Drawing.Point(590, 113);
            this.btnConnectAgent.Name = "btnConnectAgent";
            this.btnConnectAgent.Size = new System.Drawing.Size(75, 23);
            this.btnConnectAgent.TabIndex = 5;
            this.btnConnectAgent.Text = "Connect";
            this.btnConnectAgent.UseVisualStyleBackColor = true;
            this.btnConnectAgent.Click += new System.EventHandler(this.btnConnectAgent_Click);
            // 
            // lblEtlAgent
            // 
            this.lblEtlAgent.AutoSize = true;
            this.lblEtlAgent.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEtlAgent.Location = new System.Drawing.Point(11, 10);
            this.lblEtlAgent.Name = "lblEtlAgent";
            this.lblEtlAgent.Size = new System.Drawing.Size(63, 13);
            this.lblEtlAgent.TabIndex = 13;
            this.lblEtlAgent.Text = "ETL Agent";
            // 
            // cmbEtlAgentType
            // 
            this.cmbEtlAgentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEtlAgentType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEtlAgentType.DataSource = this.bndEtlAgentTypes;
            this.cmbEtlAgentType.FormattingEnabled = true;
            this.cmbEtlAgentType.Location = new System.Drawing.Point(112, 32);
            this.cmbEtlAgentType.Name = "cmbEtlAgentType";
            this.cmbEtlAgentType.Size = new System.Drawing.Size(551, 21);
            this.cmbEtlAgentType.TabIndex = 1;
            this.cmbEtlAgentType.TextChanged += new System.EventHandler(this.cmbEtlAgentType_TextChanged);
            // 
            // panAgentInfo
            // 
            this.panAgentInfo.Controls.Add(this.cmbEtlAgentType);
            this.panAgentInfo.Controls.Add(this.lblEtlAgent);
            this.panAgentInfo.Controls.Add(this.btnConnectAgent);
            this.panAgentInfo.Controls.Add(this.lblSchemaName);
            this.panAgentInfo.Controls.Add(this.lblConnectionString);
            this.panAgentInfo.Controls.Add(this.txtSchemaName);
            this.panAgentInfo.Controls.Add(this.txtConnectionString);
            this.panAgentInfo.Controls.Add(this.lblEtlAgentType);
            this.panAgentInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panAgentInfo.Location = new System.Drawing.Point(0, 0);
            this.panAgentInfo.Name = "panAgentInfo";
            this.panAgentInfo.Size = new System.Drawing.Size(675, 141);
            this.panAgentInfo.TabIndex = 3;
            // 
            // sstMain
            // 
            this.sstMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.sstMain.Location = new System.Drawing.Point(0, 600);
            this.sstMain.Name = "sstMain";
            this.sstMain.Size = new System.Drawing.Size(675, 22);
            this.sstMain.TabIndex = 6;
            this.sstMain.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 17);
            this.lblStatus.Text = "status";
            // 
            // AgentExplorerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panAgentItems);
            this.Controls.Add(this.sstMain);
            this.Controls.Add(this.panAgentInfo);
            this.Name = "AgentExplorerControl";
            this.Size = new System.Drawing.Size(675, 622);
            this.Load += new System.EventHandler(this.AgentExplorer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bndPackages)).EndInit();
            this.tbcAgentItems.ResumeLayout(false);
            this.tabPackages.ResumeLayout(false);
            this.tabPackages.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPackages)).EndInit();
            this.tlsPackages.ResumeLayout(false);
            this.tlsPackages.PerformLayout();
            this.panAgentItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bndEtlAgentTypes)).EndInit();
            this.panAgentInfo.ResumeLayout(false);
            this.panAgentInfo.PerformLayout();
            this.sstMain.ResumeLayout(false);
            this.sstMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip miniToolStrip;
        private System.Windows.Forms.BindingSource bndPackages;
        private System.Windows.Forms.TabControl tbcAgentItems;
        private System.Windows.Forms.TabPage tabPackages;
        private System.Windows.Forms.DataGridView grdPackages;
        private System.Windows.Forms.ToolStrip tlsPackages;
        private System.Windows.Forms.Panel panAgentItems;
        private System.Windows.Forms.BindingSource bndEtlAgentTypes;
        private System.Windows.Forms.Label lblEtlAgentType;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.TextBox txtSchemaName;
        private System.Windows.Forms.Label lblConnectionString;
        private System.Windows.Forms.Label lblSchemaName;
        private System.Windows.Forms.Button btnConnectAgent;
        private System.Windows.Forms.Label lblEtlAgent;
        private System.Windows.Forms.ComboBox cmbEtlAgentType;
        private System.Windows.Forms.Panel panAgentInfo;
        private System.Windows.Forms.StatusStrip sstMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnNewPackage;
        private System.Windows.Forms.ToolStripButton btnOpenPackage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnRefreshPackages;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnCopyPackageTo;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageStepName;
    }
}
