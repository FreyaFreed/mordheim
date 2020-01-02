using System;

namespace Pathfinding
{
	public interface IPathModifier
	{
		int Order { get; }

		void Apply(global::Pathfinding.Path p);

		void PreProcess(global::Pathfinding.Path p);
	}
}
