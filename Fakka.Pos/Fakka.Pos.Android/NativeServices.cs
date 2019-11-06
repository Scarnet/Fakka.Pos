using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.Provider;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Fakka.Core.Extensions;
using Fakka.Core.Interfaces;
using Fakka.Pos.Controls;
using Plugin.BLE;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Fakka.Pos.Droid
{
    public class NativeServices : INative
    {
        private Android.Views.View _nativeView;

        private Dialog _dialog;

        private bool _isInitialized;

        private LoadingIndicatorPage _loadingIndicatorPage = new LoadingIndicatorPage();

        private BluetoothManager _btManager;
        private LocationManager _gpsManager;

        public NativeServices()
        {
            _btManager = (BluetoothManager)Android.App.Application.Context.GetSystemService(Android.Content.Context
                .BluetoothService);
            _gpsManager = (LocationManager)Android.App.Application.Context.GetSystemService(Android.Content.Context
                .LocationService);

        }

        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        public string GetDeviceId()
        {
            return Android.Provider.Settings.Secure.GetString(CrossCurrentActivity.Current.Activity.ContentResolver,
                Android.Provider.Settings.Secure.AndroidId);
        }

        public void SetStatusBarColor(Color color)
        {
            MessagingCenter.Send<object, Color>(this, "SetStatusBarColor", color);
        }

        public void HideKeyboard()
        {

            var context = CrossCurrentActivity.Current.Activity;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }

        public void OpenStore()
        {
            {
                string packageName = Android.App.Application.Context.PackageName;
                Intent googleAppStoreIntent;
                try
                {
                    googleAppStoreIntent = new Intent(Intent.ActionView,
                        Android.Net.Uri.Parse("market://details?id=" + packageName));

                }
                catch (ActivityNotFoundException anfe)
                {
                    googleAppStoreIntent = new Intent(Intent.ActionView,
                        Android.Net.Uri.Parse("http://play.google.com/store/apps/details?id=" + packageName));

                }
                googleAppStoreIntent.AddFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(googleAppStoreIntent);
            }
        }

        public void InitLoadingPage()
        {
            // check if the page parameter is available
            if (_loadingIndicatorPage == null)
                return;

            var currentContext = CrossCurrentActivity.Current.Activity;
            // build the loading page with native base
            _loadingIndicatorPage.Parent = Xamarin.Forms.Application.Current.MainPage;

            _loadingIndicatorPage.Layout(new Rectangle(0, 0,
                Xamarin.Forms.Application.Current.MainPage.Width,
                Xamarin.Forms.Application.Current.MainPage.Height));

            var renderer = Platform.CreateRendererWithContext(_loadingIndicatorPage, currentContext);

            _nativeView = renderer.View;

            _dialog = new Dialog(currentContext);
            _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
            _dialog.SetCancelable(false);
            _dialog.SetContentView(_nativeView);
            Window window = _dialog.Window;
            window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds | WindowManagerFlags.TranslucentStatus |
                            WindowManagerFlags.DimBehind);
            window.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.Transparent));
            _isInitialized = true;

        }

        public void ShowLoading(string message = null)
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(); // set the loading 

            // set the indicator message if found
            _loadingIndicatorPage.SetIndicatorTitle(message);

            // showing the native loading page
            _dialog.Show();
        }

        public void HideLoading()
        {
            // check if the user has set the page or not
            if (!_isInitialized)
                InitLoadingPage(); // set the loading page

            // reset the indicator message if found
            _loadingIndicatorPage.SetIndicatorTitle();

            // Hide the page
            _dialog.Hide();
        }



        public void EnableBluetooth()
        {
            try
            {
                var currentContext = Android.App.Application.Context;//CrossCurrentActivity.Current.Activity;
                if (_gpsManager.IsProviderEnabled(LocationManager.GpsProvider) == false)
                {
                    Intent gpsSettingIntent = new Intent(Android.Provider.Settings.ActionLocationSourceSettings);
                    currentContext.StartActivity(gpsSettingIntent);
                }
                _btManager.Adapter?.Enable();
                //_gpsManager.IsProviderEnabled()
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        public void DisableBluetooth()
        {
            _btManager.Adapter?.Disable();
        }
    }
}


