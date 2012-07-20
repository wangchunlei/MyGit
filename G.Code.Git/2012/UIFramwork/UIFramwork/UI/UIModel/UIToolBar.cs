using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramwork.UI.UIModel
{
    public class UIToolBar<T> where T : class
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public Func<T, object> ActionFun { get; set; }
    }
}
