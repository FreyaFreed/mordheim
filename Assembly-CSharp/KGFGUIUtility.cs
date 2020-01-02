using System;
using System.Collections.Generic;
using UnityEngine;

public class KGFGUIUtility
{
	public static global::UnityEngine.Color itsEditorColorContent
	{
		get
		{
			return new global::UnityEngine.Color(0.1f, 0.1f, 0.1f);
		}
	}

	public static global::UnityEngine.Color itsEditorColorTitle
	{
		get
		{
			return new global::UnityEngine.Color(0.1f, 0.1f, 0.1f);
		}
	}

	public static global::UnityEngine.Color itsEditorDocumentation
	{
		get
		{
			return new global::UnityEngine.Color(0.74f, 0.79f, 0.64f);
		}
	}

	public static global::UnityEngine.Color itsEditorColorDefault
	{
		get
		{
			return new global::UnityEngine.Color(1f, 1f, 1f);
		}
	}

	public static global::UnityEngine.Color itsEditorColorInfo
	{
		get
		{
			return new global::UnityEngine.Color(1f, 1f, 1f);
		}
	}

	public static global::UnityEngine.Color itsEditorColorWarning
	{
		get
		{
			return new global::UnityEngine.Color(1f, 1f, 0f);
		}
	}

	public static global::UnityEngine.Color itsEditorColorError
	{
		get
		{
			return new global::UnityEngine.Color(0.9f, 0.5f, 0.5f);
		}
	}

	public static int GetSkinIndex()
	{
		return global::KGFGUIUtility.itsSkinIndex;
	}

	public static float GetSkinHeight()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return 16f;
		}
		if (global::KGFGUIUtility.itsStyleButton != null && global::KGFGUIUtility.itsSkinIndex < global::KGFGUIUtility.itsStyleButton.Length && global::KGFGUIUtility.itsStyleButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButton[global::KGFGUIUtility.itsSkinIndex].fixedHeight;
		}
		return 16f;
	}

	public static global::UnityEngine.GUISkin GetSkin()
	{
		if (global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex];
		}
		return null;
	}

	public static global::UnityEngine.Texture2D GetLogo()
	{
		if (global::KGFGUIUtility.itsIcon == null)
		{
			global::KGFGUIUtility.itsIcon = (global::UnityEngine.Resources.Load("KGFCore/textures/logo") as global::UnityEngine.Texture2D);
		}
		return global::KGFGUIUtility.itsIcon;
	}

	public static global::UnityEngine.Texture2D GetHelpIcon()
	{
		if (global::KGFGUIUtility.itsIconHelp == null)
		{
			global::KGFGUIUtility.itsIconHelp = (global::UnityEngine.Resources.Load("KGFCore/textures/help") as global::UnityEngine.Texture2D);
		}
		return global::KGFGUIUtility.itsIconHelp;
	}

	public static global::UnityEngine.Texture2D GetKGFCopyright()
	{
		if (global::KGFGUIUtility.itsKGFCopyright == null)
		{
			global::KGFGUIUtility.itsKGFCopyright = (global::UnityEngine.Resources.Load("KGFCore/textures/kgf_copyright_512x256") as global::UnityEngine.Texture2D);
		}
		return global::KGFGUIUtility.itsKGFCopyright;
	}

	public static global::UnityEngine.GUIStyle GetStyleToggl(global::KGFGUIUtility.eStyleToggl theTogglStyle)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.toggle;
		}
		global::KGFGUIUtility.Init();
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglStreched && global::KGFGUIUtility.itsStyleToggleStreched[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleStreched[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglCompact && global::KGFGUIUtility.itsStyleToggleCompact[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleCompact[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglSuperCompact && global::KGFGUIUtility.itsStyleToggleSuperCompact[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleSuperCompact[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglRadioStreched && global::KGFGUIUtility.itsStyleToggleRadioStreched[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleRadioStreched[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglRadioCompact && global::KGFGUIUtility.itsStyleToggleRadioCompact[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleRadioCompact[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglRadioSuperCompact && global::KGFGUIUtility.itsStyleToggleRadioSuperCompact[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleRadioSuperCompact[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglSwitch && global::KGFGUIUtility.itsStyleToggleSwitch[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleSwitch[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglBoolean && global::KGFGUIUtility.itsStyleToggleBoolean[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleBoolean[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglArrow && global::KGFGUIUtility.itsStyleToggleArrow[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleArrow[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theTogglStyle == global::KGFGUIUtility.eStyleToggl.eTogglButton && global::KGFGUIUtility.itsStyleToggleButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggleButton[global::KGFGUIUtility.itsSkinIndex];
		}
		if (global::KGFGUIUtility.itsStyleToggle[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleToggle[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.toggle;
	}

	public static global::UnityEngine.GUIStyle GetStyleTextField(global::KGFGUIUtility.eStyleTextField theStyleTextField)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.textField;
		}
		global::KGFGUIUtility.Init();
		if (theStyleTextField == global::KGFGUIUtility.eStyleTextField.eTextField && global::KGFGUIUtility.itsStyleTextField[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTextField[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleTextField == global::KGFGUIUtility.eStyleTextField.eTextFieldLeft && global::KGFGUIUtility.itsStyleTextFieldLeft[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTextFieldLeft[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleTextField == global::KGFGUIUtility.eStyleTextField.eTextFieldRight && global::KGFGUIUtility.itsStyleTextFieldRight[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTextFieldRight[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.textField;
	}

	public static global::UnityEngine.GUIStyle GetStyleTextArea()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.textArea;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTextArea != null)
		{
			return global::KGFGUIUtility.itsStyleTextArea[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.textArea;
	}

	public static global::UnityEngine.GUIStyle GetStyleHorizontalSlider()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.horizontalSlider;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalSlider[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleHorizontalSlider[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.horizontalSlider;
	}

	public static global::UnityEngine.GUIStyle GetStyleHorizontalSliderThumb()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.horizontalSliderThumb;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalSliderThumb[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleHorizontalSliderThumb[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.horizontalSliderThumb;
	}

	public static global::UnityEngine.GUIStyle GetStyleHorizontalScrollbar()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.horizontalScrollbar;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalScrollbar[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleHorizontalScrollbar[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.horizontalScrollbar;
	}

	public static global::UnityEngine.GUIStyle GetStyleHorizontalScrollbarThumb()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.horizontalScrollbarThumb;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalScrollbarThumb[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleHorizontalScrollbarThumb[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.horizontalScrollbarThumb;
	}

	public static global::UnityEngine.GUIStyle GetStyleHorizontalScrollbarLeftButton()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.horizontalScrollbarLeftButton;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalScrollbarLeftButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleHorizontalScrollbarLeftButton[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.horizontalScrollbarLeftButton;
	}

	public static global::UnityEngine.GUIStyle GetStyleHorizontalScrollbarRightButton()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.horizontalScrollbarRightButton;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalScrollbarRightButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleHorizontalScrollbarRightButton[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.horizontalScrollbarRightButton;
	}

	public static global::UnityEngine.GUIStyle GetStyleVerticalSlider()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.verticalSlider;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalSlider[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleVerticalSlider[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.verticalSlider;
	}

	public static global::UnityEngine.GUIStyle GetStyleVerticalSliderThumb()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.verticalSliderThumb;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalSliderThumb[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleVerticalSliderThumb[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.verticalSliderThumb;
	}

	public static global::UnityEngine.GUIStyle GetStyleVerticalScrollbar()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.verticalScrollbar;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalScrollbar[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleVerticalScrollbar[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.verticalScrollbar;
	}

	public static global::UnityEngine.GUIStyle GetStyleVerticalScrollbarThumb()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.verticalScrollbarThumb;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalScrollbarThumb[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleVerticalScrollbarThumb[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.verticalScrollbarThumb;
	}

	public static global::UnityEngine.GUIStyle GetStyleVerticalScrollbarUpButton()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.verticalScrollbarUpButton;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalScrollbarUpButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleVerticalScrollbarUpButton[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.verticalScrollbarUpButton;
	}

	public static global::UnityEngine.GUIStyle GetStyleVerticalScrollbarDownButton()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.verticalScrollbarDownButton;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalScrollbarDownButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleVerticalScrollbarDownButton[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.verticalScrollbarDownButton;
	}

	public static global::UnityEngine.GUIStyle GetStyleScrollView()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.scrollView;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleScrollView[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleScrollView[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.scrollView;
	}

	public static global::UnityEngine.GUIStyle GetStyleMinimapBorder()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleMinimap[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleMinimap[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetStyleMinimapButton()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleMinimapButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleMinimapButton[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.button;
	}

	public static global::UnityEngine.GUIStyle GetStyleButton(global::KGFGUIUtility.eStyleButton theStyleButton)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.button;
		}
		global::KGFGUIUtility.Init();
		if (theStyleButton == global::KGFGUIUtility.eStyleButton.eButton && global::KGFGUIUtility.itsStyleButton[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButton[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleButton == global::KGFGUIUtility.eStyleButton.eButtonLeft && global::KGFGUIUtility.itsStyleButtonLeft[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButtonLeft[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleButton == global::KGFGUIUtility.eStyleButton.eButtonRight && global::KGFGUIUtility.itsStyleButtonRight[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButtonRight[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleButton == global::KGFGUIUtility.eStyleButton.eButtonTop && global::KGFGUIUtility.itsStyleButtonTop[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButtonTop[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleButton == global::KGFGUIUtility.eStyleButton.eButtonBottom && global::KGFGUIUtility.itsStyleButtonBottom[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButtonBottom[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleButton == global::KGFGUIUtility.eStyleButton.eButtonMiddle && global::KGFGUIUtility.itsStyleButtonMiddle[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleButtonMiddle[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.button;
	}

	public static global::UnityEngine.GUIStyle GetStyleBox(global::KGFGUIUtility.eStyleBox theStyleBox)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBox && global::KGFGUIUtility.itsStyleBox[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBox[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxInvisible && global::KGFGUIUtility.itsStyleBoxInvisible[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxInvisible[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxInteractive && global::KGFGUIUtility.itsStyleBox[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxLeft && global::KGFGUIUtility.itsStyleBoxLeft[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxLeft[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxLeftInteractive && global::KGFGUIUtility.itsStyleBoxLeft[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxLeftInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxRight && global::KGFGUIUtility.itsStyleBoxRight[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxRight[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxRightInteractive && global::KGFGUIUtility.itsStyleBoxRight[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxRightInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxMiddleHorizontal && global::KGFGUIUtility.itsStyleBoxMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxMiddleHorizontalInteractive && global::KGFGUIUtility.itsStyleBoxMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxMiddleHorizontalInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxTop && global::KGFGUIUtility.itsStyleBoxTop[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxTop[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxTopInteractive && global::KGFGUIUtility.itsStyleBoxTop[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxTopInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxBottom && global::KGFGUIUtility.itsStyleBoxBottom[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxBottom[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxBottomInteractive && global::KGFGUIUtility.itsStyleBoxBottom[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxBottomInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxMiddleVertical && global::KGFGUIUtility.itsStyleBoxMiddleVertical[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxMiddleVertical[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxMiddleVerticalInteractive && global::KGFGUIUtility.itsStyleBoxMiddleVertical[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxMiddleVerticalInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDark && global::KGFGUIUtility.itsStyleBoxDark[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDark[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkInteractive && global::KGFGUIUtility.itsStyleBoxDark[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkLeft && global::KGFGUIUtility.itsStyleBoxDarkLeft[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkLeft[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkLeftInteractive && global::KGFGUIUtility.itsStyleBoxDarkLeft[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkLeftInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkRight && global::KGFGUIUtility.itsStyleBoxDarkRight[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkRight[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkRightInteractive && global::KGFGUIUtility.itsStyleBoxDarkRight[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkRightInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkMiddleHorizontal && global::KGFGUIUtility.itsStyleBoxDarkMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkMiddleHorizontalInteractive && global::KGFGUIUtility.itsStyleBoxDarkMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkMiddleHorizontalInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkTop && global::KGFGUIUtility.itsStyleBoxDarkTop[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkTop[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkTopInteractive && global::KGFGUIUtility.itsStyleBoxDarkTop[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkTopInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkBottom && global::KGFGUIUtility.itsStyleBoxDarkBottom[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkBottom[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkBottomInteractive && global::KGFGUIUtility.itsStyleBoxDarkBottom[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkBottomInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkMiddleVertical && global::KGFGUIUtility.itsStyleBoxDarkMiddleVertical[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkMiddleVertical[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDarkMiddleVerticalInteractive && global::KGFGUIUtility.itsStyleBoxDarkMiddleVertical[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDarkMiddleVerticalInteractive[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleBox == global::KGFGUIUtility.eStyleBox.eBoxDecorated && global::KGFGUIUtility.itsStyleBoxDecorated[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleBoxDecorated[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetStyleSeparator(global::KGFGUIUtility.eStyleSeparator theStyleSeparator)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (theStyleSeparator == global::KGFGUIUtility.eStyleSeparator.eSeparatorHorizontal && global::KGFGUIUtility.itsStyleSeparatorHorizontal[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleSeparatorHorizontal[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleSeparator == global::KGFGUIUtility.eStyleSeparator.eSeparatorVertical && global::KGFGUIUtility.itsStyleSeparatorVertical[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleSeparatorVertical[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleSeparator == global::KGFGUIUtility.eStyleSeparator.eSeparatorVerticalFitInBox && global::KGFGUIUtility.itsStyleSeparatorVerticalFitInBox[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleSeparatorVerticalFitInBox[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.label;
	}

	public static global::UnityEngine.GUIStyle GetStyleLabel(global::KGFGUIUtility.eStyleLabel theStyleLabel)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.label;
		}
		global::KGFGUIUtility.Init();
		if (theStyleLabel == global::KGFGUIUtility.eStyleLabel.eLabel && global::KGFGUIUtility.itsStyleLabel[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleLabel[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleLabel == global::KGFGUIUtility.eStyleLabel.eLabelFitIntoBox && global::KGFGUIUtility.itsStyleLabelFitInToBox[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleLabelFitInToBox[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleLabel == global::KGFGUIUtility.eStyleLabel.eLabelMultiline && global::KGFGUIUtility.itsStyleLabelMultiline[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleLabelMultiline[global::KGFGUIUtility.itsSkinIndex];
		}
		if (theStyleLabel == global::KGFGUIUtility.eStyleLabel.eLabelTitle && global::KGFGUIUtility.itsStyleLabelTitle[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleLabelTitle[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetStyleWindow()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.window;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleWindow[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleWindow[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.window;
	}

	public static global::UnityEngine.GUIStyle GetStyleCursor()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleCursor[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleCursor[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::KGFGUIUtility.itsStyleCursor[global::KGFGUIUtility.itsSkinIndex];
	}

	public static global::UnityEngine.GUIStyle GetTableStyle()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTable[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTable[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetTableHeadingRowStyle()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTableHeadingRow[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTableHeadingRow[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetTableHeadingCellStyle()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTableHeadingCell[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTableHeadingCell[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetTableRowStyle()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTableRow[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTableRow[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static global::UnityEngine.GUIStyle GetTableCellStyle()
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUI.skin.box;
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTableRowCell[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::KGFGUIUtility.itsStyleTableRowCell[global::KGFGUIUtility.itsSkinIndex];
		}
		return global::UnityEngine.GUI.skin.box;
	}

	public static void SetVolume(float theVolume)
	{
		global::KGFGUIUtility.itsVolume = theVolume;
	}

	public static void SetSoundForButton(global::KGFGUIUtility.eStyleButton theButtonStyle, global::UnityEngine.AudioClip theAudioClip)
	{
		global::KGFGUIUtility.SetSound(theButtonStyle.ToString(), theAudioClip);
	}

	public static void SetSoundForToggle(global::KGFGUIUtility.eStyleToggl theTogglStyle, global::UnityEngine.AudioClip theAudioClip)
	{
		global::KGFGUIUtility.SetSound(theTogglStyle.ToString(), theAudioClip);
	}

	private static void SetSound(string theStyle, global::UnityEngine.AudioClip theAudioClip)
	{
		if (theAudioClip != null && global::KGFGUIUtility.itsAudioClips.ContainsKey(theStyle))
		{
			global::KGFGUIUtility.itsAudioClips.Remove(theStyle);
		}
		else
		{
			global::KGFGUIUtility.itsAudioClips[theStyle] = theAudioClip;
		}
	}

	private static void PlaySound(string theStyle)
	{
		if (global::UnityEngine.Application.isPlaying && global::KGFGUIUtility.itsAudioClips.ContainsKey(theStyle))
		{
			global::UnityEngine.AudioSource.PlayClipAtPoint(global::KGFGUIUtility.itsAudioClips[theStyle], global::UnityEngine.Vector3.zero, global::KGFGUIUtility.itsVolume);
		}
	}

	public static void SetEnableKGFSkinsInEdior(bool theSetEnableKGFSkins)
	{
		global::KGFGUIUtility.itsEnableKGFSkins = theSetEnableKGFSkins;
	}

	public static void SetSkinIndex(int theIndex)
	{
		global::KGFGUIUtility.itsSkinIndex = theIndex;
		if (global::KGFGUIUtility.itsSkinIndex == 0 && !global::KGFGUIUtility.itsEnableKGFSkins)
		{
			global::KGFGUIUtility.itsSkinIndex = -1;
		}
	}

	public static void SetSkinPath(string thePath)
	{
		global::KGFGUIUtility.itsDefaultGuiSkinPath[1] = thePath;
		global::KGFGUIUtility.itsResetPath[1] = true;
	}

	public static void SetSkinPathEditor(string thePath)
	{
		global::KGFGUIUtility.itsDefaultGuiSkinPath[0] = thePath;
		global::KGFGUIUtility.itsResetPath[0] = true;
	}

	public static string GetSkinPath()
	{
		return global::KGFGUIUtility.itsDefaultGuiSkinPath[global::KGFGUIUtility.itsSkinIndex];
	}

	private static void Init()
	{
		global::KGFGUIUtility.Init(false);
	}

	private static void Init(bool theForceInit)
	{
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return;
		}
		if (global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex] != null && !theForceInit && !global::KGFGUIUtility.itsResetPath[global::KGFGUIUtility.itsSkinIndex])
		{
			return;
		}
		global::KGFGUIUtility.itsResetPath[global::KGFGUIUtility.itsSkinIndex] = false;
		global::UnityEngine.Debug.Log("Loading skin: " + global::KGFGUIUtility.itsDefaultGuiSkinPath[global::KGFGUIUtility.itsSkinIndex]);
		global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex] = (global::UnityEngine.Resources.Load(global::KGFGUIUtility.itsDefaultGuiSkinPath[global::KGFGUIUtility.itsSkinIndex]) as global::UnityEngine.GUISkin);
		if (global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex] == null)
		{
			global::UnityEngine.Debug.Log("Kolmich Game Framework default skin wasn`t found");
			global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex] = global::UnityEngine.GUI.skin;
			return;
		}
		global::UnityEngine.GUI.skin = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex];
		global::KGFGUIUtility.itsStyleToggle[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle");
		global::KGFGUIUtility.itsStyleTextField[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("textfield");
		global::KGFGUIUtility.itsStyleTextFieldLeft[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("textfield_left");
		global::KGFGUIUtility.itsStyleTextFieldRight[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("textfield_right");
		global::KGFGUIUtility.itsStyleTextArea[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("textarea");
		global::KGFGUIUtility.itsStyleWindow[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("window");
		global::KGFGUIUtility.itsStyleHorizontalSlider[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("horizontalslider");
		global::KGFGUIUtility.itsStyleHorizontalSliderThumb[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("horizontalsliderthumb");
		global::KGFGUIUtility.itsStyleVerticalSlider[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("verticalslider");
		global::KGFGUIUtility.itsStyleVerticalSliderThumb[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("verticalsliderthumb");
		global::KGFGUIUtility.itsStyleHorizontalScrollbar[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("horizontalscrollbar");
		global::KGFGUIUtility.itsStyleHorizontalScrollbarThumb[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("horizontalscrollbarthumb");
		global::KGFGUIUtility.itsStyleHorizontalScrollbarLeftButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("horizontalscrollbarleftbutton");
		global::KGFGUIUtility.itsStyleHorizontalScrollbarRightButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("horizontalscrollbarrightbutton");
		global::KGFGUIUtility.itsStyleVerticalScrollbar[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("verticalscrollbar");
		global::KGFGUIUtility.itsStyleVerticalScrollbarThumb[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("verticalscrollbarthumb");
		global::KGFGUIUtility.itsStyleVerticalScrollbarUpButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("verticalscrollbarupbutton");
		global::KGFGUIUtility.itsStyleVerticalScrollbarDownButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("verticalscrollbardownbutton");
		global::KGFGUIUtility.itsStyleScrollView[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("scrollview");
		global::KGFGUIUtility.itsStyleMinimap[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("minimap");
		global::KGFGUIUtility.itsStyleMinimapButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("minimap_button");
		global::KGFGUIUtility.itsStyleToggleStreched[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_stretched");
		global::KGFGUIUtility.itsStyleToggleCompact[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_compact");
		global::KGFGUIUtility.itsStyleToggleSuperCompact[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_supercompact");
		global::KGFGUIUtility.itsStyleToggleRadioStreched[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_radio_stretched");
		global::KGFGUIUtility.itsStyleToggleRadioCompact[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_radio_compact");
		global::KGFGUIUtility.itsStyleToggleRadioSuperCompact[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_radio_supercompact");
		global::KGFGUIUtility.itsStyleToggleSwitch[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_switch");
		global::KGFGUIUtility.itsStyleToggleBoolean[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_boolean");
		global::KGFGUIUtility.itsStyleToggleArrow[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_arrow");
		global::KGFGUIUtility.itsStyleToggleButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("toggle_button");
		global::KGFGUIUtility.itsStyleButton[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("Button");
		global::KGFGUIUtility.itsStyleButtonLeft[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("button_left");
		global::KGFGUIUtility.itsStyleButtonRight[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("button_right");
		global::KGFGUIUtility.itsStyleButtonTop[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("button_top");
		global::KGFGUIUtility.itsStyleButtonBottom[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("button_bottom");
		global::KGFGUIUtility.itsStyleButtonMiddle[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("button_middle");
		global::KGFGUIUtility.itsStyleBox[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("Box");
		global::KGFGUIUtility.itsStyleBoxInvisible[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_invisible");
		global::KGFGUIUtility.itsStyleBoxInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_interactive");
		global::KGFGUIUtility.itsStyleBoxLeft[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_left");
		global::KGFGUIUtility.itsStyleBoxLeftInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_left_interactive");
		global::KGFGUIUtility.itsStyleBoxRight[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_right");
		global::KGFGUIUtility.itsStyleBoxRightInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_right_interactive");
		global::KGFGUIUtility.itsStyleBoxMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_middle_horizontal");
		global::KGFGUIUtility.itsStyleBoxMiddleHorizontalInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_middle_horizontal_interactive");
		global::KGFGUIUtility.itsStyleBoxTop[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_top");
		global::KGFGUIUtility.itsStyleBoxTopInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_top_interactive");
		global::KGFGUIUtility.itsStyleBoxBottom[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_bottom");
		global::KGFGUIUtility.itsStyleBoxBottomInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_bottom_interactive");
		global::KGFGUIUtility.itsStyleBoxMiddleVertical[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_middle_vertical");
		global::KGFGUIUtility.itsStyleBoxMiddleVerticalInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_middle_vertical_interactive");
		global::KGFGUIUtility.itsStyleBoxDark[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark");
		global::KGFGUIUtility.itsStyleBoxDarkInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_interactive");
		global::KGFGUIUtility.itsStyleBoxDarkLeft[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_left");
		global::KGFGUIUtility.itsStyleBoxDarkLeftInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_left_interactive");
		global::KGFGUIUtility.itsStyleBoxDarkRight[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_right");
		global::KGFGUIUtility.itsStyleBoxDarkRightInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_right_interactive");
		global::KGFGUIUtility.itsStyleBoxDarkMiddleHorizontal[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_middle_horizontal");
		global::KGFGUIUtility.itsStyleBoxDarkMiddleHorizontalInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_middle_horizontal_interactive");
		global::KGFGUIUtility.itsStyleBoxDarkTop[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_top");
		global::KGFGUIUtility.itsStyleBoxDarkTopInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_top_interactive");
		global::KGFGUIUtility.itsStyleBoxDarkBottom[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_bottom");
		global::KGFGUIUtility.itsStyleBoxDarkBottomInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_bottom_interactive");
		global::KGFGUIUtility.itsStyleBoxDarkMiddleVertical[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_middle_vertical");
		global::KGFGUIUtility.itsStyleBoxDarkMiddleVerticalInteractive[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_dark_middle_vertical_interactive");
		global::KGFGUIUtility.itsStyleBoxDecorated[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("box_decorated");
		global::KGFGUIUtility.itsStyleSeparatorVertical[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("separator_vertical");
		global::KGFGUIUtility.itsStyleSeparatorVerticalFitInBox[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("separator_vertical_fitinbox");
		global::KGFGUIUtility.itsStyleSeparatorHorizontal[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("separator_horizontal");
		global::KGFGUIUtility.itsStyleLabel[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("label");
		global::KGFGUIUtility.itsStyleLabelFitInToBox[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("label_fitintobox");
		global::KGFGUIUtility.itsStyleLabelMultiline[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("label_multiline");
		global::KGFGUIUtility.itsStyleLabelTitle[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("label_title");
		global::KGFGUIUtility.itsStyleCursor[global::KGFGUIUtility.itsSkinIndex] = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex].GetStyle("mouse_cursor");
	}

	public static void BeginWindowHeader(string theTitle, global::UnityEngine.Texture2D theIcon)
	{
		global::KGFGUIUtility.Init();
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxDark, new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.Label(string.Empty, theIcon, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(global::KGFGUIUtility.GetSkinHeight())
		});
		global::KGFGUIUtility.Label(theTitle, global::KGFGUIUtility.eStyleLabel.eLabel, new global::UnityEngine.GUILayoutOption[0]);
	}

	public static bool EndWindowHeader(bool theCloseButton)
	{
		bool result = false;
		if (theCloseButton)
		{
			global::KGFGUIUtility.Init();
			if (global::KGFGUIUtility.itsSkinIndex == -1)
			{
				result = global::UnityEngine.GUILayout.Button("x", new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(global::KGFGUIUtility.GetSkinHeight())
				});
			}
			else
			{
				result = global::KGFGUIUtility.Button("x", global::KGFGUIUtility.eStyleButton.eButton, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.Width(global::KGFGUIUtility.GetSkinHeight())
				});
			}
		}
		global::KGFGUIUtility.EndHorizontalBox();
		return result;
	}

	public static void RenderDropDownList()
	{
		if (global::KGFGUIDropDown.itsOpenInstance != null && global::KGFGUIDropDown.itsCorrectedOffset)
		{
			global::UnityEngine.GUI.depth = 0;
			global::UnityEngine.Rect screenRect;
			bool flag;
			if (global::KGFGUIDropDown.itsOpenInstance.itsDirection == global::KGFGUIDropDown.eDropDirection.eDown || (global::KGFGUIDropDown.itsOpenInstance.itsDirection == global::KGFGUIDropDown.eDropDirection.eAuto && global::KGFGUIDropDown.itsOpenInstance.itsLastRect.y + global::KGFGUIUtility.GetStyleButton(global::KGFGUIUtility.eStyleButton.eButton).fixedHeight + global::KGFGUIDropDown.itsOpenInstance.itsHeight < (float)global::UnityEngine.Screen.height))
			{
				screenRect = new global::UnityEngine.Rect(global::KGFGUIDropDown.itsOpenInstance.itsLastRect.x, global::KGFGUIDropDown.itsOpenInstance.itsLastRect.y + global::KGFGUIUtility.GetStyleButton(global::KGFGUIUtility.eStyleButton.eButton).fixedHeight, global::KGFGUIDropDown.itsOpenInstance.itsWidth, global::KGFGUIDropDown.itsOpenInstance.itsHeight);
				flag = true;
			}
			else
			{
				screenRect = new global::UnityEngine.Rect(global::KGFGUIDropDown.itsOpenInstance.itsLastRect.x, global::KGFGUIDropDown.itsOpenInstance.itsLastRect.y - global::KGFGUIDropDown.itsOpenInstance.itsHeight, global::KGFGUIDropDown.itsOpenInstance.itsWidth, global::KGFGUIDropDown.itsOpenInstance.itsHeight);
				flag = false;
			}
			global::UnityEngine.GUILayout.BeginArea(screenRect);
			if (global::KGFGUIUtility.itsSkinIndex == -1)
			{
				global::KGFGUIDropDown.itsOpenInstance.itsScrollPosition = global::KGFGUIUtility.BeginScrollView(global::KGFGUIDropDown.itsOpenInstance.itsScrollPosition, false, false, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
			}
			else
			{
				global::KGFGUIDropDown.itsOpenInstance.itsScrollPosition = global::UnityEngine.GUILayout.BeginScrollView(global::KGFGUIDropDown.itsOpenInstance.itsScrollPosition, false, false, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				});
			}
			foreach (string text in global::KGFGUIDropDown.itsOpenInstance.GetEntrys())
			{
				if (text != string.Empty && global::KGFGUIUtility.Button(text, global::KGFGUIUtility.eStyleButton.eButtonMiddle, new global::UnityEngine.GUILayoutOption[]
				{
					global::UnityEngine.GUILayout.ExpandWidth(true)
				}))
				{
					global::KGFGUIDropDown.itsOpenInstance.SetSelectedItem(text);
					global::KGFGUIDropDown.itsOpenInstance = null;
					break;
				}
			}
			global::UnityEngine.GUILayout.EndScrollView();
			global::UnityEngine.GUILayout.EndArea();
			if (flag)
			{
				screenRect.y -= global::KGFGUIUtility.GetSkinHeight();
				screenRect.height += global::KGFGUIUtility.GetSkinHeight();
			}
			else
			{
				screenRect.height += global::KGFGUIUtility.GetSkinHeight();
			}
			global::UnityEngine.Vector3 mousePosition = global::UnityEngine.Input.mousePosition;
			mousePosition.y = (float)global::UnityEngine.Screen.height - mousePosition.y;
			if (!screenRect.Contains(mousePosition) && global::UnityEngine.Event.current.type == global::UnityEngine.EventType.MouseDown && global::UnityEngine.Event.current.button == 0)
			{
				global::KGFGUIDropDown.itsOpenInstance = null;
			}
			if (global::KGFGUIDropDown.itsOpenInstance != null)
			{
				if (screenRect.Contains(mousePosition))
				{
					global::KGFGUIDropDown.itsOpenInstance.itsHover = true;
				}
				else
				{
					global::KGFGUIDropDown.itsOpenInstance.itsHover = false;
				}
			}
		}
	}

	public static void Space()
	{
		global::UnityEngine.GUILayout.Space(global::KGFGUIUtility.GetSkinHeight());
	}

	public static void SpaceSmall()
	{
		global::UnityEngine.GUILayout.Space(global::KGFGUIUtility.GetSkinHeight() / 2f);
	}

	public static void Label(string theText, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Label(theText, global::KGFGUIUtility.eStyleLabel.eLabel, theLayout);
	}

	public static void Label(string theText, global::KGFGUIUtility.eStyleLabel theStyleLabel, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Label(theText, null, theStyleLabel, theLayout);
	}

	public static void Label(string theText, global::UnityEngine.Texture2D theImage, global::KGFGUIUtility.eStyleLabel theStyleLabel, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		global::UnityEngine.GUIContent content;
		if (theImage != null)
		{
			content = new global::UnityEngine.GUIContent(theText, theImage);
		}
		else
		{
			content = new global::UnityEngine.GUIContent(theText);
		}
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			global::UnityEngine.GUILayout.Label(content, theLayout);
		}
		else
		{
			global::UnityEngine.GUILayout.Label(content, global::KGFGUIUtility.GetStyleLabel(theStyleLabel), theLayout);
		}
	}

	public static void Separator(global::KGFGUIUtility.eStyleSeparator theStyleSeparator, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			global::UnityEngine.GUILayout.Label("|", theLayout);
		}
		else
		{
			global::UnityEngine.GUILayout.Label(string.Empty, global::KGFGUIUtility.GetStyleSeparator(theStyleSeparator), theLayout);
		}
	}

	public static bool Toggle(bool theValue, string theText, global::KGFGUIUtility.eStyleToggl theToggleStyle, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		bool flag;
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			flag = global::UnityEngine.GUILayout.Toggle(theValue, theText, theLayout);
		}
		else
		{
			flag = global::UnityEngine.GUILayout.Toggle(theValue, theText, global::KGFGUIUtility.GetStyleToggl(theToggleStyle), theLayout);
		}
		if (flag != theValue)
		{
			global::KGFGUIUtility.PlaySound(theToggleStyle.ToString());
		}
		return flag;
	}

	public static bool Toggle(bool theValue, global::UnityEngine.Texture2D theImage, global::KGFGUIUtility.eStyleToggl theToggleStyle, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		bool flag;
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			flag = global::UnityEngine.GUILayout.Toggle(theValue, theImage, theLayout);
		}
		else
		{
			flag = global::UnityEngine.GUILayout.Toggle(theValue, theImage, global::KGFGUIUtility.GetStyleToggl(theToggleStyle), theLayout);
		}
		if (flag != theValue)
		{
			global::KGFGUIUtility.PlaySound(theToggleStyle.ToString());
		}
		return flag;
	}

	public static bool Toggle(bool theValue, string theText, global::UnityEngine.Texture2D theImage, global::KGFGUIUtility.eStyleToggl theToggleStyle, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		global::UnityEngine.GUIContent content;
		if (theImage != null)
		{
			content = new global::UnityEngine.GUIContent(theText, theImage);
		}
		else
		{
			content = new global::UnityEngine.GUIContent(theText);
		}
		bool flag;
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			flag = global::UnityEngine.GUILayout.Toggle(theValue, content, theLayout);
		}
		else
		{
			flag = global::UnityEngine.GUILayout.Toggle(theValue, content, global::KGFGUIUtility.GetStyleToggl(theToggleStyle), theLayout);
		}
		if (flag != theValue)
		{
			global::KGFGUIUtility.PlaySound(theToggleStyle.ToString());
		}
		return flag;
	}

	public static global::UnityEngine.Rect Window(int theId, global::UnityEngine.Rect theRect, global::UnityEngine.GUI.WindowFunction theFunction, string theText, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		return global::KGFGUIUtility.Window(theId, theRect, theFunction, null, theText, theLayout);
	}

	public static global::UnityEngine.Rect Window(int theId, global::UnityEngine.Rect theRect, global::UnityEngine.GUI.WindowFunction theFunction, global::UnityEngine.Texture theImage, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		return global::KGFGUIUtility.Window(theId, theRect, theFunction, theImage, string.Empty, theLayout);
	}

	public static global::UnityEngine.Rect Window(int theId, global::UnityEngine.Rect theRect, global::UnityEngine.GUI.WindowFunction theFunction, global::UnityEngine.Texture theImage, string theText, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		global::UnityEngine.GUIContent content;
		if (theImage != null)
		{
			content = new global::UnityEngine.GUIContent(theText, theImage);
		}
		else
		{
			content = new global::UnityEngine.GUIContent(theText);
		}
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUILayout.Window(theId, theRect, theFunction, content, theLayout);
		}
		if (global::KGFGUIUtility.itsStyleWindow[global::KGFGUIUtility.itsSkinIndex] != null)
		{
			return global::UnityEngine.GUILayout.Window(theId, theRect, theFunction, content, global::KGFGUIUtility.itsStyleWindow[global::KGFGUIUtility.itsSkinIndex], theLayout);
		}
		return global::UnityEngine.GUILayout.Window(theId, theRect, theFunction, content, theLayout);
	}

	public static void Box(string theText, global::KGFGUIUtility.eStyleBox theStyleBox, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Box(null, theText, theStyleBox, theLayout);
	}

	public static void Box(global::UnityEngine.Texture theImage, global::KGFGUIUtility.eStyleBox theStyleBox, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Box(theImage, string.Empty, theStyleBox, theLayout);
	}

	public static void Box(global::UnityEngine.Texture theImage, string theText, global::KGFGUIUtility.eStyleBox theStyleBox, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		global::UnityEngine.GUIContent content;
		if (theImage != null)
		{
			content = new global::UnityEngine.GUIContent(theText, theImage);
		}
		else
		{
			content = new global::UnityEngine.GUIContent(theText);
		}
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			global::UnityEngine.GUILayout.Box(content, theLayout);
		}
		else
		{
			global::UnityEngine.GUILayout.Box(content, global::KGFGUIUtility.GetStyleBox(theStyleBox), theLayout);
		}
	}

	public static void BeginVerticalBox(global::KGFGUIUtility.eStyleBox theStyleBox, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			global::UnityEngine.GUILayout.BeginVertical(global::UnityEngine.GUI.skin.box, theLayout);
		}
		else
		{
			global::UnityEngine.GUILayout.BeginVertical(global::KGFGUIUtility.GetStyleBox(theStyleBox), theLayout);
		}
	}

	public static void EndVerticalBox()
	{
		global::UnityEngine.GUILayout.EndVertical();
	}

	public static void BeginVerticalPadding()
	{
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
	}

	public static void EndVerticalPadding()
	{
		global::KGFGUIUtility.EndHorizontalBox();
		global::UnityEngine.GUILayout.EndVertical();
	}

	public static void BeginHorizontalPadding()
	{
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[0]);
		global::KGFGUIUtility.BeginVerticalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
	}

	public static void EndHorizontalPadding()
	{
		global::KGFGUIUtility.EndVerticalBox();
		global::UnityEngine.GUILayout.EndHorizontal();
	}

	public static void BeginHorizontalBox(global::KGFGUIUtility.eStyleBox theStyleBox, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			global::UnityEngine.GUILayout.BeginHorizontal(global::UnityEngine.GUI.skin.box, theLayout);
		}
		else
		{
			global::UnityEngine.GUILayout.BeginHorizontal(global::KGFGUIUtility.GetStyleBox(theStyleBox), theLayout);
		}
	}

	public static void EndHorizontalBox()
	{
		global::UnityEngine.GUILayout.EndHorizontal();
	}

	public static global::UnityEngine.Vector2 BeginScrollView(global::UnityEngine.Vector2 thePosition, bool theHorizontalAlwaysVisible, bool theVerticalAlwaysVisible, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsSkinIndex != -1)
		{
			global::UnityEngine.GUI.skin = global::KGFGUIUtility.itsSkin[global::KGFGUIUtility.itsSkinIndex];
		}
		if (global::KGFGUIUtility.itsStyleHorizontalScrollbar != null && global::KGFGUIUtility.itsStyleVerticalScrollbar != null && global::KGFGUIUtility.itsSkinIndex != -1)
		{
			return global::UnityEngine.GUILayout.BeginScrollView(thePosition, theHorizontalAlwaysVisible, theVerticalAlwaysVisible, global::KGFGUIUtility.itsStyleHorizontalScrollbar[global::KGFGUIUtility.itsSkinIndex], global::KGFGUIUtility.itsStyleVerticalScrollbar[global::KGFGUIUtility.itsSkinIndex], theLayout);
		}
		return global::UnityEngine.GUILayout.BeginScrollView(thePosition, theHorizontalAlwaysVisible, theVerticalAlwaysVisible, theLayout);
	}

	public static void EndScrollView()
	{
		global::UnityEngine.GUILayout.EndScrollView();
	}

	public static string TextField(string theText, global::KGFGUIUtility.eStyleTextField theStyleTextField, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			return global::UnityEngine.GUILayout.TextField(theText, theLayout);
		}
		return global::UnityEngine.GUILayout.TextField(theText, global::KGFGUIUtility.GetStyleTextField(theStyleTextField), theLayout);
	}

	public static string TextArea(string theText, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleTextArea[global::KGFGUIUtility.itsSkinIndex] != null && global::KGFGUIUtility.itsSkinIndex != -1)
		{
			return global::UnityEngine.GUILayout.TextArea(theText, global::KGFGUIUtility.itsStyleTextArea[global::KGFGUIUtility.itsSkinIndex], theLayout);
		}
		return global::UnityEngine.GUILayout.TextArea(theText, theLayout);
	}

	public static bool Button(string theText, global::KGFGUIUtility.eStyleButton theButtonStyle, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		return global::KGFGUIUtility.Button(null, theText, theButtonStyle, theLayout);
	}

	public static bool Button(global::UnityEngine.Texture theImage, global::KGFGUIUtility.eStyleButton theButtonStyle, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		return global::KGFGUIUtility.Button(theImage, string.Empty, theButtonStyle, theLayout);
	}

	public static bool Button(global::UnityEngine.Texture theImage, string theText, global::KGFGUIUtility.eStyleButton theButtonStyle, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::UnityEngine.GUIContent content;
		if (theImage != null)
		{
			content = new global::UnityEngine.GUIContent(theText, theImage);
		}
		else
		{
			content = new global::UnityEngine.GUIContent(theText);
		}
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsSkinIndex == -1)
		{
			if (global::UnityEngine.GUILayout.Button(content, theLayout))
			{
				global::KGFGUIUtility.PlaySound(theButtonStyle.ToString());
				return true;
			}
		}
		else if (global::UnityEngine.GUILayout.Button(content, global::KGFGUIUtility.GetStyleButton(theButtonStyle), theLayout))
		{
			global::KGFGUIUtility.PlaySound(theButtonStyle.ToString());
			return true;
		}
		return false;
	}

	public static global::KGFGUIUtility.eCursorState Cursor()
	{
		return global::KGFGUIUtility.Cursor(null, null, null, null, null);
	}

	public static global::KGFGUIUtility.eCursorState Cursor(global::UnityEngine.Texture theUp, global::UnityEngine.Texture theRight, global::UnityEngine.Texture theDown, global::UnityEngine.Texture theLeft, global::UnityEngine.Texture theCenter)
	{
		float skinHeight = global::KGFGUIUtility.GetSkinHeight();
		float num = skinHeight * 3f;
		global::KGFGUIUtility.eCursorState result = global::KGFGUIUtility.eCursorState.eNone;
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(false),
			global::UnityEngine.GUILayout.ExpandHeight(false)
		});
		global::KGFGUIUtility.BeginHorizontalBox(global::KGFGUIUtility.eStyleBox.eBoxInvisible, new global::UnityEngine.GUILayoutOption[0]);
		global::UnityEngine.GUILayout.BeginVertical(new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(num),
			global::UnityEngine.GUILayout.Height(num)
		});
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(false),
			global::UnityEngine.GUILayout.ExpandHeight(false)
		});
		global::UnityEngine.GUILayout.Space(skinHeight);
		if (theUp != null)
		{
			if (global::KGFGUIUtility.Button(theUp, global::KGFGUIUtility.eStyleButton.eButtonTop, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(skinHeight)
			}))
			{
				result = global::KGFGUIUtility.eCursorState.eUp;
			}
		}
		else if (global::KGFGUIUtility.Button(string.Empty, global::KGFGUIUtility.eStyleButton.eButtonTop, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(skinHeight)
		}))
		{
			result = global::KGFGUIUtility.eCursorState.eUp;
		}
		global::UnityEngine.GUILayout.Space(skinHeight);
		global::UnityEngine.GUILayout.EndHorizontal();
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(false),
			global::UnityEngine.GUILayout.ExpandHeight(false)
		});
		if (theLeft != null)
		{
			if (global::KGFGUIUtility.Button(theLeft, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(skinHeight)
			}))
			{
				result = global::KGFGUIUtility.eCursorState.eLeft;
			}
		}
		else if (global::KGFGUIUtility.Button(string.Empty, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(skinHeight)
		}))
		{
			result = global::KGFGUIUtility.eCursorState.eLeft;
		}
		if (theCenter != null)
		{
			if (global::KGFGUIUtility.Button(theCenter, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(skinHeight)
			}))
			{
				result = global::KGFGUIUtility.eCursorState.eCenter;
			}
		}
		else if (global::KGFGUIUtility.Button(string.Empty, global::KGFGUIUtility.eStyleButton.eButtonMiddle, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(skinHeight)
		}))
		{
			result = global::KGFGUIUtility.eCursorState.eCenter;
		}
		if (theRight != null)
		{
			if (global::KGFGUIUtility.Button(theRight, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(skinHeight)
			}))
			{
				result = global::KGFGUIUtility.eCursorState.eRight;
			}
		}
		else if (global::KGFGUIUtility.Button(string.Empty, global::KGFGUIUtility.eStyleButton.eButtonRight, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(skinHeight)
		}))
		{
			result = global::KGFGUIUtility.eCursorState.eRight;
		}
		global::UnityEngine.GUILayout.EndHorizontal();
		global::UnityEngine.GUILayout.BeginHorizontal(new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.ExpandWidth(false),
			global::UnityEngine.GUILayout.ExpandHeight(false)
		});
		global::UnityEngine.GUILayout.Space(skinHeight);
		if (theDown != null)
		{
			if (global::KGFGUIUtility.Button(theDown, global::KGFGUIUtility.eStyleButton.eButtonLeft, new global::UnityEngine.GUILayoutOption[]
			{
				global::UnityEngine.GUILayout.Width(skinHeight)
			}))
			{
				result = global::KGFGUIUtility.eCursorState.eDown;
			}
		}
		else if (global::KGFGUIUtility.Button(string.Empty, global::KGFGUIUtility.eStyleButton.eButtonBottom, new global::UnityEngine.GUILayoutOption[]
		{
			global::UnityEngine.GUILayout.Width(skinHeight)
		}))
		{
			result = global::KGFGUIUtility.eCursorState.eDown;
		}
		global::UnityEngine.GUILayout.Space(skinHeight);
		global::UnityEngine.GUILayout.EndHorizontal();
		global::UnityEngine.GUILayout.EndVertical();
		global::KGFGUIUtility.EndHorizontalBox();
		global::UnityEngine.GUILayout.EndVertical();
		return result;
	}

	public static float HorizontalSlider(float theValue, float theLeftValue, float theRightValue, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleHorizontalSlider != null && global::KGFGUIUtility.itsStyleHorizontalSliderThumb != null && global::KGFGUIUtility.itsSkinIndex != -1)
		{
			return global::UnityEngine.GUILayout.HorizontalSlider(theValue, theLeftValue, theRightValue, global::KGFGUIUtility.itsStyleHorizontalSlider[global::KGFGUIUtility.itsSkinIndex], global::KGFGUIUtility.itsStyleHorizontalSliderThumb[global::KGFGUIUtility.itsSkinIndex], theLayout);
		}
		return global::UnityEngine.GUILayout.HorizontalSlider(theValue, theLeftValue, theRightValue, theLayout);
	}

	public static float VerticalSlider(float theValue, float theLeftValue, float theRightValue, params global::UnityEngine.GUILayoutOption[] theLayout)
	{
		global::KGFGUIUtility.Init();
		if (global::KGFGUIUtility.itsStyleVerticalSlider != null && global::KGFGUIUtility.itsStyleVerticalSliderThumb != null && global::KGFGUIUtility.itsSkinIndex != -1)
		{
			return global::UnityEngine.GUILayout.VerticalSlider(theValue, theLeftValue, theRightValue, global::KGFGUIUtility.itsStyleVerticalSlider[global::KGFGUIUtility.itsSkinIndex], global::KGFGUIUtility.itsStyleVerticalSliderThumb[global::KGFGUIUtility.itsSkinIndex], theLayout);
		}
		return global::UnityEngine.GUILayout.VerticalSlider(theValue, theLeftValue, theRightValue, theLayout);
	}

	private static bool itsEnableKGFSkins = true;

	private static string[] itsDefaultGuiSkinPath = new string[]
	{
		"KGFSkins/default/skins/skin_default_16",
		"KGFSkins/default/skins/skin_default_16"
	};

	private static int itsSkinIndex = 1;

	private static bool[] itsResetPath = new bool[2];

	protected static global::UnityEngine.GUISkin[] itsSkin = new global::UnityEngine.GUISkin[2];

	private static global::UnityEngine.Texture2D itsIcon = null;

	private static global::UnityEngine.Texture2D itsKGFCopyright = null;

	private static global::UnityEngine.Texture2D itsIconHelp = null;

	private static global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AudioClip> itsAudioClips = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.AudioClip>();

	private static float itsVolume = 1f;

	private static global::UnityEngine.GUIStyle[] itsStyleToggle = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTextField = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTextFieldLeft = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTextFieldRight = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTextArea = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleWindow = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleHorizontalSlider = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleHorizontalSliderThumb = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleVerticalSlider = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleVerticalSliderThumb = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleHorizontalScrollbar = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleHorizontalScrollbarThumb = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleHorizontalScrollbarLeftButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleHorizontalScrollbarRightButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleVerticalScrollbar = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleVerticalScrollbarThumb = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleVerticalScrollbarUpButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleVerticalScrollbarDownButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleScrollView = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleMinimap = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleMinimapButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleStreched = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleCompact = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleSuperCompact = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleRadioStreched = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleRadioCompact = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleRadioSuperCompact = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleSwitch = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleBoolean = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleArrow = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleToggleButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleButton = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleButtonLeft = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleButtonRight = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleButtonTop = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleButtonBottom = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleButtonMiddle = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBox = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxInvisible = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxLeft = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxLeftInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxRight = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxRightInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxMiddleHorizontal = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxMiddleHorizontalInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxTop = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxTopInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxBottom = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxBottomInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxMiddleVertical = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxMiddleVerticalInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDark = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkLeft = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkLeftInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkRight = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkRightInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkMiddleHorizontal = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkMiddleHorizontalInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkTop = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkTopInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkBottom = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkBottomInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkMiddleVertical = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDarkMiddleVerticalInteractive = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleBoxDecorated = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleSeparatorVertical = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleSeparatorVerticalFitInBox = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleSeparatorHorizontal = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleLabel = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleLabelMultiline = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleLabelTitle = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleLabelFitInToBox = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTable = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTableHeadingRow = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTableHeadingCell = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTableRow = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleTableRowCell = new global::UnityEngine.GUIStyle[2];

	private static global::UnityEngine.GUIStyle[] itsStyleCursor = new global::UnityEngine.GUIStyle[2];

	public enum eStyleButton
	{
		eButton,
		eButtonLeft,
		eButtonRight,
		eButtonTop,
		eButtonBottom,
		eButtonMiddle
	}

	public enum eStyleToggl
	{
		eToggl,
		eTogglStreched,
		eTogglCompact,
		eTogglSuperCompact,
		eTogglRadioStreched,
		eTogglRadioCompact,
		eTogglRadioSuperCompact,
		eTogglSwitch,
		eTogglBoolean,
		eTogglArrow,
		eTogglButton
	}

	public enum eStyleTextField
	{
		eTextField,
		eTextFieldLeft,
		eTextFieldRight
	}

	public enum eStyleBox
	{
		eBox,
		eBoxInvisible,
		eBoxInteractive,
		eBoxLeft,
		eBoxLeftInteractive,
		eBoxRight,
		eBoxRightInteractive,
		eBoxMiddleHorizontal,
		eBoxMiddleHorizontalInteractive,
		eBoxTop,
		eBoxTopInteractive,
		eBoxMiddleVertical,
		eBoxMiddleVerticalInteractive,
		eBoxBottom,
		eBoxBottomInteractive,
		eBoxDark,
		eBoxDarkInteractive,
		eBoxDarkLeft,
		eBoxDarkLeftInteractive,
		eBoxDarkRight,
		eBoxDarkRightInteractive,
		eBoxDarkMiddleHorizontal,
		eBoxDarkMiddleHorizontalInteractive,
		eBoxDarkTop,
		eBoxDarkTopInteractive,
		eBoxDarkBottom,
		eBoxDarkBottomInteractive,
		eBoxDarkMiddleVertical,
		eBoxDarkMiddleVerticalInteractive,
		eBoxDecorated
	}

	public enum eStyleSeparator
	{
		eSeparatorHorizontal,
		eSeparatorVertical,
		eSeparatorVerticalFitInBox
	}

	public enum eStyleLabel
	{
		eLabel,
		eLabelMultiline,
		eLabelTitle,
		eLabelFitIntoBox
	}

	public enum eStyleImage
	{
		eImage,
		eImageFitIntoBox
	}

	public enum eCursorState
	{
		eUp,
		eRight,
		eDown,
		eLeft,
		eCenter,
		eNone
	}
}
