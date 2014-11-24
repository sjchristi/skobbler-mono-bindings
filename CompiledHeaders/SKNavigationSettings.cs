using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKNavigationSettings {

		[Export ("distanceFormat")]
		SKDistanceFormat DistanceFormat { get; set; }

		[Export ("positionerVerticalAlignment")]
		float PositionerVerticalAlignment { get; set; }

		[Export ("positionerHorizontalAlignment")]
		float PositionerHorizontalAlignment { get; set; }

		[Export ("speedWarningThresholdInCity")]
		double SpeedWarningThresholdInCity { get; set; }

		[Export ("speedWarningThresholdOutsideCity")]
		double SpeedWarningThresholdOutsideCity { get; set; }

		[Export ("enableSplitRoute")]
		bool EnableSplitRoute { get; set; }

		[Export ("trail", ArgumentSemantic.Retain)]
		SKTrailSettings Trail { get; set; }

		[Export ("navigationType")]
		SKNavigationType NavigationType { get; set; }

		[Export ("showRealGPSPositions")]
		bool ShowRealGPSPositions { get; set; }

		[Static, Export ("navigationSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKNavigationSettings.h", Line = 61)]
		SKNavigationSettings NavigationSettings { get; }
	}
}
