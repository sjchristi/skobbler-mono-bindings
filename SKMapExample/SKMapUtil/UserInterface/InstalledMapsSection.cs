using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Collections;
using System.Collections.Generic;

namespace SKMapUtil
{
	public class InstalledMapsSection : Section
	{
		protected WeakReference<DialogViewController> _parentVC;
		protected Dictionary<string, OfflineMapInfoElement> _installedMaps;
		public InstalledMapsSection (DialogViewController parentVC) : base("Installed Maps")
		{
			this._parentVC = new WeakReference<DialogViewController>(parentVC);
			this._installedMaps = new Dictionary<string, OfflineMapInfoElement> (64);
			this.ReloadInstalledMaps();

			OfflineMapInfo.MapInfoInstalled += this.OnMapInfoInstalled;
			OfflineMapInfo.MapInfoUninstalled += this.OnMapInfoUninstalled;
		}

		protected DialogViewController ParentVC {
			get {
				DialogViewController vc;

				this._parentVC.TryGetTarget (out vc);

				return vc;
			}
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);

			OfflineMapInfo.MapInfoInstalled -= this.OnMapInfoInstalled;
			OfflineMapInfo.MapInfoUninstalled -= this.OnMapInfoUninstalled;
		}

		public void OnMapInfoInstalled(object sender, OfflineMapInfo info)
		{
			// SAMCTODO:  TEST
			this.ParentVC.BeginInvokeOnMainThread (() => {
				this.ReloadInstalledMaps ();
			});
		}

		public void OnMapInfoUninstalled(object sender, OfflineMapInfo info)
		{
			// SAMCTODO:  TEST
			this.ParentVC.BeginInvokeOnMainThread (() => {
				this.ReloadInstalledMaps ();
			});
		}

		public void ReloadInstalledMaps()
		{
			IEnumerable<OfflineMapInfo> installedMaps =  OfflineMapInfo.InstalledMaps;
			HashSet<string> installedMapCodes = new HashSet<string> ();
			HashSet<string> removedMapCodes = new HashSet<string> ();

			foreach (var m in installedMaps) {
				if (!this._installedMaps.ContainsKey (m.code)) {
					OfflineMapInfoElement e = new OfflineMapInfoElement (this.ParentVC, m);
					this._installedMaps.Add (m.code, e);
					this.Add (e);
				}

				installedMapCodes.Add (m.code);
			}

			foreach (var k in this._installedMaps) {
				if (!installedMapCodes.Contains (k.Key)) {
					removedMapCodes.Add (k.Key);
				}
			}

			foreach (var code in removedMapCodes) {
				Element e = this._installedMaps [code];
				this.Remove (e);
				this._installedMaps.Remove (code);
			}
		}
	}
}

