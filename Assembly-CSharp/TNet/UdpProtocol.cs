using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace TNet
{
	public class UdpProtocol
	{
		public bool isActive
		{
			get
			{
				return this.mPort != -1;
			}
		}

		public int listeningPort
		{
			get
			{
				return (this.mPort <= 0) ? 0 : this.mPort;
			}
		}

		public bool Start()
		{
			return this.Start(0);
		}

		public bool Start(int port)
		{
			this.Stop();
			this.mPort = port;
			this.mSocket = new global::System.Net.Sockets.Socket(global::System.Net.Sockets.AddressFamily.InterNetwork, global::System.Net.Sockets.SocketType.Dgram, global::System.Net.Sockets.ProtocolType.Udp);
			this.mSocket.MulticastLoopback = true;
			this.mMulticast = global::TNet.UdpProtocol.useMulticasting;
			if (global::TNet.UdpProtocol.useMulticasting)
			{
				global::TNet.List<global::System.Net.IPAddress> localAddresses = global::TNet.Tools.localAddresses;
				foreach (global::System.Net.IPAddress mcint in localAddresses)
				{
					global::System.Net.Sockets.MulticastOption optionValue = new global::System.Net.Sockets.MulticastOption(global::TNet.UdpProtocol.multicastIP, mcint);
					this.mSocket.SetSocketOption(global::System.Net.Sockets.SocketOptionLevel.IP, global::System.Net.Sockets.SocketOptionName.AddMembership, optionValue);
				}
			}
			else
			{
				this.mSocket.SetSocketOption(global::System.Net.Sockets.SocketOptionLevel.Socket, global::System.Net.Sockets.SocketOptionName.Broadcast, 1);
			}
			if (this.mPort == 0)
			{
				return true;
			}
			try
			{
				global::System.Net.IPAddress address = global::TNet.UdpProtocol.defaultNetworkInterface ?? global::System.Net.IPAddress.Any;
				this.mEndPoint = new global::System.Net.IPEndPoint(address, 0);
				global::TNet.UdpProtocol.mDefaultEndPoint = new global::System.Net.IPEndPoint(address, 0);
				this.mSocket.Bind(new global::System.Net.IPEndPoint(address, this.mPort));
				this.mSocket.BeginReceiveFrom(this.mTemp, 0, this.mTemp.Length, global::System.Net.Sockets.SocketFlags.None, ref this.mEndPoint, new global::System.AsyncCallback(this.OnReceive), null);
			}
			catch (global::System.Exception)
			{
				this.Stop();
				return false;
			}
			return true;
		}

		public void Stop()
		{
			this.mPort = -1;
			if (this.mSocket != null)
			{
				this.mSocket.Close();
				this.mSocket = null;
			}
			global::TNet.Buffer.Recycle(this.mIn);
			global::TNet.Buffer.Recycle(this.mOut);
		}

		private void OnReceive(global::System.IAsyncResult result)
		{
			if (!this.isActive)
			{
				return;
			}
			int num = 0;
			try
			{
				num = this.mSocket.EndReceiveFrom(result, ref this.mEndPoint);
			}
			catch (global::System.Exception ex)
			{
				this.Error(new global::System.Net.IPEndPoint(global::TNet.Tools.localAddress, 0), ex.Message);
			}
			if (num > 4)
			{
				global::TNet.Buffer buffer = global::TNet.Buffer.Create();
				buffer.BeginWriting(false).Write(this.mTemp, 0, num);
				buffer.BeginReading(4);
				global::TNet.Datagram item = default(global::TNet.Datagram);
				item.buffer = buffer;
				item.ip = (global::System.Net.IPEndPoint)this.mEndPoint;
				global::System.Collections.Generic.Queue<global::TNet.Datagram> obj = this.mIn;
				lock (obj)
				{
					this.mIn.Enqueue(item);
				}
			}
			if (this.mSocket != null)
			{
				this.mEndPoint = global::TNet.UdpProtocol.mDefaultEndPoint;
				this.mSocket.BeginReceiveFrom(this.mTemp, 0, this.mTemp.Length, global::System.Net.Sockets.SocketFlags.None, ref this.mEndPoint, new global::System.AsyncCallback(this.OnReceive), null);
			}
		}

		public bool ReceivePacket(out global::TNet.Buffer buffer, out global::System.Net.IPEndPoint source)
		{
			if (this.mPort == 0)
			{
				this.Stop();
				throw new global::System.InvalidOperationException("You must specify a non-zero port to UdpProtocol.Start() before you can receive data.");
			}
			if (this.mIn.Count != 0)
			{
				global::System.Collections.Generic.Queue<global::TNet.Datagram> obj = this.mIn;
				lock (obj)
				{
					global::TNet.Datagram datagram = this.mIn.Dequeue();
					buffer = datagram.buffer;
					source = datagram.ip;
					return true;
				}
			}
			buffer = null;
			source = null;
			return false;
		}

		public void SendEmptyPacket(global::System.Net.IPEndPoint ip)
		{
			global::TNet.Buffer buffer = global::TNet.Buffer.Create(false);
			buffer.BeginPacket(global::TNet.Packet.Empty);
			buffer.EndPacket();
			this.Send(buffer, ip);
		}

		public void Broadcast(global::TNet.Buffer buffer, int port)
		{
			if (buffer != null)
			{
				buffer.MarkAsUsed();
				global::System.Net.IPEndPoint ipendPoint = (!this.mMulticast) ? this.mBroadcastEndPoint : this.mMulticastEndPoint;
				ipendPoint.Port = port;
				try
				{
					this.mSocket.SendTo(buffer.buffer, buffer.position, buffer.size, global::System.Net.Sockets.SocketFlags.None, ipendPoint);
				}
				catch (global::System.Exception ex)
				{
					this.Error(null, ex.Message);
				}
				buffer.Recycle();
			}
		}

		public void Send(global::TNet.Buffer buffer, global::System.Net.IPEndPoint ip)
		{
			if (ip.Address.Equals(global::System.Net.IPAddress.Broadcast))
			{
				this.Broadcast(buffer, ip.Port);
				return;
			}
			buffer.MarkAsUsed();
			if (this.mSocket != null)
			{
				buffer.BeginReading();
				global::System.Collections.Generic.Queue<global::TNet.Datagram> obj = this.mOut;
				lock (obj)
				{
					global::TNet.Datagram item = default(global::TNet.Datagram);
					item.buffer = buffer;
					item.ip = ip;
					this.mOut.Enqueue(item);
					if (this.mOut.Count == 1)
					{
						this.mSocket.BeginSendTo(buffer.buffer, buffer.position, buffer.size, global::System.Net.Sockets.SocketFlags.None, ip, new global::System.AsyncCallback(this.OnSend), null);
					}
				}
				return;
			}
			buffer.Recycle();
			throw new global::System.InvalidOperationException("The socket is null. Did you forget to call UdpProtocol.Start()?");
		}

		private void OnSend(global::System.IAsyncResult result)
		{
			if (!this.isActive)
			{
				return;
			}
			int num = 0;
			try
			{
				num = this.mSocket.EndSendTo(result);
			}
			catch (global::System.Exception ex)
			{
				num = 1;
				global::UnityEngine.Debug.Log("[TNet] " + ex.Message);
			}
			global::System.Collections.Generic.Queue<global::TNet.Datagram> obj = this.mOut;
			lock (obj)
			{
				this.mOut.Dequeue().buffer.Recycle();
				if (num > 0 && this.mSocket != null && this.mOut.Count != 0)
				{
					global::TNet.Datagram datagram = this.mOut.Peek();
					this.mSocket.BeginSendTo(datagram.buffer.buffer, datagram.buffer.position, datagram.buffer.size, global::System.Net.Sockets.SocketFlags.None, datagram.ip, new global::System.AsyncCallback(this.OnSend), null);
				}
			}
		}

		public void Error(global::System.Net.IPEndPoint ip, string error)
		{
			global::TNet.Buffer buffer = global::TNet.Buffer.Create();
			buffer.BeginPacket(global::TNet.Packet.Error).Write(error);
			buffer.EndTcpPacketWithOffset(4);
			global::TNet.Datagram item = default(global::TNet.Datagram);
			item.buffer = buffer;
			item.ip = ip;
			global::System.Collections.Generic.Queue<global::TNet.Datagram> obj = this.mIn;
			lock (obj)
			{
				this.mIn.Enqueue(item);
			}
		}

		public static bool useMulticasting = true;

		public static global::System.Net.IPAddress defaultNetworkInterface = null;

		private int mPort = -1;

		private global::System.Net.Sockets.Socket mSocket;

		private bool mMulticast = true;

		private byte[] mTemp = new byte[8192];

		private global::System.Net.EndPoint mEndPoint;

		private static global::System.Net.EndPoint mDefaultEndPoint;

		private static global::System.Net.IPAddress multicastIP = global::System.Net.IPAddress.Parse("224.168.100.17");

		private global::System.Net.IPEndPoint mMulticastEndPoint = new global::System.Net.IPEndPoint(global::TNet.UdpProtocol.multicastIP, 0);

		private global::System.Net.IPEndPoint mBroadcastEndPoint = new global::System.Net.IPEndPoint(global::System.Net.IPAddress.Broadcast, 0);

		protected global::System.Collections.Generic.Queue<global::TNet.Datagram> mIn = new global::System.Collections.Generic.Queue<global::TNet.Datagram>();

		protected global::System.Collections.Generic.Queue<global::TNet.Datagram> mOut = new global::System.Collections.Generic.Queue<global::TNet.Datagram>();
	}
}
