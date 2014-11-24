using System;

namespace SKMap {

	[BaseType (typeof (NSObject))]
	public partial interface SKPositionerService {

		[Export ("delegate", ArgumentSemantic.Assign)]
		SKPositionerServiceDelegate Delegate { get; set; }

		[Export ("activityType")]
		CLActivityType ActivityType { get; set; }

		[Export ("gpsAccuracyLevel")]
		SKGPSAccuracyLevel GpsAccuracyLevel { get; }

		[Export ("currentCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D CurrentCoordinate { get; }

		[Export ("currentPosition", ArgumentSemantic.Assign)]
		SKPosition CurrentPosition { get; }

		[Export ("currentMatchedPosition", ArgumentSemantic.Assign)]
		SKPosition CurrentMatchedPosition { get; }

		[Export ("currentHeading", ArgumentSemantic.Assign)]
		CLHeading CurrentHeading { get; }

		[Export ("positionsLogFilePath", ArgumentSemantic.Retain)]
		string PositionsLogFilePath { get; set; }

		[Static, Export ("sharedInstance"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPositionerService.h", Line = 52)]
		SKPositionerService SharedInstance { get; }

		[Export ("startLocationUpdate")]
		void StartLocationUpdate ();

		[Export ("cancelLocationUpdate")]
		void CancelLocationUpdate ();

		[Export ("startSignificantLocationUpdate")]
		void StartSignificantLocationUpdate ();

		[Export ("cancelSignificantLocationUpdate")]
		void CancelSignificantLocationUpdate ();

		[Export ("startPositionReplayFromLog:")]
		bool StartPositionReplayFromLog (string logFileNameWithPath);

		[Export ("stopPositionReplay"), Verify ("ObjC method massaged into getter property", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKPositionerService.h", Line = 81)]
		bool StopPositionReplay { get; }

		[Export ("setPositionReplayRate:")]
		bool SetPositionReplayRate (double rate);

		[Export ("increaseRouteSimulationSpeed:")]
		bool IncreaseRouteSimulationSpeed (int withValue);

		[Export ("decreaseRouteSimulationSpeed:")]
		bool DecreaseRouteSimulationSpeed (double withValue);
	}
}
