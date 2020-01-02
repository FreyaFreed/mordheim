using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TNet
{
	public class GameServer : global::TNet.FileServer
	{
		public bool isActive
		{
			get
			{
				return this.mThread != null;
			}
		}

		public bool isListening
		{
			get
			{
				return this.mListener != null;
			}
		}

		public int tcpPort
		{
			get
			{
				return (this.mListener == null) ? 0 : this.mListenerPort;
			}
		}

		public int udpPort
		{
			get
			{
				return this.mUdp.listeningPort;
			}
		}

		public int playerCount
		{
			get
			{
				return (!this.isActive) ? 0 : this.mPlayers.size;
			}
		}

		public bool Start(int tcpPort)
		{
			return this.Start(tcpPort, 0);
		}

		public bool Start(int tcpPort, int udpPort)
		{
			this.Stop();
			try
			{
				this.mListenerPort = tcpPort;
				this.mListener = new global::System.Net.Sockets.TcpListener(global::System.Net.IPAddress.Any, tcpPort);
				this.mListener.Start(50);
			}
			catch (global::System.Exception ex)
			{
				base.Error(ex.Message);
				return false;
			}
			if (!this.mUdp.Start(udpPort))
			{
				base.Error("Unable to listen to UDP port " + udpPort);
				this.Stop();
				return false;
			}
			this.mAllowUdp = (udpPort > 0);
			if (this.lobbyLink != null)
			{
				this.lobbyLink.Start();
				this.lobbyLink.SendUpdate(this);
			}
			this.mThread = new global::System.Threading.Thread(new global::System.Threading.ThreadStart(this.ThreadFunction));
			this.mThread.Start();
			return true;
		}

		public void Stop()
		{
			if (this.lobbyLink != null)
			{
				this.lobbyLink.Stop();
			}
			this.mAllowUdp = false;
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
			this.mUdp.Stop();
			int i = this.mPlayers.size;
			while (i > 0)
			{
				this.RemovePlayer(this.mPlayers[--i]);
			}
			this.mChannels.Clear();
		}

		public void MakePrivate()
		{
			this.mListenerPort = 0;
		}

		private void ThreadFunction()
		{
			for (;;)
			{
				bool flag = false;
				this.mTime = global::System.DateTime.Now.Ticks / 10000L;
				if (this.mListenerPort == 0)
				{
					if (this.mListener != null)
					{
						this.mListener.Stop();
						this.mListener = null;
						if (this.lobbyLink != null)
						{
							this.lobbyLink.Stop();
						}
						if (this.onShutdown != null)
						{
							this.onShutdown();
						}
					}
				}
				else
				{
					while (this.mListener != null && this.mListener.Pending())
					{
						this.AddPlayer(this.mListener.AcceptSocket());
					}
				}
				global::TNet.Buffer buffer;
				global::System.Net.IPEndPoint ipendPoint;
				while (this.mUdp.listeningPort != 0 && this.mUdp.ReceivePacket(out buffer, out ipendPoint))
				{
					if (buffer.size > 0)
					{
						global::TNet.TcpPlayer player = this.GetPlayer(ipendPoint);
						if (player != null)
						{
							if (!player.udpIsUsable)
							{
								player.udpIsUsable = true;
							}
							try
							{
								if (this.ProcessPlayerPacket(buffer, player, false))
								{
									flag = true;
								}
							}
							catch (global::System.Exception ex)
							{
								base.Error(ex.Message);
								this.RemovePlayer(player);
							}
						}
						else if (buffer.size > 4)
						{
							try
							{
								global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
								global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
								if (packet == global::TNet.Packet.RequestActivateUDP)
								{
									int id = binaryReader.ReadInt32();
									player = this.GetPlayer(id);
									if (player != null && player.udpEndPoint != null && player.udpEndPoint.Address == ipendPoint.Address)
									{
										player.udpEndPoint = ipendPoint;
										player.udpIsUsable = true;
										this.mUdp.SendEmptyPacket(player.udpEndPoint);
									}
								}
								else if (packet == global::TNet.Packet.RequestPing)
								{
									this.BeginSend(global::TNet.Packet.ResponsePing);
									this.EndSend(ipendPoint);
								}
							}
							catch (global::System.Exception ex2)
							{
								base.Error(ex2.Message);
								this.RemovePlayer(player);
							}
						}
					}
					buffer.Recycle();
				}
				for (int i = 0; i < this.mPlayers.size; i++)
				{
					global::TNet.TcpPlayer tcpPlayer = this.mPlayers[i];
					int num = 0;
					while (num < 100 && tcpPlayer.ReceivePacket(out buffer))
					{
						if (buffer.size > 0)
						{
							try
							{
								if (this.ProcessPlayerPacket(buffer, tcpPlayer, true))
								{
									flag = true;
								}
							}
							catch (global::System.Exception ex3)
							{
								base.Error(ex3.Message);
								this.RemovePlayer(tcpPlayer);
							}
						}
						buffer.Recycle();
						num++;
					}
					if (tcpPlayer.stage == global::TNet.TcpProtocol.Stage.Connected)
					{
						if (tcpPlayer.timeoutTime > 0L && tcpPlayer.lastReceivedTime + tcpPlayer.timeoutTime < this.mTime)
						{
							this.RemovePlayer(tcpPlayer);
							continue;
						}
					}
					else if (tcpPlayer.lastReceivedTime + 2000L < this.mTime)
					{
						this.RemovePlayer(tcpPlayer);
						continue;
					}
				}
				if (!flag)
				{
					global::System.Threading.Thread.Sleep(1);
				}
			}
		}

		private void Error(global::TNet.TcpPlayer p, string error)
		{
		}

		private global::TNet.TcpPlayer AddPlayer(global::System.Net.Sockets.Socket socket)
		{
			global::TNet.TcpPlayer tcpPlayer = new global::TNet.TcpPlayer();
			tcpPlayer.StartReceiving(socket);
			this.mPlayers.Add(tcpPlayer);
			return tcpPlayer;
		}

		private void RemovePlayer(global::TNet.TcpPlayer p)
		{
			if (p != null)
			{
				this.SendLeaveChannel(p, false);
				p.Release();
				this.mPlayers.Remove(p);
				if (p.udpEndPoint != null)
				{
					this.mDictionaryEP.Remove(p.udpEndPoint);
					p.udpEndPoint = null;
					p.udpIsUsable = false;
				}
				if (p.id != 0)
				{
					if (this.mDictionaryID.Remove(p.id))
					{
						if (this.lobbyLink != null)
						{
							this.lobbyLink.SendUpdate(this);
						}
						if (this.onPlayerDisconnect != null)
						{
							this.onPlayerDisconnect(p);
						}
					}
					p.id = 0;
				}
			}
		}

		private global::TNet.TcpPlayer GetPlayer(int id)
		{
			global::TNet.TcpPlayer result = null;
			this.mDictionaryID.TryGetValue(id, out result);
			return result;
		}

		private global::TNet.TcpPlayer GetPlayer(global::System.Net.IPEndPoint ip)
		{
			global::TNet.TcpPlayer result = null;
			this.mDictionaryEP.TryGetValue(ip, out result);
			return result;
		}

		private void SetPlayerUdpEndPoint(global::TNet.TcpPlayer player, global::System.Net.IPEndPoint udp)
		{
			if (player.udpEndPoint != null)
			{
				this.mDictionaryEP.Remove(player.udpEndPoint);
			}
			player.udpEndPoint = udp;
			player.udpIsUsable = false;
			if (udp != null)
			{
				this.mDictionaryEP[udp] = player;
			}
		}

		private global::TNet.Channel CreateChannel(int channelID, out bool isNew)
		{
			int i = 0;
			global::TNet.Channel channel;
			while (i < this.mChannels.size)
			{
				channel = this.mChannels[i];
				if (channel.id == channelID)
				{
					isNew = false;
					if (channel.closed)
					{
						return null;
					}
					return channel;
				}
				else
				{
					i++;
				}
			}
			channel = new global::TNet.Channel();
			channel.id = channelID;
			this.mChannels.Add(channel);
			isNew = true;
			return channel;
		}

		private bool ChannelExists(int id)
		{
			for (int i = 0; i < this.mChannels.size; i++)
			{
				if (this.mChannels[i].id == id)
				{
					return true;
				}
			}
			return false;
		}

		private global::System.IO.BinaryWriter BeginSend(global::TNet.Packet type)
		{
			this.mBuffer = global::TNet.Buffer.Create();
			return this.mBuffer.BeginPacket(type);
		}

		private void EndSend(global::System.Net.IPEndPoint ip)
		{
			this.mBuffer.EndPacket();
			this.mUdp.Send(this.mBuffer, ip);
			this.mBuffer.Recycle();
			this.mBuffer = null;
		}

		private void EndSend(bool reliable, global::TNet.TcpPlayer player)
		{
			this.mBuffer.EndPacket();
			if (this.mBuffer.size > 1024)
			{
				reliable = true;
			}
			if (reliable || !player.udpIsUsable || player.udpEndPoint == null || !this.mAllowUdp)
			{
				player.SendTcpPacket(this.mBuffer);
			}
			else
			{
				this.mUdp.Send(this.mBuffer, player.udpEndPoint);
			}
			this.mBuffer.Recycle();
			this.mBuffer = null;
		}

		private void EndSend(bool reliable, global::TNet.Channel channel, global::TNet.TcpPlayer exclude)
		{
			this.mBuffer.EndPacket();
			if (this.mBuffer.size > 1024)
			{
				reliable = true;
			}
			for (int i = 0; i < channel.players.size; i++)
			{
				global::TNet.TcpPlayer tcpPlayer = channel.players[i];
				if (tcpPlayer.stage == global::TNet.TcpProtocol.Stage.Connected && tcpPlayer != exclude)
				{
					if (reliable || !tcpPlayer.udpIsUsable || tcpPlayer.udpEndPoint == null || !this.mAllowUdp)
					{
						tcpPlayer.SendTcpPacket(this.mBuffer);
					}
					else
					{
						this.mUdp.Send(this.mBuffer, tcpPlayer.udpEndPoint);
					}
				}
			}
			this.mBuffer.Recycle();
			this.mBuffer = null;
		}

		private void EndSend(bool reliable)
		{
			this.mBuffer.EndPacket();
			if (this.mBuffer.size > 1024)
			{
				reliable = true;
			}
			for (int i = 0; i < this.mChannels.size; i++)
			{
				global::TNet.Channel channel = this.mChannels[i];
				for (int j = 0; j < channel.players.size; j++)
				{
					global::TNet.TcpPlayer tcpPlayer = channel.players[j];
					if (tcpPlayer.stage == global::TNet.TcpProtocol.Stage.Connected)
					{
						if (reliable || !tcpPlayer.udpIsUsable || tcpPlayer.udpEndPoint == null || !this.mAllowUdp)
						{
							tcpPlayer.SendTcpPacket(this.mBuffer);
						}
						else
						{
							this.mUdp.Send(this.mBuffer, tcpPlayer.udpEndPoint);
						}
					}
				}
			}
			this.mBuffer.Recycle();
			this.mBuffer = null;
		}

		private void SendToChannel(bool reliable, global::TNet.Channel channel, global::TNet.Buffer buffer)
		{
			this.mBuffer.MarkAsUsed();
			if (this.mBuffer.size > 1024)
			{
				reliable = true;
			}
			for (int i = 0; i < channel.players.size; i++)
			{
				global::TNet.TcpPlayer tcpPlayer = channel.players[i];
				if (tcpPlayer.stage == global::TNet.TcpProtocol.Stage.Connected)
				{
					if (reliable || !tcpPlayer.udpIsUsable || tcpPlayer.udpEndPoint == null || !this.mAllowUdp)
					{
						tcpPlayer.SendTcpPacket(this.mBuffer);
					}
					else
					{
						this.mUdp.Send(this.mBuffer, tcpPlayer.udpEndPoint);
					}
				}
			}
			this.mBuffer.Recycle();
		}

		private void SendSetHost(global::TNet.TcpPlayer player)
		{
			if (player.channel != null && player.channel.host != player)
			{
				player.channel.host = player;
				global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.ResponseSetHost);
				binaryWriter.Write(player.id);
				this.EndSend(true, player.channel, null);
			}
		}

		private void SendLeaveChannel(global::TNet.TcpPlayer player, bool notify)
		{
			global::TNet.Channel channel = player.channel;
			if (channel != null)
			{
				channel.RemovePlayer(player, this.mTemp);
				player.channel = null;
				if (channel.players.size > 0)
				{
					global::System.IO.BinaryWriter binaryWriter;
					if (this.mTemp.size > 0)
					{
						binaryWriter = this.BeginSend(global::TNet.Packet.ResponseDestroy);
						binaryWriter.Write((ushort)this.mTemp.size);
						for (int i = 0; i < this.mTemp.size; i++)
						{
							binaryWriter.Write(this.mTemp[i]);
						}
						this.EndSend(true, channel, null);
					}
					if (channel.host == null)
					{
						this.SendSetHost(channel.players[0]);
					}
					binaryWriter = this.BeginSend(global::TNet.Packet.ResponsePlayerLeft);
					binaryWriter.Write(player.id);
					this.EndSend(true, channel, null);
				}
				else if (!channel.persistent)
				{
					this.mChannels.Remove(channel);
				}
				if (notify && player.isConnected)
				{
					this.BeginSend(global::TNet.Packet.ResponseLeaveChannel);
					this.EndSend(true, player);
				}
			}
		}

		private void SendJoinChannel(global::TNet.TcpPlayer player, global::TNet.Channel channel)
		{
			if (player.channel == null || player.channel != channel)
			{
				player.channel = channel;
				player.FinishJoiningChannel();
				global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.ResponsePlayerJoined);
				binaryWriter.Write(player.id);
				binaryWriter.Write((!string.IsNullOrEmpty(player.name)) ? player.name : "Guest");
				binaryWriter.WriteObject(player.data);
				this.EndSend(true, channel, null);
				channel.players.Add(player);
			}
		}

		private bool ProcessPlayerPacket(global::TNet.Buffer buffer, global::TNet.TcpPlayer player, bool reliable)
		{
			if (player.stage != global::TNet.TcpProtocol.Stage.Verifying)
			{
				global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
				global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
				global::TNet.Packet packet2 = packet;
				switch (packet2)
				{
				case global::TNet.Packet.Empty:
					break;
				case global::TNet.Packet.Error:
					this.Error(player, binaryReader.ReadString());
					break;
				case global::TNet.Packet.Disconnect:
					this.RemovePlayer(player);
					break;
				default:
					switch (packet2)
					{
					case global::TNet.Packet.ForwardToPlayer:
					{
						global::TNet.TcpPlayer player2 = this.GetPlayer(binaryReader.ReadInt32());
						if (player2 != null && player2.isConnected)
						{
							buffer.position -= 9;
							player2.SendTcpPacket(buffer);
						}
						return true;
					}
					case global::TNet.Packet.RequestSetTimeout:
						player.timeoutTime = (long)(binaryReader.ReadInt32() * 1000);
						return true;
					case global::TNet.Packet.Broadcast:
						buffer.position -= 5;
						for (int i = 0; i < this.mPlayers.size; i++)
						{
							global::TNet.TcpPlayer tcpPlayer = this.mPlayers[i];
							if (reliable || !tcpPlayer.udpIsUsable || tcpPlayer.udpEndPoint == null || !this.mAllowUdp)
							{
								tcpPlayer.SendTcpPacket(buffer);
							}
							else
							{
								this.mUdp.Send(buffer, tcpPlayer.udpEndPoint);
							}
						}
						return true;
					case global::TNet.Packet.RequestActivateUDP:
						player.udpIsUsable = true;
						if (player.udpEndPoint != null)
						{
							this.mUdp.SendEmptyPacket(player.udpEndPoint);
						}
						return true;
					case global::TNet.Packet.SyncPlayerData:
					{
						int position = buffer.position - 5;
						global::TNet.TcpPlayer player3 = this.GetPlayer(binaryReader.ReadInt32());
						if (player3 == null)
						{
							return true;
						}
						if (buffer.size > 1)
						{
							player3.data = binaryReader.ReadObject();
						}
						else
						{
							player3.data = null;
						}
						if (player3.channel != null)
						{
							buffer.position = position;
							for (int j = 0; j < player3.channel.players.size; j++)
							{
								global::TNet.TcpPlayer tcpPlayer2 = player3.channel.players[j];
								if (tcpPlayer2 != player)
								{
									if (reliable || !tcpPlayer2.udpIsUsable || tcpPlayer2.udpEndPoint == null || !this.mAllowUdp)
									{
										tcpPlayer2.SendTcpPacket(buffer);
									}
									else
									{
										this.mUdp.Send(buffer, tcpPlayer2.udpEndPoint);
									}
								}
							}
						}
						else if (player3 != player)
						{
							buffer.position = position;
							player3.SendTcpPacket(buffer);
						}
						return true;
					}
					}
					if (player.channel != null && packet <= global::TNet.Packet.ForwardToPlayerBuffered)
					{
						if (packet >= global::TNet.Packet.ForwardToAll)
						{
							this.ProcessForwardPacket(player, buffer, binaryReader, packet, reliable);
						}
						else
						{
							this.ProcessChannelPacket(player, buffer, binaryReader, packet);
						}
					}
					else if (this.onCustomPacket != null)
					{
						this.onCustomPacket(player, buffer, binaryReader, packet, reliable);
					}
					break;
				case global::TNet.Packet.RequestPing:
					this.BeginSend(global::TNet.Packet.ResponsePing);
					this.EndSend(true, player);
					break;
				case global::TNet.Packet.RequestSetUDP:
				{
					int num = (int)binaryReader.ReadUInt16();
					if (num != 0 && this.mUdp.isActive)
					{
						global::System.Net.IPAddress address = new global::System.Net.IPAddress(player.tcpEndPoint.Address.GetAddressBytes());
						this.SetPlayerUdpEndPoint(player, new global::System.Net.IPEndPoint(address, num));
					}
					else
					{
						this.SetPlayerUdpEndPoint(player, null);
					}
					ushort value = (!this.mUdp.isActive) ? 0 : ((ushort)this.mUdp.listeningPort);
					this.BeginSend(global::TNet.Packet.ResponseSetUDP).Write(value);
					this.EndSend(true, player);
					if (player.udpEndPoint != null)
					{
						this.mUdp.SendEmptyPacket(player.udpEndPoint);
					}
					break;
				}
				case global::TNet.Packet.RequestJoinChannel:
				{
					int num2 = binaryReader.ReadInt32();
					string text = binaryReader.ReadString();
					string text2 = binaryReader.ReadString();
					bool persistent = binaryReader.ReadBoolean();
					ushort playerLimit = binaryReader.ReadUInt16();
					if (num2 == -2)
					{
						bool flag = string.IsNullOrEmpty(text2);
						num2 = -1;
						for (int k = 0; k < this.mChannels.size; k++)
						{
							global::TNet.Channel channel = this.mChannels[k];
							if (channel.isOpen && (flag || text2.Equals(channel.level)) && (string.IsNullOrEmpty(channel.password) || channel.password == text))
							{
								num2 = channel.id;
								break;
							}
						}
						if (flag && num2 == -1)
						{
							global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.ResponseJoinChannel);
							binaryWriter.Write(false);
							binaryWriter.Write("No suitable channels found");
							this.EndSend(true, player);
							break;
						}
					}
					if (num2 == -1)
					{
						num2 = this.mRandom.Next(100000000);
						for (int l = 0; l < 1000; l++)
						{
							if (!this.ChannelExists(num2))
							{
								break;
							}
							num2 = this.mRandom.Next(100000000);
						}
					}
					if (player.channel == null || player.channel.id != num2)
					{
						bool flag2;
						global::TNet.Channel channel2 = this.CreateChannel(num2, out flag2);
						if (channel2 == null || !channel2.isOpen)
						{
							global::System.IO.BinaryWriter binaryWriter2 = this.BeginSend(global::TNet.Packet.ResponseJoinChannel);
							binaryWriter2.Write(false);
							binaryWriter2.Write("The requested channel is closed");
							this.EndSend(true, player);
						}
						else if (flag2)
						{
							channel2.password = text;
							channel2.persistent = persistent;
							channel2.level = text2;
							channel2.playerLimit = playerLimit;
							this.SendLeaveChannel(player, false);
							this.SendJoinChannel(player, channel2);
						}
						else if (string.IsNullOrEmpty(channel2.password) || channel2.password == text)
						{
							this.SendLeaveChannel(player, false);
							this.SendJoinChannel(player, channel2);
						}
						else
						{
							global::System.IO.BinaryWriter binaryWriter3 = this.BeginSend(global::TNet.Packet.ResponseJoinChannel);
							binaryWriter3.Write(false);
							binaryWriter3.Write("Wrong password");
							this.EndSend(true, player);
						}
					}
					break;
				}
				case global::TNet.Packet.RequestSetName:
				{
					player.name = binaryReader.ReadString();
					global::System.IO.BinaryWriter binaryWriter4 = this.BeginSend(global::TNet.Packet.ResponseRenamePlayer);
					binaryWriter4.Write(player.id);
					binaryWriter4.Write(player.name);
					if (player.channel != null)
					{
						this.EndSend(true, player.channel, null);
					}
					else
					{
						this.EndSend(true, player);
					}
					break;
				}
				case global::TNet.Packet.RequestSaveFile:
				{
					string fileName = binaryReader.ReadString();
					byte[] data = binaryReader.ReadBytes(binaryReader.ReadInt32());
					base.SaveFile(fileName, data);
					break;
				}
				case global::TNet.Packet.RequestLoadFile:
				{
					string text3 = binaryReader.ReadString();
					byte[] array = base.LoadFile(text3);
					global::System.IO.BinaryWriter binaryWriter5 = this.BeginSend(global::TNet.Packet.ResponseLoadFile);
					binaryWriter5.Write(text3);
					if (array != null)
					{
						binaryWriter5.Write(array.Length);
						binaryWriter5.Write(array);
					}
					else
					{
						binaryWriter5.Write(0);
					}
					this.EndSend(true, player);
					break;
				}
				case global::TNet.Packet.RequestDeleteFile:
					base.DeleteFile(binaryReader.ReadString());
					break;
				case global::TNet.Packet.RequestNoDelay:
					player.noDelay = binaryReader.ReadBoolean();
					break;
				case global::TNet.Packet.RequestChannelList:
				{
					global::System.IO.BinaryWriter binaryWriter6 = this.BeginSend(global::TNet.Packet.ResponseChannelList);
					int num3 = 0;
					for (int m = 0; m < this.mChannels.size; m++)
					{
						if (!this.mChannels[m].closed)
						{
							num3++;
						}
					}
					binaryWriter6.Write(num3);
					for (int n = 0; n < this.mChannels.size; n++)
					{
						global::TNet.Channel channel3 = this.mChannels[n];
						if (!channel3.closed)
						{
							binaryWriter6.Write(channel3.id);
							binaryWriter6.Write((ushort)channel3.players.size);
							binaryWriter6.Write(channel3.playerLimit);
							binaryWriter6.Write(!string.IsNullOrEmpty(channel3.password));
							binaryWriter6.Write(channel3.persistent);
							binaryWriter6.Write(channel3.level);
							binaryWriter6.Write(channel3.data);
						}
					}
					this.EndSend(true, player);
					break;
				}
				}
				return true;
			}
			if (player.VerifyRequestID(buffer, true))
			{
				this.mDictionaryID.Add(player.id, player);
				if (this.lobbyLink != null)
				{
					this.lobbyLink.SendUpdate(this);
				}
				if (this.onPlayerConnect != null)
				{
					this.onPlayerConnect(player);
				}
				return true;
			}
			this.RemovePlayer(player);
			return false;
		}

		private void ProcessForwardPacket(global::TNet.TcpPlayer player, global::TNet.Buffer buffer, global::System.IO.BinaryReader reader, global::TNet.Packet request, bool reliable)
		{
			if (!this.mUdp.isActive || buffer.size > 1024)
			{
				reliable = true;
			}
			switch (request)
			{
			case global::TNet.Packet.ForwardToHost:
				buffer.position -= 5;
				if (reliable || !player.udpIsUsable || player.channel.host.udpEndPoint == null || !this.mAllowUdp)
				{
					player.channel.host.SendTcpPacket(buffer);
				}
				else
				{
					this.mUdp.Send(buffer, player.channel.host.udpEndPoint);
				}
				return;
			case global::TNet.Packet.ForwardToPlayerBuffered:
			{
				int position = buffer.position - 5;
				global::TNet.TcpPlayer player2 = this.GetPlayer(reader.ReadInt32());
				uint num = reader.ReadUInt32();
				string funcName = ((num & 255U) != 0U) ? null : reader.ReadString();
				buffer.position = position;
				player.channel.CreateRFC(num, funcName, buffer);
				if (player2 != null && player2.isConnected)
				{
					if (reliable || !player2.udpIsUsable || player2.udpEndPoint == null || !this.mAllowUdp)
					{
						player2.SendTcpPacket(buffer);
					}
					else
					{
						this.mUdp.Send(buffer, player2.udpEndPoint);
					}
				}
				return;
			}
			}
			global::TNet.TcpPlayer tcpPlayer = (request != global::TNet.Packet.ForwardToOthers && request != global::TNet.Packet.ForwardToOthersSaved) ? null : player;
			int position2 = buffer.position - 5;
			if (request == global::TNet.Packet.ForwardToAllSaved || request == global::TNet.Packet.ForwardToOthersSaved)
			{
				uint num2 = reader.ReadUInt32();
				string funcName2 = ((num2 & 255U) != 0U) ? null : reader.ReadString();
				buffer.position = position2;
				player.channel.CreateRFC(num2, funcName2, buffer);
			}
			else
			{
				buffer.position = position2;
			}
			for (int i = 0; i < player.channel.players.size; i++)
			{
				global::TNet.TcpPlayer tcpPlayer2 = player.channel.players[i];
				if (tcpPlayer2 != tcpPlayer)
				{
					if (reliable || !tcpPlayer2.udpIsUsable || tcpPlayer2.udpEndPoint == null || !this.mAllowUdp)
					{
						tcpPlayer2.SendTcpPacket(buffer);
					}
					else
					{
						this.mUdp.Send(buffer, tcpPlayer2.udpEndPoint);
					}
				}
			}
		}

		private void ProcessChannelPacket(global::TNet.TcpPlayer player, global::TNet.Buffer buffer, global::System.IO.BinaryReader reader, global::TNet.Packet request)
		{
			switch (request)
			{
			case global::TNet.Packet.RequestLeaveChannel:
				this.SendLeaveChannel(player, true);
				break;
			case global::TNet.Packet.RequestCloseChannel:
				player.channel.persistent = false;
				player.channel.closed = true;
				break;
			case global::TNet.Packet.RequestSetPlayerLimit:
				player.channel.playerLimit = reader.ReadUInt16();
				break;
			case global::TNet.Packet.RequestLoadLevel:
				if (player.channel.host == player)
				{
					player.channel.Reset();
					player.channel.level = reader.ReadString();
					global::System.IO.BinaryWriter binaryWriter = this.BeginSend(global::TNet.Packet.ResponseLoadLevel);
					binaryWriter.Write((!string.IsNullOrEmpty(player.channel.level)) ? player.channel.level : string.Empty);
					this.EndSend(true, player.channel, null);
				}
				break;
			case global::TNet.Packet.RequestSetHost:
				if (player.channel.host == player)
				{
					global::TNet.TcpPlayer player2 = this.GetPlayer(reader.ReadInt32());
					if (player2 != null && player2.channel == player.channel)
					{
						this.SendSetHost(player2);
					}
				}
				break;
			case global::TNet.Packet.RequestRemoveRFC:
			{
				uint num = reader.ReadUInt32();
				string funcName = ((num & 255U) != 0U) ? null : reader.ReadString();
				player.channel.DeleteRFC(num, funcName);
				break;
			}
			case global::TNet.Packet.RequestCreate:
			{
				ushort num2 = reader.ReadUInt16();
				byte b = reader.ReadByte();
				uint num3 = 0U;
				if (b != 0)
				{
					num3 = (player.channel.objectCounter -= 1U);
					if (num3 < 32768U)
					{
						player.channel.objectCounter = 16777215U;
						num3 = 16777215U;
					}
					global::TNet.Channel.CreatedObject createdObject = new global::TNet.Channel.CreatedObject();
					createdObject.playerID = player.id;
					createdObject.objectID = num2;
					createdObject.uniqueID = num3;
					createdObject.type = b;
					if (buffer.size > 0)
					{
						createdObject.buffer = buffer;
						buffer.MarkAsUsed();
					}
					player.channel.created.Add(createdObject);
				}
				global::System.IO.BinaryWriter binaryWriter2 = this.BeginSend(global::TNet.Packet.ResponseCreate);
				binaryWriter2.Write(player.id);
				binaryWriter2.Write(num2);
				binaryWriter2.Write(num3);
				if (buffer.size > 0)
				{
					binaryWriter2.Write(buffer.buffer, buffer.position, buffer.size);
				}
				this.EndSend(true, player.channel, null);
				break;
			}
			case global::TNet.Packet.RequestDestroy:
			{
				uint num4 = reader.ReadUInt32();
				if (player.channel.DestroyObject(num4))
				{
					global::System.IO.BinaryWriter binaryWriter3 = this.BeginSend(global::TNet.Packet.ResponseDestroy);
					binaryWriter3.Write(1);
					binaryWriter3.Write(num4);
					this.EndSend(true, player.channel, null);
				}
				break;
			}
			case global::TNet.Packet.RequestSetChannelData:
			{
				player.channel.data = reader.ReadString();
				global::System.IO.BinaryWriter binaryWriter4 = this.BeginSend(global::TNet.Packet.ResponseSetChannelData);
				binaryWriter4.Write(player.channel.data);
				this.EndSend(true, player.channel, null);
				break;
			}
			}
		}

		public void SaveTo(string fileName)
		{
			if (this.mListener == null)
			{
				return;
			}
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			global::System.IO.BinaryWriter binaryWriter = new global::System.IO.BinaryWriter(memoryStream);
			binaryWriter.Write(0);
			int num = 0;
			for (int i = 0; i < this.mChannels.size; i++)
			{
				global::TNet.Channel channel = this.mChannels[i];
				if (!channel.closed && channel.persistent && channel.hasData)
				{
					binaryWriter.Write(channel.id);
					channel.SaveTo(binaryWriter);
					num++;
				}
			}
			if (num > 0)
			{
				memoryStream.Seek(0L, global::System.IO.SeekOrigin.Begin);
				binaryWriter.Write(num);
			}
			global::TNet.Tools.WriteFile(fileName, memoryStream.ToArray());
			memoryStream.Close();
		}

		public bool LoadFrom(string fileName)
		{
			byte[] array = global::TNet.Tools.ReadFile(fileName);
			if (array == null)
			{
				return false;
			}
			global::System.IO.MemoryStream input = new global::System.IO.MemoryStream(array);
			try
			{
				global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(input);
				int num = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int channelID = binaryReader.ReadInt32();
					bool flag;
					global::TNet.Channel channel = this.CreateChannel(channelID, out flag);
					if (flag)
					{
						channel.LoadFrom(binaryReader);
					}
				}
			}
			catch (global::System.Exception ex)
			{
				base.Error("Loading from " + fileName + ": " + ex.Message);
				return false;
			}
			return true;
		}

		public const ushort gameID = 1;

		public global::TNet.GameServer.OnCustomPacket onCustomPacket;

		public global::TNet.GameServer.OnPlayerAction onPlayerConnect;

		public global::TNet.GameServer.OnPlayerAction onPlayerDisconnect;

		public global::TNet.GameServer.OnShutdown onShutdown;

		public string name = "Game Server";

		public global::TNet.LobbyServerLink lobbyLink;

		private global::TNet.List<global::TNet.TcpPlayer> mPlayers = new global::TNet.List<global::TNet.TcpPlayer>();

		private global::System.Collections.Generic.Dictionary<int, global::TNet.TcpPlayer> mDictionaryID = new global::System.Collections.Generic.Dictionary<int, global::TNet.TcpPlayer>();

		private global::System.Collections.Generic.Dictionary<global::System.Net.IPEndPoint, global::TNet.TcpPlayer> mDictionaryEP = new global::System.Collections.Generic.Dictionary<global::System.Net.IPEndPoint, global::TNet.TcpPlayer>();

		private global::TNet.List<global::TNet.Channel> mChannels = new global::TNet.List<global::TNet.Channel>();

		private global::System.Random mRandom = new global::System.Random();

		private global::TNet.Buffer mBuffer;

		private global::System.Net.Sockets.TcpListener mListener;

		private global::System.Threading.Thread mThread;

		private int mListenerPort;

		private long mTime;

		private global::TNet.UdpProtocol mUdp = new global::TNet.UdpProtocol();

		private bool mAllowUdp;

		private global::TNet.List<uint> mTemp = new global::TNet.List<uint>();

		public delegate void OnCustomPacket(global::TNet.TcpPlayer player, global::TNet.Buffer buffer, global::System.IO.BinaryReader reader, global::TNet.Packet request, bool reliable);

		public delegate void OnPlayerAction(global::TNet.Player p);

		public delegate void OnShutdown();
	}
}
