using System;
using UnityEngine;

public class MapBeacon : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.imprint = base.GetComponent<global::MapImprint>();
		if (this.imprint != null)
		{
			this.imprint.Init("icn_mission_type_mission", null, true, global::MapImprintType.BEACON, null, null, null, null, null);
			this.imprint.Beacon = this;
		}
	}

	private void OnEnable()
	{
		this.Refresh();
	}

	public void Refresh()
	{
		if (this.imprint == null)
		{
			return;
		}
		this.imprint.RefreshPosition();
		if (this.flyingOverview != null)
		{
			this.flyingOverview.Set(this.imprint, true, false);
			this.flyingOverview.bgBeacon.color = this.imprintColor;
			this.flyingOverview.icon.color = this.imprintColor;
			this.flyingOverview.gameObject.SetActive(true);
			this.flyingOverview.transform.SetAsLastSibling();
		}
	}

	private void OnDisable()
	{
		if (this.flyingOverview != null)
		{
			this.flyingOverview.gameObject.SetActive(false);
		}
	}

	public global::MapImprint imprint;

	public global::FlyingOverview flyingOverview;

	public global::UnityEngine.Color imprintColor;
}
