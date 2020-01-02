using System;
using UnityEngine;

namespace Pathfinding.RVO
{
	[global::UnityEngine.HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_r_v_o_1_1_r_v_o_simulator.php")]
	[global::UnityEngine.AddComponentMenu("Pathfinding/Local Avoidance/RVO Simulator")]
	public class RVOSimulator : global::UnityEngine.MonoBehaviour
	{
		public global::Pathfinding.RVO.Simulator GetSimulator()
		{
			if (this.simulator == null)
			{
				this.Awake();
			}
			return this.simulator;
		}

		private void Awake()
		{
			if (this.simulator == null)
			{
				int workers = global::AstarPath.CalculateThreadCount(this.workerThreads);
				this.simulator = new global::Pathfinding.RVO.Simulator(workers, this.doubleBuffering);
			}
		}

		private void Update()
		{
			if (this.desiredSimulationFPS < 1)
			{
				this.desiredSimulationFPS = 1;
			}
			global::Pathfinding.RVO.Simulator simulator = this.GetSimulator();
			simulator.DesiredDeltaTime = 1f / (float)this.desiredSimulationFPS;
			simulator.symmetryBreakingBias = this.symmetryBreakingBias;
			simulator.Update();
		}

		private void OnDestroy()
		{
			if (this.simulator != null)
			{
				this.simulator.OnDestroy();
			}
		}

		[global::UnityEngine.Tooltip("Desired FPS for rvo simulation. It is usually not necessary to run a crowd simulation at a very high fps.\nUsually 10-30 fps is enough, but can be increased for better quality.\nThe rvo simulation will never run at a higher fps than the game")]
		public int desiredSimulationFPS = 20;

		[global::UnityEngine.Tooltip("Number of RVO worker threads. If set to None, no multithreading will be used.")]
		public global::Pathfinding.ThreadCount workerThreads = global::Pathfinding.ThreadCount.Two;

		[global::UnityEngine.Tooltip("Calculate local avoidance in between frames.\nThis can increase jitter in the agents' movement so use it only if you really need the performance boost. It will also reduce the responsiveness of the agents to the commands you send to them.")]
		public bool doubleBuffering;

		[global::UnityEngine.Range(0f, 0.2f)]
		[global::UnityEngine.Tooltip("Bias agents to pass each other on the right side.\nIf the desired velocity of an agent puts it on a collision course with another agent or an obstacle its desired velocity will be rotated this number of radians (1 radian is approximately 57°) to the right. This helps to break up symmetries and makes it possible to resolve some situations much faster.\n\nWhen many agents have the same goal this can however have the side effect that the group clustered around the target point may as a whole start to spin around the target point.")]
		public float symmetryBreakingBias = 0.1f;

		private global::Pathfinding.RVO.Simulator simulator;
	}
}
