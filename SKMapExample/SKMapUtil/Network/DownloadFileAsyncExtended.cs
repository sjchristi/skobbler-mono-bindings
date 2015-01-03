using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace SKMapUtil
{
	//// This is the main download class.
	public class DownloadFileAsyncExtended
	{

		#region "Methods"

		private string _URL = string.Empty;
		private string _LocalFilePath = string.Empty;
		private object _userToken = null;
		private long _ContentLength = 0;
		private long _TotalBytesReceived = 0;
		//// Start the asynchronous download.
		public void DownloadFileAsync(Uri uri, string LocalFilePath, object userToken)
		{
			HttpWebRequest Request = null;
			Uri fileURI = uri;
			//// Will throw exception if empty or random string.

			//// Make sure it's a valid http:// or https:// url.
			if (fileURI.Scheme != Uri.UriSchemeHttp & fileURI.Scheme != Uri.UriSchemeHttps)
			{
				throw new Exception("Invalid URL. Must be http:// or https://");
			}

			//// Save this to private variables in case we need to resume.
			_URL = uri.AbsoluteUri;
			_LocalFilePath = LocalFilePath;
			_userToken = userToken;
			_TotalBytesReceived = 0;
			_ContentLength = 0;

			//// Create the request.
			Request = (HttpWebRequest)HttpWebRequest.Create(new Uri(_URL));
			Request.Credentials = Credentials;
			Request.AllowAutoRedirect = true;
			Request.ReadWriteTimeout = 30000;
			Request.Proxy = Proxy;
			Request.KeepAlive = false;
			Request.Headers = Headers;
			//// NOTE: Will throw exception if wrong headers supplied.

			//// If we're resuming, then add the AddRange header.
			bool fileCompletelyDownloaded = false;

			if (_ResumeAsync)
			{
				FileInfo fileInfo = new FileInfo(LocalFilePath);
				if (fileInfo.Exists) {
					fileCompletelyDownloaded = true;
				} else {
					string partialPath = GetPartialFilePath (LocalFilePath);
					fileInfo = new FileInfo (partialPath);
					if (fileInfo.Exists) {
						Request.AddRange (fileInfo.Length);
						_TotalBytesReceived = fileInfo.Length;
					}
				}
			}

			if (!fileCompletelyDownloaded) {
				//// Signal we're busy downloading
				_isbusy = true;

				//// Make sure this is set to False or the download will stop immediately.
				_CancelAsync = false;

				//// This is the data we're sending to the GetResponse Callback.
				HttpWebRequestState State = new HttpWebRequestState (LocalFilePath, Request, _ResumeAsync, userToken);

				//// Begin to get a response from the server.
				IAsyncResult result = Request.BeginGetResponse (GetResponse_Callback, State);

				//// Add custom 30 second timeout for connecting.
				//// The Timeout property is ignored when using the asynchronous BeginGetResponse.
				ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle, new WaitOrTimerCallback (TimeoutCallback), State, 30000, true);
			} else {
				OnDownloadCompleted( new FileDownloadCompletedEventArgs(null, false, userToken));
			}
		}

		public static string GetPartialFilePath(string filePath)
		{
			return filePath + ".partial";
		}

		//// Here we receive the response from the server. We do not check for the "Accept-Ranges"
		//// response header, in order to find out if the server supports resuming, because it MAY
		//// send the "Accept-Ranges" response header, but is not required to do so. This is
		//// unreliable, so we'll just continue and catch the exception that will occur if not
		//// supported and send it the DownloadCompleted event. We also don't check if the
		//// Content-Length is '-1', because some servers return '-1', eventhough the file/webpage
		//// you're trying to download is valid. e.ProgressPercentage returns '-1' in that case.
		private void GetResponse_Callback(IAsyncResult result)
		{
			HttpWebRequestState webRequestState = (HttpWebRequestState)result.AsyncState;
			FileStream destinationStream = null;
			HttpWebResponse response = null;
			Stopwatch durationStopwatch = new Stopwatch();
			byte[] buffer = new byte[8192];
			int bytesRead = 0;
			long downloadProgressTime = 0;
			long bytesTransferredSinceLastUpdate = 0;
			string partialFilePath = GetPartialFilePath (webRequestState.LocalFilePath);

			try
			{
				//'// Get response
				response = (HttpWebResponse)webRequestState.Request.EndGetResponse(result);

				//// Asign Response headers to ReadOnly ResponseHeaders property.
				_ResponseHeaders = response.Headers;

				//// If the server does not reply with an 'OK (200)' message when starting
				//// the download or a 'PartialContent (206)' message when resuming.
				if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.PartialContent)
				{
					//// Send error message to anyone who is listening.
					OnDownloadCompleted(new FileDownloadCompletedEventArgs(new Exception(response.StatusCode.ToString()), false, webRequestState.userToken));
					return;
				}

				//// Create/open the file to write to.
				if (webRequestState.ResumeDownload)
				{
					//// If resumed, then create or open the file.
					destinationStream = new FileStream(partialFilePath, FileMode.OpenOrCreate, FileAccess.Write);
				}
				else
				{
					//// If not resumed, then create the file, which will delete the existing file if it already exists.
					destinationStream = new FileStream(partialFilePath, FileMode.Create, FileAccess.Write);
				}

				_ContentLength = response.ContentLength;

				//// Moves stream position to beginning of the file when starting the download.
				//// Moves stream position to end of the file when resuming the download.
				destinationStream.Seek(0, SeekOrigin.End);

				//// Start timer to get download duration / download speed, etc.
				durationStopwatch.Start();

				//// Get the Response Stream.
				using (Stream responseStream = response.GetResponseStream())
				{
					do
					{
						//// Read some bytes.
						bytesRead = responseStream.Read(buffer, 0, buffer.Length);
						if (bytesRead > 0)
						{
							//// Write incoming data to the file.
							destinationStream.Write(buffer, 0, bytesRead);
							//// Count the total number of bytes downloaded.
							_TotalBytesReceived += bytesRead;
							bytesTransferredSinceLastUpdate += bytesRead;

							//// Update frequency: No Delay, every Half a Second or every Second.
							switch (ProgressUpdateFrequency)
							{
							case UpdateFrequency.NoDelay:
								//// Send download progress to anyone who is listening.
								OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(bytesTransferredSinceLastUpdate, _TotalBytesReceived, _ContentLength, webRequestState.userToken));
								bytesTransferredSinceLastUpdate = 0;
								break;
							case UpdateFrequency.HalfSecond:
								if ((durationStopwatch.ElapsedMilliseconds - downloadProgressTime) >= 500)
								{
									downloadProgressTime = durationStopwatch.ElapsedMilliseconds;
									//// Send download progress to anyone who is listening.
									OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(bytesTransferredSinceLastUpdate, _TotalBytesReceived, _ContentLength, webRequestState.userToken));
									bytesTransferredSinceLastUpdate = 0;
								}
								break;
							case UpdateFrequency.Second:
								if ((durationStopwatch.ElapsedMilliseconds - downloadProgressTime) >= 1000)
								{
									downloadProgressTime = durationStopwatch.ElapsedMilliseconds;
									//// Send download progress to anyone who is listening.
									OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(bytesTransferredSinceLastUpdate, _TotalBytesReceived, _ContentLength, webRequestState.userToken));
									bytesTransferredSinceLastUpdate = 0;
								}
								break;
							}

							//// Exit loop when paused.
							if (_CancelAsync)
								break; // TODO: might not be correct. Was : Exit Do

						}
					} while (!(bytesRead == 0));
				}

				//// Send download progress once more. If the UpdateFrequency has been set to
				//// HalfSecond or Second, then the last percentage returned might be 98% or 99%.
				//// This makes sure it's 100%.
				OnDownloadProgressChanged(new FileDownloadProgressChangedEventArgs(0, _TotalBytesReceived, _ContentLength, webRequestState.userToken));

				if (_CancelAsync)
				{
					//// Send completed message (Paused) to anyone who is listening.
					OnDownloadCompleted(new FileDownloadCompletedEventArgs(null, true, webRequestState.userToken));
				}
				else
				{
					// make sure we close stream before we call download completed; otherwise we may be
					// missing bytes...
					if (destinationStream != null)
					{
						destinationStream.Flush();
						destinationStream.Close();
						destinationStream = null;
					}

					// Copy partial file to whole file now that it is downloaded completely
					File.Move(partialFilePath, webRequestState.LocalFilePath);

					//// Send completed message (Finished) to anyone who is listening.
					OnDownloadCompleted(new FileDownloadCompletedEventArgs(null, false, webRequestState.userToken));
				}
			}
			catch (Exception ex)
			{
				//// Send completed message (Error) to anyone who is listening.
				OnDownloadCompleted(new FileDownloadCompletedEventArgs(ex, false, webRequestState.userToken));
			}
			finally
			{
				//// Close the file.
				if (destinationStream != null)
				{
					destinationStream.Flush();
					destinationStream.Close();
					destinationStream = null;
				}
				//// Stop and reset the duration timer.
				durationStopwatch.Reset();
				durationStopwatch = null;
				//// Signal we're not downloading anymore.
				_isbusy = false;
			}
		}

		//// Here we will abort the download if it takes more than 30 seconds to connect, because
		//// the Timeout property is ignored when using the asynchronous BeginGetResponse.
		private void TimeoutCallback(object State, bool TimedOut)
		{
			if (TimedOut)
			{
				HttpWebRequestState RequestState = (HttpWebRequestState)State;
				if (RequestState != null)
				{
					RequestState.Request.Abort();
				}
			}
		}

		//// Cancel the asynchronous download.
		private bool _CancelAsync = false;
		public void CancelAsync()
		{
			_CancelAsync = true;
		}

		//// Resume the asynchronous download.
		/// SAMCTODO:  This method needs rewriting.
		private bool _ResumeAsync = true;
		public void ResumeAsync()
		{
			//// Throw exception if download is already in progress.
			if (_isbusy)
			{
				throw new Exception("Download is still busy. Use IsBusy property to check if download is already busy.");
			}

			//// Throw exception if URL or LocalFilePath is empty, which means
			//// the download wasn't even started yet with DowloadFileAsync.
			if (string.IsNullOrEmpty(_URL) && string.IsNullOrEmpty(GetPartialFilePath(_LocalFilePath)))
			{
				throw new Exception("Cannot resume a download which hasn't been started yet. Call DowloadFileAsync first.");
			}
			else
			{
				//// Set _ResumeDownload to True, so we know we need to add
				//// the Range header in order to resume the download.
				_ResumeAsync = true;
				//// Restart (Resume) the download.
				DownloadFileAsync(new Uri(_URL), _LocalFilePath, _userToken);
			}
		}

		#endregion

		#region "Properties"

		public enum UpdateFrequency
		{
			NoDelay = 0,
			HalfSecond = 1,
			Second = 2
		}

		//// Progress Update Frequency.
		public UpdateFrequency ProgressUpdateFrequency { get; set; }

		//// Proxy.
		public IWebProxy Proxy { get; set; }

		//// Credentials.
		public ICredentials Credentials { get; set; }

		//// Headers.
		public WebHeaderCollection Headers { get {return new WebHeaderCollection();} /*set;*/ }

		//// Is download busy.
		private bool _isbusy = false;
		public bool IsBusy
		{
			get { return _isbusy; }
		}

		//// ResponseHeaders.
		private WebHeaderCollection _ResponseHeaders = null;
		public WebHeaderCollection ResponseHeaders
		{
			get { return _ResponseHeaders; }
		}

		//// SynchronizingObject property to marshal events back to the UI thread.
		private System.ComponentModel.ISynchronizeInvoke _synchronizingObject;
		public System.ComponentModel.ISynchronizeInvoke SynchronizingObject
		{
			get { return this._synchronizingObject; }
			set { this._synchronizingObject = value; }
		}

		#endregion

		#region "Events"

		public event EventHandler<FileDownloadProgressChangedEventArgs> DownloadProgressChanged;
		private delegate void DownloadProgressChangedEventInvoker(FileDownloadProgressChangedEventArgs e);
		protected virtual void OnDownloadProgressChanged(FileDownloadProgressChangedEventArgs e)
		{
			if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
			{
				//Marshal the call to the thread that owns the synchronizing object.
				this.SynchronizingObject.Invoke(new DownloadProgressChangedEventInvoker(OnDownloadProgressChanged), new object[] { e });
			}
			else
			{
				if (DownloadProgressChanged != null)
				{
					DownloadProgressChanged(this, e);
				}
			}
		}

		public event EventHandler<FileDownloadCompletedEventArgs> DownloadCompleted;
		private delegate void DownloadCompletedEventInvoker(FileDownloadCompletedEventArgs e);
		protected virtual void OnDownloadCompleted(FileDownloadCompletedEventArgs e)
		{
			if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
			{
				//Marshal the call to the thread that owns the synchronizing object.
				this.SynchronizingObject.Invoke(new DownloadCompletedEventInvoker(OnDownloadCompleted), new object[] { e });
			}
			else
			{
				if (DownloadCompleted != null)
				{
					DownloadCompleted(this, e);
				}
			}
		}

		#endregion

	}


	//// This class is passed as a parameter to the GetResponse Callback,
	//// so we can work with the data in the Response Callback.
	public class HttpWebRequestState
	{
		private string _LocalFilePath;
		private HttpWebRequest _Request;
		private bool _ResumeDownload;

		private object _userToken;
		public HttpWebRequestState(string LocalFilePath, HttpWebRequest Request, bool ResumeDownload, object userToken)
		{
			_LocalFilePath = LocalFilePath;
			_Request = Request;
			_ResumeDownload = ResumeDownload;
			_userToken = userToken;
		}

		public string LocalFilePath
		{
			get { return _LocalFilePath; }
		}

		public HttpWebRequest Request
		{
			get { return _Request; }
		}

		public bool ResumeDownload
		{
			get { return _ResumeDownload; }
		}

		public object userToken
		{
			get { return _userToken; }
		}
	}


	//// This is the data returned to the user for each download in the
	//// Progress Changed event, so you can update controls with the progress.
	public class FileDownloadProgressChangedEventArgs : System.EventArgs
	{
		private long _bytesTransferred;
		private long _totalBytesTransferred;
		private long _totalBytesExpectedToTransfer;
		private object _userToken;

		public FileDownloadProgressChangedEventArgs(long bytesTransferred, long totalBytesTransferred, long totalBytesExpectedToTransfer, object userToken)
		{
			this._bytesTransferred = bytesTransferred;
			this._totalBytesTransferred = totalBytesTransferred;
			this._totalBytesExpectedToTransfer = totalBytesExpectedToTransfer;
			this._userToken = userToken;
		}

		public long BytesTransferred
		{
			get { return this._bytesTransferred; }
		}

		public long TotalBytesTransferred
		{
			get { return this._totalBytesTransferred; }
		}

		public long TotalBytesExpectedToTransfer
		{
			get { return this._totalBytesExpectedToTransfer; }
		}

		public object UserToken
		{
			get { return _userToken; }
		}
	}


	//// This is the data returned to the user for each download in the
	//// Download Completed event, so you can update controls with the result.
	public class FileDownloadCompletedEventArgs : System.EventArgs
	{

		private Exception _Error;
		private bool _Cancelled;

		private object _userToken;
		public FileDownloadCompletedEventArgs(Exception Error, bool Cancelled, object userToken)
		{
			_Error = Error;
			_Cancelled = Cancelled;
			_userToken = userToken;
		}

		public Exception Error
		{
			get { return _Error; }
		}

		public bool Cancelled
		{
			get { return _Cancelled; }
		}

		public object UserToken
		{
			get { return _userToken; }
		}
	}
}

