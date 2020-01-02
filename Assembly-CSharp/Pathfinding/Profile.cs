using System;
using System.Diagnostics;
using UnityEngine;

namespace Pathfinding
{
	public class Profile
	{
		public Profile(string name)
		{
			this.name = name;
			this.watch = new global::System.Diagnostics.Stopwatch();
		}

		public int ControlValue()
		{
			return this.control;
		}

		[global::System.Diagnostics.Conditional("PROFILE")]
		public void Start()
		{
			this.watch.Start();
		}

		[global::System.Diagnostics.Conditional("PROFILE")]
		public void Stop()
		{
			this.counter++;
			this.watch.Stop();
		}

		[global::System.Diagnostics.Conditional("PROFILE")]
		public void Log()
		{
			global::UnityEngine.Debug.Log(this.ToString());
		}

		[global::System.Diagnostics.Conditional("PROFILE")]
		public void ConsoleLog()
		{
			global::System.Console.WriteLine(this.ToString());
		}

		[global::System.Diagnostics.Conditional("PROFILE")]
		public void Stop(int control)
		{
			this.counter++;
			this.watch.Stop();
			if (this.control == 1073741824)
			{
				this.control = control;
			}
			else if (this.control != control)
			{
				throw new global::System.Exception(string.Concat(new object[]
				{
					"Control numbers do not match ",
					this.control,
					" != ",
					control
				}));
			}
		}

		[global::System.Diagnostics.Conditional("PROFILE")]
		public void Control(global::Pathfinding.Profile other)
		{
			if (this.ControlValue() != other.ControlValue())
			{
				throw new global::System.Exception(string.Concat(new object[]
				{
					"Control numbers do not match (",
					this.name,
					" ",
					other.name,
					") ",
					this.ControlValue(),
					" != ",
					other.ControlValue()
				}));
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.name,
				" #",
				this.counter,
				" ",
				this.watch.Elapsed.TotalMilliseconds.ToString("0.0 ms"),
				" avg: ",
				(this.watch.Elapsed.TotalMilliseconds / (double)this.counter).ToString("0.00 ms")
			});
		}

		private const bool PROFILE_MEM = false;

		private const bool dontCountFirst = false;

		public readonly string name;

		private readonly global::System.Diagnostics.Stopwatch watch;

		private int counter;

		private long mem;

		private long smem;

		private int control = 1073741824;
	}
}
