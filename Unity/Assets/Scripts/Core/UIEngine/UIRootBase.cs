using App;
using UnityEngine;

namespace UIEngine
{
	public class UIRootBase : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.EventSystems.EventSystem eventSystem;
		[SerializeField]
		private int queueMaxCount = 20;
		[SerializeField]
		private int layerDistance = 100;

		[SerializeField]
		private Vector2 resolutionRatio = new Vector2(1920f, 1080f);

		protected UIRootRuntime runTime;

		virtual protected void Awake()
		{

		}

		virtual protected void Start()
		{
			runTime = new UIRootRuntime();
			IUILoadAsset loadAsset = GetIUILoadAsset();
			runTime.Init(this.gameObject, eventSystem, loadAsset, layerDistance, queueMaxCount, resolutionRatio);
			GameObject.DontDestroyOnLoad(gameObject);
		}

		virtual protected void Update()
		{
			runTime.Update();
		}

		virtual protected void LateUpdate()
		{
			runTime?.LateUpdate();
		}

		private void FixedUpdate()
		{
			App.Timer.Instance.FixedUpdate();
			DoTween.Instance.FixedUpdate();
			UnscaledDoTween.Instance.FixedUpdate();
			UnscaledTimer.Instance.FixedUpdate();
			InGameTimer.Instance.FixedUpdate();
		}

		virtual protected IUILoadAsset GetIUILoadAsset()
		{
			return new ResourceLoad();
		}
	}
}
