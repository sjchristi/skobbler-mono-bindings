using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (SKOverlay))]
	public partial interface SKPolyline {

		[Export ("coordinates", ArgumentSemantic.Retain), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPolyline.h", Line = 20)]
		NSObject [] Coordinates { get; set; }

		[Export ("lineWidth")]
		int LineWidth { get; set; }

		[Export ("backgroundLineWidth")]
		int BackgroundLineWidth { get; set; }

		[Static, Export ("polyline"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPolyline.h", Line = 32)]
		SKPolyline Polyline { get; }
	}
}
