using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyList<K, T> where T : class, new()
{
	private static List<List<T>> cacheList = new List<List<T>>(100);

	private Dictionary<K, List<T>> itemDic;

	private int subCapacity;
	public KeyList(int capacity, int subCapacity)
	{
		itemDic = new Dictionary<K, List<T>>(capacity);

		this.subCapacity = subCapacity;
	}

	private List<T> CreateList()
	{
		List<T> list = null;
		if (cacheList.Count > 0)
		{
			list = cacheList[cacheList.Count - 1];
			cacheList.RemoveAt(cacheList.Count - 1);
		}
		else
		{
			list = new List<T>(subCapacity);
		}
		return list;
	}

	public void Add(K key, T item)
	{
		List<T> list = null;
		if (!itemDic.TryGetValue(key, out list))
		{
			list = CreateList();

			itemDic[key] = list;
		}

#if UNITY_EDITOR
		if (list.Contains(item))
		{
			Debug.LogError("KeyList<T> Recycle item repeat recycle ,type:" + this.GetType() + " ,item:" + item);
		}
		else
		{
			list.Add(item);
		}
#else
		list.Add(item);
#endif
	}

	public List<T> GetList(K key)
	{
		List<T> list = null;
		itemDic.TryGetValue(key, out list);
		return list;
	}

	public int GetCount(K key)
	{
		List<T> list = null;
		if (itemDic.TryGetValue(key, out list))
		{
			return list.Count;
		}
		return 0;
	}

	public T Get(K key)
	{
		List<T> list = null;
		if (itemDic.TryGetValue(key, out list))
		{
			if (list.Count > 0)
			{
				T item = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				return item;
			}
		}

		return null;
	}

	public void ForEach(Action<T> onEach)
	{
		foreach (var item in itemDic)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				onEach?.Invoke(item.Value[i]);
			}
		}
	}

	public bool Remove(K key, T item)
	{
		List<T> list = null;
		if (itemDic.TryGetValue(key, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == item)
				{
					list.RemoveAt(i);
					return true;
				}
			}
		}
		return false;
	}

	public void CopyFrom(KeyList<K, T> other)
	{
		if (other != null)
		{
			foreach (var dicItem in other.itemDic)
			{
				var list = CreateList();
				itemDic.Add(dicItem.Key, list);

				for (int i = 0; i < dicItem.Value.Count; i++)
				{
					list.Add(dicItem.Value[i]);
				}
			}
		}
	}

	public void Clear(Action<T> callback)
	{
		foreach (var dicItem in itemDic)
		{
			for (int i = 0; i < dicItem.Value.Count; i++)
			{
				callback?.Invoke(dicItem.Value[i]);
			}

			dicItem.Value.Clear();

			cacheList.Add(dicItem.Value);
		}
		itemDic.Clear();
	}
}
