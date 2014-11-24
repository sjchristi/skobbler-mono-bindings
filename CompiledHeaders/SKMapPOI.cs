using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapPOI {

		[Export ("category")]
		SKPOICategory Category { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("coordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Export ("parentSearchResults", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapPOI.h", Line = 31)]
		NSObject [] ParentSearchResults { get; set; }

		[Static, Export ("mapPOI"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapPOI.h", Line = 35)]
		SKMapPOI MapPOI { get; }
	}
}
