using System;
using System.IO;
using System.Net;
using TNet;
using UnityEngine;

public class TNUdpLobbyClient : global::TNLobbyClient
{
	private void Awake()
	{
	}

	private void OnEnable()
	{
		if (this.mRequest == null)
		{
			this.mRequest = global::TNet.Buffer.Create();
			this.mRequest.BeginPacket(global::TNet.Packet.RequestServerList).Write(1);
			this.mRequest.EndPacket();
		}
		if (this.mRemoteAddress == null)
		{
			this.mRemoteAddress = ((!string.IsNullOrEmpty(this.remoteAddress)) ? global::TNet.Tools.ResolveEndPoint(this.remoteAddress, this.remotePort) : new global::System.Net.IPEndPoint(global::System.Net.IPAddress.Broadcast, this.remotePort));
			if (this.mRemoteAddress == null)
			{
				this.mUdp.Error(new global::System.Net.IPEndPoint(global::System.Net.IPAddress.Loopback, this.mUdp.listeningPort), string.Concat(new object[]
				{
					"Invalid address: ",
					this.remoteAddress,
					":",
					this.remotePort
				}));
			}
		}
		if (!this.mUdp.Start(global::TNet.Tools.randomPort))
		{
			this.mUdp.Start(global::TNet.Tools.randomPort);
		}
	}

	protected override void OnDisable()
	{
		global::TNLobbyClient.isActive = false;
		base.OnDisable();
		try
		{
			this.mUdp.Stop();
			if (this.mRequest != null)
			{
				this.mRequest.Recycle();
				this.mRequest = null;
			}
			if (global::TNLobbyClient.onChange != null)
			{
				global::TNLobbyClient.onChange();
			}
		}
		catch (global::System.Exception)
		{
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			if (global::TNLobbyClient.isActive)
			{
				this.mReEnable = true;
				this.OnDisable();
			}
		}
		else if (this.mReEnable)
		{
			this.mReEnable = false;
			this.OnEnable();
		}
	}

	private void Update()
	{
		bool flag = false;
		long num = global::System.DateTime.Now.Ticks / 10000L;
		global::TNet.Buffer buffer;
		global::System.Net.IPEndPoint ipendPoint;
		while (this.mUdp != null && this.mUdp.ReceivePacket(out buffer, out ipendPoint))
		{
			if (buffer.size > 0)
			{
				try
				{
					global::System.IO.BinaryReader binaryReader = buffer.BeginReading();
					global::TNet.Packet packet = (global::TNet.Packet)binaryReader.ReadByte();
					if (packet == global::TNet.Packet.ResponseServerList)
					{
						global::TNLobbyClient.isActive = true;
						this.mNextSend = num + 3000L;
						global::TNLobbyClient.knownServers.ReadFrom(binaryReader, num);
						global::TNLobbyClient.knownServers.Cleanup(num);
						flag = true;
					}
					else if (packet == global::TNet.Packet.Error)
					{
						global::TNLobbyClient.errorString = binaryReader.ReadString();
						global::UnityEngine.Debug.LogWarning(global::TNLobbyClient.errorString);
						flag = true;
					}
				}
				catch (global::System.Exception)
				{
				}
			}
			buffer.Recycle();
		}
		if (global::TNLobbyClient.knownServers.Cleanup(num))
		{
			flag = true;
		}
		if (flag && global::TNLobbyClient.onChange != null)
		{
			global::TNLobbyClient.onChange();
		}
		else if (this.mNextSend < num && this.mUdp != null)
		{
			this.mNextSend = num + 3000L;
			this.mUdp.Send(this.mRequest, this.mRemoteAddress);
		}
	}

	private global::TNet.UdpProtocol mUdp = new global::TNet.UdpProtocol();

	private global::TNet.Buffer mRequest;

	private long mNextSend;

	private global::System.Net.IPEndPoint mRemoteAddress;

	private bool mReEnable;
}
