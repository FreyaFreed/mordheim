using System;
using System.IO;

namespace TNet
{
	public class Channel
	{
		public bool hasData
		{
			get
			{
				return this.rfcs.size > 0 || this.created.size > 0 || this.destroyed.size > 0;
			}
		}

		public bool isOpen
		{
			get
			{
				return !this.closed && this.players.size < (int)this.playerLimit;
			}
		}

		public void Reset()
		{
			for (int i = 0; i < this.rfcs.size; i++)
			{
				this.rfcs[i].buffer.Recycle();
			}
			for (int j = 0; j < this.created.size; j++)
			{
				this.created[j].buffer.Recycle();
			}
			this.rfcs.Clear();
			this.created.Clear();
			this.destroyed.Clear();
			this.objectCounter = 16777215U;
		}

		public void RemovePlayer(global::TNet.TcpPlayer p, global::TNet.List<uint> destroyedObjects)
		{
			destroyedObjects.Clear();
			if (p == this.host)
			{
				this.host = null;
			}
			if (this.players.Remove(p))
			{
				int i = this.created.size;
				while (i > 0)
				{
					global::TNet.Channel.CreatedObject createdObject = this.created[--i];
					if (createdObject.type == 2 && createdObject.playerID == p.id)
					{
						if (createdObject.buffer != null)
						{
							createdObject.buffer.Recycle();
						}
						this.created.RemoveAt(i);
						destroyedObjects.Add(createdObject.uniqueID);
						this.DestroyObjectRFCs(createdObject.uniqueID);
					}
				}
				if ((!this.persistent || this.playerLimit < 1) && this.players.size == 0)
				{
					this.closed = true;
					for (int j = 0; j < this.rfcs.size; j++)
					{
						global::TNet.Channel.RFC rfc = this.rfcs[j];
						if (rfc.buffer != null)
						{
							rfc.buffer.Recycle();
						}
					}
					this.rfcs.Clear();
				}
			}
		}

		public bool DestroyObject(uint uniqueID)
		{
			if (!this.destroyed.Contains(uniqueID))
			{
				for (int i = 0; i < this.created.size; i++)
				{
					global::TNet.Channel.CreatedObject createdObject = this.created[i];
					if (createdObject.uniqueID == uniqueID)
					{
						if (createdObject.buffer != null)
						{
							createdObject.buffer.Recycle();
						}
						this.created.RemoveAt(i);
						this.DestroyObjectRFCs(uniqueID);
						return true;
					}
				}
				this.destroyed.Add(uniqueID);
				this.DestroyObjectRFCs(uniqueID);
				return true;
			}
			return false;
		}

		public void DestroyObjectRFCs(uint objectID)
		{
			int i = 0;
			while (i < this.rfcs.size)
			{
				global::TNet.Channel.RFC rfc = this.rfcs[i];
				if (rfc.objectID == objectID)
				{
					this.rfcs.RemoveAt(i);
					rfc.buffer.Recycle();
				}
				else
				{
					i++;
				}
			}
		}

		public void CreateRFC(uint inID, string funcName, global::TNet.Buffer buffer)
		{
			if (this.closed || buffer == null)
			{
				return;
			}
			buffer.MarkAsUsed();
			for (int i = 0; i < this.rfcs.size; i++)
			{
				global::TNet.Channel.RFC rfc = this.rfcs[i];
				if (rfc.uid == inID && rfc.functionName == funcName)
				{
					if (rfc.buffer != null)
					{
						rfc.buffer.Recycle();
					}
					rfc.buffer = buffer;
					return;
				}
			}
			global::TNet.Channel.RFC rfc2 = new global::TNet.Channel.RFC();
			rfc2.uid = inID;
			rfc2.buffer = buffer;
			rfc2.functionName = funcName;
			this.rfcs.Add(rfc2);
		}

		public void DeleteRFC(uint inID, string funcName)
		{
			for (int i = 0; i < this.rfcs.size; i++)
			{
				global::TNet.Channel.RFC rfc = this.rfcs[i];
				if (rfc.uid == inID && rfc.functionName == funcName)
				{
					this.rfcs.RemoveAt(i);
					rfc.buffer.Recycle();
				}
			}
		}

		public void SaveTo(global::System.IO.BinaryWriter writer)
		{
			writer.Write(11);
			writer.Write(this.level);
			writer.Write(this.data);
			writer.Write(this.objectCounter);
			writer.Write(this.password);
			writer.Write(this.persistent);
			writer.Write(this.playerLimit);
			writer.Write(this.rfcs.size);
			for (int i = 0; i < this.rfcs.size; i++)
			{
				global::TNet.Channel.RFC rfc = this.rfcs[i];
				writer.Write(rfc.uid);
				if (rfc.functionID == 0U)
				{
					writer.Write(rfc.functionName);
				}
				writer.Write(rfc.buffer.size);
				if (rfc.buffer.size > 0)
				{
					rfc.buffer.BeginReading();
					writer.Write(rfc.buffer.buffer, rfc.buffer.position, rfc.buffer.size);
				}
			}
			writer.Write(this.created.size);
			for (int j = 0; j < this.created.size; j++)
			{
				global::TNet.Channel.CreatedObject createdObject = this.created[j];
				writer.Write(createdObject.playerID);
				writer.Write(createdObject.uniqueID);
				writer.Write(createdObject.objectID);
				writer.Write(createdObject.buffer.size);
				if (createdObject.buffer.size > 0)
				{
					createdObject.buffer.BeginReading();
					writer.Write(createdObject.buffer.buffer, createdObject.buffer.position, createdObject.buffer.size);
				}
			}
			writer.Write(this.destroyed.size);
			for (int k = 0; k < this.destroyed.size; k++)
			{
				writer.Write(this.destroyed[k]);
			}
		}

		public bool LoadFrom(global::System.IO.BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num != 11)
			{
				return false;
			}
			for (int i = 0; i < this.rfcs.size; i++)
			{
				global::TNet.Channel.RFC rfc = this.rfcs[i];
				if (rfc.buffer != null)
				{
					rfc.buffer.Recycle();
				}
			}
			this.rfcs.Clear();
			this.created.Clear();
			this.destroyed.Clear();
			this.level = reader.ReadString();
			this.data = reader.ReadString();
			this.objectCounter = reader.ReadUInt32();
			this.password = reader.ReadString();
			this.persistent = reader.ReadBoolean();
			this.playerLimit = reader.ReadUInt16();
			int num2 = reader.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				global::TNet.Channel.RFC rfc2 = new global::TNet.Channel.RFC();
				rfc2.uid = reader.ReadUInt32();
				if (rfc2.functionID == 0U)
				{
					rfc2.functionName = reader.ReadString();
				}
				global::TNet.Buffer buffer = global::TNet.Buffer.Create();
				buffer.BeginWriting(false).Write(reader.ReadBytes(reader.ReadInt32()));
				rfc2.buffer = buffer;
				this.rfcs.Add(rfc2);
			}
			num2 = reader.ReadInt32();
			for (int k = 0; k < num2; k++)
			{
				global::TNet.Channel.CreatedObject createdObject = new global::TNet.Channel.CreatedObject();
				createdObject.playerID = reader.ReadInt32();
				createdObject.uniqueID = reader.ReadUInt32();
				createdObject.objectID = reader.ReadUInt16();
				global::TNet.Buffer buffer2 = global::TNet.Buffer.Create();
				buffer2.BeginWriting(false).Write(reader.ReadBytes(reader.ReadInt32()));
				createdObject.buffer = buffer2;
				this.created.Add(createdObject);
			}
			num2 = reader.ReadInt32();
			for (int l = 0; l < num2; l++)
			{
				this.destroyed.Add(reader.ReadUInt32());
			}
			return true;
		}

		public int id;

		public string password = string.Empty;

		public string level = string.Empty;

		public string data = string.Empty;

		public bool persistent;

		public bool closed;

		public ushort playerLimit = ushort.MaxValue;

		public global::TNet.List<global::TNet.TcpPlayer> players = new global::TNet.List<global::TNet.TcpPlayer>();

		public global::TNet.List<global::TNet.Channel.RFC> rfcs = new global::TNet.List<global::TNet.Channel.RFC>();

		public global::TNet.List<global::TNet.Channel.CreatedObject> created = new global::TNet.List<global::TNet.Channel.CreatedObject>();

		public global::TNet.List<uint> destroyed = new global::TNet.List<uint>();

		public uint objectCounter = 16777215U;

		public global::TNet.TcpPlayer host;

		public class RFC
		{
			public uint objectID
			{
				get
				{
					return this.uid >> 8;
				}
			}

			public uint functionID
			{
				get
				{
					return this.uid & 255U;
				}
			}

			public uint uid;

			public string functionName;

			public global::TNet.Buffer buffer;
		}

		public class CreatedObject
		{
			public int playerID;

			public ushort objectID;

			public uint uniqueID;

			public byte type;

			public global::TNet.Buffer buffer;
		}
	}
}
