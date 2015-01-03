using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;

using SKMaps;

namespace SKMapExample
{
	public class ExRoutingViewController : UIViewController, ISKMapViewDelegate
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public ExRoutingViewController () : base ()
		{
			this.Title = "Routing";
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

			SKRoutingService.SharedInstance.MapView = mapView;

			SKRoutingService.SharedInstance.DidFinishRouteCalculation += (object sender, RoutingServiceFinishRouteCalculationEventArgs e) => {
				Console.WriteLine ("Route calculated");

				SKRoutingService.SharedInstance.ZoomToRouteWithInsets (UIEdgeInsets.Zero);
				//			NSArray *adviceList = [[SKRoutingService sharedInstance] routeAdviceListWithDistanceFormat:SKDistanceFormatMetric]; // array of SKRouteAdvice
			};

			SKRouteSettings route = new SKRouteSettings ();

			route.StartCoordinate = new CLLocationCoordinate2D(37.447692, -122.166016);
			route.DestinationCoordinate = new CLLocationCoordinate2D(37.437964, -122.141147);
			route.RequestAdvices = true;

			SKRoutingService.SharedInstance.CalculateRoute (route);

			CLLocationCoordinate2D center = new CLLocationCoordinate2D (37.447692, -122.166016);
			// Focus map on center coordinate...
			SKCoordinateRegion visibleRegion;

			visibleRegion.center = center;
			visibleRegion.zoomLevel = 13;

			mapView.VisibleRegion = visibleRegion;
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.CreateMapView();

			UIButton button = new UIButton (UIButtonType.InfoDark);
			RectangleF frame = button.Frame;
			frame.Location = new PointF (0.0f + 5.0f, this.View.Frame.Height - frame.Height - 5.0f);
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

