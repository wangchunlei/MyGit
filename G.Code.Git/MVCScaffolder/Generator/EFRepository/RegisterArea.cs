﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Generator.EFRepository
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System;
    
    
    #line 1 "D:\TestWork\VSIX\MVCScaffolder\Generator\EFRepository\RegisterArea.t4"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class RegisterArea : RepositoryBase
    {
        public override string TransformText()
        {
            this.Write("using System.Web.Mvc;\r\nusing Domas.Web.Tools.PortableAreas;\r\n\r\nnamespace ");
            
            #line 11 "D:\TestWork\VSIX\MVCScaffolder\Generator\EFRepository\RegisterArea.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Namespace));
            
            #line default
            #line hidden
            this.Write("\r\n{   \r\n\t//todo:修改类名\r\n    public class RegisterArea");
            
            #line 14 "D:\TestWork\VSIX\MVCScaffolder\Generator\EFRepository\RegisterArea.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write(" : PortableAreaRegistration\r\n    {\r\n\t\tpublic override void RegisterArea(AreaRegis" +
                    "trationContext context, IApplicationBus bus)\r\n\t\t{\r\n\t\t\t//bus.Send(new Registratio" +
                    "nMessage(\"Registering Portable Area\"));\r\n\r\n\t\t\tcontext.MapRoute(\r\n\t\t\t\tAreaName,\r\n" +
                    "\t\t\t\t\"Area_");
            
            #line 22 "D:\TestWork\VSIX\MVCScaffolder\Generator\EFRepository\RegisterArea.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("/{controller}/{action}\",\r\n\t\t\t\tnew {controller = \"home\", action = \"index\"});\r\n\r\n  " +
                    "          this.RegisterAreaEmbeddedResources();\r\n\t\t}\r\n\r\n\t\tpublic override string" +
                    " AreaName\r\n\t\t{\r\n\t\t\t//todo:修改\r\n\t\t\tget { return \"Area_");
            
            #line 31 "D:\TestWork\VSIX\MVCScaffolder\Generator\EFRepository\RegisterArea.t4"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("\"; }\r\n\t\t}\r\n\t}\r\n}");
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
