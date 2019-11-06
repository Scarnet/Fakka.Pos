using Fakka.Core.Business.Models;
using Fakka.Core.Validations;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchHeader : ContentView
    {
        public event EventHandler<object> ChildSelected;
        public event EventHandler SchoolLogoTapped;
        public static readonly BindableProperty SearchNameChangedCommandProperty = BindableProperty.Create(nameof(SearchNameChangedCommand), typeof(DelegateCommand<ValidatableObject<string>>), typeof(SearchHeader));
        public static readonly BindableProperty SearchCodeChangedCommandProperty = BindableProperty.Create(nameof(SearchCodeChangedCommand), typeof(DelegateCommand<ValidatableObject<string>>), typeof(SearchHeader));
        public static readonly BindableProperty GradesProperty = BindableProperty.Create(nameof(Grades), typeof(IList<Grade>), typeof(SearchHeader), propertyChanged: HandleItemsChanged);
        public static readonly BindableProperty GradeDisplayProperty = BindableProperty.Create(nameof(GradeDisplay), typeof(string), typeof(SearchHeader), propertyChanged: HandleGradeDisplayChanged);
        public static readonly BindableProperty BluetoothChildrenProperty = BindableProperty.Create(nameof(BluetoothChildren), typeof(ObservableCollection<ChildProfile>), typeof(SearchHeader), propertyChanged: HandleCBluetoothhildrenChanged);
        public static readonly BindableProperty SearchChildrenProperty = BindableProperty.Create(nameof(SearchChildren), typeof(ObservableCollection<ChildProfile>), typeof(SearchHeader), propertyChanged: HandleSearchChildrenChanged);
        public static readonly BindableProperty SearchNameProperty = BindableProperty.Create(nameof(SearchName), typeof(ValidatableObject<string>), typeof(SearchHeader), propertyChanged: HandleSearchNameChanged);
        public static readonly BindableProperty SearchCodeProperty = BindableProperty.Create(nameof(SearchCode), typeof(ValidatableObject<string>), typeof(SearchHeader), propertyChanged: HandleSearchCodeChanged);
        public static readonly BindableProperty BluetoothActiveProperty = BindableProperty.Create(nameof(BluetoothActive), typeof(bool), typeof(SearchHeader), default(bool));
        public static readonly BindableProperty BluetoothTappedCommandProperty = BindableProperty.Create(nameof(BluetoothTappedCommand), typeof(DelegateCommand), typeof(SearchHeader));
        public static readonly BindableProperty SearchNameValidationCommandProperty = BindableProperty.Create(nameof(SearchNameValidationCommand), typeof(DelegateCommand), typeof(SearchHeader),
            propertyChanged: HandelSearchNameValidationChanged);
        public static readonly BindableProperty SearchCodeValidationCommandProperty = BindableProperty.Create(nameof(SearchCodeValidationCommand), typeof(DelegateCommand), typeof(SearchHeader),
            propertyChanged: HandleSearchCodeValidationChanged);
        public static readonly BindableProperty SelectedGradeProperty = BindableProperty.Create(nameof(SelectedGrade), typeof(object), typeof(SearchHeader), default(object), BindingMode.TwoWay);
        public static readonly BindableProperty SchoolLogoProperty = BindableProperty.Create(nameof(SchoolLogo), typeof(ImageSource), typeof(SearchHeader));
        public static readonly BindableProperty SchoolNameProperty = BindableProperty.Create(nameof(SchoolName), typeof(string), typeof(SearchHeader));
        public static readonly BindableProperty SearchingProperty = BindableProperty.Create(nameof(Searching), typeof(bool), typeof(SearchHeader), false, BindingMode.TwoWay);
        public DelegateCommand<ValidatableObject<string>> SearchNameChangedCommand
        {
            get => (DelegateCommand<ValidatableObject<string>>)GetValue(SearchNameChangedCommandProperty);
            set => SetValue(SearchNameChangedCommandProperty, value);
        }

        public DelegateCommand<ValidatableObject<string>> SearchCodeChangedCommand
        {
            get => (DelegateCommand<ValidatableObject<string>>)GetValue(SearchCodeChangedCommandProperty);
            set => SetValue(SearchCodeChangedCommandProperty, value);
        }

        public DelegateCommand SearchNameValidationCommand
        {
            get => (BaseValidationsHandler<string>)GetValue(SearchNameValidationCommandProperty);
            set => SetValue(SearchNameChangedCommandProperty, value);
        }
        public DelegateCommand SearchCodeValidationCommand
        {
            get => (BaseValidationsHandler<string>)GetValue(SearchCodeValidationCommandProperty);
            set => SetValue(SearchCodeValidationCommandProperty, value);
        }

        public IList<Grade> Grades
        {
            get => (IList<Grade>)GetValue(GradesProperty);
            set => SetValue(GradesProperty, value);
        }

        public string GradeDisplay
        {
            get => (string)GetValue(GradeDisplayProperty);
            set => SetValue(GradeDisplayProperty, value);
        }

        public ObservableCollection<ChildProfile> BluetoothChildren
        {
            get => (ObservableCollection<ChildProfile>)GetValue(BluetoothChildrenProperty);
            set => SetValue(BluetoothChildrenProperty, value);
        }

        public ObservableCollection<ChildProfile> SearchChildren
        {
            get => (ObservableCollection<ChildProfile>)GetValue(SearchChildrenProperty);
            set => SetValue(SearchChildrenProperty, value);
        }

        public ValidatableObject<string> SearchName
        {
            get => (ValidatableObject<string>)GetValue(SearchNameProperty);
            set => SetValue(SearchNameProperty, value);
        }

        public ValidatableObject<string> SearchCode
        {
            get => (ValidatableObject<string>)GetValue(SearchCodeProperty);
            set => SetValue(SearchCodeProperty, value);
        }

        public object SelectedGrade
        {
            get => GetValue(SelectedGradeProperty);
            set => SetValue(SelectedGradeProperty, value);
        }
        public ImageSource SchoolLogo
        {
            get => (ImageSource)GetValue(SchoolLogoProperty);
            set => SetValue(SchoolLogoProperty, value);
        }

        public string SchoolName
        {
            get => (string)GetValue(SchoolNameProperty);
            set => SetValue(SchoolNameProperty, value);
        }

        private bool _searchWithName;
        public bool SearchWithName
        {
            get => _searchWithName;
            set
            {
                _searchWithName = value;
                OnPropertyChanged();
            }
        }

        public bool Searching
        {
            get => (bool)GetValue(SearchingProperty);
            set => SetValue(SearchingProperty, value);
        }

        public bool BluetoothActive
        {
            get => (bool)GetValue(BluetoothActiveProperty);
            set => SetValue(BluetoothActiveProperty, value);
        }

        public DelegateCommand BluetoothTappedCommand
        {
            get => (DelegateCommand)GetValue(BluetoothTappedCommandProperty);
            set => SetValue(BluetoothTappedCommandProperty, value);
        }

        private static void HandleItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            control.GradesPicker.ItemsSource = (IList)newValue;
        }

        private static void HandleGradeDisplayChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            control.GradesPicker.ItemDisplayBinding = new Binding(newValue.ToString(), source: control.GradesPicker.SelectedItem);
        }
        private static void HandleCBluetoothhildrenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            var lst = (ObservableCollection<ChildProfile>)newValue;
            var oldLst = (ObservableCollection<ChildProfile>)oldValue;

            if (oldLst != null)
                oldLst.CollectionChanged -= control.BluetoothChildrenChanged;

            if (lst != null)
                lst.CollectionChanged += control.BluetoothChildrenChanged;

            if (lst == null || !lst.Any())
            {
                control.BluetoothChildrenList.IsVisible = false;
                control.BlChildrenEmptyStateView.IsVisible = true;
            }
            else
            {
                control.BluetoothChildrenList.IsVisible = true;
                control.BlChildrenEmptyStateView.IsVisible = false;
            }
            control.BluetoothChildrenList.ItemsSource = lst;

        }

        private static void HandleSearchNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            control.NameEntry.ValidatableObject = (ValidatableObject<string>)newValue;
        }

        private static void HandleSearchCodeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            control.CodeEntry.ValidatableObject = (ValidatableObject<string>)newValue;
        }

        private static void HandelSearchNameValidationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            control.NameEntry.ValidationCommand = (BaseValidationsHandler<string>)newValue;
        }

        private static void HandleSearchCodeValidationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            control.CodeEntry.ValidationCommand = (BaseValidationsHandler<string>)newValue;
        }

        private static void HandleSearchChildrenChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchHeader)bindable;
            var lst = (ObservableCollection<ChildProfile>)newValue;
            var oldLst = (ObservableCollection<ChildProfile>)oldValue;


            if (oldLst != null)
                oldLst.CollectionChanged -= control.SearchChildrenChanged;

            if (lst != null)
                lst.CollectionChanged += control.SearchChildrenChanged;
            else
                control.Searching = false;

            if (lst == null || !lst.Any())
            {
                control.ChildrenList.IsVisible = false;
                control.SearchChildrenEmptyView.IsVisible = true;
            }
            else
            {
                control.ChildrenList.IsVisible = true;
                control.SearchChildrenEmptyView.IsVisible = false;
            }
            control.ChildrenList.ItemsSource = lst;
        }


        public SearchHeader()
        {
            InitializeComponent();
        }

        private void HandleSearchMethodLabelTapped(object sender, EventArgs e)
        {
            if (NameEntry.ValidatableObject != null)
                NameEntry.ValidatableObject.Value = string.Empty;

            if (CodeEntry.ValidatableObject != null)
                CodeEntry.ValidatableObject.Value = string.Empty;

            SearchWithName = !SearchWithName;
        }

        private void NameEntry_Completed(object sender, EventArgs e)
        {
            if (SearchNameChangedCommand == null)
                return;

           // Searching = true;
        }

        private void CodeEntry_Completed(object sender, EventArgs e)
        {
            if (SearchCodeChangedCommand == null)
                return;

           // Searching = true;
        }

        private void CloseLabel_Tapped(object sender, EventArgs e)
        {
            Searching = false;
        }

        private void ChildrenList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!e.CurrentSelection.Any() || e.CurrentSelection[0] == null)
                return;

            ChildSelected?.Invoke(this, e.CurrentSelection[0]);
            var list = (CollectionView)sender;
            list.SelectedItem = null;
        }

        private void SchoolLogo_Tapped(object sender, EventArgs e)
        {
            SchoolLogoTapped?.Invoke(sender, e);
        }
        private void SearchChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool isEmptyList = e.NewItems == null || e.NewItems.Count <= 0;

            if (isEmptyList)
            {
                ChildrenList.IsVisible = false;
                SearchChildrenEmptyView.IsVisible = true;
            }
            else
            {
                ChildrenList.IsVisible = true;
                SearchChildrenEmptyView.IsVisible = false;
            }

        }

        private void BluetoothChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool isEmptyList = BluetoothChildren == null || BluetoothChildren.Count <= 0;

            if (isEmptyList)
            {
                BluetoothChildrenList.IsVisible = false;
                BlChildrenEmptyStateView.IsVisible = true;
            }
            else
            {
                BluetoothChildrenList.IsVisible = true;
                BlChildrenEmptyStateView.IsVisible = false;
            }
        }

    }
}