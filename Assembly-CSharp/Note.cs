using System;
using UnityEngine;

[global::UnityEngine.AddComponentMenu("Infinity Code/Note")]
public class Note : global::UnityEngine.MonoBehaviour
{
	public string title
	{
		get
		{
			string text = base.gameObject.name;
			if (this.isPrefab)
			{
				text = "Prefab: " + text;
			}
			return text;
		}
	}

	public int instanceID
	{
		get
		{
			if (this._instanceID == -2147483648)
			{
				this._instanceID = base.gameObject.GetInstanceID();
			}
			return this._instanceID;
		}
	}

	public static global::UnityEngine.Texture2D defaultIcon;

	public bool expanded = true;

	public float height = 45f;

	public global::UnityEngine.Texture2D icon;

	public string iconPath = string.Empty;

	public bool isPrefab;

	public bool lockHeight;

	public float managerHeight = 45f;

	public global::UnityEngine.Vector2 managerScrollPos;

	public float maxHeight = 800f;

	public global::UnityEngine.Vector2 scrollPos;

	public string text = string.Empty;

	public bool wordWrap;

	private int _instanceID = int.MinValue;
}
