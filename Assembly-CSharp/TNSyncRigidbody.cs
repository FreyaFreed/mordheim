using System;
using TNet;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Rigidbody))]
[global::UnityEngine.AddComponentMenu("TNet/Sync Rigidbody")]
public class TNSyncRigidbody : global::TNBehaviour
{
	private void Awake()
	{
		this.mTrans = base.transform;
		this.mRb = base.GetComponent<global::UnityEngine.Rigidbody>();
		this.mLastPos = this.mTrans.position;
		this.mLastRot = this.mTrans.rotation.eulerAngles;
		this.UpdateInterval();
	}

	private void UpdateInterval()
	{
		this.mNext = global::UnityEngine.Time.time + ((this.updatesPerSecond <= 0) ? 0f : (1f / (float)this.updatesPerSecond));
	}

	private void FixedUpdate()
	{
		if (this.updatesPerSecond > 0 && this.mNext < global::UnityEngine.Time.time && base.tno.isMine && global::TNManager.isInChannel)
		{
			bool flag = this.mRb.IsSleeping();
			if (flag && this.mWasSleeping)
			{
				return;
			}
			this.UpdateInterval();
			global::UnityEngine.Vector3 position = this.mTrans.position;
			global::UnityEngine.Vector3 eulerAngles = this.mTrans.rotation.eulerAngles;
			if (this.mWasSleeping || position != this.mLastPos || eulerAngles != this.mLastRot)
			{
				this.mLastPos = position;
				this.mLastRot = eulerAngles;
				if (this.isImportant)
				{
					base.tno.Send(1, global::TNet.Target.OthersSaved, new object[]
					{
						position,
						eulerAngles,
						this.mRb.velocity,
						this.mRb.angularVelocity
					});
				}
				else
				{
					base.tno.SendQuickly(1, global::TNet.Target.OthersSaved, new object[]
					{
						position,
						eulerAngles,
						this.mRb.velocity,
						this.mRb.angularVelocity
					});
				}
			}
			this.mWasSleeping = flag;
		}
	}

	[global::TNet.RFC(1)]
	private void OnSync(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 rot, global::UnityEngine.Vector3 vel, global::UnityEngine.Vector3 ang)
	{
		this.mTrans.position = pos;
		this.mTrans.rotation = global::UnityEngine.Quaternion.Euler(rot);
		this.mRb.velocity = vel;
		this.mRb.angularVelocity = ang;
		this.UpdateInterval();
	}

	private void OnCollisionEnter()
	{
		if (global::TNManager.isHosting)
		{
			this.Sync();
		}
	}

	public void Sync()
	{
		if (global::TNManager.isInChannel)
		{
			this.UpdateInterval();
			this.mWasSleeping = false;
			this.mLastPos = this.mTrans.position;
			this.mLastRot = this.mTrans.rotation.eulerAngles;
			base.tno.Send(1, global::TNet.Target.OthersSaved, new object[]
			{
				this.mLastPos,
				this.mLastRot,
				this.mRb.velocity,
				this.mRb.angularVelocity
			});
		}
	}

	public int updatesPerSecond = 10;

	public bool isImportant;

	private global::UnityEngine.Transform mTrans;

	private global::UnityEngine.Rigidbody mRb;

	private float mNext;

	private bool mWasSleeping;

	private global::UnityEngine.Vector3 mLastPos;

	private global::UnityEngine.Vector3 mLastRot;
}
