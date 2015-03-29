# SKMap Bindings for Xamarin

This is a set of C# bindings for [Skobbler's map SDK](http://developer.skobbler.com/getting-started/ios).  I'd described the current stage as demo.  We have the bindings working in simple example projects but it hasn't shipped with a real project yet.  We're working hard (but a bit slowly due to other factors) to expose the entire SDK, and would eventually like to provide a single iOS/Android SDK for monotouch (if possible).

We can definitely use help, so please drop us a line if you would like to contribute, or even if there is a specific feature we aren't exposing yet that you want to use.

## Installation

This repo includes version 2.4 of the Skobbler SDK.  Due to some issues with the original SDK, we've had to hack it a bit to get it to work.  Specifically, the original SDK included a symbol for 'main' which causes linking errors when you try to use it in your Xamarin App.  If you'd like to use your own/newer version of the library, there are instructions below on how to fix it.  You can also just raise an issue if we aren't on the latest version of the library and we'll upgrade.

To install, just grab a copy of this repo.  Open the solution included and run one of the samples to verify that everything works.  You can use it in other applications just by adding a reference to the SKMaps.dll file that the project will build.

To use this in your own applications, you'll need an [API Key for the Skobbler SDK](http://developer.skobbler.com).

## Usage

### Using SKMaps.dll with your own iOS App

Simply add SKMaps.dll to your project's References and you should be ready to roll!

### Initialize the SDK

Perform one-time initialization of the Skobbler SDK.  In AppDelegate.cs / FinishedLaunching:, 

	SKMapsInitSettings initSettings = SKMapsInitSettings.MapsInitSettings;
	// whatwever settings you need to change here.....
	initSettings.ConnectivityMode = SKConnectivityMode.Offline;
	initSettings.MapDetailLevel = SKMapDetailLevel.Full;
	SKMap.SKMapsService.InitializeSKMapsWithAPIKey("YOUR API KEY HERE");

### Create a Map View

In your view controller / ViewDidLoad method:

	SKMapView mapView = new SKMapView (this.View.Frame);
	this.View.AddSubview (mapView);

### Add Event Handlers

You can start adding logic to the view by adding event handlers:

	mapView.DidSelectAnnotation += delegate(object sender, MapAnnotationEventArgs e) {
		Console.WriteLine("Tapped {0}", e.Annotation.Identifier);

		mapView.CalloutView.TitleLabel.Text = String.Format("Annotation {0} Title", e.Annotation.Identifier);
		mapView.CalloutView.SubtitleLabel.Text = String.Format("Annoation {0} Subtitle Label", e.Annotation.Identifier);

		mapView.ShowCalloutForAnnotation(e.Annotation, new PointF(0.0f, 42.0f), true);
	};

	mapView.DidTapAtCoordinate += delegate(object sender, MapLocationEventArgs e) {
		Console.WriteLine("Tapped at {0}, {1}", e.Coordinate.Latitude, e.Coordinate.Longitude);
		mapView.HideCallout();
	};

	mapView.CalloutView.DidTapLeftButton += delegate(object sender, CalloutViewLeftButtonTappedEventArgs e) {
		Console.WriteLine("Tapped Left Button");
	};

	mapView.CalloutView.DidTapRightButton += delegate(object sender, CalloutViewRightButtonTappedEventArgs e) {
		Console.WriteLine("Tapped Right Button");
	};

### Annotations

Add some annotations to your map:

	SKAnnotation annotation = SKAnnotation.Annotation;
	annotation.Identifier = 10;
	annotation.ImagePath = NSBundle.MainBundle.PathForResource ("customImage", "png");
	annotation.ImageSize = 64;
	annotation.Location = new CLLocationCoordinate2D (0, -80);
	annotation.MinZoomLevel = 0;
	mapView.AddAnnotation (annotation, SKAnimationSettings.DefaultAnimationSettings);

Show the Callout View for an Annotation:

	mapView.ShowCalloutForAnnotation (annotation, new PointF (0.0f, 42.0f), false);

### Focus the Map on a Specific Spot

Set the 'visible region' for the map:

	SKCoordinateRegion visibleRegion;

	visibleRegion.center = new CLLocationCoordinate2D (0, -80);
	visibleRegion.zoomLevel = 12;

	mapView.VisibleRegion = visibleRegion;

## Using your own version of the Skobbler Framework

Copy the framework (SKMaps.framework) into the ./SKMaps/ folder.

You'll need ruby; if you're on a mac, you should have it.  Fire up terminal and run the fix_lib.rb script, which will attempt to fix a few issues with symbols in the library:

```shell
$ cd SKMaps/
$ ruby fix_lib.rb
```

Now you should be able to open the SKMaps.sln file in Monotouch and re-compile the library (use release mode) and it will generate a new SKMaps.dll using the new version of the framework.

## Known Issues

### Custom Marker Images

If you want to use custom images for your markers, here are a couple things to keep in mind:

- they must be a power-of-2 size
- if they are PNG files, you need to turn pngcrush off; you can do this in Project Options -> iOS Build -> General -> Packaging Options -> Optimize PNG Image files for iOS -- make sure this option is disabled.

There may be some spew into the console if you don't do the above, but otherwise the failures are silent (no crashes or exceptions thrown).  If you notice errors like this in the console, it's likely a pngcrush issue as described above:

	libpng error: CgBI: unknown critical chunk

### C++ Symbols Linking Errors

When I was first setting up my personal project to use the SKMaps.dll, I ran into a bunch of issues where it would give linking errors on all sorts of C++ code.  I've looked through the history and can't figure out what I did to fix it exactly (maybe it was just changes to the library), but if you run into this issue, please contact me and I'll walk you through it.

## @TODO
- [ ] Support for downloadable offline maps
- [ ] Encompass entire Skobbler API
- [ ] Unify Android/iOS interfaces in MonoTouch

## License

The bindings are licensed under the MIT X11 license:

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

## Contributing

1. Fork it
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create new Pull Request

## Credits

* [Skobbler](http://developer.skobbler.com) - Thank's for making an amazing map SDK and some of the best iPhone Apps that incorporate OSM maps.
* [Open Street Maps](http://openstreetmap.com) - The amazing community making the maps that we've all come to love.
* [sjchristi](https://github.com/sjchristi) - The guy trying to glue this together
