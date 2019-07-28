using Aperture.Pages;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Aperture
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SimpleClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Simple());
        }

        private async void CoolClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cool());
        }
    }
}