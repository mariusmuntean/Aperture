using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aperture.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(true)]
    public class Cool : ContentPage
    {
        private ApertureView.ApertureView _apertureView;
        private Image _xamarinImage;

        public Cool()
        {
            SetValue(NavigationPage.BarBackgroundColorProperty, Color.FromHex("#FF666666"));

            var rootLayout = new RelativeLayout();

            var coolImg = new Image
            {
                Aspect = Aspect.AspectFill,
                Source = GetCoolImageSource(),
                Margin = new Thickness(0)
            };
            rootLayout.Children.Add(coolImg,
                Constraint.RelativeToParent(parent => 0.0d),
                Constraint.RelativeToParent(parent => 0.0d),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height)
                );

            _xamarinImage = new Image
            {
                Aspect = Aspect.AspectFill,
                Source = GetXamarinImageSource()
            };
            _apertureView = new ApertureView.ApertureView
            {
                ApertureOpening = 0.0,
                ApertureColor = Color.Black,
                BackgroundColor = Color.Transparent,
            };
            rootLayout.Children.Add(_apertureView,
                Constraint.RelativeToParent(parent => parent.Width / 2.0 - (parent.Width * 0.17)),
                Constraint.RelativeToParent(parent => parent.Height / 2.0 - (parent.Height * 0.18)),
                Constraint.RelativeToParent(parent => parent.Width * 0.34),
                Constraint.RelativeToParent(parent => parent.Height * 0.34)
                );

            var shutterButton = new Button
            {
                BackgroundColor = Color.White,
                CornerRadius = 30,
                Command = new Command(AnimateAperture)
            };
            rootLayout.Children.Add(shutterButton,
                Constraint.RelativeToParent(parent => parent.Width / 2.0 - 30.0),
                Constraint.RelativeToParent(parent => parent.Height - 95.0),
                Constraint.RelativeToParent(parent => 60.0),
                Constraint.RelativeToParent(parent => 60.0)
            );

            var xamarinSwitch = new Switch { IsToggled = false, OnColor = Color.DodgerBlue, Visual = VisualMarker.Material };
            xamarinSwitch.Toggled += (sender, args) => { _apertureView.ContentView = args.Value ? _xamarinImage : null; };
            rootLayout.Children.Add(xamarinSwitch,
                Constraint.RelativeToView(shutterButton, (p, v) => v.X + v.Width + 10),
                Constraint.RelativeToView(shutterButton, (p, v) => v.Y + v.Height / 2.0 - 15.0)
            );

            var tapMeLabel = new Label
            {
                Text = "(tap me)",
                TextColor = Color.FromHex("#BBFFFFFF"),
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center
            };
            rootLayout.Children.Add(tapMeLabel,
                Constraint.RelativeToView(shutterButton, (parent, view) => 0.0),
                Constraint.RelativeToView(shutterButton, (parent, view) => view.Y - 35.0),
                Constraint.RelativeToView(shutterButton, (parent, view) => parent.Width),
                Constraint.RelativeToView(shutterButton, (parent, view) => 20.0)
                );

            Content = rootLayout;
        }

        private void AnimateAperture()
        {
            var apertureOpeningAnimation = new Animation(d => _apertureView.ApertureOpening = d, 0.0, 1.0, easing: Easing.CubicIn);
            var apertureClosingAnimation = new Animation(d => _apertureView.ApertureOpening = d, 1.0, 0.0, easing: Easing.CubicOut);

            var parentAnimation = new Animation();
            parentAnimation.Add(0.0, 0.5, apertureOpeningAnimation);
            parentAnimation.Add(0.5, 1.0, apertureClosingAnimation);

            parentAnimation.Commit(this, "OpenCloseApertureAnimation", length: 300);
        }

        private ImageSource GetCoolImageSource()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    {
                        return ImageSource.FromFile("cool");
                    }

                case Device.iOS:
                    {
                        return ImageSource.FromFile("cool.jpg");
                    }

                case Device.UWP:
                    {
                        return ImageSource.FromFile("Assets/cool.jpg");
                    }

                default:
                    {
                        return new FileImageSource();
                    }
            }
        }

        private ImageSource GetXamarinImageSource()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    {
                        return ImageSource.FromFile("xamarin");
                    }

                case Device.iOS:
                    {
                        return ImageSource.FromFile("xamarin.jpg");
                    }

                case Device.UWP:
                    {
                        return ImageSource.FromFile("Assets/xamarin.jpg");
                    }

                default:
                    {
                        return new FileImageSource();
                    }
            }
        }
    }
}