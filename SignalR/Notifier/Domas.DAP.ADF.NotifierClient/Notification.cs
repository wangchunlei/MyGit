using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using Domas.DAP.ADF.NotifierDeploy;

namespace Domas.DAP.ADF.NotifierClient
{
    public delegate void NotificationEventHandler(Message m);

    // public delegate void ConnectionEventHandler();

    public class Notification
    {
        public event NotificationEventHandler OnReceived;

        //public event ConnectionEventHandler OnConnected;

        private string host;
        
        public Notification() : this(@"http://localhost:10824") { }//getfromconfig

        public Notification(string host)
        {
            this.host = host;
            //coment 

            //socket
            //createconnection
        }

        public void Start()
        {
            //while 
            this.BeginWaitRequest(this.host + "/MessageHttpHandler.ashx");
        }
        /// <summary>
        /// Send a command to the specific handler
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected WebResponse SendCommand(string url, string[] parameters, string[] values)
        {
            //  ok, create the request object that is going to perform this
            //  request
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);


            //  and create the post data
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //if (this.httpUsername != null)
            //{
            //    request.Credentials = new NetworkCredential(this.httpUsername, this.httpPassword);
            //}

            //
            //  ok, parse the data from the parameters into 
            //  the content data
            if (parameters != null && values != null && parameters.Length == values.Length)
            {
                //  ok, everything is ok, so craete the post data
                StringBuilder postData = new StringBuilder();

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (i != 0)
                        postData.Append("&");

                    //  append for values
                    postData.AppendFormat("{0}={1}", parameters[i], HttpUtility.UrlEncode(values[i]));
                }

                //  and now create a postdata array
                byte[] content = ASCIIEncoding.ASCII.GetBytes(postData.ToString());
                //  and the content length
                request.ContentLength = content.Length;

                //
                //  ok, sorted, so lets get the request strema and write the content
                using (Stream requestStream = request.GetRequestStream())
                {
                    //  write the content
                    requestStream.Write(content, 0, content.Length);
                }
            }

            //
            //  ok, now get the response
            return request.GetResponse();
        }

        /// <summary>
        /// Send a command to the specific handler
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected void BeginWaitRequest(string url)
        {
                //  ok, create the request object that is going to perform this
                //  request
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

                //  and create the post data
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                //if (this.httpUsername != null)
                //{
                //    request.Credentials = new NetworkCredential(this.httpUsername, this.httpPassword);
                //}

                request.AllowWriteStreamBuffering = true;
                request.Accept = "text/plain";
                request.ContentLength = 0;

                //  and start the async request
                try
                {
                    var response = request.GetResponse();
                    //request.BeginGetResponse(new AsyncCallback(BeginGetResponse_Completed), request);
              
                }
                catch (Exception)
                {
                    
                   // throw;
                }
              


        }

        private void BeginGetResponse_Completed(IAsyncResult result)
        {
            OnReceived(new Message() { ID = new Guid(), UserCode = "Code", Data = "", Expiration = DateTime.Now });
        }
    }
}
