using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Fakka.Config.Settings;
using Fakka.Core.Enums;
using Fakka.Core.Interfaces;
using Foundation;
using Prism;
using Prism.Ioc;
using UIKit;
using Fakka.Core.Extensions;
using Xamarin.Forms;

namespace Fakka.Pos.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            LoadApplication(new App(new iOSInitializer()));
            // Force override any culture info 
            if (AppSettings.CurrentCultureInfo != CultureLocale.Invariant)
            {

                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(AppSettings.CurrentCultureInfo.ToDescriptionString());
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(AppSettings.CurrentCultureInfo.ToDescriptionString());

            }

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.RegisterSingleton<INative, NativeServices>();
        }
    }

}
