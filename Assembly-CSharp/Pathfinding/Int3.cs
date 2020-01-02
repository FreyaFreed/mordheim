using System;
using UnityEngine;

namespace Pathfinding
{
	public struct Int3
	{
		public Int3(global::UnityEngine.Vector3 position)
		{
			this.x = (int)global::System.Math.Round((double)(position.x * 1000f));
			this.y = (int)global::System.Math.Round((double)(position.y * 1000f));
			this.z = (int)global::System.Math.Round((double)(position.z * 1000f));
		}

		public Int3(int _x, int _y, int _z)
		{
			this.x = _x;
			this.y = _y;
			this.z = _z;
		}

		public static global::Pathfinding.Int3 zero
		{
			get
			{
				return default(global::Pathfinding.Int3);
			}
		}

		public int this[int i]
		{
			get
			{
				return (i != 0) ? ((i != 1) ? this.z : this.y) : this.x;
			}
			set
			{
				if (i == 0)
				{
					this.x = value;
				}
				else if (i == 1)
				{
					this.y = value;
				}
				else
				{
					this.z = value;
				}
			}
		}

		public static float Angle(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			double num = (double)global::Pathfinding.Int3.Dot(lhs, rhs) / ((double)lhs.magnitude * (double)rhs.magnitude);
			num = ((num >= -1.0) ? ((num <= 1.0) ? num : 1.0) : -1.0);
			return (float)global::System.Math.Acos(num);
		}

		public static int Dot(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
		}

		public static long DotLong(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			return (long)lhs.x * (long)rhs.x + (long)lhs.y * (long)rhs.y + (long)lhs.z * (long)rhs.z;
		}

		public global::Pathfinding.Int3 Normal2D()
		{
			return new global::Pathfinding.Int3(this.z, this.y, -this.x);
		}

		public float magnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)global::System.Math.Sqrt(num * num + num2 * num2 + num3 * num3);
			}
		}

		public int costMagnitude
		{
			get
			{
				return (int)global::System.Math.Round((double)this.magnitude);
			}
		}

		[global::System.Obsolete("This property is deprecated. Use magnitude or cast to a Vector3")]
		public float worldMagnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)global::System.Math.Sqrt(num * num + num2 * num2 + num3 * num3) * 0.001f;
			}
		}

		public float sqrMagnitude
		{
			get
			{
				double num = (double)this.x;
				double num2 = (double)this.y;
				double num3 = (double)this.z;
				return (float)(num * num + num2 * num2 + num3 * num3);
			}
		}

		public long sqrMagnitudeLong
		{
			get
			{
				long num = (long)this.x;
				long num2 = (long)this.y;
				long num3 = (long)this.z;
				return num * num + num2 * num2 + num3 * num3;
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"( ",
				this.x,
				", ",
				this.y,
				", ",
				this.z,
				")"
			});
		}

		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			global::Pathfinding.Int3 @int = (global::Pathfinding.Int3)o;
			return this.x == @int.x && this.y == @int.y && this.z == @int.z;
		}

		public override int GetHashCode()
		{
			return this.x * 73856093 ^ this.y * 19349663 ^ this.z * 83492791;
		}

		public static bool operator ==(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
		}

		public static bool operator !=(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			return lhs.x != rhs.x || lhs.y != rhs.y || lhs.z != rhs.z;
		}

		public static explicit operator global::Pathfinding.Int3(global::UnityEngine.Vector3 ob)
		{
			return new global::Pathfinding.Int3((int)global::System.Math.Round((double)(ob.x * 1000f)), (int)global::System.Math.Round((double)(ob.y * 1000f)), (int)global::System.Math.Round((double)(ob.z * 1000f)));
		}

		public static explicit operator global::UnityEngine.Vector3(global::Pathfinding.Int3 ob)
		{
			return new global::UnityEngine.Vector3((float)ob.x * 0.001f, (float)ob.y * 0.001f, (float)ob.z * 0.001f);
		}

		public static global::Pathfinding.Int3 operator -(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			lhs.x -= rhs.x;
			lhs.y -= rhs.y;
			lhs.z -= rhs.z;
			return lhs;
		}

		public static global::Pathfinding.Int3 operator -(global::Pathfinding.Int3 lhs)
		{
			lhs.x = -lhs.x;
			lhs.y = -lhs.y;
			lhs.z = -lhs.z;
			return lhs;
		}

		public static global::Pathfinding.Int3 operator +(global::Pathfinding.Int3 lhs, global::Pathfinding.Int3 rhs)
		{
			lhs.x += rhs.x;
			lhs.y += rhs.y;
			lhs.z += rhs.z;
			return lhs;
		}

		public static global::Pathfinding.Int3 operator *(global::Pathfinding.Int3 lhs, int rhs)
		{
			lhs.x *= rhs;
			lhs.y *= rhs;
			lhs.z *= rhs;
			return lhs;
		}

		public static global::Pathfinding.Int3 operator *(global::Pathfinding.Int3 lhs, float rhs)
		{
			lhs.x = (int)global::System.Math.Round((double)((float)lhs.x * rhs));
			lhs.y = (int)global::System.Math.Round((double)((float)lhs.y * rhs));
			lhs.z = (int)global::System.Math.Round((double)((float)lhs.z * rhs));
			return lhs;
		}

		public static global::Pathfinding.Int3 operator *(global::Pathfinding.Int3 lhs, double rhs)
		{
			lhs.x = (int)global::System.Math.Round((double)lhs.x * rhs);
			lhs.y = (int)global::System.Math.Round((double)lhs.y * rhs);
			lhs.z = (int)global::System.Math.Round((double)lhs.z * rhs);
			return lhs;
		}

		public static global::Pathfinding.Int3 operator /(global::Pathfinding.Int3 lhs, float rhs)
		{
			lhs.x = (int)global::System.Math.Round((double)((float)lhs.x / rhs));
			lhs.y = (int)global::System.Math.Round((double)((float)lhs.y / rhs));
			lhs.z = (int)global::System.Math.Round((double)((float)lhs.z / rhs));
			return lhs;
		}

		public static implicit operator string(global::Pathfinding.Int3 ob)
		{
			return ob.ToString();
		}

		public const int Precision = 1000;

		public const float FloatPrecision = 1000f;

		public const float PrecisionFactor = 0.001f;

		public int x;

		public int y;

		public int z;
	}
}
