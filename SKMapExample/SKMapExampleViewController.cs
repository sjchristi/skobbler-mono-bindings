using System;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.Dialog;

using System.Threading.Tasks;

using SKMaps;

namespace SKMapExample
{
	public partial class SKMapExampleViewController : DialogViewController, ISKMapViewDelegate
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

		protected RootElement CreateRootElement()
		{
			return new RootElement ("Main Menu") {
				new Section("Examples") {
					new StringElement("Annotations", () => {
						UIViewController vc = new ExAnnotationViewController();
						this.NavigationController.PushViewController(vc, true);
					}),
					new StringElement("Overlays", () => {
						UIViewController vc = new ExOverlaysViewController();
						this.NavigationController.PushViewController(vc, true);
					}),
					new StringElement("Routing", () => {
						UIViewController vc = new ExRoutingViewController();
						this.NavigationController.PushViewController(vc, true);
					}),
				},
				new Section("Offline Maps") {
					new StringElement("Manage Offline Maps", () => {
						UIViewController vc = new SKMapUtil.SKMapUtilOfflineMapsRootViewController();
						this.NavigationController.PushViewController(vc, true);
					}),
				}
			};
		}

		#region View lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Root = this.CreateRootElement ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			// http://stackoverflow.com/questions/10989473/memory-resource-management-using-monotouch-and-monotouch-dialog

			// HACK: touch the ViewControllers array to refresh it (in case the user popped the nav stack)
			// this is to work around a bug in monotouch (https://bugzilla.xamarin.com/show_bug.cgi?id=1889)
			// where the UINavigationController leaks UIViewControllers when the user pops the nav stack
			int count = this.NavigationController.ViewControllers.Length;
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

