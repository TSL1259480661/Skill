using App;
using System;

public abstract class ServerItemBase
{
	abstract public void Init();

	protected ConfigModule config;
	protected SimulateServer server;
	protected EventSystemContainer eventSystemContainer = new EventSystemContainer();
	public void InheritInit(SimulateServer server, ConfigModule config, EventSystem eventSystem)
	{
		this.server = server;
		this.config = config;
		this.eventSystemContainer.Init(eventSystem);
	}

	protected void RemoveAllListeners()
	{
		eventSystemContainer.RemoveAllListeners();
	}

	protected void AddAllListeners()
	{
		eventSystemContainer.AddAllListeners();
	}

	protected int AddListener<T>(Action<T> callback) where T : struct
	{
		return eventSystemContainer.AddListener<T>(callback);
	}

	protected bool RemoveListener(int listenerId)
	{
		return eventSystemContainer.RemoveListener(listenerId);
	}

	protected void Dispatch<T>(T data) where T : struct
	{
		eventSystemContainer.Dispatch(data);
	}
}