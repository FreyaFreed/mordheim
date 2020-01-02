using System;
using System.Collections.Generic;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Dynamic Bone/Dynamic Bone")]
public class DynamicBone : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.SetupParticles();
	}

	private void Update()
	{
		if (this.m_Weight > 0f)
		{
			this.InitTransforms();
		}
	}

	private void LateUpdate()
	{
		if (this.m_Weight > 0f)
		{
			this.UpdateDynamicBones(global::UnityEngine.Time.deltaTime);
		}
	}

	private void OnEnable()
	{
		this.ResetParticlesPosition();
		this.m_ObjectPrevPosition = base.transform.position;
	}

	private void OnDisable()
	{
		this.InitTransforms();
	}

	private void OnValidate()
	{
		this.m_UpdateRate = global::UnityEngine.Mathf.Max(this.m_UpdateRate, 0f);
		this.m_Damping = global::UnityEngine.Mathf.Clamp01(this.m_Damping);
		this.m_Elasticity = global::UnityEngine.Mathf.Clamp01(this.m_Elasticity);
		this.m_Stiffness = global::UnityEngine.Mathf.Clamp01(this.m_Stiffness);
		this.m_Inert = global::UnityEngine.Mathf.Clamp01(this.m_Inert);
		this.m_Radius = global::UnityEngine.Mathf.Max(this.m_Radius, 0f);
		if (global::UnityEngine.Application.isEditor && global::UnityEngine.Application.isPlaying)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!base.enabled || this.m_Root == null)
		{
			return;
		}
		if (global::UnityEngine.Application.isEditor && !global::UnityEngine.Application.isPlaying && base.transform.hasChanged)
		{
			this.InitTransforms();
			this.SetupParticles();
		}
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.white;
		foreach (global::DynamicBone.Particle particle in this.m_Particles)
		{
			if (particle.m_ParentIndex >= 0)
			{
				global::DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				global::UnityEngine.Gizmos.DrawLine(particle.m_Position, particle2.m_Position);
			}
			if (particle.m_Radius > 0f)
			{
				global::UnityEngine.Gizmos.DrawWireSphere(particle.m_Position, particle.m_Radius * this.m_ObjectScale);
			}
		}
	}

	public void SetWeight(float w)
	{
		if (this.m_Weight != w)
		{
			if (w == 0f)
			{
				this.InitTransforms();
			}
			else if (this.m_Weight == 0f)
			{
				this.ResetParticlesPosition();
				this.m_ObjectPrevPosition = base.transform.position;
			}
			this.m_Weight = w;
		}
	}

	public float GetWeight()
	{
		return this.m_Weight;
	}

	private void UpdateDynamicBones(float t)
	{
		if (this.m_Root == null)
		{
			return;
		}
		this.m_ObjectScale = global::UnityEngine.Mathf.Abs(base.transform.lossyScale.x);
		this.m_ObjectMove = base.transform.position - this.m_ObjectPrevPosition;
		this.m_ObjectPrevPosition = base.transform.position;
		int num = 1;
		if (this.m_UpdateRate > 0f)
		{
			float num2 = 1f / this.m_UpdateRate;
			this.m_Time += t;
			num = 0;
			while (this.m_Time >= num2)
			{
				this.m_Time -= num2;
				if (++num >= 3)
				{
					this.m_Time = 0f;
					break;
				}
			}
		}
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				this.UpdateParticles1();
				this.UpdateParticles2();
				this.m_ObjectMove = global::UnityEngine.Vector3.zero;
			}
		}
		else
		{
			this.SkipUpdateParticles();
		}
		this.ApplyParticlesToTransforms();
	}

	private void SetupParticles()
	{
		this.m_Particles.Clear();
		if (this.m_Root == null)
		{
			return;
		}
		this.m_LocalGravity = this.m_Root.InverseTransformDirection(this.m_Gravity);
		this.m_ObjectScale = base.transform.lossyScale.x;
		this.m_ObjectPrevPosition = base.transform.position;
		this.m_ObjectMove = global::UnityEngine.Vector3.zero;
		this.m_BoneTotalLength = 0f;
		this.AppendParticles(this.m_Root, -1, 0f);
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			global::DynamicBone.Particle particle = this.m_Particles[i];
			particle.m_Damping = this.m_Damping;
			particle.m_Elasticity = this.m_Elasticity;
			particle.m_Stiffness = this.m_Stiffness;
			particle.m_Inert = this.m_Inert;
			particle.m_Radius = this.m_Radius;
			if (this.m_BoneTotalLength > 0f)
			{
				float time = particle.m_BoneLength / this.m_BoneTotalLength;
				if (this.m_DampingDistrib.keys.Length > 0)
				{
					particle.m_Damping *= this.m_DampingDistrib.Evaluate(time);
				}
				if (this.m_ElasticityDistrib.keys.Length > 0)
				{
					particle.m_Elasticity *= this.m_ElasticityDistrib.Evaluate(time);
				}
				if (this.m_StiffnessDistrib.keys.Length > 0)
				{
					particle.m_Stiffness *= this.m_StiffnessDistrib.Evaluate(time);
				}
				if (this.m_InertDistrib.keys.Length > 0)
				{
					particle.m_Inert *= this.m_InertDistrib.Evaluate(time);
				}
				if (this.m_RadiusDistrib.keys.Length > 0)
				{
					particle.m_Radius *= this.m_RadiusDistrib.Evaluate(time);
				}
			}
			particle.m_Damping = global::UnityEngine.Mathf.Clamp01(particle.m_Damping);
			particle.m_Elasticity = global::UnityEngine.Mathf.Clamp01(particle.m_Elasticity);
			particle.m_Stiffness = global::UnityEngine.Mathf.Clamp01(particle.m_Stiffness);
			particle.m_Inert = global::UnityEngine.Mathf.Clamp01(particle.m_Inert);
			particle.m_Radius = global::UnityEngine.Mathf.Max(particle.m_Radius, 0f);
		}
	}

	private void AppendParticles(global::UnityEngine.Transform b, int parentIndex, float boneLength)
	{
		global::DynamicBone.Particle particle = new global::DynamicBone.Particle();
		particle.m_Transform = b;
		particle.m_ParentIndex = parentIndex;
		if (b != null)
		{
			particle.m_Position = (particle.m_PrevPosition = b.position);
			particle.m_InitLocalPosition = b.localPosition;
			particle.m_InitLocalRotation = b.localRotation;
		}
		else
		{
			global::UnityEngine.Transform transform = this.m_Particles[parentIndex].m_Transform;
			if (this.m_EndLength > 0f)
			{
				global::UnityEngine.Transform parent = transform.parent;
				if (parent != null)
				{
					particle.m_EndOffset = transform.InverseTransformPoint(transform.position * 2f - parent.position) * this.m_EndLength;
				}
				else
				{
					particle.m_EndOffset = new global::UnityEngine.Vector3(this.m_EndLength, 0f, 0f);
				}
			}
			else
			{
				particle.m_EndOffset = transform.InverseTransformPoint(base.transform.TransformDirection(this.m_EndOffset) + transform.position);
			}
			particle.m_Position = (particle.m_PrevPosition = transform.TransformPoint(particle.m_EndOffset));
		}
		if (parentIndex >= 0)
		{
			boneLength += (this.m_Particles[parentIndex].m_Transform.position - particle.m_Position).magnitude;
			particle.m_BoneLength = boneLength;
			this.m_BoneTotalLength = global::UnityEngine.Mathf.Max(this.m_BoneTotalLength, boneLength);
		}
		int count = this.m_Particles.Count;
		this.m_Particles.Add(particle);
		if (b != null)
		{
			for (int i = 0; i < b.childCount; i++)
			{
				bool flag = false;
				for (int j = 0; j < this.m_Exclusions.Count; j++)
				{
					if (this.m_Exclusions[j] == b.GetChild(i))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.AppendParticles(b.GetChild(i), count, boneLength);
				}
			}
			if (b.childCount == 0 && (this.m_EndLength > 0f || this.m_EndOffset != global::UnityEngine.Vector3.zero))
			{
				this.AppendParticles(null, count, boneLength);
			}
		}
	}

	private void InitTransforms()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			if (this.m_Particles[i].m_Transform != null)
			{
				this.m_Particles[i].m_Transform.localPosition = this.m_Particles[i].m_InitLocalPosition;
				this.m_Particles[i].m_Transform.localRotation = this.m_Particles[i].m_InitLocalRotation;
			}
		}
	}

	private void ResetParticlesPosition()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			if (this.m_Particles[i].m_Transform != null)
			{
				this.m_Particles[i].m_Position = (this.m_Particles[i].m_PrevPosition = this.m_Particles[i].m_Transform.position);
			}
			else
			{
				global::UnityEngine.Transform transform = this.m_Particles[this.m_Particles[i].m_ParentIndex].m_Transform;
				this.m_Particles[i].m_Position = (this.m_Particles[i].m_PrevPosition = transform.TransformPoint(this.m_Particles[i].m_EndOffset));
			}
		}
	}

	private void UpdateParticles1()
	{
		global::UnityEngine.Vector3 vector = this.m_Gravity;
		global::UnityEngine.Vector3 normalized = this.m_Gravity.normalized;
		global::UnityEngine.Vector3 lhs = this.m_Root.TransformDirection(this.m_LocalGravity);
		global::UnityEngine.Vector3 b = normalized * global::UnityEngine.Mathf.Max(global::UnityEngine.Vector3.Dot(lhs, normalized), 0f);
		vector -= b;
		vector = (vector + this.m_Force) * this.m_ObjectScale;
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			global::DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				global::UnityEngine.Vector3 a = particle.m_Position - particle.m_PrevPosition;
				global::UnityEngine.Vector3 b2 = this.m_ObjectMove * particle.m_Inert;
				particle.m_PrevPosition = particle.m_Position + b2;
				particle.m_Position += a * (1f - particle.m_Damping) + vector + b2;
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	private void UpdateParticles2()
	{
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			global::DynamicBone.Particle particle = this.m_Particles[i];
			global::DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			float num;
			if (particle.m_Transform != null)
			{
				num = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
			}
			else
			{
				num = particle.m_EndOffset.magnitude * this.m_ObjectScale;
			}
			float num2 = global::UnityEngine.Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
			if (num2 > 0f || particle.m_Elasticity > 0f)
			{
				global::UnityEngine.Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
				localToWorldMatrix.SetColumn(3, particle2.m_Position);
				global::UnityEngine.Vector3 a;
				if (particle.m_Transform != null)
				{
					a = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
				}
				else
				{
					a = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
				}
				global::UnityEngine.Vector3 a2 = a - particle.m_Position;
				particle.m_Position += a2 * particle.m_Elasticity;
				if (num2 > 0f)
				{
					a2 = a - particle.m_Position;
					float magnitude = a2.magnitude;
					float num3 = num * (1f - num2) * 2f;
					if (magnitude > num3)
					{
						particle.m_Position += a2 * ((magnitude - num3) / magnitude);
					}
				}
			}
			float particleRadius = particle.m_Radius * this.m_ObjectScale;
			for (int j = 0; j < this.m_Colliders.Count; j++)
			{
				global::DynamicBoneCollider dynamicBoneCollider = this.m_Colliders[j];
				if (dynamicBoneCollider != null && dynamicBoneCollider.enabled)
				{
					dynamicBoneCollider.Collide(ref particle.m_Position, particleRadius);
				}
			}
			global::UnityEngine.Vector3 a3 = particle2.m_Position - particle.m_Position;
			float magnitude2 = a3.magnitude;
			if (magnitude2 > 0f)
			{
				particle.m_Position += a3 * ((magnitude2 - num) / magnitude2);
			}
		}
	}

	private void SkipUpdateParticles()
	{
		for (int i = 0; i < this.m_Particles.Count; i++)
		{
			global::DynamicBone.Particle particle = this.m_Particles[i];
			if (particle.m_ParentIndex >= 0)
			{
				global::UnityEngine.Vector3 b = this.m_ObjectMove * particle.m_Inert;
				particle.m_PrevPosition += b;
				particle.m_Position += b;
				global::DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
				float num;
				if (particle.m_Transform != null)
				{
					num = (particle2.m_Transform.position - particle.m_Transform.position).magnitude;
				}
				else
				{
					num = particle.m_EndOffset.magnitude * this.m_ObjectScale;
				}
				float num2 = global::UnityEngine.Mathf.Lerp(1f, particle.m_Stiffness, this.m_Weight);
				if (num2 > 0f)
				{
					global::UnityEngine.Matrix4x4 localToWorldMatrix = particle2.m_Transform.localToWorldMatrix;
					localToWorldMatrix.SetColumn(3, particle2.m_Position);
					global::UnityEngine.Vector3 a;
					if (particle.m_Transform != null)
					{
						a = localToWorldMatrix.MultiplyPoint3x4(particle.m_Transform.localPosition);
					}
					else
					{
						a = localToWorldMatrix.MultiplyPoint3x4(particle.m_EndOffset);
					}
					global::UnityEngine.Vector3 a2 = a - particle.m_Position;
					float magnitude = a2.magnitude;
					float num3 = num * (1f - num2) * 2f;
					if (magnitude > num3)
					{
						particle.m_Position += a2 * ((magnitude - num3) / magnitude);
					}
				}
				global::UnityEngine.Vector3 a3 = particle2.m_Position - particle.m_Position;
				float magnitude2 = a3.magnitude;
				if (magnitude2 > 0f)
				{
					particle.m_Position += a3 * ((magnitude2 - num) / magnitude2);
				}
			}
			else
			{
				particle.m_PrevPosition = particle.m_Position;
				particle.m_Position = particle.m_Transform.position;
			}
		}
	}

	private void ApplyParticlesToTransforms()
	{
		for (int i = 1; i < this.m_Particles.Count; i++)
		{
			global::DynamicBone.Particle particle = this.m_Particles[i];
			global::DynamicBone.Particle particle2 = this.m_Particles[particle.m_ParentIndex];
			if (particle2.m_Transform.childCount <= 1)
			{
				global::UnityEngine.Vector3 direction;
				if (particle.m_Transform != null)
				{
					direction = particle.m_Transform.localPosition;
				}
				else
				{
					direction = particle.m_EndOffset;
				}
				global::UnityEngine.Quaternion lhs = global::UnityEngine.Quaternion.FromToRotation(particle2.m_Transform.TransformDirection(direction), particle.m_Position - particle2.m_Position);
				particle2.m_Transform.rotation = lhs * particle2.m_Transform.rotation;
			}
			if (particle.m_Transform)
			{
				particle.m_Transform.position = particle.m_Position;
			}
		}
	}

	public global::UnityEngine.Transform m_Root;

	public float m_UpdateRate = 60f;

	[global::UnityEngine.Range(0f, 1f)]
	public float m_Damping = 0.1f;

	public global::UnityEngine.AnimationCurve m_DampingDistrib;

	[global::UnityEngine.Range(0f, 1f)]
	public float m_Elasticity = 0.1f;

	public global::UnityEngine.AnimationCurve m_ElasticityDistrib;

	[global::UnityEngine.Range(0f, 1f)]
	public float m_Stiffness = 0.1f;

	public global::UnityEngine.AnimationCurve m_StiffnessDistrib;

	[global::UnityEngine.Range(0f, 1f)]
	public float m_Inert;

	public global::UnityEngine.AnimationCurve m_InertDistrib;

	public float m_Radius;

	public global::UnityEngine.AnimationCurve m_RadiusDistrib;

	public float m_EndLength;

	public global::UnityEngine.Vector3 m_EndOffset = global::UnityEngine.Vector3.zero;

	public global::UnityEngine.Vector3 m_Gravity = global::UnityEngine.Vector3.zero;

	public global::UnityEngine.Vector3 m_Force = global::UnityEngine.Vector3.zero;

	public global::System.Collections.Generic.List<global::DynamicBoneCollider> m_Colliders;

	public global::System.Collections.Generic.List<global::UnityEngine.Transform> m_Exclusions;

	private global::UnityEngine.Vector3 m_LocalGravity = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 m_ObjectMove = global::UnityEngine.Vector3.zero;

	private global::UnityEngine.Vector3 m_ObjectPrevPosition = global::UnityEngine.Vector3.zero;

	private float m_BoneTotalLength;

	private float m_ObjectScale = 1f;

	private float m_Time;

	private float m_Weight = 1f;

	private global::System.Collections.Generic.List<global::DynamicBone.Particle> m_Particles = new global::System.Collections.Generic.List<global::DynamicBone.Particle>();

	private class Particle
	{
		public global::UnityEngine.Transform m_Transform;

		public int m_ParentIndex = -1;

		public float m_Damping;

		public float m_Elasticity;

		public float m_Stiffness;

		public float m_Inert;

		public float m_Radius;

		public float m_BoneLength;

		public global::UnityEngine.Vector3 m_Position = global::UnityEngine.Vector3.zero;

		public global::UnityEngine.Vector3 m_PrevPosition = global::UnityEngine.Vector3.zero;

		public global::UnityEngine.Vector3 m_EndOffset = global::UnityEngine.Vector3.zero;

		public global::UnityEngine.Vector3 m_InitLocalPosition = global::UnityEngine.Vector3.zero;

		public global::UnityEngine.Quaternion m_InitLocalRotation = global::UnityEngine.Quaternion.identity;
	}
}
