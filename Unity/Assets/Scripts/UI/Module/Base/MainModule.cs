using App;
using System;
using System.Collections.Generic;
using UIEngine;

public class MainModule
{
	private static UDebugger debugger = new UDebugger("MainModule");
	private List<ModuleBase> moduleList = new List<ModuleBase>();

	public ConfigModule config = new ConfigModule();

	//public GameDataModule gameDataModule;
	//public OutGameDataModule outGameDataModule;
	//public InBagModule inBagModule;
	public OutSkillModule outSkillModule;


	private void CreateAll()
	{
		//inBagModule = CreateModule<InBagModule>();
		//inShopModule = CreateModule<InShopModule>();


		outSkillModule = CreateModule<OutSkillModule>();
	}

	private SimulateServer server;
	private EventSystem eventSystem;
	private HttpClient http;
	public MainModule(SimulateServer server, EventSystem eventSystem, IUILoadAsset loadAsset, HttpClient httpClient)
	{
		CreateAll();

		this.server = server;
		this.eventSystem = eventSystem;
		this.http = httpClient;
		config.InheritInit(eventSystem, this, config, loadAsset, httpClient);

		for (int i = 0; i < moduleList.Count; i++)
		{
			moduleList[i].InheritInit(eventSystem, this, config, loadAsset, httpClient);
		}
	}

	private bool init;
	public void Init(Action onInit)
	{
		if (!init)
		{
			init = true;

			config.InitAllConfig(() =>
			{
				InitAll();
				onInit?.Invoke();
			});
		}
		else
		{
			onInit?.Invoke();
		}
	}

	private void InitAll()
	{
		server.Init(config, eventSystem);

		for (int i = 0; i < moduleList.Count; i++)
		{
			moduleList[i].Init();
		}
	}

	public T CreateModule<T>() where T : ModuleBase, new()
	{
		T module = new T();
		moduleList.Add(module);
		return module;
	}
}
