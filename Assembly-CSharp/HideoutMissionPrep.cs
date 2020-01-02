using System;
using UnityEngine.Events;

public class HideoutMissionPrep : global::WarbandManagementBaseState
{
	public HideoutMissionPrep(global::HideoutManager mng, global::HideoutCamAnchor anchor) : base(mng, global::ModuleId.MISSION_PREP)
	{
	}

	public override void Enter(int iFrom)
	{
		base.Enter(iFrom);
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.MENU);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.ActivateCenterTabModule(true, new global::ModuleId[]
		{
			global::ModuleId.TITLE
		});
		global::TitleModule moduleLeft = global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleLeft<global::TitleModule>(global::ModuleId.TITLE);
		moduleLeft.Set("units_selection_title", true);
	}

	public override void Exit(int iTo)
	{
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.MENU);
	}

	protected override void SetupDefaultButtons()
	{
		base.SetupDefaultButtons();
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.SetAction("cancel", "menu_back", 6, false, global::PandoraSingleton<global::HideoutTabManager>.Instance.icnBack, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button1.OnAction(new global::UnityEngine.Events.UnityAction(this.ReturnToMission), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.SetAction("action", "Start Mission", 6, false, null, null);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button2.OnAction(new global::UnityEngine.Events.UnityAction(this.StartMission), false, true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button3.gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.button4.gameObject.SetActive(false);
	}

	private void ReturnToMission()
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.StateMachine.ChangeState(12);
	}

	private void StartMission()
	{
		global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MISSION_CAMPAIGN, false, false);
	}
}
