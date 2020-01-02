using System;
using System.IO;

public class CombatLogger
{
	public CombatLogger()
	{
		this.Crc = 0UL;
		using (global::System.IO.StreamWriter streamWriter = global::System.IO.File.CreateText("combat.log"))
		{
			streamWriter.WriteLine("Welcome To Mordheim V1.4.4.4");
		}
	}

	public ulong Crc { get; private set; }

	public void AddLog(global::Unit unit, global::CombatLogger.LogMessage message, params string[] parms)
	{
		this.AddLog(global::PandoraSingleton<global::MissionManager>.Instance.GetUnitController(unit, false), message, parms);
	}

	public void AddLog(global::UnitController unitCtrlr, global::CombatLogger.LogMessage message, params string[] parms)
	{
		bool flag = unitCtrlr.IsImprintVisible();
		if (flag)
		{
			this.AddLog(message, parms);
		}
	}

	public void AddLog(global::CombatLogger.LogMessage message, params string[] parms)
	{
		string stringById = global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("log_" + message.ToLowerString(), parms);
		global::PandoraSingleton<global::NoticeManager>.Instance.SendNotice<string>(global::Notices.COMBAT_LOG, stringById);
		this.Write(string.Format(this.GetLogFileString(message), parms));
		global::PandoraDebug.LogInfo(stringById, "COMBAT LOG", null);
	}

	private void Write(string log)
	{
		using (global::System.IO.StreamWriter streamWriter = global::System.IO.File.AppendText("combat.log"))
		{
			streamWriter.WriteLine(log);
		}
	}

	private string GetLogFileString(global::CombatLogger.LogMessage msg)
	{
		switch (msg)
		{
		case global::CombatLogger.LogMessage.ROUT_SUCCESS:
			return "Rout test successful";
		case global::CombatLogger.LogMessage.ROUT_FAIL:
			return "Rout test failed";
		case global::CombatLogger.LogMessage.MORAL_BELOW:
			return "Warband's morale {0} / {1} is below {2}%";
		case global::CombatLogger.LogMessage.TURN_START:
			return "{0}'s turn start";
		case global::CombatLogger.LogMessage.TURN_END:
			return "{0}'s turn end";
		case global::CombatLogger.LogMessage.ROUND_START:
			return "Round {0}";
		case global::CombatLogger.LogMessage.PERFORM_SKILL:
			return "{0} is performing a {1}";
		case global::CombatLogger.LogMessage.PERFORM_SKILL_TARGETS:
			return "{0} is performing {1} and targeting {2}";
		case global::CombatLogger.LogMessage.UNIT_ROLL_SUCCESS:
			return "{0}: Roll {1} success : {2} target {3}";
		case global::CombatLogger.LogMessage.UNIT_ROLL_FAIL:
			return "{0}: Roll {1} fail : {2} target {3}";
		case global::CombatLogger.LogMessage.ROLL_SUCCESS:
			return "Roll {0} success : {1} target {2}";
		case global::CombatLogger.LogMessage.ROLL_FAIL:
			return "Roll {0} fail : {1} target {2}";
		case global::CombatLogger.LogMessage.CURSE_TARGET:
			return "Curse {0} applied on {1}";
		case global::CombatLogger.LogMessage.DAMAGE:
			return "{0} received {1} damages";
		case global::CombatLogger.LogMessage.DAMAGE_INFLICT:
			return "{0} inflicted {1} damages to {2}";
		case global::CombatLogger.LogMessage.DAMAGE_CRIT_INFLICT:
			return "{0} inflicted {1} critical damages to {2}";
		case global::CombatLogger.LogMessage.UNIT_STATUS:
			return "{0} Status is now {1}";
		case global::CombatLogger.LogMessage.UNIT_OUT_OF_ACTION:
			return "{0} is now Out of Action -{1} morale";
		case global::CombatLogger.LogMessage.LOSE_IDOL:
			return "Warband {0} has lost its idol -{1} morale";
		case global::CombatLogger.LogMessage.GAIN_IDOL:
			return "Warband {0} recovered its idol +{1} morale";
		case global::CombatLogger.LogMessage.STUNNED_HIT:
			return "{0} is stunned. Automatic hit.";
		case global::CombatLogger.LogMessage.HEALING:
			return "{0} recovered {1} wounds";
		case global::CombatLogger.LogMessage.HEALING_RECEIVED:
			return "{0} restored {1} wounds to {2}";
		default:
			return string.Empty;
		}
	}

	private const string file = "combat.log";

	public enum LogMessage
	{
		ROUT_SUCCESS,
		ROUT_FAIL,
		MORAL_BELOW,
		TURN_START,
		TURN_END,
		ROUND_START,
		PERFORM_SKILL,
		PERFORM_SKILL_TARGETS,
		UNIT_ROLL_SUCCESS,
		UNIT_ROLL_FAIL,
		ROLL_SUCCESS,
		ROLL_FAIL,
		CURSE_TARGET,
		DAMAGE,
		DAMAGE_INFLICT,
		DAMAGE_CRIT_INFLICT,
		UNIT_STATUS,
		UNIT_OUT_OF_ACTION,
		LOSE_IDOL,
		GAIN_IDOL,
		STUNNED_HIT,
		HEALING,
		HEALING_RECEIVED
	}
}
