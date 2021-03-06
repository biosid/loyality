﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vtb24.Site.Services.BankConnectorPaymentService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PaymentFormRequest", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    public partial class PaymentFormRequest : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal AmountField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ClientIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int OrderIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReturnUrlFailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ReturnUrlSuccessField;
        
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
        public decimal Amount {
            get {
                return this.AmountField;
            }
            set {
                if ((this.AmountField.Equals(value) != true)) {
                    this.AmountField = value;
                    this.RaisePropertyChanged("Amount");
                }
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
        public int OrderId {
            get {
                return this.OrderIdField;
            }
            set {
                if ((this.OrderIdField.Equals(value) != true)) {
                    this.OrderIdField = value;
                    this.RaisePropertyChanged("OrderId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReturnUrlFail {
            get {
                return this.ReturnUrlFailField;
            }
            set {
                if ((object.ReferenceEquals(this.ReturnUrlFailField, value) != true)) {
                    this.ReturnUrlFailField = value;
                    this.RaisePropertyChanged("ReturnUrlFail");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ReturnUrlSuccess {
            get {
                return this.ReturnUrlSuccessField;
            }
            set {
                if ((object.ReferenceEquals(this.ReturnUrlSuccessField, value) != true)) {
                    this.ReturnUrlSuccessField = value;
                    this.RaisePropertyChanged("ReturnUrlSuccess");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="SimpleBankConnectorResponse", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfboolean))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentInfoXZrH00sf))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf))]
    public partial class SimpleBankConnectorResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int ResultCodeField;
        
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
        public string Error {
            get {
                return this.ErrorField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorField, value) != true)) {
                    this.ErrorField = value;
                    this.RaisePropertyChanged("Error");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int ResultCode {
            get {
                return this.ResultCodeField;
            }
            set {
                if ((this.ResultCodeField.Equals(value) != true)) {
                    this.ResultCodeField = value;
                    this.RaisePropertyChanged("ResultCode");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GenericBankConnectorResponseOfboolean", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    public partial class GenericBankConnectorResponseOfboolean : Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Result {
            get {
                return this.ResultField;
            }
            set {
                if ((this.ResultField.Equals(value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GenericBankConnectorResponseOfPaymentInfoXZrH00sf", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    public partial class GenericBankConnectorResponseOfPaymentInfoXZrH00sf : Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Vtb24.Site.Services.BankConnectorPaymentService.PaymentInfo ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Vtb24.Site.Services.BankConnectorPaymentService.PaymentInfo Result {
            get {
                return this.ResultField;
            }
            set {
                if ((object.ReferenceEquals(this.ResultField, value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    public partial class GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf : Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Vtb24.Site.Services.BankConnectorPaymentService.PaymentFormParameters ResultField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Vtb24.Site.Services.BankConnectorPaymentService.PaymentFormParameters Result {
            get {
                return this.ResultField;
            }
            set {
                if ((object.ReferenceEquals(this.ResultField, value) != true)) {
                    this.ResultField = value;
                    this.RaisePropertyChanged("Result");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PaymentFormParameters", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    public partial class PaymentFormParameters : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MethodField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<string, string> ParametersField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UrlField;
        
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
        public string Method {
            get {
                return this.MethodField;
            }
            set {
                if ((object.ReferenceEquals(this.MethodField, value) != true)) {
                    this.MethodField = value;
                    this.RaisePropertyChanged("Method");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.Dictionary<string, string> Parameters {
            get {
                return this.ParametersField;
            }
            set {
                if ((object.ReferenceEquals(this.ParametersField, value) != true)) {
                    this.ParametersField = value;
                    this.RaisePropertyChanged("Parameters");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url {
            get {
                return this.UrlField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlField, value) != true)) {
                    this.UrlField = value;
                    this.RaisePropertyChanged("Url");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="PaymentInfo", Namespace="http://schemas.datacontract.org/2004/07/RapidSoft.VTB24.BankConnector.API.Entitie" +
        "s")]
    [System.SerializableAttribute()]
    public partial class PaymentInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int OrderIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UnitellerBillNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UnitellerShopIdField;
        
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
        public int OrderId {
            get {
                return this.OrderIdField;
            }
            set {
                if ((this.OrderIdField.Equals(value) != true)) {
                    this.OrderIdField = value;
                    this.RaisePropertyChanged("OrderId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UnitellerBillNumber {
            get {
                return this.UnitellerBillNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.UnitellerBillNumberField, value) != true)) {
                    this.UnitellerBillNumberField = value;
                    this.RaisePropertyChanged("UnitellerBillNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UnitellerShopId {
            get {
                return this.UnitellerShopIdField;
            }
            set {
                if ((object.ReferenceEquals(this.UnitellerShopIdField, value) != true)) {
                    this.UnitellerShopIdField = value;
                    this.RaisePropertyChanged("UnitellerShopId");
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BankConnectorPaymentService.IPaymentService")]
    public interface IPaymentService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/GetPaymentFormParameters", ReplyAction="http://tempuri.org/IPaymentService/GetPaymentFormParametersResponse")]
        Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf GetPaymentFormParameters(Vtb24.Site.Services.BankConnectorPaymentService.PaymentFormRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/GetPaymentFormParameters", ReplyAction="http://tempuri.org/IPaymentService/GetPaymentFormParametersResponse")]
        System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf> GetPaymentFormParametersAsync(Vtb24.Site.Services.BankConnectorPaymentService.PaymentFormRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/IsPaymentAuthorized", ReplyAction="http://tempuri.org/IPaymentService/IsPaymentAuthorizedResponse")]
        Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfboolean IsPaymentAuthorized(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/IsPaymentAuthorized", ReplyAction="http://tempuri.org/IPaymentService/IsPaymentAuthorizedResponse")]
        System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfboolean> IsPaymentAuthorizedAsync(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/ConfirmPayment", ReplyAction="http://tempuri.org/IPaymentService/ConfirmPaymentResponse")]
        Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse ConfirmPayment(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/ConfirmPayment", ReplyAction="http://tempuri.org/IPaymentService/ConfirmPaymentResponse")]
        System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse> ConfirmPaymentAsync(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/CancelPayment", ReplyAction="http://tempuri.org/IPaymentService/CancelPaymentResponse")]
        Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse CancelPayment(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/CancelPayment", ReplyAction="http://tempuri.org/IPaymentService/CancelPaymentResponse")]
        System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse> CancelPaymentAsync(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/GetPaymentByOrderId", ReplyAction="http://tempuri.org/IPaymentService/GetPaymentByOrderIdResponse")]
        Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentInfoXZrH00sf GetPaymentByOrderId(int orderId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPaymentService/GetPaymentByOrderId", ReplyAction="http://tempuri.org/IPaymentService/GetPaymentByOrderIdResponse")]
        System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentInfoXZrH00sf> GetPaymentByOrderIdAsync(int orderId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPaymentServiceChannel : Vtb24.Site.Services.BankConnectorPaymentService.IPaymentService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PaymentServiceClient : System.ServiceModel.ClientBase<Vtb24.Site.Services.BankConnectorPaymentService.IPaymentService>, Vtb24.Site.Services.BankConnectorPaymentService.IPaymentService {
        
        public PaymentServiceClient() {
        }
        
        public PaymentServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PaymentServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PaymentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PaymentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf GetPaymentFormParameters(Vtb24.Site.Services.BankConnectorPaymentService.PaymentFormRequest request) {
            return base.Channel.GetPaymentFormParameters(request);
        }
        
        public System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentFormParametersXZrH00sf> GetPaymentFormParametersAsync(Vtb24.Site.Services.BankConnectorPaymentService.PaymentFormRequest request) {
            return base.Channel.GetPaymentFormParametersAsync(request);
        }
        
        public Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfboolean IsPaymentAuthorized(int orderId) {
            return base.Channel.IsPaymentAuthorized(orderId);
        }
        
        public System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfboolean> IsPaymentAuthorizedAsync(int orderId) {
            return base.Channel.IsPaymentAuthorizedAsync(orderId);
        }
        
        public Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse ConfirmPayment(int orderId) {
            return base.Channel.ConfirmPayment(orderId);
        }
        
        public System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse> ConfirmPaymentAsync(int orderId) {
            return base.Channel.ConfirmPaymentAsync(orderId);
        }
        
        public Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse CancelPayment(int orderId) {
            return base.Channel.CancelPayment(orderId);
        }
        
        public System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.SimpleBankConnectorResponse> CancelPaymentAsync(int orderId) {
            return base.Channel.CancelPaymentAsync(orderId);
        }
        
        public Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentInfoXZrH00sf GetPaymentByOrderId(int orderId) {
            return base.Channel.GetPaymentByOrderId(orderId);
        }
        
        public System.Threading.Tasks.Task<Vtb24.Site.Services.BankConnectorPaymentService.GenericBankConnectorResponseOfPaymentInfoXZrH00sf> GetPaymentByOrderIdAsync(int orderId) {
            return base.Channel.GetPaymentByOrderIdAsync(orderId);
        }
    }
}
