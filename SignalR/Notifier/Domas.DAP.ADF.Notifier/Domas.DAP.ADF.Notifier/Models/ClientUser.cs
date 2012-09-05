using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.DAP.ADF.Notifier.Models
{
    public class ClientUser
    {
        public string ClinetID { get; set; }
        public string ClientAgent { get; set; }
        public DateTimeOffset LastActivity { get; set; }
    }
}
