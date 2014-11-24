using System.Drawing;
using System;

namespace SKMap {

	[Model, BaseType (typeof (NSObject))]
	public partial interface SKMapViewDelegate {

		[Export ("mapView:didStartRegionChangeFromRegion:")]
		void DidStartRegionChangeFromRegion (SKMapView mapView, SKCoordinateRegion region);

		[Export ("mapView:didChangeToRegion:")]
		void DidChangeToRegion (SKMapView mapView, SKCoordinateRegion region);

		[Export ("mapView:didEndRegionChangeToRegion:")]
		void DidEndRegionChangeToRegion (SKMapView mapView, SKCoordinateRegion region);

		[Export ("mapView:didTapAtCoordinate:")]
		void DidTapAtCoordinate (SKMapView mapView, CLLocationCoordinate2D coordinate);

		[Export ("mapView:didLongTapAtCoordinate:")]
		void DidLongTapAtCoordinate (SKMapView mapView, CLLocationCoordinate2D coordinate);

		[Export ("mapView:didDoubleTapAtCoordinate:")]
		void DidDoubleTapAtCoordinate (SKMapView mapView, CLLocationCoordinate2D coordinate);

		[Export ("mapView:didDoubleTouchAtPoint:")]
		void DidDoubleTouchAtPoint (SKMapView mapView, PointF point);

		[Export ("mapView:didPanFromPoint:toPoint:")]
		void DidPanFromPoint (SKMapView mapView, PointF fromPoint, PointF toPoint);

		[Export ("mapView:didPinchWithScale:")]
		void DidPinchWithScale (SKMapView mapView, float scale);

		[Export ("mapView:didRotateWithAngle:")]
		void DidRotateWithAngle (SKMapView mapView, float angle);

		[Export ("mapViewWillRequireOnlineConnection:")]
		void  (SKMapView mapView);

		[Export ("mapView:didSelectMapPOI:")]
		void DidSelectMapPOI (SKMapView mapView, SKMapPOI mapPOI);

		[Export ("mapView:didSelectAnnotation:")]
		void DidSelectAnnotation (SKMapView mapView, SKAnnotation annotation);

		[Export ("mapView:didSelectCustomPOI:")]
		void DidSelectCustomPOI (SKMapView mapView, SKMapCustomPOI customPOI);

		[Export ("mapView:didSelectPOICluster:")]
		void DidSelectPOICluster (SKMapView mapView, SKPOICluster poiCluster);

		[Export ("mapViewDidSelectCompass:")]
		void  (SKMapView mapView);

		[Export ("mapViewDidSelectCurrentPositionIcon:")]
		void  (SKMapView mapView);

		[Export ("mapView:calloutViewForLocation:")]
		UIView CalloutViewForLocation (SKMapView mapView, CLLocationCoordinate2D location);

		[Export ("mapView:calloutViewForAnnotation:")]
		UIView CalloutViewForAnnotation (SKMapView mapView, SKAnnotation annotation);

		[Export ("mapViewDidTapAttribution:")]
		void  (SKMapView mapView);

		[Export ("mapView:didSelectOverlayWithId:atLocation:")]
		void DidSelectOverlayWithId (SKMapView mapView, int overlayId, CLLocationCoordinate2D location);

		[Export ("mapViewDidFinishRenderingImageInBoundingBox:")]
		void  (SKMapView mapView);
	}
}
