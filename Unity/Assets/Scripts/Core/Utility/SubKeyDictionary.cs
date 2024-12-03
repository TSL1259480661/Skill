using App;
using System;
using System.Collections.Generic;

public class SubKeyDictionary<K, subK, T> where T : class, new()
{
	private static UDebugger debugger = new UDebugger("SubKeyDictionary<K, subK, T>");

	private Dictionary<K, Dictionary<subK, T>> itemDic;
	private List<T> cacheList;

	private int subCapacity;
	public SubKeyDictionary(int capacity, int subCapacity)
	{
		itemDic = new Dictionary<K, Dictionary<subK, T>>(capacity);
		cacheList = new List<T>(capacity);
		this.subCapacity = subCapacity;
	}

	public void Add(K key, subK subKey, T item)
	{
		Dictionary<subK, T> dic = null;
		if (!itemDic.TryGetValue(key, out dic))
		{
			dic = new Dictionary<subK, T>(subCapacity);
			itemDic[key] = dic;
		}

		if (!dic.ContainsKey(subKey))
		{
			dic[subKey] = item;
			cacheList.Add(item);
		}
		else
		{
			debugger.LogErrorFormat("Add item repeat ,type:{0} ,K:{2} ,subK:{3} ,item:{1}", this.GetType(), key, subKey, item);
		}
	}

	public List<T> GetList()
	{
		return cacheList;
	}

	public T Get(K key, subK subKey)
	{
		Dictionary<subK, T> dic = null;
		if (itemDic.TryGetValue(key, out dic))
		{
			T item;
			if (dic.TryGetValue(subKey, out item))
			{
				return item;
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
				onEach?.Invoke(item1.Value);
			}
		}
	}

	public T Remove(K key, subK subKey)
	{
		Dictionary<subK, T> dic = null;
		if (itemDic.TryGetValue(key, out dic))
		{
			T item = null;
			if (dic.TryGetValue(subKey, out item))
			{
				dic.Remove(subKey);
				return item;
			}
		}
		return null;
	}

	public void Clear(Action<T> callback)
	{
		foreach (var item in itemDic)
		{
			foreach (var dicItem in item.Value)
			{
				callback?.Invoke(dicItem.Value);
			}
			item.Value.Clear();
		}

		cacheList.Clear();
	}
}
