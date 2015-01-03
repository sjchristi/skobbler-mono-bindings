using System;
using MonoTouch.Dialog;
using SKMaps;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;
using System.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;

using MonoTouch.CoreLocation;

namespace SKMapUtil
{
	public class SKMapUtilOfflineMapsRootViewController : DialogViewController
	{
		protected OfflineMapInfo currentMapInfo;
		protected NearbyOfflineMapsSection _nearbyMapsSection;
		protected InstalledMapsSection _installedMapsSection;

		public SKMapUtilOfflineMapsRootViewController () : base(UITableViewStyle.Grouped, null, true)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Root = new RootElement ("Offline Maps") {
				new Section("Loading...") {
					new ActivityElement()
				}
			};

			OfflineMapInfo.LoadWorld (
				(bool loaded) => {
					string caption = null;

					if (loaded) {
						caption = "Download Complete!";
					} else {
						caption = "Download Error.";
					}

					this.BeginInvokeOnMainThread (() => {
						if (loaded) {
							this.LoadSections();
						} else {
							DialogExtensions.ShowAlert("Oops", "There was an error downloading the map metadata.  Make sure you are connected to the internet and try again.", "OK");
						}
					});
				},
				(double progress) => {
					this.BeginInvokeOnMainThread (() => {
//						this.downloadInfoElement.Caption = String.Format ("Downloading %{0}", (progress * 100.0).ToString ("N1"));
//						this.downloadInfoElement.GetImmediateRootElement ().Reload (this.downloadInfoElement, UITableViewRowAnimation.Automatic);
					});
				}
			);
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if (this._installedMapsSection != null) {
				this._installedMapsSection.ReloadInstalledMaps ();
			}
		}

		protected void LoadSections()
		{
			this.Root.Clear ();

			List<Section> sections = new List<Section> (10);

			sections.Add (new Section ("Browse Available Maps") {
				new StyledStringElement("Browse Continents", () => {
					SKMapUtilOfflineMapInfoViewController vc = new SKMapUtilOfflineMapInfoViewController(OfflineMapInfo.World);
					this.NavigationController.PushViewController(vc, true);
				}),
				new StyledStringElement("Search Countries/States", () => {
					SKMapUtilOfflineMapSearchViewController vc = new SKMapUtilOfflineMapSearchViewController((OfflineMapInfo.PackageType packageType) => {
						return packageType != OfflineMapInfo.PackageType.City;
					});
					this.NavigationController.PushViewController(vc, true);
				}),
				new StyledStringElement("Search Cities", () => {
					SKMapUtilOfflineMapSearchViewController vc = new SKMapUtilOfflineMapSearchViewController((OfflineMapInfo.PackageType packageType) => {
						return packageType == OfflineMapInfo.PackageType.City;
					});
					this.NavigationController.PushViewController(vc, true);
				}),
			});

			// TODO:  Drive based on location from device...
			CLLocationCoordinate2D nearby = new CLLocationCoordinate2D (0, -80); 

			this._nearbyMapsSection = new NearbyOfflineMapsSection (this, nearby);
			this._installedMapsSection = new InstalledMapsSection (this);

			sections.Add( this._nearbyMapsSection );
			sections.Add (this._installedMapsSection);

			this.Root.Add (sections);
		}
	}
}

