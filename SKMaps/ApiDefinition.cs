using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;

namespace SKMaps
{
	// The first step to creating a binding is to add your native library ("libNativeLibrary.a")
	// to the project by right-clicking (or Control-clicking) the folder containing this source
	// file and clicking "Add files..." and then simply select the native library (or libraries)
	// that you want to bind.
	//
	// When you do that, you'll notice that MonoDevelop generates a code-behind file for each
	// native library which will contain a [LinkWith] attribute. MonoDevelop auto-detects the
	// architectures that the native library supports and fills in that information for you,
	// however, it cannot auto-detect any Frameworks or other system libraries that the
	// native library may depend on, so you'll need to fill in that information yourself.
	//
	// Once you've done that, you're ready to move on to binding the API...
	//
	//
	// Here is where you'd define your API definition for the native Objective-C library.
	//
	// For example, to bind the following Objective-C class:
	//
	//     @interface Widget : NSObject {
	//     }
	//
	// The C# binding would look like this:
	//
	//     [BaseType (typeof (NSObject))]
	//     interface Widget {
	//     }
	//
	// To bind Objective-C properties, such as:
	//
	//     @property (nonatomic, readwrite, assign) CGPoint center;
	//
	// You would add a property definition in the C# interface like so:
	//
	//     [Export ("center")]
	//     PointF Center { get; set; }
	//
	// To bind an Objective-C method, such as:
	//
	//     -(void) doSomething:(NSObject *)object atIndex:(NSInteger)index;
	//
	// You would add a method definition to the C# interface like so:
	//
	//     [Export ("doSomething:atIndex:")]
	//     void DoSomething (NSObject object, int index);
	//
	// Objective-C "constructors" such as:
	//
	//     -(id)initWithElmo:(ElmoMuppet *)elmo;
	//
	// Can be bound as:
	//
	//     [Export ("initWithElmo:")]
	//     IntPtr Constructor (ElmoMuppet elmo);
	//
	// For more information, see http://docs.xamarin.com/ios/advanced_topics/binding_objective-c_libraries
	//

	[BaseType (typeof (NSObject))]
	public partial interface SKMapViewStyle {

		[Export ("styleID")]
		int StyleID { get; set; }

		[Export ("resourcesFolderName", ArgumentSemantic.Retain)]
		string ResourcesFolderName { get; set; }

		[Export ("styleFileName", ArgumentSemantic.Retain)]
		string StyleFileName { get; set; }

		[Export ("resourcesPath", ArgumentSemantic.Retain)]
		string ResourcesPath { get; set; }

		[Static, Export ("mapViewStyle")]
		SKMapViewStyle MapViewStyle { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKProxySettings {

		[Export ("proxyType", ArgumentSemantic.Assign)]
		SKProxyType ProxyType { get; set; }

		[Export ("ip", ArgumentSemantic.Retain)]
		string Ip { get; set; }

		[Export ("port", ArgumentSemantic.Assign)]
		int Port { get; set; }

		[Export ("mask", ArgumentSemantic.Retain)]
		string Mask { get; set; }

		[Export ("useAutenthication", ArgumentSemantic.Assign)]
		bool UseAuthentication { get; set; }

		[Export ("user", ArgumentSemantic.Retain)]
		string User { get; set; }

		[Export ("password", ArgumentSemantic.Retain)]
		string Password { get; set; }

		[Export ("styleFileName", ArgumentSemantic.Retain)]
		string StyleFileName { get; set; }

		[Export ("resourcesPath", ArgumentSemantic.Retain)]
		string ResourcesPath { get; set; }

		[Static, Export ("proxySettings")]
		SKProxySettings ProxySettings { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapsInitSettings {

		[Export ("connectivityMode")]
		SKConnectivityMode ConnectivityMode { get; set; }

		[Export ("mapDetailLevel")]
		SKMapDetailLevel MapDetailLevel { get; set; }

		[Export ("mapStyle", ArgumentSemantic.Retain)]
		SKMapViewStyle MapStyle { get; set; }

		[Export ("cachesPath", ArgumentSemantic.Retain)]
		string CachesPath { get; set; }

		[Export ("showConsoleLogs")]
		bool ShowConsoleLogs { get; set; }

		[Export ("proxySettings", ArgumentSemantic.Retain)]
		SKProxySettings ProxySettings { get; set; }

		[Static, Export ("mapsInitSettings")]
		SKMapsInitSettings MapsInitSettings { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapPackageDownloadInfo {

		[Export ("mapURL", ArgumentSemantic.Retain)]
		string MapURL { get; set; }

		[Export ("textureURL", ArgumentSemantic.Retain)]
		string TextureURL { get; set; }

		[Export ("namebrowserFilesURL", ArgumentSemantic.Retain)]
		string NamebrowserFilesURL { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapPackage {

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("version", ArgumentSemantic.Retain)]
		string Version { get; set; }

		[Export ("size")]
		long Size { get; set; }

		[Static, Export ("mapPackage")]
		SKMapPackage MapPackage { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKOfflinePackagesManager {

		[Export ("installedOfflineMapPackages", ArgumentSemantic.Retain)]
		SKMapPackage [] InstalledOfflineMapPackages { get; }

		[Export ("mapsDownloadBasePath", ArgumentSemantic.Retain)]
		string MapsDownloadBasePath { get; }

		[Export ("addOfflineMapPackageNamed:inContainingFolderPath:")]
		SKAddPackageResult AddOfflineMapPackageNamed (string packageName, string containingFolderPath);

		[Export ("deleteOfflineMapPackageNamed:")]
		bool DeleteOfflineMapPackageNamed (string packageName);

		[Export ("validateMapFileAtPath:")]
		bool ValidateMapFileAtPath (string path);

		[Export ("mapsXMLURLForVersion:")]
		string MapsXMLURLForVersion ([NullAllowed] string version);

		[Export ("mapsJSONURLForVersion:")]
		string MapsJSONURLForVersion ([NullAllowed] string version);

		[Export ("downloadInfoForPackageWithCode:")]
		SKMapPackageDownloadInfo DownloadInfoForPackageWithCode (string packageCode);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKTilesCacheManager {
		[Export ("cacheSize", ArgumentSemantic.Assign)]
		ulong CacheSize { get; }

		[Export ("cacheLimit", ArgumentSemantic.Assign)]
		ulong CacheLimit { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model, Protocol]
	public partial interface SKMapVersioningDelegate {

		[Export ("mapsVersioningManager:detectedNewAvailableMapVersion:currentMapVersion:"), EventArgs("NewMapVersion")]
		void DetectedNewAvailableMapVersion (SKMapsVersioningManager versioningManager, string latestMapVersion, string currentMapVersion);

		[Export ("mapsVersioningManager:loadedWithOfflinePackages:updatablePackages:"), EventArgs("OfflinePackages")]
		void LoadedWithOfflinePackages (SKMapsVersioningManager versioningManager, SKMapPackage [] packages, SKMapPackage [] updatablePackages);

		[Export ("mapsVersioningManager:loadedWithMapVersion:"), EventArgs("MapVersion")]
		void LoadedWithMapVersion (SKMapsVersioningManager versioningManager, string currentMapVersion);

		[Export ("mapsVersioningManagerLoadedMetadata:")]
		void LoadedMetadata (SKMapsVersioningManager versioningManager);
	}


	[BaseType (typeof (NSObject),
		Delegates=new string [] {"WeakDelegate"},
		Events=new Type [] { typeof (SKMapVersioningDelegate) })]
	public partial interface SKMapsVersioningManager {

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")][NullAllowed]
		SKMapVersioningDelegate Delegate { get; set; }

		[Export ("currentMapVersion", ArgumentSemantic.Retain)]
		string CurrentMapVersion { get; }

		[Export ("localMapVersions", ArgumentSemantic.Retain)]
		NSString [] LocalMapVersions { get; }

		[Export ("availableMapVersions", ArgumentSemantic.Retain)]
		NSString [] AvailableMapVersions { get; }

		[Export ("updateToVersion:")]
		bool UpdateToVersion (string version);

		[Export ("checkNewVersion")]
		void CheckNewVersion ();

		[Export ("metaDataDownloadedStatus")]
		SKMetaDataDownloadStatus MetaDataDownloadedStatus { get; }

		[Export ("initWithConnectivityMode:")]
		IntPtr Constructor (SKConnectivityMode connectivityMode);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapsService {

		[Static, Export ("sharedInstance")]
		SKMapsService SharedInstance { get; }

		[Export ("initializeSKMapsWithAPIKey:settings:")]
		void InitializeSKMapsWithAPIKey (string apiKey, SKMapsInitSettings initSettings);

		[Export ("packagesManager", ArgumentSemantic.Retain)]
		SKOfflinePackagesManager PackagesManager { get; }

		[Export ("tilesCacheManager", ArgumentSemantic.Retain)]
		SKTilesCacheManager TilesCacheManager { get; }

		[Export ("mapsVersioningManager", ArgumentSemantic.Retain)]
		SKMapsVersioningManager MapsVersioningManager { get; }

		[Export ("connectivityMode")]
		SKConnectivityMode ConnectivityMode { get; set; }

		[Export ("proxySettings", ArgumentSemantic.Retain)]
		SKProxySettings ProxySettings { get; set; }

		[Export ("apiKey", ArgumentSemantic.Retain)]
		string ApiKey { get; }

		[Export ("obfuscatedAPIKey", ArgumentSemantic.Retain)]
		string ObfuscatedAPIKey { get; }

		[Export ("frameworkVersion", ArgumentSemantic.Retain)]
		string FrameworkVersion { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model, Protocol]
	public partial interface SKRoutingDelegate {

		[Export ("routingService:didFinishRouteCalculationWithInfo:"), EventArgs("RoutingServiceFinishRouteCalculation")]
		void DidFinishRouteCalculation (SKRoutingService routingService, SKRouteInformation routeInformation);

		[Export ("routingService:didFailWithErrorCode:"), EventArgs("RoutingServiceDidFail")]
		void FailedToCalculateRoute (SKRoutingService routingService, SKRoutingErrorCode errorCode);

		[Export ("routingServiceDidCalculateAllRoutes:"), EventArgs("RoutingServiceFinishedAllRoutes")]
		void DidCalculateAllRoutes (SKRoutingService routingService);

		[Export ("routingService:didFinishRouteRequestWithJSONResponse:"), EventArgs("FinishedRouteRequest")]
		void FinishedRouteRequest (SKRoutingService routingService, string response);

		[Export ("routingServiceShouldRetryCalculatingRoute:withRouteHangingTime:"), EventArgs("RoutingServiceShouldRetryRouteCalcuation"), NoDefaultValue]
		bool ShouldRetryRouteCalcuation (SKRoutingService routingService, int timeInterval);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKRouteAlternativeSettings {
		[Export ("routeMode", ArgumentSemantic.Assign)]
		SKRouteMode RouteMode { get; set; }

		[Export ("numberOfRoutes", ArgumentSemantic.Assign)]
		int NumberOfRoutes { get; set; }

		[Export ("useSlopes", ArgumentSemantic.Assign)]
		bool UseSlopes { get; set; }

		[Export ("avoidTollRoads", ArgumentSemantic.Assign)]
		bool AvoidTollRoads { get; set; }

		[Export ("avoidHighways", ArgumentSemantic.Assign)]
		bool AvoidHighways { get; set; }

		[Export ("avoidFerryLines", ArgumentSemantic.Assign)]
		bool AvoidFerryLines { get; set; }

		[Export ("avoidBicycleWalk", ArgumentSemantic.Assign)]
		bool AvoidBicycleWalk { get; set; }

		[Export ("avoidBicycleCarry", ArgumentSemantic.Assign)]
		bool AvoidBicycleCarry { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKRouteSettings {
		[Export ("startCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D StartCoordinate { get; set; }

		[Export ("destinationCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D DestinationCoordinate { get; set; }

		[Export ("routeMode", ArgumentSemantic.Assign)]
		SKRouteMode RouteMode { get; set; }

		[Export ("routeConnectionMode", ArgumentSemantic.Assign)]
		SKRouteConnectionMode RouteConnectionMode { get; set; }

		[Export ("shouldBeRendered", ArgumentSemantic.Assign)]
		bool ShouldBeRendered { get; set; }

		[Export ("avoidTollRoads", ArgumentSemantic.Assign)]
		bool AvoidTollRoads { get; set; }

		[Export ("avoidHighways", ArgumentSemantic.Assign)]
		bool AvoidHighways { get; set; }

		[Export ("avoidFerryLines", ArgumentSemantic.Assign)]
		bool AvoidFerryLines { get; set; }

		[Export ("avoidBicycleWalk", ArgumentSemantic.Assign)]
		bool AvoidBicycleWalk { get; set; }

		[Export ("avoidBicycleCarry", ArgumentSemantic.Assign)]
		bool AvoidBicycleCarry { get; set; }

		[Export ("requestCountryCodes", ArgumentSemantic.Assign)]
		bool RequestCountryCodes { get; set; }

		[Export ("requestAdvices", ArgumentSemantic.Assign)]
		bool RequestAdvices { get; set; }


		[Export ("numberOfRoutes", ArgumentSemantic.Assign)]
		uint NumberOfRoutes { get; set; }

		[Export ("alternativeRoutesModes", ArgumentSemantic.Retain)]
		SKRouteAlternativeSettings [] AlternativeRoutesModes { get; set; }

		[Export ("filterAlternatives", ArgumentSemantic.Assign)]
		bool FilterAlternatives { get; set; }


		[Export ("useSlopes", ArgumentSemantic.Assign)]
		bool UseSlopes { get; set; }

		[Export ("requestExtendedRoutePointsInfo", ArgumentSemantic.Assign)]
		bool RequestExtendedRoutePointsInfo { get; set; }

		[Export ("downloadRouteCorridor", ArgumentSemantic.Assign)]
		bool DownloadRouteCorridor { get; set; }

		[Export ("routeCorridorWidth", ArgumentSemantic.Assign)]
		int RouteCorridorWidth { get; set; }

		[Export ("waitForCorridorDownload", ArgumentSemantic.Assign)]
		bool WaitForCorridorDownload { get; set; }

		[Export ("destinationIsPoint", ArgumentSemantic.Assign)]
		bool DestinationIsPoint { get; set; }


		[Static, Export ("routeSettings")]
		SKRouteSettings RouteSettings { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKRouteInformation {
		[Export ("routeID", ArgumentSemantic.Assign)]
		uint RouteID { get; }

		[Export ("distance", ArgumentSemantic.Assign)]
		int Distance { get; }

		[Export ("estimatedTime", ArgumentSemantic.Assign)]
		int EstimatedTime { get; }

		[Export ("corridorIsDownloaded", ArgumentSemantic.Assign)]
		bool CorridorIsDownloaded { get; }

		[Export ("calculatedAfterRerouting", ArgumentSemantic.Assign)]
		bool CalculatedAfterRerouting { get; }

		[Export ("containsHighways", ArgumentSemantic.Assign)]
		bool ContainsHighways { get; }

		[Export ("containsTollRoads", ArgumentSemantic.Assign)]
		bool ContainsTollRoads { get; }

		[Export ("containsFerryLines", ArgumentSemantic.Assign)]
		bool ContainsFerryLines { get; }
	}

	[BaseType (typeof (NSObject),
		Delegates=new string [] {"WeakRoutingDelegate"},
		Events=new Type [] { typeof (SKRoutingDelegate) })]
	public partial interface SKRoutingService {

		[Static, Export ("sharedInstance")]
		SKRoutingService SharedInstance { get; }

		[Export ("routingDelegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakRoutingDelegate { get; set; }

		[Wrap ("WeakRoutingDelegate")][NullAllowed]
		SKRoutingDelegate RoutingDelegate { get; set; }

		[Export ("mapView", ArgumentSemantic.Assign)]
		SKMapView MapView { get; set; }

		[Export ("calculateRoute:")]
		void CalculateRoute (SKRouteSettings route);

		[Export ("zoomToRouteWithInsets:")]
		void ZoomToRouteWithInsets (UIEdgeInsets insets);
	}


	[BaseType (typeof (NSObject))]
	public partial interface SKSearchResultParent {

		[Export ("parentIndex")]
		int ParentIndex { get; set; }

		[Export ("type")]
		SKSearchResultType Type { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Static, Export ("searchResultParentWithIndex:type:name:")]
		SKSearchResultParent CreateSearchResultParent(int index, int type, string name);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapPOI {

		[Export ("category")]
		SKPOICategory Category { get; set; }

		[Export ("name", ArgumentSemantic.Retain)]
		string Name { get; set; }

		[Export ("coordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Export ("parentSearchResults", ArgumentSemantic.Retain)]
		SKSearchResultParent [] ParentSearchResults { get; set; }

		[Static, Export ("mapPOI")]
		SKMapPOI MapPOI { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKAnnotationView {

		[Export ("view", ArgumentSemantic.Retain)]
		UIView View { get; }

		[Export ("reuseIdentifier", ArgumentSemantic.Retain)]
		string ReuseIdentifier { get; }

		[Export ("initWithView:reuseIdentifier:")]
		IntPtr Constructor (UIView view, string reuseIdentifier);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKAnnotation {

		[Export ("identifier", ArgumentSemantic.Assign)]
		int Identifier { get; set; }

		[Export ("imagePath", ArgumentSemantic.Retain)]
		string ImagePath { get; set; }

		[Export ("imageSize", ArgumentSemantic.Assign)]
		int ImageSize { get; set; }

		[Export ("annotationType")]
		SKAnnotationType AnnotationType { get; set; }

		[Export ("offset", ArgumentSemantic.Assign)]
		PointF Offset { get; set; }

		[Export ("location", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Location { get; set; }

		[Export ("minZoomLevel", ArgumentSemantic.Assign)]
		int MinZoomLevel { get; set; }

		[Export ("annotationView", ArgumentSemantic.Retain)]
		SKAnnotationView AnnotationView { get; set; }

		[Static, Export ("annotation")]
		SKAnnotation Annotation { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapCustomPOI {

		[Export ("identifier")]
		int Identifier { get; set; }

		[Export ("coordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Coordinate { get; set; }

		[Export ("type")]
		SKPOIType Type { get; set; }

		[Export ("categoryID")]
		SKPOICategory CategoryID { get; set; }

		[Export ("minZoomLevel")]
		int MinZoomLevel { get; set; }

		[Static, Export ("mapCustomPOI")]
		SKMapCustomPOI MapCustomPOI { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKPOICluster {

		[Export ("customPOIList", ArgumentSemantic.Retain)]
		NSNumber [] CustomPOIList { get; set; }

		[Export ("mapPOIList", ArgumentSemantic.Retain)]
		SKMapPOI [] MapPOIList { get; set; }

		[Export ("isCustomPOICluster")]
		bool IsCustomPOICluster { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model, Protocol]
	public partial interface SKMapViewDelegate {

		[Export ("mapView:didStartRegionChangeFromRegion:"), EventArgs("MapCoordinateRegion")]
		void DidStartRegionChangeFromRegion (SKMapView mapView, SKCoordinateRegion region);

		[Export ("mapView:didChangeToRegion:"), EventArgs("MapCoordinateRegion")]
		void DidChangeToRegion (SKMapView mapView, SKCoordinateRegion region);

		[Export ("mapView:didEndRegionChangeToRegion:"), EventArgs("MapCoordinateRegion")]
		void DidEndRegionChangeToRegion (SKMapView mapView, SKCoordinateRegion region);

		[Export ("mapView:didTapAtCoordinate:"), EventArgs("MapLocation")]
		void DidTapAtCoordinate (SKMapView mapView, CLLocationCoordinate2D coordinate);

		[Export ("mapView:didLongTapAtCoordinate:"), EventArgs("MapLocation")]
		void DidLongTapAtCoordinate (SKMapView mapView, CLLocationCoordinate2D coordinate);

		[Export ("mapView:didDoubleTapAtCoordinate:"), EventArgs("MapLocation")]
		void DidDoubleTapAtCoordinate (SKMapView mapView, CLLocationCoordinate2D coordinate);

		[Export ("mapView:didDoubleTouchAtPoint:"), EventArgs("MapPoint")]
		void DidDoubleTouchAtPoint (SKMapView mapView, PointF point);

		[Export ("mapView:didPanFromPoint:toPoint:"), EventArgs("MapPoints")]
		void DidPanFromPoint (SKMapView mapView, PointF fromPoint, PointF toPoint);

		[Export ("mapView:didPinchWithScale:"), EventArgs("MapScale")]
		void DidPinchWithScale (SKMapView mapView, float scale);

		[Export ("mapView:didRotateWithAngle:"), EventArgs("MapAngle")]
		void DidRotateWithAngle (SKMapView mapView, float angle);

		[Export ("mapViewWillRequireOnlineConnection:")]
		void WillRequireOnlineConnection (SKMapView mapView);

		[Export ("mapView:didSelectMapPOI:"), EventArgs("MapPOI")]
		void DidSelectMapPOI (SKMapView mapView, SKMapPOI mapPOI);

		[Export ("mapView:didSelectAnnotation:"), EventArgs("MapAnnotation")]
		void DidSelectAnnotation (SKMapView mapView, SKAnnotation annotation);

		[Export ("mapView:didSelectCustomPOI:"), EventArgs("MapCustomPOI")]
		void DidSelectCustomPOI (SKMapView mapView, SKMapCustomPOI customPOI);

		[Export ("mapView:didSelectPOICluster:"), EventArgs("MapPOICluster")]
		void DidSelectPOICluster (SKMapView mapView, SKPOICluster poiCluster);

		[Export ("mapViewDidSelectCompass:")]
		void DidSelectCompass (SKMapView mapView);

		[Export ("mapViewDidSelectCurrentPositionIcon:")]
		void DidSelectCurrentPositionIcon (SKMapView mapView);

		[Export ("mapView:calloutViewForLocation:"), DelegateName("GetCalloutViewForLocation"), NoDefaultValue]
		UIView CalloutViewForLocation (SKMapView mapView, CLLocationCoordinate2D location);

		[Export ("mapView:calloutViewForAnnotation:"), DelegateName ("GetCalloutViewForAnnotation"), NoDefaultValue]
		UIView CalloutViewForAnnotation (SKMapView mapView, SKAnnotation annotation);

		[Export ("mapViewDidTapAttribution:")]
		void DidTapAttribution (SKMapView mapView);

		[Export ("mapView:didSelectOverlayWithId:atLocation:"), EventArgs("MapOverlay")]
		void DidSelectOverlayWithId (SKMapView mapView, int overlayId, CLLocationCoordinate2D location);

		[Export ("mapViewDidFinishRenderingImageInBoundingBox:")]
		void DidFinishRenderingImageInBoundingBox (SKMapView mapView);
	}

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

		[Static, Export ("mapInternationalization")]
		SKMapInternationalizationSettings MapInternationalization { get; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKMapSettings {

		[Export ("rotationEnabled")]
		bool RotationEnabled { get; set; }

		[Export ("panningEnabled")]
		bool PanningEnabled { get; set; }

		[Export ("zoomWithCenterAnchor")]
		bool ZoomWithCenterAnchor { get; set; }

		[Export ("inertiaEnabled")]
		bool InertiaEnabled { get; set; }

		[Export ("followerMode")]
		SKMapFollowerMode FollowerMode { get; set; }

		[Export ("showCompass")]
		bool ShowCompass { get; set; }

		[Export ("showAccuracyCircle")]
		bool ShowAccuracyCircle { get; set; }

		[Export ("compassOffset", ArgumentSemantic.Assign)]
		PointF CompassOffset { get; set; }

		[Export ("orientationIndicatorType")]
		SKOrientationIndicatorType OrientationIndicatorType { get; set; }

		[Export ("displayMode")]
		SKMapDisplayMode DisplayMode { get; set; }

		[Export ("poiDisplayingOption")]
		SKPOIDisplayingOption PoiDisplayingOption { get; set; }

		[Export ("mapInternationalization", ArgumentSemantic.Retain)]
		SKMapInternationalizationSettings MapInternationalization { get; set; }

		[Export ("zoomLimits", ArgumentSemantic.Assign)]
		SKMapZoomLimits ZoomLimits { get; set; }

		[Export ("annotationTapZoomLimit")]
		float AnnotationTapZoomLimit { get; set; }

		[Export ("showCurrentPosition")]
		bool ShowCurrentPosition { get; set; }

		[Export ("showStreetNamePopUps")]
		bool ShowStreetNamePopUps { get; set; }

		[Export ("showBicycleLanes")]
		bool ShowBicycleLanes { get; set; }

		[Export ("showHouseNumbers")]
		bool ShowHouseNumbers { get; set; }

		[Export ("showOneWays")]
		bool ShowOneWays { get; set; }

		[Export ("showStreetBadges")]
		bool ShowStreetBadges { get; set; }

		[Export ("showMapPoiIcons")]
		bool ShowMapPoiIcons { get; set; }

		[Export ("osmAttributionPosition")]
		SKAttributionPosition OsmAttributionPosition { get; set; }

		[Export ("companyAttributionPosition")]
		SKAttributionPosition CompanyAttributionPosition { get; set; }

		[Export ("drawingOrderType")]
		SKDrawingOrderType DrawingOrderType { get; set; }

		[Static, Export ("mapSettings")]
		SKMapSettings MapSettings { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model, Protocol]
	public partial interface SKCalloutViewDelegate {

		[Export ("calloutView:didTapLeftButton:"), EventArgs("CalloutViewLeftButtonTapped")]
		void DidTapLeftButton (SKCalloutView calloutView, UIButton leftButton);

		[Export ("calloutView:didTapRightButton:"), EventArgs("CalloutViewRightButtonTapped")]
		void DidTapRightButton (SKCalloutView calloutView, UIButton rightButton);
	}

	[BaseType (typeof (UIView),
		Delegates=new string [] {"WeakDelegate"},
		Events=new Type [] { typeof (SKCalloutViewDelegate) })]
	public partial interface SKCalloutView {

		[Export ("initWithFrame:")]
		SKCalloutView Constructor (RectangleF frame);

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")][NullAllowed]
		SKCalloutViewDelegate Delegate { get; set; }

		[Export ("location", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D Location { get; set; }

		[Export ("calloutOffset", ArgumentSemantic.Assign)]
		PointF CalloutOffset { get; set; }

		[Export ("dynamicArrowPositioning")]
		bool DynamicArrowPositioning { get; set; }

		[Export ("middleImageView", ArgumentSemantic.Retain)]
		UIImageView MiddleImageView { get; set; }

		[Export ("leftButton", ArgumentSemantic.Retain)]
		UIButton LeftButton { get; set; }

		[Export ("rightButton", ArgumentSemantic.Retain)]
		UIButton RightButton { get; set; }

		[Export ("titleLabel", ArgumentSemantic.Retain)]
		UILabel TitleLabel { get; set; }

		[Export ("subtitleLabel", ArgumentSemantic.Retain)]
		UILabel SubtitleLabel { get; set; }

		[Static, Export ("calloutView")]
		SKCalloutView CalloutView { get; }
	}

	[BaseType (typeof (UIView))]
	public partial interface SKMapScaleView {

		[Export ("initWithFrame:")]
		SKMapScaleView Constructor (RectangleF frame);

		[Export ("scale")]
		float Scale { get; set; }

		[Export ("distanceFormat")]
		SKDistanceFormat DistanceFormat { get; set; }

		[Export ("scaleColor", ArgumentSemantic.Retain)]
		UIColor ScaleColor { get; set; }

		[Export ("textColor", ArgumentSemantic.Retain)]
		UIColor TextColor { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKBoundingBox {

		[Export ("topLeftCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D TopLeftCoordinate { get; set; }

		[Export ("bottomRightCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D BottomRightCoordinate { get; set; }

		[Export ("containsLocation:")]
		bool ContainsLocation (CLLocationCoordinate2D location);

		[Export ("boundingBoxIncludingLocation:")]
		SKBoundingBox BoundingBoxIncludingLocation (CLLocationCoordinate2D location);

		[Static, Export ("boundingBoxWithTopLeftCoordinate:bottomRightCoordinate:")]
		SKBoundingBox BoundingBoxWithTopLeftCoordinate (CLLocationCoordinate2D topLeft, CLLocationCoordinate2D bottomRight);

		[Static, Export ("boundingBoxForRegion:inMapViewWithSize:")]
		SKBoundingBox BoundingBoxForRegion (SKCoordinateRegion region, SizeF size);
	}

	[BaseType (typeof (NSObject))]
	public partial interface SKAnimationSettings {

		[Export ("animationType")]
		SKAnimationType AnimationType { get; set; }

		[Export ("animationEasingType")]
		SKAnimationEasingType AnimationEasingType { get; set; }

		[Export ("duration")]
		int Duration { get; set; }

		[Static, Export ("defaultAnimationSettings")]
		SKAnimationSettings DefaultAnimationSettings { get; }

		[Static, Export ("animationSettings")]
		SKAnimationSettings AnimationSettings { get; }
	}

	[BaseType (typeof (SKAnimationSettings))]
	public partial interface SKPulseAnimationSettings {

		[Export ("color", ArgumentSemantic.Retain)]
		UIColor Color { get; set; }

		[Export ("isContinuous")]
		bool IsContinuous { get; set; }

		[Export ("span")]
		float Span { get; set; }

		[Export ("fadeOutTime")]
		float FadeOutTime { get; set; }

		[Static, Export ("pulseAnimationSettings")]
		SKPulseAnimationSettings PulseAnimationSettings { get; }
	}


	[BaseType (typeof (NSObject))]
	public partial interface SKOverlay {

		[Export ("fillColor", ArgumentSemantic.Retain)]
		UIColor FillColor { get; set; }

		[Export ("borderDotsSize")]
		int BorderDotsSize { get; set; }

		[Export ("borderDotsSpacingSize")]
		int BorderDotsSpacingSize { get; set; }
	}

	[BaseType (typeof (SKOverlay))]
	public partial interface SKPolygon {

		[Export ("coordinates", ArgumentSemantic.Retain) ]
		CLLocation	[] Coordinates { get; set; }

		[Export ("strokeColor", ArgumentSemantic.Retain)]
		UIColor StrokeColor { get; set; }

		[Export ("borderWidth")]
		int BorderWidth { get; set; }

		[Export ("isMask")]
		bool IsMask { get; set; }

		[Export ("maskedObjectScale")]
		float MaskedObjectScale { get; set; }

	}

	[BaseType (typeof (SKOverlay))]
	public partial interface SKPolyline {

		[Export ("coordinates", ArgumentSemantic.Retain)]
		CLLocation [] Coordinates { get; set; }

		[Export ("lineWidth")]
		int LineWidth { get; set; }

		[Export ("backgroundLineWidth")]
		int BackgroundLineWidth { get; set; }

	}


	[BaseType (typeof (SKOverlay))]
	public partial interface SKCircle {

		[Export ("centerCoordinate", ArgumentSemantic.Assign)]
		CLLocationCoordinate2D CenterCoordinate { get; set; }

		[Export ("radius")]
		float Radius { get; set; }

		[Export ("strokeColor", ArgumentSemantic.Retain)]
		UIColor StrokeColor { get; set; }

		[Export ("borderWidth")]
		int BorderWidth { get; set; }

		[Export ("isMask")]
		bool IsMask { get; set; }

		[Export ("maskedObjectScale")]
		float MaskedObjectScale { get; set; }

		[Export ("numberOfPoints")]
		int NumberOfPoints { get; set; }

	}



	[BaseType (typeof (UIView),
		Delegates=new string [] {"WeakDelegate"},
		Events=new Type [] { typeof (SKMapViewDelegate) })]
	public partial interface SKMapView {

		[Export ("initWithFrame:")]
		SKMapView Constructor (RectangleF frame);

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")][NullAllowed]
		SKMapViewDelegate Delegate { get; set; }

		[Export ("settings", ArgumentSemantic.Retain)]
		SKMapSettings Settings { get; }

		[Export ("visibleRegion", ArgumentSemantic.Assign)]
		SKCoordinateRegion VisibleRegion { get; set; }

		[Export ("bearing")]
		float Bearing { get; set; }

		[Export ("enabledRendering")]
		bool EnabledRendering { get; set; }

		[Export ("isInNavigationMode")]
		bool IsInNavigationMode { get; }

		[Export ("calloutView", ArgumentSemantic.Retain)]
		SKCalloutView CalloutView { get; set; }

		[Export ("mapScaleView")]
		SKMapScaleView MapScaleView { get; }

		[Export ("showDebugView")]
		bool ShowDebugView { get; set; }

		[Export ("currentPositionView", ArgumentSemantic.Retain)]
		UIView CurrentPositionView { get; set; }

		[Export ("applySettingsFromFileAtPath:")]
		void ApplySettingsFromFileAtPath (string filePath);

		[Export ("centerOnCurrentPosition")]
		void CenterOnCurrentPosition ();

		[Export ("animateToZoomLevel:")]
		void AnimateToZoomLevel (float zoom);

		[Export ("animateToBearing:")]
		void AnimateToBearing (float bearing);

		[Export ("animateToLocation:withDuration:")]
		void AnimateToLocation (CLLocationCoordinate2D location, float duration);

		[Export ("fitBounds:withPadding:")]
		void FitBounds (SKBoundingBox boundingBox, SizeF padding);

		[Export ("pointForCoordinate:")]
		PointF PointForCoordinate (CLLocationCoordinate2D location);

		[Export ("coordinateForPoint:")]
		CLLocationCoordinate2D CoordinateForPoint (PointF point);

		[Export ("addAnnotation:withAnimationSettings:")]
		bool AddAnnotation (SKAnnotation annotation, SKAnimationSettings animationSettings);

		[Export ("addCustomPOI:")]
		bool AddCustomPOI (SKMapCustomPOI customPOI);

		[Export ("bringToFrontAnnotation:")]
		bool BringToFrontAnnotation (SKAnnotation annotation);

		[Export ("updateAnnotation:")]
		bool UpdateAnnotation (SKAnnotation annotation);

		[Export ("annotationForIdentifier:")]
		SKAnnotation AnnotationForIdentifier (int identifier);

		[Export ("removeAnnotationWithID:")]
		void RemoveAnnotationWithID (int identifier);

		[Export ("clearAllAnnotations")]
		void ClearAllAnnotations ();

		[Export ("showCalloutAtLocation:withOffset:animated:")]
		void ShowCalloutAtLocation (CLLocationCoordinate2D coordinate, PointF calloutOffset, bool shouldAnimate);

		[Export ("showCalloutForAnnotation:withOffset:animated:")]
		void ShowCalloutForAnnotation (SKAnnotation annotation, PointF calloutOffset, bool shouldAnimate);

		[Export ("hideCallout")]
		void HideCallout ();

		[Export ("renderMapImageInBoundingBox:toPath:withSize:")]
		void RenderMapImageInBoundingBox (SKBoundingBox boundingBox, string imagePath, SizeF size);

		[Export ("lastRenderedFrame")]
		UIImage LastRenderedFrame { get; }

		[Export ("startPulseAnnimation:")]
		bool StartPulseAnnimation (SKPulseAnimationSettings pulseAnimationSettings);

		[Export ("stopPulseAnnimation")]
		bool StopPulseAnnimation { get; }

	
		[Static, Export ("mapStyle") ]
		SKMapViewStyle MapStyle { get; }

		[Static, Export ("setMapStyle:")]
		bool SetMapStyle (SKMapViewStyle mapStyle);

		[Static, Export ("parseAlternativeMapStyle:asynchronously:")]
		bool ParseAlternativeMapStyle (SKMapViewStyle alternativeStyle, bool asynchronously);

		[Static, Export ("loadAlternativeMapStyle:")]
		bool LoadAlternativeMapStyle (SKMapViewStyle alternativeStyle);

		[Static, Export ("useAlternativeMapStyle:")]
		bool UseAlternativeMapStyle (bool useAlternative);

		[Static, Export ("unloadAlternativeMapStyle")]
		void UnloadAlternativeMapStyle ();

		[Static, Export ("removeStyle:")]
		void RemoveStyle (SKMapViewStyle alternativeStyle);

		/// <summary>
		/// Adds a polygon overlay on the map.
		/// </summary>
		/// <returns>The unique identifier of the added polygon. Can be used for removing the overlay. </returns>
		/// <param name="polygon">Stores all the information about the polygon.</param>
		[Export ("addPolygon:")]
		int AddPolygon (SKPolygon polygon);

		/// <summary>
		/// Adds a polyline overlay on the map.
		/// </summary>
		/// <returns>The unique identifier of the added polyline. Can be used for removing the overlay.</returns>
		/// <param name="polyline">Polyline.</param>
		[Export ("addPolyline:")]
		int AddPolyline (SKPolyline polyline);

		/// <summary>
		/// Adds a circle overlay on the map. 
		/// </summary>
		/// <returns>The unique identifier of the added circle. Can be used for removing the overlay.</returns>
		/// <param name="circle">Stores all the information about the circle.</param>
		[Export ("addCircle:")]
		int AddCircle (SKCircle circle);

		/// <summary>
		/// Removes an overlay from the map. 
		/// </summary>
		/// <returns><c>true</c>, if overlay with overlayId was cleared, <c>false</c> otherwise.</returns>
		/// <param name="overlayID">The id of the overlay that needs to be deleted.</param>
		[Export ("clearOverlayWithID:")]
		bool ClearOverlayWithID (int overlayID);

		/// <summary>
		/// Clears all overlays.
		/// </summary>
		[Export ("clearAllOverlays")]
		void ClearAllOverlays ();
	}
}

