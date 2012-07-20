using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UIFramwork.UI.FormRender
{
    public class Form<T> : IForm<T> where T : class
    {

        public IForm<T> WithModel(IFormModel<T> model)
        {
            throw new NotImplementedException();
        }
        private IFormModel<T> _formModel = new FormModel<T>();
        private readonly HtmlHelper<T> helper;
        public T Data { get; private set; }
        private List<CustomerUIProperty> DisplayProperties { get; set; }
        public IFormModel<T> Model
        {
            get { return _formModel; }
        }

        public Form(T data, HtmlHelper<T> helper, List<CustomerUIProperty> displayProperties)
        {
            this.helper = helper;
            Data = data;
            this.DisplayProperties = displayProperties ?? new List<CustomerUIProperty>();
        }
        public string ToHtmlString()
        {
            var writer = new StringWriter();

            Model.Renderer.Render(Model, Data, writer, helper, DisplayProperties);

            return writer.ToString();
        }

        public override string ToString()
        {
            return ToHtmlString();
        }
    }
}
