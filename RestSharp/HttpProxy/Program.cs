using System;
using System.Net;
using System.IO;

namespace HttpProxy
{
    class RequestData
    {
        public HttpWebRequest WebRequest;
        public HttpListenerContext Context;

        public RequestData(HttpWebRequest request, HttpListenerContext context)
        {
            WebRequest = request;
            Context = context;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a listener.
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://*:8082/");

            listener.Start();
            try
            {
                while (true)
                {
                    Console.WriteLine("Listening...");
                    // Note: The GetContext method blocks while waiting for a request.
                    HttpListenerContext context = listener.GetContext();
                    string requestString = context.Request.RawUrl;
                    Console.WriteLine("Got request for " + requestString);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
                    request.KeepAlive = true;
                    request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                    request.Timeout = 200000;

                    RequestData requestData = new RequestData(request, context);
                    IAsyncResult result = (IAsyncResult)request.BeginGetResponse(new AsyncCallback(RespCallback), requestData);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("\nMain Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nStatus:{0}", e.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nMain Exception raised!");
                Console.WriteLine("Source :{0} ", e.Source);
                Console.WriteLine("Message :{0} ", e.Message);
            }

            listener.Stop();
        }

        static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {

                // State of request is asynchronous.
                RequestData requestData = (RequestData)asynchronousResult.AsyncState;
                Console.WriteLine("Got back response from " + requestData.Context.Request.Url.AbsoluteUri);

                using (HttpWebResponse response = (HttpWebResponse)requestData.WebRequest.EndGetResponse(asynchronousResult))
                using (Stream receiveStream = response.GetResponseStream())
                {
                    HttpListenerResponse responseOut = requestData.Context.Response;

                    // Need to get the length of the response before it can be forwarded on
                    responseOut.ContentLength64 = response.ContentLength;
                    int bytesCopied = CopyStream(receiveStream, responseOut.OutputStream);
                    responseOut.OutputStream.Close();
                    Console.WriteLine("Copied {0} bytes", bytesCopied);
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("\nMain Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nStatus:{0}", e.Status);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nMain Exception raised!");
                Console.WriteLine("Source :{0} ", e.Source);
                Console.WriteLine("Message :{0} ", e.Message);
            }

        }

        public static int CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int bytesWritten = 0;
            while (true)
            {
                int read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    break;
                output.Write(buffer, 0, read);
                bytesWritten += read;
            }
            return bytesWritten;
        }
    }
}