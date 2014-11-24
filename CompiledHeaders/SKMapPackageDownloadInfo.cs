using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapPackageDownloadInfo {

		[Export ("mapURL", ArgumentSemantic.Retain)]
		string MapURL { get; set; }

		[Export ("textureURL", ArgumentSemantic.Retain)]
		string TextureURL { get; set; }

		[Export ("namebrowserFilesURL", ArgumentSemantic.Retain)]
		string NamebrowserFilesURL { get; set; }
	}
}
