using System;
using UnityEngine;

[global::System.Serializable]
public class DigitalAxis
{
	public void Init()
	{
		this.axis = 0f;
		this.rawAxis = 0f;
		this.oldRawAxis = 0f;
		this.inverse = ((!this.invert) ? 1 : -1);
		this.isDown = false;
		this.isNegDown = false;
		this.isDownDelay = 0f;
	}

	public float GetAxis()
	{
		return this.axis;
	}

	public float GetAxisRaw()
	{
		return this.rawAxis;
	}

	public bool GetKeyDown()
	{
		return global::UnityEngine.Input.GetKeyDown(this.posKey) || global::UnityEngine.Input.GetKeyDown(this.posAltKey) || global::UnityEngine.Input.GetKeyDown(this.posAltKey2) || (this.isDown && this.isDownDelay <= 0f);
	}

	public bool GetNegKeyDown()
	{
		return global::UnityEngine.Input.GetKeyDown(this.negKey) || global::UnityEngine.Input.GetKeyDown(this.negAltKey) || global::UnityEngine.Input.GetKeyDown(this.negAltKey2) || (this.isNegDown && this.isDownDelay <= 0f);
	}

	public bool GetKeyUp()
	{
		return global::UnityEngine.Input.GetKeyUp(this.posKey) || global::UnityEngine.Input.GetKeyUp(this.posAltKey) || global::UnityEngine.Input.GetKeyUp(this.posAltKey2);
	}

	public bool GetNegKeyUp()
	{
		return global::UnityEngine.Input.GetKeyUp(this.negKey) || global::UnityEngine.Input.GetKeyUp(this.negAltKey) || global::UnityEngine.Input.GetKeyUp(this.negAltKey2);
	}

	public bool GetKey()
	{
		return global::UnityEngine.Input.GetKey(this.posKey) || global::UnityEngine.Input.GetKey(this.posAltKey) || global::UnityEngine.Input.GetKey(this.posAltKey2);
	}

	public bool GetNegKey()
	{
		return global::UnityEngine.Input.GetKey(this.negKey) || global::UnityEngine.Input.GetKey(this.negAltKey) || global::UnityEngine.Input.GetKey(this.negAltKey2);
	}

	public void Update()
	{
		if (this.isDown || this.isNegDown)
		{
			if (this.isDownDelay > 0f)
			{
				this.isDownDelay -= global::UnityEngine.Time.deltaTime;
			}
			else
			{
				this.isDownDelay = global::PandoraSingleton<global::PandoraInput>.Instance.subsequentRepeatRate;
			}
		}
		if (global::UnityEngine.Input.GetKeyDown(this.posKey) || global::UnityEngine.Input.GetKeyDown(this.posAltKey) || global::UnityEngine.Input.GetKeyDown(this.posAltKey2))
		{
			if (this.snap && this.oldRawAxis != 1f)
			{
				this.axis = 0f;
			}
			this.rawAxis = 1f;
			this.isDown = true;
			this.isDownDelay = global::PandoraSingleton<global::PandoraInput>.Instance.firstRepeatDelay;
		}
		else if (global::UnityEngine.Input.GetKeyDown(this.negKey) || global::UnityEngine.Input.GetKeyDown(this.negAltKey) || global::UnityEngine.Input.GetKeyDown(this.negAltKey2))
		{
			if (this.snap && (double)this.oldRawAxis != -1.0)
			{
				this.axis = 0f;
			}
			this.rawAxis = -1f;
			this.isNegDown = true;
			this.isDownDelay = global::PandoraSingleton<global::PandoraInput>.Instance.firstRepeatDelay;
		}
		if (global::UnityEngine.Input.GetKeyUp(this.posKey) || global::UnityEngine.Input.GetKeyUp(this.posAltKey) || global::UnityEngine.Input.GetKeyUp(this.posAltKey2) || global::UnityEngine.Input.GetKeyUp(this.negKey) || global::UnityEngine.Input.GetKeyUp(this.negAltKey) || global::UnityEngine.Input.GetKeyUp(this.negAltKey2))
		{
			this.oldRawAxis = this.rawAxis;
			this.rawAxis = 0f;
			if (global::UnityEngine.Input.GetKeyUp(this.posKey) || global::UnityEngine.Input.GetKeyUp(this.posAltKey) || global::UnityEngine.Input.GetKeyUp(this.posAltKey2))
			{
				this.isDown = false;
			}
			else if (global::UnityEngine.Input.GetKeyUp(this.negKey) || global::UnityEngine.Input.GetKeyUp(this.negAltKey) || global::UnityEngine.Input.GetKeyUp(this.negAltKey2))
			{
				this.isNegDown = false;
			}
			this.CheckKeysDown();
		}
		if (global::UnityEngine.Input.GetKey(this.posKey) || global::UnityEngine.Input.GetKey(this.posAltKey) || global::UnityEngine.Input.GetKey(this.posAltKey2) || global::UnityEngine.Input.GetKey(this.negKey) || global::UnityEngine.Input.GetKey(this.negAltKey) || global::UnityEngine.Input.GetKey(this.negAltKey2))
		{
			this.axis += this.rawAxis * (float)this.inverse * this.sensitivity * global::UnityEngine.Time.deltaTime;
			this.axis = global::UnityEngine.Mathf.Clamp(this.axis, -1f, 1f);
		}
		else if (this.oldRawAxis != 0f)
		{
			float num = this.axis;
			this.axis -= this.oldRawAxis * (float)this.inverse * this.gravity * global::UnityEngine.Time.deltaTime;
			if ((num > 0f && this.axis <= 0f) || (num < 0f && this.axis >= 0f))
			{
				this.axis = 0f;
				this.oldRawAxis = 0f;
			}
		}
	}

	private void CheckKeysDown()
	{
		if (global::UnityEngine.Input.GetKey(this.posKey) || global::UnityEngine.Input.GetKey(this.posAltKey) || global::UnityEngine.Input.GetKey(this.posAltKey2))
		{
			if (this.snap)
			{
				this.axis = 0f;
			}
			this.rawAxis = 1f;
		}
		else if (global::UnityEngine.Input.GetKey(this.negKey) || global::UnityEngine.Input.GetKey(this.negAltKey) || global::UnityEngine.Input.GetKey(this.negAltKey2))
		{
			if (this.snap)
			{
				this.axis = 0f;
			}
			this.rawAxis = -1f;
		}
	}

	public global::UnityEngine.KeyCode posKey;

	public global::UnityEngine.KeyCode posAltKey;

	public global::UnityEngine.KeyCode posAltKey2;

	public global::UnityEngine.KeyCode negKey;

	public global::UnityEngine.KeyCode negAltKey;

	public global::UnityEngine.KeyCode negAltKey2;

	public float gravity;

	public float sensitivity;

	public bool snap;

	public bool invert;

	private float axis;

	private float rawAxis;

	private float oldRawAxis;

	private int inverse;

	private bool isDown;

	private bool isNegDown;

	private float isDownDelay;
}
