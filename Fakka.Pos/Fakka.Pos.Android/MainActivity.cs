using System;
using Fakka.Core.Extensions;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism;
using Prism.Ioc;
using Fakka.Core.Interfaces;
using Fakka.Core.Enums;
using System.Globalization;
using Fakka.Config.Settings;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Acr.UserDialogs;

namespace Fakka.Pos.Droid
{
    [Activity(Theme = "@style/MainTheme",
    MainLauncher = false, NoHistory = false, ScreenOrientation = ScreenOrientation.Landscape,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            UpdateApplicationLanguage();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void UpdateApplicationLanguage()
        {
            if (AppSettings.CurrentCultureInfo != CultureLocale.Invariant)
            {
                Calendar umAlQuraCalendar = new UmAlQuraCalendar();
                Calendar hijriCalendar = new HijriCalendar();
                var selectedCultureInfoSa =
                    CultureInfo.CreateSpecificCulture(AppSettings.CurrentCultureInfo
                        .ToDescriptionString());
                var selectedCultureInfoEg =
                    CultureInfo.CreateSpecificCulture(CultureLocale.ArEgy.ToDescriptionString());
                if (CalendarExists(selectedCultureInfoSa, hijriCalendar))
                {
                    //selectedCultureInfoSa.OptionalCalendars[0] = hijriCalendar;
                    selectedCultureInfoSa.DateTimeFormat.Calendar = hijriCalendar;
                }

                CultureInfo.DefaultThreadCurrentCulture = selectedCultureInfoSa;
                CultureInfo.DefaultThreadCurrentUICulture = selectedCultureInfoSa;
                System.Threading.Thread.CurrentThread.CurrentCulture = selectedCultureInfoSa;
                System.Threading.Thread.CurrentThread.CurrentUICulture = selectedCultureInfoSa;

                var selectedCulture = AppSettings.CurrentCultureInfo.ToDescriptionString().Split('-');
                if (selectedCulture.Length == 2)
                {
                    var selectedLang = selectedCulture[0];
                    var selectedCountry = selectedCulture[1];
                    var locale = new Java.Util.Locale(selectedLang, selectedCountry);
                    Java.Util.Locale.Default = locale;

                    var config = new Android.Content.Res.Configuration { Locale = locale };
                    Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics);
                }

            }

            // Check if the selected culture info supports a certain calender

        }

        private bool CalendarExists(CultureInfo culture, Calendar cal)
        {
            foreach (Calendar optionalCalendar in culture.OptionalCalendars)
                if (cal.ToString().Equals(optionalCalendar.ToString()))
                    return true;

            return false;
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterSingleton<INative, NativeServices>();

        }
    }

}