using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKAdvisorSettings {

		[Export ("advisorVoice", ArgumentSemantic.Retain)]
		string AdvisorVoice { get; set; }

		[Export ("resourcesPath", ArgumentSemantic.Retain)]
		string ResourcesPath { get; set; }

		[Export ("language", ArgumentSemantic.Retain)]
		string Language { get; set; }

		[Export ("configPath", ArgumentSemantic.Retain)]
		string ConfigPath { get; set; }

		[Static, Export ("advisorSettings"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKAdvisorSettings.h", Line = 32)]
		SKAdvisorSettings AdvisorSettings { get; }
	}
}
