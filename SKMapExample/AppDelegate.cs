using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using SKMaps;
using SKMapUtil;

namespace SKMapExample
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate, IBackgroundUrlEventDispatcher
	{
		// class-level declarations
		
		public override UIWindow Window {
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
//			Forms.Init ();

			// Perform one-time SKMaps initialization....
			SKMapsInitSettings initSettings = SKMapsInitSettings.MapsInitSettings;
			initSettings.ConnectivityMode = SKConnectivityMode.Offline;
			initSettings.MapDetailLevel = SKMapDetailLevel.Full;
			SKMaps.SKMapsService.InitializeSKMapsWithAPIKey("YOUR API KEY HERE");

			SKMaps.SKMapsService.SharedInstance.MapsVersioningManager.WeakDelegate = this;

			return true;
		}
		
		// This method is invoked when the application is about to move from active to inactive state.
		// OpenGL applications should use this method to pause.
		public override void OnResignActivation (UIApplication application)
		{
		}
		
		// This method should be used to release shared resources and it should store the application state.
		// If your application supports background exection this method is called instead of WillTerminate
		// when the user quits.
		public override void DidEnterBackground (UIApplication application)
		{
		}
		
		// This method is called as part of the transiton from background to active state.
		public override void WillEnterForeground (UIApplication application)
		{
		}
		
		// This method is called when the application is about to terminate. Save data, if needed.
		public override void WillTerminate (UIApplication application)
		{
		}


		// SKMapVersioningDelegate

		void DetectedNewAvailableMapVersion (SKMapsVersioningManager versioningManager, string latestMapVersion, string currentMapVersion)
		{
			Console.WriteLine ("SKMapsVersioningManager detected new map version; {0} -> {1}", currentMapVersion, latestMapVersion);
		}

		void LoadedWithOfflinePackages (SKMapsVersioningManager versioningManager, SKMapPackage [] packages, SKMapPackage [] updatablePackages)
		{
			Console.WriteLine ("SKMapsVersioningManager loaded with the following packages:");

			foreach (SKMapPackage p in packages)
			{
				Console.WriteLine ("\tName: {0}, Version: {1}, Size: {2}", p.Name, p.Version, p.Size.ToString ("N0"));
			}

			Console.WriteLine ("SKMapsVersioningManager can update the following map packages:");

			foreach (SKMapPackage p in updatablePackages)
			{
				Console.WriteLine ("\tName: {0}, Version: {1}, Size: {2}", p.Name, p.Version, p.Size.ToString ("N0"));
			}
		}

		void LoadedWithMapVersion (SKMapsVersioningManager versioningManager, string currentMapVersion)
		{
			Console.WriteLine ("SKMapsVersioningManager loaded with version {0}", currentMapVersion);
		}

		void LoadedMetadata (SKMapsVersioningManager versioningManager)
		{
			Console.WriteLine ("SKMapsVersioningManager loaded metadata");
		}

		public event EventHandler<BackgroundUrlEventArgs> HandleEventsForBackgroundUrlEvent;

		public override void HandleEventsForBackgroundUrl (UIApplication application, string sessionIdentifier, NSAction completionHandler)
		{
			if (HandleEventsForBackgroundUrlEvent != null)
			{
				BackgroundUrlEventArgs args = new BackgroundUrlEventArgs (application, sessionIdentifier, completionHandler);
				HandleEventsForBackgroundUrlEvent.Invoke (this, args);
			}
		}
	}
}

