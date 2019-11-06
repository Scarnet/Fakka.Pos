using Fakka.Core.Enums;
using Fakka.Core.Extensions;

namespace Fakka.Core.Models
{
    public class Application
    {
        /// <summary>
        /// Object used in ApplicationManager when initializing the app
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <param name="version"></param>
        /// <param name="authenticationRoute"></param>
        /// <param name="isProduction"></param>
        public Application(string code, string name, Language language, string version, string authenticationRoute, bool? isProduction = default(bool?))
        {
            Code = code;
            Name = name;
            IsProduction = isProduction;
            Language = language;
            IsRtl = (language.ToDescriptionString().Contains("ar"));
            Version = version;
            AuthenticationRoute = authenticationRoute;
        }


        public string Code { get; }
        public string Name { get; }
        public bool? IsProduction { get; }
        public Language Language { get; }
        public bool IsRtl { get; set; }
        public string Version { get; }
        public string AuthenticationRoute { get; }
    }
}