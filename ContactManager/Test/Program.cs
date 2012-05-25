using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Http;
using System.Runtime.Serialization.Json;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ListAllContacts();
        }
        static void ListAllContacts()
        {
            using (HttpClient client = new HttpClient("http://localhost:9000/api/contacts/"))
            {
                //Get
                Console.WriteLine("Get-Method Test...");
                using (var request = new HttpRequestMessage("GET", "Get/1/name"))
                {
                    request.Headers.Accept.Add("application/json");

                    using (var response = client.Send(request))
                    {
                        var status = response.StatusCode;
                        Console.WriteLine("Status Code: {0}", status);
                        var result = response.Content.ReadAsString();
                        Console.WriteLine("Content: {0}", result);
                    }
                }
                //Post
                Console.WriteLine("Post-Method Test...");
                HttpContent content = HttpContentExtensions
                    .CreateJsonDataContract(new List<Contact>
                                                {
                                                    new Contact{Name = "王春雷"},
                                                    new Contact{ContactId = 1,Name = "老张"}
                                                });
                content.LoadIntoBuffer();

                using (var response = client.Put("Filter/1/王春雷", content))
                {
                    response.EnsureStatusIsSuccessful();
                    response.Content.LoadIntoBuffer();

                    var result = response.Content.ReadAsJsonDataContract<List<Contact>>();
                    //var serializer = new JavaScriptSerializer();
                    //var con=serializer.Deserialize<List<Contact>>(result);
                    result.ForEach(r => Console.WriteLine(r.ToString()));
                }
            }
            Console.ReadKey();
        }
    }

    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("ID: {0};          Name: {1}", ContactId, Name);
        }
    }
}
