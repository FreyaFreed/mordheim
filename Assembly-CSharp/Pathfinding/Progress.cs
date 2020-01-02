using System;

namespace Pathfinding
{
	public struct Progress
	{
		public Progress(float p, string d)
		{
			this.progress = p;
			this.description = d;
		}

		public override string ToString()
		{
			return this.progress.ToString("0.0") + " " + this.description;
		}

		public readonly float progress;

		public readonly string description;
	}
}
