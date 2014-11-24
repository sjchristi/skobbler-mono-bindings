using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapsVersioningManager {

		[Export ("delegate", ArgumentSemantic.Assign)]
		SKMapVersioningDelegate Delegate { get; set; }

		[Export ("currentMapVersion", ArgumentSemantic.Retain)]
		string CurrentMapVersion { get; }

		[Export ("localMapVersions", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapsVersioningManager.h", Line = 35)]
		NSObject [] LocalMapVersions { get; }

		[Export ("availableMapVersions", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapsVersioningManager.h", Line = 41)]
		NSObject [] AvailableMapVersions { get; }

		[Export ("updateToVersion:")]
		bool UpdateToVersion (string version);

		[Export ("checkNewVersion")]
		void CheckNewVersion ();

		[Export ("metaDataDownloadedStatus")]
		SKMetaDataDownloadStatus MetaDataDownloadedStatus { get; }

		[Export ("initWithConnectivityMode:")]
		IntPtr Constructor (SKConnectivityMode connectivityMode);
	}
}
