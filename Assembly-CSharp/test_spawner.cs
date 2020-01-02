using System;
using UnityEngine;

public class test_spawner : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (global::UnityEngine.GameObject.Find("Origin").transform.childCount > 0)
		{
			this.SpawnedModel.transform.position = global::UnityEngine.GameObject.Find("Origin").transform.position;
		}
		if (this.Cams != null)
		{
			for (int i = 0; i < this.Cams.Length; i++)
			{
				this.Cams[i].transform.LookAt(global::UnityEngine.GameObject.Find("Origin").transform);
			}
		}
	}

	private void OnGUI()
	{
		global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect(0f, 0f, (float)(global::UnityEngine.Screen.width / 5), (float)global::UnityEngine.Screen.height));
		if (!this.Optsbool)
		{
			if (global::UnityEngine.GUILayout.Button("+ Options", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.Optsbool = true;
			}
		}
		else
		{
			if (global::UnityEngine.GUILayout.Button("- Options", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.Optsbool = false;
			}
			foreach (global::UnityEngine.GameObject gameObject in this.Props)
			{
				if (global::UnityEngine.GameObject.Find(gameObject.name).GetComponent<global::UnityEngine.Renderer>().enabled && global::UnityEngine.GUILayout.Button("Hide:  " + gameObject.name, this.btnStyle2, new global::UnityEngine.GUILayoutOption[0]))
				{
					global::UnityEngine.GameObject.Find(gameObject.name).GetComponent<global::UnityEngine.Renderer>().enabled = false;
				}
				if (!global::UnityEngine.GameObject.Find(gameObject.name).GetComponent<global::UnityEngine.Renderer>().enabled && global::UnityEngine.GUILayout.Button("Unhide:  " + gameObject.name, this.btnStyle2, new global::UnityEngine.GUILayoutOption[0]))
				{
					global::UnityEngine.GameObject.Find(gameObject.name).GetComponent<global::UnityEngine.Renderer>().enabled = true;
				}
			}
			if (global::UnityEngine.GUILayout.Button("Reset", this.btnStyle2, new global::UnityEngine.GUILayoutOption[0]))
			{
				global::UnityEngine.Application.LoadLevel(global::UnityEngine.Application.loadedLevelName);
			}
		}
		if (!this.CHbool)
		{
			if (global::UnityEngine.GUILayout.Button("+ Characters", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CHbool = true;
			}
		}
		else
		{
			if (global::UnityEngine.GUILayout.Button("- Characters", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.CHbool = false;
			}
			foreach (global::UnityEngine.GameObject gameObject2 in this.Models)
			{
				if (global::UnityEngine.GUILayout.Button(gameObject2.name, this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
				{
					if (global::UnityEngine.GameObject.Find("Origin").transform.childCount > 0)
					{
						global::UnityEngine.Object.Destroy(global::UnityEngine.GameObject.Find("Origin").transform.GetChild(0).gameObject);
					}
					this.SpawnedModel = (global::UnityEngine.Object.Instantiate(gameObject2, base.transform.position, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject);
					this.SpawnedModel.transform.position = global::UnityEngine.GameObject.Find("Origin").transform.position;
					this.SpawnedModel.transform.rotation = global::UnityEngine.GameObject.Find("Origin").transform.rotation;
					this.SpawnedModel.transform.parent = global::UnityEngine.GameObject.Find("Origin").transform;
					this.SpawnedModel.GetComponent<global::UnityEngine.Rigidbody>().useGravity = false;
					this.Animsbool = false;
				}
			}
		}
		if (!this.Animsbool && global::UnityEngine.GUILayout.Button("+ Animations", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.Animsbool = true;
			this.animator = this.SpawnedModel.GetComponent<global::UnityEngine.Animator>();
			this.animator.runtimeAnimatorController = this.ViewerAnimator;
		}
		if (this.Animsbool)
		{
			if (global::UnityEngine.GUILayout.Button("- Animations", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.Animsbool = false;
			}
			if (global::UnityEngine.GUILayout.Button("Idle", this.btnStyle2, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.animator.SetBool("idle", true);
				this.animator.SetInteger("animID", 0);
			}
			global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
			if (global::UnityEngine.GUILayout.Button("<", this.btnStyle2, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.animator.SetBool("idle", true);
				this.animator.SetInteger("animID", --this.anims);
				this.animator.SetBool("idle", false);
				global::UnityEngine.MonoBehaviour.print(this.anims);
			}
			if (global::UnityEngine.GUILayout.Button(">", this.btnStyle2, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.animator.SetBool("idle", true);
				this.animator.SetInteger("animID", ++this.anims);
				this.animator.SetBool("idle", false);
				global::UnityEngine.MonoBehaviour.print(this.anims);
			}
			global::UnityEngine.GUILayout.EndHorizontal();
		}
		if (!this.camerasbool && global::UnityEngine.GUILayout.Button("+  Cameras", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.camerasbool = true;
		}
		if (this.camerasbool)
		{
			if (global::UnityEngine.GUILayout.Button("-  Cameras", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.camerasbool = false;
			}
			foreach (global::UnityEngine.Camera camera in this.Cams)
			{
				if (global::UnityEngine.GUILayout.Button(camera.name, this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
				{
					for (int l = 0; l < this.Cams.Length; l++)
					{
						this.Cams[l].enabled = false;
					}
					camera.enabled = true;
				}
			}
		}
		if (!this.fxsbool && global::UnityEngine.GUILayout.Button("+  Fx's", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
		{
			this.fxsbool = true;
		}
		if (this.fxsbool)
		{
			if (global::UnityEngine.GUILayout.Button("-  Fx's", this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
			{
				this.fxsbool = false;
			}
			foreach (global::UnityEngine.GameObject gameObject3 in this.Fxs)
			{
				if (global::UnityEngine.GUILayout.Button(gameObject3.name, this.btnStyle, new global::UnityEngine.GUILayoutOption[0]))
				{
					if (global::UnityEngine.GameObject.Find("FXS").transform.childCount > 0)
					{
						global::UnityEngine.Object.Destroy(global::UnityEngine.GameObject.Find("FXS").transform.GetChild(0).gameObject);
					}
					this.Spawnedfx = (global::UnityEngine.Object.Instantiate(gameObject3, base.transform.position, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject);
					this.Spawnedfx.transform.position = global::UnityEngine.GameObject.Find("FXS").transform.position;
					this.Spawnedfx.transform.rotation = global::UnityEngine.GameObject.Find("FXS").transform.rotation;
					this.Spawnedfx.transform.parent = global::UnityEngine.GameObject.Find("FXS").transform;
				}
			}
		}
		global::UnityEngine.GUILayout.EndArea();
	}

	public global::UnityEngine.GameObject[] Models;

	private bool CHbool;

	private bool Optsbool;

	private bool Animsbool;

	private int anims;

	private global::UnityEngine.GameObject SpawnedModel;

	public global::UnityEngine.GameObject[] Props;

	public global::UnityEngine.GUIStyle btnStyle;

	public global::UnityEngine.GUIStyle btnStyle2;

	public global::UnityEngine.RuntimeAnimatorController ViewerAnimator;

	public global::UnityEngine.Camera[] Cams;

	private bool camerasbool;

	public global::UnityEngine.GameObject[] Fxs;

	private bool fxsbool;

	private global::UnityEngine.GameObject Spawnedfx;

	private global::UnityEngine.Animator animator;
}
