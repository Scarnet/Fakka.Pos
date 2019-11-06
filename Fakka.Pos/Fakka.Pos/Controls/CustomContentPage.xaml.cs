using Fakka.Core.Enums;
using Fakka.Core.Managers;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Controls
{
    public partial class CustomContentPage : ContentView
    {

        public CustomContentPage()
        {
            InitializeComponent();
            StatusBarView.IsVisible = ApplicationManager.Instance.GetTerminalInfo().TerminalCode == TerminalCode.Ios;
        }
        #region Properties
        public View ContentView
        {
            get => ViewContainer;
            set => ViewContainer.Content = value;
        }

        public ImageSource BackgroundImage
        {
            get => Background.Source;
            set => Background.Source = value;
        }
        public double BackgroundOpacity
        {
            get => Background.Opacity;
            set => Background.Opacity = value;
        }

        public bool HasBackgroundImage
        {
            get => Background.IsVisible;
            set => Background.IsVisible = value;
        }

        public bool HasBackgroundOverlay
        {
            get => BackgroundOverlay.IsVisible;
            set => BackgroundOverlay.IsVisible = value;
        }

        public Color BackgroundOverlayColor
        {
            get => BackgroundOverlay.BackgroundColor;
            set => BackgroundOverlay.BackgroundColor = value;
        }


        public double BackgroundOverlayOpacity
        {
            get => BackgroundOverlay.Opacity;
            set => BackgroundOverlay.Opacity = value;
        }


        #endregion
    }
}


