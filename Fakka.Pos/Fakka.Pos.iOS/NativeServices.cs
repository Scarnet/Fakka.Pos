using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using CoreBluetooth;
using Fakka.Core.Extensions;
using Fakka.Core.Interfaces;
using Fakka.Pos.Controls;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Fakka.Pos.iOS
{
    public class NativeServices : INative
    {
        private UIView _nativeView;

        private bool _isInitialized;

        private LoadingIndicatorPage _loadingIndicatorPage = new LoadingIndicatorPage();

        public void CloseApp()
        {
            Thread.CurrentThread.Abort();
        }

        public string GetDeviceId()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }

        public void SetStatusBarColor(Color color)
        {
            MessagingCenter.Send<object, Color>(this, "SetStatusBarColor", color);
        }

        public void HideKeyboard()
        {

            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }

        public void OpenStore()
        {
            //var appId = "1341448775";
            //var nsurl = new NSUrl(
            //    $"itms-apps://itunes.apple.com/eg/app/dryve/id{appId}&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software");
            ////$"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id={appId}&amp;onlyLatestVersion=true&amp;pageNumber=0&amp;sortOrdering=1&amp;type=Purple+Software");

            ////var nsurl = new NSUrl("itms://itunes.apple.com/app/");
            //UIApplication.SharedApplication.OpenUrl(nsurl);
        }

        public void InitLoadingPage()
        {
            // check if the page parameter is available
            if (_loadingIndicatorPage == null)
                return;

            //var currentContext = CrossCurrentActivity.Current.Activity;
            //// build the loading page with native base
            //_loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

            //_loadingIndicatorPage.Layout(new Rectangle(0, 0,
            //    Xamarin.Forms.Application.Current.MainPage.Width,
            //    Xamarin.Forms.Application.Current.MainPage.Height));

            //var renderer = Platform.CreateRendererWithContext(_loadingIndicatorPage, currentContext);

            //_nativeView = renderer?.View;

            // build the loading page with native base
            _loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

            _loadingIndicatorPage.Layout(new Rectangle(0, 0,
                Xamarin.Forms.Application.Current.MainPage.Width,
                Xamarin.Forms.Application.Current.MainPage.Height));
            _loadingIndicatorPage.BackgroundColor = new Color(0, 0, 0, 0.5);
            var renderer = _loadingIndicatorPage.GetOrCreateRenderer();
            
            _nativeView = renderer.NativeView;
            _isInitialized = true;

            //_dialog = new Dialog(currentContext);
            //_dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            //_dialog.SetCancelable(false);
            //_dialog.SetContentView(_nativeView);
            //Window window = _dialog.Window;
            //window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            //window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds | WindowManagerFlags.TranslucentStatus |
            //                WindowManagerFlags.DimBehind);
            //window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            //_isInitialized = true;

        }

        public void ShowLoading(string message = null)
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(); // set the loading 

            // set the indicator message if found
            _loadingIndicatorPage.SetIndicatorTitle(message);

            // showing the native loading page
            UIApplication.SharedApplication.KeyWindow.AddSubview(_nativeView);
        }

        public void HideLoading()
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(); // set the loading page

            // reset the indicator message if found
            _loadingIndicatorPage.SetIndicatorTitle();

            // Hide the page
            _nativeView.RemoveFromSuperview();
        }

        public void EnableBluetooth()
        {
            // Is bluetooth enabled?
            var bluetoothManager = new CoreBluetooth.CBCentralManager();
            if (bluetoothManager.State == CBCentralManagerState.PoweredOff)
            {
                // Does not go directly to bluetooth on every OS version though, but opens the Settings on most
               UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
            }

        }

        public void DisableBluetooth()
        {
            var bluetoothManager = new CoreBluetooth.CBCentralManager();
            if (bluetoothManager.State == CBCentralManagerState.PoweredOn)
            {
                // Does not go directly to bluetooth on every OS version though, but opens the Settings on most
                UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=Bluetooth"));
            }
        }
    }

    internal static class PlatformExtension
    {
        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = Platform.GetRenderer(bindable);
            if (renderer == null)
            {
                renderer = Platform.CreateRenderer(bindable);
                Platform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }
    }
}
