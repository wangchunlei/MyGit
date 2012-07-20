using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using UIFramwork.UI.UIModel;

namespace UIFramwork.UI.UIFrom
{
    public class UIForm<T> : IUIForm<T> where T : class
    {
        private readonly HtmlHelper<T> _helper;
        private UIFramwork.UI.UIModel.UIModel<T> _uiModel;
        private dynamic _modelData;
        private string _typeFullName;
        private FormTypeEnum _formType;
        private string _parentId;
        private int _pop;

        public UIForm(HtmlHelper<T> htmlHelper, FormParameter parameter)
        {
            // TODO: Complete member initialization
            this._helper = htmlHelper;
            this._typeFullName = parameter.TypeFullName;
            this._formType = parameter.FormType;
            this._parentId = parameter.ParentId;
            _pop = parameter.Pop;
        }

        public string ToHtmlString()
        {
            TextWriter writer = new StringWriter();
            _uiModel = new UIModel.UIModel<T>(_typeFullName, _formType);
            _uiModel.Pop = _pop;
            if (_uiModel.HasError)
            {
                writer.Write("出错了，找不到对应的源数据！！！");
            }
            else
            {
                if (_formType == FormTypeEnum.List)
                {
                    FormRenderer.RenderList(_uiModel, writer, _typeFullName, _parentId);
                }
                else
                {
                    FormRenderer.RenderCard(_uiModel, writer, _helper);
                }
            }

            return writer.ToString();
        }
        public override string ToString()
        {
            return ToHtmlString();
        }
    }
}
