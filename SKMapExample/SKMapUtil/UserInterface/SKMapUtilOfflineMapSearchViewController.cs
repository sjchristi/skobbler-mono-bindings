using System;
using UIKit;
using MonoTouch.Dialog;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

namespace SKMapUtil
{
	public class SKMapUtilOfflineMapSearchViewController : DialogViewController
	{
		public delegate bool PackageTypeFilter(OfflineMapInfo.PackageType packageType);

		PackageTypeFilter _packageTypeFilter;

		public SKMapUtilOfflineMapSearchViewController (PackageTypeFilter filter) : base(UITableViewStyle.Plain, null, true)
		{
			this.EnableSearch = true;
			this._packageTypeFilter = filter;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Root = new RootElement ("Offline Map Search");

			this.LoadMapSections();
		}

		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
		}

		protected void LoadMapSections()
		{
			this.Root.Clear ();

			List<Section> mapSections = new List<Section> (52);

			List<OfflineMapInfo> mapInfos = OfflineMapInfo.OfflineMapPackages.Where ((OfflineMapInfo info) => {
				return (info.file != null && this._packageTypeFilter.Invoke(info.type));
			}).ToList();

			string letter = null;
			Section currentSection = null;

			foreach (OfflineMapInfo i in mapInfos) {
				string name = i.LocalizedDescription;
				string firstLetter = name.Substring (0, 1).ToUpperInvariant();
				if (string.Compare(firstLetter, letter, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) != 0) {
					letter = firstLetter;
					currentSection = new Section (firstLetter);
					mapSections.Add (currentSection);
				}
				currentSection.Add (new OfflineMapInfoElement (this, i));
			}

			this.Root.Add (mapSections);
		}

		public string[] GetSectionTitles ()
		{
			return (
				from section in Root
				where !String.IsNullOrEmpty(section.Caption)
				select section.Caption.Substring(0,1)
			).ToArray ();
		}

		class IndexedSource : Source {
			SKMapUtilOfflineMapSearchViewController parent;

			public IndexedSource (SKMapUtilOfflineMapSearchViewController parent) : base (parent)
			{
				this.parent = parent;
			}

			public override String[] SectionIndexTitles (UITableView tableView)
			{
				var j = parent.GetSectionTitles ();
				return j;
			}
		}

		class SizingIndexedSource : Source {
			SKMapUtilOfflineMapSearchViewController parent;

			public SizingIndexedSource (SKMapUtilOfflineMapSearchViewController parent) : base (parent)
			{
				this.parent = parent;
			}

			public override String[] SectionIndexTitles (UITableView tableView)
			{
				var j = parent.GetSectionTitles ();
				return j;
			}
		}

		public override Source CreateSizingSource (bool unevenRows)
		{
			if (unevenRows)
				return new SizingIndexedSource (this);
			else
				return new IndexedSource (this);
		}
	}
}

