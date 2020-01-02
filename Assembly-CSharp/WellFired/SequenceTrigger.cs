using System;
using UnityEngine;

namespace WellFired
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Collider))]
	public class SequenceTrigger : global::UnityEngine.MonoBehaviour
	{
		private void OnTriggerEnter(global::UnityEngine.Collider other)
		{
			if (!this.sequenceToPlay)
			{
				global::UnityEngine.Debug.LogWarning("You have triggered a sequence in your scene, however, you didn't assign it a Sequence To Play", base.gameObject);
				return;
			}
			if (this.sequenceToPlay.IsPlaying)
			{
				return;
			}
			if (other.CompareTag("MainCamera") && this.isMainCameraTrigger)
			{
				this.sequenceToPlay.Play();
				return;
			}
			if (other.CompareTag("Player") && this.isPlayerTrigger)
			{
				this.sequenceToPlay.Play();
				return;
			}
			if (other.gameObject == this.triggerObject)
			{
				this.sequenceToPlay.Play();
				return;
			}
		}

		public bool isPlayerTrigger;

		public bool isMainCameraTrigger;

		public global::UnityEngine.GameObject triggerObject;

		public global::WellFired.USSequencer sequenceToPlay;
	}
}
