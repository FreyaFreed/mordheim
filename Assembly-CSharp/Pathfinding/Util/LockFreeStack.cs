using System;
using System.Threading;

namespace Pathfinding.Util
{
	public class LockFreeStack
	{
		public void Push(global::Pathfinding.Path p)
		{
			global::Pathfinding.Path path;
			do
			{
				p.next = this.head;
				path = global::System.Threading.Interlocked.CompareExchange<global::Pathfinding.Path>(ref this.head, p, p.next);
			}
			while (path != p.next);
		}

		public global::Pathfinding.Path PopAll()
		{
			return global::System.Threading.Interlocked.Exchange<global::Pathfinding.Path>(ref this.head, null);
		}

		public global::Pathfinding.Path head;
	}
}
