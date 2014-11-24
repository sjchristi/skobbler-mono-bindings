using System;

namespace SKMap {

	public partial interface SKMapViewStyle {

		[Notification, Field ("kSKMapStyleParsingFinishedNotification")]
		NSString SKMapStyleParsingFinishedNotification { get; }

		[Field ("kSKMapStyleParsingFinishedStyleIDKey")]
		NSString SKMapStyleParsingFinishedStyleIDKey { get; }
	}

	[Category, BaseType (typeof (SKMapView))]
	public partial interface Style_SKMapView {

		[Static, Export ("mapStyle"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapView+Style.h", Line = 21)]
		SKMapViewStyle MapStyle { get; }

		[Static, Export ("setMapStyle:")]
		bool SetMapStyle (SKMapViewStyle mapStyle);

		[Static, Export ("parseAlternativeMapStyle:asynchronously:")]
		bool ParseAlternativeMapStyle (SKMapViewStyle alternativeStyle, bool asynchronously);

		[Static, Export ("loadAlternativeMapStyle:")]
		bool LoadAlternativeMapStyle (SKMapViewStyle alternativeStyle);

		[Static, Export ("useAlternativeMapStyle:")]
		bool UseAlternativeMapStyle (bool useAlternative);

		[Static, Export ("unloadAlternativeMapStyle")]
		void UnloadAlternativeMapStyle ();

		[Static, Export ("removeStyle:")]
		void RemoveStyle (SKMapViewStyle alternativeStyle);
	}
}
