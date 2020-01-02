using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Pathfinding.Serialization.JsonFx;
using Rewired;
using UnityEngine;

public class LocalizationManager : global::PandoraSingleton<global::LocalizationManager>
{
	public global::SupportedLanguage CurrentLanguageId { get; private set; }

	private void Awake()
	{
		this.Init();
	}

	public void Init()
	{
		if (this.defaultFile)
		{
			this.CurrentLanguageId = this.defaultLanguage;
			this.ParseFile(this.defaultFile);
		}
	}

	public void SetLanguage(global::SupportedLanguage languageId, bool force = false)
	{
		if (this.CurrentLanguageId != languageId || force)
		{
			this.CurrentLanguageId = languageId;
			string path = string.Empty;
			switch (languageId)
			{
			case global::SupportedLanguage.enUS:
				path = "loc/loc_en";
				break;
			case global::SupportedLanguage.frFR:
				path = "loc/loc_fr";
				break;
			case global::SupportedLanguage.deDE:
				path = "loc/loc_de";
				break;
			case global::SupportedLanguage.esES:
				path = "loc/loc_es";
				break;
			case global::SupportedLanguage.itIT:
				path = "loc/loc_it";
				break;
			case global::SupportedLanguage.plPL:
				path = "loc/loc_pl";
				break;
			case global::SupportedLanguage.ruRU:
				path = "loc/loc_ru";
				break;
			}
			this.ParseFile(global::UnityEngine.Resources.Load(path) as global::UnityEngine.TextAsset);
		}
	}

	public bool HasStringId(string key)
	{
		uint key2 = global::FNV1a.ComputeHash(key);
		return this.language.ContainsKey(key2);
	}

	public string BuildStringAndLocalize(string str1, string str2, string str3 = null, string str4 = null)
	{
		global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
		if (str1 != null)
		{
			stringBuilder.Append(str1);
		}
		if (str2 != null)
		{
			stringBuilder.Append(str2);
		}
		if (str3 != null)
		{
			stringBuilder.Append(str3);
		}
		if (str4 != null)
		{
			stringBuilder.Append(str4);
		}
		return this.GetStringById(stringBuilder, this.emptyArray);
	}

	public string GetStringById(global::System.Text.StringBuilder key, params string[] parameters)
	{
		return this.GetStringById2(key, parameters);
	}

	public string GetStringById(global::System.Text.StringBuilder key)
	{
		return this.GetStringById2(key, this.emptyArray);
	}

	private string GetStringById2(global::System.Text.StringBuilder key, string[] parameters)
	{
		uint key2 = global::FNV1a.ComputeHash(key);
		if (this.language.ContainsKey(key2))
		{
			try
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (parameters[i][0] == '#')
					{
						parameters[i] = this.GetStringById(parameters[i].Replace("#", string.Empty));
					}
				}
				if (parameters.Length > 0)
				{
					global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
					stringBuilder.AppendFormat(this.language[key2], parameters);
					return stringBuilder.ToString();
				}
				return this.language[key2];
			}
			catch
			{
			}
		}
		else if (key.Length > 0 && key[0] == '#')
		{
			return global::System.Text.RegularExpressions.Regex.Replace(key.ToString(), "(?<![=])#(\\w+)", new global::System.Text.RegularExpressions.MatchEvaluator(this.ConvertMatchToLocalization));
		}
		return "++" + key;
	}

	public string GetStringById(string key, params string[] parameters)
	{
		return this.GetStringById2(key, parameters);
	}

	public string GetStringById(string key)
	{
		return this.GetStringById2(key, this.emptyArray);
	}

	private string GetStringById2(string key, string[] parameters)
	{
		uint key2 = global::FNV1a.ComputeHash(key);
		if (this.language.ContainsKey(key2))
		{
			try
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (parameters[i][0] == '#')
					{
						parameters[i] = this.GetStringById(parameters[i].Replace("#", string.Empty));
					}
				}
				if (parameters.Length > 0)
				{
					global::System.Text.StringBuilder stringBuilder = global::PandoraUtils.StringBuilder;
					stringBuilder.AppendFormat(this.language[key2], parameters);
					return stringBuilder.ToString();
				}
				return this.language[key2];
			}
			catch
			{
			}
		}
		else if (key.StartsWith("#", global::System.StringComparison.OrdinalIgnoreCase))
		{
			return global::System.Text.RegularExpressions.Regex.Replace(key, "(?<![=])#(\\w+)", new global::System.Text.RegularExpressions.MatchEvaluator(this.ConvertMatchToLocalization));
		}
		return "++" + key;
	}

	private string ConvertMatchToLocalization(global::System.Text.RegularExpressions.Match match)
	{
		return this.GetStringById(match.Value.Replace("#", string.Empty));
	}

	public void ParseFile(global::UnityEngine.TextAsset file)
	{
		string text = file.text;
		global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
		dictionary = global::Pathfinding.Serialization.JsonFx.JsonReader.Deserialize<global::System.Collections.Generic.Dictionary<string, string>>(text);
		this.language = new global::System.Collections.Generic.Dictionary<uint, string>();
		foreach (string text2 in dictionary.Keys)
		{
			this.language[global::System.Convert.ToUInt32(text2)] = dictionary[text2];
		}
	}

	public string ReplaceAllActionsWithButtonName(string inputStr)
	{
		string text = inputStr;
		foreach (global::Rewired.InputAction inputAction in global::Rewired.ReInput.mapping.ActionsInCategory("game_input"))
		{
			string[] array = new string[]
			{
				"[" + inputAction.name + "]",
				"[" + inputAction.name + "-]",
				"[" + inputAction.name + "+]"
			};
			global::System.Collections.Generic.List<global::Rewired.ActionElementMap> list;
			if (global::PandoraSingleton<global::PandoraInput>.Instance.lastInputMode == global::PandoraInput.InputMode.JOYSTICK)
			{
				list = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.ElementMapsWithAction(global::Rewired.ControllerType.Joystick, inputAction.name, true).ToDynList<global::Rewired.ActionElementMap>();
			}
			else
			{
				list = global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.ButtonMapsWithAction(global::Rewired.ControllerType.Keyboard, inputAction.name, true).ToDynList<global::Rewired.ActionElementMap>();
				list.AddRange(global::PandoraSingleton<global::PandoraInput>.Instance.player.controllers.maps.ButtonMapsWithAction(global::Rewired.ControllerType.Mouse, inputAction.name, true).ToDynList<global::Rewired.ActionElementMap>());
			}
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = string.Empty;
				global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
				for (int j = 0; j < list.Count; j++)
				{
					global::Rewired.ActionElementMap actionElementMap = list[j];
					string controllerKeyString = this.GetControllerKeyString(actionElementMap.elementIdentifierName);
					if (!list2.Contains(controllerKeyString))
					{
						list2.Add(controllerKeyString);
						if (actionElementMap.elementType == global::Rewired.ControllerElementType.Axis && actionElementMap.axisRange == global::Rewired.AxisRange.Full)
						{
							if (i == 0)
							{
								text2 += controllerKeyString;
							}
							else if (i == 1)
							{
								text2 += this.GetControllerKeyString(actionElementMap.elementIdentifierName + "_-");
							}
							else if (i == 2)
							{
								text2 += this.GetControllerKeyString(actionElementMap.elementIdentifierName + "_+");
							}
						}
						else if (actionElementMap.axisContribution == global::Rewired.Pole.Positive && (i == 2 || i == 0))
						{
							text2 = text2 + controllerKeyString + " / ";
						}
						else if (actionElementMap.axisContribution == global::Rewired.Pole.Negative && i == 1)
						{
							text2 = text2 + controllerKeyString + " / ";
						}
					}
				}
				text = text.Replace(array[i], text2.TrimEnd(new char[]
				{
					' ',
					'/'
				}));
			}
		}
		return text;
	}

	public string GetControllerKeyString(string elementIdentifierName)
	{
		elementIdentifierName = "key_" + elementIdentifierName.ToLowerInvariant().Replace(" ", "_");
		if (elementIdentifierName.Equals("key_mouse_horizontal") || elementIdentifierName.Equals("key_mouse_vertical"))
		{
			elementIdentifierName = "key_mouse_move";
		}
		return global::PandoraSingleton<global::LocalizationManager>.Instance.GetStringById(elementIdentifierName);
	}

	public global::SupportedLanguage defaultLanguage;

	public global::UnityEngine.TextAsset defaultFile;

	private global::System.Collections.Generic.Dictionary<uint, string> language;

	private string[] emptyArray = new string[0];
}
