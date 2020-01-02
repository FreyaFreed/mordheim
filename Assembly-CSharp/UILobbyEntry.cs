using System;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyEntry : global::UnityEngine.MonoBehaviour
{
	public void Set(string game, string map, int ratingMin, int ratingMax, bool isContest)
	{
		this.gameName.text = game;
		this.mapName.text = map;
		this.rating.text = string.Concat(new object[]
		{
			"[",
			ratingMin,
			",",
			ratingMax,
			"]"
		});
		this.difficultyRating.gameObject.SetActive(false);
		this.exhibitionIcon.gameObject.SetActive(!isContest);
		this.contestIcon.gameObject.SetActive(isContest);
	}

	public global::UnityEngine.UI.Text gameName;

	public global::UnityEngine.UI.Text mapName;

	public global::UnityEngine.UI.Text rating;

	public global::UnityEngine.UI.Image difficultyRating;

	public global::UnityEngine.UI.Image contestIcon;

	public global::UnityEngine.UI.Image exhibitionIcon;
}
