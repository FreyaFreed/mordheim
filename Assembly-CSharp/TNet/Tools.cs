using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TNet
{
	public static class Tools
	{
		public static string ipCheckerUrl
		{
			get
			{
				return global::TNet.Tools.mChecker;
			}
			set
			{
				if (global::TNet.Tools.mChecker != value)
				{
					global::TNet.Tools.mChecker = value;
					global::TNet.Tools.mLocalAddress = null;
					global::TNet.Tools.mExternalAddress = null;
				}
			}
		}

		public static int randomPort
		{
			get
			{
				return 10000 + (int)(global::System.DateTime.Now.Ticks % 50000L);
			}
		}

		public static global::TNet.List<global::System.Net.NetworkInformation.NetworkInterface> networkInterfaces
		{
			get
			{
				if (global::TNet.Tools.mInterfaces == null)
				{
					global::TNet.Tools.mInterfaces = new global::TNet.List<global::System.Net.NetworkInformation.NetworkInterface>();
					global::System.Net.NetworkInformation.NetworkInterface[] allNetworkInterfaces = global::System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
					foreach (global::System.Net.NetworkInformation.NetworkInterface networkInterface in allNetworkInterfaces)
					{
						if (networkInterface.Supports(global::System.Net.NetworkInformation.NetworkInterfaceComponent.IPv4) && (networkInterface.OperationalStatus == global::System.Net.NetworkInformation.OperationalStatus.Up || networkInterface.OperationalStatus == global::System.Net.NetworkInformation.OperationalStatus.Unknown))
						{
							global::TNet.Tools.mInterfaces.Add(networkInterface);
						}
					}
				}
				return global::TNet.Tools.mInterfaces;
			}
		}

		public static global::TNet.List<global::System.Net.IPAddress> localAddresses
		{
			get
			{
				if (global::TNet.Tools.mAddresses == null)
				{
					global::TNet.Tools.mAddresses = new global::TNet.List<global::System.Net.IPAddress>();
					try
					{
						global::TNet.List<global::System.Net.NetworkInformation.NetworkInterface> networkInterfaces = global::TNet.Tools.networkInterfaces;
						for (int i = 0; i < networkInterfaces.size; i++)
						{
							global::System.Net.NetworkInformation.NetworkInterface networkInterface = networkInterfaces[i];
							if (networkInterface != null)
							{
								global::System.Net.NetworkInformation.IPInterfaceProperties ipproperties = networkInterface.GetIPProperties();
								if (ipproperties != null)
								{
									global::System.Net.NetworkInformation.UnicastIPAddressInformationCollection unicastAddresses = ipproperties.UnicastAddresses;
									foreach (global::System.Net.NetworkInformation.UnicastIPAddressInformation unicastIPAddressInformation in unicastAddresses)
									{
										if (global::TNet.Tools.IsValidAddress(unicastIPAddressInformation.Address))
										{
											global::TNet.Tools.mAddresses.Add(unicastIPAddressInformation.Address);
										}
									}
								}
							}
						}
					}
					catch (global::System.Exception)
					{
					}
					global::System.Net.IPAddress[] hostAddresses = global::System.Net.Dns.GetHostAddresses(global::System.Net.Dns.GetHostName());
					foreach (global::System.Net.IPAddress ipaddress in hostAddresses)
					{
						if (global::TNet.Tools.IsValidAddress(ipaddress) && !global::TNet.Tools.mAddresses.Contains(ipaddress))
						{
							global::TNet.Tools.mAddresses.Add(ipaddress);
						}
					}
					if (global::TNet.Tools.mAddresses.size == 0)
					{
						global::TNet.Tools.mAddresses.Add(global::System.Net.IPAddress.Loopback);
					}
				}
				return global::TNet.Tools.mAddresses;
			}
		}

		public static global::System.Net.IPAddress localAddress
		{
			get
			{
				if (global::TNet.Tools.mLocalAddress == null)
				{
					global::TNet.Tools.mLocalAddress = global::System.Net.IPAddress.None;
					global::TNet.List<global::System.Net.IPAddress> localAddresses = global::TNet.Tools.localAddresses;
					if (localAddresses.size > 0)
					{
						global::TNet.Tools.mLocalAddress = global::TNet.Tools.mAddresses[0];
						for (int i = 0; i < global::TNet.Tools.mAddresses.size; i++)
						{
							global::System.Net.IPAddress ipaddress = global::TNet.Tools.mAddresses[i];
							string text = ipaddress.ToString();
							if (!text.StartsWith("25."))
							{
								global::TNet.Tools.mLocalAddress = ipaddress;
								break;
							}
						}
					}
				}
				return global::TNet.Tools.mLocalAddress;
			}
			set
			{
				global::TNet.Tools.mLocalAddress = value;
				if (value != null)
				{
					global::TNet.List<global::System.Net.IPAddress> localAddresses = global::TNet.Tools.localAddresses;
					for (int i = 0; i < localAddresses.size; i++)
					{
						if (localAddresses[i] == value)
						{
							return;
						}
					}
				}
				global::System.Console.WriteLine("[TNet] " + value + " is not one of the local IP addresses. Strange things may happen.");
			}
		}

		public static global::System.Net.IPAddress externalAddress
		{
			get
			{
				if (global::TNet.Tools.mExternalAddress == null)
				{
					global::TNet.Tools.mExternalAddress = global::TNet.Tools.GetExternalAddress();
				}
				return (global::TNet.Tools.mExternalAddress == null) ? global::TNet.Tools.localAddress : global::TNet.Tools.mExternalAddress;
			}
		}

		public static void ResolveIPs()
		{
			global::TNet.Tools.ResolveIPs(null);
		}

		public static void ResolveIPs(global::TNet.Tools.OnResolvedIPs del)
		{
			if (global::TNet.Tools.isExternalIPReliable)
			{
				if (del != null)
				{
					del(global::TNet.Tools.localAddress, global::TNet.Tools.externalAddress);
				}
			}
			else
			{
				if (global::TNet.Tools.mOnResolve == null)
				{
					global::TNet.Tools.mOnResolve = new global::TNet.Tools.OnResolvedIPs(global::TNet.Tools.ResolveDummyFunc);
				}
				global::TNet.Tools.OnResolvedIPs obj = global::TNet.Tools.mOnResolve;
				lock (obj)
				{
					if (del != null)
					{
						global::TNet.Tools.mOnResolve = (global::TNet.Tools.OnResolvedIPs)global::System.Delegate.Combine(global::TNet.Tools.mOnResolve, del);
					}
					if (global::TNet.Tools.mResolveThread == null)
					{
						global::TNet.Tools.mResolveThread = new global::System.Threading.Thread(new global::System.Threading.ThreadStart(global::TNet.Tools.ResolveThread));
						global::TNet.Tools.mResolveThread.Start();
					}
				}
			}
		}

		private static void ResolveDummyFunc(global::System.Net.IPAddress a, global::System.Net.IPAddress b)
		{
		}

		private static void ResolveThread()
		{
			global::System.Net.IPAddress localAddress = global::TNet.Tools.localAddress;
			global::System.Net.IPAddress externalAddress = global::TNet.Tools.externalAddress;
			global::TNet.Tools.OnResolvedIPs obj = global::TNet.Tools.mOnResolve;
			lock (obj)
			{
				if (global::TNet.Tools.mOnResolve != null)
				{
					global::TNet.Tools.mOnResolve(localAddress, externalAddress);
				}
				global::TNet.Tools.mResolveThread = null;
				global::TNet.Tools.mOnResolve = null;
			}
		}

		private static global::System.Net.IPAddress GetExternalAddress()
		{
			if (global::TNet.Tools.mExternalAddress != null)
			{
				return global::TNet.Tools.mExternalAddress;
			}
			if (global::TNet.Tools.ResolveExternalIP(global::TNet.Tools.ipCheckerUrl))
			{
				return global::TNet.Tools.mExternalAddress;
			}
			if (global::TNet.Tools.ResolveExternalIP("http://icanhazip.com"))
			{
				return global::TNet.Tools.mExternalAddress;
			}
			if (global::TNet.Tools.ResolveExternalIP("http://bot.whatismyipaddress.com"))
			{
				return global::TNet.Tools.mExternalAddress;
			}
			if (global::TNet.Tools.ResolveExternalIP("http://ipinfo.io/ip"))
			{
				return global::TNet.Tools.mExternalAddress;
			}
			return global::TNet.Tools.localAddress;
		}

		private static bool ResolveExternalIP(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}
			try
			{
				global::System.Net.WebClient webClient = new global::System.Net.WebClient();
				string text = webClient.DownloadString(url).Trim();
				string[] array = text.Split(new char[]
				{
					':'
				});
				if (array.Length >= 2)
				{
					string[] array2 = array[1].Trim().Split(new char[]
					{
						'<'
					});
					global::TNet.Tools.mExternalAddress = global::TNet.Tools.ResolveAddress(array2[0]);
				}
				else
				{
					global::TNet.Tools.mExternalAddress = global::TNet.Tools.ResolveAddress(text);
				}
				if (global::TNet.Tools.mExternalAddress != null)
				{
					global::TNet.Tools.isExternalIPReliable = true;
					return true;
				}
			}
			catch (global::System.Exception)
			{
			}
			return false;
		}

		public static bool IsValidAddress(global::System.Net.IPAddress address)
		{
			return address.AddressFamily == global::System.Net.Sockets.AddressFamily.InterNetwork && !address.Equals(global::System.Net.IPAddress.Loopback) && !address.Equals(global::System.Net.IPAddress.None) && !address.Equals(global::System.Net.IPAddress.Any) && !address.ToString().StartsWith("169.");
		}

		public static global::System.Net.IPAddress ResolveAddress(string address)
		{
			if (string.IsNullOrEmpty(address))
			{
				return null;
			}
			if (address == "localhost")
			{
				return global::System.Net.IPAddress.Loopback;
			}
			global::System.Net.IPAddress result;
			if (global::System.Net.IPAddress.TryParse(address, out result))
			{
				return result;
			}
			try
			{
				global::System.Net.IPAddress[] hostAddresses = global::System.Net.Dns.GetHostAddresses(address);
				for (int i = 0; i < hostAddresses.Length; i++)
				{
					if (!global::System.Net.IPAddress.IsLoopback(hostAddresses[i]))
					{
						return hostAddresses[i];
					}
				}
			}
			catch (global::System.Exception)
			{
			}
			return null;
		}

		public static global::System.Net.IPEndPoint ResolveEndPoint(string address, int port)
		{
			global::System.Net.IPEndPoint ipendPoint = global::TNet.Tools.ResolveEndPoint(address);
			if (ipendPoint != null)
			{
				ipendPoint.Port = port;
			}
			return ipendPoint;
		}

		public static global::System.Net.IPEndPoint ResolveEndPoint(string address)
		{
			int port = 0;
			string[] array = address.Split(new char[]
			{
				':'
			});
			if (array.Length > 1)
			{
				address = array[0];
				int.TryParse(array[1], out port);
			}
			global::System.Net.IPAddress ipaddress = global::TNet.Tools.ResolveAddress(address);
			return (ipaddress == null) ? null : new global::System.Net.IPEndPoint(ipaddress, port);
		}

		public static string GetSubnet(global::System.Net.IPAddress ip)
		{
			if (ip == null)
			{
				return null;
			}
			string text = ip.ToString();
			int num = text.LastIndexOf('.');
			if (num == -1)
			{
				return null;
			}
			return text.Substring(0, num);
		}

		public static string GetResponse(global::System.Net.WebRequest request)
		{
			string text = string.Empty;
			try
			{
				global::System.Net.WebResponse response = request.GetResponse();
				global::System.IO.Stream responseStream = response.GetResponseStream();
				byte[] array = new byte[2048];
				for (;;)
				{
					int num = responseStream.Read(array, 0, array.Length);
					if (num <= 0)
					{
						break;
					}
					text += global::System.Text.Encoding.ASCII.GetString(array, 0, num);
				}
			}
			catch (global::System.Exception)
			{
				return null;
			}
			return text;
		}

		public static void Serialize(global::System.IO.BinaryWriter writer, global::System.Net.IPEndPoint ip)
		{
			byte[] addressBytes = ip.Address.GetAddressBytes();
			writer.Write((byte)addressBytes.Length);
			writer.Write(addressBytes);
			writer.Write((ushort)ip.Port);
		}

		public static void Serialize(global::System.IO.BinaryReader reader, out global::System.Net.IPEndPoint ip)
		{
			byte[] address = reader.ReadBytes((int)reader.ReadByte());
			int port = (int)reader.ReadUInt16();
			ip = new global::System.Net.IPEndPoint(new global::System.Net.IPAddress(address), port);
		}

		public static string[] GetFiles(string directory)
		{
			try
			{
				if (!global::System.IO.Directory.Exists(directory))
				{
					return null;
				}
				return global::System.IO.Directory.GetFiles(directory);
			}
			catch (global::System.Exception)
			{
			}
			return null;
		}

		public static bool WriteFile(string fileName, byte[] data)
		{
			if (data == null || data.Length == 0)
			{
				return global::TNet.Tools.DeleteFile(fileName);
			}
			try
			{
				string directoryName = global::System.IO.Path.GetDirectoryName(fileName);
				if (!string.IsNullOrEmpty(directoryName) && !global::System.IO.Directory.Exists(directoryName))
				{
					global::System.IO.Directory.CreateDirectory(directoryName);
				}
				global::System.IO.File.WriteAllBytes(fileName, data);
				return true;
			}
			catch (global::System.Exception)
			{
			}
			return false;
		}

		public static byte[] ReadFile(string fileName)
		{
			try
			{
				if (global::System.IO.File.Exists(fileName))
				{
					return global::System.IO.File.ReadAllBytes(fileName);
				}
			}
			catch (global::System.Exception)
			{
			}
			return null;
		}

		public static bool DeleteFile(string fileName)
		{
			try
			{
				if (global::System.IO.File.Exists(fileName))
				{
					global::System.IO.File.Delete(fileName);
				}
				return true;
			}
			catch (global::System.Exception)
			{
			}
			return false;
		}

		private static string mChecker;

		private static global::System.Net.IPAddress mLocalAddress;

		private static global::System.Net.IPAddress mExternalAddress;

		public static bool isExternalIPReliable;

		private static global::TNet.List<global::System.Net.NetworkInformation.NetworkInterface> mInterfaces;

		private static global::TNet.List<global::System.Net.IPAddress> mAddresses;

		private static global::TNet.Tools.OnResolvedIPs mOnResolve;

		private static global::System.Threading.Thread mResolveThread;

		public delegate void OnResolvedIPs(global::System.Net.IPAddress local, global::System.Net.IPAddress ext);
	}
}
