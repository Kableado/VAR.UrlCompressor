using System;
using System.Text;

namespace VAR.UrlCompressor
{
    public class UrlCompressor
    {
        public static string Compress(string url)
        {
            // Replace protocol indicator
            if (url.StartsWith("https://") || url.StartsWith("HTTPS://"))
            {
                url = string.Format("${0}", url.Substring("https://".Length));
            }
            if (url.StartsWith("http://") || url.StartsWith("HTTP://"))
            {
                url = string.Format("#{0}", url.Substring("http://".Length));
            }

            byte[] urlBytes = Encoding.ASCII.GetBytes(url);
            return Base62.Encode(urlBytes);
        }

        public static string Decompress(string compressedUrl)
        {
            byte[] urlBytes = Base62.Decode(compressedUrl);
            string url = Encoding.ASCII.GetString(urlBytes);

            // Restore protocol indicator
            if (url.StartsWith("#"))
            {
                url = string.Format("http://{0}", url.Substring(1));
            }
            if (url.StartsWith("$"))
            {
                url = string.Format("https://{0}", url.Substring(1));
            }

            return url;
        }
    }
}
