using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_object_placer.php")]
	public class ObjectPlacer : global::UnityEngine.MonoBehaviour
	{
		private void Update()
		{
			if (global::UnityEngine.Input.GetKeyDown("p"))
			{
				this.PlaceObject();
			}
			if (global::UnityEngine.Input.GetKeyDown("r"))
			{
				this.RemoveObject();
			}
		}

		public void PlaceObject()
		{
			global::UnityEngine.Ray ray = global::UnityEngine.Camera.main.ScreenPointToRay(global::UnityEngine.Input.mousePosition);
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.Raycast(ray, out raycastHit, float.PositiveInfinity))
			{
				global::UnityEngine.Vector3 point = raycastHit.point;
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(this.go, point, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
				if (this.issueGUOs)
				{
					global::UnityEngine.Bounds bounds = gameObject.GetComponent<global::UnityEngine.Collider>().bounds;
					global::Pathfinding.GraphUpdateObject ob = new global::Pathfinding.GraphUpdateObject(bounds);
					global::AstarPath.active.UpdateGraphs(ob);
					if (this.direct)
					{
						global::AstarPath.active.FlushGraphUpdates();
					}
				}
			}
		}

		public void RemoveObject()
		{
			global::UnityEngine.Ray ray = global::UnityEngine.Camera.main.ScreenPointToRay(global::UnityEngine.Input.mousePosition);
			global::UnityEngine.RaycastHit raycastHit;
			if (global::UnityEngine.Physics.Raycast(ray, out raycastHit, float.PositiveInfinity))
			{
				if (raycastHit.collider.isTrigger || raycastHit.transform.gameObject.name == "Ground")
				{
					return;
				}
				global::UnityEngine.Bounds bounds = raycastHit.collider.bounds;
				global::UnityEngine.Object.Destroy(raycastHit.collider);
				global::UnityEngine.Object.Destroy(raycastHit.collider.gameObject);
				if (this.issueGUOs)
				{
					global::Pathfinding.GraphUpdateObject ob = new global::Pathfinding.GraphUpdateObject(bounds);
					global::AstarPath.active.UpdateGraphs(ob);
					if (this.direct)
					{
						global::AstarPath.active.FlushGraphUpdates();
					}
				}
			}
		}

		public global::UnityEngine.GameObject go;

		public bool direct;

		public bool issueGUOs = true;
	}
}
