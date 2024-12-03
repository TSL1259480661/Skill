using System;
using System.Collections.Generic;
public abstract class FactoryBase<K, T> where T : class
{
	abstract public void OnItemInit(T item, K itemType);

	abstract public void OnTypeDicInit();

	private int tCapacity;
	public FactoryBase(int uCapacity, int tCapacity)
	{
		InitFactoryBase(uCapacity);
		this.tCapacity = tCapacity;
	}

	public void InitFactoryBase(int capacity)
	{
		itemDic = new Dictionary<K, List<T>>(capacity);
		typeDic = new Dictionary<K, Type>(capacity);
		uDic = new Dictionary<Type, K>(capacity);
	}

	private Dictionary<K, List<T>> itemDic;

	public T Get(K itemType)
	{
		T item = default(T);

		List<T> list = null;
		if (itemDic.TryGetValue(itemType, out list))
		{
			if (list.Count > 0)
			{
				T triggerBase = list[list.Count - 1];
				list.RemoveAt(list.Count - 1);
				item = triggerBase;
			}
		}

		if (item == null)
		{
			item = OnCreate(itemType);
		}

		OnItemInit(item, itemType);

		return item;
	}

	protected Dictionary<K, Type> typeDic;
	protected Dictionary<Type, K> uDic;

	private bool hasInit = false;

	protected T OnCreate(K itemType)
	{
		if (!hasInit)
		{
			hasInit = true;
			OnTypeDicInit();

			foreach (var item in typeDic)
			{
				uDic[item.Value] = item.Key;
			}
		}

		Type type = null;
		if (typeDic.TryGetValue(itemType, out type))
		{
			return Activator.CreateInstance(type) as T;
		}
		return null;
	}

	public void Recycle(K key, T item)
	{
		List<T> list = null;
		if (!itemDic.TryGetValue(key, out list))
		{
			list = new List<T>(tCapacity);
			itemDic[key] = list;
		}
		list.Add(item);
	}
}
