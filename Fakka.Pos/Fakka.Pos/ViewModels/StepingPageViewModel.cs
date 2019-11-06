using AutoMapper;
using Fakka.Core.PageViewModels;
using Fakka.Pos.Routes;
using Prism.Ioc;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fakka.Pos.ViewModels
{
    public class StepingPageViewModel : BasePageViewModel
    {
        public StepingPageViewModel(IContainerProvider container, INavigationService navigationService, IMapper mapper) : base(container, navigationService, mapper)
        {
        }

        public async override Task OnNavigation(INavigationParameters parameters, NavigationMode navigationMode)
        {
        }

        public async override Task OnPageAppearing()
        {
            await base.OnPageAppearing();
            await Task.Delay(1000);
            await NavigationService.NavigateAsync($"/{AppRoutes.MainNavigation}/{AppRoutes.Main}");
        }
       
    }
}
