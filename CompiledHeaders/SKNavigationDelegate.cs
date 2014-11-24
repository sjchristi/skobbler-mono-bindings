using System;

using MonoTouch.Foundation;

namespace SKMap {

	[Model, BaseType (typeof (NSObject))]
	public partial interface SKNavigationDelegate {

		[Export ("routingService:didChangeDistanceToDestination:withFormattedDistance:")]
		void DidChangeDistanceToDestination (SKRoutingService routingService, int distance, string formattedDistance);

		[Export ("routingService:didChangeEstimatedTimeToDestination:")]
		void DidChangeEstimatedTimeToDestination (SKRoutingService routingService, int time);

		[Export ("routingService:didChangeAdviceID:")]
		void DidChangeAdviceID (SKRoutingService routingService, int adviceID);

		[Export ("routingService:didChangeCurrentStreetName:streetType:countryCode:")]
		void DidChangeCurrentStreetName (SKRoutingService routingService, string currentStreetName, SKStreetType streetType, string countryCode);

		[Export ("routingService:didChangeNextStreetName:streetType:countryCode:")]
		void DidChangeNextStreetName (SKRoutingService routingService, string nextStreetName, SKStreetType streetType, string countryCode);

		[Export ("routingService:didChangeSecondNextStreet:streetType:countryCode:")]
		void DidChangeSecondNextStreet (SKRoutingService routingService, string nextStreetName, SKStreetType streetType, string countryCode);

		[Export ("routingService:didChangeCurrentAdviceImage:withLastAdvice:")]
		void DidChangeCurrentAdviceImage (SKRoutingService routingService, UIImage adviceImage, bool isLastAdvice);

		[Export ("routingService:didChangeCurrentVisualAdviceDistance:withFormattedDistance:")]
		void DidChangeCurrentVisualAdviceDistance (SKRoutingService routingService, int distance, string formattedDistance);

		[Export ("routingService:didChangeSecondaryAdviceImage:withLastAdvice:")]
		void DidChangeSecondaryAdviceImage (SKRoutingService routingService, UIImage adviceImage, bool isLastAdvice);

		[Export ("routingService:didChangeSecondaryVisualAdviceDistance:withFormattedDistance:")]
		void DidChangeSecondaryVisualAdviceDistance (SKRoutingService routingService, int distance, string formattedDistance);

		[Export ("routingService:didUpdateFilteredAudioAdvices:")]
		void DidUpdateFilteredAudioAdvices (SKRoutingService routingService, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKNavigationDelegate.h", Line = 94)] NSObject [] audioAdvices);

		[Export ("routingService:didUpdateUnfilteredAudioAdvices:withDistance:")]
		void DidUpdateUnfilteredAudioAdvices (SKRoutingService routingService, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKNavigationDelegate.h", Line = 101)] NSObject [] audioAdvices, int distance);

		[Export ("routingService:didChangeCurrentSpeed:")]
		void DidChangeCurrentSpeed (SKRoutingService routingService, double speed);

		[Export ("routingService:didChangeCurrentSpeedLimit:")]
		void DidChangeCurrentSpeedLimit (SKRoutingService routingService, double speedLimit);

		[Export ("routingServiceDidStartRerouting:")]
		void  (SKRoutingService routingService);

		[Export ("routingService:didUpdateSpeedWarningToStatus:withAudioWarnings:insideCity:")]
		void DidUpdateSpeedWarningToStatus (SKRoutingService routingService, bool speedWarningIsActive, [Verify ("NSArray may be reliably typed, check the documentation", "/Volumes/Booyah/projects/SKMap/SKMap/SKMaps.framework/Versions/A/Headers/SKNavigationDelegate.h", Line = 126)] NSObject [] audioWarnings, bool isInsideCity);

		[Export ("routingService:didReachViaPointWithIndex:")]
		void DidReachViaPointWithIndex (SKRoutingService routingService, int index);

		[Export ("routingServiceDidReachDestination:")]
		void  (SKRoutingService routingService);

		[Export ("routingService:didChangeFirstVisualAdvice:withSecondVisualAdvice:lastAdvice:routeState:")]
		void DidChangeFirstVisualAdvice (SKRoutingService routingService, bool isFirstVisualAdviceChanged, bool isSecondaryAdviceChanged, bool isLastAdvice, SKRouteState routeState);

		[Export ("routingService:didChangeCountryCode:")]
		void DidChangeCountryCode (SKRoutingService routingService, string countryCode);

		[Export ("routingService:didChangeExitNumber:")]
		void DidChangeExitNumber (SKRoutingService routingService, string exitNumber);

		[Export ("routingService:didChangeCurrentAdviceInstruction:nextAdviceInstruction:")]
		void DidChangeCurrentAdviceInstruction (SKRoutingService routingService, string currentAdviceInstruction, string nextAdviceInstruction);
	}
}
