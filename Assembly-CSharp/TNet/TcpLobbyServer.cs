using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TNet
{
	public class TcpLobbyServer : global::TNet.LobbyServer
	{
		public override int port
		{
			get
			{
				return this.mPort;
			}
		}

		public override bool isActive
		{
			get
			{
				return this.mListener != null;
			}
		}

		public override bool Start(int listenPort)
		{
			this.Stop();
			try
			{
				this.mListener = new global::System.Net.Sockets.TcpListener(global::System.Net.IPAddress.Any, listenPort);
				this.mListener.Start(50);
				this.mPort = listenPort;
			}
			catch (global::System.Exception)
			{
				return false;
			}
			this.mThread = new global::System.Threading.Thread(new global::System.Threading.ThreadStart(this.ThreadFunction));
			this.mThread.Start();
			return true;
		}

		public override void Stop()
		{
			if (this.mThread != null)
			{
				this.mThread.Abort();
				this.mThread = null;
			}
			if (this.mListener != null)
			{
				this.mListener.Stop();
				this.mListener = null;
			}
			this.mList.Clear();
		}

		private global::System.IO.BinaryWriter BeginSend(global::TNet.Packet type)
		{
			this.mBuffer = global::TNet.Buffer.Create();
			return this.mBuffer.BeginPacket(type);
		}

		private void EndSend(global::TNet.TcpProtocol tc)
		{
			this.mBuffer.EndPacket();
			tc.SendTcpPacket(this.mBuffer);
			this.mBuffer.Recycle();
			this.mBuffer = null;
		}

		private void ThreadFunction()
		{
			for (;;)
			{
				this.mTime = global::System.DateTime.Now.Ticks / 10000L;
				while (this.mListener != null && this.mListener.Pending())
				{
					global::TNet.TcpProtocol tcpProtocol = new global::TNet.TcpProtocol();
					tcpProtocol.StartReceiving(this.mListener.AcceptSocket());
					this.mTcp.Add(tcpProtocol);
				}
				global::TNet.Buffer buffer = null;
				for (int i = 0; i < this.mTcp.size; i++)
				{
					global::TNet.TcpProtocol tcpProtocol2 = this.mTcp[i];
					while (tcpProtocol2.ReceivePacket(out buffer))
					{
						try
						{
							if (!this.ProcessPacket(buffer, tcpProtocol2))
							{
								this.RemoveServer(tcpProtocol2);
								tcpProtocol2.Disconnect();
							}
						}
						catch (global::System.Exception)
						{
							this.RemoveServer(tcpProtocol2);
							tcpProtocol2.Disconnect();
						}
						if (buffer != null)
						{
							buffer.Recycle();
							buffer = null;
						}
					}
				}
				if (this.mTcp.size > this.instantUpdatesClientLimit)
				{
					this.mInstantUpdates = false;
				}
				for (int j = 0; j < this.mTcp.size; j++)
				{
					global::TNet.TcpProtocol tcpProtocol3 = this.mTcp[j];
					if (tcpProtocol3.stage == global::TNet.TcpProtocol.Stage.Connected && tcpProtocol3.data != null && tcpProtocol3.data is long)
					{
						long num = (long)tcpProtocol3.data;
						if (num != 0L)
						{
							if (num >= this.mLastChange)
							{
								goto IL_1D5;
							}
							if (!this.mInstantUpdates && num + 4000L > this.mTime)
							{
								goto IL_1D5;
							}
						}
						if (buffer == null)
						{
							buffer = global::TNet.Buffer.Create();
							global::System.IO.BinaryWriter writer = buffer.BeginPacket(global::TNet.Packet.ResponseServerList);
							this.mList.WriteTo(writer);
							buffer.EndPacket();
						}
						tcpProtocol3.SendTcpPacket(buffer);
						tcpProtocol3.data = this.mTime;
					}
					IL_1D5:;
				}
				if (buffer != null)
				{
					buffer.Recycle();
					buffer = null;
				}
				global::System.Threading.Thread.Sleep(1);
			}
		}

		private bool ProcessPacket(global::TNet.Buffer buffer, global::TNet.TcpProtocol tc)
		{
			if (tc.stage == global::TNet.TcpProtocol.Stage.Verifying)
			{
				return tc.VerifyRequestID(buffer, false);
			}
			global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
			global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
			global::TNet.Packet packet2 = packet;
			switch (packet2)
			{
			case global::TNet.Packet.Error:
				return false;
			case global::TNet.Packet.Disconnect:
				this.RemoveServer(tc);
				this.mTcp.Remove(tc);
				return true;
			default:
				switch (packet2)
				{
				case global::TNet.Packet.RequestSaveFile:
				{
					string fileName = binaryReader.ReadString();
					byte[] data = binaryReader.ReadBytes(binaryReader.ReadInt32());
					base.SaveFile(fileName, data);
					break;
				}
				case global::TNet.Packet.RequestLoadFile:
				{
					string text = binaryReader.ReadString();
					byte[] array = base.LoadFile(text);
					global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.ResponseLoadFile);
					binaryWriter.Write(text);
					if (array != null)
					{
						binaryWriter.Write(array.Length);
						binaryWriter.Write(array);
					}
					else
					{
						binaryWriter.Write(0);
					}
					this.EndSend(tc);
					break;
				}
				case global::TNet.Packet.RequestDeleteFile:
					base.DeleteFile(binaryReader.ReadString());
					break;
				default:
					switch (packet2)
					{
					case global::TNet.Packet.RequestAddServer:
					{
						if (binaryReader.ReadUInt16() != 1)
						{
							return false;
						}
						global::TNet.ServerList.Entry entry = new global::TNet.ServerList.Entry();
						entry.ReadFrom(binaryReader);
						if (entry.externalAddress.Address.Equals(global::System.Net.IPAddress.None))
						{
							entry.externalAddress = tc.tcpEndPoint;
						}
						this.mList.Add(entry, this.mTime).data = tc;
						this.mLastChange = this.mTime;
						return true;
					}
					case global::TNet.Packet.RequestRemoveServer:
					{
						if (binaryReader.ReadUInt16() != 1)
						{
							return false;
						}
						global::System.Net.IPEndPoint internalAddress;
						global::TNet.Tools.Serialize(binaryReader, out internalAddress);
						global::System.Net.IPEndPoint tcpEndPoint;
						global::TNet.Tools.Serialize(binaryReader, out tcpEndPoint);
						if (tcpEndPoint.Address.Equals(global::System.Net.IPAddress.None))
						{
							tcpEndPoint = tc.tcpEndPoint;
						}
						this.RemoveServer(internalAddress, tcpEndPoint);
						return true;
					}
					case global::TNet.Packet.RequestServerList:
						if (binaryReader.ReadUInt16() != 1)
						{
							return false;
						}
						tc.data = 0L;
						return true;
					}
					break;
				}
				break;
			case global::TNet.Packet.RequestPing:
				this.BeginSend(global::TNet.Packet.ResponsePing);
				this.EndSend(tc);
				break;
			}
			return false;
		}

		private bool RemoveServer(global::TNet.Player player)
		{
			bool result = false;
			global::TNet.List<global::TNet.ServerList.Entry> list = this.mList.list;
			lock (list)
			{
				int i = this.mList.list.size;
				while (i > 0)
				{
					global::TNet.ServerList.Entry entry = this.mList.list[--i];
					if (entry.data == player)
					{
						this.mList.list.RemoveAt(i);
						this.mLastChange = this.mTime;
						result = true;
					}
				}
			}
			return result;
		}

		public override void AddServer(string name, int playerCount, global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress)
		{
			this.mList.Add(name, playerCount, internalAddress, externalAddress, this.mTime);
			this.mLastChange = this.mTime;
		}

		public override void RemoveServer(global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress)
		{
			if (this.mList.Remove(internalAddress, externalAddress))
			{
				this.mLastChange = this.mTime;
			}
		}

		private global::TNet.ServerList mList = new global::TNet.ServerList();

		private long mTime;

		private long mLastChange;

		private global::TNet.List<global::TNet.TcpProtocol> mTcp = new global::TNet.List<global::TNet.TcpProtocol>();

		private global::System.Net.Sockets.TcpListener mListener;

		private int mPort;

		private global::System.Threading.Thread mThread;

		private bool mInstantUpdates = true;

		private global::TNet.Buffer mBuffer;

		public int instantUpdatesClientLimit = 50;
	}
}
