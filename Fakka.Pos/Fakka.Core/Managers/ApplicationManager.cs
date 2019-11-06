using System;
using Fakka.Core.Enums;
using Fakka.Core.Models;
using Fakka.Core.Utilities;
using Application = Fakka.Core.Models.Application;

namespace Fakka.Core.Managers
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        private Application _application;
        private Server _server;
        private Terminal _terminal;
        private Client _client;

        #region public Methods
        public void Init(Application application, Server server, Client client)
        {
            _application = application;
            _server = server;
            _client = client;

            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case "iOS":
                    _terminal = new Terminal(TerminalCode.Ios);
                    break;
                case "Android":
                    _terminal = new Terminal(TerminalCode.Android);
                    break;
                default:
                    throw new NotSupportedException();
            }

        }
        public void Init(Application application, Server server, Terminal terminal, Client client)
        {
            _application = application;
            _server = server;
            _terminal = terminal;
            _client = client;
        }
        public Application GetApplicationInfo()
        {
            return _application;
        }
        public Server GetServerInfo()
        {
            return _server;
        }
        public Terminal GetTerminalInfo()
        {
            return _terminal;
        }
        public Client GetClientInfo()
        {
            return _client;
        }
        #endregion
    }
}