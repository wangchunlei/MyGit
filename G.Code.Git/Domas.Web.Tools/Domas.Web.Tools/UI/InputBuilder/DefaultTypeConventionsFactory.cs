using System.Collections.Generic;
using Domas.Web.Tools.UI.InputBuilder.InputSpecification;

namespace Domas.Web.Tools.UI.InputBuilder
{
	public class DefaultTypeConventionsFactory : List<ITypeViewModelFactory>
	{
		public DefaultTypeConventionsFactory()
		{
			Add(new DefaultTypeViewModelFactoryConvention());
		}
	}
}