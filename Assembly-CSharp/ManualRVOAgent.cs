using System;
using Pathfinding.RVO;
using UnityEngine;

[global::UnityEngine.RequireComponent(typeof(global::Pathfinding.RVO.RVOController))]
[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_manual_r_v_o_agent.php")]
public class ManualRVOAgent : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		this.rvo = base.GetComponent<global::Pathfinding.RVO.RVOController>();
	}

	private void Update()
	{
		float axis = global::UnityEngine.Input.GetAxis("Horizontal");
		float axis2 = global::UnityEngine.Input.GetAxis("Vertical");
		global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(axis, 0f, axis2) * this.speed;
		this.rvo.ForceSetVelocity(vector);
		base.transform.position += vector * global::UnityEngine.Time.deltaTime;
	}

	private global::Pathfinding.RVO.RVOController rvo;

	public float speed = 1f;
}
