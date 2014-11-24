using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

using System.Threading.Tasks;

using SKMaps;

namespace SKMapExample
{
	public partial class SKMapExampleViewController : UIViewController, ISKMapViewDelegate
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public SKMapExampleViewController (IntPtr handle) : base (handle)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			SKMapView mapView = new SKMapView (this.View.Frame);
			this.View.AddSubview (mapView);

			mapView.CalloutViewForAnnotation += delegate {
				return null;
			};

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

			CLLocationCoordinate2D center = new CLLocationCoordinate2D (0, -80);

			SKAnnotation annotation = SKAnnotation.Annotation;
//			annotation.AnnotationType = SKAnnotationType.Marker;
			annotation.Identifier = 10;
			annotation.ImagePath = NSBundle.MainBundle.PathForResource ("customImage", "png");
			annotation.ImageSize = 64;
			annotation.Location = center;
			annotation.MinZoomLevel = 0;
			mapView.AddAnnotation (annotation, SKAnimationSettings.DefaultAnimationSettings);

			mapView.ShowCalloutForAnnotation (annotation, new PointF (0.0f, 42.0f), false);


			// Focus map on center coordinate...
			SKCoordinateRegion visibleRegion;

			visibleRegion.center = center;
			visibleRegion.zoomLevel = 12;

			mapView.VisibleRegion = visibleRegion;
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);
		}

		public override void ViewDidDisappear (bool animated)
		{
			base.ViewDidDisappear (animated);
		}

		#endregion
	}
}

