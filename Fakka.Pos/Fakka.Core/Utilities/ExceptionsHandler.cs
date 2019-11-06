using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Fakka.Core.Exceptions;
using Fakka.Core.Managers;
using Fakka.Core.PageViewModels;
using Fakka.Core.Resources;
using Microsoft.AppCenter.Crashes;

namespace Fakka.Core.Utilities
{
    public class ExceptionsHandler
    {
        public static async void Handle(Exception ex, BasePageViewModel viewModel)
        {
            var view = viewModel.View;
            var navigationService = viewModel.NavigationService;

            if (ex.GetType() == typeof(BusinessException))
            {

                //if (((BusinessException) ex).ErrorResponse.ErrorCode == (int)ErrorCode.InvalidUsernameOrPassword)
                //{
                //    await view.Alert(null, CoreResources.InvalidUsernameOrPassword);

                //}
                //else
                {
                    // Backend business error that are not handled -> Like error code 1
                    await view.Alert(null, ((BusinessException) ex).ErrorResponse.ErrorMessage);
                }
            }
            else if (ex.GetType() == typeof(AuthorizationException))
            {
                
                //await view.Alert(null, ((AuthorizationException)ex).ErrorResponse.ErrorMessage);

                await navigationService.NavigateAsync(ApplicationManager.Instance.GetApplicationInfo().AuthenticationRoute);
            }
            else if (ex.GetType() == typeof(InternalServerErrorException))
            {
                await view.Alert(null, $"{CoreResources.ServiceUnAvailableOrInternalServiceError} - ({(ex as BaseException)?.ErrorResponse.ErrorCode})");
                // يوجد مشكله بالخدمه ، من فضلك حاول مره اخرى
                //view.Alert(ex.Message, ex.GetBaseException().Message);
            }
            else if (ex.GetType() == typeof(NetworkException))
            {
                await view.Alert(null, $"{CoreResources.ServiceUnAvailableTryAgain} - ({ (ex as BaseException)?.ErrorResponse.ErrorCode})");
                // لا يمكن الاتصال بالخادم، من فضلك حاول مره اخرى بعد التأكد من وحود شبكة اتصال
                //view.Alert(ex.Message, ex.GetBaseException().Message);
            }
            else if (ex.GetType() == typeof(BaseException))
            {
                await view.Alert(null,
                      $"{(ex as BaseException)?.ErrorResponse.ErrorMessage} - ({(ex as BaseException)?.ErrorResponse.ErrorCode})");
            }
            else if (ex.GetType() == typeof(HttpRequestException))
            {
                // When device network is offline

                await view.Alert(null, CoreResources.NoInternetConnectionAvailable);
                // لا يمكن الاتصال بالخادم، من فضلك تأكد من وجود انترنت
                //view.Alert(ex.Message, ex.GetBaseException().Message);
            }
            else if (ex.GetType() == typeof(TaskCanceledException))
            {
                await view.Alert(null, CoreResources.SlowNetworkConnection);

            }
            else
            {
                // Frontend expcetions
                if (ApplicationManager.Instance.GetApplicationInfo().IsProduction != null)
                {
                    var isProduction = ApplicationManager.Instance.GetApplicationInfo().IsProduction;
                    var errorMessage = isProduction != null && (bool) isProduction
                        ? $"{CoreResources.SomethingWentWrong}"
                        : $"{CoreResources.SomethingWentWrong} - {ex.GetBaseException().Message}";
                    await view.Alert(null, errorMessage);
                }
                else
                    await view.Alert(null, CoreResources.SomethingWentWrong);

                //view.Alert(ex.Message, ex.GetBaseException().Message);
            }

            Crashes.TrackError(ex);
            Debug.WriteLine(ex.StackTrace);
            viewModel.HandleExceptionOccured?.Invoke(ex);

        }
    }
}

