using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSelector : global::UnityEngine.MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.radioButtons.Count; i++)
		{
			int index = i;
			this.radioButtons[i].but.onValueChanged.AddListener(delegate(bool visible)
			{
				if (visible)
				{
					this.ActivatePanel(index);
				}
				else
				{
					this.ActivatePanel(-1);
				}
			});
		}
	}

	private void Start()
	{
		for (int i = 0; i < this.radioButtons.Count; i++)
		{
			this.radioButtons[i].panel.SetActive(false);
		}
		if (this.startingPanel >= 0)
		{
			this.radioButtons[this.startingPanel].panel.SetActive(true);
			if (this.radioButtons[this.startingPanel].defaultSelection != null)
			{
				this.radioButtons[this.startingPanel].defaultSelection.SetSelected(true);
			}
		}
	}

	private void ActivatePanel(int index)
	{
		for (int i = 0; i < this.radioButtons.Count; i++)
		{
			this.radioButtons[i].panel.gameObject.SetActive(i == index);
		}
		if (index >= 0 && index < this.radioButtons.Count && this.radioButtons[index].defaultSelection != null)
		{
			this.radioButtons[index].defaultSelection.SetSelected(true);
		}
	}

	public int startingPanel;

	public global::System.Collections.Generic.List<global::PanelSelector.ButtonPanel> radioButtons = new global::System.Collections.Generic.List<global::PanelSelector.ButtonPanel>();

	[global::System.Serializable]
	public struct ButtonPanel
	{
		public global::UnityEngine.UI.Toggle but;

		public global::UnityEngine.GameObject panel;

		public global::UnityEngine.GameObject defaultSelection;
	}
}
