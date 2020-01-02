using System;

public class Tyche
{
	public Tyche(int seed, bool log = true)
	{
		this.log = log;
		this.Seed((ulong)((long)seed));
	}

	public Tyche(ulong initState, ulong initSeq)
	{
		this.Seed(initState, initSeq);
	}

	public int Count { get; private set; }

	private void Seed(ulong initState, ulong initSeq)
	{
		this.m_state = 0UL;
		this.m_inc = (initSeq << 1 | 1UL);
		this.Random32();
		this.m_state += initState;
		this.Random32();
		this.Count = 0;
	}

	private void Seed(ulong initState)
	{
		this.m_state = 0UL;
		this.m_inc = 1442695040888963407UL;
		this.Random32();
		this.m_state += initState;
		this.Random32();
		this.Count = 0;
	}

	private uint Random32()
	{
		this.Count++;
		ulong state = this.m_state;
		this.m_state = state * 6364136223846793005UL + this.m_inc;
		uint num = (uint)((state >> 18 ^ state) >> 27);
		int num2 = (int)(state >> 59);
		return num >> num2 | num << -num2;
	}

	private double RandomDouble()
	{
		return (this.Random32() >> 8) * 5.960465E-08;
	}

	private double BoundedFloat(double max, double value)
	{
		return (value >= max) ? ((double)global::System.BitConverter.ToSingle(global::System.BitConverter.GetBytes(global::System.BitConverter.ToInt32(global::System.BitConverter.GetBytes(value), 0) - 1), 0)) : value;
	}

	public uint Range32(uint exclusiveBound)
	{
		uint num = (uint)((4294967296UL - (ulong)exclusiveBound) % (ulong)exclusiveBound);
		uint num2;
		do
		{
			num2 = this.Random32();
		}
		while (num2 < num);
		return num2 % exclusiveBound;
	}

	public int Rand(int minimum, int exclusiveBound)
	{
		if (minimum == exclusiveBound)
		{
			return exclusiveBound;
		}
		uint exclusiveBound2 = (uint)(exclusiveBound - minimum);
		uint num = this.Range32(exclusiveBound2);
		return (int)(num + (uint)minimum);
	}

	public double Rand(double minimum, double exclusiveBound)
	{
		return this.BoundedFloat(exclusiveBound, this.RandomDouble() * (exclusiveBound - minimum) + minimum);
	}

	public int RandInt()
	{
		return (int)this.Random32();
	}

	private const ulong PCG_DEFAULT_INCREMENT_64 = 1442695040888963407UL;

	private ulong m_state;

	private ulong m_inc;

	private bool log;
}
