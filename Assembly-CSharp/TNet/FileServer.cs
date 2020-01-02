using System;

namespace TNet
{
	public class FileServer
	{
		protected void Error(string error)
		{
		}

		public void SaveFile(string fileName, byte[] data)
		{
			bool flag = false;
			for (int i = 0; i < this.mSavedFiles.size; i++)
			{
				global::TNet.FileServer.FileEntry fileEntry = this.mSavedFiles[i];
				if (fileEntry.fileName == fileName)
				{
					fileEntry.data = data;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				global::TNet.FileServer.FileEntry item = default(global::TNet.FileServer.FileEntry);
				item.fileName = fileName;
				item.data = data;
				this.mSavedFiles.Add(item);
			}
			global::TNet.Tools.WriteFile(fileName, data);
		}

		public byte[] LoadFile(string fileName)
		{
			for (int i = 0; i < this.mSavedFiles.size; i++)
			{
				global::TNet.FileServer.FileEntry fileEntry = this.mSavedFiles[i];
				if (fileEntry.fileName == fileName)
				{
					return fileEntry.data;
				}
			}
			return global::TNet.Tools.ReadFile(fileName);
		}

		public void DeleteFile(string fileName)
		{
			for (int i = 0; i < this.mSavedFiles.size; i++)
			{
				if (this.mSavedFiles[i].fileName == fileName)
				{
					this.mSavedFiles.RemoveAt(i);
					global::TNet.Tools.DeleteFile(fileName);
					break;
				}
			}
		}

		private global::TNet.List<global::TNet.FileServer.FileEntry> mSavedFiles = new global::TNet.List<global::TNet.FileServer.FileEntry>();

		private struct FileEntry
		{
			public string fileName;

			public byte[] data;
		}
	}
}
