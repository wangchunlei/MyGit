using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace Host.Controllers
{
    public class UploadController : ApiController
    {
        [System.Web.Http.HttpGet]
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
                            if (startlength<length)
                            {
                                actualLength = startlength;
                            }
                        }
                    }
                });

            return response;
        }
        public async Task<HttpResponseMessage> UploadFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string fileSaveLocation = System.Web.HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
            var files = new List<string>();
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (MultipartFileData file in provider.FileData)
                {
                    files.Add(Path.GetFileName(file.LocalFileName));
                }

                return Request.CreateResponse(HttpStatusCode.OK, files);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }

    public class CustomMultipartFormDataStreamProvider : MultipartFileStreamProvider
    {
        private string path = null;
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        {
            this.path = path;
        }
        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
        }
        public override Stream GetStream(HttpContent parent, System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var filename = Path.Combine(path, GetLocalFileName(headers));
            FileMode fm = FileMode.Append;
            if (!File.Exists(filename))
            {
                fm = FileMode.Create;
            }
            var fs = new FileStream(filename, fm);


            return fs;
        }
        public override Task ExecutePostProcessingAsync()
        {
            return base.ExecutePostProcessingAsync();
        }
    }
}
