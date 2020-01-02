using System;
using UnityEngine;

namespace WellFired
{
	[global::WellFired.USequencerEventHideDuration]
	[global::WellFired.USequencerEvent("Application/Load Level")]
	[global::WellFired.USequencerFriendlyName("Load Level")]
	public class USLoadLevelEvent : global::WellFired.USEventBase
	{
		public override void FireEvent()
		{
			if (this.levelName.Length == 0 && this.levelIndex < 0)
			{
				global::UnityEngine.Debug.LogError("You have a Load Level event in your sequence, however, you didn't give it a level to load.");
				return;
			}
			if (this.levelIndex >= global::UnityEngine.Application.levelCount)
			{
				global::UnityEngine.Debug.LogError("You tried to load a level that is invalid, the level index is out of range.");
				return;
			}
			if (!global::UnityEngine.Application.isPlaying && !this.fireInEditor)
			{
				global::UnityEngine.Debug.Log("Load Level Fired, but it wasn't processed, since we are in the editor. Please set the fire In Editor flag in the inspector if you require this behaviour.");
				return;
			}
			if (this.levelName.Length != 0)
			{
				global::UnityEngine.Application.LoadLevel(this.levelName);
			}
			if (this.levelIndex != -1)
			{
				global::UnityEngine.Application.LoadLevel(this.levelIndex);
			}
		}

		public override void ProcessEvent(float deltaTime)
		{
		}

		public bool fireInEditor;

		public string levelName = string.Empty;

		public int levelIndex = -1;
	}
}
