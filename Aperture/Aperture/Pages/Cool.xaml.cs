﻿using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aperture.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(true)]
    public partial class Cool : ContentPage
    {
        public Cool()
        {
            InitializeComponent();
        }
    }
}