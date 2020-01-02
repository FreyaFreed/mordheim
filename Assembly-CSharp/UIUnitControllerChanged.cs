using System;

public abstract class UIUnitControllerChanged : global::CanvasGroupDisabler
{
	public global::UnitController CurrentUnitController { get; set; }

	public global::UnitController TargetUnitController { get; set; }

	public global::Destructible TargetDestructible { get; set; }

	public bool UpdateUnit { get; set; }

	public virtual void UnitChanged(global::UnitController unitController, global::UnitController targetUnitController, global::Destructible targetDestructible = null)
	{
		this.CurrentUnitController = unitController;
		this.TargetUnitController = targetUnitController;
		this.TargetDestructible = targetDestructible;
		this.UpdateUnit = true;
	}

	protected abstract void OnUnitChanged();

	protected virtual void LateUpdate()
	{
		if (this.UpdateUnit)
		{
			this.UpdateUnit = false;
			this.OnUnitChanged();
		}
	}
}
