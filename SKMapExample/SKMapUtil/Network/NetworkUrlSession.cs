using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.IO;

using Foundation;
using UIKit;

namespace SKMapUtil
{
	// INetwork interface implemented using NSUrlSession, which allows background
	// transfers and is available on iOS 7+
	public class NetworkUrlSession : NetworkBase, INetwork
	{
		protected NSUrlSession _urlSession;
		protected NetworkUrlSessionDelegate _urlSessionDelegate;
		protected List<NetworkUrlSessionTransfer> _transfers;

		public NetworkUrlSession (string identifier)
		{
			_transfers = new List<NetworkUrlSessionTransfer> (10);
			_urlSessionDelegate = new NetworkUrlSessionDelegate (this);

			NSUrlSessionConfiguration c = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration (identifier);
			NSOperationQueue q = new NSOperationQueue ();
			// only allow 1 file transfer at a time; SAMCTODO: not sure if this is best..
			q.MaxConcurrentOperationCount = 1;
			_urlSession = NSUrlSession.FromConfiguration (c, _urlSessionDelegate, q);
			_urlSession.GetTasks( HandleNSUrlSessionPendingTasks );
			IBackgroundUrlEventDispatcher appDelegate = UIApplication.SharedApplication.Delegate as IBackgroundUrlEventDispatcher;

			appDelegate.HandleEventsForBackgroundUrlEvent += HandleEventsForBackgroundUrl;
		}

		void HandleNSUrlSessionPendingTasks (NSUrlSessionDataTask[] dataTasks, NSUrlSessionUploadTask[] uploadTasks, NSUrlSessionDownloadTask[] downloadTasks)
		{
			// SAMCTODO
			foreach (var t in uploadTasks) {
			}

			foreach (var t in downloadTasks){
			}

			foreach (var t in dataTasks) {
			}
		}

		protected void HandleEventsForBackgroundUrl(object sender, BackgroundUrlEventArgs args)
		{
			// after all is done: 
			args.completionHandler.Invoke ();
		}

		public NetworkUrlSessionTransfer GetUrlSessionTransfer(NSUrlSessionTask task)
		{
			IEnumerable<NetworkUrlSessionTransfer> matches = this._transfers.Where ((NetworkUrlSessionTransfer arg) => {
				return (arg.Tasks.IndexOf (task) >= 0);
			});

			NetworkUrlSessionTransfer transfer = null;

			if (matches.Count() > 0) {
				transfer = matches.First ();
			}

			return transfer;
		}

		// INetwork Interface

		// Download the contents of 'url' and place it in the file at path 'destFilePath'
		public INetworkFileTransfer DownloadFile(string url, string destFilePath)
		{
			NetworkUrlSessionTransferDownload dlTask = new NetworkUrlSessionTransferDownload (_urlSession, url, destFilePath);

			this._transfers.Add (dlTask);

//			dlTask.Resume ();

			return dlTask;
		}

		// Download the contents of 'urls' and place the files in the destination folder.  File
		// names will be inferred from the url names
		public INetworkFileTransfer DownloadFiles(string [] urls, string destFolder, long estimatedDownloadSizeBytes)
		{
			NetworkUrlSessionTransferMultiDownload dlTask = new NetworkUrlSessionTransferMultiDownload (_urlSession, urls, destFolder, estimatedDownloadSizeBytes);

			this._transfers.Add (dlTask);

			// SAMCTODO:  background/close app then reopen and test this...
//			dlTask.Resume ();

			return dlTask;
		}
	}

	public class NetworkUrlSessionTransfer : NetworkFileTransferBase
	{
		protected NSUrlSession _urlSession;
		protected List<NSUrlSessionTask> _tasks;
		public List<NSUrlSessionTask> Tasks { get { return _tasks; } }

		public NetworkUrlSessionTransfer(NSUrlSession s)
		{
			this._tasks = new List<NSUrlSessionTask> (3);
			this._urlSession = s;
		}

		public override void Resume ()
		{
			base.Resume ();

			foreach (var t in this._tasks) {
				t.Resume ();
			}
		}

		public virtual void DidCompleteWithError(NSUrlSessionTask task, NSError error)
		{
			Console.WriteLine ("Completed: {0}", error != null ? error.ToString () : "no error");

			this._tasks.Remove (task);

			if (error != null) {
				this._error = new Exception (error.Domain);

				// Cancel other tasks; they are probably GG..
				foreach (var t in this._tasks) {
					t.Cancel ();
				}

				this._tasks.Clear();
			}

			if (this._tasks.Count == 0) {
				this.RaiseCompleted ();
			}
		}

		public virtual void DidSendBodyData (NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
		{
			Console.WriteLine ("DidSendBodyData: {0} / {1} / {2}", bytesSent, totalBytesSent, totalBytesExpectedToSend);
		}

		public virtual void NeedNewBodyStream (NSUrlSessionTask task, Action<NSInputStream> completionHandler)
		{
			Console.WriteLine ("NeedNewBodyStream");
		}

		public virtual void DidFinishDownloading (NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			Console.WriteLine ("DidFinishDownloading: {0}", location.ToString());
		}

		public virtual void DidResume (NSUrlSessionDownloadTask downloadTask, long resumeFileOffset, long expectedTotalBytes)
		{
			Console.WriteLine ("DidResume: {0} / {1}", resumeFileOffset, expectedTotalBytes);
		}

		public virtual void DidWriteData (NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{
			Console.WriteLine ("DidWriteData: {0} / {1} / {2}", bytesWritten, totalBytesWritten, totalBytesExpectedToWrite);

			// SAMCTODO:  How to handle multiple tasks...
			this.UpdateProgress (bytesWritten, totalBytesWritten, totalBytesExpectedToWrite);
		}
	}

	public class NetworkUrlSessionTransferDownload : NetworkUrlSessionTransfer
	{
		protected string _url;
		protected string _destFilePath;

		public NetworkUrlSessionTransferDownload(NSUrlSession s, string url, string destFilePath)
			: base(s)
		{
			this._url = url;
			this._destFilePath = destFilePath;
			string partialFilePath = this.PartialFilePath;

			NSUrlSessionDownloadTask t;

			if (File.Exists (partialFilePath)) {
				NSData resumeData = NSData.FromFile (partialFilePath);
				t = _urlSession.CreateDownloadTask (resumeData);
			} else {
				NSUrl nsUrl = NSUrl.FromString (url);
				NSUrlRequest r = NSUrlRequest.FromUrl (nsUrl);
				t = _urlSession.CreateDownloadTask (r);
			}

			this._tasks.Add(t);
		}

		protected string PartialFilePath { 
			get {
				return this._destFilePath + ".partial";
			}
		}

		public override void Cancel ()
		{
			if (this._tasks.Count == 1) {
				NSUrlSessionDownloadTask t = this._tasks.First () as NSUrlSessionDownloadTask;
				t.Cancel ((NSData resumeData) => {
					string resumeFile = this.PartialFilePath;

					if (File.Exists(resumeFile))
					{
						File.Delete(resumeFile);
					}

					using (var strm = new FileStream(resumeFile,FileMode.Create)) {
						Stream readStream = resumeData.AsStream();
						readStream.CopyTo(strm);
						readStream.Close();
						strm.Close();
					}
				});
			}
			base.Cancel ();
		}

		//  Delegate

		public override void DidFinishDownloading (NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			base.DidFinishDownloading (downloadTask, location);

			string sourcePath = location.Path;

			if (File.Exists (_destFilePath)) {
				File.Delete (_destFilePath);
			}

			File.Copy (sourcePath, _destFilePath);
		}

		public override void DidResume (NSUrlSessionDownloadTask downloadTask, long resumeFileOffset, long expectedTotalBytes)
		{
			base.DidResume (downloadTask, resumeFileOffset, expectedTotalBytes);
		}
	}

	public class NetworkUrlSessionTransferMultiDownload : NetworkUrlSessionTransfer
	{
		protected string _destFilePath;
		protected long _estimatedDownloadSizeBytes;

		public NetworkUrlSessionTransferMultiDownload(NSUrlSession s, string [] urls, string destFilePath, long estimatedDownloadSizeBytes)
			: base(s)
		{
			this._destFilePath = destFilePath;
			this._estimatedDownloadSizeBytes = estimatedDownloadSizeBytes;

			foreach (string url in urls) {
				NSUrl nsUrl = NSUrl.FromString (url);
				NSUrlRequest r = NSUrlRequest.FromUrl (nsUrl);
				NSUrlSessionDownloadTask t = _urlSession.CreateDownloadTask (r);

				this._tasks.Add(t);
			}
		}

		// Delegate

		public override void DidFinishDownloading (NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			base.DidFinishDownloading (downloadTask, location);

			string sourcePath = location.Path;
			string destPath = Path.Combine(this._destFilePath, NetworkBase.GetUrlFileName (downloadTask.OriginalRequest.Url.ToString ()));

			if (File.Exists (destPath)) {
				File.Delete (destPath);
			}

			File.Move (sourcePath, destPath);
		}

		public override void DidResume (NSUrlSessionDownloadTask downloadTask, long resumeFileOffset, long expectedTotalBytes)
		{
			base.DidResume (downloadTask, resumeFileOffset, expectedTotalBytes);
		}

		public override void DidWriteData (NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{
			base.DidWriteData (downloadTask, bytesWritten, totalBytesWritten, totalBytesExpectedToWrite);

			this.UpdateProgress (bytesWritten, totalBytesWritten, totalBytesExpectedToWrite);
		}
	}

	public class NetworkUrlSessionDelegate : NSUrlSessionDownloadDelegate
	{
		protected NetworkUrlSession _urlSession;

		public NetworkUrlSessionDelegate(NetworkUrlSession session)
		{
			this._urlSession = session;
		}

		// INSUrlSessionDelegate Interface

		public override void DidBecomeInvalid (NSUrlSession session, NSError error)
		{
			Console.WriteLine ("DidBecomeInvalid");
		}

		public override void DidFinishEventsForBackgroundSession (NSUrlSession session)
		{
			Console.WriteLine ("DidFinishEventsForBackgroundSession");
		}

		public override void DidReceiveChallenge (NSUrlSession session, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler)
		{
			Console.WriteLine ("DidReceiveChallenge");
			// No Handle
		}

		// NSUrlSessionTaskDelegate

		public override void DidCompleteWithError (NSUrlSession session, NSUrlSessionTask task, NSError error)
		{
			NetworkUrlSessionTransfer t = this._urlSession.GetUrlSessionTransfer (task);

			if (t != null) {
				t.DidCompleteWithError (task, error);
			}
		}

		public override void DidReceiveChallenge (NSUrlSession session, NSUrlSessionTask task, NSUrlAuthenticationChallenge challenge, Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler)
		{
			// No handle?
		}

		public override void DidSendBodyData (NSUrlSession session, NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
		{
			NetworkUrlSessionTransfer t = this._urlSession.GetUrlSessionTransfer (task);

			if (t != null) {
				t.DidSendBodyData(task, bytesSent, totalBytesSent, totalBytesExpectedToSend);
			}
		}

		public override void NeedNewBodyStream (NSUrlSession session, NSUrlSessionTask task, Action<NSInputStream> completionHandler)
		{
			NetworkUrlSessionTransfer t = this._urlSession.GetUrlSessionTransfer (task);

			if (t != null) {
				t.NeedNewBodyStream (task, completionHandler);
			}
		}

		public override void WillPerformHttpRedirection (NSUrlSession session, NSUrlSessionTask task, NSHttpUrlResponse response, NSUrlRequest newRequest, Action<NSUrlRequest> completionHandler)
		{
			// No handle?
		}

		// NSUrlSessionDownloadDelegate

		public override void DidFinishDownloading (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			NetworkUrlSessionTransfer t = this._urlSession.GetUrlSessionTransfer (downloadTask);

			if (t != null) {
				t.DidFinishDownloading (downloadTask, location);
			}
		}

		public override void DidResume (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long resumeFileOffset, long expectedTotalBytes)
		{
			NetworkUrlSessionTransfer t = this._urlSession.GetUrlSessionTransfer (downloadTask);

			if (t != null) {
				t.DidResume (downloadTask, resumeFileOffset, expectedTotalBytes);
			}
		}

		public override void DidWriteData (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{
			NetworkUrlSessionTransfer t = this._urlSession.GetUrlSessionTransfer (downloadTask);

			if (t != null) {
				t.DidWriteData (downloadTask, bytesWritten, totalBytesWritten, totalBytesExpectedToWrite);
			}
		}
	}
}

