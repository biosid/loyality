namespace RapidSoft.Etl.Editor
{
    partial class NewPackageWizzardForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPackageId = new System.Windows.Forms.TextBox();
            this.lblPackageId = new System.Windows.Forms.Label();
            this.btnGeneratePackageId = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPackageName = new System.Windows.Forms.TextBox();
            this.lblPackageName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtPackageId
            // 
            this.txtPackageId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackageId.Location = new System.Drawing.Point(65, 12);
            this.txtPackageId.Name = "txtPackageId";
            this.txtPackageId.Size = new System.Drawing.Size(399, 21);
            this.txtPackageId.TabIndex = 7;
            this.txtPackageId.TextChanged += new System.EventHandler(this.txtPackageId_TextChanged);
            // 
            // lblPackageId
            // 
            this.lblPackageId.AutoSize = true;
            this.lblPackageId.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPackageId.Location = new System.Drawing.Point(21, 15);
            this.lblPackageId.Name = "lblPackageId";
            this.lblPackageId.Size = new System.Drawing.Size(21, 13);
            this.lblPackageId.TabIndex = 8;
            this.lblPackageId.Text = "Id:";
            // 
            // btnGeneratePackageId
            // 
            this.btnGeneratePackageId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGeneratePackageId.Location = new System.Drawing.Point(470, 10);
            this.btnGeneratePackageId.Name = "btnGeneratePackageId";
            this.btnGeneratePackageId.Size = new System.Drawing.Size(75, 23);
            this.btnGeneratePackageId.TabIndex = 9;
            this.btnGeneratePackageId.Text = "Generate";
            this.btnGeneratePackageId.UseVisualStyleBackColor = true;
            this.btnGeneratePackageId.Click += new System.EventHandler(this.btnGeneratePackageId_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(389, 78);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(470, 78);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtPackageName
            // 
            this.txtPackageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackageName.Location = new System.Drawing.Point(65, 39);
            this.txtPackageName.Name = "txtPackageName";
            this.txtPackageName.Size = new System.Drawing.Size(480, 21);
            this.txtPackageName.TabIndex = 12;
            this.txtPackageName.TextChanged += new System.EventHandler(this.txtPackageName_TextChanged);
            // 
            // lblPackageName
            // 
            this.lblPackageName.AutoSize = true;
            this.lblPackageName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPackageName.Location = new System.Drawing.Point(20, 41);
            this.lblPackageName.Name = "lblPackageName";
            this.lblPackageName.Size = new System.Drawing.Size(38, 13);
            this.lblPackageName.TabIndex = 13;
            this.lblPackageName.Text = "Name:";
            // 
            // NewPackageWizzardForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(557, 107);
            this.Controls.Add(this.lblPackageName);
            this.Controls.Add(this.txtPackageName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnGeneratePackageId);
            this.Controls.Add(this.txtPackageId);
            this.Controls.Add(this.lblPackageId);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewPackageWizzardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New package";
            this.Shown += new System.EventHandler(this.NewPackageWizzardForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPackageId;
        private System.Windows.Forms.Label lblPackageId;
        private System.Windows.Forms.Button btnGeneratePackageId;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPackageName;
        private System.Windows.Forms.Label lblPackageName;
    }
}