using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKDetectedPOI {

		[Export ("poiID")]
		int PoiID { get; set; }

		[Export ("distance")]
		int Distance { get; set; }

		[Export ("type")]
		SKTrackablePOIType Type { get; set; }

		[Export ("referenceDistance")]
		int ReferenceDistance { get; set; }

		[Static, Export ("detectedPOI"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKDetectedPOI.h", Line = 33)]
		SKDetectedPOI DetectedPOI { get; }
	}
}
