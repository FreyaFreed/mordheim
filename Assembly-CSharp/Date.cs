using System;
using System.Collections.Generic;
using System.Text;

public class Date
{
	public Date(int currentDate)
	{
		this.CurrentDate = currentDate;
		this.Init();
	}

	public int CurrentDate { get; private set; }

	public int Day { get; private set; }

	public global::WeekDayId WeekDay { get; private set; }

	public global::MonthId Month { get; private set; }

	public int Year { get; private set; }

	public global::HolidayId Holiday { get; private set; }

	public global::MoonId Moon { get; private set; }

	private void Init()
	{
		this.Day = 0;
		this.WeekDay = global::WeekDayId.NONE;
		this.Month = global::MonthId.NONE;
		this.Holiday = global::HolidayId.NONE;
		this.Moon = global::MoonId.NONE;
		this.Year = this.CurrentDate / global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR);
		int num = this.CurrentDate - this.Year * global::Constant.GetInt(global::ConstantId.CAL_DAYS_PER_YEAR);
		this.Year += global::Constant.GetInt(global::ConstantId.CAL_YEAR_START);
		int num2 = 0;
		global::System.Collections.Generic.List<global::MonthData> list = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::MonthData>();
		list.Sort(delegate(global::MonthData x, global::MonthData y)
		{
			if (x.Id < y.Id)
			{
				return -1;
			}
			if (x.Id > y.Id)
			{
				return 1;
			}
			return 0;
		});
		foreach (global::MonthData monthData in list)
		{
			global::System.Collections.Generic.List<global::HolidayJoinMonthData> list2 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HolidayJoinMonthData>(new string[]
			{
				"fk_month_id",
				"intercalary"
			}, new string[]
			{
				((int)monthData.Id).ToString(),
				"1"
			});
			if (num == 0 && list2.Count == 1)
			{
				this.Holiday = list2[0].HolidayId;
				this.Moon = list2[0].MoonId;
				this.WeekDay = global::WeekDayId.NONE;
				this.Month = global::MonthId.NONE;
				break;
			}
			if (num < monthData.NumDays + list2.Count)
			{
				this.Day = num - list2.Count + 1;
				this.Month = monthData.Id;
				global::System.Collections.Generic.List<global::HolidayJoinMonthData> list3 = global::PandoraSingleton<global::DataFactory>.Instance.InitData<global::HolidayJoinMonthData>(new string[]
				{
					"fk_month_id",
					"day",
					"intercalary"
				}, new string[]
				{
					((int)monthData.Id).ToString(),
					this.Day.ToString(),
					"0"
				});
				if (list3.Count == 1)
				{
					this.Holiday = list3[0].HolidayId;
					this.Moon = list3[0].MoonId;
				}
				num2 += this.Day;
				this.WeekDay = (num2 - 1 + this.Year % 4 * 2) % 8 + global::WeekDayId.WELLENTAG;
				break;
			}
			num -= monthData.NumDays + list2.Count;
			num2 += monthData.NumDays;
		}
	}

	public int NextDay()
	{
		this.CurrentDate++;
		this.Init();
		return this.CurrentDate;
	}

	public string ToLocalizedHoliday()
	{
		if (this.Holiday != global::HolidayId.NONE && this.Month != global::MonthId.NONE)
		{
			return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_holiday_" + this.Holiday.ToString());
		}
		return string.Empty;
	}

	public string ToLocalizedString()
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		if (this.Month == global::MonthId.NONE)
		{
			stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_holiday_" + this.Holiday.ToString()));
		}
		else
		{
			stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_weekday_" + this.WeekDay));
			stringBuilder.Append(", ");
			stringBuilder.Append(this.Day);
			stringBuilder.Append(" ");
			stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_month_" + this.Month));
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Year);
		}
		return stringBuilder.ToString();
	}

	public string ToLocalizedAbbrString()
	{
		global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
		if (this.Month == global::MonthId.NONE)
		{
			stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_holiday_" + this.Holiday.ToString()));
		}
		else
		{
			stringBuilder.Append(this.Day);
			stringBuilder.Append(" ");
			stringBuilder.Append(global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById("calendar_month_" + this.Month));
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Year);
		}
		return stringBuilder.ToString();
	}

	public int GetNextDay(global::WeekDayId weekDayId)
	{
		global::Date date = new global::Date(this.CurrentDate + 1);
		while (date.WeekDay != weekDayId)
		{
			date.NextDay();
		}
		return date.CurrentDate;
	}
}
