using UnityEngine;
using Color = UnityEngine.Color;

/// <summary>
/// 程序配置
/// </summary>
public class CommonConfig
{
	/// <summary>
	/// 拖动缩放
	/// </summary>
	public static Vector3 dragIconScale = new Vector3(0.7F, 0.7F, 0.7F);

	/// <summary>
	/// 十六进制转颜色
	/// </summary>
	public static Color HexToColor(string hex)
	{
		Color color;
		ColorUtility.TryParseHtmlString(hex, out color);
		return color;
	}
}
