using System;
using System.Collections.Generic;

public interface IKeyValueListItem
{
	int GetKey();
	void Recycle();
}

public class KeyValueList<T> where T : IKeyValueListItem
{
	protected List<T> list;
	protected Dictionary<int, T> dic;

	public KeyValueList(int capacity)
	{
		list = new List<T>(capacity);
		dic = new Dictionary<int, T>(capacity);
	}

	public void Add(T item)
	{
		if (item != null)
		{
			dic[item.GetKey()] = item;
			list.Add(item);
		}
	}

	public bool Remove(int sId)
	{
		if (dic.ContainsKey(sId))
		{
			dic.Remove(sId);
		}

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].GetKey() == sId)
			{
				T item = list[i];
				list.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public void ForEach(Action<T> onEach)
	{
		for (int i = 0; i < list.Count; i++)
		{
			onEach?.Invoke(list[i]);
		}
	}

	public T Get(int sId)
	{
		T item = default(T);
		if (dic.TryGetValue(sId, out item))
		{
			return item;
		}
		return item;
	}

	public List<T> GetList()
	{
		return list;
	}

	private List<T> removeList = new List<T>();
	public void Clear()
	{
		removeList.AddRange(list);

		list.Clear();

		for (int i = 0; i < removeList.Count; i++)
		{
			removeList[i].Recycle();
		}
		removeList.Clear();
	}
}

public class KeyValueList<K, T> where T : KeyValueList<K, T>.IKeyValueListItem
{
	public interface IKeyValueListItem
	{
		K GetKey();
		void Recycle();
		void Update(float deltaTime);
		void LateUpdate(float deltaTime);
		void FixedUpdate(float deltaTime);
	}

	private List<T> list;
	private Dictionary<K, T> dic;

	public KeyValueList(int capacity)
	{
		list = new List<T>(capacity);
		dic = new Dictionary<K, T>(capacity);
		removeList = new List<T>(capacity);
	}

	public void Add(T item)
	{
		if (item != null)
		{
			dic[item.GetKey()] = item;
			list.Add(item);
		}
	}

	public bool Remove(K sId)
	{
		if (dic.ContainsKey(sId))
		{
			dic.Remove(sId);
		}

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].GetKey().Equals(sId))
			{
				T item = list[i];
				list.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public void ForEach(Action<T> onEach)
	{
		for (int i = 0; i < list.Count; i++)
		{
			onEach?.Invoke(list[i]);
		}
	}

	public T Get(K sId)
	{
		T item = default(T);
		if (dic.TryGetValue(sId, out item))
		{
			return item;
		}
		return item;
	}

	public List<T> GetList()
	{
		return list;
	}

	private List<T> removeList;
	public void Clear()
	{
		removeList.AddRange(list);

		list.Clear();

		for (int i = 0; i < removeList.Count; i++)
		{
			removeList[i].Recycle();
		}
		removeList.Clear();
	}
}
