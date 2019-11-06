using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockItemsGroupHeader : ContentView
    {
        public static BindableProperty GroupNameProperty = BindableProperty.Create(nameof(GroupName), typeof(string), typeof(StockItemsGroupHeader), string.Empty);

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }
        public StockItemsGroupHeader()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}