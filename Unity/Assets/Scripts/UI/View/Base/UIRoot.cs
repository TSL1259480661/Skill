using App;
using UIEngine;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering;
using YooAsset;


public class UIRoot : UIRootBase
{
	public EPlayMode assetMode = EPlayMode.EditorSimulateMode;

	private static UDebugger debugger = new UDebugger("UIRoot");

	private ResourcePackage package;

	/// <summary>
	/// 起始背景
	/// </summary>
	public Canvas StartCanvas;

	protected override void Awake()
	{
		base.Awake();
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
		}
	}

	protected override void Start()
	{
		resourceLoad = new YooResourceLoad();

		base.Start();
		YooAssets.Initialize(new YooLogger("YooAsset"));

		// 防止一帧做太多事情 卡顿
		YooAssets.SetOperationSystemMaxTimeSlice(30);
		YooAssets.SetCacheSystemDisableCacheOnWebGL();
		// 创建默认的资源包
		string packageName = "DefaultPackage";
		package = YooAssets.TryGetPackage(packageName);
		if (package == null)
		{
			package = YooAssets.CreatePackage(packageName);
		}

		InitializationOperation initializationOperation = null;

		// 单机运行模式
		if (assetMode == EPlayMode.OfflinePlayMode && initializationOperation == null)
		{
			var createParameters = new OfflinePlayModeParameters();
			//createParameters.DecryptionServices = new GameDecryptionServices();
			initializationOperation = package.InitializeAsync(createParameters);

		}
		else if (assetMode == EPlayMode.WebPlayMode && initializationOperation == null)
		{
#if UNITY_EDITOR
			var createParameters = new EditorSimulateModeParameters();
			EDefaultBuildPipeline buildPipeline = EDefaultBuildPipeline.ScriptableBuildPipeline;
			createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(buildPipeline, packageName);
			initializationOperation = package.InitializeAsync(createParameters);
#else
			string defaultHostServer = "http://127.0.0.1/CDN/WebGL/V1.0";
			string fallbackHostServer = "http://127.0.0.1/CDN/WebGL/V1.0";
			var initParameters = new WebPlayModeParameters();
			initParameters.BuildinQueryServices = new WebGLGameQueryServices();
			initParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
			initializationOperation = package.InitializeAsync(initParameters);
#endif
		}

		resourceLoad.Init(package);

		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 30;

		if (initializationOperation != null)
		{
			initializationOperation.Completed += OnCompleted;
		}

		InitLog();
	}

	/// <summary>
	/// 初始化打印日志
	/// </summary>
	public void InitLog()
	{
#if !RELEASE
		var obj = Resources.Load<GameObject>("Reporter");
		if (obj != null)
		{
			var go = GameObject.Instantiate<GameObject>(obj);
			GameObject.DontDestroyOnLoad(go);
		}
#endif
	}

	private YooResourceLoad resourceLoad;
	protected override IUILoadAsset GetIUILoadAsset()
	{
		return resourceLoad;
	}
	
	protected override void Update()
	{
		base.Update();
		resourceLoad.Update();
	}

	private void OnCompleted(AsyncOperationBase operation)
	{
		if (operation.Status == EOperationStatus.Succeed)
		{
			//runTime.ShowWindow<DlgTipsRoot_EmptyView>(UIAssetPaths.DlgTipsRoot_Empty, UILayerType.System, null, false);
			//runTime.ShowWindow<DlgLoginView>(UIAssetPaths.DlgLogin, UILayerType.Normal, null, false);
			//runTime.ShowWindow<DlgGMView>(UIAssetPaths.DlgGM, UILayerType.Top, null, false);
			//runTime.ShowWindow<DlgClickScreenEffectView>(UIAssetPaths.DlgClickScreenEffect, UILayerType.System, null, false);

			//runTime.ShowWindow<DlgGMView>(UIAssetPaths.DlgGM,UILayerType.Top,null,false);//GM界面
			//runTime.ShowWindow<DlgTipView>(UIAssetPaths.DlgTip, UILayerType.System, null, false);//最上层，数据加载界面
			//runTime.ShowWindow<DlgBattleDataView>(UIAssetPaths.DlgBattleData, UILayerType.Top, null, false);//飘字界面
			//runTime.ShowWindow<DlgBattleUIView>(UIAssetPaths.DlgBattleUI, UILayerType.Pop, null, false);//局内UI界面
			//runTime.ShowWindow<DlgBattleFlow_EmptyView>(UIAssetPaths.DlgBattleFlow_Empty, UILayerType.System, (v) =>//怪物生成数据支持
			//{
			//	runTime.ShowWindow<DlgStartGameView>(UIAssetPaths.DlgStartGame, UILayerType.Normal, (v) =>
			//	{
			//		(v as DlgStartGameView).PreLoad();
			//	}, false);//开始游戏界面
			//}, false);//怪物数据生成界面
			//debugger.Log("Init Done!");
		}
	}
}
