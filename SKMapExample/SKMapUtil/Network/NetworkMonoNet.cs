using System;

using System.IO;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SKMapUtil
{
	// INetwork interface implemented using mono's System.Net library
	public class NetworkMonoNet : NetworkBase, INetwork
	{
		protected List<MonoNetFileTransfer> _transfers = new List<MonoNetFileTransfer>(10);

		public NetworkMonoNet () : base()
		{
		}

		// INetwork Interface

		// Download the contents of 'url' and place it in the file at path 'destFilePath'
		public INetworkFileTransfer DownloadFile(string url, string destFilePath)
		{
			MonoNetFileTransferDownload dl = new MonoNetFileTransferDownload (url, destFilePath);

			this._transfers.Add (dl);

			dl.Resume ();

			return dl;
		}

		// Download the contents of 'urls' and place the files in the destination folder.  File
		// names will be inferred from the url names
		public INetworkFileTransfer DownloadFiles(string [] urls, string destFolder, long estimatedDownloadSizeBytes)
		{
			MonoNetFileTransferMultiDownload dl = new MonoNetFileTransferMultiDownload (urls, destFolder, estimatedDownloadSizeBytes);

			this._transfers.Add (dl);

			dl.Resume ();

			return dl;
		}
	}

	public class MonoNetFileTransfer : NetworkFileTransferBase
	{
		protected DownloadFileAsyncExtended _clientExtended;

		public MonoNetFileTransfer(string targetPath)
		{
			this._targetPath = targetPath;

			this._clientExtended = new DownloadFileAsyncExtended ();

			this._clientExtended.ProgressUpdateFrequency = DownloadFileAsyncExtended.UpdateFrequency.Second;
			this._clientExtended.DownloadCompleted += this.HttpDownloadFileCompleted;
			this._clientExtended.DownloadProgressChanged += this.HttpDownloadFileProgress;
		}

		protected virtual void HttpDownloadFileProgress(object sender, FileDownloadProgressChangedEventArgs e)
		{
		}

		protected virtual void HttpDownloadFileCompleted(object sender, FileDownloadCompletedEventArgs e)
		{
			this._error = e.Error;
			this._cancelled = e.Cancelled;

			if (e.Error != null) {
				Console.WriteLine ("Error: " + e.Error.ToString ());
			} else if (e.Cancelled) {
				Console.WriteLine ("Download cancelled");
			} else {
			}
		}

		public override void Cancel ()
		{
			base.Cancel ();

			this._clientExtended.CancelAsync ();
		}
	}

	public class MonoNetFileTransferMultiDownload : MonoNetFileTransfer
	{
		protected string [] _urls;
		protected string _destFolder;
		protected int _currentUrlIndex;
		protected long _estimatedDownloadSizeBytes;
		protected long _totalBytesDownloaded;

		public MonoNetFileTransferMultiDownload(string [] urls, string destFolder, long estimatedDownloadSizeBytes)
			: base(destFolder)
		{
			this._urls = urls;
			this._destFolder = destFolder;
			this._estimatedDownloadSizeBytes = estimatedDownloadSizeBytes;
			this._totalBytesDownloaded = (int)SizeOfFilesOnDisk ();

			this.DownloadNextFile ();
		}

		public long SizeOfFilesOnDisk()
		{
			long size = 0;
			foreach (string url in this._urls) {
				string destFileName = NetworkBase.GetUrlFileName(url);
				string destFilePath = Path.Combine (this._destFolder, destFileName);

				FileInfo info = new FileInfo (destFilePath);

				if (info.Exists) {
					size += info.Length;
				} else {
					// SAMCTODO:  Assumes partial knowledge of network interface...
					info = new FileInfo (destFilePath + ".partial");
					if (info.Exists) {
						size += info.Length;
					}
				}
			}
			return size;
		}

		public void DownloadNextFile()
		{
			string url = this._urls [this._currentUrlIndex];
			Uri uri = new Uri (url);
			string destFileName = NetworkBase.GetUrlFileName(url);
			string destFilePath = Path.Combine (this._destFolder, destFileName);
			//client.DownloadFileAsync(uri, destFilePath, this);
			this._clientExtended.DownloadFileAsync (uri, destFilePath, this);
		}

		protected override void HttpDownloadFileProgress (object sender, FileDownloadProgressChangedEventArgs e)
		{
			base.HttpDownloadFileProgress (sender, e);

			this._totalBytesDownloaded += e.BytesTransferred;

			this.UpdateProgress (e.BytesTransferred, this._totalBytesDownloaded, this._estimatedDownloadSizeBytes);
		}

		protected override void HttpDownloadFileCompleted (object sender, FileDownloadCompletedEventArgs e)
		{
			base.HttpDownloadFileCompleted (sender, e);

			// see if we have more files to download...
			int nextUrlIndex = this._currentUrlIndex + 1;
			if (nextUrlIndex >= this._urls.Length || e.Cancelled || e.Error != null) {
				this.RaiseCompleted ();
			} else {
				this._currentUrlIndex = nextUrlIndex;
				this.DownloadNextFile ();
			}
		}
	}

	public class MonoNetFileTransferDownload : MonoNetFileTransfer
	{
		public string url;
		public string destFilePath;
		public string destFileMD5;

		public MonoNetFileTransferDownload(string url, string destFilePath)
			: base(destFilePath)
		{
			Uri uri = new Uri (url);

			this.url = url;
			this.destFilePath = destFilePath;

			this._clientExtended.DownloadFileAsync(uri, destFilePath, this);
		}

		protected override void HttpDownloadFileProgress (object sender, FileDownloadProgressChangedEventArgs e)
		{
			base.HttpDownloadFileProgress (sender, e);

			this.UpdateProgress (e.BytesTransferred, e.TotalBytesTransferred, e.TotalBytesExpectedToTransfer);
		}

		protected override void HttpDownloadFileCompleted (object sender, FileDownloadCompletedEventArgs e)
		{
			base.HttpDownloadFileCompleted (sender, e);

			this.RaiseCompleted ();
		}
	}
}

