using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Fakka.Pos.Droid
{
    [Activity(
        Label = "نقطة البيع",
        Theme = "@style/SplashTheme",
        Icon = "@drawable/icon",
        ScreenOrientation = ScreenOrientation.Landscape,
        NoHistory = true,
        MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            StartActivity(typeof(MainActivity));
        }
    }
}