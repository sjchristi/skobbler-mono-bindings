using System.Drawing;
using System;

namespace SKMap {

	[BaseType (typeof (UIView))]
	public partial interface SKCalloutView {

		[Export ("delegate", ArgumentSemantic.Assign)]
		SKCalloutViewDelegate Delegate { get; set; }

		[Export ("location", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Location { get; set; }

		[Export ("calloutOffset", ArgumentSemantic.Assign)]
		PointF CalloutOffset { get; set; }

		[Export ("dynamicArrowPositioning")]
		bool DynamicArrowPositioning { get; set; }

		[Export ("middleImageView", ArgumentSemantic.Retain)]
		UIImageView MiddleImageView { get; set; }

		[Export ("leftButton", ArgumentSemantic.Retain)]
		UIButton LeftButton { get; set; }

		[Export ("rightButton", ArgumentSemantic.Retain)]
		UIButton RightButton { get; set; }

		[Export ("titleLabel", ArgumentSemantic.Retain)]
		UILabel TitleLabel { get; set; }

		[Export ("subtitleLabel", ArgumentSemantic.Retain)]
		UILabel SubtitleLabel { get; set; }

		[Static, Export ("calloutView"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKCalloutView.h", Line = 56)]
		SKCalloutView CalloutView { get; }
	}
}
