﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CopyFileService.UploadWeb {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UploadWeb.UploadSoap")]
    public interface UploadSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetFileSize", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        long GetFileSize(string sourceFile);
        
        // CODEGEN: Parameter 'fileByte' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UploadFileBybyte", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        CopyFileService.UploadWeb.UploadFileBybyteResponse UploadFileBybyte(CopyFileService.UploadWeb.UploadFileBybyteRequest request);
        
        // CODEGEN: Parameter 'DownFileResult' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DownFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        CopyFileService.UploadWeb.DownFileResponse DownFile(CopyFileService.UploadWeb.DownFileRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/DelelteFileByFileName", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void DelelteFileByFileName(string fileName);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadFileBybyte", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class UploadFileBybyteRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string fileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] fileByte;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public bool isFirst;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public bool isLast;
        
        public UploadFileBybyteRequest() {
        }
        
        public UploadFileBybyteRequest(string fileName, byte[] fileByte, bool isFirst, bool isLast) {
            this.fileName = fileName;
            this.fileByte = fileByte;
            this.isFirst = isFirst;
            this.isLast = isLast;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadFileBybyteResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class UploadFileBybyteResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool UploadFileBybyteResult;
        
        public UploadFileBybyteResponse() {
        }
        
        public UploadFileBybyteResponse(bool UploadFileBybyteResult) {
            this.UploadFileBybyteResult = UploadFileBybyteResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DownFile", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class DownFileRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string fileName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public long startPosition;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public long chunkSize;
        
        public DownFileRequest() {
        }
        
        public DownFileRequest(string fileName, long startPosition, long chunkSize) {
            this.fileName = fileName;
            this.startPosition = startPosition;
            this.chunkSize = chunkSize;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="DownFileResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class DownFileResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] DownFileResult;
        
        public DownFileResponse() {
        }
        
        public DownFileResponse(byte[] DownFileResult) {
            this.DownFileResult = DownFileResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface UploadSoapChannel : CopyFileService.UploadWeb.UploadSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class UploadSoapClient : System.ServiceModel.ClientBase<CopyFileService.UploadWeb.UploadSoap>, CopyFileService.UploadWeb.UploadSoap {
        
        public UploadSoapClient() {
        }
        
        public UploadSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public UploadSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UploadSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public UploadSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public long GetFileSize(string sourceFile) {
            return base.Channel.GetFileSize(sourceFile);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CopyFileService.UploadWeb.UploadFileBybyteResponse CopyFileService.UploadWeb.UploadSoap.UploadFileBybyte(CopyFileService.UploadWeb.UploadFileBybyteRequest request) {
            return base.Channel.UploadFileBybyte(request);
        }
        
        public bool UploadFileBybyte(string fileName, byte[] fileByte, bool isFirst, bool isLast) {
            CopyFileService.UploadWeb.UploadFileBybyteRequest inValue = new CopyFileService.UploadWeb.UploadFileBybyteRequest();
            inValue.fileName = fileName;
            inValue.fileByte = fileByte;
            inValue.isFirst = isFirst;
            inValue.isLast = isLast;
            CopyFileService.UploadWeb.UploadFileBybyteResponse retVal = ((CopyFileService.UploadWeb.UploadSoap)(this)).UploadFileBybyte(inValue);
            return retVal.UploadFileBybyteResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        CopyFileService.UploadWeb.DownFileResponse CopyFileService.UploadWeb.UploadSoap.DownFile(CopyFileService.UploadWeb.DownFileRequest request) {
            return base.Channel.DownFile(request);
        }
        
        public byte[] DownFile(string fileName, long startPosition, long chunkSize) {
            CopyFileService.UploadWeb.DownFileRequest inValue = new CopyFileService.UploadWeb.DownFileRequest();
            inValue.fileName = fileName;
            inValue.startPosition = startPosition;
            inValue.chunkSize = chunkSize;
            CopyFileService.UploadWeb.DownFileResponse retVal = ((CopyFileService.UploadWeb.UploadSoap)(this)).DownFile(inValue);
            return retVal.DownFileResult;
        }
        
        public void DelelteFileByFileName(string fileName) {
            base.Channel.DelelteFileByFileName(fileName);
        }
    }
}