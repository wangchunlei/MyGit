using System.Collections.Generic;
using Domas.Web.Tools.UI.InputBuilder.Conventions;

namespace Domas.Web.Tools.UI.InputBuilder
{
	public class DefaultPropertyConventionsFactory : List<IPropertyViewModelFactory>
	{
		public DefaultPropertyConventionsFactory()
		{
			Add(new ArrayPropertyConvention());
			Add(new GuidPropertyConvention());
			Add(new EnumPropertyConvention());
			Add(new DateTimePropertyConvention());
			Add(new DefaultPropertyConvention());
		}
	}
}