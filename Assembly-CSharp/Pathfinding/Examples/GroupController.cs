using System;
using System.Collections.Generic;
using Pathfinding.RVO;
using Pathfinding.RVO.Sampled;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_group_controller.php")]
	public class GroupController : global::UnityEngine.MonoBehaviour
	{
		public void Start()
		{
			this.cam = global::UnityEngine.Camera.main;
			global::Pathfinding.RVO.RVOSimulator rvosimulator = global::UnityEngine.Object.FindObjectOfType(typeof(global::Pathfinding.RVO.RVOSimulator)) as global::Pathfinding.RVO.RVOSimulator;
			if (rvosimulator == null)
			{
				base.enabled = false;
				throw new global::System.Exception("No RVOSimulator in the scene. Please add one");
			}
			this.sim = rvosimulator.GetSimulator();
		}

		public void Update()
		{
			if (global::UnityEngine.Screen.fullScreen && global::UnityEngine.Screen.width != global::UnityEngine.Screen.resolutions[global::UnityEngine.Screen.resolutions.Length - 1].width)
			{
				global::UnityEngine.Screen.SetResolution(global::UnityEngine.Screen.resolutions[global::UnityEngine.Screen.resolutions.Length - 1].width, global::UnityEngine.Screen.resolutions[global::UnityEngine.Screen.resolutions.Length - 1].height, true);
			}
			if (this.adjustCamera)
			{
				global::System.Collections.Generic.List<global::Pathfinding.RVO.Sampled.Agent> agents = this.sim.GetAgents();
				float num = 0f;
				for (int i = 0; i < agents.Count; i++)
				{
					float num2 = global::UnityEngine.Mathf.Max(global::UnityEngine.Mathf.Abs(agents[i].Position.x), global::UnityEngine.Mathf.Abs(agents[i].Position.y));
					if (num2 > num)
					{
						num = num2;
					}
				}
				float a = num / global::UnityEngine.Mathf.Tan(this.cam.fieldOfView * 0.0174532924f / 2f);
				float b = num / global::UnityEngine.Mathf.Tan(global::UnityEngine.Mathf.Atan(global::UnityEngine.Mathf.Tan(this.cam.fieldOfView * 0.0174532924f / 2f) * this.cam.aspect));
				float num3 = global::UnityEngine.Mathf.Max(a, b) * 1.1f;
				num3 = global::UnityEngine.Mathf.Max(num3, 20f);
				this.cam.transform.position = global::UnityEngine.Vector3.Lerp(this.cam.transform.position, new global::UnityEngine.Vector3(0f, num3, 0f), global::UnityEngine.Time.smoothDeltaTime * 2f);
			}
			if (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.A) && global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Mouse0))
			{
				this.Order();
			}
		}

		private void OnGUI()
		{
			if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseUp && global::UnityEngine.Event.current.button == 0 && !global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.A))
			{
				this.Select(this.start, this.end);
				this.wasDown = false;
			}
			if (global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseDrag && global::UnityEngine.Event.current.button == 0)
			{
				this.end = global::UnityEngine.Event.current.mousePosition;
				if (!this.wasDown)
				{
					this.start = this.end;
					this.wasDown = true;
				}
			}
			if (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.A))
			{
				this.wasDown = false;
			}
			if (this.wasDown)
			{
				global::UnityEngine.Rect position = global::UnityEngine.Rect.MinMaxRect(global::UnityEngine.Mathf.Min(this.start.x, this.end.x), global::UnityEngine.Mathf.Min(this.start.y, this.end.y), global::UnityEngine.Mathf.Max(this.start.x, this.end.x), global::UnityEngine.Mathf.Max(this.start.y, this.end.y));
				if (position.width > 4f && position.height > 4f)
				{
					global::UnityEngine.GUI.Box(position, string.Empty, this.selectionBox);
				}
			}
		}

		public void Order()
		{
			global::UnityEngine.Ray ray = this.cam.ScreenPointToRay(global::UnityEngine.Input.mousePosition);
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.Raycast(ray, out raycastHit))
			{
				float num = 0f;
				for (int i = 0; i < this.selection.Count; i++)
				{
					num += this.selection[i].GetComponent<global::Pathfinding.RVO.RVOController>().radius;
				}
				float num2 = num / 3.14159274f;
				num2 *= 2f;
				for (int j = 0; j < this.selection.Count; j++)
				{
					float num3 = 6.28318548f * (float)j / (float)this.selection.Count;
					global::UnityEngine.Vector3 target = raycastHit.point + new global::UnityEngine.Vector3(global::UnityEngine.Mathf.Cos(num3), 0f, global::UnityEngine.Mathf.Sin(num3)) * num2;
					this.selection[j].SetTarget(target);
					this.selection[j].SetColor(this.GetColor(num3));
					this.selection[j].RecalculatePath();
				}
			}
		}

		public void Select(global::UnityEngine.Vector2 _start, global::UnityEngine.Vector2 _end)
		{
			_start.y = (float)global::UnityEngine.Screen.height - _start.y;
			_end.y = (float)global::UnityEngine.Screen.height - _end.y;
			global::UnityEngine.Vector2 b = global::UnityEngine.Vector2.Min(_start, _end);
			global::UnityEngine.Vector2 a = global::UnityEngine.Vector2.Max(_start, _end);
			if ((a - b).sqrMagnitude < 16f)
			{
				return;
			}
			this.selection.Clear();
			global::Pathfinding.Examples.RVOExampleAgent[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.Examples.RVOExampleAgent)) as global::Pathfinding.Examples.RVOExampleAgent[];
			for (int i = 0; i < array.Length; i++)
			{
				global::UnityEngine.Vector2 vector = this.cam.WorldToScreenPoint(array[i].transform.position);
				if (vector.x > b.x && vector.y > b.y && vector.x < a.x && vector.y < a.y)
				{
					this.selection.Add(array[i]);
				}
			}
		}

		public global::UnityEngine.Color GetColor(float angle)
		{
			return global::Pathfinding.AstarMath.HSVToRGB(angle * 57.2957764f, 0.8f, 0.6f);
		}

		private const float rad2Deg = 57.2957764f;

		public global::UnityEngine.GUIStyle selectionBox;

		public bool adjustCamera = true;

		private global::UnityEngine.Vector2 start;

		private global::UnityEngine.Vector2 end;

		private bool wasDown;

		private global::System.Collections.Generic.List<global::Pathfinding.Examples.RVOExampleAgent> selection = new global::System.Collections.Generic.List<global::Pathfinding.Examples.RVOExampleAgent>();

		private global::Pathfinding.RVO.Simulator sim;

		private global::UnityEngine.Camera cam;
	}
}
