using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TNet
{
	public class GameClient
	{
		public int channelID
		{
			get
			{
				return this.mChannelID;
			}
		}

		public int hostID
		{
			get
			{
				return (!this.mTcp.isConnected) ? this.mTcp.id : this.mHost;
			}
		}

		public bool isConnected
		{
			get
			{
				return this.mTcp.isConnected;
			}
		}

		public bool isTryingToConnect
		{
			get
			{
				return this.mTcp.isTryingToConnect;
			}
		}

		public bool isHosting
		{
			get
			{
				return !this.mIsInChannel || this.mHost == this.mTcp.id;
			}
		}

		public bool isInChannel
		{
			get
			{
				return this.mIsInChannel;
			}
		}

		public int listeningPort
		{
			get
			{
				return this.mUdp.listeningPort;
			}
		}

		public global::System.Net.IPEndPoint packetSource
		{
			get
			{
				return (this.mPacketSource == null) ? this.mTcp.tcpEndPoint : this.mPacketSource;
			}
		}

		public string channelData
		{
			get
			{
				return (!this.isInChannel) ? string.Empty : this.mData;
			}
			set
			{
				if (this.isHosting && this.isInChannel && !this.mData.Equals(value))
				{
					this.mData = value;
					this.BeginSend(global::TNet.Packet.RequestSetChannelData).Write(value);
					this.EndSend();
				}
			}
		}

		public bool noDelay
		{
			get
			{
				return this.mTcp.noDelay;
			}
			set
			{
				if (this.mTcp.noDelay != value)
				{
					this.mTcp.noDelay = value;
					this.BeginSend(global::TNet.Packet.RequestNoDelay).Write(value);
					this.EndSend();
				}
			}
		}

		public int ping
		{
			get
			{
				return (!this.isConnected) ? 0 : this.mPing;
			}
		}

		public bool canUseUDP
		{
			get
			{
				return this.mUdp.isActive && this.mServerUdpEndPoint != null;
			}
		}

		public global::TNet.Player player
		{
			get
			{
				return this.mTcp;
			}
		}

		public int playerID
		{
			get
			{
				return this.mTcp.id;
			}
		}

		public string playerName
		{
			get
			{
				return this.mTcp.name;
			}
			set
			{
				if (this.mTcp.name != value)
				{
					if (this.isConnected)
					{
						global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.RequestSetName);
						binaryWriter.Write(value);
						this.EndSend();
					}
					else
					{
						this.mTcp.name = value;
					}
				}
			}
		}

		public object playerData
		{
			get
			{
				return this.mTcp.data;
			}
			set
			{
				this.mTcp.data = value;
				this.SyncPlayerData();
			}
		}

		public void SyncPlayerData()
		{
			if (this.isConnected)
			{
				global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.SyncPlayerData);
				binaryWriter.Write(this.mTcp.id);
				binaryWriter.WriteObject(this.mTcp.data);
				this.EndSend();
			}
		}

		public global::TNet.Player GetPlayer(int id)
		{
			if (id == this.mTcp.id)
			{
				return this.mTcp;
			}
			if (this.isConnected)
			{
				global::TNet.Player result = null;
				this.mDictionary.TryGetValue(id, out result);
				return result;
			}
			return null;
		}

		public global::System.IO.BinaryWriter BeginSend(global::TNet.Packet type)
		{
			global::TNet.GameClient.mBuffer = global::TNet.Buffer.Create();
			return global::TNet.GameClient.mBuffer.BeginPacket(type);
		}

		public global::System.IO.BinaryWriter BeginSend(byte packetID)
		{
			global::TNet.GameClient.mBuffer = global::TNet.Buffer.Create();
			return global::TNet.GameClient.mBuffer.BeginPacket(packetID);
		}

		public void EndSend()
		{
			if (global::TNet.GameClient.mBuffer != null)
			{
				global::TNet.GameClient.mBuffer.EndPacket();
				this.mTcp.SendTcpPacket(global::TNet.GameClient.mBuffer);
				global::TNet.GameClient.mBuffer.Recycle();
				global::TNet.GameClient.mBuffer = null;
			}
		}

		public void EndSend(bool reliable)
		{
			global::TNet.GameClient.mBuffer.EndPacket();
			if (reliable || !this.mUdpIsUsable || this.mServerUdpEndPoint == null || !this.mUdp.isActive)
			{
				this.mTcp.SendTcpPacket(global::TNet.GameClient.mBuffer);
			}
			else
			{
				this.mUdp.Send(global::TNet.GameClient.mBuffer, this.mServerUdpEndPoint);
			}
			global::TNet.GameClient.mBuffer.Recycle();
			global::TNet.GameClient.mBuffer = null;
		}

		public void EndSend(int port)
		{
			global::TNet.GameClient.mBuffer.EndPacket();
			this.mUdp.Broadcast(global::TNet.GameClient.mBuffer, port);
			global::TNet.GameClient.mBuffer.Recycle();
			global::TNet.GameClient.mBuffer = null;
		}

		public void EndSend(global::System.Net.IPEndPoint target)
		{
			global::TNet.GameClient.mBuffer.EndPacket();
			this.mUdp.Send(global::TNet.GameClient.mBuffer, target);
			global::TNet.GameClient.mBuffer.Recycle();
			global::TNet.GameClient.mBuffer = null;
		}

		public void Connect(global::System.Net.IPEndPoint externalIP, global::System.Net.IPEndPoint internalIP)
		{
			this.Disconnect();
			this.mTcp.Connect(externalIP, internalIP);
		}

		public void Disconnect()
		{
			this.mTcp.Disconnect();
		}

		public bool StartUDP(int udpPort)
		{
			if (this.mUdp.Start(udpPort))
			{
				if (this.isConnected)
				{
					this.BeginSend(global::TNet.Packet.RequestSetUDP).Write((ushort)udpPort);
					this.EndSend();
				}
				return true;
			}
			return false;
		}

		public void StopUDP()
		{
			if (this.mUdp.isActive)
			{
				if (this.isConnected)
				{
					this.BeginSend(global::TNet.Packet.RequestSetUDP).Write(0);
					this.EndSend();
				}
				this.mUdp.Stop();
				this.mUdpIsUsable = false;
			}
		}

		public void JoinChannel(int channelID, string levelName, bool persistent, int playerLimit, string password)
		{
			if (this.isConnected)
			{
				global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.RequestJoinChannel);
				binaryWriter.Write(channelID);
				binaryWriter.Write((!string.IsNullOrEmpty(password)) ? password : string.Empty);
				binaryWriter.Write((!string.IsNullOrEmpty(levelName)) ? levelName : string.Empty);
				binaryWriter.Write(persistent);
				binaryWriter.Write((ushort)playerLimit);
				this.EndSend();
			}
		}

		public void CloseChannel()
		{
			if (this.isConnected && this.isInChannel)
			{
				this.BeginSend(global::TNet.Packet.RequestCloseChannel);
				this.EndSend();
			}
		}

		public void LeaveChannel()
		{
			if (this.isConnected && this.isInChannel)
			{
				this.BeginSend(global::TNet.Packet.RequestLeaveChannel);
				this.EndSend();
			}
		}

		public void SetPlayerLimit(int max)
		{
			if (this.isConnected && this.isInChannel)
			{
				this.BeginSend(global::TNet.Packet.RequestSetPlayerLimit).Write((ushort)max);
				this.EndSend();
			}
		}

		public void LoadLevel(string levelName)
		{
			if (this.isConnected && this.isInChannel)
			{
				this.BeginSend(global::TNet.Packet.RequestLoadLevel).Write(levelName);
				this.EndSend();
			}
		}

		public void SetHost(global::TNet.Player player)
		{
			if (this.isConnected && this.isInChannel && this.isHosting)
			{
				global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.RequestSetHost);
				binaryWriter.Write(player.id);
				this.EndSend();
			}
		}

		public void SetTimeout(int seconds)
		{
			if (this.isConnected)
			{
				this.BeginSend(global::TNet.Packet.RequestSetTimeout).Write(seconds);
				this.EndSend();
			}
		}

		public void Ping(global::System.Net.IPEndPoint udpEndPoint, global::TNet.GameClient.OnPing callback)
		{
			this.onPing = callback;
			this.mPingTime = global::System.DateTime.Now.Ticks / 10000L;
			this.BeginSend(global::TNet.Packet.RequestPing);
			this.EndSend(udpEndPoint);
		}

		public void LoadFile(string filename, global::TNet.GameClient.OnLoadFile callback)
		{
			this.mLoadFiles.Add(callback);
			global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.RequestLoadFile);
			binaryWriter.Write(filename);
			this.EndSend();
		}

		public void SaveFile(string filename, byte[] data)
		{
			global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.RequestSaveFile);
			binaryWriter.Write(filename);
			binaryWriter.Write(data.Length);
			binaryWriter.Write(data);
			this.EndSend();
		}

		public void ProcessPackets()
		{
			this.mTime = global::System.DateTime.Now.Ticks / 10000L;
			if (this.mTcp.isConnected && this.mCanPing && this.mPingTime + 4000L < this.mTime)
			{
				this.mCanPing = false;
				this.mPingTime = this.mTime;
				this.BeginSend(global::TNet.Packet.RequestPing);
				this.EndSend();
			}
			global::TNet.Buffer buffer = null;
			bool flag = true;
			global::System.Net.IPEndPoint ip = null;
			while (flag && this.isActive && this.mUdp.ReceivePacket(out buffer, out ip))
			{
				this.mUdpIsUsable = true;
				flag = this.ProcessPacket(buffer, ip);
				buffer.Recycle();
			}
			while (flag && this.isActive && this.mTcp.ReceivePacket(out buffer))
			{
				flag = this.ProcessPacket(buffer, null);
				buffer.Recycle();
			}
		}

		private bool ProcessPacket(global::TNet.Buffer buffer, global::System.Net.IPEndPoint ip)
		{
			this.mPacketSource = ip;
			global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
			if (buffer.size == 0)
			{
				return true;
			}
			int num = (int)binaryReader.ReadByte();
			global::TNet.Packet packet = (global::TNet.Packet)num;
			if (packet == global::TNet.Packet.ResponseID || this.mTcp.stage == global::TNet.TcpProtocol.Stage.Verifying)
			{
				if (this.mTcp.VerifyResponseID(packet, binaryReader))
				{
					if (this.mUdp.isActive)
					{
						this.BeginSend(global::TNet.Packet.RequestSetUDP).Write((ushort)this.mUdp.listeningPort);
						this.EndSend();
					}
					this.mCanPing = true;
					if (this.onConnect != null)
					{
						this.onConnect(true, null);
					}
				}
				return true;
			}
			global::TNet.GameClient.OnPacket onPacket;
			if (this.packetHandlers.TryGetValue((byte)packet, out onPacket) && onPacket != null)
			{
				onPacket(packet, binaryReader, ip);
				return true;
			}
			global::TNet.Packet packet2 = packet;
			switch (packet2)
			{
			case global::TNet.Packet.ResponsePing:
			{
				int milliSeconds = (int)(this.mTime - this.mPingTime);
				if (ip != null)
				{
					if (this.onPing != null && ip != null)
					{
						this.onPing(ip, milliSeconds);
					}
				}
				else
				{
					this.mCanPing = true;
					this.mPing = milliSeconds;
				}
				break;
			}
			case global::TNet.Packet.ResponseSetUDP:
			{
				ushort num2 = binaryReader.ReadUInt16();
				if (num2 != 0)
				{
					global::System.Net.IPAddress address = new global::System.Net.IPAddress(this.mTcp.tcpEndPoint.Address.GetAddressBytes());
					this.mServerUdpEndPoint = new global::System.Net.IPEndPoint(address, (int)num2);
					if (this.mUdp.isActive)
					{
						global::TNet.GameClient.mBuffer = global::TNet.Buffer.Create();
						global::TNet.GameClient.mBuffer.BeginPacket(global::TNet.Packet.RequestActivateUDP).Write(this.playerID);
						global::TNet.GameClient.mBuffer.EndPacket();
						this.mUdp.Send(global::TNet.GameClient.mBuffer, this.mServerUdpEndPoint);
						global::TNet.GameClient.mBuffer.Recycle();
						global::TNet.GameClient.mBuffer = null;
					}
				}
				else
				{
					this.mServerUdpEndPoint = null;
				}
				break;
			}
			case global::TNet.Packet.ResponsePlayerLeft:
			{
				global::TNet.Player player = this.GetPlayer(binaryReader.ReadInt32());
				if (player != null)
				{
					this.mDictionary.Remove(player.id);
				}
				this.players.Remove(player);
				if (this.onPlayerLeft != null)
				{
					this.onPlayerLeft(player);
				}
				break;
			}
			case global::TNet.Packet.ResponsePlayerJoined:
			{
				global::TNet.Player player2 = new global::TNet.Player();
				player2.id = binaryReader.ReadInt32();
				player2.name = binaryReader.ReadString();
				player2.data = binaryReader.ReadObject();
				this.mDictionary.Add(player2.id, player2);
				this.players.Add(player2);
				if (this.onPlayerJoined != null)
				{
					this.onPlayerJoined(player2);
				}
				break;
			}
			case global::TNet.Packet.ResponseJoiningChannel:
			{
				this.mIsInChannel = true;
				this.mDictionary.Clear();
				this.players.Clear();
				this.mChannelID = binaryReader.ReadInt32();
				int num3 = (int)binaryReader.ReadInt16();
				for (int i = 0; i < num3; i++)
				{
					global::TNet.Player player3 = new global::TNet.Player();
					player3.id = binaryReader.ReadInt32();
					player3.name = binaryReader.ReadString();
					player3.data = binaryReader.ReadObject();
					this.mDictionary.Add(player3.id, player3);
					this.players.Add(player3);
				}
				break;
			}
			case global::TNet.Packet.ResponseJoinChannel:
				this.mIsInChannel = binaryReader.ReadBoolean();
				if (this.onJoinChannel != null)
				{
					this.onJoinChannel(this.mIsInChannel, (!this.mIsInChannel) ? binaryReader.ReadString() : null);
				}
				break;
			case global::TNet.Packet.ResponseLeaveChannel:
				this.mData = string.Empty;
				this.mChannelID = 0;
				this.mIsInChannel = false;
				this.mDictionary.Clear();
				this.players.Clear();
				if (this.onLeftChannel != null)
				{
					this.onLeftChannel();
				}
				break;
			case global::TNet.Packet.ResponseRenamePlayer:
			{
				global::TNet.Player player4 = this.GetPlayer(binaryReader.ReadInt32());
				string name = player4.name;
				if (player4 != null)
				{
					player4.name = binaryReader.ReadString();
				}
				if (this.onRenamePlayer != null)
				{
					this.onRenamePlayer(player4, name);
				}
				break;
			}
			case global::TNet.Packet.ResponseSetHost:
				this.mHost = binaryReader.ReadInt32();
				if (this.onSetHost != null)
				{
					this.onSetHost(this.isHosting);
				}
				break;
			case global::TNet.Packet.ResponseLoadLevel:
				if (this.onLoadLevel != null)
				{
					this.onLoadLevel(binaryReader.ReadString());
				}
				return false;
			case global::TNet.Packet.ResponseCreate:
				if (this.onCreate != null)
				{
					int creator = binaryReader.ReadInt32();
					ushort index = binaryReader.ReadUInt16();
					uint objID = binaryReader.ReadUInt32();
					this.onCreate(creator, (int)index, objID, binaryReader);
				}
				break;
			case global::TNet.Packet.ResponseDestroy:
				if (this.onDestroy != null)
				{
					int num4 = (int)binaryReader.ReadUInt16();
					for (int j = 0; j < num4; j++)
					{
						this.onDestroy(binaryReader.ReadUInt32());
					}
				}
				break;
			case global::TNet.Packet.ResponseLoadFile:
			{
				string filename = binaryReader.ReadString();
				int count = binaryReader.ReadInt32();
				byte[] data = binaryReader.ReadBytes(count);
				global::TNet.GameClient.OnLoadFile onLoadFile = this.mLoadFiles.Pop();
				if (onLoadFile != null)
				{
					onLoadFile(filename, data);
				}
				break;
			}
			case global::TNet.Packet.ResponseSetChannelData:
				this.mData = binaryReader.ReadString();
				if (this.onSetChannelData != null)
				{
					this.onSetChannelData(this.mData);
				}
				break;
			default:
				switch (packet2)
				{
				case global::TNet.Packet.Error:
					if (this.mTcp.stage != global::TNet.TcpProtocol.Stage.Connected && this.onConnect != null)
					{
						this.onConnect(false, binaryReader.ReadString());
					}
					else if (this.onError != null)
					{
						this.onError(binaryReader.ReadString());
					}
					break;
				case global::TNet.Packet.Disconnect:
					this.mData = string.Empty;
					if (this.isInChannel && this.onLeftChannel != null)
					{
						this.onLeftChannel();
					}
					this.players.Clear();
					this.mDictionary.Clear();
					this.mTcp.Close(false);
					this.mLoadFiles.Clear();
					if (this.onDisconnect != null)
					{
						this.onDisconnect();
					}
					break;
				}
				break;
			case global::TNet.Packet.ForwardToAll:
			case global::TNet.Packet.ForwardToAllSaved:
			case global::TNet.Packet.ForwardToOthers:
			case global::TNet.Packet.ForwardToOthersSaved:
			case global::TNet.Packet.ForwardToHost:
			case global::TNet.Packet.Broadcast:
				if (this.onForwardedPacket != null)
				{
					this.onForwardedPacket(binaryReader);
				}
				break;
			case global::TNet.Packet.ForwardToPlayer:
				binaryReader.ReadInt32();
				if (this.onForwardedPacket != null)
				{
					this.onForwardedPacket(binaryReader);
				}
				break;
			case global::TNet.Packet.SyncPlayerData:
			{
				global::TNet.Player player5 = this.GetPlayer(binaryReader.ReadInt32());
				if (player5 != null)
				{
					player5.data = binaryReader.ReadObject();
					if (this.onPlayerSync != null)
					{
						this.onPlayerSync(player5);
					}
				}
				break;
			}
			}
			return true;
		}

		public global::System.Collections.Generic.Dictionary<byte, global::TNet.GameClient.OnPacket> packetHandlers = new global::System.Collections.Generic.Dictionary<byte, global::TNet.GameClient.OnPacket>();

		public global::TNet.GameClient.OnPing onPing;

		public global::TNet.GameClient.OnError onError;

		public global::TNet.GameClient.OnConnect onConnect;

		public global::TNet.GameClient.OnDisconnect onDisconnect;

		public global::TNet.GameClient.OnJoinChannel onJoinChannel;

		public global::TNet.GameClient.OnLeftChannel onLeftChannel;

		public global::TNet.GameClient.OnLoadLevel onLoadLevel;

		public global::TNet.GameClient.OnPlayerJoined onPlayerJoined;

		public global::TNet.GameClient.OnPlayerLeft onPlayerLeft;

		public global::TNet.GameClient.OnPlayerSync onPlayerSync;

		public global::TNet.GameClient.OnRenamePlayer onRenamePlayer;

		public global::TNet.GameClient.OnSetHost onSetHost;

		public global::TNet.GameClient.OnSetChannelData onSetChannelData;

		public global::TNet.GameClient.OnCreate onCreate;

		public global::TNet.GameClient.OnDestroy onDestroy;

		public global::TNet.GameClient.OnForwardedPacket onForwardedPacket;

		public global::TNet.List<global::TNet.Player> players = new global::TNet.List<global::TNet.Player>();

		public bool isActive = true;

		private global::System.Collections.Generic.Dictionary<int, global::TNet.Player> mDictionary = new global::System.Collections.Generic.Dictionary<int, global::TNet.Player>();

		private global::TNet.TcpProtocol mTcp = new global::TNet.TcpProtocol();

		private global::TNet.UdpProtocol mUdp = new global::TNet.UdpProtocol();

		private bool mUdpIsUsable;

		private int mHost;

		private int mChannelID;

		private long mTime;

		private long mPingTime;

		private int mPing;

		private bool mCanPing;

		private bool mIsInChannel;

		private string mData = string.Empty;

		private global::TNet.List<global::TNet.GameClient.OnLoadFile> mLoadFiles = new global::TNet.List<global::TNet.GameClient.OnLoadFile>();

		private global::System.Net.IPEndPoint mServerUdpEndPoint;

		private global::System.Net.IPEndPoint mPacketSource;

		private static global::TNet.Buffer mBuffer;

		public delegate void OnPing(global::System.Net.IPEndPoint ip, int milliSeconds);

		public delegate void OnError(string message);

		public delegate void OnConnect(bool success, string message);

		public delegate void OnDisconnect();

		public delegate void OnJoinChannel(bool success, string message);

		public delegate void OnLeftChannel();

		public delegate void OnLoadLevel(string levelName);

		public delegate void OnPlayerJoined(global::TNet.Player p);

		public delegate void OnPlayerLeft(global::TNet.Player p);

		public delegate void OnPlayerSync(global::TNet.Player p);

		public delegate void OnRenamePlayer(global::TNet.Player p, string previous);

		public delegate void OnSetHost(bool hosting);

		public delegate void OnSetChannelData(string data);

		public delegate void OnCreate(int creator, int index, uint objID, global::System.IO.BinaryReader reader);

		public delegate void OnDestroy(uint objID);

		public delegate void OnForwardedPacket(global::System.IO.BinaryReader reader);

		public delegate void OnPacket(global::TNet.Packet response, global::System.IO.BinaryReader reader, global::System.Net.IPEndPoint source);

		public delegate void OnLoadFile(string filename, byte[] data);
	}
}
