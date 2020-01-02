using System;
using System.Text;

namespace Pathfinding.Util
{
	public struct Guid
	{
		public Guid(byte[] bytes)
		{
			ulong num = (ulong)bytes[0] | (ulong)bytes[1] << 8 | (ulong)bytes[2] << 16 | (ulong)bytes[3] << 24 | (ulong)bytes[4] << 32 | (ulong)bytes[5] << 40 | (ulong)bytes[6] << 48 | (ulong)bytes[7] << 56;
			ulong num2 = (ulong)bytes[8] | (ulong)bytes[9] << 8 | (ulong)bytes[10] << 16 | (ulong)bytes[11] << 24 | (ulong)bytes[12] << 32 | (ulong)bytes[13] << 40 | (ulong)bytes[14] << 48 | (ulong)bytes[15] << 56;
			this._a = ((!global::System.BitConverter.IsLittleEndian) ? global::Pathfinding.Util.Guid.SwapEndianness(num) : num);
			this._b = ((!global::System.BitConverter.IsLittleEndian) ? global::Pathfinding.Util.Guid.SwapEndianness(num2) : num2);
		}

		public Guid(string str)
		{
			this._a = 0UL;
			this._b = 0UL;
			if (str.Length < 32)
			{
				throw new global::System.FormatException("Invalid Guid format");
			}
			int i = 0;
			int num = 0;
			int num2 = 60;
			while (i < 16)
			{
				if (num >= str.Length)
				{
					throw new global::System.FormatException("Invalid Guid format. String too short");
				}
				char c = str[num];
				if (c != '-')
				{
					int num3 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c));
					if (num3 == -1)
					{
						throw new global::System.FormatException("Invalid Guid format : " + c + " is not a hexadecimal character");
					}
					this._a |= (ulong)((ulong)((long)num3) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
			num2 = 60;
			while (i < 32)
			{
				if (num >= str.Length)
				{
					throw new global::System.FormatException("Invalid Guid format. String too short");
				}
				char c2 = str[num];
				if (c2 != '-')
				{
					int num4 = "0123456789ABCDEF".IndexOf(char.ToUpperInvariant(c2));
					if (num4 == -1)
					{
						throw new global::System.FormatException("Invalid Guid format : " + c2 + " is not a hexadecimal character");
					}
					this._b |= (ulong)((ulong)((long)num4) << num2);
					num2 -= 4;
					i++;
				}
				num++;
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Guid()
		{
			global::Pathfinding.Util.Guid guid = new global::Pathfinding.Util.Guid(new byte[16]);
			global::Pathfinding.Util.Guid.zeroString = guid.ToString();
			global::Pathfinding.Util.Guid.random = new global::System.Random();
		}

		public static global::Pathfinding.Util.Guid Parse(string input)
		{
			return new global::Pathfinding.Util.Guid(input);
		}

		private static ulong SwapEndianness(ulong value)
		{
			ulong num = value & 255UL;
			ulong num2 = value >> 8 & 255UL;
			ulong num3 = value >> 16 & 255UL;
			ulong num4 = value >> 24 & 255UL;
			ulong num5 = value >> 32 & 255UL;
			ulong num6 = value >> 40 & 255UL;
			ulong num7 = value >> 48 & 255UL;
			ulong num8 = value >> 56 & 255UL;
			return num << 56 | num2 << 48 | num3 << 40 | num4 << 32 | num5 << 24 | num6 << 16 | num7 << 8 | num8;
		}

		public byte[] ToByteArray()
		{
			byte[] array = new byte[16];
			byte[] bytes = global::System.BitConverter.GetBytes(global::System.BitConverter.IsLittleEndian ? this._a : global::Pathfinding.Util.Guid.SwapEndianness(this._a));
			byte[] bytes2 = global::System.BitConverter.GetBytes(global::System.BitConverter.IsLittleEndian ? this._b : global::Pathfinding.Util.Guid.SwapEndianness(this._b));
			for (int i = 0; i < 8; i++)
			{
				array[i] = bytes[i];
				array[i + 8] = bytes2[i];
			}
			return array;
		}

		public static global::Pathfinding.Util.Guid NewGuid()
		{
			byte[] array = new byte[16];
			global::Pathfinding.Util.Guid.random.NextBytes(array);
			return new global::Pathfinding.Util.Guid(array);
		}

		public override bool Equals(object _rhs)
		{
			if (!(_rhs is global::Pathfinding.Util.Guid))
			{
				return false;
			}
			global::Pathfinding.Util.Guid guid = (global::Pathfinding.Util.Guid)_rhs;
			return this._a == guid._a && this._b == guid._b;
		}

		public override int GetHashCode()
		{
			ulong num = this._a ^ this._b;
			return (int)(num >> 32) ^ (int)num;
		}

		public override string ToString()
		{
			if (global::Pathfinding.Util.Guid.text == null)
			{
				global::Pathfinding.Util.Guid.text = new global::System.Text.StringBuilder();
			}
			global::System.Text.StringBuilder obj = global::Pathfinding.Util.Guid.text;
			string result;
			lock (obj)
			{
				global::Pathfinding.Util.Guid.text.Length = 0;
				global::Pathfinding.Util.Guid.text.Append(this._a.ToString("x16")).Append('-').Append(this._b.ToString("x16"));
				result = global::Pathfinding.Util.Guid.text.ToString();
			}
			return result;
		}

		public static bool operator ==(global::Pathfinding.Util.Guid lhs, global::Pathfinding.Util.Guid rhs)
		{
			return lhs._a == rhs._a && lhs._b == rhs._b;
		}

		public static bool operator !=(global::Pathfinding.Util.Guid lhs, global::Pathfinding.Util.Guid rhs)
		{
			return lhs._a != rhs._a || lhs._b != rhs._b;
		}

		private const string hex = "0123456789ABCDEF";

		public static readonly global::Pathfinding.Util.Guid zero = new global::Pathfinding.Util.Guid(new byte[16]);

		public static readonly string zeroString;

		private readonly ulong _a;

		private readonly ulong _b;

		private static global::System.Random random;

		private static global::System.Text.StringBuilder text;
	}
}
