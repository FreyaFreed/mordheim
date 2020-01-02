using System;
using System.IO;
using System.Net;

namespace TNet
{
	public class ServerList
	{
		public global::TNet.ServerList.Entry Add(string name, int playerCount, global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress, long time)
		{
			global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
			global::TNet.ServerList.Entry result;
			lock (obj)
			{
				for (int i = 0; i < this.list.size; i++)
				{
					global::TNet.ServerList.Entry entry = this.list[i];
					if (entry.internalAddress.Equals(internalAddress) && entry.externalAddress.Equals(externalAddress))
					{
						entry.name = name;
						entry.playerCount = playerCount;
						entry.recordTime = time;
						this.list[i] = entry;
						return entry;
					}
				}
				global::TNet.ServerList.Entry entry2 = new global::TNet.ServerList.Entry();
				entry2.name = name;
				entry2.playerCount = playerCount;
				entry2.internalAddress = internalAddress;
				entry2.externalAddress = externalAddress;
				entry2.recordTime = time;
				this.list.Add(entry2);
				result = entry2;
			}
			return result;
		}

		public global::TNet.ServerList.Entry Add(global::TNet.ServerList.Entry newEntry, long time)
		{
			global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
			lock (obj)
			{
				for (int i = 0; i < this.list.size; i++)
				{
					global::TNet.ServerList.Entry entry = this.list[i];
					if (entry.internalAddress.Equals(newEntry.internalAddress) && entry.externalAddress.Equals(newEntry.externalAddress))
					{
						entry.name = newEntry.name;
						entry.playerCount = newEntry.playerCount;
						entry.recordTime = time;
						return entry;
					}
				}
				newEntry.recordTime = time;
				this.list.Add(newEntry);
			}
			return newEntry;
		}

		public bool Remove(global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress)
		{
			global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
			lock (obj)
			{
				for (int i = 0; i < this.list.size; i++)
				{
					global::TNet.ServerList.Entry entry = this.list[i];
					if (entry.internalAddress.Equals(internalAddress) && entry.externalAddress.Equals(externalAddress))
					{
						this.list.RemoveAt(i);
						return true;
					}
				}
			}
			return false;
		}

		public bool Cleanup(long time)
		{
			time -= 7000L;
			bool result = false;
			global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
			lock (obj)
			{
				int i = 0;
				while (i < this.list.size)
				{
					global::TNet.ServerList.Entry entry = this.list[i];
					if (entry.recordTime < time)
					{
						result = true;
						this.list.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			return result;
		}

		public void Clear()
		{
			global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
			lock (obj)
			{
				this.list.Clear();
			}
		}

		public void WriteTo(global::System.IO.BinaryWriter writer)
		{
			writer.Write(1);
			global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
			lock (obj)
			{
				writer.Write((ushort)this.list.size);
				for (int i = 0; i < this.list.size; i++)
				{
					this.list[i].WriteTo(writer);
				}
			}
		}

		public void ReadFrom(global::System.IO.BinaryReader reader, long time)
		{
			if (reader.ReadUInt16() == 1)
			{
				global::TNet.List<global::TNet.ServerList.Entry> obj = this.list;
				lock (obj)
				{
					int num = (int)reader.ReadUInt16();
					for (int i = 0; i < num; i++)
					{
						global::TNet.ServerList.Entry entry = new global::TNet.ServerList.Entry();
						entry.ReadFrom(reader);
						this.AddInternal(entry, time);
					}
				}
			}
		}

		private void AddInternal(global::TNet.ServerList.Entry newEntry, long time)
		{
			for (int i = 0; i < this.list.size; i++)
			{
				global::TNet.ServerList.Entry entry = this.list[i];
				if (entry.internalAddress.Equals(newEntry.internalAddress) && entry.externalAddress.Equals(newEntry.externalAddress))
				{
					entry.name = newEntry.name;
					entry.playerCount = newEntry.playerCount;
					entry.recordTime = time;
					return;
				}
			}
			newEntry.recordTime = time;
			this.list.Add(newEntry);
		}

		public global::TNet.List<global::TNet.ServerList.Entry> list = new global::TNet.List<global::TNet.ServerList.Entry>();

		public class Entry
		{
			public void WriteTo(global::System.IO.BinaryWriter writer)
			{
				writer.Write(this.name);
				writer.Write((ushort)this.playerCount);
				global::TNet.Tools.Serialize(writer, this.internalAddress);
				global::TNet.Tools.Serialize(writer, this.externalAddress);
			}

			public void ReadFrom(global::System.IO.BinaryReader reader)
			{
				this.name = reader.ReadString();
				this.playerCount = (int)reader.ReadUInt16();
				global::TNet.Tools.Serialize(reader, out this.internalAddress);
				global::TNet.Tools.Serialize(reader, out this.externalAddress);
			}

			public string name;

			public int playerCount;

			public global::System.Net.IPEndPoint internalAddress;

			public global::System.Net.IPEndPoint externalAddress;

			public long recordTime;

			public object data;
		}
	}
}
