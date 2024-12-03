using UnityEngine;
using UnityEngine.UI;

public class GImage : Image, IGray
{
	[Tooltip("置灰时的图片")]
	public Sprite graySprite = null;
	[Tooltip("使用置灰Shader")]
	public Material grayMaterial;
	[Tooltip("置灰时的颜色")]
	[SerializeField]
	public Color grayColor = Color.gray;
	protected Color oldColor = Color.white;

	private bool _isGray;
	public bool IsGray
	{
		get => _isGray;
		set
		{
			if (color.a == 0) return;
			if (graySprite != null)
			{
				this.overrideSprite = value ? graySprite : null;
			}
			else if (grayMaterial != null)
			{
				//YooAsset.YooAssets.LoadAssetAsync<Material>("");
				material = value ? grayMaterial : null;
			}
			else
			{
				if (_isGray != value)
				{
					if (_isGray == false && value)
						oldColor = color;
					color = value ? grayColor : oldColor;
				}
			}
			_isGray = value;
		}
	}

	UIItemBase viewBase;

	public void InitView(UIItemBase view)
	{
		viewBase = view;
	}

	/// <summary>
	/// 纯路径组装加载
	/// </summary>
	public void LoadSprite(string spritePath, string sprtieName, bool nativeSize = false)
	{
		viewBase.LoadSprite(this, spritePath, sprtieName, (image) =>
		{
			if (nativeSize)
			{
				SetNativeSize();
			}
		});
	}

	/// <summary>
	/// 完整路径加载
	/// </summary>
	public void LoadSprite(string spritePath, bool nativeSize = false)
	{
		viewBase?.LoadSprite(this, spritePath, (image) =>
		{
			if (nativeSize)
			{
				SetNativeSize();
			}
		});
	}
}
