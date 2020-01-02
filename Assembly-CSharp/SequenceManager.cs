using System;
using System.Collections.Generic;
using UnityEngine;
using WellFired;

public class SequenceManager : global::PandoraSingleton<global::SequenceManager>
{
	private void Start()
	{
		this.seqs = new global::System.Collections.Generic.Dictionary<string, global::WellFired.USSequencer>();
		this.isPlaying = false;
		this.destroyAfterPlay = true;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.SEQUENCE_ENDED, new global::DelReceiveNotice(this.EndSequence));
	}

	public void PlaySequence(global::WellFired.USSequencer seq)
	{
		this.seqs[seq.name] = seq;
		this.PlaySequence(seq.name, null);
	}

	public void PlaySequence(string name, global::DelSequenceDone onFinishDel = null)
	{
		this.isPlaying = true;
		if (this.currentSeq != null && this.currentSeq.IsPlaying)
		{
			this.EndSequence();
		}
		this.finishDel = onFinishDel;
		if (!this.seqs.ContainsKey(name))
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.GameObject>("prefabs/sequences/" + name + "/" + name, delegate(global::UnityEngine.Object prefab)
			{
				global::UnityEngine.GameObject gameObject = (global::UnityEngine.GameObject)global::UnityEngine.Object.Instantiate(prefab);
				global::WellFired.USSequencer component = gameObject.GetComponent<global::WellFired.USSequencer>();
				this.seqs[name] = component;
				this.LaunchSequence(name);
			}, false);
		}
		else
		{
			this.LaunchSequence(name);
		}
	}

	private void LaunchSequence(string name)
	{
		this.currentSeq = this.seqs[name];
		this.currentSeq.Stop();
		this.currentSeq.Play();
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.SEQUENCE_STARTED);
	}

	public void EndSequence()
	{
		if (this.currentSeq != null)
		{
			this.currentSeq.SkipTimelineTo(this.currentSeq.Duration);
			if (this.destroyAfterPlay)
			{
				global::UnityEngine.Object.Destroy(this.currentSeq.gameObject);
				this.seqs.Clear();
			}
			this.currentSeq = null;
			this.isPlaying = false;
			if (this.finishDel != null)
			{
				global::DelSequenceDone delSequenceDone = this.finishDel;
				this.finishDel = null;
				delSequenceDone();
			}
		}
	}

	private global::System.Collections.Generic.Dictionary<string, global::WellFired.USSequencer> seqs;

	[global::UnityEngine.HideInInspector]
	public global::WellFired.USSequencer currentSeq;

	private bool relative;

	private global::DelSequenceDone finishDel;

	[global::UnityEngine.HideInInspector]
	public bool isPlaying;

	[global::UnityEngine.HideInInspector]
	public bool destroyAfterPlay;
}
