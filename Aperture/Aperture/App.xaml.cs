using System.ComponentModel;
using Xamarin.Forms;

namespace Aperture
{
    public partial class App : Application
    {
        private NavigationPage _navigationPage;
        private Color _initialColor;

        public App()
        {
            InitializeComponent();

            _navigationPage = new NavigationPage(new MainPage());
            _initialColor = _navigationPage.BarBackgroundColor;
            _navigationPage.PropertyChanged += NavigationPageOnPropertyChanged;
            _navigationPage.BarBackgroundColor = Color.FromHex("#FF666666");
            MainPage = _navigationPage;
        }

        private void NavigationPageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != NavigationPage.CurrentPageProperty.PropertyName)
            {
                return;
            }

            if (_navigationPage.CurrentPage.GetValue(NavigationPage.BarBackgroundColorProperty) is Color desiredBaBackgroundColor)
            {
                _navigationPage.BarBackgroundColor = desiredBaBackgroundColor;
            }
            else
            {
                _navigationPage.BarBackgroundColor = _initialColor;
            }
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}