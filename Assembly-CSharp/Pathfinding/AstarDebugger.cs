using System;
using System.Text;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.AddComponentMenu("Pathfinding/Pathfinding Debugger")]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_astar_debugger.php")]
	[global::UnityEngine.ExecuteInEditMode]
	public class AstarDebugger : global::UnityEngine.MonoBehaviour
	{
		public AstarDebugger()
		{
			global::Pathfinding.AstarDebugger.PathTypeDebug[] array = new global::Pathfinding.AstarDebugger.PathTypeDebug[7];
			array[0] = new global::Pathfinding.AstarDebugger.PathTypeDebug("ABPath", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.ABPath)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.ABPath)));
			array[1] = new global::Pathfinding.AstarDebugger.PathTypeDebug("MultiTargetPath", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.MultiTargetPath)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.MultiTargetPath)));
			array[2] = new global::Pathfinding.AstarDebugger.PathTypeDebug("RandomPath", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.RandomPath)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.RandomPath)));
			array[3] = new global::Pathfinding.AstarDebugger.PathTypeDebug("FleePath", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.FleePath)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.FleePath)));
			array[4] = new global::Pathfinding.AstarDebugger.PathTypeDebug("ConstantPath", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.ConstantPath)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.ConstantPath)));
			array[5] = new global::Pathfinding.AstarDebugger.PathTypeDebug("FloodPath", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.FloodPath)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.FloodPath)));
			array[6] = new global::Pathfinding.AstarDebugger.PathTypeDebug("FloodPathTracer", () => global::Pathfinding.PathPool.GetSize(typeof(global::Pathfinding.FloodPathTracer)), () => global::Pathfinding.PathPool.GetTotalCreated(typeof(global::Pathfinding.FloodPathTracer)));
			this.debugTypes = array;
			base..ctor();
		}

		public void Start()
		{
			base.useGUILayout = false;
			this.fpsDrops = new float[this.fpsDropCounterSize];
			this.cam = base.GetComponent<global::UnityEngine.Camera>();
			if (this.cam == null)
			{
				this.cam = global::UnityEngine.Camera.main;
			}
			this.graph = new global::Pathfinding.AstarDebugger.GraphPoint[this.graphBufferSize];
			if (global::UnityEngine.Time.unscaledDeltaTime > 0f)
			{
				for (int i = 0; i < this.fpsDrops.Length; i++)
				{
					this.fpsDrops[i] = 1f / global::UnityEngine.Time.unscaledDeltaTime;
				}
			}
		}

		public void LateUpdate()
		{
			if (!this.show || (!global::UnityEngine.Application.isPlaying && !this.showInEditor))
			{
				return;
			}
			if (global::UnityEngine.Time.unscaledDeltaTime <= 0.0001f)
			{
				return;
			}
			int num = global::System.GC.CollectionCount(0);
			if (this.lastCollectNum != (float)num)
			{
				this.lastCollectNum = (float)num;
				this.delta = global::UnityEngine.Time.realtimeSinceStartup - this.lastCollect;
				this.lastCollect = global::UnityEngine.Time.realtimeSinceStartup;
				this.lastDeltaTime = global::UnityEngine.Time.unscaledDeltaTime;
				this.collectAlloc = this.allocMem;
			}
			this.allocMem = (int)global::System.GC.GetTotalMemory(false);
			bool flag = this.allocMem < this.peakAlloc;
			this.peakAlloc = (flag ? this.peakAlloc : this.allocMem);
			if (global::UnityEngine.Time.realtimeSinceStartup - this.lastAllocSet > 0.3f || !global::UnityEngine.Application.isPlaying)
			{
				int num2 = this.allocMem - this.lastAllocMemory;
				this.lastAllocMemory = this.allocMem;
				this.lastAllocSet = global::UnityEngine.Time.realtimeSinceStartup;
				this.delayedDeltaTime = global::UnityEngine.Time.unscaledDeltaTime;
				if (num2 >= 0)
				{
					this.allocRate = num2;
				}
			}
			if (global::UnityEngine.Application.isPlaying)
			{
				this.fpsDrops[global::UnityEngine.Time.frameCount % this.fpsDrops.Length] = ((global::UnityEngine.Time.unscaledDeltaTime <= 1E-05f) ? 0f : (1f / global::UnityEngine.Time.unscaledDeltaTime));
				int num3 = global::UnityEngine.Time.frameCount % this.graph.Length;
				this.graph[num3].fps = ((global::UnityEngine.Time.unscaledDeltaTime >= 1E-05f) ? 0f : (1f / global::UnityEngine.Time.unscaledDeltaTime));
				this.graph[num3].collectEvent = flag;
				this.graph[num3].memory = (float)this.allocMem;
			}
			if (global::UnityEngine.Application.isPlaying && this.cam != null && this.showGraph)
			{
				this.graphWidth = (float)this.cam.pixelWidth * 0.8f;
				float num4 = float.PositiveInfinity;
				float num5 = 0f;
				float num6 = float.PositiveInfinity;
				float num7 = 0f;
				for (int i = 0; i < this.graph.Length; i++)
				{
					num4 = global::UnityEngine.Mathf.Min(this.graph[i].memory, num4);
					num5 = global::UnityEngine.Mathf.Max(this.graph[i].memory, num5);
					num6 = global::UnityEngine.Mathf.Min(this.graph[i].fps, num6);
					num7 = global::UnityEngine.Mathf.Max(this.graph[i].fps, num7);
				}
				int num8 = global::UnityEngine.Time.frameCount % this.graph.Length;
				global::UnityEngine.Matrix4x4 m = global::UnityEngine.Matrix4x4.TRS(new global::UnityEngine.Vector3(((float)this.cam.pixelWidth - this.graphWidth) / 2f, this.graphOffset, 1f), global::UnityEngine.Quaternion.identity, new global::UnityEngine.Vector3(this.graphWidth, this.graphHeight, 1f));
				for (int j = 0; j < this.graph.Length - 1; j++)
				{
					if (j != num8)
					{
						this.DrawGraphLine(j, m, (float)j / (float)this.graph.Length, (float)(j + 1) / (float)this.graph.Length, global::Pathfinding.AstarMath.MapTo(num4, num5, this.graph[j].memory), global::Pathfinding.AstarMath.MapTo(num4, num5, this.graph[j + 1].memory), global::UnityEngine.Color.blue);
						this.DrawGraphLine(j, m, (float)j / (float)this.graph.Length, (float)(j + 1) / (float)this.graph.Length, global::Pathfinding.AstarMath.MapTo(num6, num7, this.graph[j].fps), global::Pathfinding.AstarMath.MapTo(num6, num7, this.graph[j + 1].fps), global::UnityEngine.Color.green);
					}
				}
			}
		}

		private void DrawGraphLine(int index, global::UnityEngine.Matrix4x4 m, float x1, float x2, float y1, float y2, global::UnityEngine.Color col)
		{
			global::UnityEngine.Debug.DrawLine(this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new global::UnityEngine.Vector3(x1, y1))), this.cam.ScreenToWorldPoint(m.MultiplyPoint3x4(new global::UnityEngine.Vector3(x2, y2))), col);
		}

		public void OnGUI()
		{
			if (!this.show || (!global::UnityEngine.Application.isPlaying && !this.showInEditor))
			{
				return;
			}
			if (this.style == null)
			{
				this.style = new global::UnityEngine.GUIStyle();
				this.style.normal.textColor = global::UnityEngine.Color.white;
				this.style.padding = new global::UnityEngine.RectOffset(5, 5, 5, 5);
			}
			if (global::UnityEngine.Time.realtimeSinceStartup - this.lastUpdate > 0.5f || this.cachedText == null || !global::UnityEngine.Application.isPlaying)
			{
				this.lastUpdate = global::UnityEngine.Time.realtimeSinceStartup;
				this.boxRect = new global::UnityEngine.Rect(5f, (float)this.yOffset, 310f, 40f);
				this.text.Length = 0;
				this.text.AppendLine("A* Pathfinding Project Debugger");
				this.text.Append("A* Version: ").Append(global::AstarPath.Version.ToString());
				if (this.showMemProfile)
				{
					this.boxRect.height = this.boxRect.height + 200f;
					this.text.AppendLine();
					this.text.AppendLine();
					this.text.Append("Currently allocated".PadRight(25));
					this.text.Append(((float)this.allocMem / 1000000f).ToString("0.0 MB"));
					this.text.AppendLine();
					this.text.Append("Peak allocated".PadRight(25));
					this.text.Append(((float)this.peakAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Last collect peak".PadRight(25));
					this.text.Append(((float)this.collectAlloc / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Allocation rate".PadRight(25));
					this.text.Append(((float)this.allocRate / 1000000f).ToString("0.0 MB")).AppendLine();
					this.text.Append("Collection frequency".PadRight(25));
					this.text.Append(this.delta.ToString("0.00"));
					this.text.Append("s\n");
					this.text.Append("Last collect fps".PadRight(25));
					this.text.Append((1f / this.lastDeltaTime).ToString("0.0 fps"));
					this.text.Append(" (");
					this.text.Append(this.lastDeltaTime.ToString("0.000 s"));
					this.text.Append(")");
				}
				if (this.showFPS)
				{
					this.text.AppendLine();
					this.text.AppendLine();
					float num = (this.delayedDeltaTime <= 1E-05f) ? 0f : (1f / this.delayedDeltaTime);
					this.text.Append("FPS".PadRight(25)).Append(num.ToString("0.0 fps"));
					float num2 = float.PositiveInfinity;
					for (int i = 0; i < this.fpsDrops.Length; i++)
					{
						if (this.fpsDrops[i] < num2)
						{
							num2 = this.fpsDrops[i];
						}
					}
					this.text.AppendLine();
					this.text.Append(("Lowest fps (last " + this.fpsDrops.Length + ")").PadRight(25)).Append(num2.ToString("0.0"));
				}
				if (this.showPathProfile)
				{
					global::AstarPath active = global::AstarPath.active;
					this.text.AppendLine();
					if (active == null)
					{
						this.text.Append("\nNo AstarPath Object In The Scene");
					}
					else
					{
						if (global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.GetSize() > this.maxVecPool)
						{
							this.maxVecPool = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.GetSize();
						}
						if (global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.GetSize() > this.maxNodePool)
						{
							this.maxNodePool = global::Pathfinding.Util.ListPool<global::Pathfinding.GraphNode>.GetSize();
						}
						this.text.Append("\nPool Sizes (size/total created)");
						for (int j = 0; j < this.debugTypes.Length; j++)
						{
							this.debugTypes[j].Print(this.text);
						}
					}
				}
				this.cachedText = this.text.ToString();
			}
			if (this.font != null)
			{
				this.style.font = this.font;
				this.style.fontSize = this.fontSize;
			}
			this.boxRect.height = this.style.CalcHeight(new global::UnityEngine.GUIContent(this.cachedText), this.boxRect.width);
			global::UnityEngine.GUI.Box(this.boxRect, string.Empty);
			global::UnityEngine.GUI.Label(this.boxRect, this.cachedText, this.style);
			if (this.showGraph)
			{
				float num3 = float.PositiveInfinity;
				float num4 = 0f;
				float num5 = float.PositiveInfinity;
				float num6 = 0f;
				for (int k = 0; k < this.graph.Length; k++)
				{
					num3 = global::UnityEngine.Mathf.Min(this.graph[k].memory, num3);
					num4 = global::UnityEngine.Mathf.Max(this.graph[k].memory, num4);
					num5 = global::UnityEngine.Mathf.Min(this.graph[k].fps, num5);
					num6 = global::UnityEngine.Mathf.Max(this.graph[k].fps, num6);
				}
				global::UnityEngine.GUI.color = global::UnityEngine.Color.blue;
				float num7 = (float)global::UnityEngine.Mathf.RoundToInt(num4 / 100000f);
				global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(5f, (float)global::UnityEngine.Screen.height - global::Pathfinding.AstarMath.MapTo(num3, num4, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				num7 = global::UnityEngine.Mathf.Round(num3 / 100000f);
				global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(5f, (float)global::UnityEngine.Screen.height - global::Pathfinding.AstarMath.MapTo(num3, num4, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7 * 1000f * 100f) - 10f, 100f, 20f), (num7 / 10f).ToString("0.0 MB"));
				global::UnityEngine.GUI.color = global::UnityEngine.Color.green;
				num7 = global::UnityEngine.Mathf.Round(num6);
				global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(55f, (float)global::UnityEngine.Screen.height - global::Pathfinding.AstarMath.MapTo(num5, num6, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
				num7 = global::UnityEngine.Mathf.Round(num5);
				global::UnityEngine.GUI.Label(new global::UnityEngine.Rect(55f, (float)global::UnityEngine.Screen.height - global::Pathfinding.AstarMath.MapTo(num5, num6, 0f + this.graphOffset, this.graphHeight + this.graphOffset, num7) - 10f, 100f, 20f), num7.ToString("0 FPS"));
			}
		}

		public int yOffset = 5;

		public bool show = true;

		public bool showInEditor;

		public bool showFPS;

		public bool showPathProfile;

		public bool showMemProfile;

		public bool showGraph;

		public int graphBufferSize = 200;

		public global::UnityEngine.Font font;

		public int fontSize = 12;

		private global::System.Text.StringBuilder text = new global::System.Text.StringBuilder();

		private string cachedText;

		private float lastUpdate = -999f;

		private global::Pathfinding.AstarDebugger.GraphPoint[] graph;

		private float delayedDeltaTime = 1f;

		private float lastCollect;

		private float lastCollectNum;

		private float delta;

		private float lastDeltaTime;

		private int allocRate;

		private int lastAllocMemory;

		private float lastAllocSet = -9999f;

		private int allocMem;

		private int collectAlloc;

		private int peakAlloc;

		private int fpsDropCounterSize = 200;

		private float[] fpsDrops;

		private global::UnityEngine.Rect boxRect;

		private global::UnityEngine.GUIStyle style;

		private global::UnityEngine.Camera cam;

		private float graphWidth = 100f;

		private float graphHeight = 100f;

		private float graphOffset = 50f;

		private int maxVecPool;

		private int maxNodePool;

		private global::Pathfinding.AstarDebugger.PathTypeDebug[] debugTypes;

		private struct GraphPoint
		{
			public float fps;

			public float memory;

			public bool collectEvent;
		}

		private struct PathTypeDebug
		{
			public PathTypeDebug(string name, global::System.Func<int> getSize, global::System.Func<int> getTotalCreated)
			{
				this.name = name;
				this.getSize = getSize;
				this.getTotalCreated = getTotalCreated;
			}

			public void Print(global::System.Text.StringBuilder text)
			{
				int num = this.getTotalCreated();
				if (num > 0)
				{
					text.Append("\n").Append(("  " + this.name).PadRight(25)).Append(this.getSize()).Append("/").Append(num);
				}
			}

			private string name;

			private global::System.Func<int> getSize;

			private global::System.Func<int> getTotalCreated;
		}
	}
}
