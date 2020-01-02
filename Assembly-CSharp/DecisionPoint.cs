using System;
using UnityEngine;

public class DecisionPoint : global::UnityEngine.MonoBehaviour
{
	private void OnDrawGizmos()
	{
		string str = "tga";
		switch (this.id)
		{
		case global::DecisionPointId.OVERWATCH:
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.red;
			break;
		case global::DecisionPointId.AMBUSH:
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.magenta;
			break;
		case global::DecisionPointId.PATROL:
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.cyan;
			break;
		case global::DecisionPointId.SPAWN:
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.green;
			str = "png";
			break;
		case global::DecisionPointId.FLY:
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.yellow;
			str = "png";
			break;
		}
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + new global::UnityEngine.Vector3(0f, 1f, 0f), this.id.ToString().ToLower() + "_point." + str, true);
		global::PandoraUtils.DrawFacingGizmoCube(base.transform, 1f, 0.5f, 0.5f, 0f, 0f, true);
	}

	public void Register()
	{
		if (global::UnityEngine.Physics.Raycast(base.transform.position + base.transform.up * 2f, base.transform.up * -1f, 1.8f, global::LayerMaskManager.decisionMask) || global::UnityEngine.Physics.Raycast(base.transform.position + base.transform.up + base.transform.right / 3f, base.transform.right * -1f, 1f, global::LayerMaskManager.decisionMask) || global::UnityEngine.Physics.Raycast(base.transform.position + base.transform.up - base.transform.right / 3f, base.transform.right, 1f, global::LayerMaskManager.decisionMask) || global::UnityEngine.Physics.Raycast(base.transform.position + base.transform.up + base.transform.forward / 3f, base.transform.forward * -1f, 1f, global::LayerMaskManager.decisionMask) || global::UnityEngine.Physics.Raycast(base.transform.position + base.transform.up - base.transform.forward / 3f, base.transform.forward, 1f, global::LayerMaskManager.decisionMask) || !global::UnityEngine.Physics.Raycast(base.transform.position + base.transform.up, base.transform.up * -1f, 1.5f, global::LayerMaskManager.decisionMask))
		{
			global::UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
		else
		{
			global::PandoraSingleton<global::MissionManager>.Instance.RegisterDecisionPoint(this);
		}
	}

	public global::DecisionPointId id;

	[global::UnityEngine.HideInInspector]
	public float closestUnitSqrDist;
}
