using System;
using System.Collections.Generic;
using RAIN.Core;

public class AIGatherWeapons : global::AIGatherLoot
{
	public override void Start(global::RAIN.Core.AI ai)
	{
		base.Start(ai);
		this.actionName = "GatherWyrdstones";
	}

	protected override int GetSearchSlot(global::System.Collections.Generic.List<global::Item> searchItems)
	{
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < searchItems.Count; i++)
		{
			if (searchItems[i].Id != global::ItemId.NONE && searchItems[i].TypeData.ItemCategoryId == global::ItemCategoryId.WEAPONS)
			{
				int rating = searchItems[i].GetRating();
				if (num == -1 || rating > num2)
				{
					num = i;
					num2 = rating;
				}
			}
		}
		return num;
	}
}
