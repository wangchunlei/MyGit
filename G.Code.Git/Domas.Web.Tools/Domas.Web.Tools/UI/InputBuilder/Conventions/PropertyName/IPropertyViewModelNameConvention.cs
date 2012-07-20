using System.Reflection;

namespace Domas.Web.Tools.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelNameConvention
	{
		string PropertyName(PropertyInfo propertyInfo);
	}
}