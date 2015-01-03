using System;
using System.IO;
using System.IO.Compression;

namespace SKMapUtil
{
	public static class FileExtensions
	{
		// Some utilities....
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

		public static string GetFilePath(string baseFileName)
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

		public static void NukeFolder(string destFolder)
		{
			if (Directory.Exists (destFolder)) {
				// Must delete files before we can delete the folder they reside in...
				DirectoryInfo dir = new DirectoryInfo(destFolder);
				foreach(FileInfo fi in dir.GetFiles())
				{
					fi.Delete();
				}
				foreach (DirectoryInfo di in dir.GetDirectories()) {
					NukeFolder (di.FullName);
				}
				Directory.Delete (destFolder);
			}
		}

		public static string [] FilesMatchingPattern(string rootFolder, string pattern)
		{
			DirectoryInfo dir = new DirectoryInfo (rootFolder);
			FileInfo [] files = dir.GetFiles (pattern);
			string[] matches = null;

			if (files != null && files.Length > 0) {
				matches = new string [files.Length];
				for (int i=0; i<files.Length; i++) {
					matches [i] = files [i].FullName;
				}
			} else {
				matches = new string [0];
			}

			return matches;
		}

		public static void UnzipFileToFolder(string zipFile, string destFolder)
		{
			ZipFile.ExtractToDirectory (zipFile, destFolder);
		}
	}
}

