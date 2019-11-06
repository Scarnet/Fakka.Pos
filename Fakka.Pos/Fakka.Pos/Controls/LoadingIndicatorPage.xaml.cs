using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingIndicatorPage : ContentPage
    {
        public LoadingIndicatorPage()
        {
            InitializeComponent();
        }

        public void SetIndicatorTitle(string busyMessage = null)
        {
            if (busyMessage == null)
            {
                IndicatorLabel.IsVisible = false;
                IndicatorLabel.Text = string.Empty;
                return;
            }
            IndicatorLabel.Text = busyMessage;
            IndicatorLabel.IsVisible = true;
        }
    }
}