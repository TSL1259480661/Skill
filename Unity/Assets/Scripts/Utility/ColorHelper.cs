using App;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
	private static UDebugger debugger = new UDebugger("ColorHelper");

	private static Dictionary<string, Color> ColorDict = new Dictionary<string, Color>()
	{
		{"red", Color.red},
		{"green", Color.green},
		{"white", Color.white},
		{"black", Color.black},
		{"blue", Color.blue},
		{"yellow", Color.yellow},
		{"gray", Color.gray},
	};


	/// <summary>
	/// 根据十六进制值字符串装换
	/// </summary>
	/// <param name="colorStr">#000000</param>
	/// <returns></returns>
	public static Color CoverFromHEX(string colorStr)
	{
		if (string.IsNullOrEmpty(colorStr))
		{
			return Color.black;
		}
		if (colorStr.StartsWith("#"))
		{
			colorStr = colorStr.Substring(1); // 去掉#
		}

		if (colorStr.Length != 6)
		{
			debugger.LogError("Invalid color code. It must be in the form #RRGGBB.");
			return Color.black;
		}

		byte r, g, b;
		bool success = true;

		// 尝试从字符串中解析每一种颜色分量
		success &= byte.TryParse(colorStr.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out r);
		success &= byte.TryParse(colorStr.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out g);
		success &= byte.TryParse(colorStr.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out b);

		if (!success)
		{
			debugger.LogError("Failed to parse color components from the string.");
			return Color.black;
		}

		return new Color(r / 255f, g / 255f, b / 255f);
	}

	public static Color GetColorFromHex(int color)
	{
		return new Color((color >> 16 & 0xFF) / 255f,
			   (color >> 8 & 0xFF) / 255f, (color & 0xFF) / 255f, 1f);
	}

	public static Color CoverFromName(string colorName)
	{
		ColorDict.TryGetValue(colorName, out var color);
		if (color == null)
		{
			color = Color.black;
		}
		return color;
	}
}
