using System.Drawing;
using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapSettings {

		[Export ("rotationEnabled")]
		bool RotationEnabled { get; set; }

		[Export ("panningEnabled")]
		bool PanningEnabled { get; set; }

		[Export ("zoomWithCenterAnchor")]
		bool ZoomWithCenterAnchor { get; set; }

		[Export ("inertiaEnabled")]
		bool InertiaEnabled { get; set; }

		[Export ("followerMode")]
		SKMapFollowerMode FollowerMode { get; set; }

		[Export ("showCompass")]
		bool ShowCompass { get; set; }

		[Export ("showAccuracyCircle")]
		bool ShowAccuracyCircle { get; set; }

		[Export ("compassOffset", ArgumentSemantic.Assign)]
		PointF CompassOffset { get; set; }

		[Export ("orientationIndicatorType")]
		SKOrientationIndicatorType OrientationIndicatorType { get; set; }

		[Export ("displayMode")]
		SKMapDisplayMode DisplayMode { get; set; }

		[Export ("poiDisplayingOption")]
		SKPOIDisplayingOption PoiDisplayingOption { get; set; }

		[Export ("mapInternationalization", ArgumentSemantic.Retain)]
		SKMapInternationalizationSettings MapInternationalization { get; set; }

		[Export ("zoomLimits", ArgumentSemantic.Assign)]
		SKMapZoomLimits ZoomLimits { get; set; }

		[Export ("annotationTapZoomLimit")]
		float AnnotationTapZoomLimit { get; set; }

		[Export ("showCurrentPosition")]
		bool ShowCurrentPosition { get; set; }

		[Export ("showStreetNamePopUps")]
		bool ShowStreetNamePopUps { get; set; }

		[Export ("showBicycleLanes")]
		bool ShowBicycleLanes { get; set; }

		[Export ("showHouseNumbers")]
		bool ShowHouseNumbers { get; set; }

		[Export ("showOneWays")]
		bool ShowOneWays { get; set; }

		[Export ("showStreetBadges")]
		bool ShowStreetBadges { get; set; }

		[Export ("showMapPoiIcons")]
		bool ShowMapPoiIcons { get; set; }

		[Export ("osmAttributionPosition")]
		SKAttributionPosition OsmAttributionPosition { get; set; }

		[Export ("companyAttributionPosition")]
		SKAttributionPosition CompanyAttributionPosition { get; set; }

		[Export ("drawingOrderType")]
		SKDrawingOrderType DrawingOrderType { get; set; }

		[Static, Export ("mapSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapSettings.h", Line = 126)]
		SKMapSettings MapSettings { get; }
	}
}
