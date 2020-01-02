using System;
using System.Threading;

namespace Pathfinding
{
	internal class ThreadControlQueue
	{
		public ThreadControlQueue(int numReceivers)
		{
			this.numReceivers = numReceivers;
		}

		public bool IsEmpty
		{
			get
			{
				return this.head == null;
			}
		}

		public bool IsTerminating
		{
			get
			{
				return this.terminate;
			}
		}

		public void Block()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.blocked = true;
				this.block.Reset();
			}
		}

		public void Unblock()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.blocked = false;
				this.block.Set();
			}
		}

		public void Lock()
		{
			global::System.Threading.Monitor.Enter(this.lockObj);
		}

		public void Unlock()
		{
			global::System.Threading.Monitor.Exit(this.lockObj);
		}

		public bool AllReceiversBlocked
		{
			get
			{
				object obj = this.lockObj;
				bool result;
				lock (obj)
				{
					result = (this.blocked && this.blockedReceivers == this.numReceivers);
				}
				return result;
			}
		}

		public void PushFront(global::Pathfinding.Path p)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (!this.terminate)
				{
					if (this.tail == null)
					{
						this.head = p;
						this.tail = p;
						if (this.starving && !this.blocked)
						{
							this.starving = false;
							this.block.Set();
						}
						else
						{
							this.starving = false;
						}
					}
					else
					{
						p.next = this.head;
						this.head = p;
					}
				}
			}
		}

		public void Push(global::Pathfinding.Path p)
		{
			object obj = this.lockObj;
			lock (obj)
			{
				if (!this.terminate)
				{
					if (this.tail == null)
					{
						this.head = p;
						this.tail = p;
						if (this.starving && !this.blocked)
						{
							this.starving = false;
							this.block.Set();
						}
						else
						{
							this.starving = false;
						}
					}
					else
					{
						this.tail.next = p;
						this.tail = p;
					}
				}
			}
		}

		private void Starving()
		{
			this.starving = true;
			this.block.Reset();
		}

		public void TerminateReceivers()
		{
			object obj = this.lockObj;
			lock (obj)
			{
				this.terminate = true;
				this.block.Set();
			}
		}

		public global::Pathfinding.Path Pop()
		{
			global::Pathfinding.Path result;
			lock (this.lockObj)
			{
				if (this.terminate)
				{
					this.blockedReceivers++;
					throw new global::Pathfinding.ThreadControlQueue.QueueTerminationException();
				}
				if (this.head == null)
				{
					this.Starving();
				}
				while (this.blocked || this.starving)
				{
					this.blockedReceivers++;
					if (this.blockedReceivers > this.numReceivers)
					{
						throw new global::System.InvalidOperationException(string.Concat(new object[]
						{
							"More receivers are blocked than specified in constructor (",
							this.blockedReceivers,
							" > ",
							this.numReceivers,
							")"
						}));
					}
					global::System.Threading.Monitor.Exit(this.lockObj);
					this.block.WaitOne();
					global::System.Threading.Monitor.Enter(this.lockObj);
					if (this.terminate)
					{
						throw new global::Pathfinding.ThreadControlQueue.QueueTerminationException();
					}
					this.blockedReceivers--;
					if (this.head == null)
					{
						this.Starving();
					}
				}
				global::Pathfinding.Path path = this.head;
				if (this.head.next == null)
				{
					this.tail = null;
				}
				this.head = this.head.next;
				result = path;
			}
			return result;
		}

		public void ReceiverTerminated()
		{
			global::System.Threading.Monitor.Enter(this.lockObj);
			this.blockedReceivers++;
			global::System.Threading.Monitor.Exit(this.lockObj);
		}

		public global::Pathfinding.Path PopNoBlock(bool blockedBefore)
		{
			global::Pathfinding.Path result;
			lock (this.lockObj)
			{
				if (this.terminate)
				{
					this.blockedReceivers++;
					throw new global::Pathfinding.ThreadControlQueue.QueueTerminationException();
				}
				if (this.head == null)
				{
					this.Starving();
				}
				if (this.blocked || this.starving)
				{
					if (!blockedBefore)
					{
						this.blockedReceivers++;
						if (this.terminate)
						{
							throw new global::Pathfinding.ThreadControlQueue.QueueTerminationException();
						}
						if (this.blockedReceivers != this.numReceivers)
						{
							if (this.blockedReceivers > this.numReceivers)
							{
								throw new global::System.InvalidOperationException(string.Concat(new object[]
								{
									"More receivers are blocked than specified in constructor (",
									this.blockedReceivers,
									" > ",
									this.numReceivers,
									")"
								}));
							}
						}
					}
					result = null;
				}
				else
				{
					if (blockedBefore)
					{
						this.blockedReceivers--;
					}
					global::Pathfinding.Path path = this.head;
					if (this.head.next == null)
					{
						this.tail = null;
					}
					this.head = this.head.next;
					result = path;
				}
			}
			return result;
		}

		private global::Pathfinding.Path head;

		private global::Pathfinding.Path tail;

		private readonly object lockObj = new object();

		private readonly int numReceivers;

		private bool blocked;

		private int blockedReceivers;

		private bool starving;

		private bool terminate;

		private global::System.Threading.ManualResetEvent block = new global::System.Threading.ManualResetEvent(true);

		public class QueueTerminationException : global::System.Exception
		{
		}
	}
}
