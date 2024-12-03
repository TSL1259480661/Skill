using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GText : Text, IGray
{
	[SerializeField]
	[Tooltip("置灰时的颜色")]
	public Color grayColor = new Color(0.6f, 0.6f, 0.6f, 1f);
	protected Color oldColor = Color.white;
	//[Tooltip("使用置灰Shader")]
	//public bool grayMaterial;
	private static string InitKey = " ";

	public GText()
	{
		m_Text = InitKey;
	}

	private bool _isGray;
	public bool IsGray
	{
		get => _isGray;
		set
		{
			if (color.a == 0) return;

			if (_isGray != value)
			{
				if (_isGray == false && value)
					oldColor = color;
				color = value ? grayColor : oldColor;
			}

			_isGray = value;
		}
	}

	public void SetHexColor(string hex)
	{
		if (ColorUtility.TryParseHtmlString(hex, out Color _color))
		{
			color = _color;
		}
		else
		{
			Debug.LogWarning("输入的颜色异常:" + hex);
		}
	}
}
