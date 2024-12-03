using App;
using System;
using UIEngine;
using UnityEngine;

public class ResourceInstanceItem : IObjectPoolItem, IUILoadAssetItem
{
	private UDebugger debugger = new UDebugger("ResourceInstanceItem", UDebugger.SYSTEM_LEVEL);

	public Action<ResourceInstanceItem> onReuse;
	public Action<ResourceInstanceItem> onRecycle;
	public Action<ResourceInstanceItem> onDestroy;
	public Action<ResourceInstanceItem> onRecycledLoadDone;

	public string assetPath { private set; get; }
	public bool loadDone { private set; get; }

	object IUILoadAssetItem.content => gameObject;

	string IUILoadAssetItem.path => assetPath;
	bool IUILoadAssetItem.loadDone => loadDone;
	void IUILoadAssetItem.Recycle() => Recycle();

	public override string ToString()
	{
		string printPath = assetPath;
		if (printPath == null)
		{
			printPath = string.Empty;
		}
		return GetHashCode() + " ,path:" + printPath;
	}

	private IUILoadAsset loadAsset;
	public void Init(IUILoadAsset loadAsset)
	{
		this.loadAsset = loadAsset;
	}

	private Action<ResourceInstanceItem> onLoadDone;
	private IUILoadAssetItem loadAssetItem;
	public void Load(string assetPath, Action<ResourceInstanceItem> onLoadDone)
	{
		if (loadAssetItem != null)
		{
			OnRecycle();
		}
		loadDone = false;
		this.assetPath = assetPath;
		this.onLoadDone = onLoadDone;
		loadAssetItem = loadAsset.LoadAsset(assetPath, UIAssetType.Instance, OnLoadDone);
	}

	private bool active = true;
	public void SetActive(bool active)
	{
		this.active = active;

		UpdateActive();
	}

	private void UpdateActive()
	{
		if (gameObject != null)
		{
			gameObject.SetActive(active);
		}
	}

	public GameObject gameObject { private set; get; }
	private void OnLoadDone(IUILoadAssetItem item)
	{
		gameObject = item.content as GameObject;

		loadDone = true;

		UpdateActive();

		var callback = onLoadDone;
		onLoadDone = null;
		callback?.Invoke(this);
	}

	public void OnReuse()
	{
		onReuse?.Invoke(this);
	}

	public void OnRecycle()
	{
		if (loadAssetItem != null)
		{
			loadAssetItem.Recycle();
			loadAssetItem = null;
		}

		gameObject = null;

		onLoadDone = null;

		loadDone = false;

		active = true;
	}

	public void Recycle()
	{
		onLoadDone = onRecycledLoadDone;

		active = false;

		onRecycle?.Invoke(this);
	}

	public void Destory()
	{
		onDestroy?.Invoke(this);
	}
}
