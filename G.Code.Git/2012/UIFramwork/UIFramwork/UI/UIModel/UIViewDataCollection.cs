using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIFramwork.UI.UIModel
{
    public class UIViewDataCollection
    {
        private IDictionary<string, UIViewData> _viewDataDic = new Dictionary<string, UIViewData>();

        public UIViewData this[string pos]
        {
            get { return _viewDataDic[pos]; }
            set { _viewDataDic[pos] = value; }
        }
    }
}
