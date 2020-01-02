using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TNet
{
	public class TcpProtocol : global::TNet.Player
	{
		public bool isConnected
		{
			get
			{
				return this.stage == global::TNet.TcpProtocol.Stage.Connected;
			}
		}

		public bool isTryingToConnect
		{
			get
			{
				return this.mConnecting.size != 0;
			}
		}

		public bool noDelay
		{
			get
			{
				return this.mNoDelay;
			}
			set
			{
				if (this.mNoDelay != value)
				{
					this.mNoDelay = value;
					this.mSocket.SetSocketOption(global::System.Net.Sockets.SocketOptionLevel.Tcp, global::System.Net.Sockets.SocketOptionName.Debug, this.mNoDelay);
				}
			}
		}

		public string address
		{
			get
			{
				return (this.tcpEndPoint == null) ? "0.0.0.0:0" : this.tcpEndPoint.ToString();
			}
		}

		public void Connect(global::System.Net.IPEndPoint externalIP)
		{
			this.Connect(externalIP, null);
		}

		public void Connect(global::System.Net.IPEndPoint externalIP, global::System.Net.IPEndPoint internalIP)
		{
			this.Disconnect();
			if (internalIP != null && global::TNet.Tools.GetSubnet(global::TNet.Tools.localAddress) == global::TNet.Tools.GetSubnet(internalIP.Address))
			{
				this.tcpEndPoint = internalIP;
				this.mFallback = externalIP;
			}
			else
			{
				this.tcpEndPoint = externalIP;
				this.mFallback = internalIP;
			}
			this.ConnectToTcpEndPoint();
		}

		private bool ConnectToTcpEndPoint()
		{
			if (this.tcpEndPoint != null)
			{
				this.stage = global::TNet.TcpProtocol.Stage.Connecting;
				try
				{
					global::TNet.List<global::System.Net.Sockets.Socket> obj = this.mConnecting;
					lock (obj)
					{
						this.mSocket = new global::System.Net.Sockets.Socket(global::System.Net.Sockets.AddressFamily.InterNetwork, global::System.Net.Sockets.SocketType.Stream, global::System.Net.Sockets.ProtocolType.Tcp);
						this.mConnecting.Add(this.mSocket);
					}
					global::System.IAsyncResult parameter = this.mSocket.BeginConnect(this.tcpEndPoint, new global::System.AsyncCallback(this.OnConnectResult), this.mSocket);
					global::System.Threading.Thread thread = new global::System.Threading.Thread(new global::System.Threading.ParameterizedThreadStart(this.CancelConnect));
					thread.Start(parameter);
					return true;
				}
				catch (global::System.Exception ex)
				{
					this.Error(ex.Message);
				}
				return false;
			}
			this.Error("Unable to resolve the specified address");
			return false;
		}

		private bool ConnectToFallback()
		{
			this.tcpEndPoint = this.mFallback;
			this.mFallback = null;
			return this.tcpEndPoint != null && this.ConnectToTcpEndPoint();
		}

		private void CancelConnect(object obj)
		{
			global::System.IAsyncResult asyncResult = (global::System.IAsyncResult)obj;
			if (asyncResult != null && !asyncResult.AsyncWaitHandle.WaitOne(3000, true))
			{
				try
				{
					global::System.Net.Sockets.Socket socket = (global::System.Net.Sockets.Socket)asyncResult.AsyncState;
					if (socket != null)
					{
						socket.Close();
						global::TNet.List<global::System.Net.Sockets.Socket> obj2 = this.mConnecting;
						lock (obj2)
						{
							if (this.mConnecting.size > 0 && this.mConnecting[this.mConnecting.size - 1] == socket)
							{
								this.mSocket = null;
								if (!this.ConnectToFallback())
								{
									this.Error("Unable to connect");
									this.Close(false);
								}
							}
							this.mConnecting.Remove(socket);
						}
					}
				}
				catch (global::System.Exception)
				{
				}
			}
		}

		private void OnConnectResult(global::System.IAsyncResult result)
		{
			global::System.Net.Sockets.Socket socket = (global::System.Net.Sockets.Socket)result.AsyncState;
			if (socket == null)
			{
				return;
			}
			if (this.mSocket != null && socket == this.mSocket)
			{
				bool flag = true;
				string error = "Failed to connect";
				try
				{
					socket.EndConnect(result);
				}
				catch (global::System.Exception ex)
				{
					if (socket == this.mSocket)
					{
						this.mSocket = null;
					}
					socket.Close();
					error = ex.Message;
					flag = false;
				}
				if (flag)
				{
					this.stage = global::TNet.TcpProtocol.Stage.Verifying;
					global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.RequestID);
					binaryWriter.Write(11);
					binaryWriter.Write((!string.IsNullOrEmpty(this.name)) ? this.name : "Guest");
					binaryWriter.WriteObject(this.data);
					this.EndSend();
					this.StartReceiving();
				}
				else if (!this.ConnectToFallback())
				{
					this.Error(error);
					this.Close(false);
				}
			}
			global::TNet.List<global::System.Net.Sockets.Socket> obj = this.mConnecting;
			lock (obj)
			{
				this.mConnecting.Remove(socket);
			}
		}

		public void Disconnect()
		{
			this.Disconnect(false);
		}

		public void Disconnect(bool notify)
		{
			if (!this.isConnected)
			{
				return;
			}
			try
			{
				global::TNet.List<global::System.Net.Sockets.Socket> obj = this.mConnecting;
				lock (obj)
				{
					int i = this.mConnecting.size;
					while (i > 0)
					{
						global::System.Net.Sockets.Socket socket = this.mConnecting[--i];
						this.mConnecting.RemoveAt(i);
						if (socket != null)
						{
							socket.Close();
						}
					}
				}
				if (this.mSocket != null)
				{
					this.Close(notify || this.mSocket.Connected);
				}
			}
			catch (global::System.Exception)
			{
				global::TNet.List<global::System.Net.Sockets.Socket> obj2 = this.mConnecting;
				lock (obj2)
				{
					this.mConnecting.Clear();
				}
				this.mSocket = null;
			}
		}

		public void Close(bool notify)
		{
			this.stage = global::TNet.TcpProtocol.Stage.NotConnected;
			this.name = "Guest";
			this.data = null;
			if (this.mReceiveBuffer != null)
			{
				this.mReceiveBuffer.Recycle();
				this.mReceiveBuffer = null;
			}
			if (this.mSocket != null)
			{
				try
				{
					if (this.mSocket.Connected)
					{
						this.mSocket.Shutdown(global::System.Net.Sockets.SocketShutdown.Both);
					}
					this.mSocket.Close();
				}
				catch (global::System.Exception)
				{
				}
				this.mSocket = null;
				if (notify)
				{
					global::TNet.Buffer buffer = global::TNet.Buffer.Create();
					buffer.BeginPacket(global::TNet.Packet.Disconnect);
					buffer.EndTcpPacketWithOffset(4);
					global::System.Collections.Generic.Queue<global::TNet.Buffer> obj = this.mIn;
					lock (obj)
					{
						this.mIn.Enqueue(buffer);
					}
				}
			}
		}

		public void Release()
		{
			this.Close(false);
			global::TNet.Buffer.Recycle(this.mIn);
			global::TNet.Buffer.Recycle(this.mOut);
		}

		public global::System.IO.BinaryWriter BeginSend(global::TNet.Packet type)
		{
			global::TNet.TcpProtocol.mBuffer = global::TNet.Buffer.Create(false);
			return global::TNet.TcpProtocol.mBuffer.BeginPacket(type);
		}

		public global::System.IO.BinaryWriter BeginSend(byte packetID)
		{
			global::TNet.TcpProtocol.mBuffer = global::TNet.Buffer.Create(false);
			return global::TNet.TcpProtocol.mBuffer.BeginPacket(packetID);
		}

		public void EndSend()
		{
			global::TNet.TcpProtocol.mBuffer.EndPacket();
			this.SendTcpPacket(global::TNet.TcpProtocol.mBuffer);
			global::TNet.TcpProtocol.mBuffer = null;
		}

		public void SendTcpPacket(global::TNet.Buffer buffer)
		{
			buffer.MarkAsUsed();
			if (this.mSocket != null && this.mSocket.Connected)
			{
				buffer.BeginReading();
				global::System.Collections.Generic.Queue<global::TNet.Buffer> obj = this.mOut;
				lock (obj)
				{
					this.mOut.Enqueue(buffer);
					if (this.mOut.Count == 1)
					{
						try
						{
							this.mSocket.BeginSend(buffer.buffer, buffer.position, buffer.size, global::System.Net.Sockets.SocketFlags.None, new global::System.AsyncCallback(this.OnSend), buffer);
						}
						catch (global::System.Exception ex)
						{
							this.Error(ex.Message);
							this.Close(false);
							this.Release();
						}
					}
				}
			}
			else
			{
				buffer.Recycle();
			}
		}

		private void OnSend(global::System.IAsyncResult result)
		{
			if (this.stage == global::TNet.TcpProtocol.Stage.NotConnected)
			{
				return;
			}
			int num;
			try
			{
				num = this.mSocket.EndSend(result);
			}
			catch (global::System.Exception ex)
			{
				num = 0;
				this.Close(true);
				this.Error(ex.Message);
				return;
			}
			global::System.Collections.Generic.Queue<global::TNet.Buffer> obj = this.mOut;
			lock (obj)
			{
				this.mOut.Dequeue().Recycle();
				if (num > 0 && this.mSocket != null && this.mSocket.Connected)
				{
					global::TNet.Buffer buffer = (this.mOut.Count != 0) ? this.mOut.Peek() : null;
					if (buffer != null)
					{
						try
						{
							this.mSocket.BeginSend(buffer.buffer, buffer.position, buffer.size, global::System.Net.Sockets.SocketFlags.None, new global::System.AsyncCallback(this.OnSend), buffer);
						}
						catch (global::System.Exception ex2)
						{
							this.Error(ex2.Message);
							this.Close(false);
						}
					}
				}
				else
				{
					this.Close(true);
				}
			}
		}

		public void StartReceiving()
		{
			this.StartReceiving(null);
		}

		public void StartReceiving(global::System.Net.Sockets.Socket socket)
		{
			if (socket != null)
			{
				this.Close(false);
				this.mSocket = socket;
			}
			if (this.mSocket != null && this.mSocket.Connected)
			{
				this.stage = global::TNet.TcpProtocol.Stage.Verifying;
				this.lastReceivedTime = global::System.DateTime.Now.Ticks / 10000L;
				this.tcpEndPoint = (global::System.Net.IPEndPoint)this.mSocket.RemoteEndPoint;
				try
				{
					this.mSocket.BeginReceive(this.mTemp, 0, this.mTemp.Length, global::System.Net.Sockets.SocketFlags.None, new global::System.AsyncCallback(this.OnReceive), null);
				}
				catch (global::System.Exception ex)
				{
					this.Error(ex.Message);
					this.Disconnect(true);
				}
			}
		}

		public bool ReceivePacket(out global::TNet.Buffer buffer)
		{
			if (this.mIn.Count != 0)
			{
				global::System.Collections.Generic.Queue<global::TNet.Buffer> obj = this.mIn;
				lock (obj)
				{
					buffer = this.mIn.Dequeue();
					return true;
				}
			}
			buffer = null;
			return false;
		}

		private void OnReceive(global::System.IAsyncResult result)
		{
			if (this.stage == global::TNet.TcpProtocol.Stage.NotConnected)
			{
				return;
			}
			int num = 0;
			try
			{
				num = this.mSocket.EndReceive(result);
			}
			catch (global::System.Exception ex)
			{
				this.Error(ex.Message);
				this.Disconnect(true);
				return;
			}
			this.lastReceivedTime = global::System.DateTime.Now.Ticks / 10000L;
			if (num == 0)
			{
				this.Close(true);
			}
			else if (this.ProcessBuffer(num))
			{
				if (this.stage == global::TNet.TcpProtocol.Stage.NotConnected)
				{
					return;
				}
				try
				{
					this.mSocket.BeginReceive(this.mTemp, 0, this.mTemp.Length, global::System.Net.Sockets.SocketFlags.None, new global::System.AsyncCallback(this.OnReceive), null);
				}
				catch (global::System.Exception ex2)
				{
					this.Error(ex2.Message);
					this.Close(false);
				}
			}
			else
			{
				this.Close(true);
			}
		}

		private bool ProcessBuffer(int bytes)
		{
			if (this.mReceiveBuffer == null)
			{
				this.mReceiveBuffer = global::TNet.Buffer.Create();
				this.mReceiveBuffer.BeginWriting(false).Write(this.mTemp, 0, bytes);
			}
			else
			{
				this.mReceiveBuffer.BeginWriting(true).Write(this.mTemp, 0, bytes);
			}
			int i = this.mReceiveBuffer.size - this.mOffset;
			while (i >= 4)
			{
				if (this.mExpected == 0)
				{
					this.mExpected = this.mReceiveBuffer.PeekInt(this.mOffset);
					if (this.mExpected < 0 || this.mExpected > 16777216)
					{
						this.Close(true);
						return false;
					}
				}
				i -= 4;
				if (i == this.mExpected)
				{
					this.mReceiveBuffer.BeginReading(this.mOffset + 4);
					global::System.Collections.Generic.Queue<global::TNet.Buffer> obj = this.mIn;
					lock (obj)
					{
						this.mIn.Enqueue(this.mReceiveBuffer);
					}
					this.mReceiveBuffer = null;
					this.mExpected = 0;
					this.mOffset = 0;
					break;
				}
				if (i <= this.mExpected)
				{
					break;
				}
				int num = this.mExpected + 4;
				global::TNet.Buffer buffer = global::TNet.Buffer.Create();
				buffer.BeginWriting(false).Write(this.mReceiveBuffer.buffer, this.mOffset, num);
				buffer.BeginReading(4);
				global::System.Collections.Generic.Queue<global::TNet.Buffer> obj2 = this.mIn;
				lock (obj2)
				{
					this.mIn.Enqueue(buffer);
				}
				i -= this.mExpected;
				this.mOffset += num;
				this.mExpected = 0;
			}
			return true;
		}

		public void Error(string error)
		{
			this.Error(global::TNet.Buffer.Create(), error);
		}

		private void Error(global::TNet.Buffer buffer, string error)
		{
			buffer.BeginPacket(global::TNet.Packet.Error).Write(error);
			buffer.EndTcpPacketWithOffset(4);
			global::System.Collections.Generic.Queue<global::TNet.Buffer> obj = this.mIn;
			lock (obj)
			{
				this.mIn.Enqueue(buffer);
			}
		}

		public bool VerifyRequestID(global::TNet.Buffer buffer, bool uniqueID)
		{
			global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
			global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
			if (packet == global::TNet.Packet.RequestID)
			{
				if (binaryReader.ReadInt32() == 11)
				{
					this.id = ((!uniqueID) ? 0 : global::System.Threading.Interlocked.Increment(ref global::TNet.Player.mPlayerCounter));
					this.name = binaryReader.ReadString();
					if (buffer.size > 1)
					{
						this.data = binaryReader.ReadObject();
					}
					else
					{
						this.data = null;
					}
					this.stage = global::TNet.TcpProtocol.Stage.Connected;
					global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.ResponseID);
					binaryWriter.Write(11);
					binaryWriter.Write(this.id);
					this.EndSend();
					return true;
				}
				global::System.IO.BinaryWriter binaryWriter2 = this.BeginSend(global::TNet.Packet.ResponseID);
				binaryWriter2.Write(11);
				binaryWriter2.Write(0);
				this.EndSend();
				this.Close(false);
			}
			return false;
		}

		public bool VerifyResponseID(global::TNet.Packet packet, global::System.IO.BinaryReader reader)
		{
			if (packet != global::TNet.Packet.ResponseID)
			{
				this.Error("Expected a response ID, got " + packet);
				this.Close(false);
				return false;
			}
			int num = reader.ReadInt32();
			if (num == 11)
			{
				this.id = reader.ReadInt32();
				this.stage = global::TNet.TcpProtocol.Stage.Connected;
				return true;
			}
			this.id = 0;
			this.Error(string.Concat(new object[]
			{
				"Version mismatch! Server is running protocol version ",
				num,
				" while you are on version ",
				11
			}));
			this.Close(false);
			return false;
		}

		public global::TNet.TcpProtocol.Stage stage;

		public global::System.Net.IPEndPoint tcpEndPoint;

		public long lastReceivedTime;

		public long timeoutTime = 10000L;

		private global::System.Collections.Generic.Queue<global::TNet.Buffer> mIn = new global::System.Collections.Generic.Queue<global::TNet.Buffer>();

		private global::System.Collections.Generic.Queue<global::TNet.Buffer> mOut = new global::System.Collections.Generic.Queue<global::TNet.Buffer>();

		private byte[] mTemp = new byte[8192];

		private global::TNet.Buffer mReceiveBuffer;

		private int mExpected;

		private int mOffset;

		private global::System.Net.Sockets.Socket mSocket;

		private bool mNoDelay;

		private global::System.Net.IPEndPoint mFallback;

		private global::TNet.List<global::System.Net.Sockets.Socket> mConnecting = new global::TNet.List<global::System.Net.Sockets.Socket>();

		private static global::TNet.Buffer mBuffer;

		public enum Stage
		{
			NotConnected,
			Connecting,
			Verifying,
			Connected
		}
	}
}
