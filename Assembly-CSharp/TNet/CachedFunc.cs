using System;
using System.Reflection;

namespace TNet
{
	public struct CachedFunc
	{
		public byte id;

		public object obj;

		public global::System.Reflection.MethodInfo func;

		public global::System.Reflection.ParameterInfo[] parameters;
	}
}
