using System;
using System.Reflection;
using System.Runtime.Serialization;

public sealed class VersionDeserializationBinderEdge : global::System.Runtime.Serialization.SerializationBinder
{
	public override global::System.Type BindToType(string assemblyName, string typeName)
	{
		if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
		{
			assemblyName = global::System.Reflection.Assembly.GetExecutingAssembly().FullName;
			return global::System.Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
		}
		return null;
	}
}
