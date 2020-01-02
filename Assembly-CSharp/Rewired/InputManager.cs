using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired
{
	[global::System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Never)]
	public sealed class InputManager : global::Rewired.InputManager_Base
	{
		protected override void DetectPlatform()
		{
			this.editorPlatform = global::Rewired.Platforms.EditorPlatform.None;
			this.platform = global::Rewired.Platforms.Platform.Unknown;
			this.webplayerPlatform = global::Rewired.Platforms.WebplayerPlatform.None;
			this.isEditor = false;
			string text = global::UnityEngine.SystemInfo.deviceName ?? string.Empty;
			string text2 = global::UnityEngine.SystemInfo.deviceModel ?? string.Empty;
			this.platform = global::Rewired.Platforms.Platform.Windows;
		}

		protected override void CheckRecompile()
		{
		}

		protected override string GetFocusedEditorWindowTitle()
		{
			return string.Empty;
		}

		protected override global::Rewired.Utils.Interfaces.IExternalTools GetExternalTools()
		{
			return new global::Rewired.Utils.ExternalTools();
		}

		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return global::System.Text.RegularExpressions.Regex.IsMatch(deviceName, searchPattern, global::System.Text.RegularExpressions.RegexOptions.IgnoreCase) || global::System.Text.RegularExpressions.Regex.IsMatch(deviceModel, searchPattern, global::System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		}
	}
}
