using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApertureView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class ApertureView : RelativeLayout
    {
        private const int BladeCount = 6;

        private readonly Frame _apertureFrame;
        private readonly Grid _apertureFrameContentGrid;
        private readonly Grid _apertureFrameBladeGrid;

        public ApertureView()
        {
            _apertureFrame = new Frame
            {
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

            _apertureFrameBladeGrid = new Grid
            {
                BackgroundColor = Color.Transparent
            };
            _apertureFrameContentGrid.Children.Add(_apertureFrameBladeGrid);

            for (var i = 0; i < BladeCount; i++)
            {
                _apertureFrameBladeGrid.Children.Add(new BoxView { BackgroundColor = ApertureColor, Margin = 0 });
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
            Color.Gold
        );

        public Color ApertureColor { get => (Color)GetValue(ApertureColorProperty); set => SetValue(ApertureColorProperty, value); }

        public static readonly BindableProperty ContentViewProperty = BindableProperty.Create(
            nameof(ContentView),
            typeof(View),
            typeof(ApertureView),
            null,
            propertyChanged: OnContentViewChanged
        );

        private static void OnContentViewChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ApertureView apertureView)
            {
                if (oldvalue is View oldChildView && apertureView._apertureFrameContentGrid.Children.Contains(oldChildView))
                {
                    apertureView._apertureFrameContentGrid.Children.Remove(oldChildView);
                }

                if (newvalue is View newContentView)
                {
                    apertureView._apertureFrameContentGrid.Children.Insert(0, newContentView);
                    //apertureView._apertureFrameContentGrid.Children.Insert(0, new Frame
                    //{
                    //    WidthRequest = 22,
                    //    HeightRequest = 22,
                    //    BackgroundColor = Color.Aqua,
                    //    VerticalOptions = LayoutOptions.Center,
                    //    HorizontalOptions = LayoutOptions.Center
                    //});
                    //apertureView.InvalidateLayout();
                }
            }
        }

        public View ContentView { get => (View)GetValue(ContentViewProperty); set => SetValue(ContentViewProperty, value); }

        private void ApertureFrameOnSizeChanged(object sender, EventArgs e)
        {
            Redraw();
        }

        private void Redraw()
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

                PositionBlade(_apertureFrameBladeGrid.Children[i] as BoxView, currentBladeX, currentBladeY, currentBladeOffset);

                currentBladeOffset += angularBladeOffset;
            }
        }

        private void PositionBlade(BoxView bladeGridChild, double currentBladeX, double currentBladeY, double currentBladeOffset)
        {
            if (bladeGridChild == null)
            {
                return;
            }

            bladeGridChild.BackgroundColor = ApertureColor;
            bladeGridChild.WidthRequest = _apertureFrame.Width; //TBD
            bladeGridChild.HeightRequest = _apertureFrame.Width; //TBD
            bladeGridChild.TranslationX = currentBladeX;
            bladeGridChild.TranslationY = currentBladeY;

            bladeGridChild.Rotation = currentBladeOffset;
        }

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

        private double Remap(double value, double rangeMinimum, double rangeMaximum)
        {
            var range = rangeMaximum - rangeMinimum;

            return rangeMinimum + (range * value);
        }
    }
}