using System;

namespace Steamworks
{
	internal class CallbackIdentities
	{
		public static int GetCallbackIdentity(global::System.Type callbackStruct)
		{
			object[] customAttributes = callbackStruct.GetCustomAttributes(typeof(global::Steamworks.CallbackIdentityAttribute), false);
			int num = 0;
			if (num >= customAttributes.Length)
			{
				throw new global::System.Exception("Callback number not found for struct " + callbackStruct);
			}
			global::Steamworks.CallbackIdentityAttribute callbackIdentityAttribute = (global::Steamworks.CallbackIdentityAttribute)customAttributes[num];
			return callbackIdentityAttribute.Identity;
		}
	}
}
