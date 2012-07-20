using System;
using System.Web.Mvc;
using Domas.Web.Tools.UI.Html;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.InputSpecification
{
	public class InputPropertySpecification : IInputSpecification<PropertyViewModel>,IInputSpecification<TypeViewModel>
	{
		public Func<HtmlHelper, PropertyViewModel, string> Render =
			(helper, model) =>
			{
				helper.RenderPartial(model.PartialName, model, model.Layout);
				return "";
			};

		public HtmlHelper HtmlHelper { get; set; }


		public PropertyViewModel Model { get; set; }


		public override string ToString()
		{
			return Render(HtmlHelper, Model);
		}

		TypeViewModel IInputSpecification<TypeViewModel>.Model
		{
			get
			{
				return this.Model;
			}
		}
	}
}