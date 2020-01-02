using System;
using System.Collections.Generic;

public class BrowseOptionsMenuState : global::ICheapState
{
	public BrowseOptionsMenuState(global::OptionsMenuState ctrlr)
	{
		this.controller = ctrlr;
		this.options = new global::System.Collections.Generic.List<global::OptionsMenuState.State>();
		this.options.Add(global::OptionsMenuState.State.GRAPHICS);
		this.options.Add(global::OptionsMenuState.State.AUDIO);
		this.options.Add(global::OptionsMenuState.State.GAMEPLAY);
		this.options.Add(global::OptionsMenuState.State.CONTROL);
		if (this.controller.canQuit)
		{
			this.options.Add(global::OptionsMenuState.State.TO_MAIN_MENU);
			this.options.Add(global::OptionsMenuState.State.QUIT);
		}
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_ACTION, new global::DelReceiveNotice(this.InputAction));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_CANCEL, new global::DelReceiveNotice(this.InputCancel));
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_OPTIONS_CAT, new global::DelReceiveNotice(this.InputCat));
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool, bool>(global::Notices.MENU_OPTIONS, !this.controller.canQuit, true);
		this.UpdateSelection(0);
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_ACTION, new global::DelReceiveNotice(this.InputAction));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_CANCEL, new global::DelReceiveNotice(this.InputCancel));
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_OPTIONS_CAT, new global::DelReceiveNotice(this.InputCat));
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
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("v", 0))
		{
			this.UpdateSelection(-1);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown("v", 0))
		{
			this.UpdateSelection(1);
		}
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void InputAction()
	{
		this.controller.GoTo((global::OptionsMenuState.State)this.selection);
	}

	private void InputCancel()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool, bool>(global::Notices.MENU_OPTIONS, !this.controller.canQuit, false);
		this.controller.ExitState();
	}

	private void InputCat()
	{
		int num = (int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
		if (num != this.selection)
		{
			this.UpdateSelection(num - this.selection);
		}
	}

	private void UpdateSelection(int direction)
	{
		this.selection += direction;
		if (this.selection >= this.options.Count)
		{
			this.selection = 0;
		}
		else if (this.selection < 0)
		{
			this.selection = this.options.Count - 1;
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<int>(global::Notices.MENU_OPTIONS_SELECTED, this.selection);
	}

	private global::OptionsMenuState controller;

	private global::System.Collections.Generic.List<global::OptionsMenuState.State> options;

	private int selection;
}
