using System;

namespace TNet
{
	public class Player
	{
		public Player()
		{
		}

		public Player(string playerName)
		{
			this.name = playerName;
		}

		public global::TNet.DataNode dataNode
		{
			get
			{
				global::TNet.DataNode dataNode = this.data as global::TNet.DataNode;
				return dataNode ?? global::TNet.Player.mDummy;
			}
		}

		public const int version = 11;

		protected static int mPlayerCounter = 0;

		public int id = 1;

		public string name = "Guest";

		public object data;

		private static global::TNet.DataNode mDummy = new global::TNet.DataNode("Version", 11);
	}
}
