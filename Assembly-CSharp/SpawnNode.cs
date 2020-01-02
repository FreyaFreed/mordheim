using System;
using UnityEngine;

public class SpawnNode : global::UnityEngine.MonoBehaviour
{
	public void ShowImprint(bool isMine)
	{
		if (this.imprint == null)
		{
			this.imprint = base.gameObject.AddComponent<global::MapImprint>();
		}
		this.imprint.Init("overview/deploy", "overview/deploy", true, (!isMine) ? global::MapImprintType.ENEMY_DEPLOYMENT : global::MapImprintType.PLAYER_DEPLOYMENT, null, null, null, null, null);
	}

	public bool IsOfType(global::SpawnNodeId id)
	{
		return (this.types & 1 << id - global::SpawnNodeId.START) != 0;
	}

	private void OnDrawGizmos()
	{
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.white;
		global::UnityEngine.Gizmos.DrawIcon(base.transform.position + new global::UnityEngine.Vector3(0f, 2f, 0f), "start.png", true);
		float height = (!this.IsOfType(global::SpawnNodeId.IMPRESSIVE)) ? 1.6f : 2.18f;
		float num = (!this.IsOfType(global::SpawnNodeId.IMPRESSIVE)) ? 0.5f : 0.9f;
		global::PandoraUtils.DrawFacingGizmoCube(base.transform, height, num, num, 0f, 0f, true);
		global::UnityEngine.Gizmos.color = global::UnityEngine.Color.red;
		global::UnityEngine.Gizmos.DrawWireSphere(base.transform.position, (!this.IsOfType(global::SpawnNodeId.IMPRESSIVE)) ? 2.15f : 2.7f);
	}

	public int types;

	[global::UnityEngine.HideInInspector]
	public bool claimed;

	public global::MapImprint imprint;

	public global::SpawnNodeId overrideStyle = global::SpawnNodeId.START;
}
