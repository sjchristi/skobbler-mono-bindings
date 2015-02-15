using System;
using UIKit;
using MonoTouch.Dialog;

namespace SKMapUtil
{
	public class SKMapUtilOfflineMapInfoViewController : DialogViewController
	{
		OfflineMapInfo _mapInfo;

		public SKMapUtilOfflineMapInfoViewController (OfflineMapInfo mapInfo) : base(UITableViewStyle.Plain, null, true)
		{
			this._mapInfo = mapInfo;
			this.EnableSearch = true;
			this.AutoHideSearch = (mapInfo.children.Count < 8);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Section s = new Section ();

			foreach (var c in this._mapInfo.children) {
				OfflineMapInfoElement e = new OfflineMapInfoElement (this, c);
				s.Add (e);
			}

			RootElement root = new RootElement (this._mapInfo.LocalizedName) {
				s
			};

			this.Root = root;
		}
	}
}

