using System;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using Fakka.Core.Actions;
using Fakka.Core.PageViewModels;
using Prism.Ioc;
using Fakka.Core.Validations;
using Fakka.Pos.Validations.Commands;
using Fakka.Pos.Resources;
using Fakka.Core.Business.Interfaces;
using Fakka.Pos.Routes;
using AutoMapper;

namespace Fakka.Pos.ViewModels
{
    public class LoginPageViewModel : BasePageViewModel
    {
        public LoginPageViewModel(IContainerProvider container, INavigationService navigationService, IMapper mapper) : base(container,
            navigationService, mapper)
        {
        }

        #region Private Properties

        private ValidatableObject<string> _username = new ValidatableObject<string>();
        private ValidatableObject<string> _password = new ValidatableObject<string>();

        #endregion

        #region Public Properties

        public ValidatableObject<string> Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public ValidatableObject<string> Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        #region Commands

        public DelegateCommand UsernameValidationCommand =>
            new RequiredValidationCommand<string>(Username, UiResources.Username);

        public DelegateCommand PasswordValidationCommand =>
            new RequiredValidationCommand<string>(Password, UiResources.Password);

        public DelegateCommand LoginCommand => new BaseCommandHandler(this, async () =>
        {
            ShowLoading();
            var response = await DataContext.Get<IAuthenticationContext>().Login(Username.Value, Password.Value);
            var newSession = await SessionManager.GetCurrentSession();
            await DataContext.Get<IPosUserContext>().GetProfile(newSession.Uid);
            HideLoading();
            var tt = await NavigationService.NavigateAsync(
                 $"/{AppRoutes.MainNavigation}/{AppRoutes.Main}");

        }, () =>
        {
            var isValidUsername = Username.Validate();
            var isValidPassword = Password.Validate();
            return isValidUsername && isValidPassword;
        });


        #endregion

        public override Task OnNavigation(INavigationParameters parameters, NavigationMode navigationMode)
        {
            //TODO: remove credentials
            //#if DEBUG

            //            Username.Value = "parent101";
            //            Password.Value = "test1234";
            //#endif
            return Task.FromResult(0);

        }
    }
}
