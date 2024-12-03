using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 渐变字体
/// </summary>
public class GradientText : GText
{
	/// <summary>
	/// 渐变颜色控件
	/// </summary>
	private TextGradient gradient;

	/// <summary>
	/// 访问渐变颜色控件
	/// </summary>
	public TextGradient Gradient
	{
		get
		{
			if (gradient == null)
			{
				gradient = GetComponent<TextGradient>();
				if (gradient == null)
				{
					gradient = gameObject.AddComponent<TextGradient>();
				}
			}
			return gradient;
		}
	}

	/// <summary>
	/// 设置渐变颜色
	/// </summary>
	public void SetGradientColor(Color source, Color target)
	{
		Gradient.sourceColor = source;
		Gradient.targetColor = target;
	}
}
