using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKOverlay {

		[Export ("fillColor", ArgumentSemantic.Retain)]
		UIColor FillColor { get; set; }

		[Export ("borderDotsSize")]
		int BorderDotsSize { get; set; }

		[Export ("borderDotsSpacingSize")]
		int BorderDotsSpacingSize { get; set; }
	}
}
