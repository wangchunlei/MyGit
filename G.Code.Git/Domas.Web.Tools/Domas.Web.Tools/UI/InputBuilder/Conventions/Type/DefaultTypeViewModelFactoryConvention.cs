using System;
using Domas.Web.Tools.UI.InputBuilder.Attributes;
using Domas.Web.Tools.UI.InputBuilder.Conventions;
using Domas.Web.Tools.UI.InputBuilder.Helpers;
using Domas.Web.Tools.UI.InputBuilder.Views;

namespace Domas.Web.Tools.UI.InputBuilder.InputSpecification
{
	public class DefaultTypeViewModelFactoryConvention : ITypeViewModelFactory {
		public bool CanHandle(Type type)
		{
			return true;
		}

		public TypeViewModel Create(Type type)
		{
			return new TypeViewModel()
			{
				Label = LabelForTypeConvention(type),				
				PartialName = "Form",
				Type = type,
                
			};
		}

		public string LabelForTypeConvention(Type type)
		{
			if (type.AttributeExists<LabelAttribute>())
			{
				return type.GetAttribute<LabelAttribute>().Label;
			}
			return type.Name.ToSeparatedWords();
		}
	}
}