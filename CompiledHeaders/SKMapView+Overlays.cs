using System;

namespace SKMap {

	[Category, BaseType (typeof (SKMapView))]
	public partial interface Overlays_SKMapView {

		[Export ("addPolygon:")]
		int AddPolygon (SKPolygon polygon);

		[Export ("addPolyline:")]
		int AddPolyline (SKPolyline polyline);

		[Export ("addCircle:")]
		int AddCircle (SKCircle circle);

		[Export ("clearOverlayWithID:")]
		bool ClearOverlayWithID (int overlayID);

		[Export ("clearAllOverlays")]
		void ClearAllOverlays ();
	}
}
