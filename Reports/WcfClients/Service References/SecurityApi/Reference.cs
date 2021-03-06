﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rapidsoft.VTB24.Reports.WcfClients.SecurityApi {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreateUserOptions", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Inputs")]
    [System.SerializableAttribute()]
    public partial class CreateUserOptions : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.RegistrationType RegistrationTypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientId {
            get {
                return this.ClientIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientIdField, value) != true)) {
                    this.ClientIdField = value;
                    this.RaisePropertyChanged("ClientId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhoneNumber {
            get {
                return this.PhoneNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneNumberField, value) != true)) {
                    this.PhoneNumberField = value;
                    this.RaisePropertyChanged("PhoneNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.RegistrationType RegistrationType {
            get {
                return this.RegistrationTypeField;
            }
            set {
                if ((this.RegistrationTypeField.Equals(value) != true)) {
                    this.RegistrationTypeField = value;
                    this.RaisePropertyChanged("RegistrationType");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RegistrationType", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels")]
    public enum RegistrationType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SiteRegistration = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        BankRegistration = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="DenyRegistrationRequestOptions", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Inputs")]
    [System.SerializableAttribute()]
    public partial class DenyRegistrationRequestOptions : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RegistrationRequestBankStatusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhoneNumber {
            get {
                return this.PhoneNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneNumberField, value) != true)) {
                    this.PhoneNumberField = value;
                    this.RaisePropertyChanged("PhoneNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RegistrationRequestBankStatus {
            get {
                return this.RegistrationRequestBankStatusField;
            }
            set {
                if ((this.RegistrationRequestBankStatusField.Equals(value) != true)) {
                    this.RegistrationRequestBankStatusField = value;
                    this.RaisePropertyChanged("RegistrationRequestBankStatus");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CreateUserAndPasswordOptions", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.Security.SecurityService.Model" +
        "s.Inputs")]
    [System.SerializableAttribute()]
    public partial class CreateUserAndPasswordOptions : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ForcePasswordChangeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneNumberField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientId {
            get {
                return this.ClientIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientIdField, value) != true)) {
                    this.ClientIdField = value;
                    this.RaisePropertyChanged("ClientId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool ForcePasswordChange {
            get {
                return this.ForcePasswordChangeField;
            }
            set {
                if ((this.ForcePasswordChangeField.Equals(value) != true)) {
                    this.ForcePasswordChangeField = value;
                    this.RaisePropertyChanged("ForcePasswordChange");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhoneNumber {
            get {
                return this.PhoneNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneNumberField, value) != true)) {
                    this.PhoneNumberField = value;
                    this.RaisePropertyChanged("PhoneNumber");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="User", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.Security.SecurityService.Model" +
        "s")]
    [System.SerializableAttribute()]
    public partial class User : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsDisabledField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsPasswordSetField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PhoneNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime RegistrationDateField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ClientId {
            get {
                return this.ClientIdField;
            }
            set {
                if ((object.ReferenceEquals(this.ClientIdField, value) != true)) {
                    this.ClientIdField = value;
                    this.RaisePropertyChanged("ClientId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsDisabled {
            get {
                return this.IsDisabledField;
            }
            set {
                if ((this.IsDisabledField.Equals(value) != true)) {
                    this.IsDisabledField = value;
                    this.RaisePropertyChanged("IsDisabled");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsPasswordSet {
            get {
                return this.IsPasswordSetField;
            }
            set {
                if ((this.IsPasswordSetField.Equals(value) != true)) {
                    this.IsPasswordSetField = value;
                    this.RaisePropertyChanged("IsPasswordSet");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PhoneNumber {
            get {
                return this.PhoneNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.PhoneNumberField, value) != true)) {
                    this.PhoneNumberField = value;
                    this.RaisePropertyChanged("PhoneNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime RegistrationDate {
            get {
                return this.RegistrationDateField;
            }
            set {
                if ((this.RegistrationDateField.Equals(value) != true)) {
                    this.RegistrationDateField = value;
                    this.RaisePropertyChanged("RegistrationDate");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChangePhoneNumberOptions", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Inputs")]
    [System.SerializableAttribute()]
    public partial class ChangePhoneNumberOptions : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChangedByField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LoginField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NewPhoneNumberField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ChangedBy {
            get {
                return this.ChangedByField;
            }
            set {
                if ((object.ReferenceEquals(this.ChangedByField, value) != true)) {
                    this.ChangedByField = value;
                    this.RaisePropertyChanged("ChangedBy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Login {
            get {
                return this.LoginField;
            }
            set {
                if ((object.ReferenceEquals(this.LoginField, value) != true)) {
                    this.LoginField = value;
                    this.RaisePropertyChanged("Login");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NewPhoneNumber {
            get {
                return this.NewPhoneNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.NewPhoneNumberField, value) != true)) {
                    this.NewPhoneNumberField = value;
                    this.RaisePropertyChanged("NewPhoneNumber");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChangeUserPhoneNumberResult", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Outputs")]
    [System.SerializableAttribute()]
    public partial class ChangeUserPhoneNumberResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ChangeUserPhoneNumberStatus StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ChangeUserPhoneNumberStatus Status {
            get {
                return this.StatusField;
            }
            set {
                if ((this.StatusField.Equals(value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ChangeUserPhoneNumberStatus", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Outputs")]
    public enum ChangeUserPhoneNumberStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Changed = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserNotFound = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PhoneNumberIsUsedByAnotherUser = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FailedToSendNotification = 3,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResetUserPasswordOptions", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Inputs")]
    [System.SerializableAttribute()]
    public partial class ResetUserPasswordOptions : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LoginField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Login {
            get {
                return this.LoginField;
            }
            set {
                if ((object.ReferenceEquals(this.LoginField, value) != true)) {
                    this.LoginField = value;
                    this.RaisePropertyChanged("Login");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResetUserPasswordResult", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Outputs")]
    [System.SerializableAttribute()]
    public partial class ResetUserPasswordResult : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ResetUserPasswordStatus StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ResetUserPasswordStatus Status {
            get {
                return this.StatusField;
            }
            set {
                if ((this.StatusField.Equals(value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResetUserPasswordStatus", Namespace="http://schemas.datacontract.org/2004/07/Vtb24.Site.SecurityWebServices.Security.M" +
        "odels.Outputs")]
    public enum ResetUserPasswordStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Changed = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        UserNotFound = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FailedToSendNotification = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SecurityApi.ISecurityWebApi")]
    public interface ISecurityWebApi {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/CreateUser", ReplyAction="http://tempuri.org/ISecurityWebApi/CreateUserResponse")]
        void CreateUser(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.CreateUserOptions options);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/DenyRegistrationRequest", ReplyAction="http://tempuri.org/ISecurityWebApi/DenyRegistrationRequestResponse")]
        void DenyRegistrationRequest(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.DenyRegistrationRequestOptions options);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/CreateUserAndPassword", ReplyAction="http://tempuri.org/ISecurityWebApi/CreateUserAndPasswordResponse")]
        void CreateUserAndPassword(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.CreateUserAndPasswordOptions options);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/DeleteUser", ReplyAction="http://tempuri.org/ISecurityWebApi/DeleteUserResponse")]
        void DeleteUser(string login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/DisableUser", ReplyAction="http://tempuri.org/ISecurityWebApi/DisableUserResponse")]
        void DisableUser(string login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/EnableUser", ReplyAction="http://tempuri.org/ISecurityWebApi/EnableUserResponse")]
        void EnableUser(string login);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/BatchResolveUsersByClientId", ReplyAction="http://tempuri.org/ISecurityWebApi/BatchResolveUsersByClientIdResponse")]
        System.Collections.Generic.Dictionary<string, Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.User> BatchResolveUsersByClientId(string[] clientIds);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/BatchResolveUsersByPhone", ReplyAction="http://tempuri.org/ISecurityWebApi/BatchResolveUsersByPhoneResponse")]
        System.Collections.Generic.Dictionary<string, Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.User> BatchResolveUsersByPhone(string[] clientPhones);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/ChangeUserPhoneNumber", ReplyAction="http://tempuri.org/ISecurityWebApi/ChangeUserPhoneNumberResponse")]
        Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ChangeUserPhoneNumberResult ChangeUserPhoneNumber(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ChangePhoneNumberOptions options);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/ResetUserPassword", ReplyAction="http://tempuri.org/ISecurityWebApi/ResetUserPasswordResponse")]
        Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ResetUserPasswordResult ResetUserPassword(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ResetUserPasswordOptions options);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISecurityWebApi/Echo", ReplyAction="http://tempuri.org/ISecurityWebApi/EchoResponse")]
        string Echo(string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISecurityWebApiChannel : Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ISecurityWebApi, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SecurityWebApiClient : System.ServiceModel.ClientBase<Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ISecurityWebApi>, Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ISecurityWebApi {
        
        public SecurityWebApiClient() {
        }
        
        public SecurityWebApiClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SecurityWebApiClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SecurityWebApiClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SecurityWebApiClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CreateUser(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.CreateUserOptions options) {
            base.Channel.CreateUser(options);
        }
        
        public void DenyRegistrationRequest(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.DenyRegistrationRequestOptions options) {
            base.Channel.DenyRegistrationRequest(options);
        }
        
        public void CreateUserAndPassword(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.CreateUserAndPasswordOptions options) {
            base.Channel.CreateUserAndPassword(options);
        }
        
        public void DeleteUser(string login) {
            base.Channel.DeleteUser(login);
        }
        
        public void DisableUser(string login) {
            base.Channel.DisableUser(login);
        }
        
        public void EnableUser(string login) {
            base.Channel.EnableUser(login);
        }
        
        public System.Collections.Generic.Dictionary<string, Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.User> BatchResolveUsersByClientId(string[] clientIds) {
            return base.Channel.BatchResolveUsersByClientId(clientIds);
        }
        
        public System.Collections.Generic.Dictionary<string, Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.User> BatchResolveUsersByPhone(string[] clientPhones) {
            return base.Channel.BatchResolveUsersByPhone(clientPhones);
        }
        
        public Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ChangeUserPhoneNumberResult ChangeUserPhoneNumber(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ChangePhoneNumberOptions options) {
            return base.Channel.ChangeUserPhoneNumber(options);
        }
        
        public Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ResetUserPasswordResult ResetUserPassword(Rapidsoft.VTB24.Reports.WcfClients.SecurityApi.ResetUserPasswordOptions options) {
            return base.Channel.ResetUserPassword(options);
        }
        
        public string Echo(string message) {
            return base.Channel.Echo(message);
        }
    }
}
