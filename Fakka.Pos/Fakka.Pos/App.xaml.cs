using Fakka.Core.Enums;
using Microsoft.AppCenter;
using Prism;
using Prism.Ioc;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Fakka.Core.Managers;
using Fakka.Pos.Routes;
using Fakka.Pos.Resources;
using Fakka.Core.Models;
using Prism.Modularity;
using Fakka.Pos.Modules;
using Fakka.Core.Interfaces;
using Prism.Unity;
using Fakka.Config.Settings;
using Prism.Navigation;
using AutoMapper;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

namespace Fakka.Pos
{
    public partial class App : PrismApplication
    {
        public App() : this(null)
        {

        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
            AppCenter.LogLevel = LogLevel.Verbose;
            AppCenter.Start($"android={AppSettings.AndroidAppKey};ios={AppSettings.IosAppKey};",
                typeof(Analytics), typeof(Crashes) /*, typeof(Push)*/);

        }

        protected async override void OnInitialized()
        {
            InitializeComponent();
            var appLang = CultureInfo.CurrentCulture.Name.Contains("ar") ? Language.Ksa : AppSettings.DefaultLanguage;
            ApplicationManager.Instance.Init(
                new Core.Models.Application(AppSettings.AppCode, AppResources.AppName, appLang,
                    AppSettings.Version, AppRoutes.MainNavigation, AppSettings.IsProduction),
                new Server(AppSettings.BaseServerUrl, AppSettings.BaseServerPort, AppSettings.IsSecure,
                    AppSettings.BaseApiUrl, AppSettings.DefaultCaseStrategy, AppSettings.BaseImageUrl, AppSettings.FileAccessToken),
                new Client(AuthenticationSettings.ClientId, AuthenticationSettings.Scope,
                    AuthenticationSettings.ClientSecret, AuthenticationSettings.GrantType, AuthenticationSettings.ValidUserRoles));

            var sessionManager = Container.Resolve<ISessionManager>();

            bool isAnonymous = await sessionManager.IsAnonymousSession();
            bool hasExpired = await sessionManager.IsSessionExpired();

            INavigationResult tt;
            if (isAnonymous || hasExpired)
                await NavigationService.NavigateAsync($"/{AppRoutes.MainNavigation}/{AppRoutes.Login}");
            else
            {
                await NavigationService.NavigateAsync($"/{AppRoutes.MainNavigation}/{AppRoutes.Main}");
            }


        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(Container);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<NavigationModule>();
            moduleCatalog.AddModule<ContextsModule>();
            moduleCatalog.AddModule<CoreModule>();
            moduleCatalog.AddModule<MapperModule>();
        }


    }
}
