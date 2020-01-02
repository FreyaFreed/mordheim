using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.ExecuteInEditMode]
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_unity_reference_helper.php")]
	public class UnityReferenceHelper : global::UnityEngine.MonoBehaviour
	{
		public string GetGUID()
		{
			return this.guid;
		}

		public void Awake()
		{
			this.Reset();
		}

		public void Reset()
		{
			if (string.IsNullOrEmpty(this.guid))
			{
				this.guid = global::Pathfinding.Util.Guid.NewGuid().ToString();
				global::UnityEngine.Debug.Log("Created new GUID - " + this.guid);
			}
			else
			{
				foreach (global::Pathfinding.UnityReferenceHelper unityReferenceHelper in global::UnityEngine.Object.FindObjectsOfType(typeof(global::Pathfinding.UnityReferenceHelper)) as global::Pathfinding.UnityReferenceHelper[])
				{
					if (unityReferenceHelper != this && this.guid == unityReferenceHelper.guid)
					{
						this.guid = global::Pathfinding.Util.Guid.NewGuid().ToString();
						global::UnityEngine.Debug.Log("Created new GUID - " + this.guid);
						return;
					}
				}
			}
		}

		[global::UnityEngine.SerializeField]
		[global::UnityEngine.HideInInspector]
		private string guid;
	}
}
