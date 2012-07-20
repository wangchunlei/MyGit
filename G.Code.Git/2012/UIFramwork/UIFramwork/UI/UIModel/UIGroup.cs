using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramwork.UI.UIModel
{
    public class UIGroup<T> where T : class
    {
        public string Title { get; set; }
        public string GroupName { get; set; }
        public int? Position { get; set; }
        public new IList<UIProperty<T>> Properties { get; set; }
    }
}
