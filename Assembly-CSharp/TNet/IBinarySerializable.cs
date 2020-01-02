using System;
using System.IO;

namespace TNet
{
	public interface IBinarySerializable
	{
		void Serialize(global::System.IO.BinaryWriter writer);

		void Deserialize(global::System.IO.BinaryReader reader);
	}
}
