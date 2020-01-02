using System;

namespace Pathfinding.Serialization
{
	public class SerializeSettings
	{
		public static global::Pathfinding.Serialization.SerializeSettings Settings
		{
			get
			{
				return new global::Pathfinding.Serialization.SerializeSettings
				{
					nodes = false
				};
			}
		}

		public bool nodes = true;

		[global::System.Obsolete("There is no support for pretty printing the json anymore")]
		public bool prettyPrint;

		public bool editorSettings;
	}
}
