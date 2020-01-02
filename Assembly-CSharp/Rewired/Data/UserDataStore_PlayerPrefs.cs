using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Data
{
	public class UserDataStore_PlayerPrefs : global::Rewired.Data.UserDataStore
	{
		public override void Save()
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveAll();
		}

		public override void SaveControllerData(int playerId, global::Rewired.ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(playerId, controllerType, controllerId);
		}

		public override void SaveControllerData(global::Rewired.ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		public override void SavePlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SavePlayerDataNow(playerId);
		}

		public override void SaveInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", this);
				return;
			}
			this.SaveInputBehaviorNow(playerId, behaviorId);
		}

		public override void Load()
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadAll();
		}

		public override void LoadControllerData(int playerId, global::Rewired.ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(playerId, controllerType, controllerId);
		}

		public override void LoadControllerData(global::Rewired.ControllerType controllerType, int controllerId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadControllerDataNow(controllerType, controllerId);
		}

		public override void LoadPlayerData(int playerId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadPlayerDataNow(playerId);
		}

		public override void LoadInputBehavior(int playerId, int behaviorId)
		{
			if (!this.isEnabled)
			{
				global::UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", this);
				return;
			}
			this.LoadInputBehaviorNow(playerId, behaviorId);
		}

		protected override void OnInitialize()
		{
			if (this.loadDataOnStart)
			{
				this.Load();
			}
		}

		protected override void OnControllerConnected(global::Rewired.ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == global::Rewired.ControllerType.Joystick)
			{
				this.LoadJoystickData(args.controllerId);
			}
		}

		protected override void OnControllerPreDiscconnect(global::Rewired.ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
			if (args.controllerType == global::Rewired.ControllerType.Joystick)
			{
				this.SaveJoystickData(args.controllerId);
			}
		}

		protected override void OnControllerDisconnected(global::Rewired.ControllerStatusChangedEventArgs args)
		{
			if (!this.isEnabled)
			{
				return;
			}
		}

		private void LoadAll()
		{
			global::System.Collections.Generic.IList<global::Rewired.Player> allPlayers = global::Rewired.ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.LoadPlayerDataNow(allPlayers[i]);
			}
			this.LoadAllJoystickCalibrationData();
		}

		private void LoadPlayerDataNow(int playerId)
		{
			this.LoadPlayerDataNow(global::Rewired.ReInput.players.GetPlayer(playerId));
		}

		private void LoadPlayerDataNow(global::Rewired.Player player)
		{
			if (player == null)
			{
				return;
			}
			this.LoadInputBehaviors(player.id);
			this.LoadControllerMaps(player.id, global::Rewired.ControllerType.Keyboard, 0);
			this.LoadControllerMaps(player.id, global::Rewired.ControllerType.Mouse, 0);
			foreach (global::Rewired.Joystick joystick in player.controllers.Joysticks)
			{
				this.LoadControllerMaps(player.id, global::Rewired.ControllerType.Joystick, joystick.id);
			}
		}

		private void LoadAllJoystickCalibrationData()
		{
			global::System.Collections.Generic.IList<global::Rewired.Joystick> joysticks = global::Rewired.ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.LoadJoystickCalibrationData(joysticks[i]);
			}
		}

		private void LoadJoystickCalibrationData(global::Rewired.Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			joystick.ImportCalibrationMapFromXmlString(this.GetJoystickCalibrationMapXml(joystick));
		}

		private void LoadJoystickCalibrationData(int joystickId)
		{
			this.LoadJoystickCalibrationData(global::Rewired.ReInput.controllers.GetJoystick(joystickId));
		}

		private void LoadJoystickData(int joystickId)
		{
			global::System.Collections.Generic.IList<global::Rewired.Player> allPlayers = global::Rewired.ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				global::Rewired.Player player = allPlayers[i];
				if (player.controllers.ContainsController(global::Rewired.ControllerType.Joystick, joystickId))
				{
					this.LoadControllerMaps(player.id, global::Rewired.ControllerType.Joystick, joystickId);
				}
			}
			this.LoadJoystickCalibrationData(joystickId);
		}

		private void LoadControllerDataNow(int playerId, global::Rewired.ControllerType controllerType, int controllerId)
		{
			this.LoadControllerMaps(playerId, controllerType, controllerId);
			this.LoadControllerDataNow(controllerType, controllerId);
		}

		private void LoadControllerDataNow(global::Rewired.ControllerType controllerType, int controllerId)
		{
			if (controllerType == global::Rewired.ControllerType.Joystick)
			{
				this.LoadJoystickCalibrationData(controllerId);
			}
		}

		private void LoadControllerMaps(int playerId, global::Rewired.ControllerType controllerType, int controllerId)
		{
			global::Rewired.Player player = global::Rewired.ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			global::Rewired.Controller controller = global::Rewired.ReInput.controllers.GetController(controllerType, controllerId);
			if (controller == null)
			{
				return;
			}
			global::System.Collections.Generic.List<string> allControllerMapsXml = this.GetAllControllerMapsXml(player, true, controllerType, controller);
			if (allControllerMapsXml.Count == 0)
			{
				return;
			}
			player.controllers.maps.AddMapsFromXml(controllerType, controllerId, allControllerMapsXml);
		}

		private void LoadInputBehaviors(int playerId)
		{
			global::Rewired.Player player = global::Rewired.ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Rewired.InputBehavior> inputBehaviors = global::Rewired.ReInput.mapping.GetInputBehaviors(player.id);
			for (int i = 0; i < inputBehaviors.Count; i++)
			{
				this.LoadInputBehaviorNow(player, inputBehaviors[i]);
			}
		}

		private void LoadInputBehaviorNow(int playerId, int behaviorId)
		{
			global::Rewired.Player player = global::Rewired.ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			global::Rewired.InputBehavior inputBehavior = global::Rewired.ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			this.LoadInputBehaviorNow(player, inputBehavior);
		}

		private void LoadInputBehaviorNow(global::Rewired.Player player, global::Rewired.InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			string inputBehaviorXml = this.GetInputBehaviorXml(player, inputBehavior.id);
			if (inputBehaviorXml == null || inputBehaviorXml == string.Empty)
			{
				return;
			}
			inputBehavior.ImportXmlString(inputBehaviorXml);
		}

		private void SaveAll()
		{
			global::System.Collections.Generic.IList<global::Rewired.Player> allPlayers = global::Rewired.ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				this.SavePlayerDataNow(allPlayers[i]);
			}
			this.SaveAllJoystickCalibrationData();
			global::UnityEngine.PlayerPrefs.Save();
		}

		private void SavePlayerDataNow(int playerId)
		{
			this.SavePlayerDataNow(global::Rewired.ReInput.players.GetPlayer(playerId));
		}

		private void SavePlayerDataNow(global::Rewired.Player player)
		{
			if (player == null)
			{
				return;
			}
			global::Rewired.PlayerSaveData saveData = player.GetSaveData(true);
			this.SaveInputBehaviors(player, saveData);
			this.SaveControllerMaps(player, saveData);
		}

		private void SaveAllJoystickCalibrationData()
		{
			global::System.Collections.Generic.IList<global::Rewired.Joystick> joysticks = global::Rewired.ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				this.SaveJoystickCalibrationData(joysticks[i]);
			}
		}

		private void SaveJoystickCalibrationData(int joystickId)
		{
			this.SaveJoystickCalibrationData(global::Rewired.ReInput.controllers.GetJoystick(joystickId));
		}

		private void SaveJoystickCalibrationData(global::Rewired.Joystick joystick)
		{
			if (joystick == null)
			{
				return;
			}
			global::Rewired.JoystickCalibrationMapSaveData calibrationMapSaveData = joystick.GetCalibrationMapSaveData();
			string joystickCalibrationMapPlayerPrefsKey = this.GetJoystickCalibrationMapPlayerPrefsKey(calibrationMapSaveData);
			global::UnityEngine.PlayerPrefs.SetString(joystickCalibrationMapPlayerPrefsKey, calibrationMapSaveData.map.ToXmlString());
		}

		private void SaveJoystickData(int joystickId)
		{
			global::System.Collections.Generic.IList<global::Rewired.Player> allPlayers = global::Rewired.ReInput.players.AllPlayers;
			for (int i = 0; i < allPlayers.Count; i++)
			{
				global::Rewired.Player player = allPlayers[i];
				if (player.controllers.ContainsController(global::Rewired.ControllerType.Joystick, joystickId))
				{
					this.SaveControllerMaps(player.id, global::Rewired.ControllerType.Joystick, joystickId);
				}
			}
			this.SaveJoystickCalibrationData(joystickId);
		}

		private void SaveControllerDataNow(int playerId, global::Rewired.ControllerType controllerType, int controllerId)
		{
			this.SaveControllerMaps(playerId, controllerType, controllerId);
			this.SaveControllerDataNow(controllerType, controllerId);
		}

		private void SaveControllerDataNow(global::Rewired.ControllerType controllerType, int controllerId)
		{
			if (controllerType == global::Rewired.ControllerType.Joystick)
			{
				this.SaveJoystickCalibrationData(controllerId);
			}
		}

		private void SaveControllerMaps(global::Rewired.Player player, global::Rewired.PlayerSaveData playerSaveData)
		{
			foreach (global::Rewired.ControllerMapSaveData controllerMapSaveData in playerSaveData.AllControllerMapSaveData)
			{
				string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, controllerMapSaveData);
				global::UnityEngine.PlayerPrefs.SetString(controllerMapPlayerPrefsKey, controllerMapSaveData.map.ToXmlString());
			}
		}

		private void SaveControllerMaps(int playerId, global::Rewired.ControllerType controllerType, int controllerId)
		{
			global::Rewired.Player player = global::Rewired.ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(controllerType, controllerId))
			{
				return;
			}
			global::Rewired.ControllerMapSaveData[] mapSaveData = player.controllers.maps.GetMapSaveData(controllerType, controllerId, true);
			if (mapSaveData == null)
			{
				return;
			}
			for (int i = 0; i < mapSaveData.Length; i++)
			{
				string controllerMapPlayerPrefsKey = this.GetControllerMapPlayerPrefsKey(player, mapSaveData[i]);
				global::UnityEngine.PlayerPrefs.SetString(controllerMapPlayerPrefsKey, mapSaveData[i].map.ToXmlString());
			}
		}

		private void SaveInputBehaviors(global::Rewired.Player player, global::Rewired.PlayerSaveData playerSaveData)
		{
			if (player == null)
			{
				return;
			}
			global::Rewired.InputBehavior[] inputBehaviors = playerSaveData.inputBehaviors;
			for (int i = 0; i < inputBehaviors.Length; i++)
			{
				this.SaveInputBehaviorNow(player, inputBehaviors[i]);
			}
		}

		private void SaveInputBehaviorNow(int playerId, int behaviorId)
		{
			global::Rewired.Player player = global::Rewired.ReInput.players.GetPlayer(playerId);
			if (player == null)
			{
				return;
			}
			global::Rewired.InputBehavior inputBehavior = global::Rewired.ReInput.mapping.GetInputBehavior(playerId, behaviorId);
			if (inputBehavior == null)
			{
				return;
			}
			this.SaveInputBehaviorNow(player, inputBehavior);
		}

		private void SaveInputBehaviorNow(global::Rewired.Player player, global::Rewired.InputBehavior inputBehavior)
		{
			if (player == null || inputBehavior == null)
			{
				return;
			}
			string inputBehaviorPlayerPrefsKey = this.GetInputBehaviorPlayerPrefsKey(player, inputBehavior);
			global::UnityEngine.PlayerPrefs.SetString(inputBehaviorPlayerPrefsKey, inputBehavior.ToXmlString());
		}

		private string GetBasePlayerPrefsKey(global::Rewired.Player player)
		{
			string str = this.playerPrefsKeyPrefix;
			return str + "|playerName=" + player.name;
		}

		private string GetControllerMapPlayerPrefsKey(global::Rewired.Player player, global::Rewired.ControllerMapSaveData saveData)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap";
			text = text + "|controllerMapType=" + saveData.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				saveData.map.categoryId,
				"|layoutId=",
				saveData.map.layoutId
			});
			text = text + "|hardwareIdentifier=" + saveData.controllerHardwareIdentifier;
			if (saveData.mapType == typeof(global::Rewired.JoystickMap))
			{
				text = text + "|hardwareGuid=" + ((global::Rewired.JoystickMapSaveData)saveData).joystickHardwareTypeGuid.ToString();
			}
			return text;
		}

		private string GetControllerMapXml(global::Rewired.Player player, global::Rewired.ControllerType controllerType, int categoryId, int layoutId, global::Rewired.Controller controller)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=ControllerMap";
			text = text + "|controllerMapType=" + controller.mapTypeString;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"|categoryId=",
				categoryId,
				"|layoutId=",
				layoutId
			});
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier;
			if (controllerType == global::Rewired.ControllerType.Joystick)
			{
				global::Rewired.Joystick joystick = (global::Rewired.Joystick)controller;
				text = text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
			}
			if (!global::UnityEngine.PlayerPrefs.HasKey(text))
			{
				return string.Empty;
			}
			return global::UnityEngine.PlayerPrefs.GetString(text);
		}

		private global::System.Collections.Generic.List<string> GetAllControllerMapsXml(global::Rewired.Player player, bool userAssignableMapsOnly, global::Rewired.ControllerType controllerType, global::Rewired.Controller controller)
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			global::System.Collections.Generic.IList<global::Rewired.InputMapCategory> mapCategories = global::Rewired.ReInput.mapping.MapCategories;
			for (int i = 0; i < mapCategories.Count; i++)
			{
				global::Rewired.InputMapCategory inputMapCategory = mapCategories[i];
				if (!userAssignableMapsOnly || inputMapCategory.userAssignable)
				{
					global::System.Collections.Generic.IList<global::Rewired.InputLayout> list2 = global::Rewired.ReInput.mapping.MapLayouts(controllerType);
					for (int j = 0; j < list2.Count; j++)
					{
						global::Rewired.InputLayout inputLayout = list2[j];
						string controllerMapXml = this.GetControllerMapXml(player, controllerType, inputMapCategory.id, inputLayout.id, controller);
						if (!(controllerMapXml == string.Empty))
						{
							list.Add(controllerMapXml);
						}
					}
				}
			}
			return list;
		}

		private string GetJoystickCalibrationMapPlayerPrefsKey(global::Rewired.JoystickCalibrationMapSaveData saveData)
		{
			string str = this.playerPrefsKeyPrefix;
			str += "|dataType=CalibrationMap";
			str = str + "|controllerType=" + saveData.controllerType.ToString();
			str = str + "|hardwareIdentifier=" + saveData.hardwareIdentifier;
			return str + "|hardwareGuid=" + saveData.joystickHardwareTypeGuid.ToString();
		}

		private string GetJoystickCalibrationMapXml(global::Rewired.Joystick joystick)
		{
			string text = this.playerPrefsKeyPrefix;
			text += "|dataType=CalibrationMap";
			text = text + "|controllerType=" + joystick.type.ToString();
			text = text + "|hardwareIdentifier=" + joystick.hardwareIdentifier;
			text = text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString();
			if (!global::UnityEngine.PlayerPrefs.HasKey(text))
			{
				return string.Empty;
			}
			return global::UnityEngine.PlayerPrefs.GetString(text);
		}

		private string GetInputBehaviorPlayerPrefsKey(global::Rewired.Player player, global::Rewired.InputBehavior saveData)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=InputBehavior";
			return text + "|id=" + saveData.id;
		}

		private string GetInputBehaviorXml(global::Rewired.Player player, int id)
		{
			string text = this.GetBasePlayerPrefsKey(player);
			text += "|dataType=InputBehavior";
			text = text + "|id=" + id;
			if (!global::UnityEngine.PlayerPrefs.HasKey(text))
			{
				return string.Empty;
			}
			return global::UnityEngine.PlayerPrefs.GetString(text);
		}

		[global::UnityEngine.SerializeField]
		private bool isEnabled = true;

		[global::UnityEngine.SerializeField]
		private bool loadDataOnStart = true;

		[global::UnityEngine.SerializeField]
		private string playerPrefsKeyPrefix = "RewiredSaveData";
	}
}
