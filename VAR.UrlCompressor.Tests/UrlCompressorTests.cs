using System.Collections.Generic;
using Xunit;

namespace VAR.UrlCompressor.Tests
{
    public class UrlCompressorTests
    {
        #region Helpers

        private Dictionary<string, string> GenerateHostsConversions()
        {
            Dictionary<string, string> hostConversions = new Dictionary<string, string> {
                { "com", "C" },
                { "net", "N" },
                { "org", "O" },
                { "localhost", "L" },
                { "azurewebsites", "A" },
                { "unifikas", "U" },
                { "cyc", "Y" },
                { "es", "E" },
                { "google", "G" },
                { "facebook", "F" },
                { "twitter", "T" },
                { "github", "GH" },
                { "varstudio", "V" },
            };
            return hostConversions;
        }

        #endregion Helpers

        #region Compress

        [Fact]
        public void Compress__Http_Google_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://google.com";
            string compressedUrl = "zBUW9UxS1";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Google_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://google.com";
            string compressedUrl = "oMyuFVR41";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Http_Facebook_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://facebook.com";
            string compressedUrl = "EAyWnGuy0";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Facebook_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://facebook.com";
            string compressedUrl = "2AyutHOa0";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Twitter_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://twitter.com";
            string compressedUrl = "bpzuhuDB1";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Twitter_Com_Kableado()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://twitter.com/Kableado";
            string compressedUrl = "zYfD7gXd8ApW9HZtR4";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Github_Com_Kableado()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://github.com/Kableado";
            string compressedUrl = "JzJv5CTnllbrEVp1BhF";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Http_Localhost_30000_0()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://localhost:30000|0";
            string compressedUrl = "0fT4k50uE3WwvqMB42";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Http_Localhost_30000_1()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://localhost:30000|0";
            string compressedUrl = "0fT4k50uE3WwvqMB42";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Unifikas_Azurewebsites_Net_1()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://unifikas.azurewebsites.net|1";
            string compressedUrl = "7R15qGtuLPGD";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Unifikas_Azurewebsites_Net_2()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://unifikas.azurewebsites.net|2";
            string compressedUrl = "7B54q0pvLPKC";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        [Fact]
        public void Compress__Https_Varstudio_Net()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://varstudio.net";
            string compressedUrl = "5B1u05oK";

            string result = UrlCompressor.Compress(url, hostConversions);

            Assert.Equal(compressedUrl, result);
        }

        #endregion Compress

        #region Decompress

        [Fact]
        public void Decompress__Http_Google_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://google.com";
            string compressedUrl = "zBUW9UxS1";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Google_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://google.com";
            string compressedUrl = "oMyuFVR41";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Http_Facebook_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://facebook.com";
            string compressedUrl = "EAyWnGuy0";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Facebook_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://facebook.com";
            string compressedUrl = "2AyutHOa0";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Twitter_Com()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://twitter.com";
            string compressedUrl = "bpzuhuDB1";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Twitter_Com_Kableado()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://twitter.com/Kableado";
            string compressedUrl = "zYfD7gXd8ApW9HZtR4";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Github_Com_Kableado()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://github.com/Kableado";
            string compressedUrl = "JzJv5CTnllbrEVp1BhF";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Http_Localhost_30000_0()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://localhost:30000|0";
            string compressedUrl = "0fT4k50uE3WwvqMB42";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Http_Localhost_30000_1()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "http://localhost:30000|0";
            string compressedUrl = "0fT4k50uE3WwvqMB42";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Unifikas_Azurewebsites_Net_1()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://unifikas.azurewebsites.net|1";
            string compressedUrl = "7R15qGtuLPGD";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Unifikas_Azurewebsites_Net_2()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://unifikas.azurewebsites.net|2";
            string compressedUrl = "7B54q0pvLPKC";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        [Fact]
        public void Decompress__Https_Varstudio_Net()
        {
            Dictionary<string, string> hostConversions = GenerateHostsConversions();
            string url = "https://varstudio.net";
            string compressedUrl = "5B1u05oK";

            string result = UrlCompressor.Decompress(compressedUrl, hostConversions);

            Assert.Equal(url, result);
        }

        #endregion Decompress
    }
}