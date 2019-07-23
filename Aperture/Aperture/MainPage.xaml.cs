using System;
using System.ComponentModel;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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

            for (int i = 0; i < 7; i++)
            {
                BladeGrid.Children.Add(new BoxView(){BackgroundColor = Color.Gold, Margin = 0});
            }

            ApertureFrame.SizeChanged += ApertureFrameOnSizeChanged;
        }

        private void ApertureFrameOnSizeChanged(object sender, EventArgs e)
        {
            PositionBlades(ApertureFrame.Width, ApertureFrame.Height);
        }

        private void PositionBlades(double width, double height)
        {
            var bladeCount = 7;
            var angularBladeOffset = 360.0d / bladeCount;
            var currentBladeOffset = 0.0d;
            var degreesToRadiansFactor = 0.01745329d;

            var r = width / 2.0d;

            for (int i = 0; i < bladeCount; i++)
            {
                var currentBladeX = r * Math.Cos(currentBladeOffset * degreesToRadiansFactor);
                var currentBladeY = r * Math.Sin(currentBladeOffset * degreesToRadiansFactor);

                PostionBlade(BladeGrid.Children[i] as BoxView, currentBladeX, currentBladeY, currentBladeOffset);

                currentBladeOffset += angularBladeOffset;
            }
        }


        Random rand = new Random();
        private void PostionBlade(BoxView bladeGridChild, double currentBladeX, double currentBladeY, double currentBladeOffset)
        {
            bladeGridChild.BackgroundColor = Color.Gold;
            bladeGridChild.WidthRequest = Math.Sqrt(ApertureFrame.Width * ApertureFrame.Width / 2.0d); //TBD
            bladeGridChild.HeightRequest = Math.Sqrt(ApertureFrame.Width * ApertureFrame.Width / 2.0d); //TBD
            bladeGridChild.TranslationX = currentBladeX;
            bladeGridChild.TranslationY = currentBladeY;

            bladeGridChild.Rotation = currentBladeOffset - 45.0d;

            //bladeGridChild.BackgroundColor = Color.FromRgb(rand.NextDouble(), rand.NextDouble(), rand.NextDouble()).WithLuminosity(0.8).WithSaturation(0.8);
        }

        private void Slider_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            BladeGrid.Children.ForEach(view => view.Rotation += e.NewValue - e.OldValue); 
        }
    }
}