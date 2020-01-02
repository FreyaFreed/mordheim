using System;
using UnityEngine;

public abstract class TransitionBase : global::UnityEngine.MonoBehaviour
{
	public abstract void Show(bool visible, float duration);

	public abstract void ProcessTransition(float progress);

	public abstract void EndTransition();

	public abstract void Reset();
}
