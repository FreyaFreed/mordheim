using System;

public class SystemPopupView : global::ConfirmationPopupView
{
	protected override void Awake()
	{
		base.Awake();
		this.isSystem = true;
	}

	public override void Show(global::System.Action<bool> callback, bool hideButtons = false, bool hideCancel = false)
	{
		base.Show(callback, hideButtons, hideCancel);
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetCurrentState() == global::PandoraInput.States.MISSION)
		{
			this.inMission = true;
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MENU, true);
		}
		global::PandoraSingleton<global::PandoraInput>.Instance.SetActive(true);
	}

	public override void Hide()
	{
		base.Hide();
		if (this.inMission)
		{
			this.inMission = false;
			global::PandoraSingleton<global::PandoraInput>.Instance.SetCurrentState(global::PandoraInput.States.MISSION, false);
		}
	}

	private bool inMission;
}
