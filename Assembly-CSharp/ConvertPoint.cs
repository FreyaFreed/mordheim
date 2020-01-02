using System;
using System.Collections.Generic;

public class ConvertPoint : global::SearchPoint
{
	public override void Init(uint id)
	{
		base.Init(id);
		this.SetCapacity(this.capacity);
	}

	public void SetCapacity(int cap)
	{
		this.capacity = cap;
		this.convertItems.Clear();
		for (int i = 0; i < this.capacity; i++)
		{
			this.convertItems.Add(new global::Item(this.convertedItemId, global::ItemQualityId.NORMAL));
		}
	}

	protected override void OnItemSwitched(global::UnitController unitctrlr)
	{
		int num = 0;
		for (int i = 0; i < this.items.Count; i++)
		{
			global::Item item = this.items[i];
			if (item.Id == this.slots[i].restrictedItemId && this.capacity > 0)
			{
				this.capacity--;
				num++;
				this.items[i] = this.convertItems[0];
				this.convertItems.RemoveAt(0);
			}
		}
		if (num > 0)
		{
			unitctrlr.GetWarband().ConvertItem(this.convertedItemId, num);
		}
		base.SpawnFxs(true);
	}

	public override bool IsEmpty()
	{
		return this.capacity == 0 && base.IsEmpty();
	}

	public override void Refresh()
	{
		base.Refresh();
		for (int i = 0; i < this.triggers.Count; i++)
		{
			if (this.triggers[i] != null)
			{
				this.triggers[i].SetActive(this.capacity > 0);
			}
		}
	}

	protected override bool CanInteract(global::UnitController unitCtrlr)
	{
		return (this.capacity > 0 || !this.IsEmpty()) && base.CanInteract(unitCtrlr);
	}

	public override global::System.Collections.Generic.List<global::Item> GetObjectiveItems()
	{
		this.objectiveItems.Clear();
		this.objectiveItems.AddRange(this.convertItems);
		this.objectiveItems.AddRange(this.items);
		return this.objectiveItems;
	}

	public override void Close(bool force = false)
	{
		base.Close(false);
		global::PandoraSingleton<global::MissionManager>.Instance.MissionEndData.UpdateConverters(this.guid, this.capacity);
	}

	public global::ItemId convertedItemId;

	public int capacity;

	public global::System.Collections.Generic.List<global::Item> convertItems = new global::System.Collections.Generic.List<global::Item>();

	public global::System.Collections.Generic.List<global::Item> objectiveItems = new global::System.Collections.Generic.List<global::Item>();
}
