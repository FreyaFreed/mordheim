using System;
using UnityEngine;

public class WheelAction
{
	public WheelAction(global::ActionStatus action, global::Item item, global::UIWheelController.Category category) : this(action.LocalizedName, action.LocalizedDescription, category, action.GetIcon())
	{
		this.action = action;
		this.item = item;
		this.available = action.Available;
	}

	public WheelAction(global::ActionStatus action, global::UIWheelController.Category category) : this(action.LocalizedName, action.LocalizedDescription, category, action.GetIcon())
	{
		this.action = action;
		this.item = null;
		this.available = action.Available;
	}

	public WheelAction(global::Item item) : this(item.LocalizedName, item.GetLocalizedDescription(null), global::UIWheelController.Category.INVENTORY, item.GetIcon())
	{
		this.item = item;
		this.available = false;
	}

	private WheelAction(string name, string description, global::UIWheelController.Category category, global::UnityEngine.Sprite icon)
	{
		this.name = name;
		this.description = description;
		this.category = category;
		this.icon = icon;
		this.available = true;
	}

	public bool Available
	{
		get
		{
			return this.available;
		}
	}

	public global::UnityEngine.Sprite GetIcon()
	{
		return this.icon;
	}

	public string name;

	public string description;

	public global::UIWheelController.Category category;

	public bool available;

	public global::UnityEngine.Sprite icon;

	public global::ActionStatus action;

	public global::Item item;
}
