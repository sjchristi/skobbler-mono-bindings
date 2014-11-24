using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKPOICluster {

		[Export ("customPOIList", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOICluster.h", Line = 16)]
		NSObject [] CustomPOIList { get; set; }

		[Export ("mapPOIList", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOICluster.h", Line = 20)]
		NSObject [] MapPOIList { get; set; }

		[Export ("isCustomPOICluster")]
		bool IsCustomPOICluster { get; set; }
	}
}
