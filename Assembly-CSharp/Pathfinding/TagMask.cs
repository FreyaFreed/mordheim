using System;

namespace Pathfinding
{
	[global::System.Serializable]
	public class TagMask
	{
		public TagMask()
		{
		}

		public TagMask(int change, int set)
		{
			this.tagsChange = change;
			this.tagsSet = set;
		}

		public override string ToString()
		{
			return string.Empty + global::System.Convert.ToString(this.tagsChange, 2) + "\n" + global::System.Convert.ToString(this.tagsSet, 2);
		}

		public int tagsChange;

		public int tagsSet;
	}
}
