using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RestSharp
{
    class Program
    {
        private const string BING = "http://www.bing.com";
        private const string IMG_URL = "http://www.bing.com/HPImageArchive" +
                                       ".aspx?format=xml&idx=0&n={0}&mkt={1}";
        private static string[] Markets = new string[] { "en-US", "zh-CN", 
                                      "ja-JP", "en-AU", "en-UK", 
                                      "de-DE", "en-NZ", "en-CA" };
        private const int NUMBER_OF_IMAGES = 1;
        static void Main(string[] args)
        {
            var client = new RestClient(BING);
            var request = new RestRequest("HPImageArchive.aspx??format=xml&idx=0&n={number}&mkt={local}", Method.GET);

            request.AddUrlSegment("number", NUMBER_OF_IMAGES.ToString());
            request.AddUrlSegment("local", Markets[0]);
            
            var response = client.Execute(request);

            var content = response.Content;


        }
    }
}
