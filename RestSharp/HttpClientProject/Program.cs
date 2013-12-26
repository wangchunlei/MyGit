using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Upload();
            //RunClient();
            UploadClient.RunClient();
            //var client = new RestClient("http://localhost:2616/api/upload/downloadfile?filename=1.pdf");
            //var request = new RestRequest("api/upload/downloadfile?filename=1.pdf");

            //var data = client.DownloadData(request);
            Console.ReadKey();
        }
        static void RunClient()
        {
            HttpClient client = new HttpClient();

            // Issue GET request 
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("http://localhost:2616/api/upload/downloadfile?filename=1.txt"),
            };

            client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ContinueWith(
                (getTask) =>
                {
                    if (getTask.IsCanceled)
                    {
                        return;
                    }
                    if (getTask.IsFaulted)
                    {
                        throw getTask.Exception;
                    }
                    HttpResponseMessage response = getTask.Result;

                    // Get response stream
                    response.Content.ReadAsStreamAsync().ContinueWith(
                        (streamTask) =>
                        {
                            if (streamTask.IsCanceled)
                            {
                                return;
                            }
                            if (streamTask.IsFaulted)
                            {
                                throw streamTask.Exception;
                            }

                            // Read response stream
                            byte[] readBuffer = new byte[1024 * 1024 * 8];
                            ReadResponseStream(streamTask.Result, readBuffer);
                            Console.WriteLine("transover");
                        });
                });
        }

        private static void ReadResponseStream(Stream rspStream, byte[] readBuffer)
        {
            Task.Factory.FromAsync<byte[], int, int, int>(rspStream.BeginRead, rspStream.EndRead, readBuffer, 0, readBuffer.Length, state: null).ContinueWith(
                (readTask) =>
                {
                    if (readTask.IsCanceled)
                    {
                        return;
                    }
                    if (readTask.IsFaulted)
                    {
                        throw readTask.Exception;
                    }

                    int bytesRead = readTask.Result;
                    string content = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                    Console.WriteLine("Received: {0}", content);

                    if (bytesRead != 0)
                    {
                        ReadResponseStream(rspStream, readBuffer);
                    }
                });
        }
        private static void Upload()
        {
            using (FileStream reader = File.OpenRead("d:\\download\\professional_asp.net_3.5_security_membership_and_role_management_with_c_and_vb.pdf"))
            {
                var client = new RestClient("http://localhost/");
                var length = 8388608;

                int bytesRead = 0;
                int read = 0;
                var last = reader.Length;
                while (last > 0)
                {
                    var request = new RestRequest("api/upload/UploadFile?chunk={chunk}");
                    request.AddUrlSegment("chunk", bytesRead.ToString());
                    var bl = (int)Math.Min(length, last);
                    byte[] buffer = new byte[bl]; //8M buffer
                    read = reader.Read(buffer, 0, bl);

                    request.Method = Method.POST;
                    request.AddFile("file", buffer, "professional_asp.net_3.5_security_membership_and_role_management_with_c_and_vb.pdf");
                    var response = client.Execute(request);
                    //System.Threading.Thread.Sleep(TimeSpan.FromDays(1));
                    bytesRead += read;
                    last -= read;
                    Console.WriteLine("uploading: " + (int)((double)bytesRead / reader.Length * 100) + "%");
                }
                reader.Close();
            }
        }

        private static void writer(Stream stream)
        {
            using (FileStream reader = File.OpenRead("d:\\download\\largefile.iso"))
            {
                byte[] buffer = new byte[16384]; //16k buffer
                int bytesRead = 0;
                int read = 0;

                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, read);
                    bytesRead += read;
                    Console.WriteLine("uploading: " + (int)((double)bytesRead / reader.Length * 100) + "%");
                }
                reader.Close();
            }

        }
    }
}
