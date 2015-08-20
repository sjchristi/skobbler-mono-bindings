using System;
using CoreGraphics;

using UIKit;
using Foundation;
using CoreLocation;

using SKMaps;

namespace SKMapExample
{
	public class ExAnnotationViewController : UIViewController, ISKMapViewDelegate
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public ExAnnotationViewController () : base ()
		{
			this.Title = "Annotations";
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}


		protected void CreateMapView()
		{
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

				mapView.ShowCalloutForAnnotation(e.Annotation, new CGPoint(0.0f, 42.0f), true);
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
			annotation.AnnotationView = new SKAnnotationView (new UIImageView(UIImage.FromFile("customImage.png")), "imageView");
			annotation.Location = center;
			annotation.MinZoomLevel = 0;
			mapView.AddAnnotation (annotation, SKAnimationSettings.DefaultAnimationSettings);

			mapView.ShowCalloutForAnnotation (annotation, new CGPoint (0.0f, 42.0f), false);


			// Focus map on center coordinate...
			SKCoordinateRegion visibleRegion;

			visibleRegion.center = center;
			visibleRegion.zoomLevel = 12;

			mapView.VisibleRegion = visibleRegion;
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.CreateMapView();

			UIButton button = new UIButton (UIButtonType.InfoDark);
			CGRect frame = button.Frame;
			frame.Location = new CGPoint (0.0f + 5.0f, this.View.Frame.Height - frame.Height - 5.0f);
			button.Frame = frame;
			button.TouchUpInside += (sender, e) => {
				UIViewController vc = new SKMapUtil.SKMapUtilOfflineMapsRootViewController();
				this.NavigationController.PushViewController(vc, true);
			};
			this.View.AddSubview (button);
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
