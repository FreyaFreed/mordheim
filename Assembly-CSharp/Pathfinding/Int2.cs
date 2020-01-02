using System;

namespace Pathfinding
{
	public struct Int2
	{
		public Int2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public long sqrMagnitudeLong
		{
			get
			{
				return (long)this.x * (long)this.x + (long)this.y * (long)this.y;
			}
		}

		public static long DotLong(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return (long)a.x * (long)b.x + (long)a.y * (long)b.y;
		}

		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			global::Pathfinding.Int2 @int = (global::Pathfinding.Int2)o;
			return this.x == @int.x && this.y == @int.y;
		}

		public override int GetHashCode()
		{
			return this.x * 49157 + this.y * 98317;
		}

		[global::System.Obsolete("Deprecated becuase it is not used by any part of the A* Pathfinding Project")]
		public static global::Pathfinding.Int2 Rotate(global::Pathfinding.Int2 v, int r)
		{
			r %= 4;
			return new global::Pathfinding.Int2(v.x * global::Pathfinding.Int2.Rotations[r * 4] + v.y * global::Pathfinding.Int2.Rotations[r * 4 + 1], v.x * global::Pathfinding.Int2.Rotations[r * 4 + 2] + v.y * global::Pathfinding.Int2.Rotations[r * 4 + 3]);
		}

		public static global::Pathfinding.Int2 Min(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return new global::Pathfinding.Int2(global::System.Math.Min(a.x, b.x), global::System.Math.Min(a.y, b.y));
		}

		public static global::Pathfinding.Int2 Max(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return new global::Pathfinding.Int2(global::System.Math.Max(a.x, b.x), global::System.Math.Max(a.y, b.y));
		}

		public static global::Pathfinding.Int2 FromInt3XZ(global::Pathfinding.Int3 o)
		{
			return new global::Pathfinding.Int2(o.x, o.z);
		}

		public static global::Pathfinding.Int3 ToInt3XZ(global::Pathfinding.Int2 o)
		{
			return new global::Pathfinding.Int3(o.x, 0, o.y);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.x,
				", ",
				this.y,
				")"
			});
		}

		public static global::Pathfinding.Int2 operator +(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return new global::Pathfinding.Int2(a.x + b.x, a.y + b.y);
		}

		public static global::Pathfinding.Int2 operator -(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return new global::Pathfinding.Int2(a.x - b.x, a.y - b.y);
		}

		public static bool operator ==(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(global::Pathfinding.Int2 a, global::Pathfinding.Int2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public int x;

		public int y;

		private static readonly int[] Rotations = new int[]
		{
			1,
			0,
			0,
			1,
			0,
			1,
			-1,
			0,
			-1,
			0,
			0,
			-1,
			0,
			-1,
			1,
			0
		};
	}
}
