using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapImprint : global::UnityEngine.MonoBehaviour
{
	public global::MapImprintStateId State { get; private set; }

	public global::System.Collections.Generic.List<global::UnitController> Viewers { get; private set; }

	public global::UnitController UnitCtrlr { get; private set; }

	public global::SearchPoint Search { get; private set; }

	public global::Trap Trap { get; private set; }

	public global::Destructible Destructible { get; private set; }

	public global::WarbandWagon Wagon { get; set; }

	public global::MapBeacon Beacon { get; set; }

	public int Flag { get; private set; }

	public void Init(global::UnityEngine.Sprite visibleIcon, global::UnityEngine.Sprite lostIcon, bool alwaysVisible, global::MapImprintType imprintType, global::UnityEngine.Events.UnityAction<bool, bool, global::UnityEngine.Events.UnityAction> hideDelegate = null, global::UnitController unit = null, global::SearchPoint searchPoint = null, global::Trap trap = null, global::Destructible destructible = null)
	{
		this.Viewers = new global::System.Collections.Generic.List<global::UnitController>();
		this.imprintType = imprintType;
		this.visibleTexture = visibleIcon;
		this.lostTexture = lostIcon;
		this.alwaysVisible = alwaysVisible;
		this.alive = true;
		this.State = ((!alwaysVisible) ? global::MapImprintStateId.INVISIBLE : global::MapImprintStateId.VISIBLE);
		this.hideDel = hideDelegate;
		this.UnitCtrlr = unit;
		this.Search = searchPoint;
		this.Trap = trap;
		this.Destructible = destructible;
		this.SetCheckFlag();
		global::PandoraSingleton<global::MissionManager>.Instance.MapImprints.Add(this);
	}

	public void Init(string visibleIcon, string lostIcon, bool alwaysVisible, global::MapImprintType imprintType, global::UnityEngine.Events.UnityAction<bool, bool, global::UnityEngine.Events.UnityAction> hideDelegate = null, global::UnitController unit = null, global::SearchPoint searchPoint = null, global::Trap trap = null, global::Destructible destructible = null)
	{
		this.Viewers = new global::System.Collections.Generic.List<global::UnitController>();
		this.imprintType = imprintType;
		if (visibleIcon != null)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.Sprite>(visibleIcon, delegate(global::UnityEngine.Object o)
			{
				this.visibleTexture = (global::UnityEngine.Sprite)o;
			}, false);
		}
		if (lostIcon != null)
		{
			global::PandoraSingleton<global::AssetBundleLoader>.Instance.LoadResourceAsync<global::UnityEngine.Sprite>(lostIcon, delegate(global::UnityEngine.Object o)
			{
				this.lostTexture = (global::UnityEngine.Sprite)o;
			}, false);
		}
		this.alwaysVisible = alwaysVisible;
		this.alive = true;
		this.State = ((!alwaysVisible) ? global::MapImprintStateId.INVISIBLE : global::MapImprintStateId.VISIBLE);
		this.hideDel = hideDelegate;
		this.UnitCtrlr = unit;
		this.Search = searchPoint;
		this.Trap = trap;
		this.Destructible = destructible;
		this.SetCheckFlag();
		global::PandoraSingleton<global::MissionManager>.Instance.MapImprints.Add(this);
	}

	private void SetCheckFlag()
	{
		this.Flag = (int)((float)global::MapImprint.currentCount / 15f);
		global::MapImprint.maxFlag = global::UnityEngine.Mathf.Max(this.Flag, global::MapImprint.maxFlag);
		global::MapImprint.currentCount++;
	}

	private void SetState(bool visible, bool alive)
	{
		visible |= (this.alwaysVisible || (this.UnitCtrlr != null && this.UnitCtrlr.unit.UnitAlwaysVisible));
		visible &= !this.alwaysHide;
		switch (this.State)
		{
		case global::MapImprintStateId.VISIBLE:
			this.State = (visible ? ((!alive) ? global::MapImprintStateId.DESTROYED : global::MapImprintStateId.VISIBLE) : global::MapImprintStateId.LOST);
			break;
		case global::MapImprintStateId.INVISIBLE:
			this.State = (visible ? ((!alive) ? global::MapImprintStateId.DESTROYED : global::MapImprintStateId.VISIBLE) : global::MapImprintStateId.INVISIBLE);
			break;
		case global::MapImprintStateId.LOST:
			this.State = (visible ? ((!alive) ? global::MapImprintStateId.DESTROYED : global::MapImprintStateId.VISIBLE) : global::MapImprintStateId.LOST);
			break;
		case global::MapImprintStateId.DESTROYED:
			this.alwaysVisible = true;
			break;
		}
		if (this.hideDel != null)
		{
			this.hideDel(!visible, false, null);
		}
	}

	public void RefreshPosition()
	{
		if (this.needsRefresh)
		{
			this.needsRefresh = false;
			this.SetState(this.Viewers.Count > 0, this.alive);
		}
		if (this.State == global::MapImprintStateId.VISIBLE)
		{
			this.lastKnownPos = base.transform.position;
		}
	}

	public void RemoveViewer(global::UnitController ctrlr)
	{
		int num = this.Viewers.IndexOf(ctrlr);
		if (num != -1)
		{
			this.Viewers.RemoveAt(num);
			this.needsRefresh = true;
			if (this.UnitCtrlr != null)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.resendLadder = true;
			}
		}
	}

	public void Hide()
	{
		this.Viewers.Clear();
		this.State = global::MapImprintStateId.INVISIBLE;
		this.needsRefresh = true;
	}

	public void AddViewer(global::UnitController ctrlr)
	{
		if (ctrlr == this.UnitCtrlr)
		{
			global::PandoraDebug.LogWarning("Adding unit as its own viewer", "uncategorised", null);
			return;
		}
		int num = this.Viewers.IndexOf(ctrlr);
		if (num == -1)
		{
			this.Viewers.Add(ctrlr);
			this.needsRefresh = true;
			if (this.UnitCtrlr != null)
			{
				global::PandoraSingleton<global::MissionManager>.Instance.resendLadder = true;
			}
		}
	}

	public void SetCurrent(bool current)
	{
		this.needsRefresh = true;
	}

	private const int SIMULTANEOUS_CHECK = 15;

	public static int currentCount;

	public static int currentFlagChecked;

	public static int maxFlag;

	public bool alwaysVisible;

	public bool alwaysHide;

	public global::MapImprintType imprintType;

	public bool alive;

	public global::UnityEngine.Vector3 lastKnownPos;

	public global::UnityEngine.Sprite visibleTexture;

	public global::UnityEngine.Sprite lostTexture;

	public global::UnityEngine.Sprite idolTexture;

	public bool needsRefresh;

	private global::UnityEngine.Events.UnityAction<bool, bool, global::UnityEngine.Events.UnityAction> hideDel;
}
