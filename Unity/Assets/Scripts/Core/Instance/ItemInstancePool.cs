using App;
using System;
using System.Collections.Generic;
using UIEngine;
using UnityEngine;

public class ItemInstancePool
{
	private UDebugger debugger = new UDebugger("ItemInstancePool");

	private GameObject go;
	private Transform parent;
	private bool init = false;
	public void Init(GameObject go, Transform parent, int increaseNum = 1)
	{
		if (!init)
		{
			init = true;

			this.go = go;
			this.parent = parent;

			if (increaseNum < 1)
			{
				increaseNum = 1;
			}

			IncreaseItems(increaseNum);
		}
	}

	private IUILoadAssetItem loadAssetItem = null;
	public void LoadInit(IUILoadAsset loadAsset, string assetPath, Transform parent, Action<ItemInstancePool> onLoadDone, int increaseNum = 1)
	{
		if (!init)
		{
			if (loadAsset != null)
			{
				if (loadAssetItem != null)
				{
					Clear();
				}

				loadAssetItem = loadAsset.LoadAsset(assetPath, UIAssetType.Resource, (item) =>
				{
					Init(item.content as GameObject, parent, increaseNum);

					onLoadDone?.Invoke(this);
				});
			}
		}
	}

	private Queue<GameObject> emptyList = new Queue<GameObject>();
	private List<GameObject> allList = new List<GameObject>();

	public GameObject Get()
	{
		if (emptyList.Count <= 0)
		{
			IncreaseItems(1);
		}

		GameObject go = emptyList.Dequeue();
		go.SetActive(true);
		return go;
	}

	public void Recycle(GameObject go)
	{
		emptyList.Enqueue(go);
	}

	private void IncreaseItems(int increaseNum)
	{
		for (int i = 0; i < increaseNum; i++)
		{
			GameObject instance = GameObject.Instantiate(go);
			instance.SetActive(false);
			if (parent != null)
			{
				instance.transform.SetParent(parent);
			}
			emptyList.Enqueue(instance);
			allList.Add(instance);
		}
	}

	public void Clear()
	{
		if (loadAssetItem != null)
		{
			loadAssetItem.Recycle();
			loadAssetItem = null;
		}

		init = false;

		for (int i = 0; i < allList.Count; i++)
		{
			GameObject.Destroy(allList[i]);
		}
		emptyList.Clear();
		allList.Clear();
	}
}
