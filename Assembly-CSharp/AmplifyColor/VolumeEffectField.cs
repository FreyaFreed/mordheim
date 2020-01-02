using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffectField
	{
		public VolumeEffectField(string fieldName, string fieldType)
		{
			this.fieldName = fieldName;
			this.fieldType = fieldType;
		}

		public VolumeEffectField(global::System.Reflection.FieldInfo pi, global::UnityEngine.Component c) : this(pi.Name, pi.FieldType.FullName)
		{
			object value = pi.GetValue(c);
			this.UpdateValue(value);
		}

		public static bool IsValidType(string type)
		{
			if (type != null)
			{
				if (global::AmplifyColor.VolumeEffectField.<>f__switch$map0 == null)
				{
					global::AmplifyColor.VolumeEffectField.<>f__switch$map0 = new global::System.Collections.Generic.Dictionary<string, int>(6)
					{
						{
							"System.Single",
							0
						},
						{
							"System.Boolean",
							0
						},
						{
							"UnityEngine.Color",
							0
						},
						{
							"UnityEngine.Vector2",
							0
						},
						{
							"UnityEngine.Vector3",
							0
						},
						{
							"UnityEngine.Vector4",
							0
						}
					};
				}
				int num;
				if (global::AmplifyColor.VolumeEffectField.<>f__switch$map0.TryGetValue(type, out num))
				{
					if (num == 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void UpdateValue(object val)
		{
			string text = this.fieldType;
			switch (text)
			{
			case "System.Single":
				this.valueSingle = (float)val;
				break;
			case "System.Boolean":
				this.valueBoolean = (bool)val;
				break;
			case "UnityEngine.Color":
				this.valueColor = (global::UnityEngine.Color)val;
				break;
			case "UnityEngine.Vector2":
				this.valueVector2 = (global::UnityEngine.Vector2)val;
				break;
			case "UnityEngine.Vector3":
				this.valueVector3 = (global::UnityEngine.Vector3)val;
				break;
			case "UnityEngine.Vector4":
				this.valueVector4 = (global::UnityEngine.Vector4)val;
				break;
			}
		}

		public string fieldName;

		public string fieldType;

		public float valueSingle;

		public global::UnityEngine.Color valueColor;

		public bool valueBoolean;

		public global::UnityEngine.Vector2 valueVector2;

		public global::UnityEngine.Vector3 valueVector3;

		public global::UnityEngine.Vector4 valueVector4;
	}
}
