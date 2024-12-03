using System;
using System.Collections.Generic;
using UnityEngine;

public class UObjectPool<T> where T : IObjectPoolItem, new()
{
	private List<T> emptyList;
	private List<T> recycleList;
	private Dictionary<T, bool> allDic;

	private Func<T> onCreate;

	private T defaultT;

	public UObjectPool(int capacity, Func<T> onCreate = null)
	{
		if (onCreate == null)
		{
			this.onCreate = DefaultOnCreate;
		}
		else
		{
			this.onCreate = onCreate;
		}

		defaultT = default(T);
		emptyList = new List<T>(capacity);
		recycleList = new List<T>(capacity);
		allDic = new Dictionary<T, bool>(capacity);
	}

	public UObjectPool()
	{
		this.onCreate = DefaultOnCreate;
		defaultT = default(T);
		emptyList = new List<T>();
		recycleList = new List<T>();
		allDic = new Dictionary<T, bool>();
	}

	protected T DefaultOnCreate()
	{
		return new T();
	}

	public UObjectPool(Func<T> onCreate)
	{
		if (onCreate == null)
		{
			this.onCreate = DefaultOnCreate;
		}
		else
		{
			this.onCreate = onCreate;
		}

		defaultT = default(T);
	}

	public void InCrease(int increaseNum)
	{
		T item = defaultT;
		for (int i = emptyList.Count; i < increaseNum; i++)
		{
			if (this.onCreate != null)
			{
				item = this.onCreate();
				allDic[item] = true;
			}
			emptyList.Add(item);
		}
	}

	public T Get()
	{
		T item = defaultT;
		if (emptyList.Count > 0)
		{
			int index = emptyList.Count - 1;
			item = emptyList[index];
			emptyList.RemoveAt(index);
		}
		else
		{
			if (this.onCreate != null)
			{
				item = this.onCreate();
				allDic[item] = true;
			}
		}

		if (item != null)
		{
			item.OnReuse();
		}
		else
		{
			Debug.LogError("UObjectPool get item is null:" + typeof(T));
		}

		return item;
	}

	public void Recycle(T item)
	{
		if (!item.Equals(defaultT))
		{
			if (allDic.ContainsKey(item))
			{
				item.OnRecycle();

#if UNITY_EDITOR
				if (emptyList.Contains(item))
				{
					Debug.LogError("UObjectPool Recycle item repeat recycle ,poolName:" + typeof(T) + " ,item:" + item);
				}
				else
				{
					emptyList.Add(item);
				}
#else
					emptyList.Add(item);
#endif
			}
			else
			{
				Debug.LogError("UObjectPool Recycle item is not from self poolName:" + typeof(T));
			}
		}
	}

	public void RecycleAll()
	{
		foreach (var item in allDic)
		{
			recycleList.Add(item.Key);
		}

		for (int i = 0; i < recycleList.Count; i++)
		{
			Recycle(recycleList[i]);
		}

		recycleList.Clear();
	}
}
