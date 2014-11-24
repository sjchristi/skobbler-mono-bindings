using System;

namespace SKMap {

	[Category, BaseType (typeof (SKMapView))]
	public partial interface RealReach_SKMapView {

		[Export ("displayRealReachWithSettings:")]
		void DisplayRealReachWithSettings (SKRealReachSettings realReachSettings);

		[Export ("clearRealReachDisplay")]
		void ClearRealReachDisplay ();

		[Export ("isRealReachDisplayedInBoundingBox:")]
		bool IsRealReachDisplayedInBoundingBox (SKBoundingBox boundingBox);
	}
}
