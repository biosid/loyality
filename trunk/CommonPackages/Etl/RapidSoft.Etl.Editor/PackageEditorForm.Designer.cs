namespace RapidSoft.Etl.Editor
{
    partial class PackageEditorForm
    {
        /// <summary>
        /// Required designer parameter.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param stepName="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PackageEditorForm));
            this.panWorkspace = new System.Windows.Forms.Panel();
            this.panDocument = new System.Windows.Forms.Panel();
            this.tabDocuments = new System.Windows.Forms.TabControl();
            this.tabSteps = new System.Windows.Forms.TabPage();
            this.panSteps = new System.Windows.Forms.Panel();
            this.grdPackageSteps = new System.Windows.Forms.DataGridView();
            this.colRowNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageStepType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPackageStepName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndPackageSteps = new System.Windows.Forms.BindingSource(this.components);
            this.tlsPackageMenu = new System.Windows.Forms.ToolStrip();
            this.lblSteps = new System.Windows.Forms.ToolStripLabel();
            this.btnAddPackageStep = new System.Windows.Forms.ToolStripSplitButton();
            this.btnMoveUpPackageStep = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDownPackageStep = new System.Windows.Forms.ToolStripButton();
            this.sepRemoveStep = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemovePackageStep = new System.Windows.Forms.ToolStripButton();
            this.splSteps = new System.Windows.Forms.Splitter();
            this.panVariables = new System.Windows.Forms.Panel();
            this.grdVariables = new System.Windows.Forms.DataGridView();
            this.colVariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVariableModifier = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVariableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVariableBinding = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParameterUse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndVariables = new System.Windows.Forms.BindingSource(this.components);
            this.tlsVariables = new System.Windows.Forms.ToolStrip();
            this.lblVariables = new System.Windows.Forms.ToolStripLabel();
            this.btnAddVariable = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUpVariable = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDownVariable = new System.Windows.Forms.ToolStripButton();
            this.sepRemoveVariable = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemoveVariable = new System.Windows.Forms.ToolStripButton();
            this.tabPackageXml = new System.Windows.Forms.TabPage();
            this.txtPackageXml = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkPackageXmlWordWrap = new System.Windows.Forms.CheckBox();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.panMessage = new System.Windows.Forms.Panel();
            this.grdMessages = new System.Windows.Forms.DataGridView();
            this.colSequentialId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessageLogDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessageStep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessageType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessageText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrace = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndMessages = new System.Windows.Forms.BindingSource(this.components);
            this.txtLogMessage = new System.Windows.Forms.TextBox();
            this.splSessions = new System.Windows.Forms.Splitter();
            this.panCounters = new System.Windows.Forms.Panel();
            this.grdCounters = new System.Windows.Forms.DataGridView();
            this.bndCounters = new System.Windows.Forms.BindingSource(this.components);
            this.panSessions = new System.Windows.Forms.Panel();
            this.grdSessions = new System.Windows.Forms.DataGridView();
            this.colSessionId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSessionStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bndSessions = new System.Windows.Forms.BindingSource(this.components);
            this.splDocument = new System.Windows.Forms.Splitter();
            this.panBars = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.bndBindings = new System.Windows.Forms.BindingSource(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tlsMainMenu = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSaveAndRun = new System.Windows.Forms.ToolStripButton();
            this.saveNewFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAgentConnectionString = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAgentSchema = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.panPackageInfo = new System.Windows.Forms.Panel();
            this.txtPackageId = new System.Windows.Forms.TextBox();
            this.lblPackageId = new System.Windows.Forms.Label();
            this.txtPackageName = new System.Windows.Forms.TextBox();
            this.lblPackageName = new System.Windows.Forms.Label();
            this.colCounterEntity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogVariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogVariableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLogVariableDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panWorkspace.SuspendLayout();
            this.panDocument.SuspendLayout();
            this.tabDocuments.SuspendLayout();
            this.tabSteps.SuspendLayout();
            this.panSteps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPackageSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndPackageSteps)).BeginInit();
            this.tlsPackageMenu.SuspendLayout();
            this.panVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndVariables)).BeginInit();
            this.tlsVariables.SuspendLayout();
            this.tabPackageXml.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabLog.SuspendLayout();
            this.panMessage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndMessages)).BeginInit();
            this.panCounters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCounters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndCounters)).BeginInit();
            this.panSessions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSessions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSessions)).BeginInit();
            this.panBars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bndBindings)).BeginInit();
            this.tlsMainMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.panPackageInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panWorkspace
            // 
            this.panWorkspace.Controls.Add(this.panDocument);
            this.panWorkspace.Controls.Add(this.splDocument);
            this.panWorkspace.Controls.Add(this.panBars);
            this.panWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panWorkspace.Location = new System.Drawing.Point(0, 90);
            this.panWorkspace.Name = "panWorkspace";
            this.panWorkspace.Size = new System.Drawing.Size(746, 493);
            this.panWorkspace.TabIndex = 1;
            // 
            // panDocument
            // 
            this.panDocument.Controls.Add(this.tabDocuments);
            this.panDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panDocument.Location = new System.Drawing.Point(0, 0);
            this.panDocument.Name = "panDocument";
            this.panDocument.Size = new System.Drawing.Size(405, 493);
            this.panDocument.TabIndex = 0;
            // 
            // tabDocuments
            // 
            this.tabDocuments.Controls.Add(this.tabSteps);
            this.tabDocuments.Controls.Add(this.tabPackageXml);
            this.tabDocuments.Controls.Add(this.tabLog);
            this.tabDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDocuments.Location = new System.Drawing.Point(0, 0);
            this.tabDocuments.Name = "tabDocuments";
            this.tabDocuments.SelectedIndex = 0;
            this.tabDocuments.Size = new System.Drawing.Size(405, 493);
            this.tabDocuments.TabIndex = 0;
            this.tabDocuments.SelectedIndexChanged += new System.EventHandler(this.tabDocuments_SelectedIndexChanged);
            // 
            // tabSteps
            // 
            this.tabSteps.Controls.Add(this.panSteps);
            this.tabSteps.Controls.Add(this.splSteps);
            this.tabSteps.Controls.Add(this.panVariables);
            this.tabSteps.Location = new System.Drawing.Point(4, 22);
            this.tabSteps.Name = "tabSteps";
            this.tabSteps.Padding = new System.Windows.Forms.Padding(3);
            this.tabSteps.Size = new System.Drawing.Size(397, 467);
            this.tabSteps.TabIndex = 0;
            this.tabSteps.Text = "Steps";
            this.tabSteps.UseVisualStyleBackColor = true;
            // 
            // panSteps
            // 
            this.panSteps.Controls.Add(this.grdPackageSteps);
            this.panSteps.Controls.Add(this.tlsPackageMenu);
            this.panSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSteps.Location = new System.Drawing.Point(3, 106);
            this.panSteps.Name = "panSteps";
            this.panSteps.Size = new System.Drawing.Size(391, 358);
            this.panSteps.TabIndex = 4;
            // 
            // grdPackageSteps
            // 
            this.grdPackageSteps.AllowUserToAddRows = false;
            this.grdPackageSteps.AllowUserToDeleteRows = false;
            this.grdPackageSteps.AllowUserToOrderColumns = true;
            this.grdPackageSteps.AllowUserToResizeRows = false;
            this.grdPackageSteps.AutoGenerateColumns = false;
            this.grdPackageSteps.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdPackageSteps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdPackageSteps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRowNumber,
            this.colPackageStepType,
            this.colPackageStepName});
            this.grdPackageSteps.DataSource = this.bndPackageSteps;
            this.grdPackageSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdPackageSteps.Location = new System.Drawing.Point(0, 25);
            this.grdPackageSteps.MultiSelect = false;
            this.grdPackageSteps.Name = "grdPackageSteps";
            this.grdPackageSteps.ReadOnly = true;
            this.grdPackageSteps.RowHeadersVisible = false;
            this.grdPackageSteps.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdPackageSteps.Size = new System.Drawing.Size(391, 333);
            this.grdPackageSteps.StandardTab = true;
            this.grdPackageSteps.TabIndex = 2;
            this.grdPackageSteps.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdPackageSteps_CellFormatting);
            this.grdPackageSteps.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPackageSteps_RowEnter);
            // 
            // colRowNumber
            // 
            this.colRowNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colRowNumber.DefaultCellStyle = dataGridViewCellStyle1;
            this.colRowNumber.FillWeight = 41.49897F;
            this.colRowNumber.HeaderText = "N";
            this.colRowNumber.Name = "colRowNumber";
            this.colRowNumber.ReadOnly = true;
            this.colRowNumber.Width = 39;
            // 
            // colPackageStepType
            // 
            this.colPackageStepType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colPackageStepType.HeaderText = "Type";
            this.colPackageStepType.Name = "colPackageStepType";
            this.colPackageStepType.ReadOnly = true;
            this.colPackageStepType.Width = 56;
            // 
            // colPackageStepName
            // 
            this.colPackageStepName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPackageStepName.DataPropertyName = "Name";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colPackageStepName.DefaultCellStyle = dataGridViewCellStyle2;
            this.colPackageStepName.FillWeight = 132.4435F;
            this.colPackageStepName.HeaderText = "Name";
            this.colPackageStepName.Name = "colPackageStepName";
            this.colPackageStepName.ReadOnly = true;
            // 
            // tlsPackageMenu
            // 
            this.tlsPackageMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSteps,
            this.btnAddPackageStep,
            this.btnMoveUpPackageStep,
            this.btnMoveDownPackageStep,
            this.sepRemoveStep,
            this.btnRemovePackageStep});
            this.tlsPackageMenu.Location = new System.Drawing.Point(0, 0);
            this.tlsPackageMenu.Name = "tlsPackageMenu";
            this.tlsPackageMenu.Size = new System.Drawing.Size(391, 25);
            this.tlsPackageMenu.TabIndex = 3;
            this.tlsPackageMenu.Text = "toolStrip1";
            // 
            // lblSteps
            // 
            this.lblSteps.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(41, 22);
            this.lblSteps.Text = "Steps:";
            // 
            // btnAddPackageStep
            // 
            this.btnAddPackageStep.Image = global::RapidSoft.Etl.Editor.Properties.Resources.AddTable;
            this.btnAddPackageStep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPackageStep.Name = "btnAddPackageStep";
            this.btnAddPackageStep.Size = new System.Drawing.Size(61, 22);
            this.btnAddPackageStep.Text = "Add";
            // 
            // btnMoveUpPackageStep
            // 
            this.btnMoveUpPackageStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUpPackageStep.Image = global::RapidSoft.Etl.Editor.Properties.Resources.MoveUp;
            this.btnMoveUpPackageStep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUpPackageStep.Name = "btnMoveUpPackageStep";
            this.btnMoveUpPackageStep.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUpPackageStep.Text = "Move Up";
            this.btnMoveUpPackageStep.Click += new System.EventHandler(this.btnMoveUpPackageStep_Click);
            // 
            // btnMoveDownPackageStep
            // 
            this.btnMoveDownPackageStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDownPackageStep.Image = global::RapidSoft.Etl.Editor.Properties.Resources.MoveDown;
            this.btnMoveDownPackageStep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDownPackageStep.Name = "btnMoveDownPackageStep";
            this.btnMoveDownPackageStep.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDownPackageStep.Text = "Move Down";
            this.btnMoveDownPackageStep.Click += new System.EventHandler(this.btnMoveDownPackageStep_Click);
            // 
            // sepRemoveStep
            // 
            this.sepRemoveStep.Name = "sepRemoveStep";
            this.sepRemoveStep.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemovePackageStep
            // 
            this.btnRemovePackageStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemovePackageStep.Image = global::RapidSoft.Etl.Editor.Properties.Resources.DeleteTable;
            this.btnRemovePackageStep.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemovePackageStep.Name = "btnRemovePackageStep";
            this.btnRemovePackageStep.Size = new System.Drawing.Size(23, 22);
            this.btnRemovePackageStep.Text = "Delete";
            this.btnRemovePackageStep.Click += new System.EventHandler(this.btnRemovePackageStep_Click);
            // 
            // splSteps
            // 
            this.splSteps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splSteps.Dock = System.Windows.Forms.DockStyle.Top;
            this.splSteps.Location = new System.Drawing.Point(3, 103);
            this.splSteps.Name = "splSteps";
            this.splSteps.Size = new System.Drawing.Size(391, 3);
            this.splSteps.TabIndex = 3;
            this.splSteps.TabStop = false;
            // 
            // panVariables
            // 
            this.panVariables.BackColor = System.Drawing.SystemColors.Control;
            this.panVariables.Controls.Add(this.grdVariables);
            this.panVariables.Controls.Add(this.tlsVariables);
            this.panVariables.Dock = System.Windows.Forms.DockStyle.Top;
            this.panVariables.Location = new System.Drawing.Point(3, 3);
            this.panVariables.Name = "panVariables";
            this.panVariables.Size = new System.Drawing.Size(391, 100);
            this.panVariables.TabIndex = 2;
            // 
            // grdVariables
            // 
            this.grdVariables.AllowUserToAddRows = false;
            this.grdVariables.AllowUserToDeleteRows = false;
            this.grdVariables.AllowUserToOrderColumns = true;
            this.grdVariables.AllowUserToResizeRows = false;
            this.grdVariables.AutoGenerateColumns = false;
            this.grdVariables.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVariableName,
            this.colVariableModifier,
            this.colVariableValue,
            this.colVariableBinding,
            this.colParameterUse});
            this.grdVariables.DataSource = this.bndVariables;
            this.grdVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdVariables.Location = new System.Drawing.Point(0, 25);
            this.grdVariables.MultiSelect = false;
            this.grdVariables.Name = "grdVariables";
            this.grdVariables.ReadOnly = true;
            this.grdVariables.RowHeadersVisible = false;
            this.grdVariables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdVariables.Size = new System.Drawing.Size(391, 75);
            this.grdVariables.StandardTab = true;
            this.grdVariables.TabIndex = 2;
            this.grdVariables.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdParameters_CellFormatting);
            this.grdVariables.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdVariables_RowEnter);
            // 
            // colVariableName
            // 
            this.colVariableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colVariableName.DataPropertyName = "Name";
            this.colVariableName.FillWeight = 84.41341F;
            this.colVariableName.HeaderText = "Name";
            this.colVariableName.Name = "colVariableName";
            this.colVariableName.ReadOnly = true;
            this.colVariableName.Width = 59;
            // 
            // colVariableModifier
            // 
            this.colVariableModifier.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colVariableModifier.DataPropertyName = "Modifier";
            this.colVariableModifier.HeaderText = "Modifier";
            this.colVariableModifier.Name = "colVariableModifier";
            this.colVariableModifier.ReadOnly = true;
            this.colVariableModifier.Width = 70;
            // 
            // colVariableValue
            // 
            this.colVariableValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colVariableValue.DataPropertyName = "DefaultValue";
            this.colVariableValue.FillWeight = 126.7963F;
            this.colVariableValue.HeaderText = "DefaultValue";
            this.colVariableValue.Name = "colVariableValue";
            this.colVariableValue.ReadOnly = true;
            // 
            // colVariableBinding
            // 
            this.colVariableBinding.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colVariableBinding.DataPropertyName = "Binding";
            this.colVariableBinding.FillWeight = 26.35371F;
            this.colVariableBinding.HeaderText = "Binding";
            this.colVariableBinding.Name = "colVariableBinding";
            this.colVariableBinding.ReadOnly = true;
            this.colVariableBinding.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colVariableBinding.Width = 66;
            // 
            // colParameterUse
            // 
            this.colParameterUse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colParameterUse.FillWeight = 162.4365F;
            this.colParameterUse.HeaderText = "How To Use";
            this.colParameterUse.Name = "colParameterUse";
            this.colParameterUse.ReadOnly = true;
            this.colParameterUse.Width = 89;
            // 
            // tlsVariables
            // 
            this.tlsVariables.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblVariables,
            this.btnAddVariable,
            this.btnMoveUpVariable,
            this.btnMoveDownVariable,
            this.sepRemoveVariable,
            this.btnRemoveVariable});
            this.tlsVariables.Location = new System.Drawing.Point(0, 0);
            this.tlsVariables.Name = "tlsVariables";
            this.tlsVariables.Size = new System.Drawing.Size(391, 25);
            this.tlsVariables.TabIndex = 2;
            this.tlsVariables.Text = "toolStrip1";
            // 
            // lblVariables
            // 
            this.lblVariables.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(60, 22);
            this.lblVariables.Text = "Variables:";
            // 
            // btnAddVariable
            // 
            this.btnAddVariable.Image = global::RapidSoft.Etl.Editor.Properties.Resources.AddTable;
            this.btnAddVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddVariable.Name = "btnAddVariable";
            this.btnAddVariable.Size = new System.Drawing.Size(49, 22);
            this.btnAddVariable.Text = "Add";
            this.btnAddVariable.Click += new System.EventHandler(this.btnAddParameter_Click);
            // 
            // btnMoveUpVariable
            // 
            this.btnMoveUpVariable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveUpVariable.Image = global::RapidSoft.Etl.Editor.Properties.Resources.MoveUp;
            this.btnMoveUpVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUpVariable.Name = "btnMoveUpVariable";
            this.btnMoveUpVariable.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUpVariable.Text = "Move Up";
            this.btnMoveUpVariable.Click += new System.EventHandler(this.btnMoveUpParameter_Click);
            // 
            // btnMoveDownVariable
            // 
            this.btnMoveDownVariable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveDownVariable.Image = global::RapidSoft.Etl.Editor.Properties.Resources.MoveDown;
            this.btnMoveDownVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDownVariable.Name = "btnMoveDownVariable";
            this.btnMoveDownVariable.Size = new System.Drawing.Size(23, 22);
            this.btnMoveDownVariable.Text = "Move Down";
            this.btnMoveDownVariable.Click += new System.EventHandler(this.btnMoveDownParameter_Click);
            // 
            // sepRemoveVariable
            // 
            this.sepRemoveVariable.Name = "sepRemoveVariable";
            this.sepRemoveVariable.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemoveVariable
            // 
            this.btnRemoveVariable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemoveVariable.Image = global::RapidSoft.Etl.Editor.Properties.Resources.DeleteTable;
            this.btnRemoveVariable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveVariable.Name = "btnRemoveVariable";
            this.btnRemoveVariable.Size = new System.Drawing.Size(23, 22);
            this.btnRemoveVariable.Text = "Delete";
            this.btnRemoveVariable.Click += new System.EventHandler(this.btnRemoveParameter_Click);
            // 
            // tabPackageXml
            // 
            this.tabPackageXml.Controls.Add(this.txtPackageXml);
            this.tabPackageXml.Controls.Add(this.panel1);
            this.tabPackageXml.Location = new System.Drawing.Point(4, 22);
            this.tabPackageXml.Name = "tabPackageXml";
            this.tabPackageXml.Padding = new System.Windows.Forms.Padding(3);
            this.tabPackageXml.Size = new System.Drawing.Size(397, 467);
            this.tabPackageXml.TabIndex = 2;
            this.tabPackageXml.Text = "XML";
            this.tabPackageXml.UseVisualStyleBackColor = true;
            // 
            // txtPackageXml
            // 
            this.txtPackageXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPackageXml.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtPackageXml.Location = new System.Drawing.Point(3, 3);
            this.txtPackageXml.Multiline = true;
            this.txtPackageXml.Name = "txtPackageXml";
            this.txtPackageXml.ReadOnly = true;
            this.txtPackageXml.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPackageXml.Size = new System.Drawing.Size(391, 427);
            this.txtPackageXml.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkPackageXmlWordWrap);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 430);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(391, 34);
            this.panel1.TabIndex = 1;
            // 
            // chkPackageXmlWordWrap
            // 
            this.chkPackageXmlWordWrap.AutoSize = true;
            this.chkPackageXmlWordWrap.Location = new System.Drawing.Point(6, 7);
            this.chkPackageXmlWordWrap.Name = "chkPackageXmlWordWrap";
            this.chkPackageXmlWordWrap.Size = new System.Drawing.Size(78, 17);
            this.chkPackageXmlWordWrap.TabIndex = 0;
            this.chkPackageXmlWordWrap.Text = "Word wrap";
            this.chkPackageXmlWordWrap.UseVisualStyleBackColor = true;
            this.chkPackageXmlWordWrap.CheckedChanged += new System.EventHandler(this.chkPackageXmlWordWrap_CheckedChanged);
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.panMessage);
            this.tabLog.Controls.Add(this.splSessions);
            this.tabLog.Controls.Add(this.panCounters);
            this.tabLog.Controls.Add(this.panSessions);
            this.tabLog.Location = new System.Drawing.Point(4, 22);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabLog.Size = new System.Drawing.Size(397, 467);
            this.tabLog.TabIndex = 1;
            this.tabLog.Text = "Log";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // panMessage
            // 
            this.panMessage.BackColor = System.Drawing.SystemColors.Control;
            this.panMessage.Controls.Add(this.grdMessages);
            this.panMessage.Controls.Add(this.txtLogMessage);
            this.panMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMessage.Location = new System.Drawing.Point(3, 166);
            this.panMessage.Name = "panMessage";
            this.panMessage.Size = new System.Drawing.Size(391, 298);
            this.panMessage.TabIndex = 2;
            // 
            // grdMessages
            // 
            this.grdMessages.AllowUserToAddRows = false;
            this.grdMessages.AllowUserToDeleteRows = false;
            this.grdMessages.AllowUserToOrderColumns = true;
            this.grdMessages.AllowUserToResizeRows = false;
            this.grdMessages.AutoGenerateColumns = false;
            this.grdMessages.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSequentialId,
            this.colMessageLogDateTime,
            this.colMessageStep,
            this.colMessageType,
            this.colMessageText,
            this.colTrace});
            this.grdMessages.DataSource = this.bndMessages;
            this.grdMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMessages.Location = new System.Drawing.Point(0, 0);
            this.grdMessages.Name = "grdMessages";
            this.grdMessages.ReadOnly = true;
            this.grdMessages.RowHeadersVisible = false;
            this.grdMessages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdMessages.Size = new System.Drawing.Size(391, 234);
            this.grdMessages.StandardTab = true;
            this.grdMessages.TabIndex = 0;
            this.grdMessages.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdMessages_CellFormatting);
            this.grdMessages.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMessages_RowEnter);
            // 
            // colSequentialId
            // 
            this.colSequentialId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSequentialId.DataPropertyName = "SequentialId";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colSequentialId.DefaultCellStyle = dataGridViewCellStyle3;
            this.colSequentialId.FillWeight = 60.9137F;
            this.colSequentialId.HeaderText = "N";
            this.colSequentialId.Name = "colSequentialId";
            this.colSequentialId.ReadOnly = true;
            this.colSequentialId.Width = 39;
            // 
            // colMessageLogDateTime
            // 
            this.colMessageLogDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMessageLogDateTime.DataPropertyName = "LogDateTime";
            this.colMessageLogDateTime.HeaderText = "Log DateTime";
            this.colMessageLogDateTime.Name = "colMessageLogDateTime";
            this.colMessageLogDateTime.ReadOnly = true;
            this.colMessageLogDateTime.Width = 97;
            // 
            // colMessageStep
            // 
            this.colMessageStep.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMessageStep.DataPropertyName = "EtlStepName";
            this.colMessageStep.HeaderText = "Step";
            this.colMessageStep.Name = "colMessageStep";
            this.colMessageStep.ReadOnly = true;
            this.colMessageStep.Width = 54;
            // 
            // colMessageType
            // 
            this.colMessageType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMessageType.DataPropertyName = "MessageType";
            this.colMessageType.FillWeight = 142.1728F;
            this.colMessageType.HeaderText = "Type";
            this.colMessageType.Name = "colMessageType";
            this.colMessageType.ReadOnly = true;
            this.colMessageType.Width = 56;
            // 
            // colMessageText
            // 
            this.colMessageText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMessageText.DataPropertyName = "Text";
            this.colMessageText.FillWeight = 142.1728F;
            this.colMessageText.HeaderText = "Message";
            this.colMessageText.Name = "colMessageText";
            this.colMessageText.ReadOnly = true;
            this.colMessageText.Width = 74;
            // 
            // colTrace
            // 
            this.colTrace.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colTrace.DataPropertyName = "StackTrace";
            this.colTrace.HeaderText = "Trace";
            this.colTrace.Name = "colTrace";
            this.colTrace.ReadOnly = true;
            // 
            // bndMessages
            // 
            this.bndMessages.AllowNew = true;
            // 
            // txtLogMessage
            // 
            this.txtLogMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLogMessage.Location = new System.Drawing.Point(0, 234);
            this.txtLogMessage.Multiline = true;
            this.txtLogMessage.Name = "txtLogMessage";
            this.txtLogMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogMessage.Size = new System.Drawing.Size(391, 64);
            this.txtLogMessage.TabIndex = 1;
            // 
            // splSessions
            // 
            this.splSessions.Dock = System.Windows.Forms.DockStyle.Top;
            this.splSessions.Location = new System.Drawing.Point(3, 163);
            this.splSessions.Name = "splSessions";
            this.splSessions.Size = new System.Drawing.Size(391, 3);
            this.splSessions.TabIndex = 3;
            this.splSessions.TabStop = false;
            // 
            // panCounters
            // 
            this.panCounters.BackColor = System.Drawing.SystemColors.Control;
            this.panCounters.Controls.Add(this.grdCounters);
            this.panCounters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panCounters.Location = new System.Drawing.Point(3, 63);
            this.panCounters.Name = "panCounters";
            this.panCounters.Size = new System.Drawing.Size(391, 100);
            this.panCounters.TabIndex = 4;
            // 
            // grdCounters
            // 
            this.grdCounters.AllowUserToAddRows = false;
            this.grdCounters.AllowUserToDeleteRows = false;
            this.grdCounters.AllowUserToOrderColumns = true;
            this.grdCounters.AllowUserToResizeRows = false;
            this.grdCounters.AutoGenerateColumns = false;
            this.grdCounters.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdCounters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCounterEntity,
            this.colLogVariableName,
            this.colLogVariableValue,
            this.colLogVariableDateTime});
            this.grdCounters.DataSource = this.bndCounters;
            this.grdCounters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCounters.Location = new System.Drawing.Point(0, 0);
            this.grdCounters.Name = "grdCounters";
            this.grdCounters.ReadOnly = true;
            this.grdCounters.RowHeadersVisible = false;
            this.grdCounters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdCounters.Size = new System.Drawing.Size(391, 100);
            this.grdCounters.StandardTab = true;
            this.grdCounters.TabIndex = 0;
            // 
            // panSessions
            // 
            this.panSessions.BackColor = System.Drawing.SystemColors.Control;
            this.panSessions.Controls.Add(this.grdSessions);
            this.panSessions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSessions.Location = new System.Drawing.Point(3, 3);
            this.panSessions.Name = "panSessions";
            this.panSessions.Size = new System.Drawing.Size(391, 60);
            this.panSessions.TabIndex = 1;
            // 
            // grdSessions
            // 
            this.grdSessions.AllowUserToAddRows = false;
            this.grdSessions.AllowUserToDeleteRows = false;
            this.grdSessions.AllowUserToOrderColumns = true;
            this.grdSessions.AllowUserToResizeRows = false;
            this.grdSessions.AutoGenerateColumns = false;
            this.grdSessions.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grdSessions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSessions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSessionId,
            this.colSessionStatus});
            this.grdSessions.DataSource = this.bndSessions;
            this.grdSessions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSessions.Location = new System.Drawing.Point(0, 0);
            this.grdSessions.Name = "grdSessions";
            this.grdSessions.ReadOnly = true;
            this.grdSessions.RowHeadersVisible = false;
            this.grdSessions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSessions.Size = new System.Drawing.Size(391, 60);
            this.grdSessions.StandardTab = true;
            this.grdSessions.TabIndex = 0;
            this.grdSessions.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSessions_RowEnter);
            // 
            // colSessionId
            // 
            this.colSessionId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSessionId.DataPropertyName = "EtlSessionId";
            this.colSessionId.HeaderText = "SessionId";
            this.colSessionId.Name = "colSessionId";
            this.colSessionId.ReadOnly = true;
            this.colSessionId.Width = 78;
            // 
            // colSessionStatus
            // 
            this.colSessionStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSessionStatus.DataPropertyName = "Status";
            this.colSessionStatus.HeaderText = "Status";
            this.colSessionStatus.Name = "colSessionStatus";
            this.colSessionStatus.ReadOnly = true;
            this.colSessionStatus.Width = 63;
            // 
            // bndSessions
            // 
            this.bndSessions.AllowNew = false;
            // 
            // splDocument
            // 
            this.splDocument.Dock = System.Windows.Forms.DockStyle.Right;
            this.splDocument.Location = new System.Drawing.Point(405, 0);
            this.splDocument.Name = "splDocument";
            this.splDocument.Size = new System.Drawing.Size(3, 493);
            this.splDocument.TabIndex = 2;
            this.splDocument.TabStop = false;
            // 
            // panBars
            // 
            this.panBars.Controls.Add(this.propertyGrid);
            this.panBars.Dock = System.Windows.Forms.DockStyle.Right;
            this.panBars.Location = new System.Drawing.Point(408, 0);
            this.panBars.Name = "panBars";
            this.panBars.Size = new System.Drawing.Size(338, 493);
            this.panBars.TabIndex = 1;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(338, 493);
            this.propertyGrid.TabIndex = 3;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // bndBindings
            // 
            this.bndBindings.AllowNew = false;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "etl.xml";
            this.openFileDialog.Filter = "ReportTypeInfo files (*.etl.xml)|";
            this.openFileDialog.SupportMultiDottedExtensions = true;
            this.openFileDialog.Title = "Open ETL Package";
            // 
            // tlsMainMenu
            // 
            this.tlsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.toolStripSeparator2,
            this.btnSaveAndRun});
            this.tlsMainMenu.Location = new System.Drawing.Point(0, 0);
            this.tlsMainMenu.Name = "tlsMainMenu";
            this.tlsMainMenu.Size = new System.Drawing.Size(746, 25);
            this.tlsMainMenu.TabIndex = 2;
            this.tlsMainMenu.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::RapidSoft.Etl.Editor.Properties.Resources.Save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(51, 22);
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSaveAndRun
            // 
            this.btnSaveAndRun.Image = global::RapidSoft.Etl.Editor.Properties.Resources.PlayHS;
            this.btnSaveAndRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAndRun.Name = "btnSaveAndRun";
            this.btnSaveAndRun.Size = new System.Drawing.Size(88, 22);
            this.btnSaveAndRun.Text = "Save && &Run";
            this.btnSaveAndRun.Click += new System.EventHandler(this.btnSaveAndRun_Click);
            // 
            // saveNewFileDialog
            // 
            this.saveNewFileDialog.DefaultExt = "etl.xml";
            this.saveNewFileDialog.Filter = "ReportTypeInfo files (*.etl.xml)|";
            this.saveNewFileDialog.SupportMultiDottedExtensions = true;
            this.saveNewFileDialog.Title = "Save New ETL Package As...";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.progressBar,
            this.toolStripStatusLabel1,
            this.lblAgentConnectionString,
            this.toolStripStatusLabel2,
            this.lblAgentSchema});
            this.statusBar.Location = new System.Drawing.Point(0, 583);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(746, 22);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(38, 17);
            this.lblStatus.Text = "status";
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.progressBar.Size = new System.Drawing.Size(280, 16);
            this.progressBar.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // lblAgentConnectionString
            // 
            this.lblAgentConnectionString.Name = "lblAgentConnectionString";
            this.lblAgentConnectionString.Size = new System.Drawing.Size(37, 17);
            this.lblAgentConnectionString.Text = "agent";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // lblAgentSchema
            // 
            this.lblAgentSchema.Name = "lblAgentSchema";
            this.lblAgentSchema.Size = new System.Drawing.Size(81, 17);
            this.lblAgentSchema.Text = "agent schema";
            // 
            // tmrStatus
            // 
            this.tmrStatus.Interval = 3000;
            this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
            // 
            // panPackageInfo
            // 
            this.panPackageInfo.Controls.Add(this.txtPackageId);
            this.panPackageInfo.Controls.Add(this.lblPackageId);
            this.panPackageInfo.Controls.Add(this.txtPackageName);
            this.panPackageInfo.Controls.Add(this.lblPackageName);
            this.panPackageInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panPackageInfo.Location = new System.Drawing.Point(0, 25);
            this.panPackageInfo.Name = "panPackageInfo";
            this.panPackageInfo.Size = new System.Drawing.Size(746, 65);
            this.panPackageInfo.TabIndex = 4;
            // 
            // txtPackageId
            // 
            this.txtPackageId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackageId.Location = new System.Drawing.Point(58, 6);
            this.txtPackageId.Name = "txtPackageId";
            this.txtPackageId.ReadOnly = true;
            this.txtPackageId.Size = new System.Drawing.Size(676, 21);
            this.txtPackageId.TabIndex = 4;
            // 
            // lblPackageId
            // 
            this.lblPackageId.AutoSize = true;
            this.lblPackageId.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPackageId.Location = new System.Drawing.Point(13, 7);
            this.lblPackageId.Name = "lblPackageId";
            this.lblPackageId.Size = new System.Drawing.Size(21, 13);
            this.lblPackageId.TabIndex = 6;
            this.lblPackageId.Text = "Id:";
            // 
            // txtPackageName
            // 
            this.txtPackageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackageName.Location = new System.Drawing.Point(58, 32);
            this.txtPackageName.Name = "txtPackageName";
            this.txtPackageName.Size = new System.Drawing.Size(676, 21);
            this.txtPackageName.TabIndex = 5;
            // 
            // lblPackageName
            // 
            this.lblPackageName.AutoSize = true;
            this.lblPackageName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPackageName.Location = new System.Drawing.Point(11, 33);
            this.lblPackageName.Name = "lblPackageName";
            this.lblPackageName.Size = new System.Drawing.Size(38, 13);
            this.lblPackageName.TabIndex = 3;
            this.lblPackageName.Text = "Name:";
            // 
            // colCounterEntity
            // 
            this.colCounterEntity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colCounterEntity.DataPropertyName = "EntityName";
            this.colCounterEntity.HeaderText = "Entity";
            this.colCounterEntity.Name = "colCounterEntity";
            this.colCounterEntity.ReadOnly = true;
            this.colCounterEntity.Width = 60;
            // 
            // colLogVariableName
            // 
            this.colLogVariableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLogVariableName.DataPropertyName = "CounterName";
            this.colLogVariableName.HeaderText = "Counter";
            this.colLogVariableName.Name = "colLogVariableName";
            this.colLogVariableName.ReadOnly = true;
            this.colLogVariableName.Width = 71;
            // 
            // colLogVariableValue
            // 
            this.colLogVariableValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLogVariableValue.DataPropertyName = "CounterValue";
            this.colLogVariableValue.HeaderText = "Value";
            this.colLogVariableValue.Name = "colLogVariableValue";
            this.colLogVariableValue.ReadOnly = true;
            // 
            // colLogVariableDateTime
            // 
            this.colLogVariableDateTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colLogVariableDateTime.DataPropertyName = "DateTime";
            this.colLogVariableDateTime.HeaderText = "Date&Time";
            this.colLogVariableDateTime.Name = "colLogVariableDateTime";
            this.colLogVariableDateTime.ReadOnly = true;
            this.colLogVariableDateTime.Width = 84;
            // 
            // PackageEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 605);
            this.Controls.Add(this.panWorkspace);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.panPackageInfo);
            this.Controls.Add(this.tlsMainMenu);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PackageEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ETL Package Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panWorkspace.ResumeLayout(false);
            this.panDocument.ResumeLayout(false);
            this.tabDocuments.ResumeLayout(false);
            this.tabSteps.ResumeLayout(false);
            this.panSteps.ResumeLayout(false);
            this.panSteps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPackageSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndPackageSteps)).EndInit();
            this.tlsPackageMenu.ResumeLayout(false);
            this.tlsPackageMenu.PerformLayout();
            this.panVariables.ResumeLayout(false);
            this.panVariables.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndVariables)).EndInit();
            this.tlsVariables.ResumeLayout(false);
            this.tlsVariables.PerformLayout();
            this.tabPackageXml.ResumeLayout(false);
            this.tabPackageXml.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.panMessage.ResumeLayout(false);
            this.panMessage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndMessages)).EndInit();
            this.panCounters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCounters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndCounters)).EndInit();
            this.panSessions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSessions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bndSessions)).EndInit();
            this.panBars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bndBindings)).EndInit();
            this.tlsMainMenu.ResumeLayout(false);
            this.tlsMainMenu.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.panPackageInfo.ResumeLayout(false);
            this.panPackageInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panWorkspace;
        private System.Windows.Forms.Panel panDocument;
        private System.Windows.Forms.Panel panBars;
        private System.Windows.Forms.Splitter splDocument;
        private System.Windows.Forms.TabControl tabDocuments;
        private System.Windows.Forms.TabPage tabSteps;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.BindingSource bndPackageSteps;
        private System.Windows.Forms.ToolStrip tlsMainMenu;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSaveAndRun;
        private System.Windows.Forms.BindingSource bndVariables;
        private System.Windows.Forms.BindingSource bndBindings;
        private System.Windows.Forms.Splitter splSteps;
        private System.Windows.Forms.Panel panVariables;
        private System.Windows.Forms.DataGridView grdVariables;
        private System.Windows.Forms.ToolStrip tlsVariables;
        private System.Windows.Forms.ToolStripLabel lblVariables;
        private System.Windows.Forms.ToolStripButton btnAddVariable;
        private System.Windows.Forms.ToolStripButton btnRemoveVariable;
        private System.Windows.Forms.ToolStripSeparator sepRemoveVariable;
        private System.Windows.Forms.SaveFileDialog saveNewFileDialog;
        private System.Windows.Forms.Panel panMessage;
        private System.Windows.Forms.Panel panSessions;
        private System.Windows.Forms.Splitter splSessions;
        private System.Windows.Forms.DataGridView grdMessages;
        private System.Windows.Forms.BindingSource bndMessages;
        private System.Windows.Forms.DataGridView grdSessions;
        private System.Windows.Forms.BindingSource bndSessions;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.ToolStripButton btnMoveDownVariable;
        private System.Windows.Forms.ToolStripButton btnMoveUpVariable;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.TextBox txtLogMessage;
        private System.Windows.Forms.Panel panSteps;
        private System.Windows.Forms.DataGridView grdPackageSteps;
        private System.Windows.Forms.ToolStrip tlsPackageMenu;
        private System.Windows.Forms.ToolStripLabel lblSteps;
        private System.Windows.Forms.ToolStripSplitButton btnAddPackageStep;
        private System.Windows.Forms.ToolStripButton btnMoveUpPackageStep;
        private System.Windows.Forms.ToolStripButton btnMoveDownPackageStep;
        private System.Windows.Forms.ToolStripSeparator sepRemoveStep;
        private System.Windows.Forms.ToolStripButton btnRemovePackageStep;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabPage tabPackageXml;
        private System.Windows.Forms.TextBox txtPackageXml;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkPackageXmlWordWrap;
        private System.Windows.Forms.Panel panPackageInfo;
        private System.Windows.Forms.TextBox txtPackageId;
        private System.Windows.Forms.Label lblPackageId;
        private System.Windows.Forms.TextBox txtPackageName;
        private System.Windows.Forms.Label lblPackageName;
        private System.Windows.Forms.ToolStripStatusLabel lblAgentConnectionString;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblAgentSchema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRowNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageStepType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPackageStepName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequentialId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessageLogDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessageStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessageType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessageText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrace;
        private System.Windows.Forms.Panel panCounters;
        private System.Windows.Forms.DataGridView grdCounters;
        private System.Windows.Forms.BindingSource bndCounters;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVariableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVariableModifier;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVariableValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVariableBinding;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParameterUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessionId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSessionStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCounterEntity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogVariableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogVariableValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogVariableDateTime;
    }
}

