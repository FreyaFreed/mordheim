using System;
using System.Collections;
using UnityEngine;

namespace Pathfinding.Examples
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_examples_1_1_r_v_o_agent_placer.php")]
	public class RVOAgentPlacer : global::UnityEngine.MonoBehaviour
	{
		private global::System.Collections.IEnumerator Start()
		{
			yield return null;
			for (int i = 0; i < this.agents; i++)
			{
				float angle = (float)i / (float)this.agents * 3.14159274f * 2f;
				global::UnityEngine.Vector3 pos = new global::UnityEngine.Vector3((float)global::System.Math.Cos((double)angle), 0f, (float)global::System.Math.Sin((double)angle)) * this.ringSize;
				global::UnityEngine.Vector3 antipodal = -pos + this.goalOffset;
				global::UnityEngine.GameObject go = global::UnityEngine.Object.Instantiate(this.prefab, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.Euler(0f, angle + 180f, 0f)) as global::UnityEngine.GameObject;
				global::Pathfinding.Examples.RVOExampleAgent ag = go.GetComponent<global::Pathfinding.Examples.RVOExampleAgent>();
				if (ag == null)
				{
					global::UnityEngine.Debug.LogError("Prefab does not have an RVOExampleAgent component attached");
					yield break;
				}
				go.transform.parent = base.transform;
				go.transform.position = pos;
				ag.repathRate = this.repathRate;
				ag.SetTarget(antipodal);
				ag.SetColor(this.GetColor(angle));
			}
			yield break;
		}

		public global::UnityEngine.Color GetColor(float angle)
		{
			return global::Pathfinding.AstarMath.HSVToRGB(angle * 57.2957764f, 0.8f, 0.6f);
		}

		private const float rad2Deg = 57.2957764f;

		public int agents = 100;

		public float ringSize = 100f;

		public global::UnityEngine.LayerMask mask;

		public global::UnityEngine.GameObject prefab;

		public global::UnityEngine.Vector3 goalOffset;

		public float repathRate = 1f;
	}
}
