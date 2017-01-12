using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RapidSoft.Etl.Editor
{
    public partial class MessageForm : Form
    {
        #region Constructors

        public MessageForm()
            : this
            (
                DEFAULT_MESSAGE_FORM_TYPE,
                null,
                null,
                null,
                DEFAULT_BUTTONS,
                null,
                new MessageFormSettings()
            )
        {
        }

        public MessageForm(MessageFormType type, string message, string caption, ExceptionInfo exceptionInfo, MessageBoxButtons buttons, string[] attachedFilePathes, MessageFormSettings settingsOverrides)
        {
            var defaultSettings = DefaultMessageFormSettingsProvider.GetDefaultMessageFormSettings();
            _settings = MessageFormSettings.Merge(settingsOverrides, defaultSettings);
            _messageData = CreateMessageFormData(type, message, caption, exceptionInfo, _settings);
            _buttons = buttons;
            _attachedFilePathes = attachedFilePathes;

            InitializeComponent();

            this.Text = _messageData.Title;
            this.SetMessage(_messageData.DisplayText);
            this.SetIcon(type);

            switch (_messageData.MessageType)
            {
                case MessageFormType.Error:
                case MessageFormType.Warning:
                    this.ShowErrorDetails();
                    break;
            }

            this.InitializeButtons(type, buttons);
            this.ResizeView(_messageData.DisplayText);

        }

        #endregion

        #region Constants

        private const int MESSAGE_PADDING = 80;
        private const int DETAILS_HEIGHT = 250;
        private const int MESSAGE_MINIMUM_HEIGHT = 136;

        private static readonly MessageFormType DEFAULT_MESSAGE_FORM_TYPE = MessageFormType.Error;
        private static readonly MessageBoxButtons DEFAULT_BUTTONS = MessageBoxButtons.OK;

        #endregion

        #region Fields

        private readonly MessageFormData _messageData;
        private readonly MessageBoxButtons _buttons;
        private readonly string[] _attachedFilePathes;
        private readonly MessageFormSettings _settings;

        #endregion

        #region Properties

        public MessageFormSettings Settings
        {
            [DebuggerStepThrough]
            get
            {
                return _settings;
            }
        }

        #endregion

        #region Static methods

        public static DialogResult ShowMessage(Exception exception)
        {
            var exceptionInfo = new ExceptionInfo(exception);
            return ShowMessage(MessageFormType.Error, null, null, exceptionInfo, DEFAULT_BUTTONS, null, new MessageFormSettings());
        }

        public static DialogResult ShowMessage(MessageFormType type, string message, string caption)
        {
            return ShowMessage(type, message, caption, null, DEFAULT_BUTTONS, null, new MessageFormSettings());
        }

        public static DialogResult ShowMessage(MessageFormType type, string message, string caption, ExceptionInfo exceptionInfo)
        {
            return ShowMessage(type, message, caption, exceptionInfo, DEFAULT_BUTTONS, null, new MessageFormSettings());
        }

        public static DialogResult ShowMessage(MessageFormType type, string message, string caption, ExceptionInfo exceptionInfo, MessageBoxButtons buttons)
        {
            return ShowMessage(type, message, caption, exceptionInfo, buttons, null, new MessageFormSettings());
        }

        public static DialogResult ShowMessage(MessageFormType type, string message, string caption, ExceptionInfo exceptionInfo, MessageBoxButtons buttons, string[] attachedFilePathes, MessageFormSettings settings)
        {
            using (var view = new MessageForm(type, message, caption, exceptionInfo, buttons, attachedFilePathes, settings))
            {
                TraceMessageData(type, view._messageData);
                var result = view.ShowDialog();
                return result;
            }
        }

        private static MessageFormData CreateMessageFormData(MessageFormType type, string message, string caption, ExceptionInfo exceptionInfo, MessageFormSettings settingsOverrides)
        {
            var messageData = new MessageFormData
            {
                MessageType = type,
                EnvironmentInfo = EnvironmentInfo.GetEnvironmentInfo(),
                ExceptionInfo = exceptionInfo,
            };

            if (!string.IsNullOrEmpty(message))
            {
                messageData.DisplayText = message;
            }
            else
            {
                if (exceptionInfo != null && !string.IsNullOrEmpty(exceptionInfo.Message))
                {
                    messageData.DisplayText = exceptionInfo.Message;
                }
                else
                {
                    messageData.DisplayText = settingsOverrides.DefaultDisplayText;
                }
            }

            if (!string.IsNullOrEmpty(caption))
            {
                messageData.Title = caption;
            }
            else
            {
                messageData.Title = messageData.EnvironmentInfo.ProductName;
            }

            return messageData;
        }

        private static void TraceMessageData(MessageFormType type, MessageFormData messageData)
        {
            if (messageData == null)
            {
                return;
            }

            if (type == MessageFormType.Error)
            {
                var messageDataXml = MessageFormData.SerializeToXml(messageData);
                Trace.TraceError(messageDataXml);
            }
            else if (type == MessageFormType.Warning)
            {
                var messageDataXml = MessageFormData.SerializeToXml(messageData);
                Trace.TraceWarning(messageDataXml);
            }
        }

        #endregion

        #region Private methods

        private void SetMessage(string message)
        {
            lblMessage.Text = message;
        }

        private void SetIcon(MessageFormType type)
        {
            switch (type)
            {
                case MessageFormType.Error:
                    pictureBox.Image = Properties.Resources.resErrorImage;
                    break;
                case MessageFormType.Warning:
                    pictureBox.Image = Properties.Resources.resWarningImage;
                    break;
                case MessageFormType.Information:
                case MessageFormType.Question:
                    pictureBox.Image = Properties.Resources.resInfoImage;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void ShowErrorDetails()
        {
            txtErrorDetails.Text = MessageFormData.SerializeToXml(_messageData);
        }

        private void InitializeButtons(MessageFormType type, MessageBoxButtons buttons)
        {
            this.btnCopy.Visible = !_settings.HideCopyButton;
            this.btnCopy.Text = _settings.CopyButtonText;

            switch (_messageData.MessageType)
            {
                case MessageFormType.Error:
                case MessageFormType.Warning:
                    this.btnDetails.Visible = !_settings.HideDetailsButton;
                    this.btnDetails.Text = _settings.DetailsButtonText;
                    break;
                default:
                    this.btnDetails.Visible = false;
                    break;
            }


            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                    {
                        var btnOK = new Button
                        {
                            Text = _settings.OkButtonText,
                            TabIndex = 1
                        };
                        var btnCancel = new Button
                        {
                            Text = _settings.CancelButtonText,
                            TabIndex = 2
                        };

                        btnOK.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.OK;
                            Close();
                        };
                        btnCancel.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Cancel;
                            Close();
                        };

                        flpButtons.Controls.Add(btnCancel);
                        flpButtons.Controls.Add(btnOK);

                        this.AcceptButton = btnOK;
                        this.CancelButton = btnCancel;
                    }
                    break;

                case MessageBoxButtons.RetryCancel:
                    {
                        var btnRetry = new Button
                        {
                            Text = _settings.RetryButtonText,
                            TabIndex = 1
                        };
                        var btnCancel = new Button
                        {
                            Text = _settings.CancelButtonText,
                            AutoSize = true,
                            TabIndex = 2
                        };

                        btnRetry.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Yes;
                            Close();
                        };
                        btnCancel.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.No;
                            Close();
                        };

                        flpButtons.Controls.Add(btnCancel);
                        flpButtons.Controls.Add(btnRetry);

                        this.AcceptButton = btnRetry;
                        this.CancelButton = btnCancel;
                    }
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    {
                        var btnAbort = new Button
                        {
                            Text = _settings.AbortButtonText,
                            AutoSize = true,
                            TabIndex = 1
                        };
                        var btnRetry = new Button
                        {
                            Text = _settings.RetryButtonText,
                            AutoSize = true,
                            TabIndex = 2
                        };
                        var btnIgnore = new Button
                        {
                            Text = _settings.IgnoreButtonText,
                            AutoSize = true,
                            TabIndex = 3
                        };

                        btnAbort.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Yes;
                            Close();
                        };
                        btnRetry.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.No;
                            Close();
                        };
                        btnIgnore.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Cancel;
                            Close();
                        };

                        flpButtons.Controls.Add(btnIgnore);
                        flpButtons.Controls.Add(btnRetry);
                        flpButtons.Controls.Add(btnAbort);

                        this.AcceptButton = btnAbort;
                        this.CancelButton = btnIgnore;
                    }
                    break;

                case MessageBoxButtons.YesNoCancel:
                    {
                        var btnYes = new Button
                        {
                            Text = _settings.YesButtonText,
                            TabIndex = 1
                        };
                        var btnNo = new Button
                        {
                            Text = _settings.NoButtonText,
                            TabIndex = 2
                        };
                        var btnCancel = new Button
                        {
                            Text = _settings.CancelButtonText,
                            TabIndex = 3
                        };

                        btnYes.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Yes;
                            Close();
                        };
                        btnNo.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.No;
                            Close();
                        };
                        btnCancel.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Cancel;
                            Close();
                        };

                        flpButtons.Controls.Add(btnCancel);
                        flpButtons.Controls.Add(btnNo);
                        flpButtons.Controls.Add(btnYes);

                        this.AcceptButton = btnYes;
                        this.CancelButton = btnCancel;
                    }
                    break;

                case MessageBoxButtons.YesNo:
                    {
                        var btnYes = new Button
                        {
                            Text = _settings.YesButtonText,
                            TabIndex = 1
                        };
                        var btnNo = new Button
                        {
                            Text = _settings.NoButtonText,
                            TabIndex = 2
                        };

                        btnYes.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.Yes;
                            Close();
                        };
                        btnNo.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.No;
                            Close();
                        };

                        flpButtons.Controls.Add(btnNo);
                        flpButtons.Controls.Add(btnYes);

                        this.AcceptButton = btnYes;
                        this.CancelButton = btnNo;
                    }
                    break;

                case MessageBoxButtons.OK:
                default:
                    {
                        var btnOK = new Button
                        {
                            Text = _settings.OkButtonText,
                            TabIndex = 1
                        };

                        btnOK.Click +=
                        (s, e) =>
                        {
                            DialogResult = DialogResult.OK;
                            Close();
                        };

                        flpButtons.Controls.Add(btnOK);

                        this.AcceptButton = btnOK;
                        this.CancelButton = btnOK;
                    }
                    break;
            }

            ((Button)AcceptButton).Select();
        }

        private void ResizeView(string message)
        {
            var sz = new Size(lblMessage.Width, Int32.MaxValue);
            sz = TextRenderer.MeasureText(message, Font, sz, TextFormatFlags.WordBreak);

            var messageHeight = sz.Height + MESSAGE_PADDING;
            if (messageHeight < MESSAGE_MINIMUM_HEIGHT)
            {
                messageHeight = MESSAGE_MINIMUM_HEIGHT;
            }

            ResizeBottomPanel();
            Height = messageHeight + pBottom.Height;

            var desiredWidth = GetDesiredWidth();

            if (Width < desiredWidth)
            {
                Width = desiredWidth;
            }
        }

        private int GetDesiredWidth()
        {
            var w = 0;
            w += flpLinks.Width;

            foreach (Control control in flpButtons.Controls)
            {
                w += control.Width + flpButtons.Padding.Right;
            }

            return w;
        }

        private void ResizeBottomPanel()
        {
            if (txtErrorDetails.Visible)
            {
                pBottom.Height = pButtons.Height + DETAILS_HEIGHT;
            }
            else
            {
                pBottom.Height = pButtons.Height;
            }
        }

        private void CopyToClipboard()
        {
            var text = MessageFormData.SerializeToText(_messageData);
            Clipboard.SetText(text);
        }

        #endregion

        #region Event handlers

        private void btnErrorDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtErrorDetails.Text))
            {
                return;
            }

            txtErrorDetails.Visible = !txtErrorDetails.Visible;
            ResizeBottomPanel();
            ResizeView(lblMessage.Text);
        }

        private void btnCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CopyToClipboard();
        }

        private void ErrorView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && e.Control)
            {
                CopyToClipboard();
            }
        }

        #endregion

        #region Nested classes

        internal static class DefaultMessageFormSettingsProvider
        {
            #region Constants

            private const string DISPLAY_TEXT = "Unknown error";

            private const string COPY_BUTTON_TEXT = "Copy";
            private const string DETAILS_BUTTON_TEXT = "Details...";
            private const string OK_BUTTON_TEXT = "OK";
            private const string CANCEL_BUTTON_TEXT = "Cancel";
            private const string YES_BUTTON_TEXT = "Yes";
            private const string NO_BUTTON_TEXT = "No";
            private const string ABORT_BUTTON_TEXT = "Abort";
            private const string RETRY_BUTTON_TEXT = "Retry";
            private const string IGNORE_BUTTON_TEXT = "Ignore";

            #endregion

            #region Methods

            public static MessageFormSettings GetDefaultMessageFormSettings()
            {
                var settings = new MessageFormSettings
                {
                    DefaultDisplayText = DISPLAY_TEXT,
                    HideCopyButton = false,
                    CopyButtonText = COPY_BUTTON_TEXT,
                    HideDetailsButton = false,
                    DetailsButtonText = DETAILS_BUTTON_TEXT,
                    OkButtonText = OK_BUTTON_TEXT,
                    CancelButtonText = CANCEL_BUTTON_TEXT,
                    YesButtonText = YES_BUTTON_TEXT,
                    NoButtonText = NO_BUTTON_TEXT,
                    AbortButtonText = ABORT_BUTTON_TEXT,
                    RetryButtonText = RETRY_BUTTON_TEXT,
                    IgnoreButtonText = IGNORE_BUTTON_TEXT,
                };

                return settings;
            }

            #endregion
        }

        #endregion
    }
}
