using System;
using System.Reflection;
using Domas.Web.Tools.UI.InputBuilder.InputSpecification;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelFactory
	{
		bool CanHandle(PropertyInfo propertyInfo);
		PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type);
	}

	public interface  IRequireViewModelFactory
	{
		void Set(IViewModelFactory factory);
	}
}