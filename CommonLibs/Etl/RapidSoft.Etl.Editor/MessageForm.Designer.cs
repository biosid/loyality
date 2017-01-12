namespace RapidSoft.Etl.Editor
{
	partial class MessageForm
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pBottom = new System.Windows.Forms.Panel();
            this.txtErrorDetails = new System.Windows.Forms.TextBox();
            this.pButtons = new System.Windows.Forms.Panel();
            this.flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.flpLinks = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCopy = new System.Windows.Forms.LinkLabel();
            this.btnDetails = new System.Windows.Forms.LinkLabel();
            this.panIcon = new System.Windows.Forms.Panel();
            this.pMessage = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.pBottom.SuspendLayout();
            this.pButtons.SuspendLayout();
            this.flpLinks.SuspendLayout();
            this.panIcon.SuspendLayout();
            this.pMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(0, 0, 15, 0);
            this.lblMessage.Size = new System.Drawing.Size(402, 104);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Error";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessage.UseMnemonic = false;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox.Image = global::RapidSoft.Etl.Editor.Properties.Resources.resErrorImage;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(113, 106);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // pBottom
            // 
            this.pBottom.Controls.Add(this.txtErrorDetails);
            this.pBottom.Controls.Add(this.pButtons);
            this.pBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pBottom.Location = new System.Drawing.Point(0, 104);
            this.pBottom.Name = "pBottom";
            this.pBottom.Size = new System.Drawing.Size(515, 253);
            this.pBottom.TabIndex = 2;
            // 
            // txtErrorDetails
            // 
            this.txtErrorDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrorDetails.Location = new System.Drawing.Point(0, 34);
            this.txtErrorDetails.Multiline = true;
            this.txtErrorDetails.Name = "txtErrorDetails";
            this.txtErrorDetails.ReadOnly = true;
            this.txtErrorDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrorDetails.Size = new System.Drawing.Size(515, 219);
            this.txtErrorDetails.TabIndex = 1;
            this.txtErrorDetails.Visible = false;
            // 
            // pButtons
            // 
            this.pButtons.Controls.Add(this.flpButtons);
            this.pButtons.Controls.Add(this.flpLinks);
            this.pButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pButtons.Location = new System.Drawing.Point(0, 0);
            this.pButtons.Name = "pButtons";
            this.pButtons.Size = new System.Drawing.Size(515, 34);
            this.pButtons.TabIndex = 2;
            // 
            // flpButtons
            // 
            this.flpButtons.AutoSize = true;
            this.flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpButtons.Location = new System.Drawing.Point(103, 0);
            this.flpButtons.Name = "flpButtons";
            this.flpButtons.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.flpButtons.Size = new System.Drawing.Size(412, 34);
            this.flpButtons.TabIndex = 0;
            this.flpButtons.WrapContents = false;
            // 
            // flpLinks
            // 
            this.flpLinks.AutoSize = true;
            this.flpLinks.Controls.Add(this.btnCopy);
            this.flpLinks.Controls.Add(this.btnDetails);
            this.flpLinks.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpLinks.Location = new System.Drawing.Point(0, 0);
            this.flpLinks.Name = "flpLinks";
            this.flpLinks.Padding = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.flpLinks.Size = new System.Drawing.Size(103, 34);
            this.flpLinks.TabIndex = 4;
            this.flpLinks.WrapContents = false;
            // 
            // btnCopy
            // 
            this.btnCopy.AutoSize = true;
            this.btnCopy.Location = new System.Drawing.Point(11, 8);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(32, 13);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.TabStop = true;
            this.btnCopy.Text = "Copy";
            this.btnCopy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnCopy_LinkClicked);
            // 
            // btnDetails
            // 
            this.btnDetails.AutoSize = true;
            this.btnDetails.Location = new System.Drawing.Point(49, 8);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(51, 13);
            this.btnDetails.TabIndex = 5;
            this.btnDetails.TabStop = true;
            this.btnDetails.Text = "Details...";
            this.btnDetails.Visible = false;
            this.btnDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnErrorDetails_LinkClicked);
            // 
            // panIcon
            // 
            this.panIcon.Controls.Add(this.pictureBox);
            this.panIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.panIcon.Location = new System.Drawing.Point(0, 0);
            this.panIcon.Name = "panIcon";
            this.panIcon.Size = new System.Drawing.Size(113, 104);
            this.panIcon.TabIndex = 3;
            // 
            // pMessage
            // 
            this.pMessage.AutoSize = true;
            this.pMessage.Controls.Add(this.lblMessage);
            this.pMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMessage.Location = new System.Drawing.Point(113, 0);
            this.pMessage.Name = "pMessage";
            this.pMessage.Size = new System.Drawing.Size(402, 104);
            this.pMessage.TabIndex = 4;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 357);
            this.ControlBox = false;
            this.Controls.Add(this.pMessage);
            this.Controls.Add(this.panIcon);
            this.Controls.Add(this.pBottom);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(490, 170);
            this.Name = "MessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MessageForm";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ErrorView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.pBottom.ResumeLayout(false);
            this.pBottom.PerformLayout();
            this.pButtons.ResumeLayout(false);
            this.pButtons.PerformLayout();
            this.flpLinks.ResumeLayout(false);
            this.flpLinks.PerformLayout();
            this.panIcon.ResumeLayout(false);
            this.pMessage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Panel pBottom;
        private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Panel panIcon;
		private System.Windows.Forms.Panel pMessage;
		private System.Windows.Forms.TextBox txtErrorDetails;
        private System.Windows.Forms.Panel pButtons;
		private System.Windows.Forms.FlowLayoutPanel flpButtons;
        private System.Windows.Forms.FlowLayoutPanel flpLinks;
        private System.Windows.Forms.LinkLabel btnCopy;
        private System.Windows.Forms.LinkLabel btnDetails;
	}
}