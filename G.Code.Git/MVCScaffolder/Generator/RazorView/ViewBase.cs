using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.MetaData;

namespace Generator.RazorView
{
    public abstract class ViewBase : Base
    {
        protected ViewBase(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className)
        {
            Model.ViewDataType = new ViewDataType
                                     {
                                         FullName = Model.ModelType,
                                         Name = Model.ModelName
                                     };
            Model.ViewDataType.ViewModelProperties = new List<ViewModelProperty>();

            var component = GetComponent();

            var entity =
                component.EntityCollection.SingleOrDefault(
                    c => c.Namespace == Model.ModelTypeNamespace && c.Code == Model.ModelName);
            if (entity == null)
            {
                HasError = true;
                return;
            }
            Model.EntityName = entity.Name;
            Model.IsApprovalOrder = entity.EntityType == EntityType.Order;

            var properties = entity.PropertyCollection;

            foreach (var property in properties)
            {
                ViewModelProperty viewModelProperty = new ViewModelProperty();
                viewModelProperty.DisplayName = property.Name;
                viewModelProperty.IsDateTime = property.Type == MetaDataType.Datetime;
                viewModelProperty.IsDecimal = property.Type == MetaDataType.Decimal;
                viewModelProperty.IsEnum = property.Type == MetaDataType.Enumeration;
                viewModelProperty.IsForeignKey = property.Type == MetaDataType.Entity;
                viewModelProperty.IsGeneric = false;
                viewModelProperty.IsInt = property.Type == MetaDataType.Integer;
                viewModelProperty.IsPrimaryKey = property.Code == "ID";
                viewModelProperty.IsReadOnly = false;
                viewModelProperty.IsRefType = property.Type == MetaDataType.Entity;
                viewModelProperty.Name = property.Code;
                viewModelProperty.Type = property.MetaDataType;

                Model.ViewDataType.ViewModelProperties.Add(viewModelProperty);
            }
        }
    }

    public partial class Index
    {
        public Index(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }

    public partial class Create
    {
        public Create(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }

    public partial class Edit
    {
        public Edit(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }
    public partial class Delete
    {
        public Delete(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }
    public partial class Details
    {
        public Details(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }
    public partial class Search
    {
        public Search(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }
    public partial class _CreateOrEdit
    {
        public _CreateOrEdit(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className)
        {
            if (HasError)
            {
                return;
            }
            var component = GetComponent();

            var entity =
                component.EntityCollection.SingleOrDefault(
                    c => c.Namespace == Model.ModelTypeNamespace && c.Code == Model.ModelName);

            Model.IsApprovalOrder = entity.EntityType == EntityType.Order;
            var relations =
               component.AssociationCollection.Where(a => a.TargetEntityID == entity.ID);

            Model.RelatedEntities = new Dictionary<string, RelatedEntityInfo>();
            var plura = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(
                CultureInfo.GetCultureInfo("en-us"));

            foreach (var relation in relations)
            {
                Model.RelatedEntities.Add(relation.SourceEntity.Code, new RelatedEntityInfo
                {
                    Name = relation.SourceEntity.Code,
                    FullName = relation.SourceEntity.Namespace + "." + relation.SourceEntity.Code,
                    DisplayName = relation.SourceEntity.Name,
                    RelationNamePlural = plura.Pluralize(relation.SourceEntity.Code)
                });
            }

            var enums = entity.PropertyCollection.Where(p => p.Type == Domas.DAP.ADF.MetaData.MetaDataType.Enumeration);
            Model.EnumTypeProperties = new Dictionary<string, EnumTypeInfo>();
            foreach (var property in enums)
            {
                Model.EnumTypeProperties.Add(property.Code, new EnumTypeInfo
                {
                    Name = property.Code,
                    TypeName = property.MetaDataType
                });
            }
        }
    }
}
