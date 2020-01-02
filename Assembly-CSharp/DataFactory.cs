using System;
using System.Collections.Generic;
using System.Text;

public class DataFactory : global::PandoraSingleton<global::DataFactory>
{
	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		this.cachedTypeName = new global::System.Collections.Generic.Dictionary<global::System.Type, string>();
		global::PandoraSingleton<global::DBConnector>.Instance.Connect(global::PandoraSingleton<global::DBConnector>.Instance.databasePath);
		global::Constant.Init();
	}

	private void Clear()
	{
		this.whereBuilder.Remove(0, this.whereBuilder.Length);
		this.sortBuilder.Remove(0, this.sortBuilder.Length);
	}

	public T InitData<T>(int id) where T : global::DataCore, new()
	{
		this.Clear();
		global::DataFactory.AppendWhere(this.whereBuilder, "id", global::Constant.ToString(id));
		return this.InitDBObject<T>();
	}

	private string GetTableName(global::System.Type type)
	{
		string text;
		if (!this.cachedTypeName.TryGetValue(type, out text))
		{
			text = global::DBConnector.ClassNameToTableName(type.Name);
			this.cachedTypeName[type] = text;
		}
		return text;
	}

	public global::System.Collections.Generic.List<T> InitData<T>() where T : global::DataCore, new()
	{
		this.Clear();
		return this.InitDBObjects<T>();
	}

	public global::System.Collections.Generic.List<T> InitData<T>(string field1, string id1) where T : global::DataCore, new()
	{
		this.Clear();
		global::DataFactory.AppendWhere(this.whereBuilder, field1, id1);
		return this.InitDBObjects<T>();
	}

	public global::System.Collections.Generic.List<T> InitData<T>(string field1, string id1, string field2, string id2) where T : global::DataCore, new()
	{
		this.Clear();
		global::DataFactory.AppendWhere(this.whereBuilder, field1, id1);
		this.whereBuilder.Append(" AND ");
		global::DataFactory.AppendWhere(this.whereBuilder, field2, id2);
		return this.InitDBObjects<T>();
	}

	public global::System.Collections.Generic.List<T> InitData<T>(string[] fields, string[] ids) where T : global::DataCore, new()
	{
		this.Clear();
		for (int i = 0; i < fields.Length; i++)
		{
			if (i > 0)
			{
				this.whereBuilder.Append(" AND ");
			}
			global::DataFactory.AppendWhere(this.whereBuilder, fields[i], ids[i]);
		}
		return this.InitDBObjects<T>();
	}

	public T InitDataClosest<T>(string field, int id, bool lower) where T : global::DataCore, new()
	{
		return this.InitDataClosest<T>(field, id, new string[0], new string[0], lower);
	}

	public T InitDataClosest<T>(string field, int id, string[] fields, string[] ids, bool lower) where T : global::DataCore, new()
	{
		this.Clear();
		this.whereBuilder.Append(field);
		this.whereBuilder.Append((!lower) ? " >= " : " <= ");
		this.whereBuilder.Append(id.ToConstantString());
		for (int i = 0; i < fields.Length; i++)
		{
			this.whereBuilder.Append(" AND ");
			global::DataFactory.AppendWhere(this.whereBuilder, fields[i], ids[i]);
		}
		if (lower)
		{
			global::DataFactory.AppendSortDesc(this.sortBuilder, field);
		}
		else
		{
			global::DataFactory.AppendSortAsc(this.sortBuilder, field);
		}
		return this.InitDBObject<T>();
	}

	public global::System.Collections.Generic.List<T> InitDataRange<T>(string field, int low, int high, string[] fields, string[] ids) where T : global::DataCore, new()
	{
		this.Clear();
		this.whereBuilder.Append(field).Append(" >= ").Append(low);
		this.whereBuilder.Append(" AND ");
		this.whereBuilder.Append(field).Append(" <= ").Append(high);
		for (int i = 0; i < fields.Length; i++)
		{
			this.whereBuilder.Append(" AND ");
			global::DataFactory.AppendWhere(this.whereBuilder, fields[i], ids[i]);
		}
		global::DataFactory.AppendSortAsc(this.sortBuilder, field);
		return this.InitDBObjects<T>();
	}

	private T InitDBObject<T>() where T : global::DataCore, new()
	{
		global::System.Collections.Generic.List<T> list = global::PandoraSingleton<global::DBConnector>.Instance.Read<T>(this.GetTableName(typeof(T)), "*", this.whereBuilder, this.sortBuilder, 1);
		if (list.Count == 0)
		{
			return (T)((object)null);
		}
		return list[0];
	}

	private global::System.Collections.Generic.List<T> InitDBObjects<T>() where T : global::DataCore, new()
	{
		return global::PandoraSingleton<global::DBConnector>.Instance.Read<T>(this.GetTableName(typeof(T)), "*", this.whereBuilder, this.sortBuilder, 0);
	}

	private static void AppendWhere(global::System.Text.StringBuilder stringBuilder, string field, string id)
	{
		stringBuilder.Append(field).Append('=').Append('"').Append(id).Append('"');
	}

	private static void AppendSortAsc(global::System.Text.StringBuilder stringBuilder, string field)
	{
		stringBuilder.Append(field).Append(' ').Append("ASC");
	}

	private static void AppendSortDesc(global::System.Text.StringBuilder stringBuilder, string field)
	{
		stringBuilder.Append(field).Append(' ').Append("DESC");
	}

	private static void AppendLike(global::System.Text.StringBuilder stringBuilder, string field, string like)
	{
		stringBuilder.Append(field).Append(" LIKE \"%").Append(like).Append("%\"");
	}

	private const string WHERE_FORMAT = "{0} = \"{1}\"";

	private const string LIKE_STATEMENT = "{0} LIKE \"%{1}%\"";

	private const string SORT_ASC = "ASC";

	private const string SORT_DESC = "DESC";

	private const string AND_STATEMENT = " AND ";

	private const char SPACE = ' ';

	private const char QUOTE = '"';

	private const char EQUAL = '=';

	private global::System.Collections.Generic.Dictionary<global::System.Type, string> cachedTypeName;

	private readonly global::System.Text.StringBuilder whereBuilder = new global::System.Text.StringBuilder(1024);

	private readonly global::System.Text.StringBuilder sortBuilder = new global::System.Text.StringBuilder(1024);
}
