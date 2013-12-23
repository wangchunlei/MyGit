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
