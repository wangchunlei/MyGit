using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Domas.Web.Tools.UI.InputBuilder.Conventions;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.InputSpecification
{
	public class Input<T> where T : class
	{
		private readonly HtmlHelper<T> _htmlHelper;

		public Input(HtmlHelper<T> htmlHelper)
		{
			_htmlHelper = htmlHelper;
		}

		public IInputSpecification<PropertyViewModel> RenderInput(Expression<Func<T, object>> expression)
		{
			return new InputPropertySpecification
			{
				Model =
					new ViewModelFactory<T>(_htmlHelper, InputBuilder.Conventions.ToArray(), new DefaultNameConvention(),InputBuilder.TypeConventions.ToArray()).Create(expression),
				HtmlHelper = _htmlHelper,
			};
		}

		public IInputSpecification<TypeViewModel> RenderForm(string controller, string action)
		{
			return new InputTypeSpecification<T>
			{
				HtmlHelper = _htmlHelper,
				Controller = controller,
				Action = action,
			};
		}

		public IInputSpecification<TypeViewModel> RenderForm()
		{
			return new InputTypeSpecification<T>
			{
				Model = new ViewModelFactory<T>(_htmlHelper, InputBuilder.Conventions.ToArray(), new DefaultNameConvention(),InputBuilder.TypeConventions.ToArray()).Create(),
				HtmlHelper = _htmlHelper,
			};
		}
	}
}