using App;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SubKeyList<K, subK, T> where T : class, new()
{
	private Dictionary<K, Dictionary<subK, List<T>>> itemDic;
	private List<List<T>> cacheList;

	private int subCapacity;
	public SubKeyList(int capacity, int subCapacity)
	{
		itemDic = new Dictionary<K, Dictionary<subK, List<T>>>(capacity);
		cacheList = new List<List<T>>(capacity);
		this.subCapacity = subCapacity;
	}

	public void Add(K key, subK subKey, T item)
	{
		Dictionary<subK, List<T>> dic = null;
		if (!itemDic.TryGetValue(key, out dic))
		{
			dic = new Dictionary<subK, List<T>>(subCapacity);
			itemDic[key] = dic;
		}

		List<T> list = null;
		if (!dic.TryGetValue(subKey, out list))
		{
			if (cacheList.Count > 0)
			{
				list = cacheList[cacheList.Count - 1];
				cacheList.RemoveAt(cacheList.Count - 1);
			}
			else
			{
				list = new List<T>(subCapacity);
			}
			dic[subKey] = list;
		}

#if UNITY_EDITOR
		if (list.Contains(item))
		{
			Debug.LogError("SubKeyDic<T> Recycle item repeat recycle ,type:" + this.GetType() + " ,item:" + item);
		}
		else
		{
			list.Add(item);
		}
#else
		list.Add(item);
#endif
	}

	public List<T> GetList(K key, subK subKey)
	{
		List<T> list = null;
		Dictionary<subK, List<T>> dic = null;
		if (itemDic.TryGetValue(key, out dic))
		{
			dic.TryGetValue(subKey, out list);
		}
		return list;
	}

	public T Get(K key, subK subKey, bool autoRemove = true)
	{
		Dictionary<subK, List<T>> dic = null;
		if (!itemDic.TryGetValue(key, out dic))
		{
			dic = new Dictionary<subK, List<T>>(subCapacity);
			itemDic[key] = dic;
		}

		List<T> list = null;
		if (dic.TryGetValue(subKey, out list))
		{
			if (list.Count > 0)
			{
				if (autoRemove)
				{
					T item = list[list.Count - 1];
					list.RemoveAt(list.Count - 1);
					return item;
				}
				else
				{
					return list[0];
				}
			}
		}

		return null;
	}

	public void ForEach(Action<T> onEach)
	{
		foreach (var item in itemDic)
		{
			foreach (var item1 in item.Value)
			{
				for (int i = 0; i < item1.Value.Count; i++)
				{
					onEach?.Invoke(item1.Value[i]);
				}
			}
		}
	}

	public bool Remove(K key, subK subKey, T item)
	{
		Dictionary<subK, List<T>> dic = null;
		if (itemDic.TryGetValue(key, out dic))
		{
			List<T> list = null;
			if (dic.TryGetValue(subKey, out list))
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
		}
		return false;
	}

	public void Clear(Action<T> callback)
	{
		foreach (var item in itemDic)
		{
			foreach (var dicItem in item.Value)
			{
				for (int i = 0; i < dicItem.Value.Count; i++)
				{
					callback?.Invoke(dicItem.Value[i]);
				}

				dicItem.Value.Clear();

				cacheList.Add(dicItem.Value);
			}
			item.Value.Clear();
		}
	}
}
