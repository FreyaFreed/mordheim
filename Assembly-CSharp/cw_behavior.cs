using System;
using UnityEngine;

public class cw_behavior : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.PointReached = false;
		this.animator = base.GetComponent<global::UnityEngine.Animator>();
		base.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
		this.cw_freepts = global::UnityEngine.GameObject.FindGameObjectsWithTag("cw_freepts");
		this.cw_foodpts = global::UnityEngine.GameObject.FindGameObjectsWithTag("cw_foodpts");
		this.FoundFoodTarget = false;
		this.FoundFreeTarget = false;
	}

	private void SetFoodTarget()
	{
		int num = global::UnityEngine.Random.Range(0, this.cw_foodpts.Length - 1);
		this.target = this.cw_foodpts[num].transform;
		this.FoundFoodTarget = true;
	}

	private void SetFreeTarget()
	{
		int num = global::UnityEngine.Random.Range(0, this.cw_freepts.Length - 1);
		this.target = this.cw_freepts[num].transform;
		this.FoundFreeTarget = true;
	}

	private void Update()
	{
		this.timetoSelectAgain += global::UnityEngine.Time.deltaTime;
		int num = global::UnityEngine.Random.Range(16, 50);
		if (this.timetoSelectAgain > (float)num)
		{
			if (!this.FoundFoodTarget)
			{
				this.SetFoodTarget();
			}
			base.transform.LookAt(this.target);
			if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) > 0.75f)
			{
				this.PointReached = false;
				this.animator.SetBool("feasting", false);
				if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) > 15f)
				{
					this.animator.SetFloat("Speed", 3f, 0.25f, global::UnityEngine.Time.deltaTime);
				}
				if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) > 10f && global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) < 15f)
				{
					this.animator.SetFloat("Speed", 1.5f, 0.25f, global::UnityEngine.Time.deltaTime);
				}
				if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) < 10f)
				{
					this.animator.SetFloat("Speed", 0.5f, 0.25f, global::UnityEngine.Time.deltaTime);
				}
				base.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
			}
			else
			{
				this.animator.SetFloat("Speed", 0f, 0.25f, global::UnityEngine.Time.deltaTime);
				base.transform.LookAt(this.target);
				this.animator.SetBool("feasting", true);
				if (!this.PointReached)
				{
					base.transform.position = new global::UnityEngine.Vector3(this.target.position.x, this.target.position.y, this.target.position.z);
					this.PointReached = true;
				}
				base.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = false;
			}
		}
		if (this.timetoSelectAgain < 15f)
		{
			if (!this.FoundFreeTarget)
			{
				this.SetFreeTarget();
			}
			base.transform.LookAt(this.target);
			if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) > 0.5f)
			{
				this.PointReached = false;
				this.animator.SetBool("feasting", false);
				if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) > 15f)
				{
					this.animator.SetFloat("Speed", 3f, 0.25f, global::UnityEngine.Time.deltaTime);
				}
				if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) > 10f && global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) < 15f)
				{
					this.animator.SetFloat("Speed", 1.5f, 0.25f, global::UnityEngine.Time.deltaTime);
				}
				if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) < 10f)
				{
					this.animator.SetFloat("Speed", 0.5f, 0.25f, global::UnityEngine.Time.deltaTime);
				}
				base.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
			}
			else
			{
				this.animator.SetFloat("Speed", 0f, 0.25f, global::UnityEngine.Time.deltaTime);
				base.transform.LookAt(this.target);
				if (!this.PointReached)
				{
					base.transform.position = new global::UnityEngine.Vector3(this.target.position.x, this.target.position.y, this.target.position.z);
					this.PointReached = true;
				}
				base.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = false;
			}
		}
		int num2 = global::UnityEngine.Random.Range(80, 120);
		if (this.timetoSelectAgain > (float)num2)
		{
			this.timetoSelectAgain = 0f;
			this.FoundFoodTarget = false;
			this.FoundFreeTarget = false;
		}
		if (global::UnityEngine.Vector3.Distance(this.target.position, this.animator.rootPosition) < 0.2f)
		{
			base.GetComponent<global::UnityEngine.Rigidbody>().isKinematic = false;
		}
	}

	private const float m_DirectionDampTime = 0.25f;

	private const float m_SpeedDampTime = 0.25f;

	public global::UnityEngine.GameObject[] cw_foodpts;

	public global::UnityEngine.GameObject[] cw_freepts;

	public float timetoSelectAgain;

	public global::UnityEngine.Transform target;

	public bool FoundFoodTarget;

	public bool FoundFreeTarget;

	public bool PointReached;

	private global::UnityEngine.Animator animator;
}
