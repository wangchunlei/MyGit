using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Domas.DAP.ADF.MetaData;
using Microsoft.VisualStudio.TextTemplating;

namespace Generator
{
    public abstract class Base : TextTransformation
    {
        protected ModelProperty Model;
        public bool HasError { get; set; }
        protected Base(string projectName, string assemblyName, string classFullName, string className)
        {
            var byteName = assemblyName.Split('.');
            var serviceName = byteName[byteName.Length - 2];
            byteName[byteName.Length - 1] = serviceName + "Context";
            var contextName = String.Join(".", byteName);

            var projPath = Path.GetDirectoryName(projectName);
            var projRootNamespace = Path.GetFileNameWithoutExtension(projectName);

            Model = new ModelProperty
                        {
                            ServerName = serviceName,
                            ModelTypeNamespace = GetNamespaceByClassname(classFullName),
                            DbContextNamespace = GetNamespaceByClassname(contextName),
                            RepositoryNamespace = projRootNamespace + ".Models",
                            Namespace = projRootNamespace,
                            ModelType = classFullName,
                            ContextType = contextName,//
                            ModelName = classFullName.Split('.').Last(),//User
                            PrimaryKey = "ID",
                            PrimaryKeyProperty = "System.Guid",
                            Name = projRootNamespace.Split('.').Last(),//baseportal
                            ModelTypePluralized = className            //Users
                        };
        }
        private string GetNamespaceByClassname(string cName)
        {
            if (String.IsNullOrEmpty(cName))
            {
                return String.Empty;
            }
            var byteName = cName.Split('.');
            var bl = byteName.ToList();
            bl.RemoveAt(byteName.Length - 1);
            byteName = bl.ToArray();
            return String.Join(".", byteName);
        }

        #region ToString Helpers

        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField = CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                                                                                                    typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                                                                     this.formatProviderField })));
                }
            }
        }

        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion

        #region Metadata Helpers
        protected Component GetComponent()
        {
            string metadatapath = @"D:\View\Trunk\Domas.Component\MetaData\" + Model.ServerName + ".metadata";
            var server = Domas.DAP.ADF.MetaData.Service.Load(metadatapath);

            var component = server.ComponentCollection.SingleOrDefault(s => s.Namespace == Model.ModelTypeNamespace);
            return component;
        }
        #endregion

        #region ModelProperty
        public class RepositoryInfo
        {
            public string RepositoryTypeName { get; set; }
            public string VariableName { get; set; }
        }
        public class RelatedEntityInfo
        {
            public string Name { get; set; }
            public string FullName { get; set; }
            public string DisplayName { get; set; }
            public string RelationNamePlural { get; set; }
        }
        public class EnumTypeInfo
        {
            public string Name { get; set; }
            public string TypeName { get; set; }
            public string ValueName { get; set; }
        }

        public class ViewDataType
        {
            public string FullName { get; set; }
            public string Name { get; set; }
            public List<ViewModelProperty> ViewModelProperties { get; set; }
        }

        public class ModelProperty
        {
            public string EntityName { get; set; }
            public string ServerName { get; set; }
            public string ModelTypeNamespace { get; set; }
            public string DbContextNamespace { get; set; }
            public string RepositoryNamespace { get; set; }
            public string ModelType { get; set; }
            public string ContextType { get; set; }
            public string ModelName { get; set; }
            public string PrimaryKeyProperty { get; set; }
            public string PrimaryKey { get; set; }
            public string ModelTypePluralized { get; set; }
            public string Name { get; set; }
            public string Namespace { get; set; }
            public string RoutingName { get; set; }
            public string ControllerName { get; set; }
            public bool IsApprovalOrder { get; set; }
            public Dictionary<string, RepositoryInfo> Repositories { get; set; }
            public Dictionary<string, RelatedEntityInfo> RelatedEntities { get; set; }
            public Dictionary<string, EnumTypeInfo> EnumTypeProperties { get; set; }
            public ViewDataType ViewDataType { get; set; }
        }
        public class ViewModelProperty
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public bool IsDateTime { get; set; }
            public string ValueExpression { get; set; }
            public string Type { get; set; }
            public bool IsPrimaryKey { get; set; }
            public bool IsForeignKey { get; set; }
            public bool IsReadOnly { get; set; }
            public bool IsRefType { get; set; }
            public bool IsGeneric { get; set; }
            public bool IsEnum { get; set; }
            public bool IsInt { get; set; }
            public bool IsDecimal { get; set; }
        }
        #endregion
    }
}
