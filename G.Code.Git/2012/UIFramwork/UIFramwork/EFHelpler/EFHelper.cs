using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Domas.DAP.ADF.MetaData;
using UIFramwork.Util;
using System.Linq.Dynamic;

namespace UIFramwork.EFHelpler
{
    public class EfHelper
    {
        public static dynamic CreateInstance(string typeName)
        {
            var type = MetadataExtensions.GetTypeByTypeFullName(typeName);
            return Activator.CreateInstance(type);
        }
        public static dynamic BindRequestToModel(string modelType, NameValueCollection reqValues)
        {
            var type = MetadataExtensions.GetTypeByTypeFullName(modelType);

            if (type != null)
            {
                dynamic model = Activator.CreateInstance(type);
                //IDictionary<string, object> eo = MetadataExtensions.ToExpando(model);
                var entity = MetadataExtensions.FindEntityByName(modelType);
                model.ID = new Guid(reqValues["ID"]);

                foreach (var property in entity.PropertyCollection)
                {
                    var pro = property.Code;
                    if (property.Type == MetaDataType.Enumeration)
                    {
                        pro += "_EnumValue";
                    }
                    if (reqValues.AllKeys.Any(k => k == pro))
                    {
                        var value = reqValues[pro];
                        var pType = type.GetProperty(pro);

                        pType.SetValue(model, ConvertChangeType(pType.PropertyType, value), null);
                    }
                }
                if (reqValues.AllKeys.Any(k => k == "ParentName"))
                {
                    var parentName = reqValues["ParentName"];
                    var ptype = type.GetProperty(parentName);

                    ptype.SetValue(model, ConvertChangeType(ptype.PropertyType, reqValues[parentName]), null);
                }
                return model;
            }

            return null;
        }
        private static object ConvertChangeType(Type propertyType, string propertyValue)
        {
            object value;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (String.IsNullOrEmpty(propertyValue))
                    value = null;
                else
                    value = Convert.ChangeType(propertyValue, propertyType.GetGenericArguments()[0]);
            }
            else
            {
                if (string.IsNullOrEmpty(propertyValue))
                {
                    if (propertyType.IsValueType)
                    {
                        value = Activator.CreateInstance(propertyType);
                    }
                    else
                    {
                        value = null;
                    }
                }
                else
                {
                    if (propertyType == typeof(Guid))
                    {
                        value = new Guid(propertyValue);
                    }
                    else
                    {
                        value = Convert.ChangeType(propertyValue, propertyType);
                    }
                }
            }
            return value;
        }

        public static void SaveContextChanges(string typeFullName, string id, NameValueCollection paras)
        {
            var model = EfHelper.BindRequestToModel(typeFullName, paras);

            using (dynamic context = MetadataExtensions.GetDbContext(typeFullName))
            {
                if (id == Guid.Empty.ToString())
                {
                    context.Set(model.GetType()).Add(model);
                }
                else
                {
                    context.Entry(model).State = EntityState.Modified;
                }

                context.SaveChanges();
            }
        }
        public static void DeleteEntity(string typeFullName, string[] ids)
        {
            var type = MetadataExtensions.GetTypeByTypeFullName(typeFullName);

            dynamic model = Activator.CreateInstance(type);

            using (DbContext context = MetadataExtensions.GetDbContext(typeFullName))
            {

                var deleteds = context.Set(type).Join("en", ids.Select(id => new Guid(id)).AsQueryable(), "id", "en.ID", "id", "en");
                foreach (var deleted in deleteds)
                {
                    context.Entry(deleted).State = EntityState.Deleted;
                }
                context.SaveChanges();
            }
        }
        public static dynamic GetList(int page, int rows, string sort, string order, string entity, NameValueCollection nvc)
        {
            int total;
            IList<dynamic> selectRows;

            var cols = new string[] { "page", "rows", "sort", "order" };

            var col = nvc.AllKeys.Except(cols).FirstOrDefault();

            var type = MetadataExtensions.GetTypeByTypeFullName(entity);


            using (DbContext context = MetadataExtensions.GetDbContext(entity))
            {
                var poList = context.Set(type).AsQueryable();

                if (!string.IsNullOrEmpty(col))
                {
                    var cType = type.GetProperty(col).PropertyType;
                    if (cType.IsGenericType && cType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                        cType.GetGenericArguments()[0] == typeof(DateTime))
                    {
                        poList = poList.Where(string.Format("{0}=@0", col), DateTime.Parse(nvc[col]));
                    }
                    else if (cType.IsValueType || cType == typeof(string))
                    {
                        poList = poList.Where(string.Format("{0}.Contains(@0)", col), nvc[col]);
                    }
                    else
                    {
                        poList = poList.Where(string.Format("{0}.Code=@0", col), nvc[col]);
                    }
                }
                total = poList.Count();
                selectRows = poList.OrderBy(sort + " " + order).Skip((page - 1) * rows).Take(rows).ToList();
            }
            var returnValue =
                        new
                        {
                            total = total,
                            rows = selectRows
                            .Select(c =>
                            {
                                var expandObj = MetadataExtensions.ToExpando(c);
                                var dictionary = expandObj as IDictionary<string, object>;
                                if (dictionary != null)
                                {
                                    var enums = MetadataExtensions.GetAllEnums(entity, c);
                                    foreach (var @enum in enums)
                                    {
                                        SelectListItem firstOrDefault = null;
                                        var selectListItems = @enum.EnumItem as IEnumerable<SelectListItem>;
                                        if (selectListItems != null)
                                            foreach (SelectListItem @select in selectListItems)
                                            {
                                                if (@select.Selected)
                                                {
                                                    firstOrDefault = @select;
                                                    break;
                                                }
                                            }
                                        dictionary.Add(@enum.Property + "_Name",
                                                       firstOrDefault != null ? firstOrDefault.Text : string.Empty);
                                    }
                                }

                                return (new JavaScriptSerializer()).Deserialize<dynamic>((expandObj as ExpandoObject).Flatten());
                            })
                        };
            return returnValue;
        }

        public static dynamic FindById(string fullEntityName, Guid id)
        {
            var type = MetadataExtensions.GetTypeByTypeFullName(fullEntityName);

            var value = type.GetMethod("FindByID").Invoke(null, new object[] { id });
            return value;
        }
    }
}
