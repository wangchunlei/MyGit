using System;

namespace Domas.Web.Tools.UI.InputBuilder.Attributes
{
    public class DisplayOrderAttribute : Attribute
    {
        public DisplayOrderAttribute(int order)
        {
            Order = order;
        }
        public int Order { get; set; }
    }
}