using System;
using UnityEngine;

public class KGFFPSDisplay : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		this.itsStyleText = new global::UnityEngine.GUIStyle();
		this.itsStyleText.fontSize = this.itsFontSize;
		this.itsStyleText.normal.textColor = global::UnityEngine.Color.white;
	}

	private void Update()
	{
		this.itsFrameCounter++;
		if (global::UnityEngine.Time.time - this.itsLastMeasurePoint > this.itsTimeBetweenMeasurePoints)
		{
			this.itsFPS = (float)this.itsFrameCounter / (global::UnityEngine.Time.time - this.itsLastMeasurePoint);
			this.itsFrameCounter = 0;
			this.itsLastMeasurePoint = global::UnityEngine.Time.time;
		}
	}

	private void OnGUI()
	{
		global::UnityEngine.GUI.color = global::UnityEngine.Color.black;
		global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(1f, 1f, 200f, 200f), string.Empty + (int)this.itsFPS + " FPS", this.itsStyleText);
		global::UnityEngine.GUI.color = this.itsFontColor;
		global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(0f, 0f, 200f, 200f), string.Empty + (int)this.itsFPS + " FPS", this.itsStyleText);
	}

	private float itsFPS;

	private int itsFrameCounter;

	private float itsLastMeasurePoint;

	public float itsTimeBetweenMeasurePoints = 2f;

	public int itsFontSize = 30;

	public global::UnityEngine.Color itsFontColor = global::UnityEngine.Color.white;

	private global::UnityEngine.GUIStyle itsStyleText;
}
