using System;
using System.Collections;
using System.Reflection;

namespace WellFired.Shared
{
	public class ReflectionHelper : global::WellFired.Shared.IReflectionHelper
	{
		public bool IsAssignableFrom(global::System.Type first, global::System.Type second)
		{
			return first.IsAssignableFrom(second);
		}

		public bool IsEnum(global::System.Type type)
		{
			return type.IsEnum;
		}

		private global::System.Collections.IEnumerable GetBaseTypes(global::System.Type type)
		{
			yield return type;
			global::System.Type baseType = type.BaseType;
			if (baseType != null)
			{
				foreach (object t in this.GetBaseTypes(baseType))
				{
					yield return t;
				}
			}
			yield break;
		}

		public global::System.Reflection.PropertyInfo GetProperty(global::System.Type type, string name)
		{
			return type.GetProperty(name);
		}

		public global::System.Reflection.PropertyInfo GetNonPublicInstanceProperty(global::System.Type type, string name)
		{
			return type.GetProperty(name, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic);
		}

		public global::System.Reflection.MethodInfo GetMethod(global::System.Type type, string name)
		{
			return type.GetMethod(name);
		}

		public global::System.Reflection.MethodInfo GetNonPublicStaticMethod(global::System.Type type, string name)
		{
			return type.GetMethod(name, global::System.Reflection.BindingFlags.Static | global::System.Reflection.BindingFlags.NonPublic);
		}

		public global::System.Reflection.FieldInfo GetField(global::System.Type type, string name)
		{
			return type.GetField(name);
		}

		public global::System.Reflection.FieldInfo GetNonPublicInstanceField(global::System.Type type, string name)
		{
			return type.GetField(name, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.NonPublic);
		}

		public bool IsValueType(global::System.Type type)
		{
			return type.IsValueType;
		}
	}
}
