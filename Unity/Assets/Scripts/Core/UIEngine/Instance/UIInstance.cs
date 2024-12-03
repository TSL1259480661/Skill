using System.Collections.Generic;
using System;
using App;

namespace UIEngine
{
	public class UIInstance
	{
		private UIInstanceQueue recycledInstanceQueue;
		private UIInstanceShowQueue preShowQueue;
		private IUILoadAsset loadAsset;
		private Action<UIInstanceItem, UIViewBase> onItemInit;
		private TypeInstancePool scriptPool = new TypeInstancePool();
		private MainModule module;
		private EventSystem eventSystem;
		private AudioManager audio;
		private UICamera uiCamera;
		private GameRoot gameRoot;

		public UIInstance(GameRoot gameRoot, UICamera uiCamera, AudioManager audio, MainModule module, EventSystem eventSystem, int queueMaxCount, IUILoadAsset loadAsset, Action<UIInstanceItem, UIViewBase> onItemInit)
		{
			this.gameRoot = gameRoot;
			this.uiCamera = uiCamera;
			this.audio = audio;
			this.module = module;
			this.eventSystem = eventSystem;
			recycledInstanceQueue = new UIInstanceQueue(queueMaxCount);
			preShowQueue = new UIInstanceShowQueue();
			this.loadAsset = loadAsset;
			this.onItemInit = onItemInit;
		}

		public UIInstanceItem LoadInstance(string assetPath, string scriptName, UILayerType layerType, Action<UIViewBase> onLoadDone, UIViewBase autoHideItem, params object[] paramList)
		{
			UIInstanceItem instanceItem = null;

			if (instanceItem == null)
			{
				instanceItem = recycledInstanceQueue.Get(assetPath, scriptName, loadAsset);

				if (instanceItem != null)
				{
					if (instanceItem.script != null && instanceItem.script.viewInit)
					{
						instanceItem.autoHideItem = autoHideItem;
						instanceItem.onLoadDone = onLoadDone;
						instanceItem.paramList = paramList;
						instanceItem.script.ShowInit();
						instanceItem.script.BeforeShow(instanceItem.paramList);
					}
					else
					{
						instanceItem.layerType = layerType;
						instanceItem.scriptName = scriptName;
						instanceItem.autoHideItem = autoHideItem;
						instanceItem.paramList = paramList;
						instanceItem.onLoadDone = onLoadDone;
						instanceItem.onInit = OnItemInit;
						instanceItem.recycle = RecycleInstance;

						Type type = Type.GetType(instanceItem.scriptName);
						if (type.IsSubclassOf(typeof(UIViewBase)))
						{
							UIViewBase script = scriptPool.Get(instanceItem.scriptName) as UIViewBase;

							instanceItem.script = script;

							script.Init(gameRoot, uiCamera, audio, this, eventSystem, module, loadAsset);
							script.BeforeInit(instanceItem.paramList);
							script.BeforeShow(instanceItem.paramList);
						}
						else
						{
							throw new Exception("Script must be the subclass of UIViewBase.");
						}
					}

					preShowQueue.Add(instanceItem);
				}
			}
			return instanceItem;
		}

		private void OnItemInit(UIInstanceItem instanceItem)
		{
			if (instanceItem != null)
			{
				if (instanceItem.instance != null)
				{
					if (instanceItem.script != null)
					{
						if (instanceItem.script.viewInit)
						{
							onItemInit?.Invoke(instanceItem, instanceItem.script);

							instanceItem.script.ViewInit(instanceItem.instance, instanceItem.autoHideItem);
							instanceItem.autoHideItem = null;
							instanceItem.script.UpdateShowState(instanceItem.paramList);
						}
						else
						{
							onItemInit?.Invoke(instanceItem, instanceItem.script);

#if UNITY_EDITOR
							instanceItem.instance.transform.name = instanceItem.instance.transform.name.Replace("(Clone)", string.Empty) + "_" + instanceItem.scriptName;
#endif

							instanceItem.script.ViewInit(instanceItem.instance, instanceItem.autoHideItem);
							instanceItem.autoHideItem = null;
							instanceItem.script.OnInit(instanceItem.paramList);
							instanceItem.script.Lock();
							instanceItem.script.UpdateShowState(instanceItem.paramList);
						}
					}
					else
					{
						throw new Exception("instanceItem.script is null.");
					}
				}
				else
				{
					throw new Exception("instanceItem.instance is null.");
				}
			}
			else
			{
				throw new Exception("instanceItem is null.");
			}
		}

		public void Update()
		{
			preShowQueue.Update();
		}

		public List<UIInstanceItem> GetUIInstanceList()
		{
			return recycledInstanceQueue.GetUIInstanceList();
		}

		public void Clear()
		{
			recycledInstanceQueue.Clear();
		}

		private void RecycleInstance(UIInstanceItem instanceItem)
		{
			UIViewBase script = instanceItem.script;
			if (script != null)
			{
				script.OnRecycle();

				script.Clear();

				instanceItem.script = null;

				scriptPool.Recycle(script);
			}

			preShowQueue.Remove(instanceItem);

			recycledInstanceQueue.Remove(instanceItem);
		}
	}
}
