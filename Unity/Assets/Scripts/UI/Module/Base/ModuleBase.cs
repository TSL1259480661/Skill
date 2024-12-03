using App;
using System;
using UIEngine;

public abstract class ModuleBase
{
	protected MainModule module;
	protected ConfigModule config;
	protected IUILoadAsset loadAsset;
	protected HttpClient http;
	public void InheritInit(EventSystem eventSystem, MainModule module, ConfigModule config, IUILoadAsset loadAsset, HttpClient httpClient)
	{
		this.eventSystemContainer.Init(eventSystem);
		this.module = module;
		this.config = config;
		this.loadAsset = loadAsset;
		this.http = httpClient;
		removed = false;
	}
	abstract public void Init();
	abstract public void Clear();

	private EventSystemContainer eventSystemContainer = new EventSystemContainer();

	private bool removed = false;
	protected void RemoveAllListeners()
	{
		eventSystemContainer.RemoveAllListeners();
		removed = true;
	}

	protected void AddAllListeners()
	{
		if (removed)
		{
			removed = false;
			eventSystemContainer.AddAllListeners();
		}
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
