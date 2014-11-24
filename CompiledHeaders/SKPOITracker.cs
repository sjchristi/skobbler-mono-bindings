using System;

using MonoTouch.Foundation;

namespace SKMap {

	[Model, BaseType (typeof (NSObject))]
	public partial interface SKPOITrackerDataSource {

		[Export ("poiTracker:trackablePOIsAroundLocation:inRadius:withType:"), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOITracker.h", Line = 25)]
		NSObject [] PoiTracker (SKPOITracker poiTracker, CLLocationCoordinate2D location, int radius, int poiType);
	}

	[Model, BaseType (typeof (NSObject))]
	public partial interface SKPOITrackerDelegate {

		[Export ("poiTracker:didDectectPOIs:withType:")]
		void DidDectectPOIs (SKPOITracker poiTracker, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOITracker.h", Line = 38)] NSObject [] detectedPOIs, int type);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKPOITracker {

		[Export ("dataSource", ArgumentSemantic.Assign)]
		SKPOITrackerDataSource DataSource { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)]
		SKPOITrackerDelegate Delegate { get; set; }

		[Export ("startPOITrackerWithRadius:refreshMargin:forPOITypes:")]
		bool StartPOITrackerWithRadius (int radius, double refreshMargin, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOITracker.h", Line = 62)] NSObject [] poiTypes);

		[Export ("stopPOITracker"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOITracker.h", Line = 67)]
		bool StopPOITracker { get; }

		[Export ("setRule:forPOIType:")]
		void SetRule (SKTrackablePOIRule rule, SKTrackablePOIType type);

		[Export ("ruleForPOIType:")]
		SKTrackablePOIRule RuleForPOIType (SKTrackablePOIType type);

		[Export ("setWarningRulesForType:withFilePath:")]
		void SetWarningRulesForType (SKTrackablePOIType type, string filePath);

		[Export ("forceUpdateTrackedPOIs")]
		void ForceUpdateTrackedPOIs ();

		[Export ("trackablePOIsOnRoute:"), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOITracker.h", Line = 100)]
		NSObject [] TrackablePOIsOnRoute ([Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPOITracker.h", Line = 100)] NSObject [] providedPOIs);
	}
}
