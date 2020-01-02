using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerFriendlyName("Spawn Prefab")]
	[global::WellFired.USequencerEvent("Spawn/Spawn Prefab")]
	[global::WellFired.USequencerEventHideDuration]
	public class USSpawnPrefabEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (!this.spawnPrefab)
			{
				global::UnityEngine.Debug.Log("Attempting to spawn a prefab, but you haven't given a prefab to the event from USSpawnPrefabEvent::FireEvent");
				return;
			}
			if (this.spawnTransform)
			{
				global::UnityEngine.Object.Instantiate(this.spawnPrefab, this.spawnTransform.position, this.spawnTransform.rotation);
			}
			else
			{
				global::UnityEngine.Object.Instantiate(this.spawnPrefab, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public global::UnityEngine.GameObject spawnPrefab;

		public global::UnityEngine.Transform spawnTransform;
	}
}
