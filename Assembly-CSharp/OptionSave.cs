using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OptionSave : global::IThoth
{
	public OptionSave()
	{
		bool useLowSettings = global::Pandora.useLowSettings;
		this.fullScreen = true;
		this.resolution = global::UnityEngine.Screen.resolutions[global::UnityEngine.Screen.resolutions.Length - 1];
		this.vsync = (global::UnityEngine.QualitySettings.vSyncCount != 0 && !useLowSettings);
		this.textureQuality = ((!useLowSettings) ? 3 : 0);
		this.shadowsQuality = ((!useLowSettings) ? 4 : 0);
		this.shadowCascades = ((!useLowSettings) ? 2 : 0);
		this.graphicsDof = !useLowSettings;
		this.graphicsSsao = !useLowSettings;
		this.graphicsSmaa = ((!useLowSettings) ? 4 : 0);
		this.graphicsBrightness = 0.5f;
		this.graphicsBloom = !useLowSettings;
		this.graphicsGuiScale = 1f;
		this.masterVolume = 1f;
		this.fxVolume = 1f;
		this.musicVolume = 0.45f;
		this.voiceVolume = 1f;
		this.ambientVolume = 0.75f;
		this.language = 0;
		this.gamepadEnabled = true;
		this.cameraXInverted = false;
		this.cameraYInverted = false;
		this.leftHandedMouse = false;
		this.leftHandedController = false;
		this.mouseSensitivity = 0.25f;
		this.joystickSensitivity = 0.25f;
		this.tacticalViewHelpersEnabled = true;
		this.wagonBeaconsEnabled = true;
		this.autoExitTacticalEnabled = true;
		this.displayFullUI = true;
		this.fastForwarded = false;
		this.skipTuto = false;
	}

	int global::IThoth.GetVersion()
	{
		return 56;
	}

	void global::IThoth.Write(global::System.IO.BinaryWriter writer)
	{
		global::Thoth.Write(writer, ((global::IThoth)this).GetVersion());
		int crc = this.GetCRC(false);
		global::Thoth.Write(writer, crc);
		global::Thoth.Write(writer, this.fullScreen);
		global::Thoth.Write(writer, this.resolution.height);
		global::Thoth.Write(writer, this.resolution.width);
		global::Thoth.Write(writer, this.masterVolume);
		global::Thoth.Write(writer, this.fxVolume);
		global::Thoth.Write(writer, this.musicVolume);
		global::Thoth.Write(writer, this.voiceVolume);
		global::Thoth.Write(writer, this.ambientVolume);
		global::Thoth.Write(writer, this.gamepadEnabled);
		global::Thoth.Write(writer, this.cameraXInverted);
		global::Thoth.Write(writer, this.cameraYInverted);
		global::Thoth.Write(writer, this.leftHandedMouse);
		global::Thoth.Write(writer, this.leftHandedController);
		global::Thoth.Write(writer, this.tacticalViewHelpersEnabled);
		global::Thoth.Write(writer, this.wagonBeaconsEnabled);
		global::Thoth.Write(writer, this.autoExitTacticalEnabled);
		this.SaveInputMaps(writer, this.keyboardMappingData);
		this.SaveInputMaps(writer, this.joystickMappingData);
		this.SaveInputMaps(writer, this.mouseMappingData);
		global::Thoth.Write(writer, this.mouseSensitivity);
		global::Thoth.Write(writer, this.joystickSensitivity);
		global::Thoth.Write(writer, this.language);
		global::Thoth.Write(writer, this.vsync);
		global::Thoth.Write(writer, this.textureQuality);
		global::Thoth.Write(writer, this.shadowsQuality);
		global::Thoth.Write(writer, this.shadowCascades);
		global::Thoth.Write(writer, this.graphicsDof);
		global::Thoth.Write(writer, this.graphicsSsao);
		global::Thoth.Write(writer, this.graphicsSmaa);
		global::Thoth.Write(writer, this.graphicsBrightness);
		global::Thoth.Write(writer, this.graphicsBloom);
		global::Thoth.Write(writer, this.graphicsGuiScale);
		global::Thoth.Write(writer, this.displayFullUI);
		global::Thoth.Write(writer, this.fastForwarded);
		global::Thoth.Write(writer, this.skipTuto);
	}

	void global::IThoth.Read(global::System.IO.BinaryReader reader)
	{
		int num = 0;
		int num2;
		global::Thoth.Read(reader, out num2);
		this.lastVersion = num2;
		if (num2 > 50)
		{
			global::Thoth.Read(reader, out num);
		}
		global::Thoth.Read(reader, out this.fullScreen);
		int num3;
		global::Thoth.Read(reader, out num3);
		this.resolution.height = num3;
		global::Thoth.Read(reader, out num3);
		this.resolution.width = num3;
		if (num2 > 1)
		{
			global::Thoth.Read(reader, out this.masterVolume);
			global::Thoth.Read(reader, out this.fxVolume);
			global::Thoth.Read(reader, out this.musicVolume);
			global::Thoth.Read(reader, out this.voiceVolume);
			if (num2 > 44)
			{
				global::Thoth.Read(reader, out this.ambientVolume);
			}
			if (num2 > 2)
			{
				global::Thoth.Read(reader, out this.gamepadEnabled);
				global::Thoth.Read(reader, out this.cameraXInverted);
				global::Thoth.Read(reader, out this.cameraYInverted);
				if (num2 > 5)
				{
					global::Thoth.Read(reader, out this.leftHandedMouse);
					global::Thoth.Read(reader, out this.leftHandedController);
					if (num2 > 35)
					{
						global::Thoth.Read(reader, out this.tacticalViewHelpersEnabled);
					}
					if (num2 > 38)
					{
						global::Thoth.Read(reader, out this.wagonBeaconsEnabled);
					}
					if (num2 > 40)
					{
						global::Thoth.Read(reader, out this.autoExitTacticalEnabled);
					}
					if (num2 > 6)
					{
						this.keyboardMappingData = this.LoadInputMaps(reader);
						this.joystickMappingData = this.LoadInputMaps(reader);
						this.mouseMappingData = this.LoadInputMaps(reader);
						if (num2 > 7)
						{
							global::Thoth.Read(reader, out this.mouseSensitivity);
							global::Thoth.Read(reader, out this.joystickSensitivity);
							if (num2 > 18)
							{
								global::Thoth.Read(reader, out this.language);
								if (num2 > 34)
								{
									global::Thoth.Read(reader, out this.vsync);
									global::Thoth.Read(reader, out this.textureQuality);
									global::Thoth.Read(reader, out this.shadowsQuality);
									if (num2 > 42)
									{
										global::Thoth.Read(reader, out this.shadowCascades);
									}
									global::Thoth.Read(reader, out this.graphicsDof);
									if (num2 < 43)
									{
										int num4;
										global::Thoth.Read(reader, out num4);
										this.graphicsSsao = (num4 > 0);
									}
									else
									{
										global::Thoth.Read(reader, out this.graphicsSsao);
									}
									global::Thoth.Read(reader, out this.graphicsSmaa);
									if (num2 < 43)
									{
										int num5;
										global::Thoth.Read(reader, out num5);
										this.graphicsBrightness = (float)num5 / 100f;
										int num6;
										global::Thoth.Read(reader, out num6);
										global::Thoth.Read(reader, out num6);
									}
									else
									{
										global::Thoth.Read(reader, out this.graphicsBrightness);
									}
									if (num2 > 42)
									{
										global::Thoth.Read(reader, out this.graphicsBloom);
									}
									if (num2 > 55)
									{
										global::Thoth.Read(reader, out this.graphicsGuiScale);
									}
								}
							}
						}
					}
				}
			}
		}
		if (num2 > 51)
		{
			global::Thoth.Read(reader, out this.displayFullUI);
		}
		if (num2 > 53)
		{
			global::Thoth.Read(reader, out this.fastForwarded);
		}
		if (num2 > 54)
		{
			global::Thoth.Read(reader, out this.skipTuto);
		}
	}

	public int GetCRC(bool read)
	{
		return this.CalculateCRC(read);
	}

	private int CalculateCRC(bool read)
	{
		int num = (!read) ? ((global::IThoth)this).GetVersion() : this.lastVersion;
		int num2 = 0;
		num2 += ((!this.fullScreen) ? 0 : 1);
		num2 += this.resolution.height;
		num2 += this.resolution.width;
		num2 += ((!this.vsync) ? 0 : 1);
		num2 += this.textureQuality;
		num2 += this.shadowsQuality;
		num2 += this.shadowCascades;
		num2 += ((!this.graphicsDof) ? 0 : 1);
		num2 += ((!this.graphicsSsao) ? 0 : 1);
		num2 += this.graphicsSmaa;
		num2 += (int)(this.graphicsBrightness * 10f);
		num2 += ((!this.graphicsBloom) ? 0 : 1);
		num2 += (int)(this.graphicsGuiScale * 10f);
		num2 += (int)(this.masterVolume * 10f);
		num2 += (int)(this.fxVolume * 10f);
		num2 += (int)(this.musicVolume * 10f);
		num2 += (int)(this.voiceVolume * 10f);
		num2 += (int)(this.ambientVolume * 10f);
		num2 += this.language;
		num2 += ((!this.gamepadEnabled) ? 0 : 1);
		num2 += ((!this.cameraXInverted) ? 0 : 1);
		num2 += ((!this.cameraYInverted) ? 0 : 1);
		num2 += ((!this.leftHandedMouse) ? 0 : 1);
		num2 += ((!this.leftHandedController) ? 0 : 1);
		num2 += (int)(this.mouseSensitivity * 10f);
		num2 += (int)(this.joystickSensitivity * 10f);
		num2 += ((!this.tacticalViewHelpersEnabled) ? 0 : 1);
		num2 += ((!this.wagonBeaconsEnabled) ? 0 : 1);
		num2 += ((!this.autoExitTacticalEnabled) ? 0 : 1);
		if (num > 51)
		{
			num2 += ((!this.displayFullUI) ? 0 : 1);
		}
		if (num > 53)
		{
			num2 += ((!this.fastForwarded) ? 0 : 1);
		}
		if (num > 54)
		{
			num2 += ((!this.skipTuto) ? 0 : 1);
		}
		return num2;
	}

	private global::System.Collections.Generic.List<string> LoadInputMaps(global::System.IO.BinaryReader reader)
	{
		int num;
		global::Thoth.Read(reader, out num);
		global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
		for (int i = 0; i < num; i++)
		{
			string item;
			global::Thoth.Read(reader, out item);
			list.Add(item);
		}
		return list;
	}

	private void SaveInputMaps(global::System.IO.BinaryWriter writer, global::System.Collections.Generic.List<string> mappingData)
	{
		if (mappingData != null)
		{
			global::Thoth.Write(writer, mappingData.Count);
			for (int i = 0; i < mappingData.Count; i++)
			{
				global::Thoth.Write(writer, mappingData[i]);
			}
		}
		else
		{
			global::Thoth.Write(writer, 0);
		}
	}

	private int lastVersion;

	public bool fullScreen;

	public global::UnityEngine.Resolution resolution;

	public bool vsync;

	public int textureQuality;

	public int shadowsQuality;

	public int shadowCascades;

	public bool graphicsDof;

	public bool graphicsSsao;

	public int graphicsSmaa;

	public float graphicsBrightness;

	public bool graphicsBloom;

	public float graphicsGuiScale;

	public float masterVolume;

	public float fxVolume;

	public float musicVolume;

	public float voiceVolume;

	public float ambientVolume;

	public int language;

	public bool gamepadEnabled;

	public bool cameraXInverted;

	public bool cameraYInverted;

	public bool leftHandedMouse;

	public bool leftHandedController;

	public float mouseSensitivity;

	public float joystickSensitivity;

	public bool tacticalViewHelpersEnabled;

	public bool wagonBeaconsEnabled;

	public bool autoExitTacticalEnabled;

	public bool displayFullUI;

	public bool fastForwarded;

	public bool skipTuto;

	public global::System.Collections.Generic.List<string> keyboardMappingData;

	public global::System.Collections.Generic.List<string> joystickMappingData;

	public global::System.Collections.Generic.List<string> mouseMappingData;
}
