using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffectComponentFlags
	{
		public VolumeEffectComponentFlags(string name)
		{
			this.componentName = name;
			this.componentFields = new global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectFieldFlags>();
		}

		public VolumeEffectComponentFlags(global::AmplifyColor.VolumeEffectComponent comp) : this(comp.componentName)
		{
			this.blendFlag = true;
			foreach (global::AmplifyColor.VolumeEffectField volumeEffectField in comp.fields)
			{
				if (global::AmplifyColor.VolumeEffectField.IsValidType(volumeEffectField.fieldType))
				{
					this.componentFields.Add(new global::AmplifyColor.VolumeEffectFieldFlags(volumeEffectField));
				}
			}
		}

		public VolumeEffectComponentFlags(global::UnityEngine.Component c) : this(c.GetType() + string.Empty)
		{
			global::System.Reflection.FieldInfo[] fields = c.GetType().GetFields();
			foreach (global::System.Reflection.FieldInfo fieldInfo in fields)
			{
				if (global::AmplifyColor.VolumeEffectField.IsValidType(fieldInfo.FieldType.FullName))
				{
					this.componentFields.Add(new global::AmplifyColor.VolumeEffectFieldFlags(fieldInfo));
				}
			}
		}

		public void UpdateComponentFlags(global::AmplifyColor.VolumeEffectComponent comp)
		{
			global::AmplifyColor.VolumeEffectField field;
			foreach (global::AmplifyColor.VolumeEffectField field2 in comp.fields)
			{
				field = field2;
				if (this.componentFields.Find((global::AmplifyColor.VolumeEffectFieldFlags s) => s.fieldName == field.fieldName) == null && global::AmplifyColor.VolumeEffectField.IsValidType(field.fieldType))
				{
					this.componentFields.Add(new global::AmplifyColor.VolumeEffectFieldFlags(field));
				}
			}
		}

		public void UpdateComponentFlags(global::UnityEngine.Component c)
		{
			global::System.Reflection.FieldInfo[] fields = c.GetType().GetFields();
			foreach (global::System.Reflection.FieldInfo pi in fields)
			{
				if (!this.componentFields.Exists((global::AmplifyColor.VolumeEffectFieldFlags s) => s.fieldName == pi.Name) && global::AmplifyColor.VolumeEffectField.IsValidType(pi.FieldType.FullName))
				{
					this.componentFields.Add(new global::AmplifyColor.VolumeEffectFieldFlags(pi));
				}
			}
		}

		public string[] GetFieldNames()
		{
			return (from r in this.componentFields
			where r.blendFlag
			select r.fieldName).ToArray<string>();
		}

		public string componentName;

		public global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectFieldFlags> componentFields;

		public bool blendFlag;
	}
}
