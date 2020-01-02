using System;
using System.Threading;

namespace Flowmap
{
	internal class ThreadedFieldBakeInfo : global::Flowmap.ArrayThreadedInfo
	{
		public ThreadedFieldBakeInfo(int start, int length, global::System.Threading.ManualResetEvent resetEvent, global::FlowSimulationField[] fields, global::FlowmapGenerator generator) : base(start, length, resetEvent)
		{
			this.fields = fields;
			this.generator = generator;
		}

		public global::FlowSimulationField[] fields;

		public global::FlowmapGenerator generator;
	}
}
