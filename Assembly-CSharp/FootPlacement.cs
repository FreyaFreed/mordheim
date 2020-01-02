using System;
using UnityEngine;

public class FootPlacement : global::UnityEngine.MonoBehaviour
{
	public void FixFoot(global::UnityEngine.AvatarIKGoal footGoal, global::UnityEngine.RaycastHit hit, float _weight)
	{
		this.animator.GetComponent<global::UnityEngine.Rigidbody>().useGravity = !this.animator.GetComponent<global::UnityEngine.Rigidbody>().useGravity;
		global::UnityEngine.CapsuleCollider component = this.animator.GetComponent<global::UnityEngine.CapsuleCollider>();
		if (hit.collider.Equals(component))
		{
			return;
		}
		if (hit.distance < 1f + this.weight)
		{
			_weight = 1f;
		}
		this.animator.SetIKPosition(footGoal, hit.point + global::UnityEngine.Vector3.up * this.weight);
		this.animator.SetIKPositionWeight(footGoal, _weight);
		global::UnityEngine.Quaternion quaternion = this.animator.GetIKRotation(footGoal);
		global::UnityEngine.Vector3 fromDirection = quaternion * global::UnityEngine.Vector3.up;
		quaternion = global::UnityEngine.Quaternion.FromToRotation(fromDirection, hit.normal) * quaternion;
		this.animator.SetIKRotation(footGoal, quaternion);
		this.animator.SetIKRotationWeight(footGoal, _weight);
	}

	private void Start()
	{
		this.leftRot = this.leftBone.eulerAngles.x;
		if (this.leftRot > 180f)
		{
			this.leftRot -= 180f;
		}
		else
		{
			this.leftRot += 180f;
		}
		this.rightRot = this.rightBone.eulerAngles.x;
		if (this.rightRot > 180f)
		{
			this.rightRot -= 180f;
		}
		else
		{
			this.rightRot += 180f;
		}
	}

	public void legPosition()
	{
		float num = this.leftBone.eulerAngles.x;
		if (num > 180f)
		{
			num -= 180f;
		}
		else
		{
			num += 180f;
		}
		float num2 = this.rightBone.eulerAngles.x;
		if (num2 > 180f)
		{
			num2 -= 180f;
		}
		else
		{
			num2 += 180f;
		}
		if (this.leftRot < this.rightRot)
		{
			this.firstFoot = "left";
			this.secondFoot = "right";
		}
		else if (this.leftRot > this.rightRot)
		{
			this.firstFoot = "right";
			this.secondFoot = "left";
		}
		if (this.leftRot < num && this.firstFoot == "left")
		{
			this.earthFoot = "left";
			this.airFoot = "right";
		}
		else if (this.leftRot > num && this.firstFoot == "left")
		{
			this.earthFoot = "right";
			this.airFoot = "left";
		}
		else if (this.rightRot < num2 && this.firstFoot == "right")
		{
			this.earthFoot = "right";
			this.airFoot = "left";
		}
		else if (this.rightRot > num2 && this.firstFoot == "right")
		{
			this.earthFoot = "left";
			this.airFoot = "right";
		}
		this.leftRot = num;
		this.rightRot = num2;
	}

	public void FixCollider(global::UnityEngine.RaycastHit Lhit, global::UnityEngine.RaycastHit Rhit, bool Idle)
	{
		this.legPosition();
		global::UnityEngine.CapsuleCollider component = this.animator.GetComponent<global::UnityEngine.CapsuleCollider>();
		if (Lhit.collider.Equals(component) || Rhit.collider.Equals(component))
		{
			return;
		}
		float num = global::UnityEngine.Vector3.Angle(global::UnityEngine.Vector3.up, Lhit.normal);
		float num2 = global::UnityEngine.Vector3.Angle(global::UnityEngine.Vector3.up, Rhit.normal);
		global::UnityEngine.Vector3 center = component.center;
		float num3 = global::UnityEngine.Mathf.Abs(Lhit.point.y - Rhit.point.y);
		float num4 = 0f;
		if (num == 0f && num2 == 0f)
		{
			if (Idle)
			{
				num4 = 1.05f + num3;
			}
			else if (num3 > 0f)
			{
				if (this.secondFoot == this.earthFoot && this.firstFoot == this.airFoot)
				{
					this.animator.GetComponent<global::UnityEngine.Rigidbody>().constraints = (global::UnityEngine.RigidbodyConstraints)116;
					num4 = 1.05f;
				}
				else if (this.firstFoot == this.earthFoot && this.secondFoot == this.airFoot)
				{
					this.animator.GetComponent<global::UnityEngine.Rigidbody>().constraints = global::UnityEngine.RigidbodyConstraints.FreezeRotation;
					num4 = 1.05f;
				}
				if (this.earthFoot == "left" && Lhit.distance > 1.05f)
				{
					num4 += Lhit.distance - 1.05f;
				}
				else if (this.earthFoot == "right" && Rhit.distance > 1.05f)
				{
					num4 += Rhit.distance - 1.05f;
				}
			}
			else
			{
				this.animator.GetComponent<global::UnityEngine.Rigidbody>().constraints = global::UnityEngine.RigidbodyConstraints.FreezeRotation;
				num4 = 1.05f;
			}
		}
		else
		{
			num4 = 1.05f + num3 * ((num + num2) / 360f);
		}
		if (global::UnityEngine.Mathf.Abs(center.y - num4) < num4 / 100f)
		{
			center.y = num4;
		}
		else if (center.y < num4)
		{
			center.y += num4 / 100f;
		}
		else if (center.y > num4)
		{
			center.y -= num4 / 100f;
		}
		center.y = num4;
		component.center = center;
	}

	public void footPlanting()
	{
		bool flag = false;
		global::UnityEngine.Vector3 origin = this.animator.GetIKPosition(global::UnityEngine.AvatarIKGoal.LeftFoot) + global::UnityEngine.Vector3.up * 1f;
		global::UnityEngine.Vector3 origin2 = this.animator.GetIKPosition(global::UnityEngine.AvatarIKGoal.RightFoot) + global::UnityEngine.Vector3.up * 1f;
		global::UnityEngine.RaycastHit raycastHit = default(global::UnityEngine.RaycastHit);
		global::UnityEngine.RaycastHit raycastHit2 = default(global::UnityEngine.RaycastHit);
		if (global::UnityEngine.Physics.Raycast(origin, -global::UnityEngine.Vector3.up, out raycastHit, 2f, global::LayerMaskManager.footMask) && global::UnityEngine.Physics.Raycast(origin2, -global::UnityEngine.Vector3.up, out raycastHit2, 2f, global::LayerMaskManager.footMask))
		{
			if (flag)
			{
				this.leftWeight = (this.rightWeight = 1f);
			}
			else
			{
				this.leftWeight = global::UnityEngine.Mathf.Pow(1f - raycastHit.distance, 5f);
				this.rightWeight = global::UnityEngine.Mathf.Pow(1f - raycastHit2.distance, 5f);
			}
			this.FixFoot(global::UnityEngine.AvatarIKGoal.LeftFoot, raycastHit, this.leftWeight);
			this.FixFoot(global::UnityEngine.AvatarIKGoal.RightFoot, raycastHit2, this.rightWeight);
			this.FixCollider(raycastHit, raycastHit2, flag);
		}
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (layerIndex == 0)
		{
			this.footPlanting();
		}
	}

	private float leftWeight = 1f;

	private float rightWeight = 1f;

	public float weight;

	public global::UnityEngine.Animator animator;

	public global::UnityEngine.AnimationClip[] Idles;

	public global::UnityEngine.Transform leftBone;

	public global::UnityEngine.Transform rightBone;

	private float leftRot;

	private float rightRot;

	private string firstFoot;

	private string secondFoot;

	private string earthFoot;

	private string airFoot;
}
