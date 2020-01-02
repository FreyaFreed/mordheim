using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TNet
{
	[global::System.Serializable]
	public class List<T> : global::TNet.TList
	{
		public global::System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			if (this.buffer != null)
			{
				for (int i = 0; i < this.size; i++)
				{
					yield return this.buffer[i];
				}
			}
			yield break;
		}

		public T this[int i]
		{
			get
			{
				return this.buffer[i];
			}
			set
			{
				this.buffer[i] = value;
			}
		}

		public int Count
		{
			get
			{
				return this.size;
			}
		}

		public object Get(int index)
		{
			return this.buffer[index];
		}

		private void AllocateMore()
		{
			int num = (this.buffer != null) ? (this.buffer.Length << 1) : 0;
			if (num < 32)
			{
				num = 32;
			}
			T[] array = new T[num];
			if (this.buffer != null && this.size > 0)
			{
				this.buffer.CopyTo(array, 0);
			}
			this.buffer = array;
		}

		private void Trim()
		{
			if (this.size > 0)
			{
				if (this.size < this.buffer.Length)
				{
					T[] array = new T[this.size];
					for (int i = 0; i < this.size; i++)
					{
						array[i] = this.buffer[i];
					}
					this.buffer = array;
				}
			}
			else
			{
				this.buffer = new T[0];
			}
		}

		public void Clear()
		{
			this.size = 0;
		}

		public void Release()
		{
			this.size = 0;
			this.buffer = null;
		}

		public void Add(T item)
		{
			if (this.buffer == null || this.size == this.buffer.Length)
			{
				this.AllocateMore();
			}
			this.buffer[this.size++] = item;
		}

		public void Add(object item)
		{
			if (this.buffer == null || this.size == this.buffer.Length)
			{
				this.AllocateMore();
			}
			this.buffer[this.size++] = (T)((object)item);
		}

		public void Insert(int index, T item)
		{
			if (this.buffer == null || this.size == this.buffer.Length)
			{
				this.AllocateMore();
			}
			if (index > -1 && index < this.size)
			{
				for (int i = this.size; i > index; i--)
				{
					this.buffer[i] = this.buffer[i - 1];
				}
				this.buffer[index] = item;
				this.size++;
			}
			else
			{
				this.Add(item);
			}
		}

		public bool Contains(T item)
		{
			if (this.buffer == null)
			{
				return false;
			}
			for (int i = 0; i < this.size; i++)
			{
				if (this.buffer[i].Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		public int IndexOf(T item)
		{
			if (this.buffer == null)
			{
				return -1;
			}
			for (int i = 0; i < this.size; i++)
			{
				if (this.buffer[i].Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		public bool Remove(T item)
		{
			if (this.buffer != null)
			{
				global::System.Collections.Generic.EqualityComparer<T> @default = global::System.Collections.Generic.EqualityComparer<T>.Default;
				for (int i = 0; i < this.size; i++)
				{
					if (@default.Equals(this.buffer[i], item))
					{
						this.size--;
						this.buffer[i] = default(T);
						for (int j = i; j < this.size; j++)
						{
							this.buffer[j] = this.buffer[j + 1];
						}
						return true;
					}
				}
			}
			return false;
		}

		public void RemoveAt(int index)
		{
			if (this.buffer != null && index > -1 && index < this.size)
			{
				this.size--;
				this.buffer[index] = default(T);
				for (int i = index; i < this.size; i++)
				{
					this.buffer[i] = this.buffer[i + 1];
				}
			}
		}

		public T Pop()
		{
			if (this.buffer != null && this.size != 0)
			{
				T result = this.buffer[--this.size];
				this.buffer[this.size] = default(T);
				return result;
			}
			return default(T);
		}

		public T[] ToArray()
		{
			this.Trim();
			return this.buffer;
		}

		[global::System.Diagnostics.DebuggerStepThrough]
		[global::System.Diagnostics.DebuggerHidden]
		public void Sort(global::TNet.List<T>.CompareFunc comparer)
		{
			int num = 0;
			int num2 = this.size - 1;
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = num; i < num2; i++)
				{
					if (comparer(this.buffer[i], this.buffer[i + 1]) > 0)
					{
						T t = this.buffer[i];
						this.buffer[i] = this.buffer[i + 1];
						this.buffer[i + 1] = t;
						flag = true;
					}
					else if (!flag)
					{
						num = ((i != 0) ? (i - 1) : 0);
					}
				}
			}
		}

		public T[] buffer;

		public int size;

		public delegate int CompareFunc(T left, T right);
	}
}
