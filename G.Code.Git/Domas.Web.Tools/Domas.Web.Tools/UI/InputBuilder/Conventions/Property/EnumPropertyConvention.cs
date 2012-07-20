using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Domas.Web.Tools.UI.InputBuilder.InputSpecification;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.Conventions
{
	public class EnumPropertyConvention : DefaultPropertyConvention
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType.IsEnum;
		}

		public override PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type)
		{
			object value = base.ValueFromModelPropertyConvention(propertyInfo, model, name);

			SelectListItem[] selectListItems = Enum.GetNames(propertyInfo.PropertyType).Select(
				s => new SelectListItem {Text = s, Value = s, Selected = s == value.ToString()}).ToArray();

			PropertyViewModel viewModel = base.Create(propertyInfo, model, name, type);
			viewModel.Value = selectListItems;
			viewModel.PartialName = "Enum";

			return viewModel;
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<IEnumerable<SelectListItem>> {};
		}
	}
}