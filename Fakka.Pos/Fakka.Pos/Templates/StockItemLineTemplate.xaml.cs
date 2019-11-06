using Fakka.Pos.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockItemLineTemplate : ContentView
    {

        public static readonly BindableProperty LineIncreasedCommandProperty = BindableProperty.Create(nameof(LineIncreasedCommand), typeof(DelegateCommand<object>), typeof(StockItemLineTemplate));
        public static readonly BindableProperty LineDecreasedCommandProperty = BindableProperty.Create(nameof(LineDecreasedCommand), typeof(DelegateCommand<object>), typeof(StockItemLineTemplate));


        public DelegateCommand<object> LineIncreasedCommand
        {
            get => (DelegateCommand<object>)GetValue(LineIncreasedCommandProperty);
            set => SetValue(LineIncreasedCommandProperty, value);
        }

        public DelegateCommand<object> LineDecreasedCommand
        {
            get => (DelegateCommand<object>)GetValue(LineDecreasedCommandProperty);
            set => SetValue(LineDecreasedCommandProperty, value);
        }
        public StockItemLineTemplate()
        {
            InitializeComponent();
        }
    }
}