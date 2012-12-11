using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Coze.Core.ContentProviders
{
    public class ImageContentProvider : IContentProvider
    {
        private static readonly HashSet<string> _imageMimiTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
           {
               "image/png",
               "image/jpg",
               "image/jpeg",
               "image/bmp",
               "image/gif"
           };

        public string GetContent(HttpWebResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
