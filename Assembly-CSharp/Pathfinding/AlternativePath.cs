using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_alternative_path.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Modifiers/Alternative Path")]
	[global::System.Serializable]
	public class AlternativePath : global::Pathfinding.MonoModifier
	{
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		public override void Apply(global::Pathfinding.Path p)
		{
			if (this == null)
			{
				return;
			}
			object obj = this.lockObject;
			lock (obj)
			{
				this.toBeApplied = p.path.ToArray();
				if (!this.waitingForApply)
				{
					this.waitingForApply = true;
					global::AstarPath.OnPathPreSearch = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Combine(global::AstarPath.OnPathPreSearch, new global::Pathfinding.OnPathDelegate(this.ApplyNow));
				}
			}
		}

		public new void OnDestroy()
		{
			this.destroyed = true;
			object obj = this.lockObject;
			lock (obj)
			{
				if (!this.waitingForApply)
				{
					this.waitingForApply = true;
					global::AstarPath.OnPathPreSearch = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Combine(global::AstarPath.OnPathPreSearch, new global::Pathfinding.OnPathDelegate(this.ClearOnDestroy));
				}
			}
			this.OnDestroy();
		}

		private void ClearOnDestroy(global::Pathfinding.Path p)
		{
			object obj = this.lockObject;
			lock (obj)
			{
				global::AstarPath.OnPathPreSearch = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Remove(global::AstarPath.OnPathPreSearch, new global::Pathfinding.OnPathDelegate(this.ClearOnDestroy));
				this.waitingForApply = false;
				this.InversePrevious();
			}
		}

		private void InversePrevious()
		{
			int seed = this.prevSeed;
			this.rnd = new global::System.Random(seed);
			if (this.prevNodes != null)
			{
				bool flag = false;
				int num = this.rnd.Next(this.randomStep);
				for (int i = num; i < this.prevNodes.Length; i += this.rnd.Next(1, this.randomStep))
				{
					if ((ulong)this.prevNodes[i].Penalty < (ulong)((long)this.prevPenalty))
					{
						flag = true;
						this.prevNodes[i].Penalty = 0U;
					}
					else
					{
						this.prevNodes[i].Penalty = (uint)((ulong)this.prevNodes[i].Penalty - (ulong)((long)this.prevPenalty));
					}
				}
				if (flag)
				{
					global::UnityEngine.Debug.LogWarning("Penalty for some nodes has been reset while this modifier was active. Penalties might not be correctly set.", this);
				}
			}
		}

		private void ApplyNow(global::Pathfinding.Path somePath)
		{
			object obj = this.lockObject;
			lock (obj)
			{
				this.waitingForApply = false;
				global::AstarPath.OnPathPreSearch = (global::Pathfinding.OnPathDelegate)global::System.Delegate.Remove(global::AstarPath.OnPathPreSearch, new global::Pathfinding.OnPathDelegate(this.ApplyNow));
				this.InversePrevious();
				if (!this.destroyed)
				{
					int seed = this.seedGenerator.Next();
					this.rnd = new global::System.Random(seed);
					if (this.toBeApplied != null)
					{
						int num = this.rnd.Next(this.randomStep);
						for (int i = num; i < this.toBeApplied.Length; i += this.rnd.Next(1, this.randomStep))
						{
							this.toBeApplied[i].Penalty = (uint)((ulong)this.toBeApplied[i].Penalty + (ulong)((long)this.penalty));
						}
					}
					this.prevPenalty = this.penalty;
					this.prevSeed = seed;
					this.prevNodes = this.toBeApplied;
				}
			}
		}

		public int penalty = 1000;

		public int randomStep = 10;

		private global::Pathfinding.GraphNode[] prevNodes;

		private int prevSeed;

		private int prevPenalty;

		private bool waitingForApply;

		private readonly object lockObject = new object();

		private global::System.Random rnd = new global::System.Random();

		private readonly global::System.Random seedGenerator = new global::System.Random();

		private bool destroyed;

		private global::Pathfinding.GraphNode[] toBeApplied;
	}
}
