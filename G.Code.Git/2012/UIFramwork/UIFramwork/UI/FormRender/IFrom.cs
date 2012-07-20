using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UIFramwork.UI.FormRender
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
