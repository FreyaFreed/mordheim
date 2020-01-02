using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_door_controller.php")]
	public class DoorController : global::UnityEngine.MonoBehaviour
	{
		public void Start()
		{
			this.bounds = base.GetComponent<global::UnityEngine.Collider>().bounds;
			this.SetState(this.open);
		}

		private void OnGUI()
		{
			if (global::UnityEngine.GUI.Button(new global::UnityEngine.Rect(5f, this.yOffset, 100f, 22f), "Toggle Door"))
			{
				this.SetState(!this.open);
			}
		}

		public void SetState(bool open)
		{
			this.open = open;
			if (this.updateGraphsWithGUO)
			{
				global::Pathfinding.GraphUpdateObject graphUpdateObject = new global::Pathfinding.GraphUpdateObject(this.bounds);
				int num = (!open) ? this.closedtag : this.opentag;
				if (num > 31)
				{
					global::UnityEngine.Debug.LogError("tag > 31");
					return;
				}
				graphUpdateObject.modifyTag = true;
				graphUpdateObject.setTag = num;
				graphUpdateObject.updatePhysics = false;
				global::AstarPath.active.UpdateGraphs(graphUpdateObject);
			}
			if (open)
			{
				base.GetComponent<global::UnityEngine.Animation>().Play("Open");
			}
			else
			{
				base.GetComponent<global::UnityEngine.Animation>().Play("Close");
			}
		}

		private bool open;

		public int opentag = 1;

		public int closedtag = 1;

		public bool updateGraphsWithGUO = true;

		public float yOffset = 5f;

		private global::UnityEngine.Bounds bounds;
	}
}
