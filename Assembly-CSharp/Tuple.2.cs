using System;

public sealed class Tuple<T1, T2>
{
	public Tuple(T1 item1, T2 item2)
	{
		this.item1 = item1;
		this.item2 = item2;
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

	public override string ToString()
	{
		return string.Format("Tuple({0}, {1})", this.Item1, this.Item2);
	}

	public override int GetHashCode()
	{
		int num = 17;
		int num2 = num * 23;
		T1 t = this.item1;
		num = num2 + t.GetHashCode();
		int num3 = num * 23;
		T2 t2 = this.item2;
		return num3 + t2.GetHashCode();
	}

	public override bool Equals(object o)
	{
		if (o.GetType() != typeof(global::Tuple<T1, T2>))
		{
			return false;
		}
		global::Tuple<T1, T2> tuple = (global::Tuple<T1, T2>)o;
		return this == tuple;
	}

	public void Unpack(global::System.Action<T1, T2> unpackerDelegate)
	{
		unpackerDelegate(this.Item1, this.Item2);
	}

	private readonly T1 item1;

	private readonly T2 item2;
}
