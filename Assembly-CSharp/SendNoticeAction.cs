using System;
using UnityEngine;

public class SendNoticeAction : global::UnityEngine.MonoBehaviour
{
	private void SendNotice<T>(global::Notices notice, T arg)
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<T>(this.noticeId, arg);
	}

	public void SendNotice()
	{
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice(this.noticeId);
	}

	public void SendNoticeInt(int arg)
	{
		this.SendNotice<int>(this.noticeId, arg);
	}

	public void SendNoticeFloat(float arg)
	{
		this.SendNotice<float>(this.noticeId, arg);
	}

	public void SendNoticeString(string arg)
	{
		this.SendNotice<string>(this.noticeId, arg);
	}

	public void SendNoticeGameObject(global::UnityEngine.GameObject arg)
	{
		this.SendNotice<global::UnityEngine.GameObject>(this.noticeId, arg);
	}

	public void SendNoticeTransform(global::UnityEngine.Transform arg)
	{
		this.SendNotice<global::UnityEngine.Transform>(this.noticeId, arg);
	}

	public void SendNoticeRectTransform(global::UnityEngine.RectTransform arg)
	{
		this.SendNotice<global::UnityEngine.RectTransform>(this.noticeId, arg);
	}

	public void SendNoticeMainMenuState(global::MainMenuController.State arg)
	{
		this.SendNotice<global::MainMenuController.State>(this.noticeId, arg);
	}

	public global::Notices noticeId;
}
