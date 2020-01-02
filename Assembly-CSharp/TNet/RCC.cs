using System;

namespace TNet
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RCC : global::System.Attribute
	{
		public RCC(byte rid)
		{
			this.id = rid;
		}

		public byte id;
	}
}
