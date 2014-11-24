using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKCoordinate {

		[Export ("latitude")]
		double Latitude { get; set; }

		[Export ("longitude")]
		double Longitude { get; set; }

		[Export ("altitude")]
		double Altitude { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("description", ArgumentSemantic.Retain)]
		string Description { get; set; }

		[Export ("time", ArgumentSemantic.Retain)]
		NSDate Time { get; set; }

		[Static, Export ("coordinate"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKCoordinate.h", Line = 40)]
		SKCoordinate Coordinate { get; }
	}
}
