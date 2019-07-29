## Aperture

A Xamarin.Forms custom control that shows either the background or a child view using an aperture-like mask.

It is implemented exclusively in Xmarin.Forms and hence should run on all platforms where Xamarin.Forms is supported. Also, it is tiny. 
Would probably look great on watches :D

Get it from NuGet: [![NuGet](https://img.shields.io/badge/Aperture-NuGet-brightgreen.svg?style=popout)](https://www.nuget.org/packages/ApertureView/)

Here's a quick rundown of the control's features:

* Specify the aperture opening, where 0.0 corresponds to closed and 1.0 to fully open.
* Choose an aperture color.
* OPTIONAL: provide a child view to be shown, instead of the control's background

## Known Limitations

On iOS you might have jittery movement.
Just add thesetwo lines to your App.xaml

             xmlns:ios="clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core"
             ios:Application.HandleControlUpdatesOnMainThread="true"

Here's a gif of the included sample app:

![Sample](media/ApertureSampleGif2.gif)



As usual you can do everything you want with my code. Have fun!

Have a question?  Tweet at me: https://twitter.com/MunteanMarius
