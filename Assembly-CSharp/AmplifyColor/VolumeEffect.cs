using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AmplifyColor
{
	[global::System.Serializable]
	public class VolumeEffect
	{
		public VolumeEffect(global::AmplifyColorBase effect)
		{
			this.gameObject = effect;
			this.components = new global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectComponent>();
		}

		public static global::AmplifyColor.VolumeEffect BlendValuesToVolumeEffect(global::AmplifyColor.VolumeEffectFlags flags, global::AmplifyColor.VolumeEffect volume1, global::AmplifyColor.VolumeEffect volume2, float blend)
		{
			global::AmplifyColor.VolumeEffect volumeEffect = new global::AmplifyColor.VolumeEffect(volume1.gameObject);
			foreach (global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags in flags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					global::AmplifyColor.VolumeEffectComponent volumeEffectComponent = volume1.FindEffectComponent(volumeEffectComponentFlags.componentName);
					global::AmplifyColor.VolumeEffectComponent volumeEffectComponent2 = volume2.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (volumeEffectComponent != null && volumeEffectComponent2 != null)
					{
						global::AmplifyColor.VolumeEffectComponent volumeEffectComponent3 = new global::AmplifyColor.VolumeEffectComponent(volumeEffectComponent.componentName);
						foreach (global::AmplifyColor.VolumeEffectFieldFlags volumeEffectFieldFlags in volumeEffectComponentFlags.componentFields)
						{
							if (volumeEffectFieldFlags.blendFlag)
							{
								global::AmplifyColor.VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								global::AmplifyColor.VolumeEffectField volumeEffectField2 = volumeEffectComponent2.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (volumeEffectField != null && volumeEffectField2 != null)
								{
									global::AmplifyColor.VolumeEffectField volumeEffectField3 = new global::AmplifyColor.VolumeEffectField(volumeEffectField.fieldName, volumeEffectField.fieldType);
									string fieldType = volumeEffectField3.fieldType;
									switch (fieldType)
									{
									case "System.Single":
										volumeEffectField3.valueSingle = global::UnityEngine.Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blend);
										break;
									case "System.Boolean":
										volumeEffectField3.valueBoolean = volumeEffectField2.valueBoolean;
										break;
									case "UnityEngine.Vector2":
										volumeEffectField3.valueVector2 = global::UnityEngine.Vector2.Lerp(volumeEffectField.valueVector2, volumeEffectField2.valueVector2, blend);
										break;
									case "UnityEngine.Vector3":
										volumeEffectField3.valueVector3 = global::UnityEngine.Vector3.Lerp(volumeEffectField.valueVector3, volumeEffectField2.valueVector3, blend);
										break;
									case "UnityEngine.Vector4":
										volumeEffectField3.valueVector4 = global::UnityEngine.Vector4.Lerp(volumeEffectField.valueVector4, volumeEffectField2.valueVector4, blend);
										break;
									case "UnityEngine.Color":
										volumeEffectField3.valueColor = global::UnityEngine.Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blend);
										break;
									}
									volumeEffectComponent3.fields.Add(volumeEffectField3);
								}
							}
						}
						volumeEffect.components.Add(volumeEffectComponent3);
					}
				}
			}
			return volumeEffect;
		}

		public global::AmplifyColor.VolumeEffectComponent AddComponent(global::UnityEngine.Component c, global::AmplifyColor.VolumeEffectComponentFlags compFlags)
		{
			if (compFlags == null)
			{
				global::AmplifyColor.VolumeEffectComponent volumeEffectComponent = new global::AmplifyColor.VolumeEffectComponent(c.GetType() + string.Empty);
				this.components.Add(volumeEffectComponent);
				return volumeEffectComponent;
			}
			global::AmplifyColor.VolumeEffectComponent volumeEffectComponent2;
			if ((volumeEffectComponent2 = this.FindEffectComponent(c.GetType() + string.Empty)) != null)
			{
				volumeEffectComponent2.UpdateComponent(c, compFlags);
				return volumeEffectComponent2;
			}
			global::AmplifyColor.VolumeEffectComponent volumeEffectComponent3 = new global::AmplifyColor.VolumeEffectComponent(c, compFlags);
			this.components.Add(volumeEffectComponent3);
			return volumeEffectComponent3;
		}

		public void RemoveEffectComponent(global::AmplifyColor.VolumeEffectComponent comp)
		{
			this.components.Remove(comp);
		}

		public void UpdateVolume()
		{
			if (this.gameObject == null)
			{
				return;
			}
			global::AmplifyColor.VolumeEffectFlags effectFlags = this.gameObject.EffectFlags;
			foreach (global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags in effectFlags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					global::UnityEngine.Component component = this.gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					if (component != null)
					{
						this.AddComponent(component, volumeEffectComponentFlags);
					}
				}
			}
		}

		public void SetValues(global::AmplifyColorBase targetColor)
		{
			global::AmplifyColor.VolumeEffectFlags effectFlags = targetColor.EffectFlags;
			global::UnityEngine.GameObject gameObject = targetColor.gameObject;
			foreach (global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags in effectFlags.components)
			{
				if (volumeEffectComponentFlags.blendFlag)
				{
					global::UnityEngine.Component component = gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					global::AmplifyColor.VolumeEffectComponent volumeEffectComponent = this.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (!(component == null) && volumeEffectComponent != null)
					{
						foreach (global::AmplifyColor.VolumeEffectFieldFlags volumeEffectFieldFlags in volumeEffectComponentFlags.componentFields)
						{
							if (volumeEffectFieldFlags.blendFlag)
							{
								global::System.Reflection.FieldInfo field = component.GetType().GetField(volumeEffectFieldFlags.fieldName);
								global::AmplifyColor.VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (field != null && volumeEffectField != null)
								{
									string fullName = field.FieldType.FullName;
									switch (fullName)
									{
									case "System.Single":
										field.SetValue(component, volumeEffectField.valueSingle);
										break;
									case "System.Boolean":
										field.SetValue(component, volumeEffectField.valueBoolean);
										break;
									case "UnityEngine.Vector2":
										field.SetValue(component, volumeEffectField.valueVector2);
										break;
									case "UnityEngine.Vector3":
										field.SetValue(component, volumeEffectField.valueVector3);
										break;
									case "UnityEngine.Vector4":
										field.SetValue(component, volumeEffectField.valueVector4);
										break;
									case "UnityEngine.Color":
										field.SetValue(component, volumeEffectField.valueColor);
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		public void BlendValues(global::AmplifyColorBase targetColor, global::AmplifyColor.VolumeEffect other, float blendAmount)
		{
			global::AmplifyColor.VolumeEffectFlags effectFlags = targetColor.EffectFlags;
			global::UnityEngine.GameObject gameObject = targetColor.gameObject;
			for (int i = 0; i < effectFlags.components.Count; i++)
			{
				global::AmplifyColor.VolumeEffectComponentFlags volumeEffectComponentFlags = effectFlags.components[i];
				if (volumeEffectComponentFlags.blendFlag)
				{
					global::UnityEngine.Component component = gameObject.GetComponent(volumeEffectComponentFlags.componentName);
					global::AmplifyColor.VolumeEffectComponent volumeEffectComponent = this.FindEffectComponent(volumeEffectComponentFlags.componentName);
					global::AmplifyColor.VolumeEffectComponent volumeEffectComponent2 = other.FindEffectComponent(volumeEffectComponentFlags.componentName);
					if (!(component == null) && volumeEffectComponent != null && volumeEffectComponent2 != null)
					{
						for (int j = 0; j < volumeEffectComponentFlags.componentFields.Count; j++)
						{
							global::AmplifyColor.VolumeEffectFieldFlags volumeEffectFieldFlags = volumeEffectComponentFlags.componentFields[j];
							if (volumeEffectFieldFlags.blendFlag)
							{
								global::System.Reflection.FieldInfo field = component.GetType().GetField(volumeEffectFieldFlags.fieldName);
								global::AmplifyColor.VolumeEffectField volumeEffectField = volumeEffectComponent.FindEffectField(volumeEffectFieldFlags.fieldName);
								global::AmplifyColor.VolumeEffectField volumeEffectField2 = volumeEffectComponent2.FindEffectField(volumeEffectFieldFlags.fieldName);
								if (field != null && volumeEffectField != null && volumeEffectField2 != null)
								{
									string fullName = field.FieldType.FullName;
									switch (fullName)
									{
									case "System.Single":
										field.SetValue(component, global::UnityEngine.Mathf.Lerp(volumeEffectField.valueSingle, volumeEffectField2.valueSingle, blendAmount));
										break;
									case "System.Boolean":
										field.SetValue(component, volumeEffectField2.valueBoolean);
										break;
									case "UnityEngine.Vector2":
										field.SetValue(component, global::UnityEngine.Vector2.Lerp(volumeEffectField.valueVector2, volumeEffectField2.valueVector2, blendAmount));
										break;
									case "UnityEngine.Vector3":
										field.SetValue(component, global::UnityEngine.Vector3.Lerp(volumeEffectField.valueVector3, volumeEffectField2.valueVector3, blendAmount));
										break;
									case "UnityEngine.Vector4":
										field.SetValue(component, global::UnityEngine.Vector4.Lerp(volumeEffectField.valueVector4, volumeEffectField2.valueVector4, blendAmount));
										break;
									case "UnityEngine.Color":
										field.SetValue(component, global::UnityEngine.Color.Lerp(volumeEffectField.valueColor, volumeEffectField2.valueColor, blendAmount));
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		public global::AmplifyColor.VolumeEffectComponent FindEffectComponent(string compName)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i].componentName == compName)
				{
					return this.components[i];
				}
			}
			return null;
		}

		public static global::UnityEngine.Component[] ListAcceptableComponents(global::AmplifyColorBase go)
		{
			if (go == null)
			{
				return new global::UnityEngine.Component[0];
			}
			global::UnityEngine.Component[] source = go.GetComponents(typeof(global::UnityEngine.Component));
			return (from comp in source
			where comp != null && (!(comp.GetType() + string.Empty).StartsWith("UnityEngine.") && comp.GetType() != typeof(global::AmplifyColorBase))
			select comp).ToArray<global::UnityEngine.Component>();
		}

		public string[] GetComponentNames()
		{
			return (from r in this.components
			select r.componentName).ToArray<string>();
		}

		public global::AmplifyColorBase gameObject;

		public global::System.Collections.Generic.List<global::AmplifyColor.VolumeEffectComponent> components;
	}
}
