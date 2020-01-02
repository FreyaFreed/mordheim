using System;

public class NetworkManager : global::IMyrtilus
{
	public NetworkManager()
	{
		this.RegisterToHermes();
		this.uid = 4294967292U;
	}

	public void Remove()
	{
		this.RemoveFromHermes();
	}

	public void SendLoadingDone(int warbandIndex)
	{
		global::PandoraDebug.LogDebug("SendLoadingDone WarbandIndex = " + warbandIndex, "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 1U, new object[]
		{
			warbandIndex
		});
	}

	private void LoadingDoneRPC(int warbandIndex)
	{
		global::PandoraDebug.LogDebug("SendLoadingDoneRPC  warbandIndex = " + warbandIndex, "HERMES", null);
		global::PandoraSingleton<global::MissionManager>.Instance.SetLoadingDone();
	}

	public void SendReadyToStart()
	{
		global::PandoraDebug.LogDebug("SendReadyToStart", "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.OTHERS, this.uid, 2U, new object[0]);
	}

	private void ReadyToStartRPC()
	{
		global::PandoraDebug.LogDebug("ReadyToStartRPC", "HERMES", null);
		global::PandoraSingleton<global::TransitionManager>.Instance.OnPlayersReady();
	}

	public void SendTurnReady()
	{
		global::PandoraDebug.LogDebug("SendTurnReady", "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 3U, new object[0]);
	}

	private void TurnReadyRPC()
	{
		global::PandoraDebug.LogDebug("TurnReadyRPC", "HERMES", null);
		global::PandoraSingleton<global::MissionManager>.Instance.SetTurnReady();
	}

	public void SendForfeitMission(int warbandIdx)
	{
		global::PandoraDebug.LogDebug("SendForfeitMission", "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 5U, new object[]
		{
			warbandIdx
		});
	}

	private void ForfeitMissionRPC(int warbandIdx)
	{
		global::PandoraDebug.LogDebug("ForfeitMissionRPC", "HERMES", null);
		global::PandoraSingleton<global::MissionManager>.Instance.ForfeitMission(warbandIdx);
	}

	public void SendQuitMission()
	{
		global::PandoraDebug.LogDebug("SendQuitMission", "HERMES", null);
		this.Send(true, global::Hermes.SendTarget.ALL, this.uid, 4U, new object[0]);
	}

	private void QuitMissionRPC()
	{
		global::PandoraDebug.LogDebug("QuitMissionRPC", "HERMES", null);
		global::PandoraSingleton<global::MissionManager>.Instance.ForceQuitMission();
	}

	public uint uid { get; set; }

	public uint owner { get; set; }

	public void RegisterToHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RegisterMyrtilus(this, true);
	}

	public void RemoveFromHermes()
	{
		global::PandoraSingleton<global::Hermes>.Instance.RemoveMyrtilus(this);
	}

	public void Send(bool reliable, global::Hermes.SendTarget target, uint id, uint command, params object[] parms)
	{
		global::PandoraSingleton<global::Hermes>.Instance.Send(reliable, target, id, command, parms);
	}

	public void Receive(ulong from, uint command, object[] parms)
	{
		switch (command)
		{
		case 1U:
		{
			int warbandIndex = (int)parms[0];
			this.LoadingDoneRPC(warbandIndex);
			break;
		}
		case 2U:
			this.ReadyToStartRPC();
			break;
		case 3U:
			this.TurnReadyRPC();
			break;
		case 4U:
			this.QuitMissionRPC();
			break;
		case 5U:
		{
			int warbandIdx = (int)parms[0];
			this.ForfeitMissionRPC(warbandIdx);
			break;
		}
		}
	}

	private enum CommandList
	{
		NONE,
		LOADING_DONE,
		READY_TO_START,
		TURN_READY,
		QUIT_MISSION,
		FORFEIT,
		COUNT
	}
}
