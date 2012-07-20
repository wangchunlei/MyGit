using System.Web.Mvc;

namespace Domas.Web.Tools.Binders
{
    public interface ITypeStampOperator
    {
        string DetectTypeStamp(ModelBindingContext bindingContext, IPropertyNameProvider propertyNameProvider);
    }
}
