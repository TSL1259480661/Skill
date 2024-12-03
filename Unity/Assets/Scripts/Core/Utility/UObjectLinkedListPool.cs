using System;
using UnityEngine;

public class UObjectLinkedListPool<T> where T : LinkedItemNode, IObjectPoolItem, new()
{
	private LinkedItemList emptyList;

	private Func<T> onCreate;

	private T defaultT;

	public UObjectLinkedListPool(Func<T> onCreate = null)
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
		emptyList = new LinkedItemList();
	}

	public UObjectLinkedListPool()
	{
		this.onCreate = DefaultOnCreate;
		defaultT = default(T);
		emptyList = new LinkedItemList();
	}

	protected T DefaultOnCreate()
	{
		return new T();
	}

	public void InCrease(int increaseNum)
	{
		T item = defaultT;
		for (int i = emptyList.Count; i < increaseNum; i++)
		{
			if (this.onCreate != null)
			{
				item = this.onCreate();
			}
			emptyList.AddLast(item);
		}
	}

	public T Get()
	{
		T item = defaultT;
		if (emptyList.Count > 0)
		{
			var last = emptyList.First;
			emptyList.Delink(last);
			return last as T;
		}
		else
		{
			if (this.onCreate != null)
			{
				item = this.onCreate();
			}
		}

		if (!item.Equals(defaultT))
		{
			item.OnReuse();
		}
		else
		{
			Debug.LogError("UObjectLinkedListPool get item is null:" + typeof(T));
		}

		return item;
	}

	public void Recycle(T item)
	{
		if (!item.Equals(defaultT))
		{
			item.OnRecycle();

			emptyList.AddLast(item);
		}
	}
}
