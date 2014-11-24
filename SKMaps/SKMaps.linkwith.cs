using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("SKMaps.a", LinkTarget.ArmV6 | LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, "-lstdc++ -lc++ -lz", SmartLink = true, ForceLoad = true, IsCxx = true, Frameworks = "CoreMotion CoreTelephony SystemConfiguration AVFoundation AudioToolbox CoreLocation QuartzCore OpenGLES UIKit Foundation CoreGraphics")]
