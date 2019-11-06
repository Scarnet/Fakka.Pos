using Fakka.Core.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components.Children
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChildCard : ContentView
    {

        public readonly static BindableProperty ChildProperty = BindableProperty.Create(nameof(Child), typeof(ChildProfile), typeof(ChildCard), propertyChanged: HandleChildChanged);

        public ChildProfile Child
        {
            get => (ChildProfile)GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }

        #region Properties
        private string _childName;
        public string ChildName
        {
            get => _childName;
            protected set
            {
                _childName = value;
                OnPropertyChanged();
            }
        }

        private string _parentName;
        public string ParentName
        {
            get => _parentName; 
            protected set
            {
                _parentName = value;
                OnPropertyChanged();
            }
        }

        private bool _hasImage;
        public bool HasImage
        {
            get => _hasImage;
            protected set
            {
                _hasImage = value;
                OnPropertyChanged();
            }
        }

        private string _childImage;
        public string ChildImage
        {
            get => _childImage;
            protected set
            {
                _childImage = value;
                OnPropertyChanged();
            }
        }

        private string _childCode;
        public string  ChildCode
        {
            get => _childCode;
            protected set
            {
                _childCode = value;
                OnPropertyChanged();
            }
        }
        public string _gradeName;
        public string GradeName
        {
            get => _gradeName;
            protected set
            {
                _gradeName = value;
                OnPropertyChanged();
            }
        }

        private string _currentBalance;
        public string CurrentBalance
        {
            get => _currentBalance;
            protected set
            {
                _currentBalance = value;
                OnPropertyChanged();
            }
        }

        private string _dailyLimit;
        public string DailyLimit
        {
            get => _dailyLimit;
            protected set
            {
                _dailyLimit = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public ChildCard()
        {
            InitializeComponent();
        }

        private static void HandleChildChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ChildCard)bindable;

            var child = (ChildProfile)newValue;

            if (child == null)                //Empty child
            {
                EmptyChildProperties(control);
                control.BannedSign.IsVisible = false;
                //control.BannedProductsContainer.IsVisible = false;
                //control.BannedProductsList.ItemsSource = null;
            }
            else
            {
                SetChildProperties(control, child);
                bool hasBannedProducts = child.BannedProducts != null && child.BannedProducts.Any();
                control.BannedSign.IsVisible = hasBannedProducts;
                //control.BannedProductsContainer.IsVisible = hasBannedProducts;
                //control.BannedProductsList.ItemsSource = child.BannedProducts;
            }
        }

        private static void SetChildProperties(ChildCard control, ChildProfile child)
        {
            control.ChildName = child.Name;
            control.ParentName = child.ParentName;
            control.HasImage = child.HasImage;
            control.ChildImage = child.ChildImage;
            control.ChildCode = child.Code.ToString();
            control.GradeName = child.GradeName;
            control.CurrentBalance = child.CurrentBalance?.ToString("N2") ?? "0";
            control.DailyLimit = child.DailyLimit?.ToString("N2") ?? "0";
        }

        private static void EmptyChildProperties(ChildCard control)
        {
            control.ChildName = string.Empty;
            control.ParentName = string.Empty;
            control.HasImage = false;
            control.ChildImage = string.Empty;
            control.ChildCode = string.Empty;
            control.GradeName = string.Empty;
            control.CurrentBalance = "0";
            control.DailyLimit = "0";
        }

    }
}