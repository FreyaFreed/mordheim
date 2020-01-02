using System;

namespace TNet
{
	public interface TList
	{
		int Count { get; }

		object Get(int index);

		void Add(object obj);

		void RemoveAt(int index);

		void Clear();
	}
}
