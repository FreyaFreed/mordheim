using System;
using UnityEngine;
using UnityEngine.UI;

public class InjuryMutationItem : global::UnityEngine.MonoBehaviour
{
	public void Set(global::Injury inj)
	{
		this.Set(inj.LocName, inj.LocDesc, inj.Data.Rating.ToConstantString());
	}

	public void Set(global::Mutation mut)
	{
		this.Set(mut.LocName, mut.LocDesc, mut.Data.Rating.ToConstantString());
	}

	public void Set(string title, string desc, string rating = "0")
	{
		this.title.text = title;
		this.desc.text = desc;
		this.rating.text = rating;
	}

	public global::UnityEngine.UI.Text title;

	public global::UnityEngine.UI.Text desc;

	public global::UnityEngine.UI.Text rating;
}
