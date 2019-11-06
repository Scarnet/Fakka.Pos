using System.Threading.Tasks;
using AutoMapper;
using Fakka.Core.PageViewModels;
using Prism.Ioc;
using Prism.Navigation;

namespace Fakka.Pos.ViewModels
{
    public class MainNavigationPageViewModel : BasePageViewModel
    {
        public MainNavigationPageViewModel(IContainerProvider container, INavigationService navigationService, IMapper mapper) : base(container, navigationService, mapper)
        {
        }

        public override Task OnNavigation(INavigationParameters parameters, NavigationMode navigationMode)
        {
            return Task.FromResult(0);
        }
    }
}
