using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_path_types_demo.php")]
	public class PathTypesDemo : global::UnityEngine.MonoBehaviour
	{
		private void Update()
		{
			global::UnityEngine.Ray ray = global::UnityEngine.Camera.main.ScreenPointToRay(global::UnityEngine.Input.mousePosition);
			global::UnityEngine.Vector3 vector = ray.origin + ray.direction * (ray.origin.y / -ray.direction.y);
			this.end.position = vector;
			if (global::UnityEngine.Input.GetMouseButtonDown(0))
			{
				this.mouseDragStart = global::UnityEngine.Input.mousePosition;
				this.mouseDragStartTime = global::UnityEngine.Time.realtimeSinceStartup;
			}
			if (global::UnityEngine.Input.GetMouseButtonUp(0))
			{
				global::UnityEngine.Vector2 a = global::UnityEngine.Input.mousePosition;
				if ((a - this.mouseDragStart).sqrMagnitude > 25f && global::UnityEngine.Time.realtimeSinceStartup - this.mouseDragStartTime > 0.3f)
				{
					global::UnityEngine.Rect rect = global::UnityEngine.Rect.MinMaxRect(global::UnityEngine.Mathf.Min(this.mouseDragStart.x, a.x), global::UnityEngine.Mathf.Min(this.mouseDragStart.y, a.y), global::UnityEngine.Mathf.Max(this.mouseDragStart.x, a.x), global::UnityEngine.Mathf.Max(this.mouseDragStart.y, a.y));
					global::Pathfinding.RichAI[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.RichAI)) as global::Pathfinding.RichAI[];
					global::System.Collections.Generic.List<global::Pathfinding.RichAI> list = new global::System.Collections.Generic.List<global::Pathfinding.RichAI>();
					for (int i = 0; i < array.Length; i++)
					{
						global::UnityEngine.Vector2 point = global::UnityEngine.Camera.main.WorldToScreenPoint(array[i].transform.position);
						if (rect.Contains(point))
						{
							list.Add(array[i]);
						}
					}
					this.agents = list.ToArray();
				}
				else
				{
					if (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.LeftShift))
					{
						this.multipoints.Add(vector);
					}
					if (global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.LeftControl))
					{
						this.multipoints.Clear();
					}
					if (global::UnityEngine.Input.mousePosition.x > 225f)
					{
						this.DemoPath();
					}
				}
			}
			if (global::UnityEngine.Input.GetMouseButton(0) && global::UnityEngine.Input.GetKey(global::UnityEngine.KeyCode.LeftAlt) && (this.lastPath == null || this.lastPath.IsDone()))
			{
				this.DemoPath();
			}
		}

		public void OnGUI()
		{
			global::UnityEngine.GUILayout.BeginArea(new global::UnityEngine.Rect(5f, 5f, 220f, (float)(global::UnityEngine.Screen.height - 10)), string.Empty, "Box");
			switch (this.activeDemo)
			{
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.ABPath:
				global::UnityEngine.GUILayout.Label("Basic path. Finds a path from point A to point B.", new global::UnityEngine.GUILayoutOption[0]);
				break;
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.MultiTargetPath:
				global::UnityEngine.GUILayout.Label("Multi Target Path. Finds a path quickly from one point to many others in a single search.", new global::UnityEngine.GUILayoutOption[0]);
				break;
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.RandomPath:
				global::UnityEngine.GUILayout.Label("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth.", new global::UnityEngine.GUILayoutOption[0]);
				break;
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.FleePath:
				global::UnityEngine.GUILayout.Label("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!", new global::UnityEngine.GUILayoutOption[0]);
				break;
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.ConstantPath:
				global::UnityEngine.GUILayout.Label("Finds all nodes which it costs less than some value to reach.", new global::UnityEngine.GUILayoutOption[0]);
				break;
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.FloodPath:
				global::UnityEngine.GUILayout.Label("Searches the whole graph from a specific point. FloodPathTracer can then be used to quickly find a path to that point", new global::UnityEngine.GUILayoutOption[0]);
				break;
			case global::Pathfinding.Examples.PathTypesDemo.DemoMode.FloodPathTracer:
				global::UnityEngine.GUILayout.Label("Traces a path to where the FloodPath started. Compare the claculation times for this path with ABPath!\nGreat for TD games", new global::UnityEngine.GUILayoutOption[0]);
				break;
			}
			global::UnityEngine.GUILayout.Space(5f);
			global::UnityEngine.GUILayout.Label("Note that the paths are rendered without ANY post-processing applied, so they might look a bit edgy", new global::UnityEngine.GUILayoutOption[0]);
			global::UnityEngine.GUILayout.Space(5f);
			global::UnityEngine.GUILayout.Label("Click anywhere to recalculate the path. Hold Alt to continuously recalculate the path while the mouse is pressed.", new global::UnityEngine.GUILayoutOption[0]);
			if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.ConstantPath || this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.RandomPath || this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.FleePath)
			{
				global::UnityEngine.GUILayout.Label("Search Distance (" + this.searchLength + ")", new global::UnityEngine.GUILayoutOption[0]);
				this.searchLength = global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.GUILayout.HorizontalSlider((float)this.searchLength, 0f, 100000f, new global::UnityEngine.GUILayoutOption[0]));
			}
			if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.RandomPath || this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.FleePath)
			{
				global::UnityEngine.GUILayout.Label("Spread (" + this.spread + ")", new global::UnityEngine.GUILayoutOption[0]);
				this.spread = global::UnityEngine.Mathf.RoundToInt(global::UnityEngine.GUILayout.HorizontalSlider((float)this.spread, 0f, 40000f, new global::UnityEngine.GUILayoutOption[0]));
				global::UnityEngine.GUILayout.Label(string.Concat(new object[]
				{
					(this.activeDemo != global::Pathfinding.Examples.PathTypesDemo.DemoMode.RandomPath) ? "Flee strength" : "Aim strength",
					" (",
					this.aimStrength,
					")"
				}), new global::UnityEngine.GUILayoutOption[0]);
				this.aimStrength = global::UnityEngine.GUILayout.HorizontalSlider(this.aimStrength, 0f, 1f, new global::UnityEngine.GUILayoutOption[0]);
			}
			if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.MultiTargetPath)
			{
				global::UnityEngine.GUILayout.Label("Hold shift and click to add new target points. Hold ctr and click to remove all target points", new global::UnityEngine.GUILayoutOption[0]);
			}
			if (global::UnityEngine.GUILayout.Button("A to B path", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.ABPath;
			}
			if (global::UnityEngine.GUILayout.Button("Multi Target Path", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.MultiTargetPath;
			}
			if (global::UnityEngine.GUILayout.Button("Random Path", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.RandomPath;
			}
			if (global::UnityEngine.GUILayout.Button("Flee path", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.FleePath;
			}
			if (global::UnityEngine.GUILayout.Button("Constant Path", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.ConstantPath;
			}
			if (global::UnityEngine.GUILayout.Button("Flood Path", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.FloodPath;
			}
			if (global::UnityEngine.GUILayout.Button("Flood Path Tracer", new global::UnityEngine.GUILayoutOption[0]))
			{
				this.activeDemo = global::Pathfinding.Examples.PathTypesDemo.DemoMode.FloodPathTracer;
			}
			global::UnityEngine.GUILayout.EndArea();
		}

		public void OnPathComplete(global::Pathfinding.Path p)
		{
			if (this.lastRender == null)
			{
				return;
			}
			if (p.error)
			{
				this.ClearPrevious();
				return;
			}
			if (p.GetType() == typeof(global::Pathfinding.MultiTargetPath))
			{
				global::System.Collections.Generic.List<global::UnityEngine.GameObject> list = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>(this.lastRender);
				this.lastRender.Clear();
				global::Pathfinding.MultiTargetPath multiTargetPath = p as global::Pathfinding.MultiTargetPath;
				for (int i = 0; i < multiTargetPath.vectorPaths.Length; i++)
				{
					if (multiTargetPath.vectorPaths[i] != null)
					{
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = multiTargetPath.vectorPaths[i];
						global::UnityEngine.GameObject gameObject;
						if (list.Count > i && list[i].GetComponent<global::UnityEngine.LineRenderer>() != null)
						{
							gameObject = list[i];
							list.RemoveAt(i);
						}
						else
						{
							gameObject = new global::UnityEngine.GameObject("LineRenderer_" + i, new global::System.Type[]
							{
								typeof(global::UnityEngine.LineRenderer)
							});
						}
						global::UnityEngine.LineRenderer component = gameObject.GetComponent<global::UnityEngine.LineRenderer>();
						component.sharedMaterial = this.lineMat;
						component.SetWidth(this.lineWidth, this.lineWidth);
						component.SetVertexCount(list2.Count);
						for (int j = 0; j < list2.Count; j++)
						{
							component.SetPosition(j, list2[j] + this.pathOffset);
						}
						this.lastRender.Add(gameObject);
					}
				}
				for (int k = 0; k < list.Count; k++)
				{
					global::UnityEngine.Object.Destroy(list[k]);
				}
			}
			else if (p.GetType() == typeof(global::Pathfinding.ConstantPath))
			{
				this.ClearPrevious();
				global::Pathfinding.ConstantPath constantPath = p as global::Pathfinding.ConstantPath;
				global::System.Collections.Generic.List<global::Pathfinding.GraphNode> allNodes = constantPath.allNodes;
				global::UnityEngine.Mesh mesh = new global::UnityEngine.Mesh();
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
				bool flag = false;
				for (int l = allNodes.Count - 1; l >= 0; l--)
				{
					global::UnityEngine.Vector3 a = (global::UnityEngine.Vector3)allNodes[l].position + this.pathOffset;
					if (list3.Count == 65000 && !flag)
					{
						global::UnityEngine.Debug.LogError("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
						flag = true;
					}
					if (flag)
					{
						global::UnityEngine.Debug.DrawRay(a, global::UnityEngine.Vector3.up, global::UnityEngine.Color.blue);
					}
					else
					{
						global::Pathfinding.GridGraph gridGraph = global::Pathfinding.AstarData.GetGraph(allNodes[l]) as global::Pathfinding.GridGraph;
						float d = 1f;
						if (gridGraph != null)
						{
							d = gridGraph.nodeSize;
						}
						list3.Add(a + new global::UnityEngine.Vector3(-0.5f, 0f, -0.5f) * d);
						list3.Add(a + new global::UnityEngine.Vector3(0.5f, 0f, -0.5f) * d);
						list3.Add(a + new global::UnityEngine.Vector3(-0.5f, 0f, 0.5f) * d);
						list3.Add(a + new global::UnityEngine.Vector3(0.5f, 0f, 0.5f) * d);
					}
				}
				global::UnityEngine.Vector3[] array = list3.ToArray();
				int[] array2 = new int[3 * array.Length / 2];
				int m = 0;
				int num = 0;
				while (m < array.Length)
				{
					array2[num] = m;
					array2[num + 1] = m + 1;
					array2[num + 2] = m + 2;
					array2[num + 3] = m + 1;
					array2[num + 4] = m + 3;
					array2[num + 5] = m + 2;
					num += 6;
					m += 4;
				}
				global::UnityEngine.Vector2[] array3 = new global::UnityEngine.Vector2[array.Length];
				for (int n = 0; n < array3.Length; n += 4)
				{
					array3[n] = new global::UnityEngine.Vector2(0f, 0f);
					array3[n + 1] = new global::UnityEngine.Vector2(1f, 0f);
					array3[n + 2] = new global::UnityEngine.Vector2(0f, 1f);
					array3[n + 3] = new global::UnityEngine.Vector2(1f, 1f);
				}
				mesh.vertices = array;
				mesh.triangles = array2;
				mesh.uv = array3;
				mesh.RecalculateNormals();
				global::UnityEngine.GameObject gameObject2 = new global::UnityEngine.GameObject("Mesh", new global::System.Type[]
				{
					typeof(global::UnityEngine.MeshRenderer),
					typeof(global::UnityEngine.MeshFilter)
				});
				global::UnityEngine.MeshFilter component2 = gameObject2.GetComponent<global::UnityEngine.MeshFilter>();
				component2.mesh = mesh;
				global::UnityEngine.MeshRenderer component3 = gameObject2.GetComponent<global::UnityEngine.MeshRenderer>();
				component3.material = this.squareMat;
				this.lastRender.Add(gameObject2);
			}
			else
			{
				this.ClearPrevious();
				global::UnityEngine.GameObject gameObject3 = new global::UnityEngine.GameObject("LineRenderer", new global::System.Type[]
				{
					typeof(global::UnityEngine.LineRenderer)
				});
				global::UnityEngine.LineRenderer component4 = gameObject3.GetComponent<global::UnityEngine.LineRenderer>();
				component4.sharedMaterial = this.lineMat;
				component4.SetWidth(this.lineWidth, this.lineWidth);
				component4.SetVertexCount(p.vectorPath.Count);
				for (int num2 = 0; num2 < p.vectorPath.Count; num2++)
				{
					component4.SetPosition(num2, p.vectorPath[num2] + this.pathOffset);
				}
				this.lastRender.Add(gameObject3);
			}
		}

		private void ClearPrevious()
		{
			for (int i = 0; i < this.lastRender.Count; i++)
			{
				global::UnityEngine.Object.Destroy(this.lastRender[i]);
			}
			this.lastRender.Clear();
		}

		private void OnApplicationQuit()
		{
			this.ClearPrevious();
			this.lastRender = null;
		}

		private void DemoPath()
		{
			global::Pathfinding.Path path = null;
			if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.ABPath)
			{
				path = global::Pathfinding.ABPath.Construct(this.start.position, this.end.position, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
				if (this.agents != null && this.agents.Length > 0)
				{
					global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim(this.agents.Length);
					global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
					for (int i = 0; i < this.agents.Length; i++)
					{
						list.Add(this.agents[i].transform.position);
						vector += list[i];
					}
					vector /= (float)list.Count;
					for (int j = 0; j < this.agents.Length; j++)
					{
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3;
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = list3 = list;
						int index2;
						int index = index2 = j;
						global::UnityEngine.Vector3 a = list3[index2];
						list2[index] = a - vector;
					}
					global::Pathfinding.PathUtilities.GetPointsAroundPoint(this.end.position, global::AstarPath.active.graphs[0] as global::Pathfinding.IRaycastableGraph, list, 0f, 0.2f);
					for (int k = 0; k < this.agents.Length; k++)
					{
						if (!(this.agents[k] == null))
						{
							this.agents[k].target.position = list[k];
							this.agents[k].UpdatePath();
						}
					}
				}
			}
			else if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.MultiTargetPath)
			{
				global::Pathfinding.MultiTargetPath multiTargetPath = global::Pathfinding.MultiTargetPath.Construct(this.multipoints.ToArray(), this.end.position, null, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
				path = multiTargetPath;
			}
			else if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.RandomPath)
			{
				global::Pathfinding.RandomPath randomPath = global::Pathfinding.RandomPath.Construct(this.start.position, this.searchLength, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
				randomPath.spread = this.spread;
				randomPath.aimStrength = this.aimStrength;
				randomPath.aim = this.end.position;
				path = randomPath;
			}
			else if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.FleePath)
			{
				global::Pathfinding.FleePath fleePath = global::Pathfinding.FleePath.Construct(this.start.position, this.end.position, this.searchLength, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
				fleePath.aimStrength = this.aimStrength;
				fleePath.spread = this.spread;
				path = fleePath;
			}
			else if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.ConstantPath)
			{
				base.StartCoroutine(this.CalculateConstantPath());
				path = null;
			}
			else if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.FloodPath)
			{
				global::Pathfinding.FloodPath floodPath = global::Pathfinding.FloodPath.Construct(this.end.position, null);
				this.lastFlood = floodPath;
				path = floodPath;
			}
			else if (this.activeDemo == global::Pathfinding.Examples.PathTypesDemo.DemoMode.FloodPathTracer && this.lastFlood != null)
			{
				global::Pathfinding.FloodPathTracer floodPathTracer = global::Pathfinding.FloodPathTracer.Construct(this.end.position, this.lastFlood, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
				path = floodPathTracer;
			}
			if (path != null)
			{
				global::AstarPath.StartPath(path, false);
				this.lastPath = path;
			}
		}

		public global::System.Collections.IEnumerator CalculateConstantPath()
		{
			global::Pathfinding.ConstantPath constPath = global::Pathfinding.ConstantPath.Construct(this.end.position, this.searchLength, new global::Pathfinding.OnPathDelegate(this.OnPathComplete));
			global::AstarPath.StartPath(constPath, false);
			this.lastPath = constPath;
			yield return constPath.WaitForPath();
			yield break;
		}

		public global::Pathfinding.Examples.PathTypesDemo.DemoMode activeDemo;

		public global::UnityEngine.Transform start;

		public global::UnityEngine.Transform end;

		public global::UnityEngine.Vector3 pathOffset;

		public global::UnityEngine.Material lineMat;

		public global::UnityEngine.Material squareMat;

		public float lineWidth;

		public global::Pathfinding.RichAI[] agents;

		public int searchLength = 1000;

		public int spread = 100;

		public float aimStrength;

		private global::Pathfinding.Path lastPath;

		private global::System.Collections.Generic.List<global::UnityEngine.GameObject> lastRender = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> multipoints = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();

		private global::UnityEngine.Vector2 mouseDragStart;

		private float mouseDragStartTime;

		private global::Pathfinding.FloodPath lastFlood;

		public enum DemoMode
		{
			ABPath,
			MultiTargetPath,
			RandomPath,
			FleePath,
			ConstantPath,
			FloodPath,
			FloodPathTracer
		}
	}
}
