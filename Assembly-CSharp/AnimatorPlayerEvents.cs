using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPlayerEvents : global::UnityEngine.MonoBehaviour
{
	private global::UnitMenuController unitCtrlr
	{
		get
		{
			if (this.unitMenuCtrlr == null)
			{
				this.unitMenuCtrlr = base.GetComponent<global::UnitMenuController>();
			}
			return this.unitMenuCtrlr;
		}
	}

	public void EventSheathe()
	{
		if (global::PandoraSingleton<global::AnimatorPlayer>.Instance.sheated)
		{
			global::PandoraSingleton<global::AnimatorPlayer>.Instance.weaponStyle = this.weaponStyle;
			global::PandoraSingleton<global::AnimatorPlayer>.Instance.EquipWeapon();
		}
		else
		{
			this.weaponStyle = global::PandoraSingleton<global::AnimatorPlayer>.Instance.weaponStyle;
			global::PandoraSingleton<global::AnimatorPlayer>.Instance.weaponStyle = global::AnimStyleId.NONE;
			global::PandoraSingleton<global::AnimatorPlayer>.Instance.EquipWeapon();
			global::PandoraSingleton<global::AnimatorPlayer>.Instance.sheated = true;
		}
	}

	public void EventHurt(int variation)
	{
	}

	public void EventAvoid(int variation)
	{
	}

	public void EventParry()
	{
	}

	public void EventFx(string fxName)
	{
	}

	public void EventSound(string soundName)
	{
	}

	public void EventSoundFoot(string soundName)
	{
	}

	public void EventDisplayDamage()
	{
	}

	public void EventDisplayActionOutcome()
	{
	}

	public void EventDisplayStatusOutcome()
	{
	}

	public void EventTrail(int active)
	{
	}

	public void EventShoot(int idx)
	{
	}

	public void EventSpellStart()
	{
	}

	public void EventSpellShoot()
	{
	}

	public void EventAttachProj(int variation)
	{
	}

	public void EventReloadWeapons(int slot)
	{
	}

	public void EventWeaponAim()
	{
	}

	public void EventTp()
	{
	}

	public void EventShowStaff(int state)
	{
	}

	public void EventShout(string soundName)
	{
		this.GetParamSound(ref soundName);
		this.GetRandomSound(ref soundName, ref this.lastShout);
		this.PlaySound(ref soundName);
	}

	private void GetParamSound(ref string soundName)
	{
		while (soundName.IndexOf("(") != -1)
		{
			int num = soundName.IndexOf("(");
			int num2 = soundName.IndexOf(")");
			string text = soundName.Substring(num + 1, num2 - num - 1);
			string text2 = string.Empty;
			string text3 = text;
			if (text3 == null)
			{
				goto IL_FC;
			}
			if (global::AnimatorPlayerEvents.<>f__switch$map11 == null)
			{
				global::AnimatorPlayerEvents.<>f__switch$map11 = new global::System.Collections.Generic.Dictionary<string, int>(2)
				{
					{
						"unit",
						0
					},
					{
						"atk_spell",
						1
					}
				};
			}
			int num3;
			if (!global::AnimatorPlayerEvents.<>f__switch$map11.TryGetValue(text3, out num3))
			{
				goto IL_FC;
			}
			if (num3 != 0)
			{
				if (num3 != 1)
				{
					goto IL_FC;
				}
				if (global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit != null && global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.CurrentAction != null)
				{
					text2 = global::PandoraSingleton<global::MissionManager>.Instance.focusedUnit.CurrentAction.skillData.Name;
				}
			}
			else
			{
				text2 = this.unitCtrlr.unit.Data.Name;
			}
			IL_117:
			if (text2 == string.Empty)
			{
				global::PandoraDebug.LogWarning("No value found for parameter (" + text + ") current soundName: " + soundName, "SOUND", null);
				return;
			}
			soundName = soundName.Substring(0, num) + text2.ToLower() + soundName.Substring(num2 + 1, soundName.Length - num2 - 1);
			continue;
			IL_FC:
			global::PandoraDebug.LogWarning("Unsupported Sound Param " + text, "SOUND", null);
			goto IL_117;
		}
	}

	private void GetRandomSound(ref string soundName, ref string lastSoundPlayed)
	{
		int num = int.Parse(soundName.Substring(soundName.Length - 1));
		string arg = soundName.Substring(0, soundName.Length - 1);
		int num2 = global::PandoraSingleton<global::GameManager>.Instance.LocalTyche.Rand(1, num + 1);
		string a = arg + num2;
		if (a == lastSoundPlayed)
		{
			num2 = (num2 + 1) % (num + 1);
			if (num2 == 0)
			{
				num2++;
			}
		}
		lastSoundPlayed = arg + num2;
		soundName = lastSoundPlayed;
	}

	private void PlaySound(ref string name)
	{
		global::PandoraSingleton<global::Pan>.Instance.GetSound(name, true, delegate(global::UnityEngine.AudioClip clip)
		{
			if (clip != null && this.unitCtrlr.audioSource != null)
			{
				this.unitCtrlr.audioSource.PlayOneShot(clip);
			}
		});
	}

	private global::AnimStyleId weaponStyle;

	protected string lastShout;

	private global::UnitMenuController unitMenuCtrlr;
}
