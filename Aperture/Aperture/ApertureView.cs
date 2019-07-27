using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aperture
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ApertureView : RelativeLayout
    {
        private const int BladeCount = 6;

        private Grid _apertureFrameContentGrid;
        private Frame _apertureFrame;

        public ApertureView()
        {
            _apertureFrame = new Frame
            {
                BackgroundColor = Color.Chartreuse,
                Padding = new Thickness(0),
                Margin = new Thickness(0),
                IsClippedToBounds = true
            };
            _apertureFrame.SizeChanged += ApertureFrameOnSizeChanged;

            _apertureFrameContentGrid = new Grid
            {
                IsClippedToBounds = true
            };
            _apertureFrame.Content = _apertureFrameContentGrid;

            for (int i = 0; i < BladeCount; i++)
            {
                _apertureFrameContentGrid.Children.Add(new BoxView { BackgroundColor = ApertureColor, Margin = 0 });
            }

            Children.Add(_apertureFrame,
                Constraint.RelativeToParent(parent =>
                {
                    var smallerParentDimension = Math.Min(parent.Width, parent.Height);
                    return parent.Width / 2.0 - smallerParentDimension / 2.0;
                }),
                Constraint.RelativeToParent(parent =>
                {
                    var smallerParentDimension = Math.Min(parent.Width, parent.Height);
                    return parent.Height / 2.0 - smallerParentDimension / 2.0;
                }),
                Constraint.RelativeToParent(parent => Math.Min(parent.Width, parent.Height)),
                Constraint.RelativeToParent(parent => Math.Min(parent.Width, parent.Height))
                );
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var min = Math.Min(widthConstraint, heightConstraint);
            return new SizeRequest(new Size(min, min));
        }

        public static readonly BindableProperty ApertureOpeningProperty = BindableProperty.Create(
            nameof(ApertureOpening),
            typeof(Double),
            typeof(ApertureView),
            1.0d,
            propertyChanged: OnApertureOpeningChanged
            );

        private static void OnApertureOpeningChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ApertureView apertureView && newvalue is double)
            {
                apertureView.PositionBlades(apertureView._apertureFrame.Width, apertureView._apertureFrame.Height);
            }
        }

        public double ApertureOpening { get => (double)GetValue(ApertureOpeningProperty); set => SetValue(ApertureOpeningProperty, value); }

        public static readonly BindableProperty ApertureColorProperty = BindableProperty.Create(
            nameof(ApertureColor),
            typeof(Color),
            typeof(ApertureView),
            Color.LightGray
        );

        public Color ApertureColor { get => (Color)GetValue(ApertureColorProperty); set => SetValue(ApertureColorProperty, value); }

        private double Bound(double value, double minValue, double maxValue)
        {
            if (value < minValue)
            {
                return minValue;
            }

            if (value > maxValue)
            {
                return maxValue;
            }

            return value;
        }

        private void ApertureFrameOnSizeChanged(object sender, EventArgs e)
        {
            _apertureFrame.CornerRadius = (float)(_apertureFrame.Width / 2.0);

            PositionBlades(_apertureFrame.Width, _apertureFrame.Height);
        }

        private void PositionBlades(double width, double height)
        {
            var angularBladeOffset = 360.0d / BladeCount;
            var currentBladeOffset = 0.0d;
            var degreesToRadiansFactor = 0.01745329d;

            var boundedNewValue = Bound(ApertureOpening, 0.0, 1.0);
            var remappedNewValue = Remap(boundedNewValue, 0.5, 1.0);
            var r = width * remappedNewValue;

            for (var i = 0; i < BladeCount; i++)
            {
                var currentBladeX = r * Math.Cos(currentBladeOffset * degreesToRadiansFactor);
                var currentBladeY = r * Math.Sin(currentBladeOffset * degreesToRadiansFactor);

                PositionBlade(_apertureFrameContentGrid.Children[i] as BoxView, currentBladeX, currentBladeY, currentBladeOffset);

                currentBladeOffset += angularBladeOffset;
            }
        }

        private double Remap(double value, double rangeMinimum, double rangeMaximum)
        {
            var range = rangeMaximum - rangeMinimum;

            return rangeMinimum + (range * value);
        }

        private void PositionBlade(BoxView bladeGridChild, double currentBladeX, double currentBladeY, double currentBladeOffset)
        {
            bladeGridChild.BackgroundColor = Color.Gold;
            bladeGridChild.WidthRequest = _apertureFrame.Width; //TBD
            bladeGridChild.HeightRequest = _apertureFrame.Width; //TBD
            bladeGridChild.TranslationX = currentBladeX;
            bladeGridChild.TranslationY = currentBladeY;

            bladeGridChild.Rotation = currentBladeOffset;
        }
    }
}