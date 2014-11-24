using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKMapInternationalizationSettings {

		[Export ("primaryOption")]
		SKMapInternationalizationOption PrimaryOption { get; set; }

		[Export ("fallbackOption")]
		SKMapInternationalizationOption FallbackOption { get; set; }

		[Export ("primaryInternationalLanguage")]
		SKMapLanguage PrimaryInternationalLanguage { get; set; }

		[Export ("fallbackInternationalLanguage")]
		SKMapLanguage FallbackInternationalLanguage { get; set; }

		[Export ("showBothRows")]
		bool ShowBothRows { get; set; }

		[Export ("backupToTransliterated")]
		bool BackupToTransliterated { get; set; }

		[Static, Export ("mapInternationalization"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKMapInternationalizationSettings.h", Line = 41)]
		SKMapInternationalizationSettings MapInternationalization { get; }
	}
}
