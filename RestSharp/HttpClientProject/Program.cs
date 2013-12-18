using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://localhost:2616/");
            var request = new RestRequest("api/upload/UploadFile");
            request.Method = Method.POST;
            request.AddFile("file", writer, "fff", "application/x-www-form-urlencoded");
            var response = client.Execute(request);
            Console.ReadKey();
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
