using System;

public class GraphicOptionsMenuState : global::ICheapState
{
	public GraphicOptionsMenuState(global::OptionsMenuState ctrlr)
	{
		this.controller = ctrlr;
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener(global::Notices.INPUT_CANCEL, new global::DelReceiveNotice(this.InputCancel));
		this.selectionX = 0;
		this.selectionY = 0;
		this.UpdateSelection(0, 0);
	}

	void global::ICheapState.Exit(int iTo)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.RemoveListener(global::Notices.INPUT_CANCEL, new global::DelReceiveNotice(this.InputCancel));
	}

	void global::ICheapState.Update()
	{
		if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("v", 0))
		{
			this.UpdateSelection(0, -1);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown("v", 0))
		{
			this.UpdateSelection(0, 1);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyDown("h", 0))
		{
			this.UpdateSelection(-1, 0);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetNegKeyDown("h", 0))
		{
			this.UpdateSelection(1, 0);
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("action", 0))
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
		global::MenuConfig.OptionsGraphics optionsGraphics = global::MenuConfig.menuOptionsGfx[this.selectionX][this.selectionY];
		if (optionsGraphics != global::MenuConfig.OptionsGraphics.REVERT)
		{
		}
	}

	private void InputCancel()
	{
		this.controller.GoTo(global::OptionsMenuState.State.BROWSE);
	}

	private void UpdateSelection(int x, int y)
	{
		this.selectionX += x;
		if (this.selectionX >= global::MenuConfig.menuOptionsGfx.Length)
		{
			this.selectionX = 0;
		}
		else if (this.selectionX < 0)
		{
			this.selectionX = global::MenuConfig.menuOptionsGfx.Length - 1;
		}
		this.selectionY += y;
		if (this.selectionY >= global::MenuConfig.menuOptionsGfx[this.selectionX].Length)
		{
			this.selectionY = 0;
		}
		else if (this.selectionY < 0)
		{
			this.selectionY = global::MenuConfig.menuOptionsGfx[this.selectionX].Length - 1;
		}
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<global::MenuConfig.OptionsGraphics>(global::Notices.MENU_GRAPHICS_SELECTED, global::MenuConfig.menuOptionsGfx[this.selectionX][this.selectionY]);
	}

	private global::OptionsMenuState controller;

	private int selectionX;

	private int selectionY;
}
