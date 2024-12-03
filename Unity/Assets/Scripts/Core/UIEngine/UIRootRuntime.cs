using App;
using UnityEngine;
using UnityEngine.UIElements;


namespace UIEngine
{
	public class UIRootRuntime : UIViewBase
	{
		public override void ClearComponentFields()
		{
			throw new System.NotImplementedException();
		}

		private static UDebugger debugger = new UDebugger("UIRootRuntime");

		private UIInstance uiInstance;
		private UILayerRoot layerRoot;
		private UIEventSystem uiEventSystem;

		public void Init(GameObject gameObject, UnityEngine.EventSystems.EventSystem eventSystem, IUILoadAsset loadAsset, int layerDistance, int queueMaxCount, Vector2 resolutionRatio)
		{
			uiEventSystem = new UIEventSystem();
			if (uiEventSystem == null)
			{
				uiEventSystem.Init(gameObject, eventSystem);
			}

			uiCamera = new UICamera();
			uiCamera.Init("UICamera", UnityEngine.Rendering.Universal.CameraRenderType.Base);

			layerRoot = new UILayerRoot(gameObject, uiCamera, layerDistance, resolutionRatio);

			SimulateServer server = new SimulateServer();

			App.EventSystem appEventSystem = new App.EventSystem(200);

			HttpClient http = gameObject.AddComponent<HttpClient>();
			http.Init(appEventSystem);

			MainModule module = new MainModule(server, appEventSystem, loadAsset, http);

			AudioManager audio = new AudioManager();
			//audio.Init(loadAsset);

			//var gameRoot = new GameRoot(layerRoot, uiCamera, audio, module, appEventSystem, loadAsset);

			//uiInstance = new UIInstance(gameRoot, uiCamera, audio, module, appEventSystem, queueMaxCount, loadAsset, OnUIItemInit);

			//Init(gameRoot, uiCamera, audio, uiInstance, appEventSystem, module, loadAsset);
			WxApiHelper.InitMina();
		}

		private void OnUIItemInit(UIInstanceItem instanceItem, UIViewBase viewBase)
		{
			if (instanceItem != null)
			{
				UILayer uiLayer = layerRoot.Add(instanceItem.layerType, instanceItem.instance) as UILayer;
				viewBase.InitUILayer(uiLayer);
			}
		}

		public void Update()
		{
			//uiInstance.Update();
			//audio.Update();

			//game.Update(Time.deltaTime);
		}

		public void LateUpdate()
		{
			//game.LateUpdate(Time.deltaTime);
		}

		public override void OnRecycle()
		{
		}

		public override void OnInit(object[] paramList)
		{
		}

		public override void BeforeInit(object[] paramList)
		{
		}

		public override void BeforeShow(object[] paramList)
		{

		}

		public override void OnShow(object[] paramList)
		{
		}

		public override void OnHide(object[] paramList)
		{
		}
	}
}
