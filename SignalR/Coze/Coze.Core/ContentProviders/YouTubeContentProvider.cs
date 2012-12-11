using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Coze.Core.ContentProviders
{
    public class YouTubeContentProvider : EmbedContentProvider
    {
        public override IEnumerable<string> Domains
        {
            get { yield return "http://www.youtube.com"; }
        }

        public override string MediaFormatString
        {
            get { return @"<object width=""425"" height=""344""><param name=""movie"" value=""http://www.youtube.com/v/{0}?fs=1""</param><param name=""allowFullScreen"" value=""true""></param><param name=""allowScriptAccess"" value=""always""></param><embed src=""http://www.youtube.com/v/{0}?fs=1"" type=""application/x-shockwave-flash"" allowfullscreen=""true"" allowscriptaccess=""always"" width=""425"" height=""344""></embed></object>"; }
        }

        protected override IEnumerable<object> ExtractParameters(Uri responseUri)
        {
            var queryString = HttpUtility.ParseQueryString(responseUri.Query);
            string videoId = queryString["v"];
            if (!string.IsNullOrEmpty(videoId))
            {
                yield return videoId;
            }
        }
    }
}
