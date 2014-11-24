using System.Drawing;
using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKAnnotation {

		[Export ("identifier")]
		int Identifier { get; set; }

		[Export ("imagePath", ArgumentSemantic.Retain)]
		string ImagePath { get; set; }

		[Export ("imageSize")]
		int ImageSize { get; set; }

		[Export ("annotationType")]
		SKAnnotationType AnnotationType { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		PointF Offset { get; set; }

		[Export ("location", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Location { get; set; }

		[Export ("minZoomLevel")]
		int MinZoomLevel { get; set; }

		[Export ("annotationView", ArgumentSemantic.Retain)]
		SKAnnotationView AnnotationView { get; set; }

		[Static, Export ("annotation"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKAnnotation.h", Line = 52)]
		SKAnnotation Annotation { get; }
	}
}
