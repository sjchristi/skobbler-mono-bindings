using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (SKOverlay))]
	public partial interface SKPolygon {

		[Export ("coordinates", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPolygon.h", Line = 19)]
		NSObject [] Coordinates { get; set; }

		[Export ("strokeColor", ArgumentSemantic.Retain)]
		UIColor StrokeColor { get; set; }

		[Export ("borderWidth")]
		int BorderWidth { get; set; }

		[Export ("isMask")]
		bool IsMask { get; set; }

		[Export ("maskedObjectScale")]
		float MaskedObjectScale { get; set; }

		[Static, Export ("polygon"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPolygon.h", Line = 39)]
		SKPolygon Polygon { get; }
	}
}
