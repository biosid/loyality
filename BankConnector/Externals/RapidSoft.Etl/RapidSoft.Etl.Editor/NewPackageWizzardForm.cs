using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RapidSoft.Etl.Editor
{
    public partial class NewPackageWizzardForm : Form
    {
        #region Constructors

        public NewPackageWizzardForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public string NewPackageId
        {
            get
            {
                return txtPackageId.Text.Trim();
            }
            set
            {
                txtPackageId.Text = value != null ? value.Trim() : "";
            }
        }

        public string NewPackageName
        {
            get
            {
                return txtPackageName.Text.Trim();
            }
            set
            {
                txtPackageName.Text = value != null ? value.Trim() : "";
            }
        }

        #endregion

        #region Event handlers

        private void NewPackageWizzardForm_Shown(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void btnGeneratePackageId_Click(object sender, EventArgs e)
        {
            txtPackageId.Text = Guid.NewGuid().ToString();
        }

        private void txtPackageId_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void txtPackageName_TextChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        #endregion

        #region Methods

        private void UpdateButtons()
        {
            btnOK.Enabled = txtPackageId.Text.Trim().Length > 0 && txtPackageName.Text.Trim().Length > 0;
        }

        #endregion
    }
}
