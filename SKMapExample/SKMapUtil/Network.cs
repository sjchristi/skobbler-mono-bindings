using System;
using System.ComponentModel;
using System.Net;
using System.IO;

using SKMapUtil;

namespace SKMapUtil
{
	public class Network
	{
		protected static INetwork network = null;
		public static INetwork theNetwork {
			get {

				if (network == null) {
					network = new NetworkMonoNet ();
				}

				return network;
			}
		}

		public Network ()
		{
		}

		public static INetworkFileTransfer HttpDownloadFiles(string [] urls, string destFolder, int estimatedDownloadSizeBytes)
		{
			if (!Directory.Exists(destFolder))
			{
				Exception e = new Exception ("Path doesn't exist: " + destFolder);

				throw(e);
			}

			return theNetwork.DownloadFiles (urls, destFolder, estimatedDownloadSizeBytes);
		}

		public static INetworkFileTransfer HttpDownloadFile(string url, string destFilePath)
		{
			if (File.Exists(destFilePath))
			{
				Exception e = new Exception ("File already exists: " + destFilePath);

				throw(e);
			}

			return theNetwork.DownloadFile (url, destFilePath);
		}

	}
}

