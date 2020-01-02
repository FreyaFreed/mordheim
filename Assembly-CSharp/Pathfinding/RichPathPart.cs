using System;
using Pathfinding.Util;

namespace Pathfinding
{
	public abstract class RichPathPart : global::Pathfinding.Util.IAstarPooledObject
	{
		public abstract void OnEnterPool();
	}
}
