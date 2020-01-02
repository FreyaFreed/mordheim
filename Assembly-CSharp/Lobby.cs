using System;
using Steamworks;

public class Lobby
{
	public void SetPrivacy(global::Steamworks.ELobbyType type)
	{
		switch (type)
		{
		case global::Steamworks.ELobbyType.k_ELobbyTypePrivate:
			this.privacy = global::Hephaestus.LobbyPrivacy.OFFLINE;
			break;
		case global::Steamworks.ELobbyType.k_ELobbyTypeFriendsOnly:
			this.privacy = global::Hephaestus.LobbyPrivacy.FRIENDS;
			break;
		case global::Steamworks.ELobbyType.k_ELobbyTypePublic:
			this.privacy = global::Hephaestus.LobbyPrivacy.PUBLIC;
			break;
		case global::Steamworks.ELobbyType.k_ELobbyTypeInvisible:
			this.privacy = global::Hephaestus.LobbyPrivacy.PRIVATE;
			break;
		}
	}

	public const string ID = "id";

	public const string VERSION = "version";

	public const string NAME = "name";

	public const string PRIVACY = "privacy";

	public const string MAP = "map";

	public const string WARBAND = "warband";

	public const string EXHIBITION = "exhibition";

	public const string RATING_MIN = "rating_min";

	public const string RATING_MAX = "rating_max";

	public const string PASSWORD = "password";

	public const string JOINABLE = "joinable";

	public ulong id;

	public ulong hostId;

	public string version;

	public string name;

	public global::Hephaestus.LobbyPrivacy privacy;

	public int mapName;

	public int warbandId;

	public int ratingMin;

	public int ratingMax = 5000;

	public bool isExhibition;

	public bool joinable = true;
}
