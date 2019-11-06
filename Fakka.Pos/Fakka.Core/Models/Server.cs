using Fakka.Core.Enums;

namespace Fakka.Core.Models
{
    /// <summary>
    /// Object used in ApplicationManager
    /// </summary>
    public class Server
    {
        public Server(string ip, int port, bool isSecure, string baseApiUrl, CaseStrategy defaultCaseStrategy,
            string baseImageUrl, string fileAccessToken)
        {
            Ip = ip;
            Port = port;
            IsSecure = isSecure;
            Protocol = IsSecure ? "https" : "http";
            BaseApiUrl = baseApiUrl;
            BaseServerUrl = $"{Protocol}://{Ip}:{Port}";
            DefaultCaseStrategy = defaultCaseStrategy;
            BaseImageUrl = baseImageUrl;
            FileAccessToken = fileAccessToken;

        }

        private string Protocol { get; }
        private string Ip { get; }
        private int Port { get; }
        private bool IsSecure { get; }
        public string BaseApiUrl { get; }
        public string BaseServerUrl { get; }
        public string BaseImageUrl { get; }
        public string FileAccessToken { get; }
        public CaseStrategy DefaultCaseStrategy { get; }
    }
}