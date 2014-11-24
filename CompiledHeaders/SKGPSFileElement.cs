using System;

namespace SKMap {

	public enum SKGPSFileElementType : [unmapped: unexposed: Elaborated] {
		GPXRoot,
		GPXRoute,
		GPXRoutePoint,
		GPXTrack,
		GPXTrackSegment,
		GPXTrackPoint,
		GPXWaypoint
	}

	public enum SKGPSFileElementType {
		GPXRoot,
		GPXRoute,
		GPXRoutePoint,
		GPXTrack,
		GPXTrackSegment,
		GPXTrackPoint,
		GPXWaypoint
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKGPSFileElement {

		[Export ("identifier")]
		int Identifier { get; set; }

		[Export ("fileIdentifier")]
		int FileIdentifier { get; set; }

		[Export ("type")]
		SKGPSFileElementType Type { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("extensions", ArgumentSemantic.Retain)]
		string Extensions { get; set; }
	}
}
