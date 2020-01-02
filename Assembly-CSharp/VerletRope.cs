using System;
using System.Collections.Generic;
using UnityEngine;

public class VerletRope : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.m_listRopeParticles = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
		this.m_listSticks = new global::System.Collections.Generic.List<global::VerletStick>();
		global::UnityEngine.Vector3 position = this.HookedPosition.position;
		this.m_iNumSticks = this.m_iNumParticles - 1U;
		global::UnityEngine.GameObject original = global::UnityEngine.GameObject.Find("RopeParticle");
		int num = 0;
		while ((long)num < (long)((ulong)this.m_iNumParticles))
		{
			global::UnityEngine.Object @object = global::UnityEngine.Object.Instantiate(original, position, this.HookedPosition.rotation);
			position.y -= this.m_fParticleRadius;
			this.m_listRopeParticles.Add((global::UnityEngine.GameObject)@object);
			num++;
		}
		int num2 = 1;
		while ((long)num2 < (long)((ulong)this.m_iNumParticles))
		{
			global::VerletStick verletStick = new global::VerletStick();
			global::UnityEngine.GameObject go = this.m_listRopeParticles[num2 - 1];
			global::UnityEngine.GameObject go2 = this.m_listRopeParticles[num2];
			verletStick.Setup(go, go2, this.m_fParticleRadius, this.m_fRelaxationPercentage, this.m_fFixedTimeStepSpeed, new global::UnityEngine.Vector3(0f, -this.m_fGravityAmount, 0f));
			this.m_listSticks.Add(verletStick);
			num2++;
		}
	}

	private void FixedUpdate()
	{
		this.m_listRopeParticles[0].transform.position = this.HookedPosition.position;
		int num = 0;
		while ((long)num < (long)((ulong)this.m_iNumSticks))
		{
			this.m_listSticks[num].Simulate();
			num += 2;
		}
		int num2 = 0;
		while ((long)num2 < (long)((ulong)this.m_iNumIterations))
		{
			this.m_listRopeParticles[0].transform.position = this.HookedPosition.position;
			int num3 = 0;
			while ((long)num3 < (long)((ulong)this.m_iNumSticks))
			{
				this.m_listSticks[num3].SatisfyConstraint();
				num3++;
			}
			this.m_listRopeParticles[0].transform.position = this.HookedPosition.position;
			for (int i = (int)(this.m_iNumSticks - 1U); i >= 0; i--)
			{
				this.m_listSticks[i].SatisfyConstraint();
			}
			num2++;
		}
		int num4 = 0;
		while ((long)num4 < (long)((ulong)this.m_iNumSticks))
		{
			this.m_listSticks[num4].Reorient();
			num4++;
		}
		this.m_listRopeParticles[0].transform.position = this.HookedPosition.position;
		int num5 = 0;
		while ((long)num5 < (long)((ulong)this.m_iNumParticles))
		{
			this.m_listRopeParticles[num5].GetComponent<global::UnityEngine.Rigidbody>().velocity = new global::UnityEngine.Vector3(0f, 0f, 0f);
			num5++;
		}
	}

	public global::UnityEngine.Transform HookedPosition;

	private uint m_iNumParticles = 60U;

	private uint m_iNumIterations = 7U;

	private float m_fParticleRadius = 0.08f;

	private float m_fRelaxationPercentage = 0.85f;

	private float m_fFixedTimeStepSpeed = 0.02f;

	private float m_fGravityAmount = 7f;

	private uint m_iNumSticks;

	private global::System.Collections.Generic.List<global::UnityEngine.GameObject> m_listRopeParticles;

	private global::System.Collections.Generic.List<global::VerletStick> m_listSticks;
}
