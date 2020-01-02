using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace TNet
{
	public class Buffer
	{
		private Buffer()
		{
			this.mStream = new global::System.IO.MemoryStream();
			this.mWriter = new global::System.IO.BinaryWriter(this.mStream);
			this.mReader = new global::System.IO.BinaryReader(this.mStream);
		}

		~Buffer()
		{
			this.mStream.Dispose();
		}

		public int size
		{
			get
			{
				return (!this.mWriting) ? (this.mSize - (int)this.mStream.Position) : ((int)this.mStream.Position);
			}
		}

		public int position
		{
			get
			{
				return (int)this.mStream.Position;
			}
			set
			{
				this.mStream.Seek((long)value, global::System.IO.SeekOrigin.Begin);
			}
		}

		public global::System.IO.MemoryStream stream
		{
			get
			{
				return this.mStream;
			}
		}

		public byte[] buffer
		{
			get
			{
				return this.mStream.GetBuffer();
			}
		}

		public static int recycleQueue
		{
			get
			{
				return global::TNet.Buffer.mPool.size;
			}
		}

		public static global::TNet.Buffer Create()
		{
			return global::TNet.Buffer.Create(true);
		}

		public static global::TNet.Buffer Create(bool markAsUsed)
		{
			global::TNet.Buffer buffer = null;
			if (global::TNet.Buffer.mPool.size == 0)
			{
				buffer = new global::TNet.Buffer();
			}
			else
			{
				global::TNet.List<global::TNet.Buffer> obj = global::TNet.Buffer.mPool;
				lock (obj)
				{
					if (global::TNet.Buffer.mPool.size != 0)
					{
						buffer = global::TNet.Buffer.mPool.Pop();
						buffer.mInPool = false;
					}
					else
					{
						buffer = new global::TNet.Buffer();
					}
				}
			}
			buffer.mCounter = ((!markAsUsed) ? 0 : 1);
			return buffer;
		}

		public bool Recycle()
		{
			return this.Recycle(true);
		}

		public bool Recycle(bool checkUsedFlag)
		{
			if (!this.mInPool && (!checkUsedFlag || this.MarkAsUnused()))
			{
				this.mInPool = true;
				global::TNet.List<global::TNet.Buffer> obj = global::TNet.Buffer.mPool;
				lock (obj)
				{
					this.Clear();
					global::TNet.Buffer.mPool.Add(this);
				}
				return true;
			}
			return false;
		}

		public static void Recycle(global::System.Collections.Generic.Queue<global::TNet.Buffer> list)
		{
			global::TNet.List<global::TNet.Buffer> obj = global::TNet.Buffer.mPool;
			lock (obj)
			{
				while (list.Count != 0)
				{
					global::TNet.Buffer buffer = list.Dequeue();
					buffer.Clear();
					global::TNet.Buffer.mPool.Add(buffer);
				}
			}
		}

		public static void Recycle(global::System.Collections.Generic.Queue<global::TNet.Datagram> list)
		{
			global::TNet.List<global::TNet.Buffer> obj = global::TNet.Buffer.mPool;
			lock (obj)
			{
				while (list.Count != 0)
				{
					global::TNet.Buffer buffer = list.Dequeue().buffer;
					buffer.Clear();
					global::TNet.Buffer.mPool.Add(buffer);
				}
			}
		}

		public static void Recycle(global::TNet.List<global::TNet.Buffer> list)
		{
			global::TNet.List<global::TNet.Buffer> obj = global::TNet.Buffer.mPool;
			lock (obj)
			{
				for (int i = 0; i < list.size; i++)
				{
					global::TNet.Buffer buffer = list[i];
					buffer.Clear();
					global::TNet.Buffer.mPool.Add(buffer);
				}
				list.Clear();
			}
		}

		public static void Recycle(global::TNet.List<global::TNet.Datagram> list)
		{
			global::TNet.List<global::TNet.Buffer> obj = global::TNet.Buffer.mPool;
			lock (obj)
			{
				for (int i = 0; i < list.size; i++)
				{
					global::TNet.Buffer buffer = list[i].buffer;
					buffer.Clear();
					global::TNet.Buffer.mPool.Add(buffer);
				}
				list.Clear();
			}
		}

		public void MarkAsUsed()
		{
			global::System.Threading.Interlocked.Increment(ref this.mCounter);
		}

		public bool MarkAsUnused()
		{
			if (global::System.Threading.Interlocked.Decrement(ref this.mCounter) > 0)
			{
				return false;
			}
			this.mSize = 0;
			this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
			this.mWriting = true;
			return true;
		}

		public void Clear()
		{
			this.mCounter = 0;
			this.mSize = 0;
			if (this.mStream.Capacity > 1024)
			{
				this.mStream.SetLength(256L);
			}
			this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
			this.mWriting = true;
		}

		public void CopyTo(global::TNet.Buffer target)
		{
			global::System.IO.BinaryWriter binaryWriter = target.BeginWriting(false);
			int size = this.size;
			if (size > 0)
			{
				binaryWriter.Write(this.buffer, this.position, size);
			}
			target.EndWriting();
		}

		public void Dispose()
		{
			this.mStream.Dispose();
		}

		public global::System.IO.BinaryWriter BeginWriting(bool append)
		{
			if (!append || !this.mWriting)
			{
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				this.mSize = 0;
			}
			this.mWriting = true;
			return this.mWriter;
		}

		public global::System.IO.BinaryWriter BeginWriting(int startOffset)
		{
			this.mStream.Seek((long)startOffset, global::System.IO.SeekOrigin.Begin);
			this.mWriting = true;
			return this.mWriter;
		}

		public int EndWriting()
		{
			if (this.mWriting)
			{
				this.mSize = this.position;
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				this.mWriting = false;
			}
			return this.mSize;
		}

		public global::System.IO.BinaryReader BeginReading()
		{
			if (this.mWriting)
			{
				this.mWriting = false;
				this.mSize = (int)this.mStream.Position;
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
			}
			return this.mReader;
		}

		public global::System.IO.BinaryReader BeginReading(int startOffset)
		{
			if (this.mWriting)
			{
				this.mWriting = false;
				this.mSize = (int)this.mStream.Position;
			}
			this.mStream.Seek((long)startOffset, global::System.IO.SeekOrigin.Begin);
			return this.mReader;
		}

		public int PeekByte(int offset)
		{
			long position = this.mStream.Position;
			if ((long)(offset + 1) > position)
			{
				return -1;
			}
			this.mStream.Seek((long)offset, global::System.IO.SeekOrigin.Begin);
			int result = (int)this.mReader.ReadByte();
			this.mStream.Seek(position, global::System.IO.SeekOrigin.Begin);
			return result;
		}

		public int PeekInt(int offset)
		{
			long position = this.mStream.Position;
			if ((long)(offset + 4) > position)
			{
				return -1;
			}
			this.mStream.Seek((long)offset, global::System.IO.SeekOrigin.Begin);
			int result = this.mReader.ReadInt32();
			this.mStream.Seek(position, global::System.IO.SeekOrigin.Begin);
			return result;
		}

		public global::System.IO.BinaryWriter BeginPacket(byte packetID)
		{
			global::System.IO.BinaryWriter binaryWriter = this.BeginWriting(false);
			binaryWriter.Write(0);
			binaryWriter.Write(packetID);
			return binaryWriter;
		}

		public global::System.IO.BinaryWriter BeginPacket(global::TNet.Packet packet)
		{
			global::System.IO.BinaryWriter binaryWriter = this.BeginWriting(false);
			binaryWriter.Write(0);
			binaryWriter.Write((byte)packet);
			return binaryWriter;
		}

		public global::System.IO.BinaryWriter BeginPacket(global::TNet.Packet packet, int startOffset)
		{
			global::System.IO.BinaryWriter binaryWriter = this.BeginWriting(startOffset);
			binaryWriter.Write(0);
			binaryWriter.Write((byte)packet);
			return binaryWriter;
		}

		public int EndPacket()
		{
			if (this.mWriting)
			{
				this.mSize = this.position;
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				this.mWriter.Write(this.mSize - 4);
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				this.mWriting = false;
			}
			return this.mSize;
		}

		public int EndTcpPacketStartingAt(int startOffset)
		{
			if (this.mWriting)
			{
				this.mSize = this.position;
				this.mStream.Seek((long)startOffset, global::System.IO.SeekOrigin.Begin);
				this.mWriter.Write(this.mSize - 4 - startOffset);
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				this.mWriting = false;
			}
			return this.mSize;
		}

		public int EndTcpPacketWithOffset(int offset)
		{
			if (this.mWriting)
			{
				this.mSize = this.position;
				this.mStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				this.mWriter.Write(this.mSize - 4);
				this.mStream.Seek((long)offset, global::System.IO.SeekOrigin.Begin);
				this.mWriting = false;
			}
			return this.mSize;
		}

		private static global::TNet.List<global::TNet.Buffer> mPool = new global::TNet.List<global::TNet.Buffer>();

		private global::System.IO.MemoryStream mStream;

		private global::System.IO.BinaryWriter mWriter;

		private global::System.IO.BinaryReader mReader;

		private int mCounter;

		private int mSize;

		private bool mWriting;

		private bool mInPool;
	}
}
