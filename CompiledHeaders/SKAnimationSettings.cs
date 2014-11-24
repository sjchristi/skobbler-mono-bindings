using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKAnimationSettings {

		[Export ("animationType")]
		SKAnimationType AnimationType { get; set; }

		[Export ("animationEasingType")]
		SKAnimationEasingType AnimationEasingType { get; set; }

		[Export ("duration")]
		int Duration { get; set; }

		[Static, Export ("defaultAnimationSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKAnimationSettings.h", Line = 29)]
		SKAnimationSettings DefaultAnimationSettings { get; }

		[Static, Export ("animationSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKAnimationSettings.h", Line = 33)]
		SKAnimationSettings AnimationSettings { get; }
	}
}
