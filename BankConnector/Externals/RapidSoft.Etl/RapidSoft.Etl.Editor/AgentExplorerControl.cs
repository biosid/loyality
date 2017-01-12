using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using RapidSoft.Etl.Runtime;
using RapidSoft.Etl.Runtime.Agents;
using RapidSoft.Etl.Editor.Properties;

namespace RapidSoft.Etl.Editor
{
    public partial class AgentExplorerControl : UserControl
    {
        #region Constructors

        public AgentExplorerControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Fields

        private EtlAgentInfo _agentInfo;
        private ILocalEtlAgent _agent;

        private PackageEditorForm _packageEditorForm;
        private NewPackageWizzardForm _newPackageWizzardForm;

        private IEtlAgent _agentToCopy;

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return lblEtlAgent.Text;
            }
            set
            {
                lblEtlAgent.Text = value;
            }
        }

        public IEtlAgent EtlAgent
        {
            get
            {
                return _agent;
            }
        }

        public IEtlAgent EtlAgentToCopy
        {
            get
            {
                return _agentToCopy;
            }
            set
            {
                _agentToCopy = value;
                UpdateButtons();
            }
        }

        public string CopyButtonText
        {
            get
            {
                return btnCopyPackageTo.Text;
            }
            set
            {
                btnCopyPackageTo.Text = value;
            }
        }

        public string SettingsSuffix
        {
            get;
            set;
        }

        #endregion

        #region Events

        public event EventHandler Connected;
        public event EventHandler<PackageCopiedEventArgs> PackageCopied;

        #endregion

        #region Event handlers

        private void AgentExplorer_Load(object sender, EventArgs e)
        {
            LoadDefaultData();

            var agentInfo = ReadEtlAgentSettings();
            cmbEtlAgentType.Text = agentInfo.EtlAgentType;
            txtConnectionString.Text = agentInfo.ConnectionString;
            txtSchemaName.Text = agentInfo.SchemaName;

            UpdateButtons();
            btnConnectAgent.Focus();
        }

        private void btnConnectAgent_Click(object sender, EventArgs e)
        {
            var agentInfo = new EtlAgentInfo
            {
                EtlAgentType = cmbEtlAgentType.Text.Trim(),
                ConnectionString = txtConnectionString.Text.Trim(),
                SchemaName = txtSchemaName.Text.Trim(),
            };

            IEtlAgent agent;

            var exc = TryCreateAgent(agentInfo, out agent);
            if (exc != null)
            {
                MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot connect to ETL agent", new ExceptionInfo(exc), MessageBoxButtons.OK);
            }
            else
            {
                if (agent is ILocalEtlAgent)
                {
                    _agentInfo = agentInfo;
                    _agent = (ILocalEtlAgent)agent;

                    SaveEtlAgentSettings(_agentInfo);
                    LoadPackages();
                    UpdateButtons();

                    if (this.Connected != null)
                    {
                        this.Connected(this, EventArgs.Empty);
                    }
                }
                else
                {
                    MessageForm.ShowMessage(MessageFormType.Information, "This ETL agent does not supported", "Cannot connect to ETL agent", null, MessageBoxButtons.OK);
                }
            }
        }

        private void btnNewPackage_Click(object sender, EventArgs e)
        {
            if (_newPackageWizzardForm == null)
            {
                _newPackageWizzardForm = new NewPackageWizzardForm();
            }

            _newPackageWizzardForm.NewPackageId = "";
            _newPackageWizzardForm.NewPackageName = "";
            var result = _newPackageWizzardForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                var exc = TryCreatePackage(_newPackageWizzardForm.NewPackageId, _newPackageWizzardForm.NewPackageName);
                if (exc != null)
                {
                    MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot create package", new ExceptionInfo(exc), MessageBoxButtons.OK);
                }
                else
                {
                    LoadPackages();
                    UpdateButtons();
                }
            }
        }

        private void btnOpenPackage_Click(object sender, EventArgs e)
        {
            var selectedPackage = GetSelectedPackage();
            if (selectedPackage != null)
            {
                _packageEditorForm = new PackageEditorForm();
                _packageEditorForm.Show(_agent, _agentInfo, selectedPackage.Id);
            }
        }

        private void btnRefreshPackages_Click(object sender, EventArgs e)
        {
            LoadPackages();
        }

        //todo: refactor btnCopyPackageTo_Click method
        private void btnCopyPackageTo_Click(object sender, EventArgs e)
        {
            if (this.EtlAgentToCopy == null)
            {
                return;
            }

            var packageToCopy = GetSelectedPackage();
            if (packageToCopy == null)
            {
                return;
            }

            var exc = TryCopyPackage(packageToCopy.Id, false);
            if (exc != null)
            {
                if (exc is EtlPackageAlreadyExistsException)
                {
                    if (MessageForm.ShowMessage(MessageFormType.Question, string.Format("Package with identifier \"{0}\" and name \"{1}\" already exists. Overwrite?", packageToCopy.Id, packageToCopy.Name), this.Title, null, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        exc = TryCopyPackage(packageToCopy.Id, true);
                        if (exc != null)
                        {
                            MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot copy package", new ExceptionInfo(exc), MessageBoxButtons.OK);
                        }
                        else
                        {
                            LoadPackages();
                            UpdateButtons();

                            if (this.PackageCopied != null)
                            {
                                this.PackageCopied(this, new PackageCopiedEventArgs(packageToCopy.Id));
                            }
                        }
                    }
                }
                else
                {
                    MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot copy package", new ExceptionInfo(exc), MessageBoxButtons.OK);
                }
            }
            else
            {
                LoadPackages();
                UpdateButtons();

                if (this.PackageCopied != null)
                {
                    this.PackageCopied(this, new PackageCopiedEventArgs(packageToCopy.Id));
                }
            }
        }

        private void grdPackages_DoubleClick(object sender, EventArgs e)
        {
            btnOpenPackage_Click(sender, e);
        }

        private void grdPackages_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOpenPackage_Click(sender, e);
            }
        }

        private void grdPackages_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void cmbEtlAgentType_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void txtConnectionString_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        #endregion

        #region Methods

        public void UpdatePackages()
        {
            LoadPackages();
            UpdateButtons();
        }

        public bool SelectPackage(string packageId)
        {
            for (var i = 0; i < grdPackages.Rows.Count; i++)
            {
                var package = (EtlPackage)grdPackages.Rows[i].DataBoundItem;
                if (package != null && string.Equals(package.Id, packageId, StringComparison.InvariantCultureIgnoreCase))
                {
                    grdPackages.Rows[i].Selected = true;
                    return true;
                }
            }

            return false;
        }

        private void LoadDefaultData()
        {
            bndEtlAgentTypes.DataSource = GetEtlAgentTypeNames();
        }

        private static string[] GetEtlAgentTypeNames()
        {
            var asm = typeof(IEtlAgent).Assembly;
            var typeNames = new List<string>();

            foreach (var type in asm.GetTypes())
            {
                if (typeof(IEtlAgent).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    typeNames.Add(type.AssemblyQualifiedName);
                }
            }

            return typeNames.ToArray();
        }

        private Exception TryCreateAgent(EtlAgentInfo agentInfo, out IEtlAgent agent)
        {
            try
            {
                agent = EtlAgents.CreateAgent(agentInfo);
                return null;
            }
            catch (Exception exc)
            {
                agent = null;
                return exc;
            }
        }

        private void SaveEtlAgentSettings(EtlAgentInfo agentInfo)
        {
            var settingsSuffix = this.SettingsSuffix ?? "";

            if (agentInfo != null)
            {
                Settings.Default["EtlAgentType" + settingsSuffix] = agentInfo.EtlAgentType;
                Settings.Default["ConnectionString" + settingsSuffix] = agentInfo.ConnectionString;
                Settings.Default["SchemaName" + settingsSuffix] = agentInfo.SchemaName;
            }
            else
            {
                Settings.Default["EtlAgentType" + settingsSuffix] = null;
                Settings.Default["ConnectionString" + settingsSuffix] = null;
                Settings.Default["SchemaName" + settingsSuffix] = null;
            }

            Settings.Default.Save();
        }

        private EtlAgentInfo ReadEtlAgentSettings()
        {
            var settingsSuffix = this.SettingsSuffix ?? "";

            return new EtlAgentInfo
            {
                EtlAgentType = (string)Settings.Default["EtlAgentType" + settingsSuffix],
                ConnectionString = (string)Settings.Default["ConnectionString" + settingsSuffix],
                SchemaName = (string)Settings.Default["SchemaName" + settingsSuffix],
            };
        }

        private void UpdateButtons()
        {
            btnConnectAgent.Enabled = !(string.IsNullOrEmpty(cmbEtlAgentType.Text) || string.IsNullOrEmpty(txtConnectionString.Text));

            btnNewPackage.Enabled = (_agent != null);
            btnOpenPackage.Enabled = (_agent != null) && HasSelectedPackage();
            btnRefreshPackages.Enabled = (_agent != null);
            btnCopyPackageTo.Visible = (_agent != null) && (_agentToCopy != null);
            btnCopyPackageTo.Enabled = HasSelectedPackage();

            if (_agentInfo != null)
            {
                lblStatus.Text = "Connected";
            }
            else
            {
                lblStatus.Text = "Not connected";
            }
        }

        private void LoadPackages()
        {
            if (_agent != null)
            {
                EtlPackage[] packages;
                var exc = TryGetPackages(out packages);
                if (exc != null)
                {
                    MessageForm.ShowMessage(MessageFormType.Error, exc.Message, "Cannot load packages", new ExceptionInfo(exc), MessageBoxButtons.OK);
                }
                else
                {
                    bndPackages.DataSource = packages;
                }
            }
            else
            {
                bndPackages.DataSource = null;
            }
        }

        private Exception TryGetPackages(out EtlPackage[] packages)
        {
            try
            {
                packages = _agent.GetEtlPackages();
                return null;
            }
            catch (Exception exc)
            {
                packages = null;
                return exc;
            }
        }

        private Exception TryCreatePackage(string packageId, string packageName)
        {
            try
            {
                var package = new EtlPackage
                {
                    Id = packageId,
                    Name = packageName,
                    Enabled = true,
                };

                _agent.DeployEtlPackage(package, null);
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        private Exception TryCopyPackage(string packageId, bool overwrite)
        {
            try
            {
                var package = _agent.GetEtlPackage(packageId);
                if (package == null)
                {
                    return new InvalidOperationException(string.Format("Package \"{0}\" does not exist", packageId));
                }

                _agentToCopy.DeployEtlPackage(package, new EtlPackageDeploymentOptions
                {
                    Overwrite = overwrite
                });
                return null;
            }
            catch (Exception exc)
            {
                return exc;
            }
        }

        private bool HasSelectedPackage()
        {
            return GetSelectedPackage() != null;
        }

        private EtlPackage GetSelectedPackage()
        {
            if (grdPackages.SelectedRows.Count == 0)
            {
                return null;
            }

            var selectedPackage = (EtlPackage)grdPackages.SelectedRows[0].DataBoundItem;
            if (selectedPackage == null)
            {
                return null;
            }

            return selectedPackage;
        }

        #endregion
    }
}
