using System;
using UnityEngine;

public class SkirmishController : global::UnityEngine.MonoBehaviour
{
	private void Update()
	{
		if (!this.quitMenu)
		{
			if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("start", 0))
			{
				global::PandoraDebug.LogDebug("Main Menu Start Game Pressed", "FLOW", this);
				global::SceneLauncher.Instance.LaunchScene(global::SceneLoadingId.LAUNCH_MULTI, false, false);
			}
			else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("quit", 0))
			{
				global::PandoraDebug.LogDebug("Main Menu Quit Game Pressed", "FLOW", this);
				global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(global::Notices.MENU_QUIT_GAME);
				this.quitMenu = true;
			}
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("quit", 0) || global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("cancel", 0))
		{
			global::PandoraDebug.LogDebug("Main Menu Quit Game CANCEL", "FLOW", this);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MENU_QUIT_ACTION, false);
			this.quitMenu = false;
		}
		else if (global::PandoraSingleton<global::PandoraInput>.Instance.GetKeyUp("start", 0))
		{
			global::PandoraDebug.LogDebug("Main Menu Quit Game ACCEPT", "FLOW", this);
			global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<bool>(global::Notices.MENU_QUIT_ACTION, true);
		}
	}

	private bool quitMenu;
}
