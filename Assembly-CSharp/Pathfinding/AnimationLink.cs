using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_animation_link.php")]
	public class AnimationLink : global::Pathfinding.NodeLink2
	{
		private static global::UnityEngine.Transform SearchRec(global::UnityEngine.Transform tr, string name)
		{
			int childCount = tr.childCount;
			for (int i = 0; i < childCount; i++)
			{
				global::UnityEngine.Transform child = tr.GetChild(i);
				if (child.name == name)
				{
					return child;
				}
				global::UnityEngine.Transform transform = global::Pathfinding.AnimationLink.SearchRec(child, name);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		public void CalculateOffsets(global::System.Collections.Generic.List<global::UnityEngine.Vector3> trace, out global::UnityEngine.Vector3 endPosition)
		{
			endPosition = base.transform.position;
			if (this.referenceMesh == null)
			{
				return;
			}
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(this.referenceMesh, base.transform.position, base.transform.rotation) as global::UnityEngine.GameObject;
			gameObject.hideFlags = global::UnityEngine.HideFlags.HideAndDontSave;
			global::UnityEngine.Transform transform = global::Pathfinding.AnimationLink.SearchRec(gameObject.transform, this.boneRoot);
			if (transform == null)
			{
				throw new global::System.Exception("Could not find root transform");
			}
			global::UnityEngine.Animation animation = gameObject.GetComponent<global::UnityEngine.Animation>();
			if (animation == null)
			{
				animation = gameObject.AddComponent<global::UnityEngine.Animation>();
			}
			for (int i = 0; i < this.sequence.Length; i++)
			{
				animation.AddClip(this.sequence[i].clip, this.sequence[i].clip.name);
			}
			global::UnityEngine.Vector3 a = global::UnityEngine.Vector3.zero;
			global::UnityEngine.Vector3 vector = base.transform.position;
			global::UnityEngine.Vector3 b = global::UnityEngine.Vector3.zero;
			for (int j = 0; j < this.sequence.Length; j++)
			{
				global::Pathfinding.AnimationLink.LinkClip linkClip = this.sequence[j];
				if (linkClip == null)
				{
					endPosition = vector;
					return;
				}
				animation[linkClip.clip.name].enabled = true;
				animation[linkClip.clip.name].weight = 1f;
				for (int k = 0; k < linkClip.loopCount; k++)
				{
					animation[linkClip.clip.name].normalizedTime = 0f;
					animation.Sample();
					global::UnityEngine.Vector3 vector2 = transform.position - base.transform.position;
					if (j > 0)
					{
						vector += a - vector2;
					}
					else
					{
						b = vector2;
					}
					for (int l = 0; l <= 20; l++)
					{
						float num = (float)l / 20f;
						animation[linkClip.clip.name].normalizedTime = num;
						animation.Sample();
						global::UnityEngine.Vector3 item = vector + (transform.position - base.transform.position) + linkClip.velocity * num * linkClip.clip.length;
						trace.Add(item);
					}
					vector += linkClip.velocity * 1f * linkClip.clip.length;
					animation[linkClip.clip.name].normalizedTime = 1f;
					animation.Sample();
					global::UnityEngine.Vector3 vector3 = transform.position - base.transform.position;
					a = vector3;
				}
				animation[linkClip.clip.name].enabled = false;
				animation[linkClip.clip.name].weight = 0f;
			}
			vector += a - b;
			global::UnityEngine.Object.DestroyImmediate(gameObject);
			endPosition = vector;
		}

		public override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = global::Pathfinding.Util.ListPool<global::UnityEngine.Vector3>.Claim();
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			this.CalculateOffsets(list, out zero);
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.blue;
			for (int i = 0; i < list.Count - 1; i++)
			{
				global::UnityEngine.Gizmos.DrawLine(list[i], list[i + 1]);
			}
		}

		public string clip;

		public float animSpeed = 1f;

		public bool reverseAnim = true;

		public global::UnityEngine.GameObject referenceMesh;

		public global::Pathfinding.AnimationLink.LinkClip[] sequence;

		public string boneRoot = "bn_COG_Root";

		[global::System.Serializable]
		public class LinkClip
		{
			public string name
			{
				get
				{
					return (!(this.clip != null)) ? string.Empty : this.clip.name;
				}
			}

			public global::UnityEngine.AnimationClip clip;

			public global::UnityEngine.Vector3 velocity;

			public int loopCount = 1;
		}
	}
}
