using System.Reflection;

namespace Domas.Web.Tools.UI.InputBuilder.Conventions
{
	public class DefaultNameConvention : IPropertyViewModelNameConvention
	{
		public virtual string PropertyName(PropertyInfo propertyInfo)
		{
			return propertyInfo.Name;
		}
	}
}