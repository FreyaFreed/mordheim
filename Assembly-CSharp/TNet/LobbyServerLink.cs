using System;
using System.Net;
using System.Threading;

namespace TNet
{
	public class LobbyServerLink
	{
		public LobbyServerLink(global::TNet.LobbyServer lobbyServer)
		{
			this.mLobby = lobbyServer;
		}

		public virtual bool isActive
		{
			get
			{
				return this.mLobby != null && this.mExternal != null;
			}
		}

		public virtual void Start()
		{
			this.mShutdown = false;
		}

		public virtual void Stop()
		{
			if (!this.mShutdown)
			{
				this.mShutdown = true;
				if (this.mExternal != null && this.mLobby != null)
				{
					this.mLobby.RemoveServer(this.mInternal, this.mExternal);
				}
			}
		}

		public virtual void SendUpdate(global::TNet.GameServer gameServer)
		{
			if (!this.mShutdown)
			{
				this.mGameServer = gameServer;
				if (this.mExternal != null)
				{
					long num = global::System.DateTime.Now.Ticks / 10000L;
					this.mNextSend = num + 3000L;
					this.mLobby.AddServer(this.mGameServer.name, this.mGameServer.playerCount, this.mInternal, this.mExternal);
				}
				else if (this.mThread == null)
				{
					this.mThread = new global::System.Threading.Thread(new global::System.Threading.ThreadStart(this.SendThread));
					this.mThread.Start();
				}
			}
		}

		private void SendThread()
		{
			this.mInternal = new global::System.Net.IPEndPoint(global::TNet.Tools.localAddress, this.mGameServer.tcpPort);
			this.mExternal = new global::System.Net.IPEndPoint(global::TNet.Tools.externalAddress, this.mGameServer.tcpPort);
			if (this.mLobby is global::TNet.UdpLobbyServer)
			{
				while (!this.mShutdown)
				{
					long num = global::System.DateTime.Now.Ticks / 10000L;
					if (this.mNextSend < num && this.mGameServer != null)
					{
						this.mNextSend = num + 3000L;
						this.mLobby.AddServer(this.mGameServer.name, this.mGameServer.playerCount, this.mInternal, this.mExternal);
					}
					global::System.Threading.Thread.Sleep(10);
				}
			}
			else
			{
				this.mLobby.AddServer(this.mGameServer.name, this.mGameServer.playerCount, this.mInternal, this.mExternal);
			}
			this.mThread = null;
		}

		private global::TNet.LobbyServer mLobby;

		private long mNextSend;

		protected global::TNet.GameServer mGameServer;

		protected global::System.Threading.Thread mThread;

		protected global::System.Net.IPEndPoint mInternal;

		protected global::System.Net.IPEndPoint mExternal;

		protected bool mShutdown;
	}
}
