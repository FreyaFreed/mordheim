using System;
using UnityEngine;
using UnityEngine.UI;

public class UISequenceMessage : global::UIUnitControllerChanged
{
	private void Awake()
	{
		this.actionIcon.enabled = false;
		global::PandoraSingleton<global::NoticeManager>.Instance.RegisterListener((!this.isLeft) ? global::Notices.COMBAT_RIGHT_MESSAGE : global::Notices.COMBAT_LEFT_MESSAGE, new global::DelReceiveNotice(this.OnMessage));
	}

	private void OnMessage()
	{
		string titleTextId = null;
		int percentRoll = -1;
		int minDamage = -1;
		int maxDamage = -1;
		int num = 0;
		while (num < global::PandoraSingleton<global::NoticeManager>.Instance.Parameters.Count && num < 4)
		{
			switch (num)
			{
			case 0:
				titleTextId = (string)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[0];
				break;
			case 1:
				percentRoll = (int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[1];
				break;
			case 2:
				minDamage = (int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[2];
				break;
			case 3:
				maxDamage = (int)global::PandoraSingleton<global::NoticeManager>.Instance.Parameters[3];
				break;
			}
			num++;
		}
		this.Message(titleTextId, percentRoll, minDamage, maxDamage);
	}

	private void ActionChanged()
	{
	}

	public void Message(string titleTextId, int percentRoll = -1, int minDamage = -1, int maxDamage = -1)
	{
		this.background.sprite = this.messageBackground;
		if (this.mastery != null)
		{
			this.mastery.gameObject.SetActive(titleTextId.EndsWith("_mstr", global::System.StringComparison.OrdinalIgnoreCase));
		}
		this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleTextId);
		this.SetMessage(-1, minDamage, maxDamage, true);
	}

	private void SetMessage(int percentRoll = -1, int minDamage = -1, int maxDamage = -1, bool hideWithTimer = true)
	{
		this.hideMessageWithTimer = hideWithTimer;
		this.OnEnable();
		this.title.enabled = true;
		this.actionIcon.enabled = false;
		this.percent.enabled = (percentRoll != -1);
		if (this.percent.enabled)
		{
			this.percent.text = percentRoll + "%";
		}
		this.damage.enabled = (minDamage + maxDamage > -1);
		if (this.damage.enabled)
		{
			this.damage.text = string.Format("{0}-{1}", minDamage, maxDamage);
		}
	}

	public void Warning(string titleTextId, int percentRoll = -1, int minDamage = -1, int maxDamage = -1, bool isPotential = true)
	{
		this.background.sprite = this.warningBackground;
		if (isPotential)
		{
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("reaction_potential", new string[]
			{
				"#" + titleTextId
			});
		}
		else
		{
			this.title.text = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(titleTextId);
		}
		this.SetMessage(percentRoll, minDamage, maxDamage, true);
	}

	public void WarningNoTimer(string titleTextId, int percentRoll = -1, int minDamage = -1, int maxDamage = -1, bool isPotential = true)
	{
		this.Warning(titleTextId, percentRoll, minDamage, maxDamage, isPotential);
		this.hideMessageWithTimer = false;
	}

	protected override void OnUnitChanged()
	{
	}

	public void HideWithTimer()
	{
		if (base.IsVisible)
		{
			if (!this.hideMessageWithTimer)
			{
				this.OnDisable();
				this.timer = 0f;
			}
			else if (this.timer <= 0f)
			{
				this.timer = 4f;
			}
		}
	}

	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= global::UnityEngine.Time.deltaTime;
			if (this.timer <= 0f)
			{
				this.OnDisable();
			}
		}
	}

	public const int NO_VALUE = -1;

	public global::UnityEngine.Sprite messageBackground;

	public global::UnityEngine.Sprite warningBackground;

	public global::UnityEngine.UI.Image background;

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Image actionIcon;

	public global::UnityEngine.UI.Image mastery;

	public global::UnityEngine.UI.Text damage;

	public global::UnityEngine.UI.Text percent;

	public bool isLeft;

	private float timer;

	private bool hideMessageWithTimer = true;
}
