using System;
using System.Diagnostics;
using System.IO;

namespace SKMapUtil
{
	public abstract class NetworkBase
	{
		public NetworkBase ()
		{
		}

		public static string GetUrlFileName(string url)
		{
			string targetFileName = Path.GetFileName (url);
			int idx = targetFileName.IndexOf ("?"); // strip off url parameters if necessary...

			if (idx > 0) {
				targetFileName = targetFileName.Substring (0, idx);
			}

			return targetFileName;
		}
	}

	public class  NetworkFileTransferBase : INetworkFileTransfer
	{
		public event EventHandler<INetworkFileTransfer> ProgressChanged;
		public event EventHandler<INetworkFileTransfer> TransferCompleted;

		public object UserToken { get; set; }

		protected double _progress;
		public double Progress { get { return _progress; } }

		protected Exception _error;
		public Exception Error { get { return _error; } }

		protected bool _cancelled;
		public bool Cancelled { get { return _cancelled; } }

		protected string _targetPath;
		public string TargetPath { get { return _targetPath; } }

		protected Stopwatch _stopwatch = new Stopwatch();
		protected long _bytesTransferredThisInterval;

		// Frequency with which we update the progress (in seconds)
		public double UpdateFrequency = 1.0;

		protected double _transferRate;
		public double TransferRate { get { return _transferRate; } }

		public NetworkFileTransferBase()
		{
		}

		public virtual void Resume()
		{
			_stopwatch.Restart ();
		}

		public virtual void Cancel()
		{
		}

		protected void RaiseProgressChanged()
		{
			if (this.ProgressChanged != null) {
				this.ProgressChanged.Invoke (this, this);
			}
		}

		protected void RaiseCompleted()
		{
			if (this.TransferCompleted != null) {
				this._stopwatch.Reset ();
				this.TransferCompleted.Invoke (this, this);
			}
		}

		protected void UpdateProgress(long bytesTransferred, long totalBytesTransferred, long totalBytesExpectedToTransfer)
		{
			double elapsed = _stopwatch.Elapsed.TotalSeconds;

			this._bytesTransferredThisInterval += bytesTransferred;
			this._progress = (double)totalBytesTransferred / (double)totalBytesExpectedToTransfer;

			if (_progress >= 1.0 || elapsed >= UpdateFrequency) {
				_transferRate = (double)this._bytesTransferredThisInterval / elapsed;

				this._bytesTransferredThisInterval = 0;

				_stopwatch.Restart ();

				this.RaiseProgressChanged ();
			}
		}
	}
}

