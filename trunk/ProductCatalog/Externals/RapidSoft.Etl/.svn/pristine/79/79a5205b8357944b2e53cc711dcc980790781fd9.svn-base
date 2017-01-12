namespace RapidSoft.Etl.Editor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Steps;

    public partial class PackageEditorForm : Form
    {
        #region Constructors

        public PackageEditorForm()
        {
            InitializeComponent();

            EtlModelCollectionEditor.AnyPropertyValueChanged += new PropertyValueChangedEventHandler(InnerPropertyEditor_PropertyValueChanged);

            _formTitle = this.Text;
        }

        #endregion

        #region Constants

        private const string READY_STATUS = "Ready";
        private const string RUNNING_STATUS = "Running...";
        private const string SAVED_STATUS = "File {0} successfully saved";

        #endregion

        #region Fields

        private readonly string _formTitle;

        private EtlAgentInfo _agentInfo;
        private ILocalEtlAgent _agent;
        private string _packageId;
        private EtlPackage _currentPackage;

        #endregion

        #region Event handlers

        private void frmMain_Load(object sender, EventArgs e)
        {
            ResizeControlsByDefault();
            LoadDefaultData();
            lblStatus.Text = READY_STATUS;

            UpdateButtons();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !AboutToCloseCurrentPackage();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var exc = TryDeployCurrentPackage();
            if (exc != null)
            {
                MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot save package", new ExceptionInfo(exc), MessageBoxButtons.OK);
            }
            else
            {
                UpdateButtons();
            }
        }

        private void btnSaveAndRun_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            RunCurrentPackage();
        }

        private void chkPackageXmlWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            txtPackageXml.WordWrap = chkPackageXmlWordWrap.Checked;
        }

        private void grdParameters_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var currentParameter = GetParameterFromRowIndex(e.RowIndex);
            if (currentParameter == null)
            {
                return;
            }

            if (e.ColumnIndex == colParameterUse.Index)
            {
                //todo: remove hardcode
                e.Value = string.Concat("$(", currentParameter.Name, ")");
            }
        }

        private EtlVariableInfo GetParameterFromRowIndex(int rowIndex)
        {
            if (rowIndex < bndVariables.Count)
            {
                return (EtlVariableInfo)grdVariables.Rows[rowIndex].DataBoundItem;
            }
            else
            {
                return null;
            }
        }

        private void grdPackageSteps_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var currentStep = GetPackageFromRowIndex(e.RowIndex);
            if (currentStep == null)
            {
                return;
            }

            //if (currentStep.Disabled)
            //{
            //    e.CellStyle.ForeColor = SystemColors.InactiveCaption;
            //}
            //else
            //{
            //    e.CellStyle.ForeColor = grdPackageSteps.ForeColor;
            //}

            if (e.ColumnIndex == colRowNumber.Index)
            {
                var stepNumber = e.RowIndex + 1;
                e.Value = string.Format("{0:D3}", stepNumber);
            }
            else if (e.ColumnIndex == colPackageStepType.Index)
            {
                e.Value = GetStepDisplayName(currentStep.GetType());
            }
        }

        private void grdMessages_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var currentMessage = GetMessageRowIndex(e.RowIndex);
            if (currentMessage == null)
            {
                return;
            }

            if (e.ColumnIndex == colMessageType.Index)
            {
                e.Value = EtlMessageTypes.ToString(currentMessage.MessageType);
            }
        }

        private EtlMessage GetMessageRowIndex(int rowIndex)
        {
            if (rowIndex < bndMessages.Count)
            {
                return (EtlMessage)grdMessages.Rows[rowIndex].DataBoundItem;
            }
            else
            {
                return null;
            }
        }

        private EtlStep GetPackageFromRowIndex(int rowIndex)
        {
            if (rowIndex < bndPackageSteps.Count)
            {
                return (EtlStep)grdPackageSteps.Rows[rowIndex].DataBoundItem;
            }
            else
            {
                return null;
            }
        }

        private void btnMoveDownPackageStep_Click(object sender, EventArgs e)
        {
            if (grdPackageSteps.CurrentRow == null)
            {
                return;
            }

            MovePackageStep(grdPackageSteps.CurrentRow.Index, 1);
        }

        private void btnMoveUpPackageStep_Click(object sender, EventArgs e)
        {
            if (grdPackageSteps.CurrentRow == null)
            {
                return;
            }

            MovePackageStep(grdPackageSteps.CurrentRow.Index, -1);
        }

        private void btnRemovePackageStep_Click(object sender, EventArgs e)
        {
            if (grdPackageSteps.CurrentRow == null)
            {
                return;
            }

            if (MessageForm.ShowMessage(MessageFormType.Question, "Delete selected step?", this.Text, null, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RemovePackageStep(grdPackageSteps.CurrentRow.Index);
            }
        }

        private void btnAddParameter_Click(object sender, EventArgs e)
        {
            AddParameter();
        }

        private void btnMoveDownParameter_Click(object sender, EventArgs e)
        {
            if (grdVariables.CurrentRow == null)
            {
                return;
            }

            MoveParameter(grdVariables.CurrentRow.Index, 1);
        }

        private void btnMoveUpParameter_Click(object sender, EventArgs e)
        {
            if (grdVariables.CurrentRow == null)
            {
                return;
            }

            MoveParameter(grdVariables.CurrentRow.Index, -1);
        }

        private void btnRemoveParameter_Click(object sender, EventArgs e)
        {
            if (grdVariables.CurrentRow == null)
            {
                return;
            }

            if (MessageForm.ShowMessage(MessageFormType.Question, "Delete selected parameter?", this.Text, null, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                RemoveParameter(grdVariables.CurrentRow.Index);
            }
        }

        private void tmrStatus_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = READY_STATUS;
            tmrStatus.Enabled = false;
        }

        private void grdSessions_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            propertyGrid.SelectedObject = grdSessions.Rows[e.RowIndex].DataBoundItem;
        }

        private void grdPackageSteps_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            propertyGrid.SelectedObject = grdPackageSteps.Rows[e.RowIndex].DataBoundItem;
        }

        private void grdVariables_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            propertyGrid.SelectedObject = grdVariables.Rows[e.RowIndex].DataBoundItem;
        }

        private void grdMessages_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            propertyGrid.SelectedObject = grdMessages.Rows[e.RowIndex].DataBoundItem;
            txtLogMessage.Text = (string)grdMessages.Rows[e.RowIndex].Cells[colMessageText.Index].Value +
                Environment.NewLine +
                "-------------------------" + 
                Environment.NewLine +
                (string)grdMessages.Rows[e.RowIndex].Cells[colTrace.Index].Value;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            InnerPropertyEditor_PropertyValueChanged(s, e);
        }

        private void InnerPropertyEditor_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            var typeCode = Convert.GetTypeCode(e.ChangedItem.Value);
            if (typeCode == TypeCode.String && (string)(e.ChangedItem.Value) == "")
            {
                GridItem parent = e.ChangedItem.Parent;
                object obj = null;
                while (parent != null)
                {
                    if (parent.Value != null)
                    {
                        obj = parent.Value;
                        break;
                    }

                    parent = parent.Parent;
                }

                e.ChangedItem.PropertyDescriptor.SetValue(obj, null);
            }
        }

        private void tabDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {
            var objectSelected = false;

            if (tabDocuments.SelectedTab == tabSteps)
            {
                if (grdPackageSteps.SelectedRows.Count > 0)
                {
                    propertyGrid.SelectedObject = grdPackageSteps.Rows[grdPackageSteps.SelectedRows[0].Index].DataBoundItem;
                    objectSelected = true;
                }
            }
            else if (tabDocuments.SelectedTab == tabPackageXml)
            {
                UpdateCurrentPackageXml();
            }
            else if (tabDocuments.SelectedTab == tabLog)
            {
                if (grdMessages.SelectedRows.Count > 0)
                {
                    propertyGrid.SelectedObject = grdMessages.Rows[grdMessages.SelectedRows[0].Index].DataBoundItem;
                    objectSelected = true;
                }
            }

            if (!objectSelected)
            {
                propertyGrid.SelectedObject = null;
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var package = _currentPackage;

            var backgroundLogger = new BackgroundEtlLogger(backgroundWorker, package.Steps.Count);
            var memoryLogger = new MemoryEtlLogger();

            _agent.AttachLogger(backgroundLogger);
            _agent.AttachLogger(memoryLogger);

            _agent.InvokeEtlPackage(_currentPackage.Id, null, null);
            e.Result = memoryLogger;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var message = (EtlMessage)e.UserState;
            if (message != null)
            {
                bndMessages.Add(message);
            }

            progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var memoryLogger = (MemoryEtlLogger)e.Result;

            bndSessions.DataSource = memoryLogger.EtlSessions;
            bndMessages.DataSource = memoryLogger.EtlMessages;
            bndCounters.DataSource = memoryLogger.EtlCounters;

            lblStatus.Text = READY_STATUS;
            progressBar.Visible = false;
        }

        #endregion

        #region Methods

        public void Show(ILocalEtlAgent agent, EtlAgentInfo agentInfo, string packageId)
        {
            if (agent == null)
            {
                throw new ArgumentNullException("agent");
            }

            if (agentInfo == null)
            {
                throw new ArgumentNullException("agentInfo");
            }

            if (packageId == null)
            {
                throw new ArgumentNullException("packageId");
            }

            _agent = agent;
            _agentInfo = agentInfo;
            _packageId = packageId;

            EtlPackage package;
            var exc = TryGetPackage(_packageId, out package);
            if (exc != null)
            {
                MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot open package", new ExceptionInfo(exc), MessageBoxButtons.OK);
            }
            else
            {
                _currentPackage = package;
                UpdateButtons();
                ResetLogView();
                ShowCurrentPackage();

                this.Show();
            }
        }

        private void ResizeControlsByDefault()
        {
            panBars.Width = this.Width / 3;
            panVariables.Height = tabSteps.Height / 3;

            colRowNumber.Width = 35;
            colVariableName.Width = 70;
            colSessionStatus.Width = 100;
            colMessageType.Width = 180;
        }

        private void LoadDefaultData()
        {
            LoadParameterBindings();
            LoadPackageStepTypes();
        }

        private void LoadParameterBindings()
        {
            var bindingList = new BindingList<EtlVariableBinding>();
            foreach (EtlVariableBinding item in Enum.GetValues(typeof(EtlVariableBinding)))
            {
                bindingList.Add(item);
            }
            bndBindings.DataSource = bindingList;
        }

        private void LoadPackageStepTypes()
        {
            //todo: avoid this hack
            var asm = typeof(EtlDelayStep).Assembly;
            var types = new List<Type>();

            foreach (var type in asm.GetTypes())
            {
                if (typeof(EtlStep).IsAssignableFrom(type) && !type.IsAbstract && !type.IsGenericTypeDefinition)
                {
                    types.Add(type);
                }
            }

            Comparison<Type> compare = delegate(Type left, Type right)
            {
                return string.CompareOrdinal(left.Name, right.Name);
            };
            types.Sort(compare);

            foreach (var type in types)
            {
                var closureType = type;

                EventHandler onClick = delegate(object sender, EventArgs e)
                {
                    AddPackageStep(closureType.AssemblyQualifiedName);
                };

                btnAddPackageStep.DropDownItems.Add(GetStepDisplayName(type), null, onClick);                
            }
        }

        private bool AboutToCloseCurrentPackage()
        {
            if (_currentPackage == null)
            {
                return true;
            }

            var answer = MessageForm.ShowMessage(MessageFormType.Question, "Save package?", this.Text, null, MessageBoxButtons.YesNoCancel);

            if (answer == DialogResult.Cancel)
            {
                return false;
            }
            else if (answer == DialogResult.Yes)
            {
                var exc = TryDeployCurrentPackage();
                if (exc != null)
                {
                    MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot save package", new ExceptionInfo(exc), MessageBoxButtons.OK);
                    return false;
                }
                else
                {
                    return true;
                }                
            }
            else
            {
                return true;
            }
        }

        private void UpdateButtons()
        {
            btnSave.Enabled =  (_agent != null) && (_currentPackage != null);
            btnSaveAndRun.Enabled =  (_agent != null) && (_currentPackage != null);

            if (_agentInfo != null)
            {
                lblAgentConnectionString.Text = _agentInfo.ConnectionString;
                lblAgentSchema.Text = _agentInfo.SchemaName;
            }
            else
            {
                lblAgentConnectionString.Text = "No ETL agent";
                lblAgentSchema.Text = "";
            }

            if (_currentPackage != null)
            {
                txtPackageId.Text = _currentPackage.Id;
                txtPackageName.Text = _currentPackage.Name;
            }
            else
            {
                txtPackageId.Text = "";
                txtPackageName.Text = "";
            }

            chkPackageXmlWordWrap.Checked = txtPackageXml.WordWrap;
        }

        private Exception TryGetPackage(string packageId, out EtlPackage package)
        {
            try
            {
                package = _agent.GetEtlPackage(packageId);
                return null;
            }
            catch (Exception exc)
            {
                package = null;
                return exc;
            }
        }

        private Exception TryDeployCurrentPackage()
        {
            try
            {
                _currentPackage.Id = txtPackageId.Text.Trim();
                _currentPackage.Name = txtPackageName.Text.Trim();

                _agent.DeployEtlPackage(_currentPackage, new EtlPackageDeploymentOptions
                {
                    Overwrite = true,
                });
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        private void ShowCurrentPackage()
        {
            bndVariables.Position = 0;
            bndPackageSteps.Position = 0;

            if (_currentPackage != null)
            {
                txtPackageId.Text = _currentPackage.Id;
                txtPackageName.Text = _currentPackage.Name;

                bndVariables.DataSource = _currentPackage.Variables;
                bndPackageSteps.DataSource = _currentPackage.Steps;
            }
            else
            {
                txtPackageId.Text = "";
                txtPackageName.Text = "";

                bndVariables.DataSource = null;
                bndPackageSteps.DataSource = null;

                propertyGrid.SelectedObject = null;
            }
        }

        private void UpdateCurrentPackageXml()
        {
            if (_currentPackage != null)
            {
                var xml = EtlPackageXmlSerializer.Serialize(_currentPackage);
                txtPackageXml.Text = xml;
            }
            else
            {
                txtPackageXml.Text = "";
            }
        }

        private void RunCurrentPackage()
        {
            tabDocuments.SelectedTab = tabLog;

            ResetLogView();

            lblStatus.Text = RUNNING_STATUS;
            progressBar.Visible = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void ResetLogView()
        {
            bndSessions.DataSource = null;
            bndCounters.DataSource = null;
            bndMessages.DataSource = new List<EtlMessage>();
        }

        private static string GetStepDisplayName(Type stepType)
        {
            var name = stepType.Name;

            if (name.StartsWith("Etl"))
            {
                name = stepType.Name.Remove(0, 3);

                if (name.EndsWith("Step"))
                {
                    name = name.Remove(name.Length - 4, 4);
                }
            }

            return name;
        }

        private void AddPackageStep(string typeFullName)
        {
            var stepType = Type.GetType(typeFullName);
            var newStep = (EtlStep)Activator.CreateInstance(stepType);
            newStep.Name = GetStepDisplayName(stepType);

            bndPackageSteps.Add(newStep);
            grdPackageSteps.Rows[grdPackageSteps.Rows.Count - 1].Cells[0].Selected = true;
        }

        private bool MovePackageStep(int index, int offset)
        {
            if (!(index >= 0 && index < bndPackageSteps.Count))
            {
                return false;
            }

            if (!(index + offset >= 0 && index + offset < bndPackageSteps.Count))
            {
                return false;
            }

            var temp = bndPackageSteps[index + offset];
            bndPackageSteps[index + offset] = bndPackageSteps[index];
            bndPackageSteps[index] = temp;

            grdPackageSteps.Rows[index].Selected = false;
            grdPackageSteps.Rows[index + offset].Selected = true;
            grdPackageSteps.CurrentCell = grdPackageSteps.Rows[index + offset].Cells[0];

            return true;
        }

        private void RemovePackageStep(int index)
        {
            bndPackageSteps.RemoveAt(index);
        }

        private void AddParameter()
        {
            var newVariable = new EtlVariableInfo();
            bndVariables.Add(newVariable);

            grdVariables.Rows[grdVariables.Rows.Count - 1].Cells[0].Selected = true;
        }

        private void RemoveParameter(int index)
        {
            bndVariables.RemoveAt(index);
        }

        private bool MoveParameter(int index, int offset)
        {
            if (!(index >= 0 && index < bndVariables.Count))
            {
                return false;
            }

            if (!(index + offset >= 0 && index + offset < bndVariables.Count))
            {
                return false;
            }

            var temp = bndVariables[index + offset];
            bndVariables[index + offset] = bndVariables[index];
            bndVariables[index] = temp;

            grdVariables.CurrentCell = grdVariables.Rows[index + offset].Cells[0];

            return true;
        }

        #endregion    
    }
}
