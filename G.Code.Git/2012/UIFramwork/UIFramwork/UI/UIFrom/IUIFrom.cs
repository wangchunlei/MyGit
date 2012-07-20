using System.Web;
using UIFramwork.UI.UIModel;

namespace UIFramwork.UI.UIFrom
{
    public interface IUIForm<T> : IFormWithOptions<T> where T : class
    {
        //IUIForm<T> WithModel(UIModel<T> model);
    }

    public interface IFormWithOptions<T> : IHtmlString where T : class
    {
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //IFormModel<T> Model { get; }

        //IFormWithOptions<T> RenderUsing(IFormRenderer<T> renderer);

        //void Render();
    }
}
