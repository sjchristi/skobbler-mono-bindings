using System.Drawing;
using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKBoundingBox {

		[Export ("topLeftCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D TopLeftCoordinate { get; set; }

		[Export ("bottomRightCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D BottomRightCoordinate { get; set; }

		[Export ("containsLocation:")]
		bool ContainsLocation (CLLocationCoordinate2D location);

		[Export ("boundingBoxIncludingLocation:")]
		SKBoundingBox BoundingBoxIncludingLocation (CLLocationCoordinate2D location);

		[Static, Export ("boundingBoxWithTopLeftCoordinate:bottomRightCoordinate:")]
		SKBoundingBox BoundingBoxWithTopLeftCoordinate (CLLocationCoordinate2D topLeft, CLLocationCoordinate2D bottomRight);

		[Static, Export ("boundingBoxForRegion:inMapViewWithSize:")]
		SKBoundingBox BoundingBoxForRegion (SKCoordinateRegion region, SizeF size);
	}
}
