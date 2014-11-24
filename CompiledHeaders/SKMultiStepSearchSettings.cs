using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMultiStepSearchSettings {

		[Export ("listLevel")]
		SKListLevel ListLevel { get; set; }

		[Export ("offlinePackageCode", ArgumentSemantic.Retain)]
		string OfflinePackageCode { get; set; }

		[Export ("parentIndex")]
		ulong ParentIndex { get; set; }

		[Export ("searchTerm", ArgumentSemantic.Retain)]
		string SearchTerm { get; set; }

		[Static, Export ("multiStepSearchSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMultiStepSearchSettings.h", Line = 34)]
		SKMultiStepSearchSettings MultiStepSearchSettings { get; }
	}
}
