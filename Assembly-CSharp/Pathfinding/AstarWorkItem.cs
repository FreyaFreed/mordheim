using System;

namespace Pathfinding
{
	public struct AstarWorkItem
	{
		public AstarWorkItem(global::System.Func<bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = null;
			this.update = update;
		}

		public AstarWorkItem(global::System.Func<global::Pathfinding.IWorkItemContext, bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = update;
			this.update = null;
		}

		public AstarWorkItem(global::System.Action init, global::System.Func<bool, bool> update = null)
		{
			this.init = init;
			this.initWithContext = null;
			this.update = update;
			this.updateWithContext = null;
		}

		public AstarWorkItem(global::System.Action<global::Pathfinding.IWorkItemContext> init, global::System.Func<global::Pathfinding.IWorkItemContext, bool, bool> update = null)
		{
			this.init = null;
			this.initWithContext = init;
			this.update = null;
			this.updateWithContext = update;
		}

		public global::System.Action init;

		public global::System.Action<global::Pathfinding.IWorkItemContext> initWithContext;

		public global::System.Func<bool, bool> update;

		public global::System.Func<global::Pathfinding.IWorkItemContext, bool, bool> updateWithContext;
	}
}
