using System;
using System.Reflection;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffectFieldFlags
	{
		public VolumeEffectFieldFlags(global::System.Reflection.FieldInfo pi)
		{
			this.fieldName = pi.Name;
			this.fieldType = pi.FieldType.FullName;
		}

		public VolumeEffectFieldFlags(global::AmplifyColor.VolumeEffectField field)
		{
			this.fieldName = field.fieldName;
			this.fieldType = field.fieldType;
			this.blendFlag = true;
		}

		public string fieldName;

		public string fieldType;

		public bool blendFlag;
	}
}
