using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIFramwork.Util;

namespace UIFramwork.UI.UIModel
{
    public class UIViewData : DynamicProperty
    {
        public dynamic Value { get; set; }
        public Type Datatype { get; set; }
        public UIViewData(object instance)
            : base(instance)
        {
        }
    }
}
