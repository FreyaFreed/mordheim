using System;
using UnityEngine;

namespace Pathfinding
{
	public struct IntRect
	{
		public IntRect(int xmin, int ymin, int xmax, int ymax)
		{
			this.xmin = xmin;
			this.xmax = xmax;
			this.ymin = ymin;
			this.ymax = ymax;
		}

		public bool Contains(int x, int y)
		{
			return x >= this.xmin && y >= this.ymin && x <= this.xmax && y <= this.ymax;
		}

		public int Width
		{
			get
			{
				return this.xmax - this.xmin + 1;
			}
		}

		public int Height
		{
			get
			{
				return this.ymax - this.ymin + 1;
			}
		}

		public bool IsValid()
		{
			return this.xmin <= this.xmax && this.ymin <= this.ymax;
		}

		public override bool Equals(object _b)
		{
			global::Pathfinding.IntRect intRect = (global::Pathfinding.IntRect)_b;
			return this.xmin == intRect.xmin && this.xmax == intRect.xmax && this.ymin == intRect.ymin && this.ymax == intRect.ymax;
		}

		public override int GetHashCode()
		{
			return this.xmin * 131071 ^ this.xmax * 3571 ^ this.ymin * 3109 ^ this.ymax * 7;
		}

		public static global::Pathfinding.IntRect Intersection(global::Pathfinding.IntRect a, global::Pathfinding.IntRect b)
		{
			global::Pathfinding.IntRect result = new global::Pathfinding.IntRect(global::System.Math.Max(a.xmin, b.xmin), global::System.Math.Max(a.ymin, b.ymin), global::System.Math.Min(a.xmax, b.xmax), global::System.Math.Min(a.ymax, b.ymax));
			return result;
		}

		public static bool Intersects(global::Pathfinding.IntRect a, global::Pathfinding.IntRect b)
		{
			return a.xmin <= b.xmax && a.ymin <= b.ymax && a.xmax >= b.xmin && a.ymax >= b.ymin;
		}

		public static global::Pathfinding.IntRect Union(global::Pathfinding.IntRect a, global::Pathfinding.IntRect b)
		{
			global::Pathfinding.IntRect result = new global::Pathfinding.IntRect(global::System.Math.Min(a.xmin, b.xmin), global::System.Math.Min(a.ymin, b.ymin), global::System.Math.Max(a.xmax, b.xmax), global::System.Math.Max(a.ymax, b.ymax));
			return result;
		}

		public global::Pathfinding.IntRect ExpandToContain(int x, int y)
		{
			global::Pathfinding.IntRect result = new global::Pathfinding.IntRect(global::System.Math.Min(this.xmin, x), global::System.Math.Min(this.ymin, y), global::System.Math.Max(this.xmax, x), global::System.Math.Max(this.ymax, y));
			return result;
		}

		public global::Pathfinding.IntRect Expand(int range)
		{
			return new global::Pathfinding.IntRect(this.xmin - range, this.ymin - range, this.xmax + range, this.ymax + range);
		}

		public global::Pathfinding.IntRect Rotate(int r)
		{
			int num = global::Pathfinding.IntRect.Rotations[r * 4];
			int num2 = global::Pathfinding.IntRect.Rotations[r * 4 + 1];
			int num3 = global::Pathfinding.IntRect.Rotations[r * 4 + 2];
			int num4 = global::Pathfinding.IntRect.Rotations[r * 4 + 3];
			int val = num * this.xmin + num2 * this.ymin;
			int val2 = num3 * this.xmin + num4 * this.ymin;
			int val3 = num * this.xmax + num2 * this.ymax;
			int val4 = num3 * this.xmax + num4 * this.ymax;
			return new global::Pathfinding.IntRect(global::System.Math.Min(val, val3), global::System.Math.Min(val2, val4), global::System.Math.Max(val, val3), global::System.Math.Max(val2, val4));
		}

		public global::Pathfinding.IntRect Offset(global::Pathfinding.Int2 offset)
		{
			return new global::Pathfinding.IntRect(this.xmin + offset.x, this.ymin + offset.y, this.xmax + offset.x, this.ymax + offset.y);
		}

		public global::Pathfinding.IntRect Offset(int x, int y)
		{
			return new global::Pathfinding.IntRect(this.xmin + x, this.ymin + y, this.xmax + x, this.ymax + y);
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"[x: ",
				this.xmin,
				"...",
				this.xmax,
				", y: ",
				this.ymin,
				"...",
				this.ymax,
				"]"
			});
		}

		public void DebugDraw(global::UnityEngine.Matrix4x4 matrix, global::UnityEngine.Color col)
		{
			global::UnityEngine.Vector3 vector = matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)this.xmin, 0f, (float)this.ymin));
			global::UnityEngine.Vector3 vector2 = matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)this.xmin, 0f, (float)this.ymax));
			global::UnityEngine.Vector3 vector3 = matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)this.xmax, 0f, (float)this.ymax));
			global::UnityEngine.Vector3 vector4 = matrix.MultiplyPoint3x4(new global::UnityEngine.Vector3((float)this.xmax, 0f, (float)this.ymin));
			global::UnityEngine.Debug.DrawLine(vector, vector2, col);
			global::UnityEngine.Debug.DrawLine(vector2, vector3, col);
			global::UnityEngine.Debug.DrawLine(vector3, vector4, col);
			global::UnityEngine.Debug.DrawLine(vector4, vector, col);
		}

		public static bool operator ==(global::Pathfinding.IntRect a, global::Pathfinding.IntRect b)
		{
			return a.xmin == b.xmin && a.xmax == b.xmax && a.ymin == b.ymin && a.ymax == b.ymax;
		}

		public static bool operator !=(global::Pathfinding.IntRect a, global::Pathfinding.IntRect b)
		{
			return a.xmin != b.xmin || a.xmax != b.xmax || a.ymin != b.ymin || a.ymax != b.ymax;
		}

		public int xmin;

		public int ymin;

		public int xmax;

		public int ymax;

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
