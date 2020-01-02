using System;
using UnityEngine;
using UnityEngine.Events;

public class TurnTimer
{
	public TurnTimer(float timer, global::UnityEngine.Events.UnityAction onDone)
	{
		this.turnDuration = timer;
		this.timerDone = onDone;
		this.Paused = true;
		global::PandoraSingleton<global::Pan>.Instance.GetSound("turn_begin", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.startTurnSound = clip;
		});
		global::PandoraSingleton<global::Pan>.Instance.GetSound("turn_end", true, delegate(global::UnityEngine.AudioClip clip)
		{
			this.tenSecondSound = clip;
		});
	}

	public float Timer { get; private set; }

	public bool Paused { get; private set; }

	public void Pause()
	{
		this.Paused = true;
	}

	public void Resume()
	{
		this.Paused = false;
		if (!this.startTurnSoundPlayed)
		{
			this.startTurnSoundPlayed = true;
			if (this.startTurnSound != null)
			{
				global::PandoraSingleton<global::UIMissionManager>.Instance.audioSource.PlayOneShot(this.startTurnSound);
			}
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::TurnTimer>(global::Notices.TIMER_STARTING, this);
	}

	public void Reset(float time = -1f)
	{
		this.Timer = ((!global::UnityEngine.Mathf.Approximately(time, -1f)) ? time : this.turnDuration);
		this.startTurnSoundPlayed = false;
		this.tenSecondSoundPlayed = false;
	}

	public void Update()
	{
		if (!this.Paused && this.Timer > 0f)
		{
			this.Timer -= global::UnityEngine.Time.deltaTime;
			if (!this.tenSecondSoundPlayed && this.Timer <= 10f)
			{
				this.tenSecondSoundPlayed = true;
				if (this.tenSecondSound != null)
				{
					global::PandoraSingleton<global::MissionManager>.Instance.GetCurrentUnit().audioSource.PlayOneShot(this.tenSecondSound);
				}
			}
			if (this.Timer <= 0f)
			{
				this.Timer = 0f;
				this.Pause();
				this.timerDone();
			}
		}
	}

	private const float RESET_EMPTY_TIMER = -1f;

	private float turnDuration;

	private global::UnityEngine.Events.UnityAction timerDone;

	private bool tenSecondSoundPlayed;

	private bool startTurnSoundPlayed;

	private global::UnityEngine.AudioClip tenSecondSound;

	private global::UnityEngine.AudioClip startTurnSound;
}
