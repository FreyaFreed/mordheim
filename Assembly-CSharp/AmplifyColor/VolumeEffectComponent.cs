using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffectComponent
	{
		public VolumeEffectComponent(string name)
		{
			this.componentName = name;
			this.fields = new global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectField>();
		}

		public VolumeEffectComponent(global::UnityEngine.Component c, global::AmplifyColor.VolumeEffectComponentFlags compFlags) : this(compFlags.componentName)
		{
			foreach (global::AmplifyColor.VolumeEffectFieldFlags volumeEffectFieldFlags in compFlags.componentFields)
			{
				if (volumeEffectFieldFlags.blendFlag)
				{
					global::System.Reflection.FieldInfo field = c.GetType().GetField(volumeEffectFieldFlags.fieldName);
					global::AmplifyColor.VolumeEffectField volumeEffectField = (!global::AmplifyColor.VolumeEffectField.IsValidType(field.FieldType.FullName)) ? null : new global::AmplifyColor.VolumeEffectField(field, c);
					if (volumeEffectField != null)
					{
						this.fields.Add(volumeEffectField);
					}
				}
			}
		}

		public global::AmplifyColor.VolumeEffectField AddField(global::System.Reflection.FieldInfo pi, global::UnityEngine.Component c)
		{
			return this.AddField(pi, c, -1);
		}

		public global::AmplifyColor.VolumeEffectField AddField(global::System.Reflection.FieldInfo pi, global::UnityEngine.Component c, int position)
		{
			global::AmplifyColor.VolumeEffectField volumeEffectField = (!global::AmplifyColor.VolumeEffectField.IsValidType(pi.FieldType.FullName)) ? null : new global::AmplifyColor.VolumeEffectField(pi, c);
			if (volumeEffectField != null)
			{
				if (position < 0 || position >= this.fields.Count)
				{
					this.fields.Add(volumeEffectField);
				}
				else
				{
					this.fields.Insert(position, volumeEffectField);
				}
			}
			return volumeEffectField;
		}

		public void RemoveEffectField(global::AmplifyColor.VolumeEffectField field)
		{
			this.fields.Remove(field);
		}

		public void UpdateComponent(global::UnityEngine.Component c, global::AmplifyColor.VolumeEffectComponentFlags compFlags)
		{
			global::AmplifyColor.VolumeEffectFieldFlags fieldFlags;
			foreach (global::AmplifyColor.VolumeEffectFieldFlags fieldFlags2 in compFlags.componentFields)
			{
				fieldFlags = fieldFlags2;
				if (fieldFlags.blendFlag)
				{
					if (!this.fields.Exists((global::AmplifyColor.VolumeEffectField s) => s.fieldName == fieldFlags.fieldName))
					{
						global::System.Reflection.FieldInfo field = c.GetType().GetField(fieldFlags.fieldName);
						global::AmplifyColor.VolumeEffectField volumeEffectField = (!global::AmplifyColor.VolumeEffectField.IsValidType(field.FieldType.FullName)) ? null : new global::AmplifyColor.VolumeEffectField(field, c);
						if (volumeEffectField != null)
						{
							this.fields.Add(volumeEffectField);
						}
					}
				}
			}
		}

		public global::AmplifyColor.VolumeEffectField FindEffectField(string fieldName)
		{
			for (int i = 0; i < this.fields.Count; i++)
			{
				if (this.fields[i].fieldName == fieldName)
				{
					return this.fields[i];
				}
			}
			return null;
		}

		public static global::System.Reflection.FieldInfo[] ListAcceptableFields(global::UnityEngine.Component c)
		{
			if (c == null)
			{
				return new global::System.Reflection.FieldInfo[0];
			}
			global::System.Reflection.FieldInfo[] source = c.GetType().GetFields();
			return (from f in source
			where global::AmplifyColor.VolumeEffectField.IsValidType(f.FieldType.FullName)
			select f).ToArray<global::System.Reflection.FieldInfo>();
		}

		public string[] GetFieldNames()
		{
			return (from r in this.fields
			select r.fieldName).ToArray<string>();
		}

		public string componentName;

		public global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectField> fields;
	}
}
