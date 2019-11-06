using System.Threading.Tasks;
using Fakka.Core.Interfaces;
using Fakka.Core.Managers;
using Fakka.Core.Resources;
using Prism.Services;

namespace Fakka.Core.Providers
{
    public class ViewProvider : IView
    {
        private readonly IPageDialogService _pageDialogService;

        public ViewProvider(IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
        }
        public async Task Alert(string title, string details)
        {
           await Alert(title, details, CoreResources.Ok);
        }

        public async Task Alert(string title, string details, string buttonText)
        {
            title = title ?? "خطأ"; //ApplicationManager.Instance.GetApplicationInfo().Name;
            await _pageDialogService.DisplayAlertAsync(title, details, buttonText);
        }

        public async Task<bool> Alert(string title, string details, string buttonText, string cancelText)
        {
            title = title ?? ApplicationManager.Instance.GetApplicationInfo().Name;
            return await _pageDialogService.DisplayAlertAsync(title, details, buttonText, cancelText);
        }


        public async Task<string> DisplayActionSheet(string title, string okTitle, string cancelTitle, params string[] otherButtons) {
            // title = title ?? ApplicationManager.Instance.GetApplicationInfo().Name;
            var result =  await _pageDialogService.DisplayActionSheetAsync(title, okTitle, cancelTitle, otherButtons);
            return result;
        }

    }
}