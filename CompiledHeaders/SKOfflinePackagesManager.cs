using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKOfflinePackagesManager {

		[Export ("installedOfflineMapPackages", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKOfflinePackagesManager.h", Line = 20)]
		NSObject [] InstalledOfflineMapPackages { get; }

		[Export ("mapsDownloadBasePath", ArgumentSemantic.Retain)]
		string MapsDownloadBasePath { get; }

		[Export ("addOfflineMapPackageNamed:inContainingFolderPath:")]
		SKAddPackageResult AddOfflineMapPackageNamed (string packageName, string containingFolderPath);

		[Export ("deleteOfflineMapPackageNamed:")]
		bool DeleteOfflineMapPackageNamed (string packageName);

		[Export ("validateMapFileAtPath:")]
		bool ValidateMapFileAtPath (string path);

		[Export ("mapsXMLURLForVersion:")]
		string MapsXMLURLForVersion (string version);

		[Export ("mapsJSONURLForVersion:")]
		string MapsJSONURLForVersion (string version);

		[Export ("downloadInfoForPackageWithCode:")]
		SKMapPackageDownloadInfo DownloadInfoForPackageWithCode (string packageCode);
	}
}
