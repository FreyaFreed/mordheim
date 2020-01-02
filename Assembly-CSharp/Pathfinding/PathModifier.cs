using System;

namespace Pathfinding
{
	[global::System.Serializable]
	public abstract class PathModifier : global::Pathfinding.IPathModifier
	{
		public abstract int Order { get; }

		public void Awake(global::Seeker s)
		{
			this.seeker = s;
			if (s != null)
			{
				s.RegisterModifier(this);
			}
		}

		public void OnDestroy(global::Seeker s)
		{
			if (s != null)
			{
				s.DeregisterModifier(this);
			}
		}

		public virtual void PreProcess(global::Pathfinding.Path p)
		{
		}

		public abstract void Apply(global::Pathfinding.Path p);

		[global::System.NonSerialized]
		public global::Seeker seeker;
	}
}
