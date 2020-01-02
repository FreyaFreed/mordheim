using System;
using UnityEngine;

[global::System.Serializable]
public class AnalogAxis
{
	public void Init()
	{
		this.inverse = ((!this.invert) ? 1 : -1);
	}

	public void Update()
	{
		if (string.IsNullOrEmpty(this.inputName))
		{
			return;
		}
		if (this.wasPosDown || this.wasNegDown)
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
		this.isPosDown = false;
		this.isPosUp = false;
		this.isNegDown = false;
		this.isNegUp = false;
		float num = global::UnityEngine.Input.GetAxisRaw(this.inputName);
		if (!string.IsNullOrEmpty(this.altInputName))
		{
			num += global::UnityEngine.Input.GetAxisRaw(this.altInputName);
		}
		if (num > 0.95f && this.prevAxis < 0.95f && !this.wasPosDown)
		{
			this.isPosDown = true;
			this.wasPosDown = true;
			this.isDownDelay = global::PandoraSingleton<global::PandoraInput>.Instance.firstRepeatDelay;
			this.wasPosUp = false;
		}
		else if (num < 0.5f && this.prevAxis > 0.5f && !this.wasPosUp)
		{
			this.isPosUp = true;
			this.wasPosUp = true;
			this.wasPosDown = false;
		}
		if (num < -0.95f && this.prevAxis > -0.95f && !this.wasNegDown)
		{
			this.isNegDown = true;
			this.wasNegDown = true;
			this.isDownDelay = global::PandoraSingleton<global::PandoraInput>.Instance.firstRepeatDelay;
			this.wasNegUp = false;
		}
		else if (num > -0.5f && this.prevAxis < -0.5f && !this.wasNegUp)
		{
			this.isNegUp = true;
			this.wasNegUp = true;
			this.wasNegDown = false;
		}
		this.prevAxis = num;
	}

	public bool GetKeyDown()
	{
		return ((!this.invert) ? this.isPosDown : this.isNegDown) || (((!this.invert) ? this.wasPosDown : this.wasNegDown) && this.isDownDelay <= 0f);
	}

	public bool GetNegKeyDown()
	{
		return (!this.invert) ? (this.isNegDown || (((!this.invert) ? this.wasNegDown : this.wasPosDown) && this.isDownDelay <= 0f)) : this.isPosDown;
	}

	public bool GetKeyUp()
	{
		return (!this.invert) ? this.isPosUp : this.isNegUp;
	}

	public bool GetNegKeyUp()
	{
		return (!this.invert) ? this.isNegUp : this.isPosUp;
	}

	public bool GetKey()
	{
		if (string.IsNullOrEmpty(this.inputName))
		{
			return false;
		}
		bool flag;
		if (this.invert)
		{
			flag = (global::UnityEngine.Input.GetAxis(this.inputName) < 0f);
			if (!string.IsNullOrEmpty(this.altInputName))
			{
				flag = (flag || global::UnityEngine.Input.GetAxis(this.altInputName) < 0f);
			}
		}
		else
		{
			flag = (global::UnityEngine.Input.GetAxis(this.inputName) > 0f);
			if (!string.IsNullOrEmpty(this.altInputName))
			{
				flag = (flag || global::UnityEngine.Input.GetAxis(this.altInputName) > 0f);
			}
		}
		return flag;
	}

	public bool GetNegKey()
	{
		if (string.IsNullOrEmpty(this.inputName))
		{
			return false;
		}
		bool flag;
		if (this.invert)
		{
			flag = (global::UnityEngine.Input.GetAxis(this.inputName) > 0f);
			if (!string.IsNullOrEmpty(this.altInputName))
			{
				flag = (flag || global::UnityEngine.Input.GetAxis(this.altInputName) > 0f);
			}
		}
		else
		{
			flag = (global::UnityEngine.Input.GetAxis(this.inputName) < 0f);
			if (!string.IsNullOrEmpty(this.altInputName))
			{
				flag = (flag || global::UnityEngine.Input.GetAxis(this.altInputName) < 0f);
			}
		}
		return flag;
	}

	public float GetAxis()
	{
		if (string.IsNullOrEmpty(this.inputName))
		{
			return 0f;
		}
		float num = global::UnityEngine.Input.GetAxis(this.inputName) * (float)this.inverse * this.sensitivity;
		if (!string.IsNullOrEmpty(this.altInputName))
		{
			num += global::UnityEngine.Input.GetAxis(this.altInputName) * (float)this.inverse * this.sensitivity;
		}
		return num;
	}

	public float GetAxisRaw()
	{
		if (string.IsNullOrEmpty(this.inputName))
		{
			return 0f;
		}
		float num = global::UnityEngine.Input.GetAxisRaw(this.inputName) * (float)this.inverse * this.sensitivity;
		if (!string.IsNullOrEmpty(this.altInputName))
		{
			num += global::UnityEngine.Input.GetAxisRaw(this.altInputName) * (float)this.inverse * this.sensitivity;
		}
		return num;
	}

	public string inputName;

	public string altInputName;

	public float sensitivity = 1f;

	public bool invert;

	public bool repeatDown;

	private int inverse;

	private float prevAxis;

	private bool isPosDown;

	private bool isPosUp;

	private bool wasPosUp;

	private bool wasPosDown;

	private bool isNegDown;

	private bool isNegUp;

	private bool wasNegUp;

	private bool wasNegDown;

	private float isDownDelay;
}
