using System;

namespace SKMap {

	[Category, BaseType (typeof (SKMapView))]
	public partial interface GPSFiles_SKMapView {

		[Export ("drawGPSFileElement:")]
		bool DrawGPSFileElement (SKGPSFileElement element);

		[Export ("removeGPSFileElement:")]
		bool RemoveGPSFileElement (SKGPSFileElement element);

		[Export ("fitGPSFileElement:")]
		void FitGPSFileElement (SKGPSFileElement element);
	}
}
