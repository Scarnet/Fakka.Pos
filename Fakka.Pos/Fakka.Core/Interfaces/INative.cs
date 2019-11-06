using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Fakka.Core.Interfaces
{
    public interface INative
    {
        void CloseApp();
        void HideKeyboard();
        void OpenStore();
        string GetDeviceId();
        void InitLoadingPage();
        void ShowLoading(string message = null);
        void HideLoading();
        void SetStatusBarColor(Color color);
        void EnableBluetooth();
        void DisableBluetooth();
    }
}
