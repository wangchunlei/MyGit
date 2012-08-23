using System;
using System.Web;
using System.Collections.Generic;
using Domas.DAP.ADF.NotifierDeploy;

namespace Domas.DAP.ADF.Notifier
{
    public class MessageHttpHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            MessageContainer list;
            string usercode = GetUserCode(context);
            int maxCount = 0;
            while (maxCount < 6)
            {
                if(NotificationManager.CheckNewMessage(usercode,out list))
                {
                    var str = list.Serialize();
                    context.Response.ContentType = "text/plain";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.Write(str);
                    break;
                }
                maxCount++;
                System.Threading.Thread.Sleep(3000);
            }
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Write(0);
        }

        private string GetUserCode(HttpContext context)
        {
           // throw new NotImplementedException();
            return string.Empty;
        }

        #endregion
    }
}
