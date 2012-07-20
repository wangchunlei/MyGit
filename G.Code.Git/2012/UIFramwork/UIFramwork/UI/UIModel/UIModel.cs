using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Domas.DAP.ADF.Context;
using Domas.DAP.ADF.MetaData;
using UIFramwork.UI.UIFrom;
using UIFramwork.Util;

namespace UIFramwork.UI.UIModel
{
    public class UIModel<T> where T : class
    {
        private string _typeFullName;
        private FormTypeEnum _formType;
        public string ModelName
        {
            get { return _typeFullName; }
        }
        public IFormLayout FormLayout { get; set; }
        public bool HasError = false;
        public IList<UIGroup<T>> UIGroups { get; set; }
        public UIViewDataCollection UIViewDataCollection { get; set; }
        public int Pop { get; set; }
        public UIModel(string _typeFullName, FormTypeEnum _formType)
        {
            this._typeFullName = _typeFullName;
            this._formType = _formType;

            FormLayout = new FormLayout
            {
                ColumnCount = 3,
                LabelColumnWidth = 115,
                GapColumnWidth = 10,
                ControlColumnWidth = 200,
                SpaceColumnWidth = 50,
                Direction = LayoutDirection.Horizontal,
                RowCount = 100,
                RowHeight = 20,
                SpaceRow = 5
            };

            var entity = MetadataExtensions.FindEntityByName(_typeFullName);
            if (entity != null)
            {
                var visibility = VisibilityType.Query;
                var readOnly = EditableType.AnyTime;

                switch (_formType)
                {
                    case FormTypeEnum.Create:
                        visibility = VisibilityType.Create;
                        readOnly = EditableType.BeforeSave;
                        break;
                    case FormTypeEnum.Update:
                        visibility = VisibilityType.Edit;
                        readOnly = EditableType.BeforeComplete;
                        break;
                    default:
                        visibility = VisibilityType.Query;
                        readOnly = EditableType.AnyTime;
                        break;
                }
                InitGroups(entity, visibility, readOnly);
            }
            else
            {
                HasError = true;
            }
        }

        private void InitGroups(Entity entity, VisibilityType visibility, EditableType readOnly)
        {
            UIGroups = entity.PropertyCollection.GroupBy(p => p.Group)
                .GroupJoin(entity.PropertyCollection
                , o => o.Key
                , inner => inner.Group
                           , (left, right) => new UIGroup<T>
                               {
                                   Title = left.Key,
                                   Position = left.Max(l => l.GroupSeq),
                                   GroupName = "Group" + left.Max(l => l.GroupSeq),
                                   Properties = right.Where(p => p.IsSystem == false)
                                                  .Select(p => new UIProperty<T>
                                                      {
                                                          Code = p.Code,
                                                          Name = p.Name,
                                                          DisplayName = p.Name,
                                                          Position = p.Sequence,
                                                          PropertyType = p.Type,
                                                          FullTypeName = p.TypeFullName,
                                                          Readonly = p.EditableType != readOnly
                                                      }).ToList()
                               }).OrderBy(g => g.Position).ToList();
        }
    }
}
