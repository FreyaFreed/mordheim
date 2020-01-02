using System;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : global::PandoraSingleton<global::NoticeManager>
{
	public global::System.Collections.Generic.List<object> Parameters { get; private set; }

	public global::Notices NoticeId { get; private set; }

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.registry = new global::System.Collections.Generic.Dictionary<global::Notices, global::System.Collections.Generic.List<global::DelReceiveNotice>>(global::NoticesComparer.Instance);
		this.Parameters = new global::System.Collections.Generic.List<object>(8);
	}

	public void RegisterListener(global::Notices noticeId, global::DelReceiveNotice del)
	{
		if (!this.registry.ContainsKey(noticeId))
		{
			this.registry[noticeId] = new global::System.Collections.Generic.List<global::DelReceiveNotice>();
		}
		if (this.registry[noticeId].IndexOf(del) == -1)
		{
			this.registry[noticeId].Add(del);
		}
	}

	public void RemoveListener(global::Notices noticeId, global::DelReceiveNotice del)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			int num = this.registry[noticeId].IndexOf(del);
			if (num != -1)
			{
				this.registry[noticeId].RemoveAt(num);
			}
		}
	}

	public void SendNotice(global::Notices noticeId)
	{
		if (this.registry != null && this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			this.Parameters.Clear();
			for (int i = list.Count - 1; i >= 0; i--)
			{
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
		}
	}

	public void SendNotice<T1>(global::Notices noticeId, T1 v1)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.Parameters.Clear();
				this.Parameters.Add(v1);
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
			this.Parameters.Clear();
		}
	}

	public void SendNotice<T1, T2>(global::Notices noticeId, T1 v1, T2 v2)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.Parameters.Clear();
				this.Parameters.Add(v1);
				this.Parameters.Add(v2);
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
			this.Parameters.Clear();
		}
	}

	public void SendNotice<T1, T2, T3>(global::Notices noticeId, T1 v1, T2 v2, T3 v3)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.Parameters.Clear();
				this.Parameters.Add(v1);
				this.Parameters.Add(v2);
				this.Parameters.Add(v3);
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
			this.Parameters.Clear();
		}
	}

	public void SendNotice<T1, T2, T3, T4>(global::Notices noticeId, T1 v1, T2 v2, T3 v3, T4 v4)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.Parameters.Clear();
				this.Parameters.Add(v1);
				this.Parameters.Add(v2);
				this.Parameters.Add(v3);
				this.Parameters.Add(v4);
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
			this.Parameters.Clear();
		}
	}

	public void SendNotice<T1, T2, T3, T4, T5>(global::Notices noticeId, T1 v1, T2 v2, T3 v3, T4 v4, T5 v5)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.Parameters.Clear();
				this.Parameters.Add(v1);
				this.Parameters.Add(v2);
				this.Parameters.Add(v3);
				this.Parameters.Add(v4);
				this.Parameters.Add(v5);
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
			this.Parameters.Clear();
		}
	}

	public void SendNotice<T1, T2, T3, T4, T5, T6>(global::Notices noticeId, T1 v1, T2 v2, T3 v3, T4 v4, T5 v5, T6 v6)
	{
		if (this.registry.ContainsKey(noticeId))
		{
			global::System.Collections.Generic.List<global::DelReceiveNotice> list = this.registry[noticeId];
			this.NoticeId = noticeId;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.Parameters.Clear();
				this.Parameters.Add(v1);
				this.Parameters.Add(v2);
				this.Parameters.Add(v3);
				this.Parameters.Add(v4);
				this.Parameters.Add(v5);
				this.Parameters.Add(v6);
				try
				{
					list[i]();
				}
				catch (global::System.Exception exception)
				{
					global::UnityEngine.Debug.LogException(exception);
				}
			}
			this.Parameters.Clear();
		}
	}

	public void Clear()
	{
		this.Parameters.Clear();
		this.registry.Clear();
	}

	private global::System.Collections.Generic.Dictionary<global::Notices, global::System.Collections.Generic.List<global::DelReceiveNotice>> registry;
}
