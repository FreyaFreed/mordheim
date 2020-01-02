using System;
using System.IO;
using System.Net;

namespace TNet
{
	public class TcpPlayer : global::TNet.TcpProtocol
	{
		public void FinishJoiningChannel()
		{
			global::TNet.Buffer buffer = global::TNet.Buffer.Create();
			global::System.IO.BinaryWriter binaryWriter = buffer.BeginPacket(global::TNet.Packet.ResponseJoiningChannel);
			binaryWriter.Write(this.channel.id);
			binaryWriter.Write((short)this.channel.players.size);
			for (int i = 0; i < this.channel.players.size; i++)
			{
				global::TNet.TcpPlayer tcpPlayer = this.channel.players[i];
				binaryWriter.Write(tcpPlayer.id);
				binaryWriter.Write((!string.IsNullOrEmpty(tcpPlayer.name)) ? tcpPlayer.name : "Guest");
				binaryWriter.WriteObject(tcpPlayer.data);
			}
			int startOffset = buffer.EndPacket();
			if (this.channel.host == null)
			{
				this.channel.host = this;
			}
			buffer.BeginPacket(global::TNet.Packet.ResponseSetHost, startOffset);
			binaryWriter.Write(this.channel.host.id);
			startOffset = buffer.EndTcpPacketStartingAt(startOffset);
			if (!string.IsNullOrEmpty(this.channel.data))
			{
				buffer.BeginPacket(global::TNet.Packet.ResponseSetChannelData, startOffset);
				binaryWriter.Write(this.channel.data);
				startOffset = buffer.EndTcpPacketStartingAt(startOffset);
			}
			buffer.BeginPacket(global::TNet.Packet.ResponseLoadLevel, startOffset);
			binaryWriter.Write((!string.IsNullOrEmpty(this.channel.level)) ? this.channel.level : string.Empty);
			startOffset = buffer.EndTcpPacketStartingAt(startOffset);
			for (int j = 0; j < this.channel.created.size; j++)
			{
				global::TNet.Channel.CreatedObject createdObject = this.channel.created.buffer[j];
				buffer.BeginPacket(global::TNet.Packet.ResponseCreate, startOffset);
				binaryWriter.Write(createdObject.playerID);
				binaryWriter.Write(createdObject.objectID);
				binaryWriter.Write(createdObject.uniqueID);
				binaryWriter.Write(createdObject.buffer.buffer, createdObject.buffer.position, createdObject.buffer.size);
				startOffset = buffer.EndTcpPacketStartingAt(startOffset);
			}
			if (this.channel.destroyed.size != 0)
			{
				buffer.BeginPacket(global::TNet.Packet.ResponseDestroy, startOffset);
				binaryWriter.Write((ushort)this.channel.destroyed.size);
				for (int k = 0; k < this.channel.destroyed.size; k++)
				{
					binaryWriter.Write(this.channel.destroyed.buffer[k]);
				}
				startOffset = buffer.EndTcpPacketStartingAt(startOffset);
			}
			for (int l = 0; l < this.channel.rfcs.size; l++)
			{
				global::TNet.Buffer buffer2 = this.channel.rfcs[l].buffer;
				buffer2.BeginReading();
				buffer.BeginWriting(startOffset);
				binaryWriter.Write(buffer2.buffer, buffer2.position, buffer2.size);
				startOffset = buffer.EndWriting();
			}
			buffer.BeginPacket(global::TNet.Packet.ResponseJoinChannel, startOffset);
			binaryWriter.Write(true);
			startOffset = buffer.EndTcpPacketStartingAt(startOffset);
			base.SendTcpPacket(buffer);
			buffer.Recycle();
		}

		public global::TNet.Channel channel;

		public global::System.Net.IPEndPoint udpEndPoint;

		public bool udpIsUsable;
	}
}
