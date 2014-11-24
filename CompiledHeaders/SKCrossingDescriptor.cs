using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKCrossingDescriptor {

		[Export ("crossingType")]
		int CrossingType { get; set; }

		[Export ("routeAngle")]
		float RouteAngle { get; set; }

		[Export ("turnToRight")]
		bool TurnToRight { get; set; }

		[Export ("directionUK")]
		bool DirectionUK { get; set; }

		[Export ("allowedRoutesAngles", ArgumentSemantic.Retain)]
		NSMutableArray AllowedRoutesAngles { get; set; }

		[Export ("forbiddenRoutesAngles", ArgumentSemantic.Retain)]
		NSMutableArray ForbiddenRoutesAngles { get; set; }
	}
}
