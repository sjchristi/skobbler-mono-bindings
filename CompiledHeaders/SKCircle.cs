using System;

namespace SKMap {

	[BaseType (typeof (SKOverlay))]
	public partial interface SKCircle {

		[Export ("centerCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D CenterCoordinate { get; set; }

		[Export ("radius")]
		float Radius { get; set; }

		[Export ("strokeColor", ArgumentSemantic.Retain)]
		UIColor StrokeColor { get; set; }

		[Export ("borderWidth")]
		int BorderWidth { get; set; }

		[Export ("isMask")]
		bool IsMask { get; set; }

		[Export ("maskedObjectScale")]
		float MaskedObjectScale { get; set; }

		[Export ("numberOfPoints")]
		int NumberOfPoints { get; set; }

		[Static, Export ("circle"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKCircle.h", Line = 47)]
		SKCircle Circle { get; }
	}
}
