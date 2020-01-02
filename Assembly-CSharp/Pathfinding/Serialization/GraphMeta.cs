using System;
using System.Collections.Generic;

namespace Pathfinding.Serialization
{
	public class GraphMeta
	{
		public global::System.Type GetGraphType(int i)
		{
			if (string.IsNullOrEmpty(this.typeNames[i]))
			{
				return null;
			}
			global::System.Type type = typeof(global::AstarPath).Assembly.GetType(this.typeNames[i]);
			if (!object.Equals(type, null))
			{
				return type;
			}
			throw new global::System.Exception("No graph of type '" + this.typeNames[i] + "' could be created, type does not exist");
		}

		public global::System.Version version;

		public int graphs;

		public global::System.Collections.Generic.List<string> guids;

		public global::System.Collections.Generic.List<string> typeNames;
	}
}
