using System;
using System.IO;
using System.Net;
using System.Threading;

namespace TNet
{
	public class UdpLobbyServer : global::TNet.LobbyServer
	{
		public override int port
		{
			get
			{
				return (!this.mUdp.isActive) ? 0 : this.mUdp.listeningPort;
			}
		}

		public override bool isActive
		{
			get
			{
				return this.mUdp != null && this.mUdp.isActive;
			}
		}

		public override bool Start(int listenPort)
		{
			this.Stop();
			this.mUdp = new global::TNet.UdpProtocol();
			if (!this.mUdp.Start(listenPort))
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
			if (this.mUdp != null)
			{
				this.mUdp.Stop();
				this.mUdp = null;
			}
			this.mList.Clear();
		}

		private void ThreadFunction()
		{
			for (;;)
			{
				this.mTime = global::System.DateTime.Now.Ticks / 10000L;
				this.mList.Cleanup(this.mTime);
				global::TNet.Buffer buffer;
				global::System.Net.IPEndPoint ip;
				while (this.mUdp != null && this.mUdp.listeningPort != 0 && this.mUdp.ReceivePacket(out buffer, out ip))
				{
					try
					{
						this.ProcessPacket(buffer, ip);
					}
					catch (global::System.Exception)
					{
					}
					if (buffer != null)
					{
						buffer.Recycle();
						buffer = null;
					}
				}
				global::System.Threading.Thread.Sleep(1);
			}
		}

		private bool ProcessPacket(global::TNet.Buffer buffer, global::System.Net.IPEndPoint ip)
		{
			global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
			global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
			global::TNet.Packet packet2 = packet;
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
					entry.externalAddress = ip;
				}
				this.mList.Add(entry, this.mTime);
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
				global::System.Net.IPEndPoint ipendPoint;
				global::TNet.Tools.Serialize(binaryReader, out ipendPoint);
				if (ipendPoint.Address.Equals(global::System.Net.IPAddress.None))
				{
					ipendPoint = ip;
				}
				this.RemoveServer(internalAddress, ipendPoint);
				return true;
			}
			case global::TNet.Packet.RequestServerList:
				if (binaryReader.ReadUInt16() != 1)
				{
					return false;
				}
				this.mList.WriteTo(this.BeginSend(global::TNet.Packet.ResponseServerList));
				this.EndSend(ip);
				return true;
			default:
				if (packet2 == global::TNet.Packet.RequestPing)
				{
					this.BeginSend(global::TNet.Packet.ResponsePing);
					this.EndSend(ip);
				}
				return false;
			}
		}

		public override void AddServer(string name, int playerCount, global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress)
		{
			this.mList.Add(name, playerCount, internalAddress, externalAddress, this.mTime);
		}

		public override void RemoveServer(global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress)
		{
			this.mList.Remove(internalAddress, externalAddress);
		}

		private global::System.IO.BinaryWriter BeginSend(global::TNet.Packet packet)
		{
			this.mBuffer = global::TNet.Buffer.Create();
			return this.mBuffer.BeginPacket(packet);
		}

		private void EndSend(global::System.Net.IPEndPoint ip)
		{
			this.mBuffer.EndPacket();
			this.mUdp.Send(this.mBuffer, ip);
			this.mBuffer.Recycle();
			this.mBuffer = null;
		}

		private global::TNet.ServerList mList = new global::TNet.ServerList();

		private long mTime;

		private global::TNet.UdpProtocol mUdp;

		private global::System.Threading.Thread mThread;

		private global::TNet.Buffer mBuffer;
	}
}
