using System;

namespace Steamworks
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Struct, AllowMultiple = false)]
	internal class CallbackIdentityAttribute : global::System.Attribute
	{
		public CallbackIdentityAttribute(int callbackNum)
		{
			this.Identity = callbackNum;
		}

		public int Identity { get; set; }
	}
}
