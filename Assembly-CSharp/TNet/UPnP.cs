using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TNet
{
	public class UPnP
	{
		public UPnP()
		{
			global::System.Threading.Thread thread = new global::System.Threading.Thread(new global::System.Threading.ParameterizedThreadStart(this.ThreadDiscover));
			this.mDiscover = thread;
			this.mThreads.Add(thread);
			thread.Start(thread);
		}

		public global::TNet.UPnP.Status status
		{
			get
			{
				return this.mStatus;
			}
		}

		public global::System.Net.IPAddress gatewayAddress
		{
			get
			{
				return this.mGatewayAddress;
			}
		}

		public bool hasThreadsActive
		{
			get
			{
				return this.mThreads.size > 0;
			}
		}

		~UPnP()
		{
			this.mDiscover = null;
			this.Close();
			this.WaitForThreads();
		}

		public void Close()
		{
			global::TNet.List<global::System.Threading.Thread> obj = this.mThreads;
			lock (obj)
			{
				int i = this.mThreads.size;
				while (i > 0)
				{
					global::System.Threading.Thread thread = this.mThreads[--i];
					if (thread != this.mDiscover)
					{
						thread.Abort();
						this.mThreads.RemoveAt(i);
					}
				}
			}
			int j = this.mPorts.size;
			while (j > 0)
			{
				int num = this.mPorts[--j];
				int port = num >> 8;
				bool tcp = (num & 1) == 1;
				this.Close(port, tcp, null);
			}
		}

		public void WaitForThreads()
		{
			int num = 0;
			while (this.mThreads.size > 0 && num < 2000)
			{
				global::System.Threading.Thread.Sleep(1);
				num++;
			}
		}

		private void ThreadDiscover(object obj)
		{
			global::System.Threading.Thread item = (global::System.Threading.Thread)obj;
			string s = "M-SEARCH * HTTP/1.1\r\nHOST: 239.255.255.250:1900\r\nST:upnp:rootdevice\r\nMAN:\"ssdp:discover\"\r\nMX:3\r\n\r\n";
			byte[] bytes = global::System.Text.Encoding.ASCII.GetBytes(s);
			int port = 10000 + (int)(global::System.DateTime.Now.Ticks % 45000L);
			global::TNet.List<global::System.Net.IPAddress> localAddresses = global::TNet.Tools.localAddresses;
			for (int i = 0; i < localAddresses.size; i++)
			{
				global::System.Net.IPAddress address = localAddresses[i];
				this.mStatus = global::TNet.UPnP.Status.Searching;
				global::System.Net.Sockets.UdpClient udpClient = null;
				try
				{
					global::System.Net.Sockets.UdpClient udpClient2 = new global::System.Net.Sockets.UdpClient(new global::System.Net.IPEndPoint(address, port));
					udpClient2.Connect(global::System.Net.IPAddress.Broadcast, 1900);
					udpClient2.Send(bytes, bytes.Length);
					udpClient2.Close();
					udpClient = new global::System.Net.Sockets.UdpClient(new global::System.Net.IPEndPoint(address, port));
					udpClient.Client.ReceiveTimeout = 3000;
					global::System.Net.IPEndPoint ipendPoint = new global::System.Net.IPEndPoint(global::System.Net.IPAddress.Any, 0);
					byte[] array;
					do
					{
						array = udpClient.Receive(ref ipendPoint);
					}
					while (!this.ParseResponse(global::System.Text.Encoding.ASCII.GetString(array, 0, array.Length)));
					udpClient.Close();
					global::TNet.List<global::System.Threading.Thread> obj2 = this.mThreads;
					lock (obj2)
					{
						this.mGatewayAddress = ipendPoint.Address;
						this.mStatus = global::TNet.UPnP.Status.Success;
						this.mThreads.Remove(item);
					}
					this.mDiscover = null;
					return;
				}
				catch (global::System.Exception)
				{
				}
				if (udpClient != null)
				{
					udpClient.Close();
				}
				global::TNet.List<global::System.Threading.Thread> obj3 = this.mThreads;
				lock (obj3)
				{
					this.mStatus = global::TNet.UPnP.Status.Failure;
					this.mThreads.Remove(item);
				}
				this.mDiscover = null;
				if (this.mStatus == global::TNet.UPnP.Status.Success)
				{
					break;
				}
			}
			if (this.mStatus != global::TNet.UPnP.Status.Success)
			{
				global::System.Console.WriteLine("UPnP discovery failed. TNet won't be able to open ports automatically.");
				return;
			}
		}

		private bool ParseResponse(string response)
		{
			int num = response.IndexOf("LOCATION:", global::System.StringComparison.OrdinalIgnoreCase);
			if (num == -1)
			{
				return false;
			}
			num += 9;
			int num2 = response.IndexOf('\r', num);
			if (num2 == -1)
			{
				return false;
			}
			string text = response.Substring(num, num2 - num).Trim();
			int num3 = text.IndexOf("://");
			num3 = text.IndexOf('/', num3 + 3);
			this.mGatewayURL = text.Substring(0, num3);
			return this.GetControlURL(text);
		}

		private bool GetControlURL(string url)
		{
			string response = global::TNet.Tools.GetResponse(global::System.Net.WebRequest.Create(url));
			if (string.IsNullOrEmpty(response))
			{
				return false;
			}
			this.mServiceType = "WANIPConnection";
			int num = response.IndexOf(this.mServiceType);
			if (num == -1)
			{
				this.mServiceType = "WANPPPConnection";
				num = response.IndexOf(this.mServiceType);
				if (num == -1)
				{
					return false;
				}
			}
			int num2 = response.IndexOf("</service>", num);
			if (num2 == -1)
			{
				return false;
			}
			int num3 = response.IndexOf("<controlURL>", num, num2 - num);
			if (num3 == -1)
			{
				return false;
			}
			num3 += 12;
			num2 = response.IndexOf("</controlURL>", num3, num2 - num3);
			if (num2 == -1)
			{
				return false;
			}
			this.mControlURL = this.mGatewayURL + response.Substring(num3, num2 - num3);
			return true;
		}

		private string SendRequest(string action, string content, int timeout, int repeat)
		{
			string text = string.Concat(new string[]
			{
				"<?xml version=\"1.0\"?>\n<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n<s:Body>\n<m:",
				action,
				" xmlns:m=\"urn:schemas-upnp-org:service:",
				this.mServiceType,
				":1\">\n"
			});
			if (!string.IsNullOrEmpty(content))
			{
				text += content;
			}
			text = text + "</m:" + action + ">\n</s:Body>\n</s:Envelope>\n";
			byte[] bytes = global::System.Text.Encoding.UTF8.GetBytes(text);
			try
			{
				for (int i = 0; i < repeat; i++)
				{
					global::System.Net.WebRequest webRequest = global::System.Net.WebRequest.Create(this.mControlURL);
					webRequest.Timeout = timeout;
					webRequest.Method = "POST";
					webRequest.Headers.Add("SOAPACTION", string.Concat(new string[]
					{
						"\"urn:schemas-upnp-org:service:",
						this.mServiceType,
						":1#",
						action,
						"\""
					}));
					webRequest.ContentType = "text/xml; charset=\"utf-8\"";
					webRequest.ContentLength = (long)bytes.Length;
					webRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
					string response = global::TNet.Tools.GetResponse(webRequest);
					if (!string.IsNullOrEmpty(response))
					{
						return response;
					}
				}
			}
			catch (global::System.Exception)
			{
			}
			return null;
		}

		public void OpenTCP(int port)
		{
			this.Open(port, true, null);
		}

		public void OpenUDP(int port)
		{
			this.Open(port, false, null);
		}

		public void OpenTCP(int port, global::TNet.UPnP.OnPortRequest callback)
		{
			this.Open(port, true, callback);
		}

		public void OpenUDP(int port, global::TNet.UPnP.OnPortRequest callback)
		{
			this.Open(port, false, callback);
		}

		private void Open(int port, bool tcp, global::TNet.UPnP.OnPortRequest callback)
		{
			int item = port << 8 | ((!tcp) ? 0 : 1);
			if (port > 0 && !this.mPorts.Contains(item) && this.mStatus != global::TNet.UPnP.Status.Failure)
			{
				string text = global::TNet.Tools.localAddress.ToString();
				if (text == "127.0.0.1")
				{
					return;
				}
				this.mPorts.Add(item);
				global::TNet.UPnP.ExtraParams extraParams = new global::TNet.UPnP.ExtraParams();
				extraParams.callback = callback;
				extraParams.port = port;
				extraParams.protocol = ((!tcp) ? global::System.Net.Sockets.ProtocolType.Udp : global::System.Net.Sockets.ProtocolType.Tcp);
				extraParams.action = "AddPortMapping";
				extraParams.request = string.Concat(new object[]
				{
					"<NewRemoteHost></NewRemoteHost>\n<NewExternalPort>",
					port,
					"</NewExternalPort>\n<NewProtocol>",
					(!tcp) ? "UDP" : "TCP",
					"</NewProtocol>\n<NewInternalPort>",
					port,
					"</NewInternalPort>\n<NewInternalClient>",
					text,
					"</NewInternalClient>\n<NewEnabled>1</NewEnabled>\n<NewPortMappingDescription>",
					this.name,
					"</NewPortMappingDescription>\n<NewLeaseDuration>0</NewLeaseDuration>\n"
				});
				extraParams.th = new global::System.Threading.Thread(new global::System.Threading.ParameterizedThreadStart(this.OpenRequest));
				global::TNet.List<global::System.Threading.Thread> obj = this.mThreads;
				lock (obj)
				{
					this.mThreads.Add(extraParams.th);
				}
				extraParams.th.Start(extraParams);
			}
			else if (callback != null)
			{
				callback(this, port, (!tcp) ? global::System.Net.Sockets.ProtocolType.Udp : global::System.Net.Sockets.ProtocolType.Tcp, false);
			}
		}

		public void CloseTCP(int port)
		{
			this.Close(port, true, null);
		}

		public void CloseUDP(int port)
		{
			this.Close(port, false, null);
		}

		public void CloseTCP(int port, global::TNet.UPnP.OnPortRequest callback)
		{
			this.Close(port, true, callback);
		}

		public void CloseUDP(int port, global::TNet.UPnP.OnPortRequest callback)
		{
			this.Close(port, false, callback);
		}

		private void Close(int port, bool tcp, global::TNet.UPnP.OnPortRequest callback)
		{
			int item = port << 8 | ((!tcp) ? 0 : 1);
			if (port > 0 && this.mPorts.Remove(item) && this.mStatus == global::TNet.UPnP.Status.Success)
			{
				global::TNet.UPnP.ExtraParams extraParams = new global::TNet.UPnP.ExtraParams();
				extraParams.callback = callback;
				extraParams.port = port;
				extraParams.protocol = ((!tcp) ? global::System.Net.Sockets.ProtocolType.Udp : global::System.Net.Sockets.ProtocolType.Tcp);
				extraParams.action = "DeletePortMapping";
				extraParams.request = string.Concat(new object[]
				{
					"<NewRemoteHost></NewRemoteHost>\n<NewExternalPort>",
					port,
					"</NewExternalPort>\n<NewProtocol>",
					(!tcp) ? "UDP" : "TCP",
					"</NewProtocol>\n"
				});
				if (callback != null)
				{
					extraParams.th = new global::System.Threading.Thread(new global::System.Threading.ParameterizedThreadStart(this.CloseRequest));
					global::TNet.List<global::System.Threading.Thread> obj = this.mThreads;
					lock (obj)
					{
						this.mThreads.Add(extraParams.th);
						extraParams.th.Start(extraParams);
					}
				}
				else
				{
					this.CloseRequest(extraParams);
				}
			}
			else if (callback != null)
			{
				callback(this, port, (!tcp) ? global::System.Net.Sockets.ProtocolType.Udp : global::System.Net.Sockets.ProtocolType.Tcp, false);
			}
		}

		private void OpenRequest(object obj)
		{
			while (this.mStatus == global::TNet.UPnP.Status.Searching)
			{
				global::System.Threading.Thread.Sleep(1);
			}
			this.SendRequest((global::TNet.UPnP.ExtraParams)obj);
		}

		private void CloseRequest(object obj)
		{
			this.SendRequest((global::TNet.UPnP.ExtraParams)obj);
		}

		private void SendRequest(global::TNet.UPnP.ExtraParams xp)
		{
			string value = (this.mStatus != global::TNet.UPnP.Status.Success) ? null : this.SendRequest(xp.action, xp.request, 10000, 3);
			if (xp.callback != null)
			{
				xp.callback(this, xp.port, xp.protocol, !string.IsNullOrEmpty(value));
			}
			if (xp.th != null)
			{
				global::TNet.List<global::System.Threading.Thread> obj = this.mThreads;
				lock (obj)
				{
					this.mThreads.Remove(xp.th);
				}
			}
		}

		private global::TNet.UPnP.Status mStatus;

		private global::System.Net.IPAddress mGatewayAddress = global::System.Net.IPAddress.None;

		private global::System.Threading.Thread mDiscover;

		private string mGatewayURL;

		private string mControlURL;

		private string mServiceType;

		private global::TNet.List<global::System.Threading.Thread> mThreads = new global::TNet.List<global::System.Threading.Thread>();

		private global::TNet.List<int> mPorts = new global::TNet.List<int>();

		public string name = "TNetServer";

		public enum Status
		{
			Inactive,
			Searching,
			Success,
			Failure
		}

		private class ExtraParams
		{
			public global::System.Threading.Thread th;

			public string action;

			public string request;

			public int port;

			public global::System.Net.Sockets.ProtocolType protocol;

			public global::TNet.UPnP.OnPortRequest callback;
		}

		public delegate void OnPortRequest(global::TNet.UPnP up, int port, global::System.Net.Sockets.ProtocolType protocol, bool success);
	}
}
