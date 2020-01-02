using System;
using Pathfinding.Util;

namespace Pathfinding
{
	internal class PathReturnQueue
	{
		public PathReturnQueue(object pathsClaimedSilentlyBy)
		{
			this.pathsClaimedSilentlyBy = pathsClaimedSilentlyBy;
		}

		public void Enqueue(global::Pathfinding.Path path)
		{
			this.pathReturnStack.Push(path);
		}

		public void ReturnPaths(bool timeSlice)
		{
			global::Pathfinding.Path next = this.pathReturnStack.PopAll();
			if (this.pathReturnPop == null)
			{
				this.pathReturnPop = next;
			}
			else
			{
				global::Pathfinding.Path next2 = this.pathReturnPop;
				while (next2.next != null)
				{
					next2 = next2.next;
				}
				next2.next = next;
			}
			long num = (!timeSlice) ? 0L : (global::System.DateTime.UtcNow.Ticks + 10000L);
			int num2 = 0;
			while (this.pathReturnPop != null)
			{
				global::Pathfinding.Path path = this.pathReturnPop;
				this.pathReturnPop = this.pathReturnPop.next;
				path.next = null;
				path.ReturnPath();
				path.AdvanceState(global::Pathfinding.PathState.Returned);
				path.Release(this.pathsClaimedSilentlyBy, true);
				num2++;
				if (num2 > 5 && timeSlice)
				{
					num2 = 0;
					if (global::System.DateTime.UtcNow.Ticks >= num)
					{
						return;
					}
				}
			}
		}

		private global::Pathfinding.Util.LockFreeStack pathReturnStack = new global::Pathfinding.Util.LockFreeStack();

		private global::Pathfinding.Path pathReturnPop;

		private object pathsClaimedSilentlyBy;
	}
}
