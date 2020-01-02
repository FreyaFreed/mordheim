using System;
using System.Net;

namespace TNet
{
	public abstract class LobbyServer : global::TNet.FileServer
	{
		public abstract int port { get; }

		public abstract bool isActive { get; }

		public abstract bool Start(int listenPort);

		public abstract void Stop();

		public abstract void AddServer(string name, int playerCount, global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress);

		public abstract void RemoveServer(global::System.Net.IPEndPoint internalAddress, global::System.Net.IPEndPoint externalAddress);
	}
}
