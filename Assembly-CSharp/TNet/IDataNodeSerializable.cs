using System;

namespace TNet
{
	public interface IDataNodeSerializable
	{
		void Serialize(global::TNet.DataNode node);

		void Deserialize(global::TNet.DataNode node);
	}
}
