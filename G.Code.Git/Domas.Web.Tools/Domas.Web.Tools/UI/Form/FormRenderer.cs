using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Dynamic;
using Domas.DAP.ADF.Context;
using Domas.DAP.ADF.MetaData;
using Domas.Web.Tools.Authorize.Models;

namespace Domas.Web.Tools.UI.Form
{
    public class FormRenderer<T> : IFromRenderer<T> where T : class
    {
        public void Render(IFormModel<T> model, T data, System.IO.TextWriter output,
            System.Web.Mvc.HtmlHelper<T> helper, List<CustomerUIProperty> displayProperties)
        {
            var serverCollection = Context.GetMetadata<T>();

            var tType = typeof(T);
            var TName = tType.Name;
            var TNamespace = tType.Namespace;
            var entity = serverCollection.EntityCollection.SingleOrDefault(e => e.Code == TName && e.Namespace == TNamespace);

            StringBuilder sb = new StringBuilder();

            var entityProperities = entity.PropertyCollection.Where(p => p.Code.EndsWith("_EnumValue") == false);
            var disGroup = displayProperties.GroupBy(c => c.GroupCode);
            foreach (var dis in disGroup)
            {
                sb.Append("<fieldset>");
                sb.Append("<legend>" + dis.First().GroupName + "</legend>");
                sb.Append("<div id='" + dis.Key + "'>");
                AppendBody(model, helper, dis.ToList(), sb, entityProperities, dis.Key);
                sb.Append("</div>");
                sb.Append("</fieldset>");
            }

            //AppendBody(model, helper, displayProperties, sb, entityProperities);


            output.Write(sb.ToString());
        }

        private static void AppendBody(IFormModel<T> model, HtmlHelper<T> helper, List<CustomerUIProperty> displayProperties, StringBuilder sb,
                                       IEnumerable<Property> entityProperities, string divID)
        {
            sb.Append(model.FormLayout.TableMarkupStart);
            //blank row
            //sb.Append(model.FormLayout.TrMarkupStart(20));
            //sb.Append(model.FormLayout.TrMarkupEnd);
            //List<string> inner = new List<string>() { "CreatedOn" };

            var res = from entityProperity in entityProperities
                      join inn in displayProperties on entityProperity.Code equals inn.Code
                      where inn.IsEnable
                      select new { entityProperity, inn.IsReadOnly };

            if (!res.Any())
            {
                res = from entityProperity in entityProperities
                      select new { entityProperity, IsReadOnly = false };
            }

            bool trStart = false;
            for (int i = 0; i < res.Count(); i++)
            {
                trStart = false;
                var annoy = res.ElementAt(i);
                var property = annoy.entityProperity;

                if (i % (model.FormLayout.ColumnCount) == 0)
                {
                    sb.Append(model.FormLayout.TrMarkupStart(model.FormLayout.RowHeight));
                    //blank column
                    sb.Append(model.FormLayout.TdMarkupStart(10));
                    sb.Append(model.FormLayout.TdMarkupEnd);
                    trStart = true;
                }

                //label column
                sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.LabelColumnWidth));
                sb.Append(helper.Label(property.Name));
                sb.Append(model.FormLayout.TdMarkupEnd);

                //gap column
                sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.GapColumnWidth));
                sb.Append(model.FormLayout.TdMarkupEnd);

                //control column
                sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.ControlColumnWidth));

                var typeName = property.TypeFullName;
                var isGenericType = typeName.EndsWith("?");

                switch (property.Type)
                {
                    case MetaDataType.Datetime:
                        if (annoy.IsReadOnly)
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.DisplayFor(DynamicExpression.ParseLambda<T, DateTime?>(property.Code))
                                    : helper.DisplayFor(DynamicExpression.ParseLambda<T, DateTime>(property.Code)));
                        }
                        else
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.TextBoxFor(DynamicExpression.ParseLambda<T, DateTime?>(property.Code), new { style = "width:135px;" })
                                    : helper.TextBoxFor(DynamicExpression.ParseLambda<T, DateTime>(property.Code), new { style = "width:135px;" }));
                            var isPopClient = helper.ViewContext.ViewData["IsPopFromClient"];
                            if (isPopClient != null && !(bool)isPopClient)
                            {
                                sb.Append("<script language=\"javascript\">$('#" + divID + "').find('#" + property.Code + "').datebox();</script>");
                            }
                        }

                        break;
                    case MetaDataType.String:
                        if (annoy.IsReadOnly)
                        {
                            sb.Append(helper.DisplayFor(DynamicExpression.ParseLambda<T, string>(property.Code)));
                        }
                        else
                        {
                            sb.Append(helper.TextBoxFor(DynamicExpression.ParseLambda<T, string>(property.Code)));
                        }

                        break;
                    case MetaDataType.Boolean:
                        if (annoy.IsReadOnly)
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.DisplayFor(DynamicExpression.ParseLambda<T, bool?>(property.Code))
                                    : helper.DisplayFor(DynamicExpression.ParseLambda<T, bool>(property.Code)));
                        }
                        else
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.EditorFor(DynamicExpression.ParseLambda<T, bool?>(property.Code))
                                    : helper.EditorFor(DynamicExpression.ParseLambda<T, bool>(property.Code)));
                        }
                        break;
                    case MetaDataType.Decimal:
                        if (annoy.IsReadOnly)
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.DisplayFor(DynamicExpression.ParseLambda<T, decimal?>(property.Code))
                                    : helper.DisplayFor(DynamicExpression.ParseLambda<T, decimal>(property.Code)));
                        }
                        else
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.EditorFor(DynamicExpression.ParseLambda<T, decimal?>(property.Code))
                                    : helper.EditorFor(DynamicExpression.ParseLambda<T, decimal>(property.Code)));
                        }
                        break;
                    case MetaDataType.Integer:
                        if (annoy.IsReadOnly)
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.DisplayFor(DynamicExpression.ParseLambda<T, int?>(property.Code))
                                    : helper.DisplayFor(DynamicExpression.ParseLambda<T, int>(property.Code)));
                        }
                        else
                        {
                            sb.Append(
                                isGenericType
                                    ? helper.EditorFor(DynamicExpression.ParseLambda<T, int?>(property.Code))
                                    : helper.EditorFor(DynamicExpression.ParseLambda<T, int>(property.Code)));
                        }
                        break;
                    case MetaDataType.Entity:
                        if (isGenericType)
                        {
                            var enumExpress = DynamicExpression.ParseLambda<T, Guid?>(property.Code);
                            if (annoy.IsReadOnly)
                            {
                                sb.Append(helper.Label(helper.ViewContext.ViewData["Possible" + property.Code + "Name"].ToString()));
                            }
                            else
                            {
                                if (helper.ViewContext.ViewData["Possible" + property.Code] != null)
                                {
                                    var forStr = helper.DropDownListFor(enumExpress,
                                                 (IEnumerable<SelectListItem>)
                                                 helper.ViewContext.ViewData[
                                                     "Possible" + property.Code]);
                                    sb.Append(forStr);
                                }

                            }
                        }
                        else
                        {
                            var enumExpress = DynamicExpression.ParseLambda<T, Guid>(property.Code);

                            var forStr = helper.DropDownListFor(enumExpress,
                                                                (IEnumerable<SelectListItem>)
                                                                helper.ViewContext.ViewData[
                                                                    "Possible" + property.Code]);
                            sb.Append(forStr);

                        }

                        break;
                    case MetaDataType.Enumeration:
                        if (isGenericType)
                        {
                            var enumExpress = DynamicExpression.ParseLambda<T, int?>(property.Code + "_EnumValue");
                            if (annoy.IsReadOnly)
                            {
                                //sb.Append(helper.DisplayFor(enumExpress));
                                sb.Append(helper.Label(helper.ViewContext.ViewData["Possible" + property.Code + "Name"].ToString()));
                            }
                            else
                            {
                                var forStr = helper.DropDownListFor(enumExpress,
                                                                    (IEnumerable<SelectListItem>)
                                                                    helper.ViewContext.ViewData[
                                                                        "Possible" + property.Code]);
                                sb.Append(forStr);
                            }
                        }
                        else
                        {
                            var enumExpress = DynamicExpression.ParseLambda<T, int>(property.Code + "_EnumValue");
                            if (annoy.IsReadOnly)
                            {
                                sb.Append(helper.DisplayFor(enumExpress));
                            }
                            else
                            {
                                var forStr = helper.DropDownListFor(enumExpress,
                                                                    (IEnumerable<SelectListItem>)
                                                                    helper.ViewContext.ViewData[
                                                                        "Possible" + property.Code]);
                                sb.Append(forStr);
                            }
                        }

                        break;
                    default:
                        AppendRowEnd(model, entityProperities, i, sb, trStart);
                        continue;
                }
                AppendRowEnd(model, entityProperities, i, sb, trStart);
            }


            //var edit = System.Web.Mvc.Html.EditorExtensions.EditorFor(helper, expr);

            sb.Append(model.FormLayout.TableMarkupEnd);
        }

        private static void AppendRowEnd(IFormModel<T> model, IEnumerable<Property> entityProperities, int i, StringBuilder sb, bool trStart)
        {
            sb.Append(model.FormLayout.TdMarkupEnd);

            sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.SpaceColumnWidth));
            sb.Append(model.FormLayout.TdMarkupEnd);

            if (i % (model.FormLayout.ColumnCount) == 0 && !trStart || entityProperities.Count() - 1 == i)
            {
                sb.Append(model.FormLayout.TrMarkupEnd);

                //sb.Append(model.FormLayout.TrMarkupStart(model.FormLayout.SpaceRow));
                //sb.Append(model.FormLayout.TrMarkupEnd);
            }
        }
    }
}
