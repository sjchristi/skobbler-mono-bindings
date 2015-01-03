using System;
using System.Drawing;
using System.Collections.Generic;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreLocation;

using SKMaps;

namespace SKMapExample
{
	public class ExOverlaysViewController : UIViewController, ISKMapViewDelegate
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public ExOverlaysViewController () : base ()
		{
			this.Title = "Overlays";
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

			// Look near Berlin ;-)

			//add a circle overlay
			SKCircle circle = new SKCircle (); 
			circle.CenterCoordinate = new CLLocationCoordinate2D(52.5263, 13.4087);
			circle.Radius = 100.0f;
			circle.FillColor = UIColor.Red;
			circle.StrokeColor = UIColor.Blue; 
			circle.IsMask = false;
			mapView.AddCircle (circle);

			//add a rhombus overlay with dotted border
			CLLocation rhombusVertex1 = new CLLocation (52.5253, 13.4092);
			CLLocation rhombusVertex2 = new CLLocation (52.5233, 13.4077);
			CLLocation rhombusVertex3 = new CLLocation (52.5213, 13.4092);
			CLLocation rhombusVertex4 = new CLLocation (52.5233, 13.4117); 
			SKPolygon rhombus = new SKPolygon ();
			rhombus.Coordinates = (new List<CLLocation>() {rhombusVertex1,rhombusVertex2,rhombusVertex3,rhombusVertex4}).ToArray();
			rhombus.FillColor = UIColor.Blue; 
			rhombus.StrokeColor = UIColor.Green; 
			rhombus.BorderWidth = 5;
			rhombus.BorderDotsSize = 20;
			rhombus.BorderDotsSpacingSize = 5;
			rhombus.IsMask = false;
			mapView.AddPolygon (rhombus);

			//adding a polyline with the same coordinates as the polygon
			SKPolyline polyline = new SKPolyline (); 
			polyline.Coordinates = (new List<CLLocation> () { rhombusVertex1, rhombusVertex2, rhombusVertex3, rhombusVertex4 }).ToArray ();
			polyline.FillColor = UIColor.Red;
			polyline.LineWidth = 10;
			polyline.BackgroundLineWidth = 2;
			polyline.BorderDotsSize = 20;
			polyline.BorderDotsSpacingSize = 5;
			mapView.AddPolyline (polyline);


			CLLocationCoordinate2D center = new CLLocationCoordinate2D (52.5263, 13.4087);
			// Focus map on center coordinate...
			SKCoordinateRegion visibleRegion;

			visibleRegion.center = center;
			visibleRegion.zoomLevel = 15;

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
