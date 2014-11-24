using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKAnnotationView {

		[Export ("view", ArgumentSemantic.Retain)]
		UIView View { get; }

		[Export ("reuseIdentifier", ArgumentSemantic.Retain)]
		string ReuseIdentifier { get; }

		[Export ("initWithView:reuseIdentifier:")]
		IntPtr Constructor (UIView view, string reuseIdentifier);
	}
}
