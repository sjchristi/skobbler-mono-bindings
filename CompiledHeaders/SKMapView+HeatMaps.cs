using System;

using MonoTouch.Foundation;

namespace SKMap {

	[Category, BaseType (typeof (SKMapView))]
	public partial interface HeatMaps_SKMapView {

		[Export ("showHeatMapWithPOIType:")]
		void ShowHeatMapWithPOIType ([Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapView+HeatMaps.h", Line = 17)] NSObject [] poiTypes);

		[Export ("clearHeatMap")]
		void ClearHeatMap ();
	}
}
