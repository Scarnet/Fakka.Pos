using System;
using System.ComponentModel;
using System.Windows.Input;
using Fakka.Core.Utilities;
using Fakka.Core.Validations;
using Fakka.Pos.Enum;
using Prism.Commands;
using Xamarin.Forms;

namespace Fakka.Pos.Controls
{
    public partial class CustomEntry : ContentView
    {

        public CustomEntry()
        {
            InitializeComponent();
            CustomEntryContainer.BindingContext = this;
            TextEntry.ReturnCommand = new Command(HandleReturn);
        }


        #region Properties

        public Style EntryStyle
        {
            get => TextEntry.Style;
            set => TextEntry.Style = value;
        }

        object completedLock = string.Empty;
        public event EventHandler Completed
        {
            add
            {
                lock (completedLock)
                    TextEntry.Completed += value;
            }
            remove
            {
                lock (completedLock)
                    TextEntry.Completed -= value;
            }
        }

        public string Placeholder
        {
            get => TextEntry.Placeholder;
            set => TextEntry.Placeholder = value;
        }

        public bool IsPassword
        {
            get => TextEntry.IsPassword;
            set => TextEntry.IsPassword = value;
        }

        public ImageAlignment ImageAlignment
        {
            get => TextEntry.ImageAlignment;
            set => TextEntry.ImageAlignment = value;
        }

        public int MaxLength
        {
            get => TextEntry.MaxLength;
            set => TextEntry.MaxLength = value;
        }

        public string Icon
        {
            get => TextEntry.Icon;
            set => TextEntry.Icon = value;
        }

        public double FontSize
        {
            get => TextEntry.FontSize;
            set => TextEntry.FontSize = value;
        }

        public string FontFamily
        {
            get => TextEntry.FontFamily;
            set => TextEntry.FontFamily = value;
        }

        public DelegateCommand<ValidatableObject<string>> ReturnCommand
        {
            get => (DelegateCommand<ValidatableObject<string>>)GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }
        #endregion

        #region ValidationCommand

        public static readonly BindableProperty ValidationCommandProperty = BindableProperty.Create(
                nameof(ValidationCommand), typeof(DelegateCommand), typeof(CustomEntry), null, BindingMode.Default);
        public DelegateCommand ValidationCommand
        {
            get => (DelegateCommand)GetValue(ValidationCommandProperty);
            set => SetValue(ValidationCommandProperty, value);
        }

        #endregion

        #region ValidatableObject
        public static readonly BindableProperty ValidatableObjectProperty = BindableProperty.Create(
            nameof(ValidatableObject), typeof(ValidatableObject<string>), typeof(CustomEntry), null, BindingMode.TwoWay);


        public ValidatableObject<string> ValidatableObject
        {
            get => (ValidatableObject<string>)GetValue(ValidatableObjectProperty);
            set => SetValue(ValidatableObjectProperty, value);
        }

        #endregion

        #region ValidationOnly
        public static readonly BindableProperty IsValidationOnlyProperty = BindableProperty.Create(
            nameof(IsValidationOnly), typeof(bool), typeof(CustomEntry), default(bool), BindingMode.TwoWay);


        public bool IsValidationOnly
        {
            get => (bool)GetValue(IsValidationOnlyProperty);
            set => SetValue(IsValidationOnlyProperty, value);
        }

        #endregion

        public static readonly BindableProperty ValueChangedCommandProperty = BindableProperty.Create(
            nameof(ValueChangedCommand), typeof(DelegateCommand<ValidatableObject<string>>), typeof(CustomEntry), null, BindingMode.Default);

        public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand), typeof(DelegateCommand<ValidatableObject<string>>), typeof(CustomEntry));
        public DelegateCommand<ValidatableObject<string>> ValueChangedCommand
        {
            get => (DelegateCommand<ValidatableObject<string>>)GetValue(ValueChangedCommandProperty);
            set => SetValue(ValueChangedCommandProperty, value);
        }

        private void TextEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var element = this.ValidatableObject;
            if (element == null || TextEntry == null) return;

            TextEntry.Text = TextEntry.Text.ConvertToEnglish();

            if (!element.IsValid)
            {
                TextEntry.BorderColor = (Color)Application.Current.Resources["ErrorColor"];
            }
            else
            {
                TextEntry.BorderColor = Color.Default;
            }
            ValueChangedCommand?.Execute(element);

        }



        private void ErrorLabel_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != Label.TextProperty.PropertyName)
                return;

            var element = (Label)sender;
            if (element == null || TextEntry == null) return;

            if (element.IsVisible)
            {
                TextEntry.BorderColor = (Color)Application.Current.Resources["ErrorColor"];
            }
            else
            {
                TextEntry.BorderColor = Color.Default;
            }
        }

        private void HandleReturn()
        {
            var element = this.ValidatableObject;
            if (element == null || TextEntry == null) return;

            TextEntry.Text = TextEntry.Text.ConvertToEnglish();

            if (!element.IsValid)
            {
                TextEntry.BorderColor = (Color)Application.Current.Resources["ErrorColor"];
            }
            else
            {
                TextEntry.BorderColor = Color.Default;
            }
            ReturnCommand?.Execute(element);
        }

        private string auxPlaceholder;
        private void TextEntry_Focused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            bool emptyEntry = entry.Text == string.Empty;

            if (emptyEntry)
            {
                this.auxPlaceholder = entry.Placeholder;
                entry.Placeholder = string.Empty;
            };
        }

        private void TextEntry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            bool emptyEntry = entry.Text.IsNullOrWhiteSpace();

            if (emptyEntry)
                entry.Placeholder = this.auxPlaceholder;


        }
    }
}


