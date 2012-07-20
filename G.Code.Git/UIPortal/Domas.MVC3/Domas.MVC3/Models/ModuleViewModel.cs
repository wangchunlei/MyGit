using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domas.DAP.ADF.License.Module;

namespace Domas.MVC3.Models
{
    public class ModuleViewModel
    {
        //
        // 引用 BE Model 属性
        public IEnumerable<Module> Modules { get; set; }

        //
        //TODO: 附加属性
        public string LoginUser { get; set; }
        public string Message { get; set; }
    }
}