using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Domas.DAP.ADF.Context;
using Domas.DAP.ADF.MetaData;
using UIFramwork.UI.UIModel;
using UIFramwork.Util;

namespace UIFramwork.UI.UIFrom
{
    public static class FormRenderer
    {
        #region 卡片
        public static void RenderCard<T>(UIModel<T> model, System.IO.TextWriter output,
            System.Web.Mvc.HtmlHelper<T> helper) where T : class
        {
            var sb = new StringBuilder();
            var parent = MetadataExtensions.FindParent(model.ModelName);
            if (!string.IsNullOrEmpty(parent))
            {
                var name = parent.Split('.').LastOrDefault() + "ID";
                sb.Append(InputExtensions.Hidden(helper, name, helper.ViewBag.ParentId));
                sb.Append(helper.Hidden("ParentName", name));
            }
            foreach (var dis in model.UIGroups)
            {
                sb.Append("<fieldset>");
                sb.Append("<legend>" + dis.Title + "</legend>");
                sb.Append("<div id='" + dis.GroupName + "'>");
                AppendBody(model, helper, dis.Properties, sb, dis.GroupName);
                sb.Append("</div>");
                sb.Append("</fieldset>");
            }

            output.Write(sb.ToString());
        }

        private static void AppendBody<T>(UIModel<T> model, HtmlHelper<T> helper,
                                          IList<UIProperty<T>> uiProperities, StringBuilder sb,
                                          string divID) where T : class
        {
            sb.Append(model.FormLayout.TableMarkupStart);
            //blank row
            //sb.Append(model.FormLayout.TrMarkupStart(20));
            //sb.Append(model.FormLayout.TrMarkupEnd);
            //List<string> inner = new List<string>() { "CreatedOn" };

            bool trStart = false;
            for (int i = 0; i < uiProperities.Count(); i++)
            {
                trStart = false;
                var property = uiProperities[i];

                if (i % (model.FormLayout.ColumnCount) == 0)
                {
                    sb.Append(model.FormLayout.TrMarkupStart(model.FormLayout.RowHeight));
                    //blank column
                    sb.Append(model.FormLayout.TdMarkupStart(10));
                    sb.Append(model.FormLayout.TdMarkupEnd);
                    trStart = true;
                }

                //label column
                sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.LabelColumnWidth, "right"));
                sb.Append(helper.Label(property.Name));
                sb.Append(model.FormLayout.TdMarkupEnd);

                //gap column
                sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.GapColumnWidth));
                sb.Append(model.FormLayout.TdMarkupEnd);

                //control column
                sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.ControlColumnWidth));

                var typeName = property.FullTypeName;
                var isGenericType = typeName.EndsWith("?");

                switch (property.PropertyType)
                {
                    case MetaDataType.Datetime:
                        if (property.Readonly)
                        {
                            sb.Append(helper.Display(property.Code));
                        }
                        else
                        {
                            sb.Append(helper.TextBox(property.Code));
                            var isPopClient = helper.ViewContext.ViewData["IsPopFromClient"];
                            if (isPopClient != null && !(bool)isPopClient)
                            {
                                sb.Append("<script language=\"javascript\">$('#" + divID + "').find('#" + property.Code + "').datebox();</script>");
                            }
                        }

                        break;
                    case MetaDataType.String:
                        if (property.Readonly)
                        {
                            sb.Append(helper.Display(property.Code));
                        }
                        else
                        {
                            sb.Append(helper.TextBox(property.Code));
                        }

                        break;
                    case MetaDataType.Boolean:
                        if (property.Readonly)
                        {
                            sb.Append(helper.Display(property.Code));
                        }
                        else
                        {
                            sb.Append(helper.Editor(property.Code));
                        }
                        break;
                    case MetaDataType.Decimal:
                        if (property.Readonly)
                        {
                            sb.Append(helper.Display(property.Code));
                        }
                        else
                        {
                            sb.Append(helper.Editor(property.Code));
                        }
                        break;
                    case MetaDataType.Integer:
                        if (property.Readonly)
                        {
                            sb.Append(helper.Display(property.Code));
                        }
                        else
                        {
                            sb.Append(helper.Editor(property.Code));
                        }
                        break;
                    case MetaDataType.Entity:
                        break;
                        if (isGenericType)
                        {
                            if (property.Readonly)
                            {
                                sb.Append(helper.Label(helper.ViewContext.ViewData["Possible" + property.Code + "Name"].ToString()));
                            }
                            else
                            {
                                if (helper.ViewContext.ViewData["Possible" + property.Code] != null)
                                {
                                    var forStr = helper.DropDownList(property.Code,
                                                 (IEnumerable<SelectListItem>)
                                                 helper.ViewContext.ViewData[
                                                     "Possible" + property.Code]);
                                    sb.Append(forStr);
                                }

                            }
                        }
                        else
                        {
                            var forStr = helper.DropDownList(property.Code,
                                                                (IEnumerable<SelectListItem>)
                                                                helper.ViewContext.ViewData[
                                                                    "Possible" + property.Code]);
                            sb.Append(forStr);

                        }

                        break;
                    case MetaDataType.Enumeration:
                        var enumExpress = property.Code + "_EnumValue";
                        if (isGenericType)
                        {

                            if (property.Readonly)
                            {
                                sb.Append(helper.Display(enumExpress));
                                //sb.Append(helper.Label(helper.ViewContext.ViewData["Possible" + property.Code+"Name"].ToString()));
                            }
                            else
                            {
                                var forStr = helper.DropDownList(enumExpress,
                                                                    (IEnumerable<SelectListItem>)
                                                                    helper.ViewContext.ViewData[
                                                                        "Possible" + property.Code]);
                                sb.Append(forStr);
                            }
                        }
                        else
                        {
                            if (property.Readonly)
                            {
                                sb.Append(helper.Display(enumExpress));
                            }
                            else
                            {
                                var forStr = helper.DropDownList(enumExpress,
                                                                    (IEnumerable<SelectListItem>)
                                                                    helper.ViewContext.ViewData[
                                                                        "Possible" + property.Code]);
                                sb.Append(forStr);
                            }
                        }

                        break;
                    default:
                        AppendRowEnd(model, uiProperities, i, sb, trStart);
                        continue;
                }
                AppendRowEnd(model, uiProperities, i, sb, trStart);
            }


            //var edit = System.Web.Mvc.Html.EditorExtensions.EditorFor(helper, expr);

            sb.Append(model.FormLayout.TableMarkupEnd);
        }

        private static void AppendRowEnd<T>(UIModel<T> model, IList<UIProperty<T>> uiProperities, int i,
                                            StringBuilder sb, bool trStart) where T : class
        {
            sb.Append(model.FormLayout.TdMarkupEnd);

            sb.Append(model.FormLayout.TdMarkupStart(model.FormLayout.SpaceColumnWidth));
            sb.Append(model.FormLayout.TdMarkupEnd);

            if (i % (model.FormLayout.ColumnCount) == 0 && !trStart || uiProperities.Count() - 1 == i)
            {
                sb.Append(model.FormLayout.TrMarkupEnd);

                //sb.Append(model.FormLayout.TrMarkupStart(model.FormLayout.SpaceRow));
                //sb.Append(model.FormLayout.TrMarkupEnd);
            }
        }
        #endregion

        #region 列表
        public static void RenderList<T>(UIModel<T> model, System.IO.TextWriter output, string entityName, string parentId) where T : class
        {
            string mark = entityName.Split('.').LastOrDefault();
            var sb = new StringBuilder();

            sb.Append(@"
                        <script type='text/javascript'>
	                        $(document).ready(function () {
	                            $('#^MarkTable').datagrid({
	                                title: '^Mark List',
	           	                    iconCls: 'icon-save',          
	                                fitColumns: false,
				                    striped: true,
				                    sortName: 'CreatedOn',
				                    sortOrder: 'desc',
				                    remoteSort: true,
				                    fit: true,
				                    idField: 'ID',
				                    frozenColumns:[[
					                    {field: 'ID', checkbox: true}
				                    ]],
	                                nowrap: false,");
            sb.AppendFormat("url: 'List?entity={0}',", model.ModelName);
            sb.Append(@"columns: [[");

            var propertities = model.UIGroups.SelectMany(g => g.Properties);
            int i = 1;
            int count = propertities.Count();
            foreach (var p in propertities)
            {
                sb.AppendLine("{");
                sb.AppendFormat(@"field:'{0}',title: '{1}',width: '{2}'", p.Code, p.DisplayName, 80);
                sb.AppendLine("}");
                sb.AppendFormat("{0}", (i++) != count ? "," : string.Empty);
            }

            sb.Append(@"]],
	                    pagination: true,
				        rownumbers: true,
	                    showFooter: true,
				        toolbar:[{
	        		        text:'新增',
	        		        iconCls:'icon-add',
	        		        handler:function(){");
            //showModalpage('Create','新建','^MarkTable');
            if (string.IsNullOrEmpty(parentId))
            {
                sb.Append("showModalpage('Create','新建','^MarkTable',1);");
            }
            else
            {
                sb.AppendFormat("showModalpage('Create','新建','^MarkTable',{2},'','{0}','{1}');", parentId, entityName, model.Pop);
            }
            sb.Append(@"}
	   			        },{
			                text:'修改',
			                iconCls:'icon-save',
			                handler:function(){
			                    var row = $('#^MarkTable').datagrid('getSelected');
						        if(!row){
							        alert('选择要修改的行');
							        return;
						        }");
            sb.AppendFormat("showModalpage('Update','修改','^MarkTable',{2},row.ID,'{0}','{1}');", parentId, entityName, model.Pop);
            sb.Append(@"}
			            }, '-', {
	                        text: '提交',
	                        iconCls: 'icon-print',
	                        handler: function () {
	                            var row = $('#^MarkTable').datagrid('getSelected');
	                            if (!row) {
	                                alert('选择要提交的行');
	                                return;
	                            }
	                            //var url = '@Url.Action('Submit',new {id='_TOREPLACE_'})';
	                            url = url.replace('_TOREPLACE_', row.ID);
	                            showModalpage(url, '打印作业', '^MarkTable');
	                        }
	                    },'-',{
			                text:'删除',
			                iconCls:'icon-cut',
			                handler:function(){
			                    var row = $('#^MarkTable').datagrid('getSelections');
						        if(!row){
							        alert('选择要删除的行');
							        return;
						        }
                                $.messager.confirm('确定','是否要删除当前选择的记录?',function(r){  
                                    if (r){");
            sb.AppendFormat("Delete('{0}',row);", entityName);
            sb.Append(@"}  
                                });  
			                }
	    		        }, '-', {
                            text: '高级查询',
                            iconCls: 'icon-search',
                            handler: function () {
                                collapse();
                            }
                        }],
	                    onDblClickRow: function (rowIndex, rowData) {
	                        //var url = @Url.Action('Details',new {id='_TOREPLACE_'});
	                        url = url.replace('_TOREPLACE_', rowData.ID);
	                        showpage(url);
	                    }
	                });
	            });
		        $(document).ready(function () {
			        var fields = $('#^MarkTable').datagrid('getColumnFields');
		            for (var i = 0; i < fields.length; i++) {
		                var opts = $('#^MarkTable').datagrid('getColumnOption', fields[i]);
		                var muit = '<div name=' + fields[i] + '>' + opts.title + '</div>';
		                $('#^Mark_mm').html($('#^Mark_mm').html() + muit);
		            }
		            $('#^Mark_ss').searchbox({
		                width: 300,
		                searcher: function (value, name) {
					        var params = {};
		                    if (value) {
		                        params[name] = value;
		                        $('#^MarkTable').datagrid('options').queryParams = params;
		                        $('#^MarkTable').datagrid('reload');
		                    }else{
						        $('#^MarkTable').datagrid('options').queryParams = params;
		                        $('#^MarkTable').datagrid('reload');
					        }
		                },
		                menu: '#^Mark_mm',
		                prompt: '请输入查询条件'
		            });

		            $('#^Mark_searchbox_div').appendTo('.datagrid-toolbar');
		        });
	        </script>");
            //sb.Replace("showModalpage", mark + "_showModalpage");
            output.Write(sb.Replace("^Mark", mark).ToString());
        }
        #endregion
    }
}
