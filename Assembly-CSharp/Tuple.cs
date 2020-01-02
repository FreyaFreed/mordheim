using System;

public static class Tuple
{
	public static global::Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 second)
	{
		return new global::Tuple<T1, T2>(item1, second);
	}

	public static global::Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 second, T3 third)
	{
		return new global::Tuple<T1, T2, T3>(item1, second, third);
	}

	public static global::Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 second, T3 third, T4 fourth)
	{
		return new global::Tuple<T1, T2, T3, T4>(item1, second, third, fourth);
	}
}
