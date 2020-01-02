using System;

public sealed class Tuple<T1, T2, T3>
{
	public Tuple(T1 item1, T2 item2, T3 item3)
	{
		this.item1 = item1;
		this.item2 = item2;
		this.item3 = item3;
	}

	public T1 Item1
	{
		get
		{
			return this.item1;
		}
	}

	public T2 Item2
	{
		get
		{
			return this.item2;
		}
	}

	public T3 Item3
	{
		get
		{
			return this.item3;
		}
	}

	public override int GetHashCode()
	{
		int num = 17;
		int num2 = num * 23;
		T1 t = this.item1;
		num = num2 + t.GetHashCode();
		int num3 = num * 23;
		T2 t2 = this.item2;
		num = num3 + t2.GetHashCode();
		int num4 = num * 23;
		T3 t3 = this.item3;
		return num4 + t3.GetHashCode();
	}

	public override bool Equals(object o)
	{
		if (o.GetType() != typeof(global::Tuple<T1, T2, T3>))
		{
			return false;
		}
		global::Tuple<T1, T2, T3> tuple = (global::Tuple<T1, T2, T3>)o;
		return this == tuple;
	}

	public void Unpack(global::System.Action<T1, T2, T3> unpackerDelegate)
	{
		unpackerDelegate(this.Item1, this.Item2, this.Item3);
	}

	private readonly T1 item1;

	private readonly T2 item2;

	private readonly T3 item3;
}
