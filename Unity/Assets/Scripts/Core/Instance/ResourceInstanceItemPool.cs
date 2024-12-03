using App;
using System;
using System.Collections.Generic;
using UIEngine;
using UnityEngine;

public class ResourceInstanceItemPool
{
	private UDebugger debugger = new UDebugger("ResourceInstanceItemPool", UDebugger.SYSTEM_LEVEL);

	private static UObjectPool<ResourceInstanceItem> pool = new UObjectPool<ResourceInstanceItem>();

	private Dictionary<string, List<ResourceInstanceItem>> recycleInstanceList = new Dictionary<string, List<ResourceInstanceItem>>();

	private IUILoadAsset loadAsset;
	private Transform rootTransform;
	public ResourceInstanceItemPool(IUILoadAsset loadAsset)
	{
		this.loadAsset = loadAsset;

		var go = new GameObject("ResourceInstanceItemPool");
		rootTransform = go.transform;
		rootTransform.position = new Vector3(0f, 0f, 0f);
		GameObject.DontDestroyOnLoad(go);
	}

	public ResourceInstanceItem Create(string assetPath, Action<ResourceInstanceItem> onLoadDone)
	{
		List<ResourceInstanceItem> list = null;
		if (recycleInstanceList.TryGetValue(assetPath, out list))
		{
			if (list.Count > 0)
			{
				ResourceInstanceItem item = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				item.SetActive(true);
				if (item.loadDone)
				{
					onLoadDone?.Invoke(item);
				}
				return item;
			}
		}

		ResourceInstanceItem instanceItem = pool.Get();
		instanceItem.onRecycle = OnRecycle;
		instanceItem.onDestroy = OnDestroy;
		instanceItem.onRecycledLoadDone = OnRecycledItemLoadDone;

		instanceItem.Init(loadAsset);
		instanceItem.Load(assetPath, onLoadDone);
		return instanceItem;
	}

	public void Preload(string assetPath, int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			ResourceInstanceItem instanceItem = pool.Get();
			instanceItem.onRecycle = OnRecycle;
			instanceItem.onDestroy = OnDestroy;
			instanceItem.onRecycledLoadDone = OnRecycledItemLoadDone;

			instanceItem.Init(loadAsset);
			instanceItem.Load(assetPath, OnPreloadDone);
		}
	}

	private Queue<ResourceInstanceItem> preloadList = new Queue<ResourceInstanceItem>();
	private void OnPreloadDone(ResourceInstanceItem item)
	{
		preloadList.Enqueue(item);
	}

	public void Update()
	{
		if (preloadList.Count > 0)
		{
			OnRecycle(preloadList.Dequeue());
		}
	}

	private void OnRecycledItemLoadDone(ResourceInstanceItem item)
	{
		if (item != null && item.gameObject != null)
		{
			item.gameObject.transform.SetParent(rootTransform);
		}
	}

	private void OnRecycle(ResourceInstanceItem item)
	{
		if (item != null)
		{
			List<ResourceInstanceItem> list = null;
			if (!recycleInstanceList.TryGetValue(item.assetPath, out list))
			{
				list = new List<ResourceInstanceItem>();
				recycleInstanceList[item.assetPath] = list;
			}

			if (item.gameObject != null)
			{
				//item.gameObject.transform.SetParent(Entity.objectRoot.transform);
			}

			item.SetActive(false);

#if UNITY_EDITOR
			if (list.Contains(item))
			{
				Debug.LogError("ResourceInstanceItemPool Recycle item repeat recycle ,item:" + item.assetPath);
			}
			else
			{
				list.Add(item);
			}
#else
			list.Add(item);
#endif
		}
		else
		{
			debugger.LogError("item is null");
		}
	}

	private void OnDestroy(ResourceInstanceItem item)
	{
		if (item != null)
		{
			pool.Recycle(item);
		}
	}

	public void DestroyRecycledInstances()
	{
		foreach (var item in recycleInstanceList)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				item.Value[i].Destory();
			}
			item.Value.Clear();
		}

		loadAsset.UnloadUnusedAssets();
	}

	//public void Clear()
	//{
	//	Clear(null);
	//}

	//public void Clear(string assetPath)
	//{
	//	if (string.IsNullOrEmpty(assetPath))
	//	{
	//		foreach (var item in instanceList)
	//		{
	//			for (int i = 0; i < item.Value.Count; i++)
	//			{
	//				pool.Recycle(item.Value[i]);
	//			}
	//			item.Value.Clear();
	//		}
	//	}
	//	else
	//	{
	//		List<ResourceInstanceItem> list = null;
	//		if (instanceList.TryGetValue(assetPath, out list))
	//		{
	//			for (int i = 0; i < list.Count; i++)
	//			{
	//				pool.Recycle(list[i]);
	//			}
	//			list.Clear();
	//		}
	//	}
	//}
}
