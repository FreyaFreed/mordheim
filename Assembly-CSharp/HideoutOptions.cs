using System;

public class HideoutOptions : global::ICheapState
{
	public HideoutOptions(global::HideoutManager manager)
	{
		this.mngr = manager;
		this.optionsMan = this.mngr.optionsPanel.GetComponentInChildren<global::OptionsManager>();
		this.optionsMan.onCloseOptionsMenu = new global::System.Action(this.OnCloseOptions);
		this.optionsMan.onQuitGame = new global::System.Action<bool>(this.OnBackToMenu);
		this.optionsMan.SetBackButtonLoc("go_to_camp");
		this.optionsMan.SetQuitButtonLoc("menu_back_main_menu", string.Empty);
		this.optionsMan.HideAltQuitOption();
		this.optionsMan.HideSaveAndQuitOption();
		this.mngr.optionsPanel.SetActive(false);
	}

	void global::ICheapState.Destroy()
	{
	}

	void global::ICheapState.Enter(int iFrom)
	{
		global::PandoraSingleton<global::PandoraInput>.Instance.PushInputLayer(global::PandoraInput.InputLayer.MENU);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE).gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).gameObject.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.DeactivateAllButtons();
		this.mngr.optionsPanel.SetActive(true);
		this.optionsMan.OnShow();
	}

	void global::ICheapState.Exit(int iTo)
	{
		this.optionsMan.OnHide();
		this.mngr.optionsPanel.SetActive(false);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleCenter<global::TitleModule>(global::ModuleId.TITLE).gameObject.SetActive(true);
		global::PandoraSingleton<global::HideoutTabManager>.Instance.GetModuleRight<global::TreasuryModule>(global::ModuleId.TREASURY).gameObject.SetActive(true);
		global::PandoraSingleton<global::PandoraInput>.Instance.PopInputLayer(global::PandoraInput.InputLayer.MENU);
	}

	void global::ICheapState.Update()
	{
	}

	void global::ICheapState.FixedUpdate()
	{
	}

	private void OnCloseOptions()
	{
		this.mngr.StateMachine.ChangeState(0);
	}

	private void OnBackToMenu(bool isAlt)
	{
		global::PandoraSingleton<global::HideoutManager>.Instance.messagePopup.Show("com_btn_quit_to_main", "com_quit_to_main_menu", new global::System.Action<bool>(this.OnBackPopup), false, false);
	}

	private void OnBackPopup(bool ok)
	{
		if (ok)
		{
			this.mngr.SaveChanges();
			global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.OPTIONS_QUIT_GAME, false, false);
		}
	}

	private global::HideoutManager mngr;

	private global::OptionsManager optionsMan;
}
