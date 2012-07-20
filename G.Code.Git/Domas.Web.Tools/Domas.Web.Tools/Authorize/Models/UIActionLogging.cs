using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.Web.Tools.Authorize.Models
{
    public class UIActionLogging
    {
        public int ID { get; set; }
        public string User { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IP { get; set; }
        public DateTime DateTime { get; set; }
    }
}
