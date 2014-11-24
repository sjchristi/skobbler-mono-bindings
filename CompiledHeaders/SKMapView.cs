using System.Drawing;
using System;

namespace SKMap {

	[BaseType (typeof (UIView))]
	public partial interface SKMapView {

		[Export ("delegate", ArgumentSemantic.Assign)]
		SKMapViewDelegate Delegate { get; set; }

		[Export ("settings", ArgumentSemantic.Retain)]
		SKMapSettings Settings { get; }

		[Export ("visibleRegion", ArgumentSemantic.Assign)]
		SKCoordinateRegion VisibleRegion { get; set; }

		[Export ("bearing")]
		float Bearing { get; set; }

		[Export ("enabledRendering")]
		bool EnabledRendering { get; set; }

		[Export ("isInNavigationMode")]
		bool IsInNavigationMode { get; }

		[Export ("calloutView", ArgumentSemantic.Retain)]
		SKCalloutView CalloutView { get; set; }

		[Export ("mapScaleView")]
		SKMapScaleView MapScaleView { get; }

		[Export ("showDebugView")]
		bool ShowDebugView { get; set; }

		[Export ("currentPositionView", ArgumentSemantic.Retain)]
		UIView CurrentPositionView { get; set; }

		[Export ("applySettingsFromFileAtPath:")]
		void ApplySettingsFromFileAtPath (string filePath);

		[Export ("centerOnCurrentPosition")]
		void CenterOnCurrentPosition ();

		[Export ("animateToZoomLevel:")]
		void AnimateToZoomLevel (float zoom);

		[Export ("animateToBearing:")]
		void AnimateToBearing (float bearing);

		[Export ("animateToLocation:withDuration:")]
		void AnimateToLocation (CLLocationCoordinate2D location, float duration);

		[Export ("fitBounds:withPadding:")]
		void FitBounds (SKBoundingBox boundingBox, SizeF padding);

		[Export ("pointForCoordinate:")]
		PointF PointForCoordinate (CLLocationCoordinate2D location);

		[Export ("coordinateForPoint:")]
		CLLocationCoordinate2D CoordinateForPoint (PointF point);

		[Export ("addAnnotation:withAnimationSettings:")]
		bool AddAnnotation (SKAnnotation annotation, SKAnimationSettings animationSettings);

		[Export ("addCustomPOI:")]
		bool AddCustomPOI (SKMapCustomPOI customPOI);

		[Export ("bringToFrontAnnotation:")]
		bool BringToFrontAnnotation (SKAnnotation annotation);

		[Export ("updateAnnotation:")]
		bool UpdateAnnotation (SKAnnotation annotation);

		[Export ("annotationForIdentifier:")]
		SKAnnotation AnnotationForIdentifier (int identifier);

		[Export ("removeAnnotationWithID:")]
		void RemoveAnnotationWithID (int identifier);

		[Export ("clearAllAnnotations")]
		void ClearAllAnnotations ();

		[Export ("showCalloutAtLocation:withOffset:animated:")]
		void ShowCalloutAtLocation (CLLocationCoordinate2D coordinate, PointF calloutOffset, bool shouldAnimate);

		[Export ("showCalloutForAnnotation:withOffset:animated:")]
		void ShowCalloutForAnnotation (SKAnnotation annotation, PointF calloutOffset, bool shouldAnimate);

		[Export ("hideCallout")]
		void HideCallout ();

		[Export ("renderMapImageInBoundingBox:toPath:withSize:")]
		void RenderMapImageInBoundingBox (SKBoundingBox boundingBox, string imagePath, SizeF size);

		[Export ("lastRenderedFrame"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapView.h", Line = 187)]
		UIImage LastRenderedFrame { get; }

		[Export ("startPulseAnnimation:")]
		bool StartPulseAnnimation (SKPulseAnimationSettings pulseAnimationSettings);

		[Export ("stopPulseAnnimation"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapView.h", Line = 198)]
		bool StopPulseAnnimation { get; }
	}
}
