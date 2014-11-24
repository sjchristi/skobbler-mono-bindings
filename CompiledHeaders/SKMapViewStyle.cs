using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapViewStyle {

		[Export ("styleID")]
		int StyleID { get; set; }

		[Export ("resourcesFolderName", ArgumentSemantic.Retain)]
		string ResourcesFolderName { get; set; }

		[Export ("styleFileName", ArgumentSemantic.Retain)]
		string StyleFileName { get; set; }

		[Export ("resourcesPath", ArgumentSemantic.Retain)]
		string ResourcesPath { get; set; }

		[Static, Export ("mapViewStyle"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapViewStyle.h", Line = 34)]
		SKMapViewStyle MapViewStyle { get; }
	}
}
