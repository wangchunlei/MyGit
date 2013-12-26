using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientProject
{
    public class UploadClient
    {
        const int BufferSize = 1024 * 1024 * 8;
        const int SplitValue = 1024 * 1024 * 1024;
        static readonly Uri _baseAddress = new Uri("http://localhost:2616/");
        static readonly string _filename = @"F:\book\C#\[Head.First.HTML5.2011].Eric.Freeman.文字版.pdf";
        internal static async void RunClient()
        {
            var progress = new ProgressMessageHandler();
            progress.HttpSendProgress += (sender, eventArgs) =>
            {
                var request = sender as HttpRequestMessage;

                string message;
                if (eventArgs.TotalBytes != null)
                {
                    message = String.Format("  Request {0} uploaded {1} of {2} bytes ({3}%)",
                    request.RequestUri, eventArgs.BytesTransferred, eventArgs.TotalBytes, eventArgs.ProgressPercentage);
                }
                else
                {
                    message = String.Format("  Request {0} uploaded {1} bytes",
                    request.RequestUri, eventArgs.BytesTransferred, eventArgs.TotalBytes, eventArgs.ProgressPercentage);
                }
                Console.WriteLine(message);
            };

            await UploadToServer(progress);
        }
        static FileStream fileStream = null;
        private static async Task UploadToServer(ProgressMessageHandler progress)
        {
            using (HttpClient client = HttpClientFactory.Create(progress))
            {
                client.Timeout = TimeSpan.FromMinutes(20);
                using (fileStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
                {
                    var content = new StreamContent(fileStream, BufferSize);
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent("Me"), "submitter");
                    formData.Add(content, "filename", _filename);

                    var address = new Uri(_baseAddress, "api/upload/uploadfile");
                    HttpResponseMessage response = await client.PostAsync(address, formData);

                    var result = await response.Content.ReadAsAsync<FileResult>();
                    Console.WriteLine("{0}Result:{0}  Filename:  {1}{0}  Submitter: {2}", Environment.NewLine, result.FileNames.FirstOrDefault(), result.Submitter);
                }
            }
        }
        //private static async Task UploadToServer(ProgressMessageHandler progress)
        //{
        //    HttpClient client = HttpClientFactory.Create(progress);
        //    client.Timeout = TimeSpan.FromMinutes(20);
        //    using (var fileStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true))
        //    {
        //        var content = new StreamContent(fileStream, BufferSize);
        //        var formData = new MultipartFormDataContent();
        //        formData.Add(new StringContent("Me"), "submitter");
        //        formData.Add(content, "filename", _filename);

        //        var address = new Uri(_baseAddress, "api/upload/uploadfile");
        //        HttpResponseMessage response = await client.PostAsync(address, formData);

        //        var result = await response.Content.ReadAsAsync<FileResult>();
        //        Console.WriteLine("{0}Result:{0}  Filename:  {1}{0}  Submitter: {2}", Environment.NewLine, result.FileNames.FirstOrDefault(), result.Submitter);
        //    }
        //}

    }
    public class FileResult
    {
        /// <summary>
        /// Gets or sets the local path of the file saved on the server.
        /// </summary>
        /// <value>
        /// The local path.
        /// </value>
        public IEnumerable<string> FileNames { get; set; }

        /// <summary>
        /// Gets or sets the submitter as indicated in the HTML form used to upload the data.
        /// </summary>
        /// <value>
        /// The submitter.
        /// </value>
        public string Submitter { get; set; }
    }
}
