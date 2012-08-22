using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokeIn_Silverlight.Web
{
    public class Definer
    {
        public static void Entry(PokeIn.Comet.ConnectionDetails details, ref Dictionary<string, object> classList)
        {
            classList.Add("Dummy", new Dummy(details.ClientId));
        }
    }
}