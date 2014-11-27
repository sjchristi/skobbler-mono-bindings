using System;
using MonoTouch.Dialog;
using SKMaps;
using MonoTouch.CoreFoundation;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.IO;

namespace SKMapExample
{
	public class SKMapExampleOfflineMapsViewController : DialogViewController
	{
		protected NSMutableData jsonData;
		protected NSUrlConnection jsonConnection;

		public SKMapExampleOfflineMapsViewController () : base(UITableViewStyle.Grouped, null, true)
		{
			this.jsonData = new NSMutableData ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.DownloadMapsInfo ();
		}

		public string GetFilePath(string baseFileName)
		{
			#if __ANDROID__
			// Just use whatever directory SpecialFolder.Personal returns
			string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); ;
			#else
			// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
			// (they don't want non-user-generated data in Documents)
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
			//string sourceDbPath = Environment.GetFolderPath (Environment.SpecialFolder.Resources);
			var path = Path.Combine (libraryPath, baseFileName);
			#endif

			return path;
		}

		public void DownloadMapsInfo()
		{
			string urlPath = SKMaps.SKMapsService.SharedInstance.PackagesManager.MapsJSONURLForVersion (null);
			string destFilePath = GetFilePath("maps.json");

			Network.HttpDownloadFile (urlPath, destFilePath, FileDownloaded, FileDownloadProgress);
		}

		protected void FileDownloaded(NetworkDownloadInfo info)
		{

		}

		protected void FileDownloadProgress(NetworkDownloadInfo info)
		{
		}

		// NSURLConnectionDataDelegate
	}
}

