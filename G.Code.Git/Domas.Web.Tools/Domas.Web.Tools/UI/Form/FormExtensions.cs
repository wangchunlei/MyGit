using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Domas.Web.Tools.Authorize.Models;

namespace Domas.Web.Tools.UI.Form
{
    public static class FormExtensions
    {
        public static IForm<T> Form<T>(this HtmlHelper<T> helper, T data, List<CustomerUIProperty> displayProperties = null) where T : class
        {
            return new Form<T>(data, helper,displayProperties);
        }
    }
}
