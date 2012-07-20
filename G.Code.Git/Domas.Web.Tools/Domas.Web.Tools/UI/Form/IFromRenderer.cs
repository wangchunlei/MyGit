using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Domas.Web.Tools.Authorize.Models;

namespace Domas.Web.Tools.UI.Form
{
    public interface IFromRenderer<T> where T : class
    {
        void Render(IFormModel<T> model, T datasource, TextWriter output, HtmlHelper<T> helper, List<CustomerUIProperty> displayProperties);
    }
}
