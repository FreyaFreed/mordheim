using System;

namespace TNet
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RFC : global::System.Attribute
	{
		public RFC()
		{
		}

		public RFC(byte rid)
		{
			this.id = rid;
		}

		public byte id;
	}
}
