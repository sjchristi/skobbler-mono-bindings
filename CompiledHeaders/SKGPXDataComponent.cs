using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKGPXDataComponent {

		[Export ("componentType")]
		SKGPXComponentType ComponentType { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Static, Export ("gpxDataComponentWithType:name:")]
		SKGPXDataComponent GpxDataComponentWithType (SKGPXComponentType componentType, string name);
	}
}
