using System;
using System.Collections.Generic;
using System.Text;
using Fakka.Pos.Routes;
using Fakka.Pos.ViewModels;
using Fakka.Pos.Views;
using Prism.Ioc;
using Xamarin.Forms;

namespace Fakka.Pos.Modules
{
    public class NavigationModule : BaseModule
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainNavigationPage, MainNavigationPageViewModel>(AppRoutes.MainNavigation);
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>(AppRoutes.Login);
            containerRegistry.RegisterForNavigation<LandingPage, LandingPageViewModel>(AppRoutes.Landing);
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>(AppRoutes.Main);
            containerRegistry.RegisterForNavigation<StepingPage, StepingPageViewModel>(AppRoutes.Steping);
        }
    }
}
