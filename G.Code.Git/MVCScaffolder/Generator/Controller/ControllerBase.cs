using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.DAP.ADF.MetaData;
using Microsoft.VisualStudio.TextTemplating;

namespace Generator.Controller
{
    public abstract class ControllerBase : Base
    {
        protected ControllerBase(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className)
        {
            Model.RoutingName = Model.ModelName;
            Model.ControllerName = Model.RoutingName + "Controller";

            var component = GetComponent();

            var entity =
                component.EntityCollection.SingleOrDefault(
                    c => c.Namespace == Model.ModelTypeNamespace && c.Code == Model.ModelName);
            if (entity == null)
            {
                HasError = true;
                return;
            }

            Model.IsApprovalOrder = entity.EntityType==EntityType.Order;

            var relations =
                component.AssociationCollection.Where(a => a.TargetEntityID == entity.ID);

            Model.Repositories = new Dictionary<string, RepositoryInfo>();
            Model.RelatedEntities = new Dictionary<string, RelatedEntityInfo>();

            Model.Repositories.Add(entity.Code, new RepositoryInfo
                                                   {
                                                       RepositoryTypeName = entity.Code + "Repository",
                                                       VariableName = entity.Code.ToLower() + "Repository"
                                                   });

            foreach (var relation in relations)
            {
                Model.Repositories.Add(relation.SourceEntity.Code, new RepositoryInfo
                                                                      {
                                                                          RepositoryTypeName = relation.SourceEntity.Code + "Repository",
                                                                          VariableName = relation.SourceEntity.Code.ToLower() + "Repository"
                                                                      });
                Model.RelatedEntities.Add(relation.SourceEntity.Code, new RelatedEntityInfo
                                                                         {
                                                                             Name = relation.SourceEntity.Code
                                                                         });
            }

            var enums = entity.PropertyCollection.Where(p => p.Type == Domas.DAP.ADF.MetaData.MetaDataType.Enumeration);
            Model.EnumTypeProperties = new Dictionary<string, EnumTypeInfo>();
            foreach (var property in enums)
            {
                Model.EnumTypeProperties.Add(property.Code, new EnumTypeInfo
                                                               {
                                                                   Name = property.Code,
                                                                   TypeName = property.MetaDataType,
                                                                   ValueName = property.Code + "_EnumValue"
                                                               });
            }

        }
    }

    public partial class ControllerWithRepository
    {
        public ControllerWithRepository(string projectName, string assemblyName, string classFullName, string className)
            : base(projectName, assemblyName, classFullName, className) { }
    }
}
