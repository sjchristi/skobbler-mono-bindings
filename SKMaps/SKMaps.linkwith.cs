using System;
using ObjCRuntime;

[assembly: LinkWith ("SKMaps.a", LinkTarget.Arm64 | LinkTarget.ArmV6 | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator | LinkTarget.Simulator64, "-lstdc++ -lc++ -lz", SmartLink = false, ForceLoad = true, IsCxx = true, Frameworks = "CoreMotion CoreTelephony SystemConfiguration AVFoundation AudioToolbox CoreLocation QuartzCore OpenGLES UIKit Foundation CoreGraphics")]
