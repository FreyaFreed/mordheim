using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TNet;
using UnityEngine;

[global::UnityEngine.ExecuteInEditMode]
public class TNAutoSync : global::TNBehaviour
{
	private void Awake()
	{
		int i = 0;
		int count = this.entries.Count;
		while (i < count)
		{
			global::TNAutoSync.SavedEntry savedEntry = this.entries[i];
			if (savedEntry.target != null && !string.IsNullOrEmpty(savedEntry.propertyName))
			{
				global::System.Reflection.FieldInfo field = savedEntry.target.GetType().GetField(savedEntry.propertyName, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);
				if (field != null)
				{
					global::TNAutoSync.ExtendedEntry extendedEntry = new global::TNAutoSync.ExtendedEntry();
					extendedEntry.target = savedEntry.target;
					extendedEntry.field = field;
					extendedEntry.lastValue = field.GetValue(savedEntry.target);
					this.mList.Add(extendedEntry);
				}
				else
				{
					global::System.Reflection.PropertyInfo property = savedEntry.target.GetType().GetProperty(savedEntry.propertyName, global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);
					if (property != null)
					{
						global::TNAutoSync.ExtendedEntry extendedEntry2 = new global::TNAutoSync.ExtendedEntry();
						extendedEntry2.target = savedEntry.target;
						extendedEntry2.property = property;
						extendedEntry2.lastValue = property.GetValue(savedEntry.target, null);
						this.mList.Add(extendedEntry2);
					}
					else
					{
						global::UnityEngine.Debug.LogError(string.Concat(new object[]
						{
							"Unable to find property: '",
							savedEntry.propertyName,
							"' on ",
							savedEntry.target.GetType()
						}));
					}
				}
			}
			i++;
		}
		if (this.mList.size > 0)
		{
			if (this.updatesPerSecond > 0)
			{
				base.StartCoroutine(this.PeriodicSync());
			}
		}
		else
		{
			global::UnityEngine.Debug.LogWarning("Nothing to sync", this);
			base.enabled = false;
		}
	}

	private global::System.Collections.IEnumerator PeriodicSync()
	{
		for (;;)
		{
			if (global::TNManager.isInChannel && this.updatesPerSecond > 0)
			{
				if (this.mList.size != 0 && (!this.onlyOwnerCanSync || base.tno.isMine) && this.Cache())
				{
					this.Sync();
				}
				yield return new global::UnityEngine.WaitForSeconds(1f / (float)this.updatesPerSecond);
			}
			else
			{
				yield return new global::UnityEngine.WaitForSeconds(0.1f);
			}
		}
		yield break;
	}

	private void OnNetworkPlayerJoin(global::TNet.Player p)
	{
		if (this.mList.size != 0 && !this.isSavedOnServer && global::TNManager.isHosting)
		{
			if (this.Cache())
			{
				this.Sync();
			}
			else
			{
				base.tno.Send(byte.MaxValue, p, this.mCached);
			}
		}
	}

	private bool Cache()
	{
		bool flag = false;
		bool flag2 = false;
		if (this.mCached == null)
		{
			flag = true;
			this.mCached = new object[this.mList.size];
		}
		for (int i = 0; i < this.mList.size; i++)
		{
			global::TNAutoSync.ExtendedEntry extendedEntry = this.mList[i];
			object obj = (extendedEntry.field == null) ? extendedEntry.property.GetValue(extendedEntry.target, null) : extendedEntry.field.GetValue(extendedEntry.target);
			if (!obj.Equals(extendedEntry.lastValue))
			{
				flag2 = true;
			}
			if (flag || flag2)
			{
				extendedEntry.lastValue = obj;
				this.mCached[i] = obj;
			}
		}
		return flag2;
	}

	public void Sync()
	{
		if (global::TNManager.isInChannel && this.mList.size != 0)
		{
			if (this.isImportant)
			{
				base.tno.Send(byte.MaxValue, (!this.isSavedOnServer) ? global::TNet.Target.Others : global::TNet.Target.OthersSaved, this.mCached);
			}
			else
			{
				base.tno.SendQuickly(byte.MaxValue, (!this.isSavedOnServer) ? global::TNet.Target.Others : global::TNet.Target.OthersSaved, this.mCached);
			}
		}
	}

	[global::TNet.RFC(255)]
	private void OnSync(object[] val)
	{
		if (base.enabled)
		{
			for (int i = 0; i < this.mList.size; i++)
			{
				global::TNAutoSync.ExtendedEntry extendedEntry = this.mList[i];
				extendedEntry.lastValue = val[i];
				if (extendedEntry.field != null)
				{
					extendedEntry.field.SetValue(extendedEntry.target, extendedEntry.lastValue);
				}
				else
				{
					extendedEntry.property.SetValue(extendedEntry.target, extendedEntry.lastValue, null);
				}
			}
		}
	}

	public global::System.Collections.Generic.List<global::TNAutoSync.SavedEntry> entries = new global::System.Collections.Generic.List<global::TNAutoSync.SavedEntry>();

	public int updatesPerSecond = 10;

	public bool isSavedOnServer = true;

	public bool onlyOwnerCanSync = true;

	public bool isImportant;

	private global::TNet.List<global::TNAutoSync.ExtendedEntry> mList = new global::TNet.List<global::TNAutoSync.ExtendedEntry>();

	private object[] mCached;

	[global::System.Serializable]
	public class SavedEntry
	{
		public global::UnityEngine.Component target;

		public string propertyName;
	}

	private class ExtendedEntry : global::TNAutoSync.SavedEntry
	{
		public global::System.Reflection.FieldInfo field;

		public global::System.Reflection.PropertyInfo property;

		public object lastValue;
	}
}
