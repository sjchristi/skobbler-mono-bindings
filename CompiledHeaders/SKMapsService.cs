using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapsService {

		[Static, Export ("sharedInstance"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapsService.h", Line = 25)]
		SKMapsService SharedInstance { get; }

		[Export ("initializeSKMapsWithAPIKey:settings:")]
		void InitializeSKMapsWithAPIKey (string apiKey, SKMapsInitSettings initSettings);

		[Export ("packagesManager", ArgumentSemantic.Retain)]
		SKOfflinePackagesManager PackagesManager { get; }

		[Export ("tilesCacheManager", ArgumentSemantic.Retain)]
		SKTilesCacheManager TilesCacheManager { get; }

		[Export ("mapsVersioningManager", ArgumentSemantic.Retain)]
		SKMapsVersioningManager MapsVersioningManager { get; }

		[Export ("connectivityMode")]
		SKConnectivityMode ConnectivityMode { get; set; }

		[Export ("proxySettings", ArgumentSemantic.Retain)]
		SKProxySettings ProxySettings { get; set; }

		[Export ("apiKey", ArgumentSemantic.Retain)]
		string ApiKey { get; }

		[Export ("obfuscatedAPIKey", ArgumentSemantic.Retain)]
		string ObfuscatedAPIKey { get; }

		[Export ("frameworkVersion", ArgumentSemantic.Retain)]
		string FrameworkVersion { get; }
	}
}
