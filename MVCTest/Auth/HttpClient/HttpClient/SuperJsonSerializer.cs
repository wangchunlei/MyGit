using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using RestSharp.Deserializers;

namespace HttpClient
{
    public class SuperJsonDeserializer : IDeserializer
    {
        public T Deserialize<T>(IRestResponse response)
        {
            throw new NotImplementedException();
        }

        public string RootElement { get; set; }
        public string Namespace { get; set; }
        public string DateFormat { get; set; }
    }
}
