using System;
using System.Collections.Generic;

public class EventLogger
{
	public EventLogger(global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> log)
	{
		this.history = log;
	}

	public void RemoveLastHistory(global::EventLogger.LogEvent logEvent)
	{
		for (int i = this.history.Count - 1; i >= 0; i--)
		{
			if (this.history[i].Item2 == logEvent)
			{
				this.history.RemoveAt(i);
			}
		}
	}

	public void RemoveHistory(global::Tuple<int, global::EventLogger.LogEvent, int> log)
	{
		this.history.Remove(log);
	}

	public void AddHistory(int date, global::EventLogger.LogEvent logEvent, int data)
	{
		int num = this.history.Count;
		while (num > 0 && this.history[num - 1].Item1 > date)
		{
			num--;
		}
		this.history.Insert(num, new global::Tuple<int, global::EventLogger.LogEvent, int>(date, logEvent, data));
	}

	public global::Tuple<int, global::EventLogger.LogEvent, int> LastHistoryEvent()
	{
		if (this.history.Count > 0)
		{
			return this.history[this.history.Count - 1];
		}
		return null;
	}

	public bool HasEventAtDay(global::EventLogger.LogEvent logEvent, int date)
	{
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
		for (int i = 0; i < this.history.Count; i++)
		{
			if (this.history[i].Item1 == date && this.history[i].Item2 == logEvent)
			{
				return true;
			}
		}
		return false;
	}

	public global::Tuple<int, global::EventLogger.LogEvent, int> FindLastEvent(global::EventLogger.LogEvent logEvent)
	{
		for (int i = this.history.Count - 1; i >= 0; i--)
		{
			if (this.history[i].Item2 == logEvent)
			{
				return this.history[i];
			}
		}
		return null;
	}

	public global::Tuple<int, global::EventLogger.LogEvent, int> FindEventBefore(global::EventLogger.LogEvent logEvent, int date)
	{
		for (int i = this.history.Count - 1; i >= 0; i--)
		{
			if (this.history[i].Item1 <= date && this.history[i].Item2 == logEvent)
			{
				return this.history[i];
			}
		}
		return null;
	}

	public global::Tuple<int, global::EventLogger.LogEvent, int> FindEventAfter(global::EventLogger.LogEvent logEvent, int date)
	{
		for (int i = 0; i < this.history.Count; i++)
		{
			if (this.history[i].Item1 >= date && this.history[i].Item2 == logEvent)
			{
				return this.history[i];
			}
		}
		return null;
	}

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> FindEventsAfter(global::EventLogger.LogEvent logEvent, int date)
	{
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
		for (int i = 0; i < this.history.Count; i++)
		{
			if (this.history[i].Item1 >= date && this.history[i].Item2 == logEvent)
			{
				list.Add(this.history[i]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> FindEventsAtDay(global::EventLogger.LogEvent logEvent, int date)
	{
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
		for (int i = 0; i < this.history.Count; i++)
		{
			if (this.history[i].Item1 == date && this.history[i].Item2 == logEvent)
			{
				list.Add(this.history[i]);
			}
		}
		return list;
	}

	public global::Tuple<int, global::EventLogger.LogEvent, int> FindEventBetween(global::EventLogger.LogEvent logEvent, int from, int to)
	{
		for (int i = 0; i < this.history.Count; i++)
		{
			if (this.history[i].Item1 >= from && this.history[i].Item1 <= to && this.history[i].Item2 == logEvent)
			{
				return this.history[i];
			}
		}
		return null;
	}

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> GetEventsBetween(int from, int to)
	{
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
		for (int i = 0; i < this.history.Count; i++)
		{
			if (this.history[i].Item1 >= from && this.history[i].Item1 <= to)
			{
				list.Add(this.history[i]);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> GetEventsAtDay(int day)
	{
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
		for (int i = this.history.Count - 1; i >= 0; i--)
		{
			global::Tuple<int, global::EventLogger.LogEvent, int> tuple = this.history[i];
			if (tuple.Item1 < day)
			{
				break;
			}
			if (tuple.Item1 == day)
			{
				list.Add(tuple);
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> GetEventsFromDay(int day)
	{
		global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> list = new global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>>();
		if (this.history.Count > 0)
		{
			int i;
			for (i = this.history.Count - 1; i >= 0; i--)
			{
				if (this.history[i].Item1 <= day)
				{
					i++;
					break;
				}
			}
			if (i < 0)
			{
				i++;
			}
			while (i < this.history.Count)
			{
				list.Add(this.history[i]);
				i++;
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::Tuple<int, global::EventLogger.LogEvent, int>> history;

	public enum LogEvent
	{
		NONE,
		HIRE,
		FIRE,
		DEATH,
		RETIREMENT,
		LEFT,
		INJURY,
		MUTATION,
		RECOVERY,
		NO_TREATMENT,
		SKILL,
		SPELL,
		SHIPMENT_REQUEST,
		SHIPMENT_DELIVERY,
		SHIPMENT_LATE,
		FACTION0_DELIVERY,
		FACTION1_DELIVERY,
		FACTION2_DELIVERY,
		NEW_MISSION,
		CONTACT_ITEM,
		MARKET_ROTATION,
		OUTSIDER_ROTATION,
		WARBAND_CREATED,
		RANK_ACHIEVED,
		MEMORABLE_CAMPAIGN_VICTORY,
		VICTORY_STREAK,
		MEMORABLE_KILL,
		MISSION_REWARDS,
		RESPEC_POINT,
		COUNT
	}
}
