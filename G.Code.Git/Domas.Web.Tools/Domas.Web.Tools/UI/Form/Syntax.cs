using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using Domas.Web.Tools.UI.Form;

namespace Domas.Web.Tools.UI.Form
{
    public interface IForm<T> : IFormWithOptions<T> where T : class
    {
        IForm<T> WithModel(IFormModel<T> model);
    }
    public interface IFormWithOptions<T> : IHtmlString where T : class
    {
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //IFormModel<T> Model { get; }

        //IFormWithOptions<T> RenderUsing(IFormRenderer<T> renderer);

        //void Render();
    }
}

