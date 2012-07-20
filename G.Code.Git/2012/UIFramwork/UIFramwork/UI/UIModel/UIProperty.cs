using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domas.DAP.ADF.MetaData;

namespace UIFramwork.UI.UIModel
{
    public class UIProperty<T> where T : class
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Code { get; set; }
        public bool Readonly { get; set; }
        public bool Visible { get; set; }
        public int? Position { get; set; }
        public MetaDataType PropertyType { get; set; }
        public string FullTypeName { get; set; }
        public new Func<T, object> PropertyValueFunc { get; set; }
        public new List<Func<T, IDictionary<string, object>>> Attributes = new List<Func<T, IDictionary<string, object>>>();
    }
}
