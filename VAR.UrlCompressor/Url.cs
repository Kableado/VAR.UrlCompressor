using System;

namespace VAR.UrlCompressor
{
    class Url
    {
        public string Protocol { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Path { get; set; }
        
        private Url() { }

        private enum ParseStatus
        {
            ParsingProtocol,
            ParsingHost,
            ParsingPort,
            ParsingPath,
        };

        private ParseStatus _status = ParseStatus.ParsingProtocol;
        private int _i0 = 0;
        private int _i = 0;

        private void ResetParser()
        {
            _status = ParseStatus.ParsingProtocol;
            _i0 = 0;
            _i = 0;
        }

        private void ParseUrl(string url)
        {
            while (_i < url.Length)
            {
                switch (_status)
                {
                    case ParseStatus.ParsingProtocol:
                        if (url[_i] == ':')
                        {
                            Protocol = url.Substring(_i0, _i - _i0);

                            if (_i + 2 >= url.Length)
                            {
                                throw new Exception(string.Format("Unexpected end of URL, while parsing protocol. \"{0}\"", url));
                            }
                            if (url[_i + 1] != '/' || url[_i + 2] != '/')
                            {
                                throw new Exception(string.Format("Unexpected end of URL, while parsing protocol. \"{0}\"", url));
                            }

                            _status = ParseStatus.ParsingHost;
                            _i0 = _i + 3;
                            _i = _i0;
                        }
                        break;
                    case ParseStatus.ParsingHost:
                        if (char.IsLetterOrDigit(url[_i]) == false && url[_i] != '.')
                        {
                            Host = url.Substring(_i0, _i - _i0);
                            _i0 = _i;

                            if (url[_i] == ':')
                            {
                                _status = ParseStatus.ParsingPort;
                            }
                            else
                            {
                                _status = ParseStatus.ParsingPath;
                            }
                        }
                        break;
                    case ParseStatus.ParsingPort:
                        if (char.IsDigit(url[_i]) == false)
                        {
                            Port = url.Substring(_i0, _i - _i0);
                            _i0 = _i;
                            _status = ParseStatus.ParsingPath;
                        }
                        break;
                    case ParseStatus.ParsingPath:
                        _i = url.Length - 1;
                        break;
                }
                _i++;
            }
            switch (_status)
            {
                case ParseStatus.ParsingProtocol:
                    Protocol = url.Substring(_i0, _i - _i0);
                    break;
                case ParseStatus.ParsingHost:
                    Host = url.Substring(_i0, _i - _i0);
                    break;
                case ParseStatus.ParsingPort:
                    Port = url.Substring(_i0, _i - _i0);
                    break;
                case ParseStatus.ParsingPath:
                    Path = url.Substring(_i0, _i - _i0);
                    break;
            }
        }


        public static Url CreateFromString(string url)
        {
            var newUrl = new Url();
            newUrl.ParseUrl(url);
            return newUrl;
        }

        public static Url CreateFromShortString(string url)
        {
            var newUrl = new Url()
            {
                Protocol = url[0].ToString(),
                _status = ParseStatus.ParsingHost,
                _i = 1,
                _i0 = 1,
            };
            newUrl.ParseUrl(url);
            return newUrl;
        }

        public override string ToString()
        {
            return string.Format("{0}://{1}{2}{3}", Protocol, Host, Port, Path);
        }

        public string ToShortString()
        {
            return string.Format("{0}{1}{2}{3}", Protocol[0], Host, Port, Path);
        }
    }
}
