using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapsInitSettings {

		[Export ("connectivityMode")]
		SKConnectivityMode ConnectivityMode { get; set; }

		[Export ("mapDetailLevel")]
		SKMapDetailLevel MapDetailLevel { get; set; }

		[Export ("mapStyle", ArgumentSemantic.Retain)]
		SKMapViewStyle MapStyle { get; set; }

		[Export ("cachesPath", ArgumentSemantic.Retain)]
		string CachesPath { get; set; }

		[Export ("showConsoleLogs")]
		bool ShowConsoleLogs { get; set; }

		[Export ("proxySettings", ArgumentSemantic.Retain)]
		SKProxySettings ProxySettings { get; set; }

		[Static, Export ("mapsInitSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapsInitSettings.h", Line = 45)]
		SKMapsInitSettings MapsInitSettings { get; }
	}
}
