using System;
using System.Collections.Generic;
using UnityEngine;

public class SubKeyDic<T> where T : class, new()
{
	private Dictionary<int, Dictionary<int, List<T>>> itemDic;
	private static List<List<T>> cacheList;

	public SubKeyDic(int capacity)
	{
		itemDic = new Dictionary<int, Dictionary<int, List<T>>>(capacity);
		cacheList = new List<List<T>>(capacity);
	}

	public void Add(int key, int subKey, T item)
	{
		Dictionary<int, List<T>> dic = null;
		if (!itemDic.TryGetValue(key, out dic))
		{
			dic = new Dictionary<int, List<T>>();
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
				list = new List<T>();
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

	public List<T> GetList(int key, int subKey)
	{
		List<T> list = null;
		Dictionary<int, List<T>> dic = null;
		if (itemDic.TryGetValue(key, out dic))
		{
			dic.TryGetValue(subKey, out list);
		}
		return list;
	}

	public T Get(int key, int subKey, bool autoRemove = true)
	{
		Dictionary<int, List<T>> dic = null;
		if (!itemDic.TryGetValue(key, out dic))
		{
			dic = new Dictionary<int, List<T>>();
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

	public bool Remove(int key, int subKey, T item)
	{
		Dictionary<int, List<T>> dic = null;
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
