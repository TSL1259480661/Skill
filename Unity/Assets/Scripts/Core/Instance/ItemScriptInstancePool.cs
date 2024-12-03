using App;
using System;
using System.Collections.Generic;
using UIEngine;
using UnityEngine;

public interface IScriptInstance
{
	public GameObject GetGameObject();
	public void SetGameObject(GameObject go);
}

public class ItemScriptInstancePool<T> where T : IObjectPoolItem, IScriptInstance, new()
{
	private UDebugger debugger = new UDebugger("ItemScriptInstancePool");

	private ItemInstancePool goPool = new ItemInstancePool();

	private static UObjectPool<T> scriptPool = new UObjectPool<T>();

	private Action<GameObject, T> onCreate;
	private Action<T> onRecycle;

	private bool init = false;

	public void Init(GameObject go, Transform parent, Action<GameObject, T> onCreate, Action<T> onRecycle, int increaseNum = 1)
	{
		if (!init)
		{
			init = true;

			this.onCreate = onCreate;
			this.onRecycle = onRecycle;

			goPool.Init(go, parent, increaseNum);

			Increase(increaseNum);
		}
	}

	private List<T> emptyList = new List<T>();
	private List<T> allList = new List<T>();

	public void Increase(int increaseNum)
	{
		for (int i = 0; i < increaseNum; i++)
		{
			GameObject go = goPool.Get();
			T script = scriptPool.Get();
			script.SetGameObject(go);
			onCreate?.Invoke(go, script);
			emptyList.Add(script);
			allList.Add(script);
		}
	}

	public void Recycle(T script)
	{
		if (script != null)
		{
			emptyList.Add(script);
		}
	}

	public T Get()
	{
		if (emptyList.Count <= 0)
		{
			Increase(1);
		}

		T script = emptyList[emptyList.Count - 1];
		emptyList.RemoveAt(emptyList.Count - 1);
		return script;
	}

	private IUILoadAssetItem loadAssetItem = null;
	public void LoadInit(IUILoadAsset loadAsset, string assetPath, Transform parent, Action<GameObject, T> onCreate, Action<T> onRecycle, Action<ItemScriptInstancePool<T>> onLoadDone, int increaseNum = 1)
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
					Init(item.content as GameObject, parent, onCreate, onRecycle, increaseNum);

					onLoadDone?.Invoke(this);
				});
			}
		}
	}

	public void RecycleAll()
	{
		emptyList.Clear();
		for (int i = 0; i < allList.Count; i++)
		{
			onRecycle?.Invoke(allList[i]);

			goPool.Recycle(allList[i].GetGameObject());

			scriptPool.Recycle(allList[i]);
		}
		emptyList.AddRange(allList);
	}

	public void Clear()
	{
		if (loadAssetItem != null)
		{
			loadAssetItem.Recycle();
			loadAssetItem = null;
		}

		init = false;

		goPool.Clear();

		for (int i = 0; i < allList.Count; i++)
		{
			scriptPool.Recycle(allList[i]);
		}

		emptyList.Clear();
		allList.Clear();
	}
}
