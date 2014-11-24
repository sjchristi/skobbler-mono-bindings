using System;

using MonoTouch.Foundation;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKGPSFilesService {

		[Static, Export ("sharedInstance"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKGPSFilesService.h", Line = 18)]
		SKGPSFilesService SharedInstance { get; }

		[Export ("loadFileAtPath:error:")]
		SKGPSFileElement LoadFileAtPath (string path, out NSError error);

		[Export ("saveFileAtPath:error:")]
		bool SaveFileAtPath (string path, out NSError error);

		[Export ("resetCurrentFile:")]
		bool ResetCurrentFile (out NSError error);

		[Export ("childElementsForElement:error:"), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKGPSFilesService.h", Line = 45)]
		NSObject [] ChildElementsForElement (SKGPSFileElement parent, out NSError error);

		[Export ("childElementsForElement:withType:error:"), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKGPSFilesService.h", Line = 53)]
		NSObject [] ChildElementsForElement (SKGPSFileElement parent, SKGPSFileElementType type, out NSError error);

		[Export ("locationsForElement:"), Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKGPSFilesService.h", Line = 59)]
		NSObject [] LocationsForElement (SKGPSFileElement element);
	}
}
