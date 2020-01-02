using System;
using UnityEngine;

public class VerletParticle
{
	public void Setup(global::UnityEngine.GameObject goParticle, global::UnityEngine.Vector3 vec3Gravity, float fTimeStep)
	{
		this.m_goParticle = goParticle;
		this.m_vec3OldPos = this.m_goParticle.transform.position;
		this.m_vec3Gravity = vec3Gravity;
		this.m_fTimeStep = fTimeStep;
	}

	public void Verlet()
	{
		global::UnityEngine.Vector3 vector = this.m_goParticle.transform.position;
		global::UnityEngine.Vector3 vec3OldPos = new global::UnityEngine.Vector3(vector.x, vector.y, vector.z);
		global::UnityEngine.Vector3 vec3OldPos2 = this.m_vec3OldPos;
		vector += vector - vec3OldPos2 + this.m_vec3Gravity * this.m_fTimeStep * this.m_fTimeStep;
		this.m_goParticle.transform.position = vector;
		this.m_vec3OldPos = vec3OldPos;
	}

	private global::UnityEngine.GameObject m_goParticle;

	private global::UnityEngine.Vector3 m_vec3OldPos;

	private global::UnityEngine.Vector3 m_vec3Gravity;

	private float m_fTimeStep;
}
