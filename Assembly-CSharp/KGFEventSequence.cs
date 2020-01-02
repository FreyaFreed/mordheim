using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[global::System.Serializable]
public class KGFEventSequence : global::KGFEventBase, global::KGFIValidator
{
	protected override void KGFAwake()
	{
		global::KGFEventSequence.itsListOfSequencesAll.Add(this);
	}

	public void Step()
	{
		this.itsStayBeforeStepID++;
	}

	public void Finish()
	{
		this.itsStayBeforeStepID = this.itsEntries.Count + 1;
	}

	public static bool GetSingleStepMode()
	{
		return global::KGFEventSequence.itsStepMode;
	}

	public bool IsWaitingForDebugInput()
	{
		return this.itsStayBeforeStepID == this.itsEventDoneCounter;
	}

	public int GetCurrentStepNumber()
	{
		return this.itsEventDoneCounter.GetValueOrDefault(0);
	}

	public int GetStepCount()
	{
		return this.itsEntries.Count;
	}

	public static void SetSingleStepMode(bool theActivateStepMode)
	{
		global::KGFEventSequence.itsStepMode = theActivateStepMode;
	}

	public static global::KGFEventSequence[] GetAllSequences()
	{
		return global::KGFEventSequence.itsListOfSequencesAll.ToArray();
	}

	public static global::System.Collections.Generic.IEnumerable<global::KGFEventSequence> GetQueuedSequences()
	{
		for (int i = global::KGFEventSequence.itsListOfSequencesAll.Count - 1; i >= 0; i--)
		{
			global::KGFEventSequence aSequence = global::KGFEventSequence.itsListOfSequencesAll[i];
			if (aSequence == null)
			{
				global::KGFEventSequence.itsListOfSequencesAll.RemoveAt(i);
			}
			else if (aSequence.gameObject == null)
			{
				global::KGFEventSequence.itsListOfSequencesAll.RemoveAt(i);
			}
			else if (aSequence.IsQueued())
			{
				yield return aSequence;
			}
		}
		yield break;
	}

	public static int GetNumberOfRunningSequences()
	{
		return global::KGFEventSequence.itsListOfRunningSequences.Count;
	}

	public static global::KGFEventSequence[] GetRunningEventSequences()
	{
		return global::KGFEventSequence.itsListOfRunningSequences.ToArray();
	}

	public void InitList()
	{
		if (this.itsEntries.Count == 0)
		{
			this.itsEntries.Add(new global::KGFEventSequence.KGFEventSequenceEntry());
		}
	}

	public void Insert(global::KGFEventSequence.KGFEventSequenceEntry theElementAfterToInsert, global::KGFEventSequence.KGFEventSequenceEntry theElementToInsert)
	{
		int num = this.itsEntries.IndexOf(theElementAfterToInsert);
		this.itsEntries.Insert(num + 1, theElementToInsert);
	}

	public void Delete(global::KGFEventSequence.KGFEventSequenceEntry theElementToDelete)
	{
		if (this.itsEntries.Count > 1)
		{
			this.itsEntries.Remove(theElementToDelete);
		}
	}

	public void MoveUp(global::KGFEventSequence.KGFEventSequenceEntry theElementToMoveUp)
	{
		int num = this.itsEntries.IndexOf(theElementToMoveUp);
		if (num <= 0)
		{
			global::KGFEvent.LogWarning("cannot move up element at 0 index", "KGFEventSystem", this);
			return;
		}
		this.Delete(theElementToMoveUp);
		this.itsEntries.Insert(num - 1, theElementToMoveUp);
	}

	public void MoveDown(global::KGFEventSequence.KGFEventSequenceEntry theElementToMoveDown)
	{
		int num = this.itsEntries.IndexOf(theElementToMoveDown);
		if (num >= this.itsEntries.Count - 1)
		{
			global::KGFEvent.LogWarning("cannot move down element at end index", "KGFEventSystem", this);
			return;
		}
		this.Delete(theElementToMoveDown);
		this.itsEntries.Insert(num + 1, theElementToMoveDown);
	}

	public bool IsRunning()
	{
		return this.itsEventSequenceRunning;
	}

	public bool IsQueued()
	{
		int? num = this.itsEventDoneCounter;
		return num != null && !this.itsEventSequenceRunning;
	}

	public string GetNextExecutedJobItem()
	{
		int? num = this.itsEventDoneCounter;
		if (num == null)
		{
			return "not running";
		}
		if (this.itsEventDoneCounter.GetValueOrDefault() < this.itsEntries.Count)
		{
			return this.itsEntries[this.itsEventDoneCounter.GetValueOrDefault()].itsEvent.name;
		}
		return "finished";
	}

	[global::KGFEventExpose]
	public override void Trigger()
	{
		this.itsEventDoneCounter = new int?(0);
		if (base.gameObject.active)
		{
			this.itsEventSequenceRunning = true;
			global::KGFEvent.LogDebug("Start: " + base.gameObject.name, "KGFEventSystem", this);
			base.StartCoroutine("StartSequence");
		}
		else
		{
			global::KGFEvent.LogDebug("Queued: " + base.gameObject.name, "KGFEventSystem", this);
		}
	}

	[global::KGFEventExpose]
	public void StopSequence()
	{
		base.StopCoroutine("StartSequence");
		this.itsEventSequenceRunning = false;
		this.itsEventDoneCounter = null;
		if (global::KGFEventSequence.itsListOfRunningSequences.Contains(this))
		{
			global::KGFEventSequence.itsListOfRunningSequences.Remove(this);
		}
	}

	private global::System.Collections.IEnumerator StartSequence()
	{
		this.itsStayBeforeStepID = 0;
		if (!global::KGFEventSequence.itsListOfRunningSequences.Contains(this))
		{
			global::KGFEventSequence.itsListOfRunningSequences.Add(this);
		}
		int? num = this.itsEventDoneCounter;
		if (num == null)
		{
			yield break;
		}
		for (int i = this.itsEventDoneCounter.GetValueOrDefault(0); i < this.itsEntries.Count; i++)
		{
			global::KGFEventSequence.KGFEventSequenceEntry anEntry = this.itsEntries[i];
			if (anEntry.itsWaitBefore > 0f)
			{
				yield return new global::UnityEngine.WaitForSeconds(anEntry.itsWaitBefore);
			}
			try
			{
				if (anEntry.itsEvent != null)
				{
					anEntry.itsEvent.Trigger();
				}
				else
				{
					global::KGFEvent.LogError("events have null entries", "KGFEventSystem", this);
				}
			}
			catch (global::System.Exception ex)
			{
				global::System.Exception e = ex;
				global::KGFEvent.LogError("Exception in event_sequence:" + e, "KGFEventSystem", this);
			}
			this.itsEventDoneCounter = new int?(i + 1);
			if (anEntry.itsWaitAfter > 0f)
			{
				yield return new global::UnityEngine.WaitForSeconds(anEntry.itsWaitAfter);
			}
		}
		this.itsEventDoneCounter = null;
		this.itsEventSequenceRunning = false;
		if (global::KGFEventSequence.itsListOfRunningSequences.Contains(this))
		{
			global::KGFEventSequence.itsListOfRunningSequences.Remove(this);
		}
		yield break;
	}

	private void OnDestruct()
	{
		this.StopSequence();
	}

	public override global::KGFMessageList Validate()
	{
		global::KGFMessageList kgfmessageList = new global::KGFMessageList();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (this.itsEntries != null)
		{
			for (int i = 0; i < this.itsEntries.Count; i++)
			{
				global::KGFEventSequence.KGFEventSequenceEntry kgfeventSequenceEntry = this.itsEntries[i];
				if (kgfeventSequenceEntry.itsEvent == null)
				{
					flag = true;
				}
				if (kgfeventSequenceEntry.itsWaitBefore < 0f)
				{
					flag2 = true;
				}
				if (kgfeventSequenceEntry.itsWaitAfter < 0f)
				{
					flag3 = true;
				}
			}
		}
		if (flag)
		{
			kgfmessageList.AddError("sequence entry has null event");
		}
		if (flag2)
		{
			kgfmessageList.AddError("sequence entry itsWaitBefore <= 0");
		}
		if (flag3)
		{
			kgfmessageList.AddError("sequence entry itsWaitAfter <= 0");
		}
		return kgfmessageList;
	}

	private const string itsEventCategory = "KGFEventSystem";

	public global::System.Collections.Generic.List<global::KGFEventSequence.KGFEventSequenceEntry> itsEntries = new global::System.Collections.Generic.List<global::KGFEventSequence.KGFEventSequenceEntry>();

	private static global::System.Collections.Generic.List<global::KGFEventSequence> itsListOfRunningSequences = new global::System.Collections.Generic.List<global::KGFEventSequence>();

	private bool itsEventSequenceRunning;

	private static global::System.Collections.Generic.List<global::KGFEventSequence> itsListOfSequencesAll = new global::System.Collections.Generic.List<global::KGFEventSequence>();

	private static bool itsStepMode = false;

	private int itsStayBeforeStepID;

	private int? itsEventDoneCounter;

	[global::System.Serializable]
	public class KGFEventSequenceEntry
	{
		public float itsWaitBefore;

		public global::KGFEventBase itsEvent;

		public float itsWaitAfter;
	}
}
