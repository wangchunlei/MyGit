using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UIFramwork.UI.FormRender
{
    public static class FormExtensions
    {
        public static IForm<T> Form<T>(this HtmlHelper<T> helper, T data, List<CustomerUIProperty> displayProperties = null) where T : class
        {
            return new Form<T>(data, helper, displayProperties);
        }
    }
}
