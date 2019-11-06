using AutoMapper;
using Fakka.Core.Actions;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;
using Fakka.Core.Models;
using Fakka.Core.PageViewModels;
using Fakka.Pos.Routes;
using Prism.Commands;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Pos.ViewModels
{
    public class LandingPageViewModel : BasePageViewModel
    {
        public DelegateCommand SimplePosCommand => new BaseCommandHandler(this, async () =>
        {
            var tt = await NavigationService.NavigateAsync($"{AppRoutes.Main}");
        }, () => false);
        public DelegateCommand FullPosCommand => new BaseCommandHandler(this, async () =>
        {
            var tt = await NavigationService.NavigateAsync($"{AppRoutes.Main}");
        });

        public DelegateCommand LogoutCommand => new BaseCommandHandler(this, async () =>
        {
            ShowLoading();
            Container.Resolve<IStorageManager>().DeleteAll();
            SessionManager.EndSession();
            StateManager.Instance.DeleteAll();
            await NavigationService.NavigateAsync($"/{AppRoutes.MainNavigation}/{AppRoutes.Login}");
            HideLoading();
        });

        public LandingPageViewModel(IContainerProvider container, INavigationService navigationService, IMapper mapper) : base(container, navigationService, mapper)
        {
        }


        public async override Task OnNavigation(INavigationParameters parameters, NavigationMode navigationMode)
        {

        }
    }
}
