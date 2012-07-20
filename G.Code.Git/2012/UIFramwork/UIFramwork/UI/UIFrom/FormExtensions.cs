using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UIFramwork.UI.UIFrom
{
    public static class FormExtensions
    {
        public static IUIForm<T> DynamicForm<T>(this HtmlHelper<T> htmlHelper, FormParameter parameter) where T : class
        {
            return new UIForm<T>(htmlHelper, parameter);
        }
    }

    public enum FormTypeEnum
    {
        Create = 0x1,
        Update = 0x2,
        List = 0x3
    }

    public class FormParameter
    {
        public int Pop { get; set; }
        public string TypeFullName { get; set; }
        public FormTypeEnum FormType { get; set; }
        public string ParentId { get; set; }
    }
}
