using System;

public class QuitGamePopup : global::ConfirmationPopupView
{
	public override void Init()
	{
		base.Init();
	}

	private void Update()
	{
		if (this.isShow && global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("menu", 1))
		{
			this.Hide();
		}
	}
}
