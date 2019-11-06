using Fakka.Core.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private INative native;
        public MainPage(INative native)
        {
            InitializeComponent();
            this.native = native;
        }

        private void SearchHeader_SchoolLogoTapped(object sender, EventArgs e)
        {
            SchoolWindnow.IsVisible = !SchoolWindnow.IsVisible;
        }

        private void FullPos_Clicked(object sender, EventArgs e)
        {
            StockItemsList.IsVisible = true;
            TodaysOrderList.IsVisible = false;
        }

        private void TodaysOrders_Clicked(object sender, EventArgs e)
        {
            StockItemsList.IsVisible = false;
            TodaysOrderList.IsVisible = true;

        }

        private void PageTapped_Tapped(object sender, EventArgs e)
        {
            this.native.HideKeyboard();
        }

        private void TodaysOrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TodaysOrderList.SelectedItem = null;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}