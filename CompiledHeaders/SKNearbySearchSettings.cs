using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKNearbySearchSettings {

		[Export ("coordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Export ("radius")]
		int Radius { get; set; }

		[Export ("searchTerm", ArgumentSemantic.Retain)]
		string SearchTerm { get; set; }

		[Export ("searchMode")]
		SKSearchMode SearchMode { get; set; }

		[Export ("searchType")]
		SKSearchType SearchType { get; set; }

		[Export ("searchResultSortType")]
		SKSearchResultSortType SearchResultSortType { get; set; }

		[Export ("searchCategories", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKNearbySearchSettings.h", Line = 42)]
		NSObject [] SearchCategories { get; set; }

		[Static, Export ("nearbySearchSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKNearbySearchSettings.h", Line = 46)]
		SKNearbySearchSettings NearbySearchSettings { get; }
	}
}
