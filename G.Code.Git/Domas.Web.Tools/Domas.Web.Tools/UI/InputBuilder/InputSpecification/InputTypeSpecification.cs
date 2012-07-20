using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Domas.Web.Tools.UI.Html;
using Domas.Web.Tools.UI.InputBuilder.Conventions;
using Domas.Web.Tools.UI.InputBuilder.Helpers;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.InputSpecification
{
    public class InputTypeSpecification<T> : IInputSpecification<TypeViewModel> where T : class
    {
        public HtmlHelper<T> HtmlHelper { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        #region IInputSpecification Members

        public TypeViewModel Model { get; set; }

        #endregion

        public override string ToString()
        {
            var factory = new ViewModelFactory<T>(HtmlHelper, InputBuilder.Conventions.ToArray(), new DefaultNameConvention(), InputBuilder.TypeConventions.ToArray());

            var models = new List<PropertyViewModel>();
            foreach (PropertyInfo propertyInfo in Model.Type.GetProperties().ReOrderProperties())
            {
                models.Add(factory.Create(propertyInfo, Model.Name));
            }
            HtmlHelper.RenderPartial(Model.PartialName, models.ToArray());
            return string.Empty;
        }

        protected virtual void RenderPartial(PropertyViewModel model)
        {
            HtmlHelper.RenderPartial(model.PartialName, model, model.Layout);
        }
    }
}