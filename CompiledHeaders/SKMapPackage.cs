using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapPackage {

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("version", ArgumentSemantic.Retain)]
		string Version { get; set; }

		[Export ("size")]
		long Size { get; set; }

		[Static, Export ("mapPackage"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapPackage.h", Line = 29)]
		SKMapPackage MapPackage { get; }
	}
}
