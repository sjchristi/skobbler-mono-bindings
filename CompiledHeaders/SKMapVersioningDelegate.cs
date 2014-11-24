using System;

using MonoTouch.Foundation;

namespace SKMap {

	[Model, BaseType (typeof (NSObject))]
	public partial interface SKMapVersioningDelegate {

		[Export ("mapsVersioningManager:detectedNewAvailableMapVersion:currentMapVersion:")]
		void DetectedNewAvailableMapVersion (SKMapsVersioningManager versioningManager, string latestMapVersion, string currentMapVersion);

		[Export ("mapsVersioningManager:loadedWithOfflinePackages:updatablePackages:")]
		void LoadedWithOfflinePackages (SKMapsVersioningManager versioningManager, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapVersioningDelegate.h", Line = 30)] NSObject [] packages, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapVersioningDelegate.h", Line = 30)] NSObject [] updatablePackages);

		[Export ("mapsVersioningManager:loadedWithMapVersion:")]
		void LoadedWithMapVersion (SKMapsVersioningManager versioningManager, string currentMapVersion);

		[Export ("mapsVersioningManagerLoadedMetadata:")]
		void  (SKMapsVersioningManager versioningManager);
	}
}
