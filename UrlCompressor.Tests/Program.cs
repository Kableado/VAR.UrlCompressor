using System;
using System.Collections.Generic;

namespace UrlCompressor.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> _hostConversions = new Dictionary<string, string> {
                { "com", "C" },
                { "net", "N" },
                { "org", "O" },
            };
            
            TestUrl("http://google.com", _hostConversions);
            TestUrl("https://google.com", _hostConversions);
            TestUrl("http://facebook.com", _hostConversions);
            TestUrl("https://facebook.com", _hostConversions);
            TestUrl("https://twitter.com", _hostConversions);
            TestUrl("https://twitter.com/Kableado", _hostConversions);
            TestUrl("https://github.com/Kableado", _hostConversions);
            TestUrl("https://varstudio.net", _hostConversions);

            Console.Read();
        }

        private static bool TestUrl(string url, Dictionary<string, string> _hostConversions)
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("            Url: {0}", url);
            string compressedUrl = VAR.UrlCompressor.UrlCompressor.Compress(url, _hostConversions);
            Console.WriteLine("  CompressedUrl: {0}", compressedUrl);
            string decompressedUrl = VAR.UrlCompressor.UrlCompressor.Decompress(compressedUrl, _hostConversions);
            Console.WriteLine("DecompressedUrl: {0}", decompressedUrl);
            if(url!= decompressedUrl)
            {
                Console.WriteLine("!!!!! Failed !!!!!");
                return false;
            }
            return true;
        }
    }
}
