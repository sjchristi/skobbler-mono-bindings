using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Json;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using SKMaps;

namespace SKMapUtil
{
	/// <summary>
	/// This class is used to wrap the elements of the Skobbler 'maps.json' file -- information about
	/// the maps that the user can download for offline use.
	/// </summary>
	public class OfflineMapInfo
	{
		// Type of info this element holds
		public enum PackageType
		{
			Country = 0,
			City,
			Continent,
			Region,
			State
		}

		// State of this map on the users local device
		public enum PackageState
		{
			NotInstalled,
			Installing,
			Installed,
			InstallError,
			InstallCancelled
		}

		// The version of the maps JSON file loaded
		protected static string _offlineMapVersion;
		public static string OfflineMapVersion {
			get {
				return _offlineMapVersion;
			}
		}

		protected static List<OfflineMapInfo> _offlineMapPackages;
		public static List<OfflineMapInfo> OfflineMapPackages {
			get {
				return _offlineMapPackages;
			}
		}

		protected static Dictionary<string, OfflineMapInfo> _offlineMapPackagesDict;
		public static Dictionary<string, OfflineMapInfo> OfflineMapPackagesDict {
			get {
				return _offlineMapPackagesDict;
			}
		}

		// Given skobbler region code, returns the OfflineMapInfo associated with
		// the code, or returns null if not found.
		public static OfflineMapInfo OfflineMapInfoForCode(string code) {
			OfflineMapInfo info = null;
			if (_offlineMapPackagesDict.ContainsKey (code)) {
				info = _offlineMapPackagesDict [code];
			}
			return info;
		}

		protected static OfflineMapInfo _world = null;
		public static OfflineMapInfo World {
			get {
				return _world;
			}
		}

		protected static Dictionary<string,SKMapPackage> _installedMapsDict = null;
		public static Dictionary<string,SKMapPackage> InstalledMapsDict {
			get {
				return _installedMapsDict;
			}
		}

		public static bool WorldLoaded {
			get {
				return _world != null;
			}
		}

		public static IEnumerable<OfflineMapInfo> InstalledMaps {
			get {
				List<OfflineMapInfo> mapInfos = new List<OfflineMapInfo> (_installedMapsDict.Count);

				foreach (var k in _installedMapsDict) {
					OfflineMapInfo i = OfflineMapInfo.OfflineMapInfoForCode(k.Key);
					mapInfos.Add (i);
				}

				mapInfos.Sort ((OfflineMapInfo x, OfflineMapInfo y) => {
					return string.Compare(x.LocalizedDescription, y.LocalizedDescription, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace);
				});

				return mapInfos;
			}
		}

		public static void RefreshInstalledMaps()
		{
			SKMapPackage [] downloadedPackages = SKMaps.SKMapsService.SharedInstance.PackagesManager.InstalledOfflineMapPackages;
			Dictionary<string, SKMapPackage> installedMaps = new Dictionary<string, SKMapPackage> (downloadedPackages.Length);
			Dictionary<string, SKMapPackage> previouslyInstalledMaps = _installedMapsDict;

			if (previouslyInstalledMaps == null) {
				previouslyInstalledMaps = new Dictionary<string, SKMapPackage> ();
			}

			foreach (SKMapPackage p in downloadedPackages) {
				string code = p.Name;
				installedMaps.Add (code, p);
			}

			Dictionary<string, SKMapPackage> removedMaps = previouslyInstalledMaps.Where (x => !installedMaps.ContainsKey (x.Key)).ToDictionary (x => x.Key, x => x.Value);
			Dictionary<string, SKMapPackage> newlyInstalledMaps = installedMaps.Where (x => !previouslyInstalledMaps.ContainsKey (x.Key)).ToDictionary (x => x.Key, x => x.Value);

			foreach (var kvp in removedMaps) {
				OfflineMapInfo info = null;
				_offlineMapPackagesDict.TryGetValue (kvp.Key, out info);
				if (info != null) {
					info.SetState (PackageState.NotInstalled);
				}
			}

			foreach (var kvp in newlyInstalledMaps) {
				OfflineMapInfo info = null;
				_offlineMapPackagesDict.TryGetValue (kvp.Key, out info);
				if (info != null) {
					info.SetState (PackageState.Installed);
				}
			}

			_installedMapsDict = installedMaps;

			foreach (var kvp in removedMaps) {
				OfflineMapInfo info = null;
				_offlineMapPackagesDict.TryGetValue (kvp.Key, out info);
				if (info != null) {
					RaiseMapInfoUninstalled (info);
				}
			}

			foreach (var kvp in newlyInstalledMaps) {
				OfflineMapInfo info = null;
				_offlineMapPackagesDict.TryGetValue (kvp.Key, out info);
				if (info != null) {
					RaiseMapInfoInstalled (info);
				}
			}
		}

		public static void LoadWorld(Action<bool> worldLoadedCallback, Action<double> worldLoadedProgressCallback)
		{
			if (_world != null) {
				if (worldLoadedCallback != null) {
					worldLoadedCallback (true);
				}
			} else {
				// SAMCTODO:  Not thread safe and can be re-entrant which will cause all sorts of issues...
				string urlPath = SKMaps.SKMapsService.SharedInstance.PackagesManager.MapsJSONURLForVersion (null);
				string pathVersion = Path.GetFileName (Path.GetDirectoryName (urlPath));
				string destFilePath = FileExtensions.GetFilePath(Path.Combine(pathVersion, "maps.json"));
				string destFileDirectory = Path.GetDirectoryName (destFilePath);

				if (!Directory.Exists (destFileDirectory)) {
					Directory.CreateDirectory (destFileDirectory);
				}

				if (File.Exists (destFilePath)) {
					LoadWorldJSONAsync (destFilePath, worldLoadedCallback);
				} else {
					INetworkFileTransfer dl = Network.HttpDownloadFile (urlPath, destFilePath);

					dl.ProgressChanged += (object sender, INetworkFileTransfer e) => {
						Console.WriteLine ("Downloading file %{0}", (e.Progress * 100.0).ToString ("N1"));

						if (worldLoadedProgressCallback != null)
						{
							worldLoadedProgressCallback.Invoke(e.Progress);
						}
					};

					dl.TransferCompleted += (object sender, INetworkFileTransfer e) => {
						Console.WriteLine ("Finished downloading file: {0}", e.TargetPath);

						if (!e.Cancelled && e.Error == null) {
							LoadWorldJSONAsync(e.TargetPath, worldLoadedCallback);
						} else {
							if (e.Error != null) {
								worldLoadedCallback(false);
							}
						}
					};
				}
			}
		}

		protected static void LoadWorldJSONAsync(string filePath, Action<bool> worldLoadedCallback)
		{
			// do as task because reading can take some time...
			Task.Factory.StartNew (() => {
				LoadWorldJSON (filePath);

				if (worldLoadedCallback != null)
				{
					worldLoadedCallback.Invoke(true);
				}
			});
		}

		protected static bool LoadWorldJSON(string filePath)
		{
			JsonObject mapsJson = null;

			try {
				using (var strm = new StreamReader(filePath)) {
					mapsJson = JsonObject.Load (strm) as JsonObject;
					strm.Close();
				}
			} catch (Exception ex) {
				Console.WriteLine ("Caught exception while reading map JSON:");
				Console.WriteLine (ex.ToString ());
				mapsJson = new JsonObject ();
			}

			string version = mapsJson["version"];
			string xmlVersion = mapsJson["xmlVersion"];
			JsonArray packages = mapsJson["packages"] as JsonArray;
			JsonObject world = mapsJson["world"] as JsonObject;
			JsonArray continents = world["continents"] as JsonArray;

			List<OfflineMapInfo> packagesList = new List<OfflineMapInfo> (packages.Count);
			Dictionary<string,OfflineMapInfo> packagesDict = new Dictionary<string,OfflineMapInfo>(packages.Count);

			OfflineMapInfo worldMapInfo = new OfflineMapInfo("World");

			packagesDict.Add("world", worldMapInfo);

			foreach (JsonObject package in packages)
			{
				OfflineMapInfo mapPackage = new OfflineMapInfo();

				{
					string packageCode = package["packageCode"];
					int type = package["type"];
					JsonArray languages = package["languages"] as JsonArray;

					if (package.ContainsKey ("file")) {
						string file = package["file"];
						long skmsize = package["skmsize"];
						long size = package["size"];
						long unzipsize = package["unzipsize"];

						mapPackage.file = file;
						mapPackage.skmsize = skmsize;
						mapPackage.size = size;
						mapPackage.unzipSize = unzipsize;
					}

					if (package.ContainsKey ("nbzip")) {
						string nbzip = package["nbzip"];
						mapPackage.nbZip = nbzip;
					}

					foreach (JsonObject language in languages)
					{
						string name = language["tlName"];
						string languageCode = language["lngCode"];

						mapPackage.localizedName.Add(languageCode, name.Trim());
					}

					mapPackage.code = packageCode;
					mapPackage.type = (PackageType)type;

					packagesDict[packageCode] = mapPackage;
					packagesList.Add (mapPackage);
				}

				if (package.ContainsKey("texture"))
				{
					JsonObject texture = package["texture"] as JsonObject;
					string file = texture["file"];
					long size = texture["size"];
					long unzipsize = texture["unzipsize"];
					string bigfile = texture["texturesbigfile"];
					long sizebigfile = texture["sizebigfile"];

					mapPackage.texture = new Texture ();
					mapPackage.texture.file = file;
					mapPackage.texture.size = size;
					mapPackage.texture.unzipSize = unzipsize;
					mapPackage.texture.bigFile = bigfile;
					mapPackage.texture.bigFileSize = sizebigfile;
				}

				if (package.ContainsKey("elevation"))
				{
					JsonObject elevation = package["elevation"] as JsonObject;
					string file = elevation["file"];
					long size = elevation["size"];
					long unzipsize = elevation["unzipsize"];

					mapPackage.elevation = new Elevation();
					mapPackage.elevation.file = file;
					mapPackage.elevation.size = size;
					mapPackage.elevation.unzipSize = unzipsize;
				}

				if (package.ContainsKey("bbox"))
				{
					JsonObject bbox = package["bbox"] as JsonObject;
					double longMin = bbox["longMin"];
					double longMax = bbox["longMax"];
					double latMin = bbox["latMin"];
					double latMax = bbox["latMax"];

					mapPackage.bbox = new BBox ();
					mapPackage.bbox.longMin = longMin;
					mapPackage.bbox.longMax = longMax;
					mapPackage.bbox.latMax = latMax;
					mapPackage.bbox.latMin = latMin;
				}
			}

			foreach (JsonObject continent in continents)
			{
				string continentCode = continent["continentCode"];
				JsonArray countries = continent["countries"] as JsonArray;

//				Console.WriteLine("Continent: {0}", continentCode);

				OfflineMapInfo continentPackage = packagesDict [continentCode];

				worldMapInfo.children.Add(continentPackage);

				foreach (JsonObject country in countries)
				{
					string countryCode = country["countryCode"];
//					Console.WriteLine("    {0}", countryCode);

					OfflineMapInfo countryPackage = packagesDict [countryCode];

					continentPackage.children.Add (countryPackage);

					if (country.ContainsKey("cityCodes"))
					{
						JsonArray cities = country["cityCodes"] as JsonArray;

						foreach (JsonObject city in cities)
						{
							string cityCode = city["cityCode"];
							OfflineMapInfo cityPackage = packagesDict [cityCode];
							cityPackage.parent = countryPackage;
							//							continentPackage.children.Add (cityPackage);
//							Console.WriteLine("        City: {0}", cityCode);
						}
					}

					if (country.ContainsKey("stateCodes"))
					{
						JsonArray states = country["stateCodes"] as JsonArray;
						foreach (JsonObject state in states)
						{
							string stateCode = state["stateCode"];
//							Console.WriteLine("        State: {0}", stateCode);
							OfflineMapInfo statePackage = packagesDict [stateCode];
							countryPackage.children.Add (statePackage);
							statePackage.parent = countryPackage;
						}
					}
				}
			}

			Console.WriteLine("Version: {0}", version);

			_offlineMapVersion = version;

			packagesList.Sort ((OfflineMapInfo x, OfflineMapInfo y) => {
				return string.Compare(x.LocalizedDescription, y.LocalizedDescription, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace);
			});
			_offlineMapPackages = packagesList;
			_offlineMapPackagesDict = packagesDict;
			_world = worldMapInfo;
			RefreshInstalledMaps ();

			return true;
		}

		public static event EventHandler<OfflineMapInfo> AnyMapInfoChanged;

		protected static void RaiseAnyMapInfoChanged(OfflineMapInfo info)
		{
			if (OfflineMapInfo.AnyMapInfoChanged != null) {
				OfflineMapInfo.AnyMapInfoChanged.Invoke (info, info);
			}
		}

		public static event EventHandler<OfflineMapInfo> MapInfoInstalled;

		protected static void RaiseMapInfoInstalled(OfflineMapInfo info)
		{
			if (OfflineMapInfo.MapInfoInstalled != null) {
				OfflineMapInfo.MapInfoInstalled.Invoke (info, info);
			}
		}

		public static event EventHandler<OfflineMapInfo> MapInfoUninstalled;

		protected static void RaiseMapInfoUninstalled(OfflineMapInfo info)
		{
			if (OfflineMapInfo.MapInfoUninstalled != null) {
				OfflineMapInfo.MapInfoUninstalled.Invoke (info, info);
			}
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public OfflineMapInfo(string name = null) {
			this.children = new List<OfflineMapInfo> ();
			this.localizedName = new Dictionary<string, string> ();
			this._state = PackageState.NotInstalled;

			if (name != null) {
				string code = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
				this.localizedName.Add (code, name.Trim());
			}
		}

		public event EventHandler<OfflineMapInfo> MapInfoChanged;

		protected void RaiseMapInfoChanged() {
			if (this.MapInfoChanged != null) {
				this.MapInfoChanged.Invoke (this, this);
			}

			RaiseAnyMapInfoChanged (this);
		}

		protected PackageState _state;
		public PackageState State {
			get {
				return _state;
			}
		}

		public Exception LastError { get; set; }

		public void SetState(PackageState newState) {
			if (_state != newState) {
				this._state = newState;
				this.RaiseMapInfoChanged ();
			}
		}

		public OfflineMapInfo parent;
		public List<OfflineMapInfo> children;

		public string code;
		public PackageType type;

		public bool Installed {
			get {
				return this._state == PackageState.Installed;
			}
		}

		public bool Installing {
			get {
				return this._state == PackageState.Installing;
			}
		}

		public string InstalledVersion {
			get {
				string version = null;
				if (InstalledMapsDict.ContainsKey (this.code)) {
					SKMapPackage p = InstalledMapsDict [this.code];
					version = p.Version;
				}
				return version;
			}
		}

		public bool Matches(string text) {
			return this.LocalizedDescription.MatchesIgnoreDiacritics (text);
		}

		public long InstalledSize {
			get {
				long size = 0;
				if (InstalledMapsDict.ContainsKey (this.code)) {
					SKMapPackage p = InstalledMapsDict [this.code];
					size = p.Size;
				}
				return size;
			}
		}

		public string LocalizedName {
			get {
				string code = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
				string name;

				if (this.localizedName.ContainsKey (code)) {
					name = this.localizedName [code];
				} else if (this.localizedName.ContainsKey ("en")) {
					name = this.localizedName ["en"];
				} else {
					name = "none";
				}
				return name;
			}
		}

		public string LocalizedDescription {
			get {
				string name = this.LocalizedName;
				string desc;

				if ( this.parent != null && 
					(this.type == PackageType.City || this.type == PackageType.State || this.type == PackageType.Region)) {
					desc = string.Format ("{0} / {1}", this.parent.LocalizedName, name);
				} else {
					desc = name;
				}

				return desc;
			}
		}

		protected INetworkFileTransfer _dlInfo;

		public string DownloadFileFolder {
			get {
				string relativePath = Path.Combine (OfflineMapInfo.OfflineMapVersion, this.code);
				return FileExtensions.GetFilePath(relativePath);
			}
		}

		public int DownloadSizeBytes {
			get {
				int size = (int)this.size;

				if (this.texture != null) {
					size += (int)this.texture.bigFileSize;
				}

				return size;
			}
		}

		public void DownloadAndInstallPackage()
		{
			SKMapPackageDownloadInfo info = SKMaps.SKMapsService.SharedInstance.PackagesManager.DownloadInfoForPackageWithCode (this.code);

			string targetDir = this.DownloadFileFolder;

			// make new temp directory if necessary..
			if (!Directory.Exists (targetDir)) {
				Directory.CreateDirectory (targetDir);
			}

			string[] urls = { info.NamebrowserFilesURL, info.MapURL, info.TextureURL };

			this._dlInfo = Network.HttpDownloadFiles (urls, targetDir, this.DownloadSizeBytes);

			this._dlInfo.ProgressChanged += (object sender, INetworkFileTransfer e) => {
				this.OnPackageFileDownloadProgress(this._dlInfo);
			};

			this._dlInfo.TransferCompleted += (object sender, INetworkFileTransfer e) => {
				this.OnPackageFileDownloaded(this._dlInfo);
			};

			this._dlInfo.UserToken = code;

			Console.WriteLine ("Downloading Files:\n  NB: {0}\n  Texture: {1}\n  Map: {2}\n", info.NamebrowserFilesURL, info.TextureURL, info.MapURL);

			this.SetState (PackageState.Installing);
		}

		public string InstallProgress {
			get {
				return string.Format("%{0}", (this._dlInfo.Progress * 100.0).ToString("N1"));
			}
		}

		public void CancelInstall()
		{
			this._dlInfo.Cancel();
		}

		public void OnPackageFileDownloaded(INetworkFileTransfer infoBase)
		{
			MonoNetFileTransferMultiDownload info = infoBase as MonoNetFileTransferMultiDownload;
			string code = info.UserToken as string;
			string targetDir = this.DownloadFileFolder;
			//SKMapPackageDownloadInfo packageInfo = SKMaps.SKMapsService.SharedInstance.PackagesManager.DownloadInfoForPackageWithCode (code);

			if (!info.Cancelled && info.Error == null) {

				string[] zipFiles = FileExtensions.FilesMatchingPattern (info.TargetPath, "*.zip");

				foreach (string zipFile in zipFiles) {
					FileExtensions.UnzipFileToFolder (zipFile, info.TargetPath);
					// SAMCTODO:  nuke zip file now??
				}

				string skmFile = FileExtensions.FilesMatchingPattern (info.TargetPath, "*.skm").First();

				if (SKMapsService.SharedInstance.PackagesManager.ValidateMapFileAtPath (skmFile)) {
					SKAddPackageResult result = SKMapsService.SharedInstance.PackagesManager.AddOfflineMapPackageNamed (code, targetDir);

					Console.WriteLine ("Result: {0}", result);

					if (result == SKAddPackageResult.Success) {
						RefreshInstalledMaps ();
					}
				} else {
					// Get rid of the files; they may be corrupted...
					FileExtensions.NukeFolder (info.TargetPath);
					this.LastError = new Exception ("Files failed validation; please try again.");
					this.SetState (PackageState.InstallError);
				}

			} else {
				// error or user cancelled?
				if (info.Cancelled) {
					this.SetState (PackageState.InstallCancelled);
				} else {
					this.LastError = info.Error;
					this.SetState (PackageState.InstallError);
				}
			}
		}

		public void OnPackageFileDownloadProgress(INetworkFileTransfer info)
		{
			Console.WriteLine ("{0}: %{1}", this.LocalizedName, (info.Progress * 100.0).ToString ("N1"));
			this.RaiseMapInfoChanged ();
		}

		public bool UninstallMapPackage()
		{
			bool uninstalled = SKMapsService.SharedInstance.PackagesManager.DeleteOfflineMapPackageNamed (this.code);

			if (uninstalled) {
				// This should automatically change state of this map...
				OfflineMapInfo.RefreshInstalledMaps ();
			}

			return uninstalled;
		}

		public Dictionary<string, string> localizedName;

		public string file;
		public long skmsize;
		public long size;
		public long unzipSize;
		public string nbZip;

		public class BBox
		{
			public double latMin;
			public double latMax;
			public double longMin;
			public double longMax;

			public bool ContainsPoint(double latitude, double longitude)
			{
				return (latitude >= latMin && latitude <= latMax &&
				longitude >= longMin && longitude <= longMax);
			}

			protected double ClosestPoint2D(double min, double max, double point)
			{
				double closest;

				if (point < min) {
					closest = min;
				} else if (point > max) {
					closest = max;
				} else {
					double dmin = Math.Abs (point - min);
					double dmax = Math.Abs (point - max);

					if (dmin < dmax) {
						closest = min;
					} else {
						closest = max;
					}
				}

				return closest;
			}

			public static double DistanceBetweenPoints(double lat1, double lon1, double lat2, double lon2)
			{
				double distance;

				// http://www.movable-type.co.uk/scripts/latlong.html
				// haversine distance...
				double dLat = (lat2-lat1).ToRadians();
				double dLon = (lon2-lon1).ToRadians();
				double a = Math.Sin(dLat/2) * Math.Sin(dLat/2) +
					Math.Cos(lat1.ToRadians()) * Math.Cos(lat2.ToRadians()) *
					Math.Sin(dLon/2) * Math.Sin(dLon/2);
				double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
				distance = 6378.1 * c; // Multiply by 6378.1 to get kilometers

				return distance;
			}

			public double DistanceCenter(double latitude, double longitude)
			{
				double cLat, cLon;

				cLat = (this.latMin + this.latMax) / 2.0;
				cLon = (this.longMin + this.longMax) / 2.0;

				return DistanceBetweenPoints (latitude, longitude, cLat, cLon);
			}

			public double Distance(double latitude, double longitude)
			{
				double distance = -1.0;
				if (this.ContainsPoint (latitude, longitude)) {
					distance = 0.0;
				} else {
					double closestLat = ClosestPoint2D (this.latMin, this.latMax, latitude);
					double closestLon = ClosestPoint2D (this.longMin, this.longMax, longitude);

					distance = DistanceBetweenPoints (closestLat, closestLon, latitude, longitude);
				}
				return distance;
			}
		}
		public BBox bbox;

		public class Texture
		{
			public string file;
			public long size;
			public long unzipSize;
			public string bigFile;
			public long bigFileSize;
		}
		public Texture texture;

		public class Elevation
		{
			public string file;
			public long size;
			public long unzipSize;
		}
		public Elevation elevation;
	}
}

