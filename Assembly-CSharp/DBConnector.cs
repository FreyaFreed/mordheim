using System;
using System.Collections.Generic;
using System.Text;
using Mono.Data.Sqlite;
using UnityEngine;

public class DBConnector : global::PandoraSingleton<global::DBConnector>
{
	public global::Mono.Data.Sqlite.SqliteConnection Connection { get; private set; }

	private void Destroy()
	{
		if (this.Connection != null)
		{
			this.Connection.Close();
		}
	}

	public void Connect(string path)
	{
		this.databasePath = path;
		this.CreateConnection();
	}

	private void CreateConnection()
	{
		if (this.Connection != null)
		{
			this.Connection.Close();
		}
		string text = string.Format("URI=file:{0}{1}", global::UnityEngine.Application.streamingAssetsPath, this.databasePath);
		try
		{
			global::PandoraDebug.LogInfo("Trying to connec to : " + text, "DB Connector", null);
			this.Connection = new global::Mono.Data.Sqlite.SqliteConnection(text);
			global::UnityEngine.TextAsset textAsset = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("database/connection1");
			global::UnityEngine.TextAsset textAsset2 = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("database/connection2");
			global::UnityEngine.TextAsset textAsset3 = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("database/connection3");
			if (textAsset != null && textAsset2 != null && textAsset3 != null)
			{
				this.Connection.SetPassword(textAsset.text.Trim() + textAsset2.text.Trim() + textAsset3.text.Trim());
			}
			this.Connection.Open();
		}
		catch (global::System.Exception ex)
		{
			global::PandoraDebug.LogWarning(ex, "uncategorised", null);
			global::UnityEngine.Debug.Log(ex.Message + " [" + text + "]");
			global::UnityEngine.Debug.Log(ex.StackTrace);
			return;
		}
		global::PandoraDebug.LogInfo("Connection successful", "DB Connector", null);
	}

	public void CloseConnection()
	{
		this.Connection.Close();
		this.Connection = null;
	}

	public global::System.Collections.Generic.List<global::System.Collections.Generic.List<object>> GetTablesName()
	{
		string request = "SELECT name  FROM sqlite_master WHERE type='table' ORDER BY name";
		return this.ExecuteRequest(request);
	}

	public global::System.Collections.Generic.List<string> GetTableFieldsName(string table)
	{
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		string commandText = "SELECT * FROM " + table + " LIMIT 1";
		using (global::Mono.Data.Sqlite.SqliteCommand sqliteCommand = new global::Mono.Data.Sqlite.SqliteCommand(commandText, this.Connection))
		{
			using (global::Mono.Data.Sqlite.SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
			{
				while (sqliteDataReader.Read())
				{
					for (int i = 0; i < sqliteDataReader.FieldCount; i++)
					{
						list.Add(sqliteDataReader.GetName(i));
					}
				}
				sqliteDataReader.Close();
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::System.Collections.Generic.List<string>> GetTableFieldsTypeName(string table)
	{
		global::System.Collections.Generic.List<global::System.Collections.Generic.List<string>> list = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<string>>();
		string commandText = "SELECT * FROM " + table + " LIMIT 1";
		using (global::Mono.Data.Sqlite.SqliteCommand sqliteCommand = new global::Mono.Data.Sqlite.SqliteCommand(commandText, global::PandoraSingleton<global::DBConnector>.Instance.Connection))
		{
			using (global::Mono.Data.Sqlite.SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
			{
				while (sqliteDataReader.Read())
				{
					for (int i = 0; i < sqliteDataReader.FieldCount; i++)
					{
						list.Add(new global::System.Collections.Generic.List<string>
						{
							sqliteDataReader.GetFieldType(i).Name,
							sqliteDataReader.GetName(i)
						});
					}
				}
			}
		}
		return list;
	}

	public global::System.Collections.Generic.List<global::System.Collections.Generic.List<object>> GetTableIdName(string table)
	{
		string request = "SELECT id, name FROM " + table + " ORDER BY id;";
		return this.ExecuteRequest(request);
	}

	public global::System.Collections.Generic.List<T> Read<T>(string table, string fields, global::System.Text.StringBuilder condition, global::System.Text.StringBuilder sort, int limit = 0) where T : global::DataCore, new()
	{
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		stringBuilder.Append("SELECT ").Append(fields);
		stringBuilder.Append(" FROM ").Append(table);
		if (condition.Length > 0)
		{
			stringBuilder.Append(" WHERE ");
			for (int i = 0; i < condition.Length; i++)
			{
				stringBuilder.Append(condition[i]);
			}
		}
		if (sort.Length > 0)
		{
			stringBuilder.Append(" ORDER BY ");
			for (int j = 0; j < sort.Length; j++)
			{
				stringBuilder.Append(sort[j]);
			}
		}
		if (limit > 0)
		{
			stringBuilder.Append(" LIMIT ").Append(limit.ToConstantString());
		}
		uint key = global::FNV1a.ComputeHash(stringBuilder);
		object obj;
		if (!this.cachedListResults.TryGetValue(key, out obj))
		{
			global::System.Collections.Generic.List<T> list = this.ExecuteRequest<T>(stringBuilder.ToString());
			if (list.Count > 0)
			{
				this.cachedListResults[key] = list;
			}
			else
			{
				this.cachedListResults[key] = list;
			}
			return list;
		}
		return (global::System.Collections.Generic.List<T>)obj;
	}

	private global::System.Collections.Generic.List<T> ExecuteRequest<T>(string request) where T : global::DataCore, new()
	{
		global::System.Collections.Generic.List<T> list = new global::System.Collections.Generic.List<T>();
		try
		{
			using (global::Mono.Data.Sqlite.SqliteCommand sqliteCommand = new global::Mono.Data.Sqlite.SqliteCommand(request, this.Connection))
			{
				using (global::Mono.Data.Sqlite.SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						T item = global::System.Activator.CreateInstance<T>();
						item.Populate(sqliteDataReader);
						list.Add(item);
					}
					sqliteDataReader.Close();
				}
				sqliteCommand.Dispose();
			}
		}
		catch (global::System.Exception ex)
		{
			global::PandoraDebug.LogException(ex, false);
			global::UnityEngine.Debug.Log(ex.Message);
			return null;
		}
		return list;
	}

	private global::System.Collections.Generic.List<global::System.Collections.Generic.List<object>> ExecuteRequest(string request)
	{
		global::System.Collections.Generic.List<global::System.Collections.Generic.List<object>> list = new global::System.Collections.Generic.List<global::System.Collections.Generic.List<object>>();
		try
		{
			using (global::Mono.Data.Sqlite.SqliteCommand sqliteCommand = new global::Mono.Data.Sqlite.SqliteCommand(request, this.Connection))
			{
				using (global::Mono.Data.Sqlite.SqliteDataReader sqliteDataReader = sqliteCommand.ExecuteReader())
				{
					while (sqliteDataReader.Read())
					{
						global::System.Collections.Generic.List<object> list2 = new global::System.Collections.Generic.List<object>();
						for (int i = 0; i < sqliteDataReader.FieldCount; i++)
						{
							list2.Add(sqliteDataReader.GetValue(i));
						}
						list.Add(list2);
					}
					sqliteDataReader.Close();
				}
				sqliteCommand.Dispose();
			}
		}
		catch (global::System.Exception ex)
		{
			global::PandoraDebug.LogException(ex, false);
			global::UnityEngine.Debug.Log(ex.Message);
			return null;
		}
		return list;
	}

	public static string TableNameToClassName(string tableName)
	{
		return global::PandoraUtils.UnderToCamel(tableName, true) + "Data";
	}

	public static string ClassNameToTableName(string className)
	{
		return global::PandoraUtils.CamelToUnder(className.Replace("Data", string.Empty));
	}

	private const string SELECT = "SELECT ";

	private const string FROM = " FROM ";

	private const string WHERE = " WHERE ";

	private const string ORDER_BY = " ORDER BY ";

	private const string LIMIT = " LIMIT ";

	public string databasePath;

	private global::System.Collections.Generic.Dictionary<uint, object> cachedListResults = new global::System.Collections.Generic.Dictionary<uint, object>();

	private global::System.Collections.Generic.List<object> emptyList = new global::System.Collections.Generic.List<object>(0);
}
