using System.Collections.Generic;
using System.Text;

namespace VAR.UrlCompressor
{
    public class UrlCompressor
    {
        private static HuffmanTree _huffmanTree = null;
        
        private static void InitHuffmanTree()
        {
            if (_huffmanTree != null) { return; }

            var frequencies = new Dictionary<char, int>
            {
                // English frequencies (percetages*1000)
                { 'a', 8167},
                { 'b', 1492},
                { 'c', 2782},
                { 'd', 4253},
                { 'e', 12702},
                { 'f', 2228},
                { 'g', 2015},
                { 'h', 6094},
                { 'i', 6966},
                { 'j', 153},
                { 'k', 772},
                { 'l', 4025},
                { 'm', 2406},
                { 'n', 6749},
                { 'o', 7507},
                { 'p', 1929},
                { 'q', 95},
                { 'r', 5987},
                { 's', 6327},
                { 't', 9056},
                { 'u', 2758},
                { 'v', 978},
                { 'w', 2360},
                { 'x', 150},
                { 'y', 1974},
                { 'z', 74},
                
                // English frequencies Upper case(percetages*1000)
                { 'A', 8167},
                { 'B', 1492},
                { 'C', 2782},
                { 'D', 4253},
                { 'E', 12702},
                { 'F', 2228},
                { 'G', 2015},
                { 'H', 6094},
                { 'I', 6966},
                { 'J', 153},
                { 'K', 772},
                { 'L', 4025},
                { 'M', 2406},
                { 'N', 6749},
                { 'O', 7507},
                { 'P', 1929},
                { 'Q', 95},
                { 'R', 5987},
                { 'S', 6327},
                { 'T', 9056},
                { 'U', 2758},
                { 'V', 978},
                { 'W', 2360},
                { 'X', 150},
                { 'Y', 1974},
                { 'Z', 74},

                // Numbers, Use a fixed frequency of 1000.
                { '0', 1000},
                { '1', 1000},
                { '2', 1000},
                { '3', 1000},
                { '4', 1000},
                { '5', 1000},
                { '6', 1000},
                { '7', 1000},
                { '8', 1000},
                { '9', 1000},
                
                // Common symbols
                { ' ', 100},
                { '!', 100},
                { '"', 100},
                { '#', 20000}, // NOTE: Exagerate to minimize bitstream of this symbol '#'
                { '$', 20000}, // NOTE: Exagerate to minimize bitstream of this symbol '$'
                { '%', 100},
                { '&', 100},
                { '\'', 20000}, // NOTE: Exagerate to minimize bitstream of this symbol '/'
                { '(', 100},
                { '*', 100},
                { '+', 100},
                { ',', 100},
                { '-', 100},
                { '.', 20000}, // NOTE: Exagerate to minimize bitstream of this symbol '.'
                { '/', 100},
                { ':', 100},
                { ';', 100},
                { '<', 100},
                { '=', 100},
                { '>', 100},
                { '?', 100},
                { '@', 100},
                { '[', 100},
                { '\\', 100},
                { ']', 100},
                { '^', 100},
                { '_', 100},
                { '`', 100},
                { '{', 100},
                { '|', 100},
                { '}', 100},
                { '~', 100},
            };
            _huffmanTree = new HuffmanTree(frequencies);
        }

        public static string Compress(string url)
        {
            InitHuffmanTree();

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

            byte[] compressedUrlBytes = _huffmanTree.Encode(urlBytes);

            return Base62.Encode(compressedUrlBytes);
        }

        public static string Decompress(string compressedUrl)
        {
            InitHuffmanTree();

            byte[] urlBytes = Base62.Decode(compressedUrl);

            byte[] decompressedUrlBytes = _huffmanTree.Decode(urlBytes);

            string url = Encoding.ASCII.GetString(decompressedUrlBytes);

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
