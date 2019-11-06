using Fakka.Core.Enums;

namespace Fakka.Config.Settings
{

    public static class AppSettings
    {
        // Modules Configurtion

        public const bool IsProduction = false;
        public const TerminalCode Terminal = TerminalCode.Android;

        public const string StgVersion = Terminal == TerminalCode.Android ? "1.0.27" : "1.0.9";
        public const string ProdVersion = Terminal == TerminalCode.Android ? "1.0.8" : "1.0.4"; //android version code = 5
        public const string AppCode = "Fakka";
        public const bool IsSecure = true; // IsProduction
        public const string BaseServerUrl = IsProduction ? "fakkaapi.azurewebsites.net" : "fakkaapitest.azurewebsites.net"; //"192.168.1.28";
        public const string PaymentUrl = IsProduction ? "fakkaui.azurewebsites.net" : "fakkauitest.azurewebsites.net";

        //"fakkaegy.eastus.cloudapp.azure.com"
        public const int BaseServerPort = 443; //51148;  //2000;
        public const string BaseApiUrl = "api";

        public const string Version = IsProduction ? ProdVersion : StgVersion;
        public const CaseStrategy DefaultCaseStrategy = CaseStrategy.CamelCase;
        public const string AndroidAppKey = "74231dbc-0423-4352-a1e2-55c32be5a0f0";
        public const string IosAppKey = "e4a7c5e6-7f44-476f-a114-b0bbd707bdcd";
        public const Language DefaultLanguage = Language.Ksa;
        public const CultureLocale CurrentCultureInfo = CultureLocale.ArKsa;

        public const string BaseImageUrl = IsProduction
            ? "https://fakkacdn.azureedge.net/fakka-blob-share"
            : "https://fakkacdn.azureedge.net/fakka-blob-share-test";

        public const string FileAccessToken = "?version=";
        // IsProduction ?
        //    "?sv=2018-03-28&ss=bfqt&srt=sco&sp=rwdlacup&se=2025-01-30T19:21:03Z&st=2019-01-30T11:21:03Z&spr=https,http&sig=UXMvWqLfC8qfVaf0IqSHa5qLcFu%2FxTdwpW6Dr5wwpRM%3D": "?version=";


    }
}
