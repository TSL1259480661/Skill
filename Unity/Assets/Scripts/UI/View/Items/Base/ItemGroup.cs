using App;
using System.Collections.Generic;

public class ItemGroup<T> where T : IObjectPoolItem, new()
{
	private UDebugger debugger = new UDebugger("ItemGroup");

	private List<T> list = new List<T>();

	public void OnReuse()
	{

	}

	public void Clear()
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].OnRecycle();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="index">start from 0</param>
	/// <returns></returns>
	public T GetItem(int index)
	{
		if (index >= 0 && index < list.Count)
		{
			return list[index];
		}
		else if (index >= list.Count)
		{
			InitItems(index + 1);
			return list[index];
		}
		return default(T);
	}

	public void InitItems(int count)
	{
		int deltaCount = count - list.Count;
		if (deltaCount > 0)
		{
			for (int i = 0; i < deltaCount; i++)
			{
				list.Add(new T());
			}
		}
	}
}
