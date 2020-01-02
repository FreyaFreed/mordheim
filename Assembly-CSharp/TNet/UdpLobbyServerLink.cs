using System;
using System.IO;
using System.Net;
using System.Threading;

namespace TNet
{
	public class UdpLobbyServerLink : global::TNet.LobbyServerLink
	{
		public UdpLobbyServerLink(global::System.Net.IPEndPoint address) : base(null)
		{
			this.mRemoteAddress = address;
		}

		public override bool isActive
		{
			get
			{
				return this.mUdp != null && this.mUdp.isActive;
			}
		}

		~UdpLobbyServerLink()
		{
			if (this.mUdp != null)
			{
				this.mUdp.Stop();
				this.mUdp = null;
			}
		}

		public override void Start()
		{
			base.Start();
			if (this.mUdp == null)
			{
				this.mUdp = new global::TNet.UdpProtocol();
				this.mUdp.Start();
			}
		}

		public override void SendUpdate(global::TNet.GameServer server)
		{
			if (!this.mShutdown)
			{
				this.mNextSend = 0L;
				this.mGameServer = server;
				if (this.mThread == null)
				{
					this.mThread = new global::System.Threading.Thread(new global::System.Threading.ThreadStart(this.ThreadFunction));
					this.mThread.Start();
				}
			}
		}

		private void ThreadFunction()
		{
			this.mInternal = new global::System.Net.IPEndPoint(global::TNet.Tools.localAddress, this.mGameServer.tcpPort);
			this.mExternal = new global::System.Net.IPEndPoint(global::TNet.Tools.externalAddress, this.mGameServer.tcpPort);
			for (;;)
			{
				long num = global::System.DateTime.Now.Ticks / 10000L;
				if (this.mShutdown)
				{
					break;
				}
				if (this.mNextSend < num && this.mGameServer != null)
				{
					this.mNextSend = num + 3000L;
					global::TNet.Buffer buffer = global::TNet.Buffer.Create();
					global::System.IO.BinaryWriter binaryWriter = buffer.BeginPacket(global::TNet.Packet.RequestAddServer);
					binaryWriter.Write(1);
					binaryWriter.Write(this.mGameServer.name);
					binaryWriter.Write((short)this.mGameServer.playerCount);
					global::TNet.Tools.Serialize(binaryWriter, this.mInternal);
					global::TNet.Tools.Serialize(binaryWriter, this.mExternal);
					buffer.EndPacket();
					this.mUdp.Send(buffer, this.mRemoteAddress);
					buffer.Recycle();
				}
				global::System.Threading.Thread.Sleep(10);
			}
			global::TNet.Buffer buffer2 = global::TNet.Buffer.Create();
			global::System.IO.BinaryWriter binaryWriter2 = buffer2.BeginPacket(global::TNet.Packet.RequestRemoveServer);
			binaryWriter2.Write(1);
			global::TNet.Tools.Serialize(binaryWriter2, this.mInternal);
			global::TNet.Tools.Serialize(binaryWriter2, this.mExternal);
			buffer2.EndPacket();
			this.mUdp.Send(buffer2, this.mRemoteAddress);
			buffer2.Recycle();
			this.mThread = null;
		}

		private global::TNet.UdpProtocol mUdp;

		private global::System.Net.IPEndPoint mRemoteAddress;

		private long mNextSend;
	}
}
