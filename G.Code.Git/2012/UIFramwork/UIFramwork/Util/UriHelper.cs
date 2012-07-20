using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UIFramwork.Util
{
    public static class UriHelper
    {
        public static NameValueCollection GetQuery(string uri)
        {
            var uriBuilder = new UriBuilder(uri);

            var nvc = HttpUtility.ParseQueryString(uriBuilder.Query);

            return nvc;
        }
    }
}
