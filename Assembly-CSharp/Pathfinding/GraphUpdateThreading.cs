using System;

namespace Pathfinding
{
	public enum GraphUpdateThreading
	{
		UnityThread,
		SeparateThread,
		UnityInit,
		UnityPost = 4,
		SeparateAndUnityInit = 3
	}
}
