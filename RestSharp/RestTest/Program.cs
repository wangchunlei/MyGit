using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using RestSharp;

namespace RestTest
{
    class Program
    {
        private const string BING = "http://www.bing.com";
        private const string IMG_URL = "http://www.bing.com/HPImageArchive" +
                                       ".aspx?format=xml&idx=0&n={0}&mkt={1}";
        //private static string[] Markets = new string[] { "en-US", "zh-CN", 
        //                              "ja-JP", "en-AU", "en-UK", 
        //                              "de-DE", "en-NZ", "en-CA" };

        private static string[] Markets = new string[]
            {
                "en-US", "zh-CN",
                "ja-JP", "en-AU"
            };
        private const int NUMBER_OF_IMAGES = 1;

        public const int SPI_SETDESKWALLPAPER = 20;
        public const int SPIF_UPDATEINIFILE = 1;
        public const int SPIF_SENDCHANGE = 2;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int SystemParametersInfo(
          int uAction, int uParam, string lpvParam, int fuWinIni);

        static void Main(string[] args)
        {
            //var client = new RestClient(BING);
            //var request = new RestRequest("HPImageArchive.aspx??format=xml&idx=0&n={number}&mkt={local}", Method.GET);

            //request.AddUrlSegment("number", NUMBER_OF_IMAGES.ToString());
            //request.AddUrlSegment("local", Markets[0]);

            //var response = client.Execute(request);

            //var content = response.Content;

            //var xml = new XmlDocument();
            //xml.LoadXml(content);

            //var item = xml.GetElementsByTagName("url")[0].InnerText;
            //var picUrl = string.Format(BING + "/" + item);


            var fs = BingImages.DownLoadImages();
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, fs[0], SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }
    }
}
