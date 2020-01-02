using System;
using UnityEngine;

public class VerletStick
{
	public void Setup(global::UnityEngine.GameObject go1, global::UnityEngine.GameObject go2, float fStickLength, float fRelaxPercent, float fTimeStep, global::UnityEngine.Vector3 vec3Gravity)
	{
		this.m_goParticle1 = go1;
		this.m_goParticle2 = go2;
		this.m_fStickLength = fStickLength;
		this.m_fRelaxPercent = fRelaxPercent;
		this.m_verletParticle1 = new global::VerletParticle();
		this.m_verletParticle2 = new global::VerletParticle();
		this.m_verletParticle1.Setup(this.m_goParticle1, vec3Gravity, fTimeStep);
		this.m_verletParticle2.Setup(this.m_goParticle2, vec3Gravity, fTimeStep);
	}

	public void SatisfyConstraint()
	{
		global::UnityEngine.Vector3 vector = this.m_goParticle1.transform.position;
		global::UnityEngine.Vector3 vector2 = this.m_goParticle2.transform.position;
		this.ClampVector3(ref vector, -50f, 50f);
		this.ClampVector3(ref vector2, -50f, 50f);
		this.m_goParticle1.transform.position = vector;
		this.m_goParticle2.transform.position = vector2;
		global::UnityEngine.Vector3 a = vector - vector2;
		float magnitude = a.magnitude;
		if (magnitude <= 0f)
		{
			return;
		}
		float d = (magnitude - this.m_fStickLength) / magnitude;
		vector -= a * this.m_fRelaxPercent * d;
		vector2 += a * this.m_fRelaxPercent * d;
		this.m_goParticle1.transform.position = vector;
		this.m_goParticle2.transform.position = vector2;
	}

	public void Simulate()
	{
		this.m_verletParticle1.Verlet();
		this.m_verletParticle2.Verlet();
	}

	public void Reorient()
	{
		global::UnityEngine.Vector3 lookRotation = this.m_goParticle2.transform.position - this.m_goParticle1.transform.position;
		global::UnityEngine.Quaternion rotation = this.m_goParticle2.transform.rotation;
		rotation.SetLookRotation(lookRotation);
		this.m_goParticle2.transform.rotation = rotation;
	}

	private void ClampVector3(ref global::UnityEngine.Vector3 vec, float min, float max)
	{
		if (vec.y < min)
		{
			vec.y = min;
		}
		else if (vec.y > max)
		{
			vec.y = max;
		}
		vec.z = 0f;
	}

	private global::UnityEngine.GameObject m_goParticle1;

	private global::UnityEngine.GameObject m_goParticle2;

	private float m_fStickLength;

	private float m_fRelaxPercent;

	private global::VerletParticle m_verletParticle1;

	private global::VerletParticle m_verletParticle2;
}
