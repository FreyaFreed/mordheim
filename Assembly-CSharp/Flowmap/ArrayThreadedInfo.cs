using System;
using System.Threading;

namespace Flowmap
{
	internal class ArrayThreadedInfo
	{
		public ArrayThreadedInfo(int start, int length, global::System.Threading.ManualResetEvent resetEvent)
		{
			this.start = start;
			this.length = length;
			this.resetEvent = resetEvent;
		}

		public int start;

		public int length;

		public global::System.Threading.ManualResetEvent resetEvent;
	}
}
