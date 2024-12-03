using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

/// <summary>
/// 非生成的类继承可设置精灵
/// </summary>
public class LoadAssetComponent
{
	/// <summary>
	/// 加载委托定义
	/// </summary>
	public delegate IUILoadAssetItem LoadAssetAction(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone);

	/// <summary>
	/// 加载委托
	/// </summary>
	protected LoadAssetAction LoadAsset = null;

	/// <summary>
	/// MainModule
	/// </summary>
	protected MainModule module = null;

	/// <summary>
	/// 组装路径加载精灵
	/// </summary>
	public void LoadSprite(Image image, string spritePath, string spriteName, Action<Image> loadDone = null)
	{
		if (string.IsNullOrEmpty(spriteName))
		{
			LoadSprite(image, string.Empty, loadDone);
		}
		else
		{
			string path = AssetPathHelper.ParseSpritePath(spritePath, spriteName);
			LoadSprite(image, path, loadDone);
		}
	}

	/// <summary>
	/// 完整路径加载精灵
	/// </summary>
	public void LoadSprite(Image image, string texturePath, Action<Image> loadDone = null)
	{
		if (image != null)
		{
			// 通用空图标
			if (string.IsNullOrEmpty(texturePath))
			{
				texturePath = AssetPathHelper.ParseSpritePath(AssetPathHelper.CommonSpritePath, "empty");
			}

			// 加载图标
			LoadAsset?.Invoke(texturePath, UIAssetType.Sprite, (assetItem) =>
			{
				var sprite = (assetItem.content as Sprite);
				image.sprite = sprite;
				loadDone?.Invoke(image);
			});
		}
	}

	/// <summary>
	/// 设置RawImage
	/// </summary>
	/// <param name="rawImage"></param>
	/// <param name="texturePath"></param>
	/// <param name="loadDone"></param>
	protected void SetRawImageTexture(RawImage rawImage, string texturePath, Action<RawImage> loadDone = null)
	{
		LoadAsset?.Invoke(texturePath, UIAssetType.Texture, (item) =>
		{
			if (rawImage != null)
			{
				rawImage.texture = item.content as Texture2D;
				loadDone?.Invoke(rawImage);
			}
		});
	}

	/// <summary>
	/// 设置委托
	/// </summary>
	protected void SetLoadAssetAction(LoadAssetAction action, MainModule module)
	{
		LoadAsset = action;
		this.module = module;
	}
}
