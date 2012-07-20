using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domas.DAP.ADF.MetaData;
using Newtonsoft.Json;

namespace UIFramwork.Util
{
    public static class MetadataExtensions
    {
        public static Entity FindEntityByName(string fullName)
        {
            var pm = Domas.DAP.ADF.Context.ContextFactory.GetServiceMetadata();
            var sm = pm.GetMetadataByEntityFullName(fullName);
            var en = sm.EntityCollection.FirstOrDefault(e => (e.Namespace + "." + e.Code) == fullName);
            return en;
        }
        public static Enumeration FindEnumByName(string fullName)
        {
            var pm = Domas.DAP.ADF.Context.ContextFactory.GetServiceMetadata();
            var sm = pm.GetMetadataByEntityFullName(fullName);
            var en = sm.EnumCollection.FirstOrDefault(e => (e.Namespace + "." + e.Code) == fullName);
            return en;
        }
        public static IEnumerable<string> FindSubEntitiesByOwner(string fullName)
        {
            var pm = Domas.DAP.ADF.Context.ContextFactory.GetServiceMetadata();
            var sm = pm.GetMetadataByEntityFullName(fullName);
            var en = sm.EntityCollection.FirstOrDefault(e => (e.Namespace + "." + e.Code) == fullName);

            var subs = en.Owner.AssociationCollection.Where(a => a.SourceEntity == en).Select(a => a.TargetEntity.ToString());

            return subs;
        }
        public static string FindParent(string fullName)
        {
            var pm = Domas.DAP.ADF.Context.ContextFactory.GetServiceMetadata();
            var sm = pm.GetMetadataByEntityFullName(fullName);
            var en = sm.EntityCollection.FirstOrDefault(e => (e.Namespace + "." + e.Code) == fullName);

            var subs = en.Owner.AssociationCollection.Where(a => a.TargetEntity == en).Select(a => a.SourceEntity.ToString()).FirstOrDefault();

            return subs;
        }
        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> anonymousDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(anonymousObject);
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var item in anonymousDictionary)
                expando.Add(item);
            return (ExpandoObject)expando;
        }
        public static ExpandoObject Expando(this IDictionary<string, object> dictionary)
        {
            ExpandoObject expandoObject = new ExpandoObject();
            IDictionary<string, object> objects = expandoObject;

            foreach (var item in dictionary)
            {
                bool processed = false;

                if (item.Value is IDictionary<string, object>)
                {
                    objects.Add(item.Key, Expando((IDictionary<string, object>)item.Value));
                    processed = true;
                }
                else if (item.Value is ICollection)
                {
                    List<object> itemList = new List<object>();

                    foreach (var item2 in (ICollection)item.Value)

                        if (item2 is IDictionary<string, object>)
                            itemList.Add(Expando((IDictionary<string, object>)item2));
                        else
                            itemList.Add(Expando(new Dictionary<string, object> { { "Unknown", item2 } }));

                    if (itemList.Count > 0)
                    {
                        objects.Add(item.Key, itemList);
                        processed = true;
                    }
                }

                if (!processed)
                    objects.Add(item);
            }

            return expandoObject;
        }
        //public static string Flatten(this ExpandoObject expando)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<string> contents = new List<string>();
        //    var d = expando as IDictionary<string, object>;
        //    sb.Append("{");

        //    foreach (KeyValuePair<string, object> kvp in d)
        //    {
        //        contents.Add(String.Format("{0}: {1}", kvp.Key,
        //           JsonConvert.SerializeObject(kvp.Value)));
        //    }
        //    sb.Append(String.Join(",", contents.ToArray()));

        //    sb.Append("}");

        //    return sb.ToString();
        //}
        public static string Flatten(this ExpandoObject expando)
        {
            StringBuilder sb = new StringBuilder();
            List<string> contents = new List<string>();
            var d = expando as IDictionary<string, object>;
            sb.Append("{ ");

            foreach (KeyValuePair<string, object> kvp in d)
            {
                if (kvp.Value is ExpandoObject)
                {
                    ExpandoObject expandoValue = (ExpandoObject)kvp.Value;
                    StringBuilder expandoBuilder = new StringBuilder();
                    expandoBuilder.Append(String.Format("\"{0}\":[", kvp.Key));

                    String flat = Flatten(expandoValue);
                    expandoBuilder.Append(flat);

                    string expandoResult = expandoBuilder.ToString();
                    // expandoResult = expandoResult.Remove(expandoResult.Length - 1);
                    expandoResult += "]";
                    contents.Add(expandoResult);
                }
                else if (kvp.Value is List<Object>)
                {
                    List<Object> valueList = (List<Object>)kvp.Value;

                    StringBuilder listBuilder = new StringBuilder();
                    listBuilder.Append(String.Format("\"{0}\":[", kvp.Key));
                    foreach (Object item in valueList)
                    {
                        if (item is ExpandoObject)
                        {
                            String flat = Flatten(item as ExpandoObject);
                            listBuilder.Append(flat + ",");
                        }
                    }

                    string listResult = listBuilder.ToString();
                    listResult = listResult.Remove(listResult.Length - 1);
                    listResult += "]";
                    contents.Add(listResult);

                }
                else
                {
                    contents.Add(String.Format("\"{0}\": {1}", kvp.Key,
                       JsonConvert.SerializeObject(kvp.Value)));
                }
                //contents.Add("type: " + valueType);
            }
            sb.Append(String.Join(",", contents.ToArray()));

            sb.Append("}");

            return sb.ToString();
        }

        public static Type GetTypeByTypeFullName(string fullname)
        {
            var beName = fullname.Split('.').Take(3);
            var beDllName = string.Join(".", beName) + ".BE";
            var ty = Type.GetType(fullname + "," + beDllName);
            return ty;
        }

        public static dynamic GetDbContext(string fullname)
        {
            //Domas.Service.Print.PrintContext
            var beName = fullname.Split('.').Take(3);
            var shortName = beName.Last();
            var contextName = string.Join(".", beName) + "." + shortName + "Context";
            var beDllName = string.Join(".", beName) + ".BE";
            var contextType = Type.GetType(contextName + "," + beDllName);

            return contextType.GetConstructor(new Type[0]).Invoke(null);
        }
        public static IEnumerable<dynamic> GetAllEnums(string typeName, dynamic be = null)
        {
            var entity = FindEntityByName(typeName);
            var enumTypes = entity.PropertyCollection
                .Where(c => c.Type == MetaDataType.Enumeration)
                .Select(c =>
                {
                    var enumration = FindEnumByName(c.MetaDataType);

                    var enumItem = enumration.LiteralCollection
                        .Select(option =>
                        {
                            var beExpando = (ToExpando(be) as IDictionary<string, object>);
                            var enumValue = string.Empty;
                            var key = c.Code + "-EnumValue";
                            if (beExpando != null && beExpando.ContainsKey(key) && beExpando[key] != null)
                            {
                                enumValue = beExpando[key].ToString();
                            }
                            return new SelectListItem
                            {
                                Text = option.Name,
                                Value = option.Value.ToString(),
                                Selected = (be != null) && (option.Value.ToString() == enumValue)
                            };
                        });
                    dynamic val = new ExpandoObject();
                    val.Property = c.Code;
                    val.EnumItem = enumItem;
                    return val;
                });

            return enumTypes;
        }
    }
}
