using System;
using UnityEngine;

public class OptionsMainMenuState : global::UIStateMonoBehaviour<global::MainMenuController>
{
	public override void Awake()
	{
		base.Awake();
		this.optionsMan = this.canvasGroup.GetComponentInChildren<global::OptionsManager>();
		this.optionsMan.onCloseOptionsMenu = new global::System.Action(this.OnCloseOptions);
		this.optionsMan.SetBackButtonLoc("menu_back_main_menu");
		this.optionsMan.HideQuitOption();
	}

	public override void StateEnter()
	{
		base.Show(true);
		base.StateMachine.camManager.dummyCam.transform.position = this.camPos.transform.position;
		base.StateMachine.camManager.dummyCam.transform.rotation = this.camPos.transform.rotation;
		base.StateMachine.camManager.Transition(2f, true);
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.MENU);
		this.optionsMan.OnShow();
	}

	public void OnCloseOptions()
	{
		this.optionsMan.OnHide();
		base.StateMachine.ChangeState(global::MainMenuController.State.MAIN_MENU);
	}

	public override void StateExit()
	{
		base.Show(false);
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.MENU);
	}

	public override int StateId
	{
		get
		{
			return 3;
		}
	}

	public global::ButtonGroup butExit;

	private global::OptionsManager optionsMan;

	public global::UnityEngine.GameObject camPos;
}
