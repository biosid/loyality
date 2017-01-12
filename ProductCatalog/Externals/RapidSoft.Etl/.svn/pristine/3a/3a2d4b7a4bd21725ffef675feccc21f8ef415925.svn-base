using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using RapidSoft.Etl.Runtime.Agents;
using RapidSoft.Etl.Editor.Properties;

namespace RapidSoft.Etl.Editor
{
    public partial class AgentExplorersForm : Form
    {
        #region Constructors

        public AgentExplorersForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Fields

        private bool _rightPaneVisible;

        #endregion

        #region Event handlers

        private void AgentExplorerForm_Load(object sender, EventArgs e)
        {
            _rightPaneVisible = true;
            UpdatePanes();
            agentExplorerLeft.Focus();
        }

        private void btnOnePane_Click(object sender, EventArgs e)
        {
            _rightPaneVisible = false;
            UpdatePanes();
        }

        private void btnTwoPanes_Click(object sender, EventArgs e)
        {
            _rightPaneVisible = true;
            UpdatePanes();
        }

        private void agentExplorerLeft_Connected(object sender, EventArgs e)
        {
            if (_rightPaneVisible)
            {
                agentExplorerRight.EtlAgentToCopy = agentExplorerLeft.EtlAgent;
            }
        }

        private void agentExplorerRight_Connected(object sender, EventArgs e)
        {
            if (_rightPaneVisible)
            {
                agentExplorerLeft.EtlAgentToCopy = agentExplorerRight.EtlAgent;
            }
        }

        private void agentExplorerLeft_PackageCopied(object sender, PackageCopiedEventArgs e)
        {
            agentExplorerRight.UpdatePackages();
            agentExplorerRight.SelectPackage(e.EtlPackageId);
        }

        private void agentExplorerRight_PackageCopied(object sender, PackageCopiedEventArgs e)
        {
            agentExplorerLeft.UpdatePackages();
            agentExplorerLeft.SelectPackage(e.EtlPackageId);
        }

        private void AgentExplorersForm_Resize(object sender, EventArgs e)
        {
            ResizePanes();
        }

        #endregion

        #region Methods

        private void UpdatePanes()
        {
            btnOnePane.Checked = !_rightPaneVisible;
            btnTwoPanes.Checked = _rightPaneVisible;
            agentExplorerRight.Visible = _rightPaneVisible;

            if (_rightPaneVisible)
            {
                agentExplorerLeft.EtlAgentToCopy = agentExplorerRight.EtlAgent;
                agentExplorerRight.EtlAgentToCopy = agentExplorerLeft.EtlAgent;

                agentExplorerLeft.Title = "Left ETL agent";
                agentExplorerRight.Title = "Right ETL agent";

                ResizePanes();
            }
            else
            {
                agentExplorerLeft.EtlAgentToCopy = null;
                agentExplorerRight.EtlAgentToCopy = null;

                agentExplorerLeft.Title = "ETL agent";
            }
        }

        private void ResizePanes()
        {
            if (_rightPaneVisible)
            {
                agentExplorerRight.Width = panMain.Width / 2;
            }
        }

        #endregion
    }
}
