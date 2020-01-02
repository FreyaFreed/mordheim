using System;
using UnityEngine;

public class Crow : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.collisionMask = global::UnityEngine.LayerMask.NameToLayer("trigger_collision");
		this.animator = base.GetComponent<global::UnityEngine.Animator>();
		this.collider = base.GetComponent<global::UnityEngine.Collider>();
		this.flying = false;
		this.time = (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(2.0, 6.0);
	}

	private void Update()
	{
		if (this.flying)
		{
			base.transform.position += (2f * global::UnityEngine.Vector3.up + base.transform.forward) * global::UnityEngine.Time.deltaTime * 2f;
			if (base.transform.position.y > 40f)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (this.time > 0f)
		{
			this.time -= global::UnityEngine.Time.deltaTime;
		}
		else
		{
			this.animator.SetBool("feasting", !this.animator.GetBool("feasting"));
			this.time = (float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(2.0, 6.0);
		}
	}

	private void OnTriggerEnter(global::UnityEngine.Collider other)
	{
		if (other.gameObject.layer == this.collisionMask)
		{
			this.flying = true;
			this.animator.SetBool("flying", true);
			this.collider.enabled = false;
		}
	}

	private global::UnityEngine.Animator animator;

	private bool flying;

	private int collisionMask;

	private global::UnityEngine.Collider collider;

	private float time;
}
