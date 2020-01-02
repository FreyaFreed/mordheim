using System;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_target_mover.php")]
	public class TargetMover : global::UnityEngine.MonoBehaviour
	{
		public void Start()
		{
			this.cam = global::UnityEngine.Camera.main;
			this.ais = global::UnityEngine.Object.FindObjectsOfType<global::Pathfinding.RichAI>();
			this.ais2 = global::UnityEngine.Object.FindObjectsOfType<global::AIPath>();
			this.ais3 = global::UnityEngine.Object.FindObjectsOfType<global::AILerp>();
			base.useGUILayout = false;
		}

		public void OnGUI()
		{
			if (this.onlyOnDoubleClick && this.cam != null && global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseDown && global::UnityEngine.Event.current.clickCount == 2)
			{
				this.UpdateTargetPosition();
			}
		}

		private void Update()
		{
			if (!this.onlyOnDoubleClick && this.cam != null)
			{
				this.UpdateTargetPosition();
			}
		}

		public void UpdateTargetPosition()
		{
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			bool flag = false;
			global::UnityEngine.RaycastHit raycastHit;
			if (this.use2D)
			{
				vector = this.cam.ScreenToWorldPoint(global::UnityEngine.Input.mousePosition);
				vector.z = 0f;
				flag = true;
			}
			else if (global::UnityEngine.Physics.Raycast(this.cam.ScreenPointToRay(global::UnityEngine.Input.mousePosition), out raycastHit, float.PositiveInfinity, this.mask))
			{
				vector = raycastHit.point;
				flag = true;
			}
			if (flag && vector != this.target.position)
			{
				this.target.position = vector;
				if (this.onlyOnDoubleClick)
				{
					if (this.ais != null)
					{
						for (int i = 0; i < this.ais.Length; i++)
						{
							if (this.ais[i] != null)
							{
								this.ais[i].UpdatePath();
							}
						}
					}
					if (this.ais2 != null)
					{
						for (int j = 0; j < this.ais2.Length; j++)
						{
							if (this.ais2[j] != null)
							{
								this.ais2[j].SearchPath();
							}
						}
					}
					if (this.ais3 != null)
					{
						for (int k = 0; k < this.ais3.Length; k++)
						{
							if (this.ais3[k] != null)
							{
								this.ais3[k].SearchPath();
							}
						}
					}
				}
			}
		}

		public global::UnityEngine.LayerMask mask;

		public global::UnityEngine.Transform target;

		private global::Pathfinding.RichAI[] ais;

		private global::AIPath[] ais2;

		private global::AILerp[] ais3;

		public bool onlyOnDoubleClick;

		public bool use2D;

		private global::UnityEngine.Camera cam;
	}
}
