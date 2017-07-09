using System;

namespace UrlCompressor.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestUrl("http://google.com");
            TestUrl("https://google.com");
            TestUrl("http://facebook.com");
            TestUrl("https://facebook.com");
            TestUrl("https://twitter.com");
            TestUrl("https://twitter.com/Kableado");
            TestUrl("https://github.com/Kableado");
            TestUrl("https://varstudio.net");

            Console.Read();
        }

        private static bool TestUrl(string url)
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("            Url: {0}", url);
            string compressedUrl = VAR.UrlCompressor.UrlCompressor.Compress(url);
            Console.WriteLine("  CompressedUrl: {0}", compressedUrl);
            string decompressedUrl = VAR.UrlCompressor.UrlCompressor.Decompress(compressedUrl);
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
