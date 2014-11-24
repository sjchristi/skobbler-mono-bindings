using System;

namespace SKMap {

	[BaseType (typeof (UIView))]
	public partial interface SKMapScaleView {

		[Export ("scale")]
		float Scale { get; set; }

		[Export ("distanceFormat")]
		SKDistanceFormat DistanceFormat { get; set; }

		[Export ("scaleColor", ArgumentSemantic.Retain)]
		UIColor ScaleColor { get; set; }

		[Export ("textColor", ArgumentSemantic.Retain)]
		UIColor TextColor { get; set; }
	}
}
