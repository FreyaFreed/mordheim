using System;
using UnityEngine;

public class InputAction : global::UnityEngine.MonoBehaviour
{
	public void Init()
	{
		this.dAxis.Init();
		this.aAxis.Init();
	}

	public float GetAxis()
	{
		float axis = this.dAxis.GetAxis();
		if (axis != 0f)
		{
			return axis;
		}
		axis = this.aAxis.GetAxis();
		if (axis != 0f)
		{
			return axis;
		}
		return 0f;
	}

	public float GetAxisRaw()
	{
		float axisRaw = this.dAxis.GetAxisRaw();
		if (axisRaw != 0f)
		{
			return axisRaw;
		}
		axisRaw = this.aAxis.GetAxisRaw();
		if (axisRaw != 0f)
		{
			return axisRaw;
		}
		return 0f;
	}

	public bool GetKeyDown()
	{
		bool keyDown = this.dAxis.GetKeyDown();
		if (keyDown)
		{
			return keyDown;
		}
		keyDown = this.aAxis.GetKeyDown();
		return keyDown && keyDown;
	}

	public bool GetNegKeyDown()
	{
		bool negKeyDown = this.dAxis.GetNegKeyDown();
		if (negKeyDown)
		{
			return negKeyDown;
		}
		negKeyDown = this.aAxis.GetNegKeyDown();
		return negKeyDown && negKeyDown;
	}

	public bool GetKeyUp()
	{
		bool keyUp = this.dAxis.GetKeyUp();
		if (keyUp)
		{
			return keyUp;
		}
		keyUp = this.aAxis.GetKeyUp();
		return keyUp && keyUp;
	}

	public bool GetNegKeyUp()
	{
		bool negKeyUp = this.dAxis.GetNegKeyUp();
		if (negKeyUp)
		{
			return negKeyUp;
		}
		negKeyUp = this.aAxis.GetNegKeyUp();
		return negKeyUp && negKeyUp;
	}

	public bool GetKey()
	{
		bool key = this.dAxis.GetKey();
		if (key)
		{
			return key;
		}
		key = this.aAxis.GetKey();
		return key && key;
	}

	public bool GetNegKey()
	{
		bool negKey = this.dAxis.GetNegKey();
		if (negKey)
		{
			return negKey;
		}
		negKey = this.aAxis.GetNegKey();
		return negKey && negKey;
	}

	private void Update()
	{
	}

	public string actionName;

	[global::UnityEngine.SerializeField]
	public global::DigitalAxis dAxis;

	[global::UnityEngine.SerializeField]
	public global::AnalogAxis aAxis;
}
