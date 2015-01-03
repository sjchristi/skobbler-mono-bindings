using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace SKMapUtil
{
	public class NearbyOfflineMapsSection : Section
	{
		protected WeakReference<DialogViewController> _parentVC;
		public NearbyOfflineMapsSection (DialogViewController parentVC, CLLocationCoordinate2D nearby) : base("Nearby Places")
		{
			this._parentVC = new WeakReference<DialogViewController>(parentVC);
			this.LoadMapsNearby (nearby);
		}

		protected DialogViewController ParentVC {
			get {
				DialogViewController vc;

				this._parentVC.TryGetTarget (out vc);

				return vc;
			}
		}

		public void LoadMapsNearby(CLLocationCoordinate2D point)
		{
			this.Clear ();

			List<OfflineMapInfo> mapInfos = OfflineMapInfo.OfflineMapPackages.Where ((OfflineMapInfo info) => {
				bool filtered = false;

				if (info.bbox != null)
				{
					double distance = info.bbox.Distance(point.Latitude, point.Longitude);
					// if < 100 kilometers away...
					if (distance < 100)
					{
						filtered = true;
					}
				}

				return filtered;
			}).ToList();

			mapInfos.Sort ((OfflineMapInfo x, OfflineMapInfo y) => {
				double d1 = x.bbox.DistanceCenter(point.Latitude, point.Longitude);
				double d2 = y.bbox.DistanceCenter(point.Latitude, point.Longitude);

				if (d1 < d2) {
					return -1;
				} else if (d1 > d2) {
					return 1;
				}

				// distances are equal
				return x.LocalizedDescription.CompareTo(y.LocalizedDescription);
			});

			foreach (var m in mapInfos) {
				this.Add (new OfflineMapInfoElement (this.ParentVC, m));
			}
		}
	}
}

