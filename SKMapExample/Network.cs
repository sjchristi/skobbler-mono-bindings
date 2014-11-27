using System;
using System.ComponentModel;
using System.Net;
using System.IO;

namespace SKMapExample
{
	public class NetworkDownloadInfo
	{
		public string url;
		public string destFilePath;
		public Action<NetworkDownloadInfo> responseDelegate;
		public Action<NetworkDownloadInfo> progressDelegate;
		public WebClient client;
		public double progress;
		public Exception error;
		public bool cancelled;
		public string destFileMD5;
	}

	public class Network
	{
		public Network ()
		{
		}

		public static void HttpDownloadFile(string url, string destFilePath, Action<NetworkDownloadInfo> responseDelegate, Action<NetworkDownloadInfo> progressDelegate)
		{
			if (File.Exists(destFilePath))
			{
				Exception e = new Exception ("File already exists: " + destFilePath);

				throw(e);
			}

			NetworkDownloadInfo dlInfo = new NetworkDownloadInfo ();

			WebClient client = new WebClient();
			Uri uri = new Uri (url);
			client.DownloadFileCompleted += new AsyncCompletedEventHandler(HttpDownloadFileCompleted);
			client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(HttpDownloadFileProgress);

			dlInfo.url = url;
			dlInfo.destFilePath = destFilePath;
			dlInfo.responseDelegate = responseDelegate;
			dlInfo.progressDelegate = progressDelegate;
			dlInfo.client = client;

			client.DownloadFileAsync(uri, destFilePath, dlInfo);
		}

		public static string MD5(string filePath)
		{
			using(var md5 = System.Security.Cryptography.MD5.Create())
			{
				using(var stream = File.OpenRead(filePath))
				{
					return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-",string.Empty).ToLower();
				}
			}
		}

		protected static void HttpDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			NetworkDownloadInfo dlInfo = (NetworkDownloadInfo)e.UserState;

			dlInfo.progress = 1.0;
			dlInfo.error = e.Error;
			dlInfo.cancelled = e.Cancelled;
			dlInfo.destFileMD5 = MD5 (dlInfo.destFilePath);

			if (e.Error != null) {
				Console.WriteLine ("Error: " + e.Error.ToString ());
			} else if (e.Cancelled) {
				Console.WriteLine ("Download cancelled");
			} else {
			}
			//file finished downloading
			Console.WriteLine ("Download Completed");

			if (dlInfo.responseDelegate != null) {
				dlInfo.responseDelegate.Invoke (dlInfo);
			}
		}

		protected static void HttpDownloadFileProgress(object sender, DownloadProgressChangedEventArgs e)
		{
			NetworkDownloadInfo dlInfo = (NetworkDownloadInfo)e.UserState;

			dlInfo.progress = (double)e.BytesReceived / (double)e.TotalBytesToReceive;

			if (dlInfo.progressDelegate != null) {
				dlInfo.progressDelegate.Invoke (dlInfo);
			}
			// Displays the operation identifier, and the transfer progress.
			Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...", 
				Path.GetFileName(dlInfo.destFilePath), 
				e.BytesReceived, 
				e.TotalBytesToReceive,
				e.ProgressPercentage);
		}
	}
}

