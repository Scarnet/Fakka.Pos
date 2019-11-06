using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fakka.Pos.Components.School
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchoolWindow : ContentView
    {
        public static readonly BindableProperty LogoutButtonCommandProperty = BindableProperty.Create(nameof(LogoutButtonCommand), typeof(DelegateCommand), typeof(SchoolWindow));
        public static readonly BindableProperty SchoolNameProperty = BindableProperty.Create(nameof(SchoolName), typeof(string), typeof(SchoolWindow));
        public static readonly BindableProperty EducationalDepartmentNameProperty = BindableProperty.Create(nameof(EducationalDepartmentName), typeof(string), typeof(SchoolWindow));
        public static readonly BindableProperty SchoolLogoProperty = BindableProperty.Create(nameof(SchoolLogo), typeof(ImageSource), typeof(SchoolWindow));
        public DelegateCommand LogoutButtonCommand
        {
            get => (DelegateCommand)GetValue(LogoutButtonCommandProperty);
            set => SetValue(LogoutButtonCommandProperty, value);
        }

        public string SchoolName
        {
            get => (string)GetValue(SchoolNameProperty);
            set => SetValue(SchoolNameProperty, value);
        }

        public string EducationalDepartmentName
        {
            get => (string)GetValue(EducationalDepartmentNameProperty);
            set => SetValue(EducationalDepartmentNameProperty, value);
        }

        public ImageSource SchoolLogo
        {
            get => (ImageSource)GetValue(SchoolLogoProperty);
            set => SetValue(SchoolLogoProperty, value); 
        }

        public SchoolWindow()
        {
            InitializeComponent();
        }
    }
}