﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Generator.RazorView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    
    
    #line 1 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class Search : ViewBase
    {
        public override string TransformText()
        {
            this.Write("\r\n");
            
            #line 13 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
 var viewDataType = Model.ViewDataType; 
            
            #line default
            #line hidden
            
            #line 14 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
 if(viewDataType != null) { 
            
            #line default
            #line hidden
            this.Write("@model ");
            
            #line 15 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(viewDataType.FullName));
            
            #line default
            #line hidden
            this.Write("\r\n\r\n");
            
            #line 17 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
 } 
            
            #line default
            #line hidden
            this.Write(@"@{
    ViewBag.Title = ""Search"";
	Layout = null;
}


<script type=""text/javascript"">
    $('#a_search').click(function () {
        var allInputs = $('#search_content input[type=text]');
        var params = {};
        $.each(allInputs, function (i, input) {
            if (input.value) {
                if (input.id) {
                    params[input.id] = input.value;
                }
                else if (input.name) {
                    params[input.name] = input.value;
                } else if (allInputs[i - 1].id) {
                    params[allInputs[i - 1].id] = input.value;
                } else if (allInputs[i - 1].name) {
                    params[allInputs[i - 1].name] = input.value;
                }
            }
        });

        $('#");
            
            #line 43 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(viewDataType.Name));
            
            #line default
            #line hidden
            this.Write("Table\').datagrid(\'options\').queryParams = params;\r\n        $(\'#");
            
            #line 44 "D:\TestWork\VSIX\MVCScaffolder\Generator\RazorView\Search.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(viewDataType.Name));
            
            #line default
            #line hidden
            this.Write("Table\').datagrid(\'reload\');\r\n    })\r\n</script>\r\n<fieldset>\r\n    <legend>查询条件</leg" +
                    "end>\r\n    <div id=\"search_content\">\r\n        @Html.Partial(\"_CreateOrEdit\", Mode" +
                    "l)\r\n    </div>\r\n    <input id=\"a_search\" type=\"button\" value=\"查询\" />\r\n</fieldset" +
                    ">\r\n﻿ \r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }
        private global::Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost hostValue;
        public virtual global::Microsoft.VisualStudio.TextTemplating.ITextTemplatingEngineHost Host
        {
            get
            {
                return this.hostValue;
            }
            set
            {
                this.hostValue = value;
            }
        }
    }
    
    #line default
    #line hidden
}
