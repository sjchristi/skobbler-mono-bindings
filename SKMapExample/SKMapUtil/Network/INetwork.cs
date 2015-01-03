using System;

namespace SKMapUtil
{
	public interface INetworkFileTransfer
	{
		event EventHandler<INetworkFileTransfer> ProgressChanged;
		event EventHandler<INetworkFileTransfer> TransferCompleted;

		object UserToken { get; set; }
		double Progress { get; }
		Exception Error { get; }
		bool Cancelled { get; }
		string TargetPath { get; }

		void Cancel();
	}

	public interface INetwork
	{
		// Download the contents of 'url' and place it in the file at path 'destFilePath'
		INetworkFileTransfer DownloadFile(string url, string destFilePath);

		// Download the contents of 'urls' and place the files in the destination folder.  File
		// names will be inferred from the url names
		INetworkFileTransfer DownloadFiles(string [] urls, string destFolder, long estimatedDownloadSizeBytes);
	}
}

