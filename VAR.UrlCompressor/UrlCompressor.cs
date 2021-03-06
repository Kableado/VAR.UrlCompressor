﻿using System;
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

        private static void XorData(byte[] data, byte xorKey)
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ xorKey);
            }
        }

        private static byte ChecksumCalculate(byte[] data)
        {
            byte checksum = 0;
            foreach(byte b in data)
            {
                checksum = (byte)(checksum ^ b);
            }
            return checksum;
        }

        private static byte[] ChecksumAdd(byte[] data)
        {
            byte[] newData = new byte[data.Length + 1];
            byte checksum = ChecksumCalculate(data);
            XorData(data, checksum);
            Array.Copy(data, 0, newData, 1, data.Length);
            newData[0] = checksum;
            return newData;
        }

        private static byte[] ChecksumCheck(byte[] data)
        {
            byte[] newData = new byte[data.Length - 1];
            Array.Copy(data, 1, newData, 0, data.Length - 1);
            byte oldChecksum = data[0];
            XorData(newData, oldChecksum);
            byte checksum = ChecksumCalculate(newData);
            if (checksum != oldChecksum) { throw new Exception("Checksum mismatch."); }
            return newData;
        }

        public static string Compress(string url, Dictionary<string, string> hostConversions = null)
        {
            InitHuffmanTree();

            Url oUrl = Url.CreateFromString(url);
            
            // "Compress" protocol
            if (oUrl.Protocol == "http" || oUrl.Protocol == null) { oUrl.Protocol = "#"; }
            else if (oUrl.Protocol == "https") { oUrl.Protocol = "$"; }
            else if (oUrl.Protocol == "ftp") { oUrl.Protocol = "F"; }
            else { throw new Exception(string.Format("Unkown protocol \"{0}\"", oUrl.Protocol)); }

            if (hostConversions != null)
            {
                // "Compress" hosts
                string[] urlHostParts = oUrl.Host.Split('.');
                for (int i = 0; i < urlHostParts.Length; i++)
                {
                    foreach (KeyValuePair<string, string> hostConversion in hostConversions)
                    {
                        if (urlHostParts[i] == hostConversion.Key)
                        {
                            urlHostParts[i] = hostConversion.Value;
                            break;
                        }
                    }
                }
                oUrl.Host = string.Join(".", urlHostParts);
            }

            url = oUrl.ToShortString();

            // Reduce entropy
            byte[] urlBytes = Encoding.ASCII.GetBytes(url);
            urlBytes = _huffmanTree.Encode(urlBytes);
            urlBytes = ChecksumAdd(urlBytes);
            return Base62.Encode(urlBytes);
        }

        public static string Decompress(string compressedUrl, Dictionary<string, string> hostConversions = null)
        {
            InitHuffmanTree();

            byte[] urlBytes = Base62.Decode(compressedUrl);
            urlBytes = ChecksumCheck(urlBytes);
            urlBytes = _huffmanTree.Decode(urlBytes);
            string url = Encoding.ASCII.GetString(urlBytes);

            Url oUrl = Url.CreateFromShortString(url);
            
            // "Decompress" protocol
            if (oUrl.Protocol == "#") { oUrl.Protocol = "http"; }
            else if (oUrl.Protocol == "$") { oUrl.Protocol = "https"; }
            else if (oUrl.Protocol == "F") { oUrl.Protocol = "ftp"; }
            else { throw new Exception(string.Format("Unkown protocol \"{0}\"", oUrl.Protocol)); }

            if (hostConversions != null)
            {
                // "Decompress" hosts
                string[] urlHostParts = oUrl.Host.Split('.');
                for (int i = 0; i < urlHostParts.Length; i++)
                {
                    foreach (KeyValuePair<string, string> hostConversion in hostConversions)
                    {
                        if (urlHostParts[i] == hostConversion.Value)
                        {
                            urlHostParts[i] = hostConversion.Key;
                            break;
                        }
                    }
                }
                oUrl.Host = string.Join(".", urlHostParts);
            }
            
            url = oUrl.ToString();

            return url;
        }
    }
}
