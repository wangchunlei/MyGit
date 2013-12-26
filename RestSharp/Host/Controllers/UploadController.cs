using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Host.Controllers
{
    public class UploadController : ApiController
    {
        public HttpResponseMessage DownloadFile(string fileName)
        {
            string fileSaveLocation = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(
                (outputStream, httpContent, transportContext) =>
                {
                    using (var reader = File.OpenRead(Path.Combine(fileSaveLocation, fileName)))
                    {
                        var length = 8388608;//8M buffer
                        var buffer = new byte[length];
                        var startlength = (int)reader.Length;
                        var actualLength = (int)Math.Min(length, startlength);
                        while (reader.Read(buffer, 0, actualLength) > 0)
                        {
                            outputStream.WriteAsync(buffer, 0, actualLength);
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
                            startlength -= actualLength;
                            if (startlength < length)
                            {
                                actualLength = startlength;
                            }
                        }
                    }
                });

            return response;
        }

        public async Task<FileResult> UploadFile(string appendfilename = null)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string fileSaveLocation = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation, appendfilename);
            var files = new List<string>();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                return new FileResult
                {
                    FileNames = provider.FileData.Select(entry => entry.LocalFileName),
                    Submitter = provider.FormData["submitter"]
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private string path = null;
        private string filename = null;
        public CustomMultipartFormDataStreamProvider(string path, string filename = null)
            : base(path)
        {
            this.path = path;
            this.filename = filename;
        }
        //public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        //{
        //    if (string.IsNullOrEmpty(filename))
        //    {
        //        filename = string.Format("{0}{1}", Guid.NewGuid(), ".file");
        //    }

        //    return filename;
        //}
        //public override Stream GetStream(HttpContent parent, System.Net.Http.Headers.HttpContentHeaders headers)
        //{
        //    FileMode fm = FileMode.Append;
        //    if (!File.Exists(GetLocalFileName(headers)))
        //    {
        //        fm = FileMode.Create;
        //    }
        //    var fs = new FileStream(filename, fm);

        //    return fs;
        //}
        public override Task ExecutePostProcessingAsync()
        {
            return base.ExecutePostProcessingAsync();
        }
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
