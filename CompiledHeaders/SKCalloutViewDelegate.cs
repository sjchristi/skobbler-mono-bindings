using System;

namespace SKMap {

	[Model, BaseType (typeof (NSObject))]
	public partial interface SKCalloutViewDelegate {

		[Export ("calloutView:didTapLeftButton:")]
		void DidTapLeftButton (SKCalloutView calloutView, UIButton leftButton);

		[Export ("calloutView:didTapRightButton:")]
		void DidTapRightButton (SKCalloutView calloutView, UIButton rightButton);
	}
}
