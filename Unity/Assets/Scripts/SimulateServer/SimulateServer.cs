using App;
using System.Collections.Generic;

public class SimulateServer
{
	private static UDebugger debugger = new UDebugger("SimulateServer");
	private List<ServerItemBase> list = new List<ServerItemBase>();

	public SimulateServer()
	{

	}

	public void Init(ConfigModule config, EventSystem eventSystem)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].InheritInit(this, config, eventSystem);
		}

		for (int i = 0; i < list.Count; i++)
		{
			list[i].Init();
		}
	}

	public T CreateServerItem<T>() where T : ServerItemBase, new()
	{
		T item = new T();
		list.Add(item);
		return item;
	}
}

