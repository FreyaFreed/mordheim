using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRandomizerLibrary : global::UnityEngine.MonoBehaviour
{
	private void Start()
	{
		base.StartCoroutine(this.Randomize());
	}

	public global::System.Collections.IEnumerator Randomize()
	{
		for (;;)
		{
			yield return new global::UnityEngine.WaitForSeconds((float)global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 10));
			for (int i = 0; i < this.animators.Count; i++)
			{
				int rand = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(0, 10);
				if (rand == 1)
				{
					this.animators[i].SetTrigger("random_1");
				}
				else if (rand == 2)
				{
					this.animators[i].SetTrigger("random_2");
				}
			}
		}
		yield break;
	}

	public global::System.Collections.Generic.List<global::UnityEngine.Animator> animators;
}
