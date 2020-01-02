using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Serialization.Zip;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Serialization
{
	public class AstarSerializer
	{
		public AstarSerializer(global::Pathfinding.AstarData data)
		{
			this.data = data;
			this.settings = global::Pathfinding.Serialization.SerializeSettings.Settings;
		}

		public AstarSerializer(global::Pathfinding.AstarData data, global::Pathfinding.Serialization.SerializeSettings settings)
		{
			this.data = data;
			this.settings = settings;
		}

		private static global::System.Text.StringBuilder GetStringBuilder()
		{
			global::Pathfinding.Serialization.AstarSerializer._stringBuilder.Length = 0;
			return global::Pathfinding.Serialization.AstarSerializer._stringBuilder;
		}

		private static void CloseOrDispose(global::System.IO.BinaryWriter writer)
		{
			writer.Close();
		}

		public void SetGraphIndexOffset(int offset)
		{
			this.graphIndexOffset = offset;
		}

		private void AddChecksum(byte[] bytes)
		{
			this.checksum = global::Pathfinding.Util.Checksum.GetChecksum(bytes, this.checksum);
		}

		public uint GetChecksum()
		{
			return this.checksum;
		}

		public void OpenSerialize()
		{
			this.zip = new global::Pathfinding.Serialization.Zip.ZipFile();
			this.zip.AlternateEncoding = global::System.Text.Encoding.UTF8;
			this.zip.AlternateEncodingUsage = global::Pathfinding.Serialization.Zip.ZipOption.Always;
			this.meta = new global::Pathfinding.Serialization.GraphMeta();
		}

		public byte[] CloseSerialize()
		{
			byte[] array = this.SerializeMeta();
			this.AddChecksum(array);
			this.zip.AddEntry("meta.json", array);
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			this.zip.Save(memoryStream);
			array = memoryStream.ToArray();
			memoryStream.Dispose();
			this.zip.Dispose();
			this.zip = null;
			return array;
		}

		public void SerializeGraphs(global::Pathfinding.NavGraph[] _graphs)
		{
			if (this.graphs != null)
			{
				throw new global::System.InvalidOperationException("Cannot serialize graphs multiple times.");
			}
			this.graphs = _graphs;
			if (this.zip == null)
			{
				throw new global::System.NullReferenceException("You must not call CloseSerialize before a call to this function");
			}
			if (this.graphs == null)
			{
				this.graphs = new global::Pathfinding.NavGraph[0];
			}
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					byte[] bytes = this.Serialize(this.graphs[i]);
					this.AddChecksum(bytes);
					this.zip.AddEntry("graph" + i + ".json", bytes);
				}
			}
		}

		private byte[] SerializeMeta()
		{
			if (this.graphs == null)
			{
				throw new global::System.Exception("No call to SerializeGraphs has been done");
			}
			this.meta.version = global::AstarPath.Version;
			this.meta.graphs = this.graphs.Length;
			this.meta.guids = new global::System.Collections.Generic.List<string>();
			this.meta.typeNames = new global::System.Collections.Generic.List<string>();
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					this.meta.guids.Add(this.graphs[i].guid.ToString());
					this.meta.typeNames.Add(this.graphs[i].GetType().FullName);
				}
				else
				{
					this.meta.guids.Add(null);
					this.meta.typeNames.Add(null);
				}
			}
			global::System.Text.StringBuilder stringBuilder = global::Pathfinding.Serialization.AstarSerializer.GetStringBuilder();
			global::Pathfinding.Serialization.TinyJsonSerializer.Serialize(this.meta, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		public byte[] Serialize(global::Pathfinding.NavGraph graph)
		{
			global::System.Text.StringBuilder stringBuilder = global::Pathfinding.Serialization.AstarSerializer.GetStringBuilder();
			global::Pathfinding.Serialization.TinyJsonSerializer.Serialize(graph, stringBuilder);
			return this.encoding.GetBytes(stringBuilder.ToString());
		}

		[global::System.Obsolete("Not used anymore. You can safely remove the call to this function.")]
		public void SerializeNodes()
		{
		}

		private static int GetMaxNodeIndexInAllGraphs(global::Pathfinding.NavGraph[] graphs)
		{
			int maxIndex = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
					{
						maxIndex = global::System.Math.Max(node.NodeIndex, maxIndex);
						if (node.NodeIndex == -1)
						{
							global::UnityEngine.Debug.LogError("Graph contains destroyed nodes. This is a bug.");
						}
						return true;
					});
				}
			}
			return maxIndex;
		}

		private static byte[] SerializeNodeIndices(global::Pathfinding.NavGraph[] graphs)
		{
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			global::System.IO.BinaryWriter wr = new global::System.IO.BinaryWriter(memoryStream);
			int maxNodeIndexInAllGraphs = global::Pathfinding.Serialization.AstarSerializer.GetMaxNodeIndexInAllGraphs(graphs);
			wr.Write(maxNodeIndexInAllGraphs);
			int maxNodeIndex2 = 0;
			for (int i = 0; i < graphs.Length; i++)
			{
				if (graphs[i] != null)
				{
					graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
					{
						maxNodeIndex2 = global::System.Math.Max(node.NodeIndex, maxNodeIndex2);
						wr.Write(node.NodeIndex);
						return true;
					});
				}
			}
			if (maxNodeIndex2 != maxNodeIndexInAllGraphs)
			{
				throw new global::System.Exception("Some graphs are not consistent in their GetNodes calls, sequential calls give different results.");
			}
			byte[] result = memoryStream.ToArray();
			global::Pathfinding.Serialization.AstarSerializer.CloseOrDispose(wr);
			return result;
		}

		private static byte[] SerializeGraphExtraInfo(global::Pathfinding.NavGraph graph)
		{
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			global::System.IO.BinaryWriter writer = new global::System.IO.BinaryWriter(memoryStream);
			global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(writer);
			graph.SerializeExtraInfo(ctx);
			byte[] result = memoryStream.ToArray();
			global::Pathfinding.Serialization.AstarSerializer.CloseOrDispose(writer);
			return result;
		}

		private static byte[] SerializeGraphNodeReferences(global::Pathfinding.NavGraph graph)
		{
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			global::System.IO.BinaryWriter writer = new global::System.IO.BinaryWriter(memoryStream);
			global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(writer);
			graph.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				node.SerializeReferences(ctx);
				return true;
			});
			global::Pathfinding.Serialization.AstarSerializer.CloseOrDispose(writer);
			return memoryStream.ToArray();
		}

		public void SerializeExtraInfo()
		{
			if (!this.settings.nodes)
			{
				return;
			}
			if (this.graphs == null)
			{
				throw new global::System.InvalidOperationException("Cannot serialize extra info with no serialized graphs (call SerializeGraphs first)");
			}
			byte[] bytes = global::Pathfinding.Serialization.AstarSerializer.SerializeNodeIndices(this.graphs);
			this.AddChecksum(bytes);
			this.zip.AddEntry("graph_references.binary", bytes);
			for (int i = 0; i < this.graphs.Length; i++)
			{
				if (this.graphs[i] != null)
				{
					bytes = global::Pathfinding.Serialization.AstarSerializer.SerializeGraphExtraInfo(this.graphs[i]);
					this.AddChecksum(bytes);
					this.zip.AddEntry("graph" + i + "_extra.binary", bytes);
					bytes = global::Pathfinding.Serialization.AstarSerializer.SerializeGraphNodeReferences(this.graphs[i]);
					this.AddChecksum(bytes);
					this.zip.AddEntry("graph" + i + "_references.binary", bytes);
				}
			}
			bytes = this.SerializeNodeLinks();
			this.AddChecksum(bytes);
			this.zip.AddEntry("node_link2.binary", bytes);
		}

		private byte[] SerializeNodeLinks()
		{
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			global::System.IO.BinaryWriter writer = new global::System.IO.BinaryWriter(memoryStream);
			global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(writer);
			global::Pathfinding.NodeLink2.SerializeReferences(ctx);
			return memoryStream.ToArray();
		}

		public void SerializeEditorSettings(global::Pathfinding.GraphEditorBase[] editors)
		{
			if (editors == null || !this.settings.editorSettings)
			{
				return;
			}
			for (int i = 0; i < editors.Length; i++)
			{
				if (editors[i] == null)
				{
					return;
				}
				global::System.Text.StringBuilder stringBuilder = global::Pathfinding.Serialization.AstarSerializer.GetStringBuilder();
				global::Pathfinding.Serialization.TinyJsonSerializer.Serialize(editors[i], stringBuilder);
				byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
				if (bytes.Length > 2)
				{
					this.AddChecksum(bytes);
					this.zip.AddEntry("graph" + i + "_editor.json", bytes);
				}
			}
		}

		public bool OpenDeserialize(byte[] bytes)
		{
			this.zipStream = new global::System.IO.MemoryStream();
			this.zipStream.Write(bytes, 0, bytes.Length);
			this.zipStream.Position = 0L;
			try
			{
				this.zip = global::Pathfinding.Serialization.Zip.ZipFile.Read(this.zipStream);
			}
			catch (global::System.Exception arg)
			{
				global::UnityEngine.Debug.LogError("Caught exception when loading from zip\n" + arg);
				this.zipStream.Dispose();
				return false;
			}
			if (this.zip.ContainsEntry("meta.json"))
			{
				this.meta = this.DeserializeMeta(this.zip["meta.json"]);
			}
			else
			{
				if (!this.zip.ContainsEntry("meta.binary"))
				{
					throw new global::System.Exception("No metadata found in serialized data.");
				}
				this.meta = this.DeserializeBinaryMeta(this.zip["meta.binary"]);
			}
			if (global::Pathfinding.Serialization.AstarSerializer.FullyDefinedVersion(this.meta.version) > global::Pathfinding.Serialization.AstarSerializer.FullyDefinedVersion(global::AstarPath.Version))
			{
				global::UnityEngine.Debug.LogWarning(string.Concat(new object[]
				{
					"Trying to load data from a newer version of the A* Pathfinding Project\nCurrent version: ",
					global::AstarPath.Version,
					" Data version: ",
					this.meta.version,
					"\nThis is usually fine as the stored data is usually backwards and forwards compatible.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n"
				}));
			}
			else if (global::Pathfinding.Serialization.AstarSerializer.FullyDefinedVersion(this.meta.version) < global::Pathfinding.Serialization.AstarSerializer.FullyDefinedVersion(global::AstarPath.Version))
			{
				global::UnityEngine.Debug.LogWarning(string.Concat(new object[]
				{
					"Trying to load data from an older version of the A* Pathfinding Project\nCurrent version: ",
					global::AstarPath.Version,
					" Data version: ",
					this.meta.version,
					"\nThis is usually fine, it just means you have upgraded to a new version.\nHowever node data (not settings) can get corrupted between versions (even though I try my best to keep compatibility), so it is recommended to recalculate any caches (those for faster startup) and resave any files. Even if it seems to load fine, it might cause subtle bugs.\n"
				}));
			}
			return true;
		}

		private static global::System.Version FullyDefinedVersion(global::System.Version v)
		{
			return new global::System.Version(global::UnityEngine.Mathf.Max(v.Major, 0), global::UnityEngine.Mathf.Max(v.Minor, 0), global::UnityEngine.Mathf.Max(v.Build, 0), global::UnityEngine.Mathf.Max(v.Revision, 0));
		}

		public void CloseDeserialize()
		{
			this.zipStream.Dispose();
			this.zip.Dispose();
			this.zip = null;
			this.zipStream = null;
		}

		private global::Pathfinding.NavGraph DeserializeGraph(int zipIndex, int graphIndex)
		{
			global::System.Type graphType = this.meta.GetGraphType(zipIndex);
			if (object.Equals(graphType, null))
			{
				return null;
			}
			global::Pathfinding.NavGraph navGraph = this.data.CreateGraph(graphType);
			navGraph.graphIndex = (uint)graphIndex;
			string text = "graph" + zipIndex + ".json";
			string text2 = "graph" + zipIndex + ".binary";
			if (this.zip.ContainsEntry(text))
			{
				global::Pathfinding.Serialization.TinyJsonDeserializer.Deserialize(global::Pathfinding.Serialization.AstarSerializer.GetString(this.zip[text]), graphType, navGraph);
			}
			else
			{
				if (!this.zip.ContainsEntry(text2))
				{
					throw new global::System.IO.FileNotFoundException(string.Concat(new object[]
					{
						"Could not find data for graph ",
						zipIndex,
						" in zip. Entry 'graph",
						zipIndex,
						".json' does not exist"
					}));
				}
				global::System.IO.BinaryReader binaryReader = global::Pathfinding.Serialization.AstarSerializer.GetBinaryReader(this.zip[text2]);
				global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(binaryReader, null, navGraph.graphIndex, this.meta);
				navGraph.DeserializeSettingsCompatibility(ctx);
			}
			if (navGraph.guid.ToString() != this.meta.guids[zipIndex])
			{
				throw new global::System.Exception(string.Concat(new object[]
				{
					"Guid in graph file not equal to guid defined in meta file. Have you edited the data manually?\n",
					navGraph.guid,
					" != ",
					this.meta.guids[zipIndex]
				}));
			}
			return navGraph;
		}

		public global::Pathfinding.NavGraph[] DeserializeGraphs()
		{
			global::System.Collections.Generic.List<global::Pathfinding.NavGraph> list = new global::System.Collections.Generic.List<global::Pathfinding.NavGraph>();
			this.graphIndexInZip = new global::System.Collections.Generic.Dictionary<global::Pathfinding.NavGraph, int>();
			for (int i = 0; i < this.meta.graphs; i++)
			{
				int graphIndex = list.Count + this.graphIndexOffset;
				global::Pathfinding.NavGraph navGraph = this.DeserializeGraph(i, graphIndex);
				if (navGraph != null)
				{
					list.Add(navGraph);
					this.graphIndexInZip[navGraph] = i;
				}
			}
			this.graphs = list.ToArray();
			return this.graphs;
		}

		private bool DeserializeExtraInfo(global::Pathfinding.NavGraph graph)
		{
			int num = this.graphIndexInZip[graph];
			global::Pathfinding.Serialization.Zip.ZipEntry zipEntry = this.zip["graph" + num + "_extra.binary"];
			if (zipEntry == null)
			{
				return false;
			}
			global::System.IO.BinaryReader binaryReader = global::Pathfinding.Serialization.AstarSerializer.GetBinaryReader(zipEntry);
			global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(binaryReader, null, graph.graphIndex, this.meta);
			graph.DeserializeExtraInfo(ctx);
			return true;
		}

		private bool AnyDestroyedNodesInGraphs()
		{
			bool result = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				this.graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
				{
					if (node.Destroyed)
					{
						result = true;
					}
					return true;
				});
			}
			return result;
		}

		private global::Pathfinding.GraphNode[] DeserializeNodeReferenceMap()
		{
			global::Pathfinding.Serialization.Zip.ZipEntry zipEntry = this.zip["graph_references.binary"];
			if (zipEntry == null)
			{
				throw new global::System.Exception("Node references not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			global::System.IO.BinaryReader reader = global::Pathfinding.Serialization.AstarSerializer.GetBinaryReader(zipEntry);
			int num = reader.ReadInt32();
			global::Pathfinding.GraphNode[] int2Node = new global::Pathfinding.GraphNode[num + 1];
			try
			{
				for (int i = 0; i < this.graphs.Length; i++)
				{
					this.graphs[i].GetNodes(delegate(global::Pathfinding.GraphNode node)
					{
						int num2 = reader.ReadInt32();
						int2Node[num2] = node;
						return true;
					});
				}
			}
			catch (global::System.Exception innerException)
			{
				throw new global::System.Exception("Some graph(s) has thrown an exception during GetNodes, or some graph(s) have deserialized more or fewer nodes than were serialized", innerException);
			}
			if (reader.BaseStream.Position != reader.BaseStream.Length)
			{
				throw new global::System.Exception(string.Concat(new object[]
				{
					reader.BaseStream.Length / 4L,
					" nodes were serialized, but only data for ",
					reader.BaseStream.Position / 4L,
					" nodes was found. The data looks corrupt."
				}));
			}
			reader.Close();
			return int2Node;
		}

		private void DeserializeNodeReferences(global::Pathfinding.NavGraph graph, global::Pathfinding.GraphNode[] int2Node)
		{
			int num = this.graphIndexInZip[graph];
			global::Pathfinding.Serialization.Zip.ZipEntry zipEntry = this.zip["graph" + num + "_references.binary"];
			if (zipEntry == null)
			{
				throw new global::System.Exception("Node references for graph " + num + " not found in the data. Was this loaded from an older version of the A* Pathfinding Project?");
			}
			global::System.IO.BinaryReader binaryReader = global::Pathfinding.Serialization.AstarSerializer.GetBinaryReader(zipEntry);
			global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(binaryReader, int2Node, graph.graphIndex, this.meta);
			graph.GetNodes(delegate(global::Pathfinding.GraphNode node)
			{
				node.DeserializeReferences(ctx);
				return true;
			});
		}

		public void DeserializeExtraInfo()
		{
			bool flag = false;
			for (int i = 0; i < this.graphs.Length; i++)
			{
				flag |= this.DeserializeExtraInfo(this.graphs[i]);
			}
			if (!flag)
			{
				return;
			}
			if (this.AnyDestroyedNodesInGraphs())
			{
				global::UnityEngine.Debug.LogError("Graph contains destroyed nodes. This is a bug.");
			}
			global::Pathfinding.GraphNode[] int2Node = this.DeserializeNodeReferenceMap();
			for (int j = 0; j < this.graphs.Length; j++)
			{
				this.DeserializeNodeReferences(this.graphs[j], int2Node);
			}
			this.DeserializeNodeLinks(int2Node);
		}

		private void DeserializeNodeLinks(global::Pathfinding.GraphNode[] int2Node)
		{
			global::Pathfinding.Serialization.Zip.ZipEntry zipEntry = this.zip["node_link2.binary"];
			if (zipEntry == null)
			{
				return;
			}
			global::System.IO.BinaryReader binaryReader = global::Pathfinding.Serialization.AstarSerializer.GetBinaryReader(zipEntry);
			global::Pathfinding.Serialization.GraphSerializationContext ctx = new global::Pathfinding.Serialization.GraphSerializationContext(binaryReader, int2Node, 0U, this.meta);
			global::Pathfinding.NodeLink2.DeserializeReferences(ctx);
		}

		public void PostDeserialization()
		{
			for (int i = 0; i < this.graphs.Length; i++)
			{
				this.graphs[i].PostDeserialization();
			}
		}

		public void DeserializeEditorSettings(global::Pathfinding.GraphEditorBase[] graphEditors)
		{
			if (graphEditors == null)
			{
				return;
			}
			for (int i = 0; i < graphEditors.Length; i++)
			{
				if (graphEditors[i] != null)
				{
					for (int j = 0; j < this.graphs.Length; j++)
					{
						if (graphEditors[i].target == this.graphs[j])
						{
							int num = this.graphIndexInZip[this.graphs[j]];
							global::Pathfinding.Serialization.Zip.ZipEntry zipEntry = this.zip["graph" + num + "_editor.json"];
							if (zipEntry != null)
							{
								global::Pathfinding.Serialization.TinyJsonDeserializer.Deserialize(global::Pathfinding.Serialization.AstarSerializer.GetString(zipEntry), graphEditors[i].GetType(), graphEditors[i]);
								break;
							}
						}
					}
				}
			}
		}

		private static global::System.IO.BinaryReader GetBinaryReader(global::Pathfinding.Serialization.Zip.ZipEntry entry)
		{
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			return new global::System.IO.BinaryReader(memoryStream);
		}

		private static string GetString(global::Pathfinding.Serialization.Zip.ZipEntry entry)
		{
			global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream();
			entry.Extract(memoryStream);
			memoryStream.Position = 0L;
			global::System.IO.StreamReader streamReader = new global::System.IO.StreamReader(memoryStream);
			string result = streamReader.ReadToEnd();
			memoryStream.Position = 0L;
			streamReader.Dispose();
			return result;
		}

		private global::Pathfinding.Serialization.GraphMeta DeserializeMeta(global::Pathfinding.Serialization.Zip.ZipEntry entry)
		{
			return global::Pathfinding.Serialization.TinyJsonDeserializer.Deserialize(global::Pathfinding.Serialization.AstarSerializer.GetString(entry), typeof(global::Pathfinding.Serialization.GraphMeta), null) as global::Pathfinding.Serialization.GraphMeta;
		}

		private global::Pathfinding.Serialization.GraphMeta DeserializeBinaryMeta(global::Pathfinding.Serialization.Zip.ZipEntry entry)
		{
			global::Pathfinding.Serialization.GraphMeta graphMeta = new global::Pathfinding.Serialization.GraphMeta();
			global::System.IO.BinaryReader binaryReader = global::Pathfinding.Serialization.AstarSerializer.GetBinaryReader(entry);
			if (binaryReader.ReadString() != "A*")
			{
				throw new global::System.Exception("Invalid magic number in saved data");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			int num3 = binaryReader.ReadInt32();
			int num4 = binaryReader.ReadInt32();
			if (num < 0)
			{
				graphMeta.version = new global::System.Version(0, 0);
			}
			else if (num2 < 0)
			{
				graphMeta.version = new global::System.Version(num, 0);
			}
			else if (num3 < 0)
			{
				graphMeta.version = new global::System.Version(num, num2);
			}
			else if (num4 < 0)
			{
				graphMeta.version = new global::System.Version(num, num2, num3);
			}
			else
			{
				graphMeta.version = new global::System.Version(num, num2, num3, num4);
			}
			graphMeta.graphs = binaryReader.ReadInt32();
			graphMeta.guids = new global::System.Collections.Generic.List<string>();
			int num5 = binaryReader.ReadInt32();
			for (int i = 0; i < num5; i++)
			{
				graphMeta.guids.Add(binaryReader.ReadString());
			}
			graphMeta.typeNames = new global::System.Collections.Generic.List<string>();
			num5 = binaryReader.ReadInt32();
			for (int j = 0; j < num5; j++)
			{
				graphMeta.typeNames.Add(binaryReader.ReadString());
			}
			return graphMeta;
		}

		public static void SaveToFile(string path, byte[] data)
		{
			using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Create))
			{
				fileStream.Write(data, 0, data.Length);
			}
		}

		public static byte[] LoadFromFile(string path)
		{
			byte[] result;
			using (global::System.IO.FileStream fileStream = new global::System.IO.FileStream(path, global::System.IO.FileMode.Open))
			{
				byte[] array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
				result = array;
			}
			return result;
		}

		private const string binaryExt = ".binary";

		private const string jsonExt = ".json";

		private global::Pathfinding.AstarData data;

		private global::Pathfinding.Serialization.Zip.ZipFile zip;

		private global::System.IO.MemoryStream zipStream;

		private global::Pathfinding.Serialization.GraphMeta meta;

		private global::Pathfinding.Serialization.SerializeSettings settings;

		private global::Pathfinding.NavGraph[] graphs;

		private global::System.Collections.Generic.Dictionary<global::Pathfinding.NavGraph, int> graphIndexInZip;

		private int graphIndexOffset;

		private uint checksum = uint.MaxValue;

		private global::System.Text.UTF8Encoding encoding = new global::System.Text.UTF8Encoding();

		private static global::System.Text.StringBuilder _stringBuilder = new global::System.Text.StringBuilder();
	}
}
