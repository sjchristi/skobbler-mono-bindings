using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapCustomPOI {

		[Export ("identifier")]
		int Identifier { get; set; }

		[Export ("coordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Export ("type")]
		SKPOIType Type { get; set; }

		[Export ("categoryID")]
		SKPOICategory CategoryID { get; set; }

		[Export ("minZoomLevel")]
		int MinZoomLevel { get; set; }

		[Static, Export ("mapCustomPOI"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapCustomPOI.h", Line = 37)]
		SKMapCustomPOI MapCustomPOI { get; }
	}
}
