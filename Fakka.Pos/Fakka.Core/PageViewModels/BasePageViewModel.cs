using System;
using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;
using Fakka.Core.Actions;
using Fakka.Core.Enums;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;
using Fakka.Core.Utilities;
using Prism;
using Prism.Commands;
using Prism.Ioc;
using Fakka.Core.Models.Cache;
using AutoMapper;
using Acr.UserDialogs;

namespace Fakka.Core.PageViewModels
{
    public abstract class BasePageViewModel : BindableBase, INavigationAware, IDestructible, IActiveAware
    {
        private string _title;
        private bool _isBusy;
        private bool _isActive;
        private int _rtl;
        private string _busyMessage;
        
        private NavigationParameters _navParams;
        public readonly INavigationService NavigationService;
        public readonly ISessionManager SessionManager;
        public readonly IView View;
        public readonly IPopupDialog Dialog;
        protected readonly IDataContext DataContext;
        protected readonly IMapper Mapper;
        public readonly IContainerProvider Container;
        public readonly INative NativeService;
        public readonly ICameraHandler CameraHandler;
        public Action<Exception> HandleExceptionOccured { get; set; }

        public event EventHandler IsActiveChanged;

        public BasePageViewModel(IContainerProvider container, INavigationService navigationService, IMapper mapper)
        {
            Container = container;
            Mapper = mapper;

            NavigationService = navigationService; //Container.Resolve<INavigationService>();
            View = Container.Resolve<IView>();
            //Dialog = Container.Resolve<IPopupDialog>();
            SessionManager = Container.Resolve<ISessionManager>();
            DataContext = Container.Resolve<IDataContext>();
            NativeService = Container.Resolve<INative>();
          //  CameraHandler = Container.Resolve<ICameraHandler>();
            Rtl = ApplicationManager.Instance.GetApplicationInfo().IsRtl
                ? (int)FlowDirectionEnum.RightToLeft
                : (int)FlowDirectionEnum.LeftToRight;

        }

        public bool IsActive
        {
            get => _isActive;
            set => base.SetProperty(ref _isActive, value, (Action)(async () =>
            {
                if (value)
                {
                    await OnAppearing();
                }
                else
                {
                    await OnDisappearing();
                }
            }));
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string BusyMessage
        {
            get => _busyMessage;
            set => SetProperty(ref _busyMessage, value);
        }

        public int Rtl
        {
            get => _rtl;
            set => SetProperty(ref _rtl, value);
        }

        public async virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                //SelectedTab = parameters.GetValue<string>("selectedTab");
                await OnNavigation(parameters, parameters.GetNavigationMode());

            }
            catch (Exception ex)
            {
                HideLoading();
                ExceptionsHandler.Handle(ex, this);
            }
        }

        public abstract Task OnNavigation(INavigationParameters parameters, NavigationMode navigationMode);

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void Destroy()
        {
        }

        public void HideLoading()
        {
            IsBusy = false;
            BusyMessage = null;
            UserDialogs.Instance.HideLoading();
        }

        public void ShowLoading(string message = "")
        {
            IsBusy = true;
            BusyMessage = message;
            UserDialogs.Instance.ShowLoading(message);
        }

        public DelegateCommand GoBack => new BaseCommandHandler(this, async () =>
        {
            await NavigationService.GoBackAsync();
        });

        private async Task OnAppearing()
        {
            try
            {
                await OnPageAppearing();
            }
            catch (Exception ex)
            {
                HideLoading();
                ExceptionsHandler.Handle(ex, this);
            }
        }
        
        private async Task OnDisappearing()
        {
            try
            {
                await OnPageDisappearing();
            }
            catch (Exception ex)
            {
                HideLoading();
                ExceptionsHandler.Handle(ex, this);
            }

        }

        public virtual Task OnPageAppearing()
        {
            return Task.FromResult(0);
        }

        public virtual Task OnPageDisappearing()
        {
            return Task.FromResult(0);
        }

        /// <summary>
        /// A generic method to download media bytes to local storage to improve performance
        /// </summary>
        /// <param name="url">Download URL</param>
        /// <param name="mediaId">The id you wish to use for the local  storage, make sure it's contextually unique</param>
        /// <returns></returns>
        protected async virtual Task<byte[]> CacheMedia(string url, string mediaId)
        {
            var sqliteDataService = Container.Resolve<IDataService>();
            var sqliteStorageManager = Container.Resolve<IStorageManager>();

            var image = await sqliteStorageManager.GetItemAsync<CachedData>(mediaId);

            if (image == null || image.ExpirationDate <= DateTime.Now)
            {
                byte[] data = (await sqliteDataService.Download(url)).Data;

                if (data == null || data.Length <= 2)
                    return new byte[0];

                var cachedMedia = new CachedData
                {
                    Data = data,
                    ExpirationDate = DateTime.Now.AddDays(7),
                    OfflineId = mediaId
                };
                await sqliteStorageManager.SetItemAsync(cachedMedia);
                return data;
            }

            return image.Data;
        }


    }
}
