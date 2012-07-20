using System;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.InputSpecification
{
	public interface ITypeViewModelFactory {
		bool CanHandle(Type type);
		TypeViewModel Create(Type type);
	}
}