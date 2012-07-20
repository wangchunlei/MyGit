using System;
using System.Reflection;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.Conventions
{
	public class DateTimePropertyConvention : DefaultPropertyConvention
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType == typeof(DateTime);
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<DateTime> {};
		}
	}
}