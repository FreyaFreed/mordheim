using System;

public class UIUnitEnchantmentsContent : global::ContentView<global::UIUnitEnchantmentGroup, global::Enchantment>
{
	protected override void OnAdd(global::UIUnitEnchantmentGroup component, global::Enchantment obj)
	{
		component.Set(obj);
	}
}
