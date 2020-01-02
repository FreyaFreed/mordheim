using System;

public class ToMainMenuOptionsMenuState : global::ICheapState
{
	public ToMainMenuOptionsMenuState(global::OptionsMenuState ctrlr)
	{
		this.controller = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_ACTION, new global::DelReceiveNotice(this.InputAction));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_CANCEL, new global::DelReceiveNotice(this.InputCancel));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.MENU_QUIT);
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_ACTION, new global::DelReceiveNotice(this.InputAction));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_CANCEL, new global::DelReceiveNotice(this.InputCancel));
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
		{
			this.InputAction();
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0))
		{
			this.InputCancel();
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void InputAction()
	{
		global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.OPTIONS_QUIT_GAME, false, false);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MENU_QUIT_CONF, true);
	}

	private void InputCancel()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MENU_QUIT_CONF, false);
		this.controller.GoTo(global::OptionsMenuState.State.BROWSE);
	}

	private global::OptionsMenuState controller;
}
