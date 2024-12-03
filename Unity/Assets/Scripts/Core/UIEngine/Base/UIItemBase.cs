using UnityEngine;
using UnityEngine.UI;
using System;
using UIEngine;
using App;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using UnityEngine.U2D;
using System.ComponentModel;

public abstract class UIItemBase : IScriptInstance
{
	abstract public void ClearComponentFields();

	protected Transform transform { set; get; }
	protected GameObject gameObject { set; get; }
	private IUILoadAsset loadAsset;
	protected AudioManager audio;
	protected UIViewBase viewBase;
	private GameObject _go;
	public GameObject GetGameObject()
	{
		return _go;
	}

	public void SetGameObject(GameObject go)
	{
		_go = go;
	}

	protected void InitBind(Transform itemTransform, UIViewBase view)
	{
		ClearLoadAssetItems();
		ClearComponentFields();

		SelfBind(itemTransform, view);
	}

	protected void SelfBind(Transform itemTransform, UIViewBase view)
	{
		this.viewBase = view;
		view.SetLoadAsset(ref loadAsset);
		audio = view.audio;
		this.transform = itemTransform;
		if (transform != null)
		{
			this.gameObject = transform.gameObject;
		}
	}

	private Dictionary<Object, IUILoadAssetItem> loadItemsDic = new Dictionary<Object, IUILoadAssetItem>();
	private List<IUILoadAssetItem> tempAssetList = new List<IUILoadAssetItem>();

	public IUILoadAssetItem LoadAsset(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone)
	{
		IUILoadAssetItem item = loadAsset.LoadAsset(assetPath, assetType, onLoadDone);
		if (item != null)
		{
			tempAssetList.Add(item);
		}
		return item;
	}

	public bool CheckPreShowDone()
	{
		for (int i = 0; i < tempAssetList.Count; i++)
		{
			if (!tempAssetList[i].loadDone)
			{
				return false;
			}
		}

		return true;
	}

	protected void ClearLoadAssets()
	{
		for (int i = 0; i < tempAssetList.Count; i++)
		{
			tempAssetList[i].Recycle();
		}
		tempAssetList.Clear();
	}

	protected void LoadAsset(Object component, string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone)
	{
		ClearAsset(component);

		loadItemsDic[component] = loadAsset.LoadAsset(assetPath, assetType, onLoadDone);
	}

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
				texturePath = "Assets/Bundles/Texture2D/Icon/Item/Level_icon_unknown.png";
			}

			// 加载图标
			LoadSprite(texturePath, (sprite) =>
			{
				image.sprite = sprite;
				loadDone?.Invoke(image);
			});
		}
	}

	/// <summary>
	/// 组装路径加载精灵
	/// </summary>
	public void LoadSprite(string spritePath, string spriteName, Action<Sprite> action)
	{
		if (!string.IsNullOrEmpty(spriteName))
		{
			string path = AssetPathHelper.ParseSpritePath(spritePath, spriteName); ;
			LoadSprite(path, action);
		}
	}

	/// <summary>
	/// 加载精灵
	/// </summary>
	public void LoadSprite(string texturePath, Action<Sprite> action)
	{
		if (!string.IsNullOrEmpty(texturePath))
		{
			LoadAsset(texturePath, UIAssetType.Sprite, (assetItem) =>
			{
				Sprite sprite = (assetItem.content as Sprite);
				action?.Invoke(sprite);
			});
		}
		else
		{
			action?.Invoke(null);
		}
	}

	/// <summary>
	/// 通过图集加载Sprite
	/// </summary>
	/// <param name="atlasPath">图集路径</param>
	/// <param name="spriteName">图片名字不带后缀</param>
	/// <param name="loadDone"></param>
	public void LoadSpriteByAtlas(string atlasPath,string spriteName, Action<Sprite> loadDone = null) 
	{
		LoadAsset(atlasPath, UIAssetType.Atlas, (data) =>
		{
			SpriteAtlas atlas = (SpriteAtlas)data.content;
			loadDone?.Invoke(atlas.GetSprite(spriteName));
		});
	}

	/// <summary>
	/// 通过图集加载Sprite
	/// </summary>
	/// <param name="atlasPath">图集路径</param>
	/// <param name="spriteName">图片名字不带后缀</param>
	/// <param name="loadDone"></param>
	public void LoadSpriteByAtlas(string atlasPath, string spriteName, Image img)
	{
		LoadAsset(atlasPath, UIAssetType.Atlas, (data) =>
		{
			SpriteAtlas atlas = (SpriteAtlas)data.content;
			if (img != null) 
			{
				img.sprite = atlas.GetSprite(spriteName);
			}
		});
	}

	protected void SetRawImageTexture(RawImage rawImage, string texturePath, Action<RawImage> loadDone = null)
	{
		LoadAsset(rawImage, texturePath, UIAssetType.Texture, (item) =>
		{
			if (rawImage != null)
			{
				rawImage.texture = item.content as Texture2D;
				loadDone?.Invoke(rawImage);
			}
		});
	}

	protected void ClearAsset(Object component)
	{
		IUILoadAssetItem loadItem = null;
		if (loadItemsDic.TryGetValue(component, out loadItem))
		{
			if (loadItem != null)
			{
				loadItem.Recycle();
			}
			loadItemsDic.Remove(component);
		}
	}

	protected void ClearLoadAssetItems()
	{
		foreach (var item in loadItemsDic)
		{
			if (item.Value != null)
			{
				item.Value.Recycle();
			}
		}
		loadItemsDic.Clear();
	}
}
