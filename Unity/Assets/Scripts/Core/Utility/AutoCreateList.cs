using System;

public class AutoCreateList<K, T> where T : class, new()
{
	private KeyList<K, T> validList;
	private KeyList<K, T> allList;

	public AutoCreateList(int capacity, int subCapacity)
	{
		validList = new KeyList<K, T>(capacity, subCapacity);
		allList = new KeyList<K, T>(capacity, subCapacity);
	}

	private Func<K, T> onCreate;
	private Action<T> onClear;
	private Action<T> onReset;
	public void Init(Func<K, T> onCreate, Action<T> onClear, Action<T> onReset = null)
	{
		this.onCreate = onCreate;
		this.onReset = onReset;
		this.onClear = onClear;
	}

	public T Preload(K key)
	{
		var item = onCreate?.Invoke(key);
		if (item != null)
		{
			validList.Add(key, item);
			allList.Add(key, item);
		}
		return item;
	}

	public void Recycle(K key, T item)
	{
		onReset?.Invoke(item);
		validList.Add(key, item);
	}

	public T Get(K key)
	{
		var item = validList.Get(key);
		if (item == null)
		{
			item = onCreate?.Invoke(key);
			if (item != null)
			{
				allList.Add(key, item);
			}
		}
		return item;
	}

	public void ForEach(Action<T> onEach)
	{
		validList.ForEach(onEach);
	}

	public void Clear()
	{
		validList.Clear(null);
		allList.Clear(onClear);
	}

	public void Reset()
	{
		validList.Clear(null);
		allList.ForEach(onReset);
		validList.CopyFrom(allList);
	}
}

public class AutoCreateList<K, P1, T> where T : class, new()
{
	private KeyList<K, T> validList;
	private KeyList<K, T> allList;

	public AutoCreateList(int capacity, int subCapacity)
	{
		validList = new KeyList<K, T>(capacity, subCapacity);
		allList = new KeyList<K, T>(capacity, subCapacity);
	}

	private Func<K, P1, T> onCreate;
	private Action<T> onClear;
	private Action<T> onReset;
	public void Init(Func<K, P1, T> onCreate, Action<T> onClear, Action<T> onReset = null)
	{
		this.onCreate = onCreate;
		this.onReset = onReset;
		this.onClear = onClear;
	}

	public T Preload(K key, P1 param1)
	{
		var item = onCreate?.Invoke(key, param1);
		if (item != null)
		{
			validList.Add(key, item);
			allList.Add(key, item);
		}
		return item;
	}

	public void Recycle(K key, T item)
	{
		onReset?.Invoke(item);
		validList.Add(key, item);
	}

	public T Get(K key, P1 param1)
	{
		var item = validList.Get(key);
		if (item == null)
		{
			item = onCreate?.Invoke(key, param1);
			if (item != null)
			{
				allList.Add(key, item);
			}
		}
		return item;
	}

	public void ForEach(Action<T> onEach)
	{
		validList.ForEach(onEach);
	}

	public void Clear()
	{
		validList.Clear(null);
		allList.Clear(onClear);
	}

	public void Reset()
	{
		validList.Clear(null);
		allList.ForEach(onReset);
		validList.CopyFrom(allList);
	}
}

public class AutoCreateList<K, P1, P2, T> where T : class, new()
{
	private KeyList<K, T> validList;
	private KeyList<K, T> allList;

	public AutoCreateList(int capacity, int subCapacity)
	{
		validList = new KeyList<K, T>(capacity, subCapacity);
		allList = new KeyList<K, T>(capacity, subCapacity);
	}

	private Func<K, P1, P2, T> onCreate;
	private Action<T> onClear;
	private Action<T> onReset;
	public void Init(Func<K, P1, P2, T> onCreate, Action<T> onClear, Action<T> onReset = null)
	{
		this.onCreate = onCreate;
		this.onReset = onReset;
		this.onClear = onClear;
	}

	public void Preload(K key, P1 param1, P2 param2, int count = 1)
	{
		int maxCount = validList.GetCount(key);
		int deltaCount = count - maxCount;
		for (int i = 0; i < deltaCount; i++)
		{
			var item = onCreate?.Invoke(key, param1, param2);
			if (item != null)
			{
				validList.Add(key, item);
				allList.Add(key, item);
			}
		}
	}

	public void Recycle(K key, T item)
	{
		onReset?.Invoke(item);
		validList.Add(key, item);
	}

	public T Get(K key, P1 param1, P2 param2)
	{
		var item = validList.Get(key);
		if (item == null)
		{
			item = onCreate?.Invoke(key, param1, param2);
			if (item != null)
			{
				allList.Add(key, item);
			}
		}
		return item;
	}

	public void ForEach(Action<T> onEach)
	{
		validList.ForEach(onEach);
	}

	public void Clear()
	{
		validList.Clear(null);
		allList.Clear(onClear);
	}

	public void Reset()
	{
		validList.Clear(null);
		allList.ForEach(onReset);
		validList.CopyFrom(allList);
	}
}
