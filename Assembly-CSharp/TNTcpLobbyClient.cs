using System;
using System.IO;
using System.Net;
using TNet;
using UnityEngine;

public class TNTcpLobbyClient : global::TNLobbyClient
{
	private void OnEnable()
	{
		if (this.mRemoteAddress == null)
		{
			this.mRemoteAddress = ((!string.IsNullOrEmpty(this.remoteAddress)) ? global::TNet.Tools.ResolveEndPoint(this.remoteAddress, this.remotePort) : new global::System.Net.IPEndPoint(global::System.Net.IPAddress.Broadcast, this.remotePort));
			if (this.mRemoteAddress == null)
			{
				this.mTcp.Error(string.Concat(new object[]
				{
					"Invalid address: ",
					this.remoteAddress,
					":",
					this.remotePort
				}));
			}
		}
	}

	protected override void OnDisable()
	{
		global::TNLobbyClient.isActive = false;
		this.mTcp.Disconnect();
		base.OnDisable();
		if (global::TNLobbyClient.onChange != null)
		{
			global::TNLobbyClient.onChange();
		}
	}

	private void Update()
	{
		bool flag = false;
		long num = global::System.DateTime.Now.Ticks / 10000L;
		if (this.mRemoteAddress != null && this.mTcp.stage == global::TNet.TcpProtocol.Stage.NotConnected && this.mNextConnect < num)
		{
			this.mNextConnect = num + 5000L;
			this.mTcp.Connect(this.mRemoteAddress);
		}
		global::TNet.Buffer buffer;
		while (this.mTcp.ReceivePacket(out buffer))
		{
			if (buffer.size > 0)
			{
				try
				{
					global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
					global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
					if (packet == global::TNet.Packet.ResponseID)
					{
						if (this.mTcp.VerifyResponseID(packet, binaryReader))
						{
							global::TNLobbyClient.isActive = true;
							this.mTcp.BeginSend(global::TNet.Packet.RequestServerList).Write(1);
							this.mTcp.EndSend();
						}
					}
					else if (packet == global::TNet.Packet.Disconnect)
					{
						global::TNLobbyClient.knownServers.Clear();
						global::TNLobbyClient.isActive = false;
						flag = true;
					}
					else if (packet == global::TNet.Packet.ResponseServerList)
					{
						global::TNLobbyClient.knownServers.ReadFrom(binaryReader, num);
						flag = true;
					}
					else if (packet == global::TNet.Packet.Error)
					{
						global::TNLobbyClient.errorString = binaryReader.ReadString();
						global::UnityEngine.Debug.LogWarning(global::TNLobbyClient.errorString);
						flag = true;
					}
				}
				catch (global::System.Exception ex)
				{
					global::TNLobbyClient.errorString = ex.Message;
					global::UnityEngine.Debug.LogWarning(ex.Message);
					this.mTcp.Close(false);
				}
			}
			buffer.Recycle();
		}
		if (flag && global::TNLobbyClient.onChange != null)
		{
			global::TNLobbyClient.onChange();
		}
	}

	private global::TNet.TcpProtocol mTcp = new global::TNet.TcpProtocol();

	private long mNextConnect;

	private global::System.Net.IPEndPoint mRemoteAddress;
}
