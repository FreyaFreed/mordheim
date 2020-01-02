using System;
using System.IO;
using System.Net;
using System.Threading;

namespace TNet
{
	public class TcpLobbyServerLink : global::TNet.LobbyServerLink
	{
		public TcpLobbyServerLink(global::System.Net.IPEndPoint address) : base(null)
		{
			this.mRemoteAddress = address;
		}

		public override bool isActive
		{
			get
			{
				return this.mTcp.isConnected;
			}
		}

		public override void Start()
		{
			base.Start();
			if (this.mTcp == null)
			{
				this.mTcp = new global::TNet.TcpProtocol();
				this.mTcp.name = "Link";
			}
			this.mNextConnect = 0L;
		}

		public override void SendUpdate(global::TNet.GameServer server)
		{
			if (!this.mShutdown)
			{
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
				if (this.mGameServer != null && !this.mTcp.isConnected && this.mNextConnect < num)
				{
					this.mNextConnect = num + 15000L;
					this.mTcp.Connect(this.mRemoteAddress);
				}
				global::TNet.Buffer buffer;
				while (this.mTcp.ReceivePacket(out buffer))
				{
					global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
					global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
					if (this.mTcp.stage == global::TNet.TcpProtocol.Stage.Verifying)
					{
						if (!this.mTcp.VerifyResponseID(packet, binaryReader))
						{
							goto IL_FB;
						}
						this.mWasConnected = true;
					}
					else if (packet == global::TNet.Packet.Error)
					{
						this.mNextConnect = ((!this.mWasConnected) ? (num + 30000L) : 0L);
					}
					buffer.Recycle();
				}
				if (this.mGameServer != null && this.mTcp.isConnected)
				{
					global::System.IO.BinaryWriter binaryWriter = this.mTcp.BeginSend(global::TNet.Packet.RequestAddServer);
					binaryWriter.Write(1);
					binaryWriter.Write(this.mGameServer.name);
					binaryWriter.Write((short)this.mGameServer.playerCount);
					global::TNet.Tools.Serialize(binaryWriter, this.mInternal);
					global::TNet.Tools.Serialize(binaryWriter, this.mExternal);
					this.mTcp.EndSend();
					this.mGameServer = null;
				}
				global::System.Threading.Thread.Sleep(10);
			}
			this.mTcp.Disconnect();
			this.mThread = null;
			return;
			IL_FB:
			this.mThread = null;
		}

		private global::TNet.TcpProtocol mTcp;

		private global::System.Net.IPEndPoint mRemoteAddress;

		private long mNextConnect;

		private bool mWasConnected;
	}
}
