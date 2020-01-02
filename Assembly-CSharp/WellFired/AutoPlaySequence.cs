using System;
using UnityEngine;

namespace WellFired
{
	public class AutoPlaySequence : global::UnityEngine.MonoBehaviour
	{
		private void Start()
		{
			if (!this.sequence)
			{
				global::UnityEngine.Debug.LogError("You have added an AutoPlaySequence, however you haven't assigned it a sequence", base.gameObject);
				return;
			}
		}

		private void Update()
		{
			if (this.hasPlayed)
			{
				return;
			}
			this.currentTime += global::UnityEngine.Time.deltaTime;
			if (this.currentTime >= this.delay && this.sequence)
			{
				this.sequence.Play();
				this.hasPlayed = true;
			}
		}

		public global::WellFired.USSequencer sequence;

		public float delay = 1f;

		private float currentTime;

		private bool hasPlayed;
	}
}
