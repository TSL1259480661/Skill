using App;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UIEngine
{
	public abstract class UIViewBase : UIItemBase
	{
		abstract public void BeforeInit(object[] paramList);
		abstract public void OnInit(object[] paramList);
		abstract public void BeforeShow(object[] paramList);
		abstract public void OnShow(object[] paramList);
		abstract public void OnHide(object[] paramList);
		abstract public void OnRecycle();

		/// <summary>
		/// 引导名称
		/// </summary>
		public virtual string guideName
		{
			get
			{
				return string.Empty;
			}
		}

		public bool showing { private set; get; }
		public bool viewInit { private set; get; }
		public bool alwaysKeep;

		protected object data;
		private UILayer uiLayer;
		public void InitUILayer(UILayer uiLayer)
		{
			this.uiLayer = uiLayer;
		}

		private IUILoadAsset loadAsset;
		private UIInstance uiInstance;
		protected MainModule module;
		public UICamera uiCamera;
		protected GameRoot game;

		public MainModule Module
		{
			get {
				return module;
			}
		}

		public void Init(GameRoot gameRoot, UICamera uiCamera, AudioManager audio, UIInstance uiInstance, EventSystem eventSystem, MainModule module, IUILoadAsset loadAsset)
		{
			this.game = gameRoot;
			this.uiCamera = uiCamera;
			this.audio = audio;
			this.uiInstance = uiInstance;
			this.module = module;
			this.eventSystemContainer.Init(eventSystem, OnLockCallback);

			eventSystemContainer.SetLock(false);

			this.loadAsset = loadAsset;

			viewInit = false;

			showing = true;
		}

		private static UDebugger viewDebugger = new UDebugger("UIViewBase");

		protected void OnLockCallback()
		{
			viewDebugger.LogError("Can not call AddListener out of OnInit.");
		}

		public void Lock()
		{
			eventSystemContainer.SetLock(true);
		}

		protected EventSystemContainer eventSystemContainer = new EventSystemContainer();

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

		public void ShowInit()
		{
			showing = true;
		}

		public void ViewInit(GameObject go, UIViewBase autoHideItem)
		{
			SelfBind(go.transform, this);

			if (autoHideItem != null)
			{
				autoHideItem.Hide();
			}

			viewInit = true;
		}

		protected void ClearPreloadAssets()
		{
			for (int i = 0; i < instanceAssetList.Count; i++)
			{
				instanceAssetList[i].Recycle();
			}
			instanceAssetList.Clear();
		}

		public void Clear()
		{
			ClearComponentFields();

			ClearPreloadAssets();

			ClearLoadAssets();

			ClearLoadAssetItems();

			viewInit = false;

			showing = true;

			gameObject = null;

			alwaysKeep = false;

			eventSystemContainer.Clear();
		}

		protected List<IUILoadAssetItem> instanceAssetList = new List<IUILoadAssetItem>();
		public object GetAssetFromList(int index)
		{
			if (index >= 0 && index < instanceAssetList.Count)
			{
				return instanceAssetList[index].content;
			}
			else
			{
				throw new Exception("index is out of range:" + index);
			}
		}

		public virtual bool CheckPreLoadDone()
		{
			for (int i = 0; i < instanceAssetList.Count; i++)
			{
				if (!instanceAssetList[i].loadDone)
				{
					return false;
				}
			}

			return true;
		}

		public void PreloadAsset(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone)
		{
			IUILoadAssetItem item = loadAsset.LoadAsset(assetPath, assetType, onLoadDone);
			if (item != null)
			{
				instanceAssetList.Add(item);
			}
		}

		public void SetLoadAsset(ref IUILoadAsset loadAsset)
		{
			loadAsset = this.loadAsset;
		}

		public void AddChild(GameObject child, GameObject parent)
		{
			uiLayer.Add(child, parent);
		}

		public void AddChild(Transform child, Transform parent)
		{
			uiLayer.Add(child, parent);
		}

		public void ResetDlgSortingLayer(Transform child = null) 
		{
			uiLayer.SetSortingProperties(transform);
		}

		public void ShowWindow<T>(string assetPath, UILayerType layerType, Action<UIViewBase> onLoadDone, bool autoHideSelf, params object[] paramList)
		{
			ShowWindow(assetPath, typeof(T).Name, layerType, onLoadDone, autoHideSelf, paramList);
		}

		public void ShowWindow(string assetPath, string scriptName, UILayerType layerType, Action<UIViewBase> onLoadDone, bool autoHideSelf, params object[] paramList)
		{
			if (uiInstance != null)
			{
				UIViewBase autoHideItem = autoHideSelf ? this : null;
				uiInstance.LoadInstance(assetPath, scriptName, layerType, onLoadDone, autoHideItem, paramList);
			}
		}

		public void HideWindow<T>(string assetPath)
		{
			HideWindow(assetPath, typeof(T).Name);
		}

		public void HideWindow(string assetPath, string scriptName)
		{
			if (uiInstance != null)
			{
				List<UIInstanceItem> list = uiInstance.GetUIInstanceList();
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Equal(assetPath, scriptName))
					{
						if (list[i].script.showing)
						{
							list[i].script.Hide();
						}
						break;
					}
				}
			}
		}

		public UIViewBase GetWindow<T>(string assetPath)
		{
			if (uiInstance != null)
			{
				string scriptName = typeof(T).Name;
				List<UIInstanceItem> list = uiInstance.GetUIInstanceList();
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Equal(assetPath, scriptName))
					{
						return list[i].script;
					}
				}
			}
			return null;
		}

		protected void ClearAllUI()
		{
			if (uiInstance != null)
			{
				uiInstance.Clear();
			}

			loadAsset.UnloadUnusedAssets();

			Resources.UnloadUnusedAssets();
		}

		public void Show()
		{
			showing = true;
			UpdateShowState();
		}

		public void Hide()
		{
			showing = false;
			UpdateShowState();
		}

		public void UpdateShowState(object[] paramList = null)
		{
			if (transform != null)
			{
				if (showing)
				{
					AddAllListeners();
					transform.localPosition = Vector3.zero;
					OnShow(paramList);
				}
				else
				{
					RemoveAllListeners();
					ClearLoadAssets();
					transform.localPosition = new Vector3(10000, 10000);
					OnHide(paramList);
				}
			}
		}

		public void HideOthers(params UIViewBase[] keepView)
		{
			List<UIInstanceItem> showingList = uiInstance.GetUIInstanceList();
			for (int i = 0; i < showingList.Count; i++)
			{
				UIInstanceItem item = showingList[i];
				if (item.script != null && item.script.showing && item.script != this)
				{
					if (!item.script.alwaysKeep)
					{
						if (keepView != null && keepView.Length > 0)
						{
							for (int j = 0; j < keepView.Length; j++)
							{
								if (item.script != keepView[j])
								{
									item.script.Hide();
								}
							}
						}
						else
						{
							item.script.Hide();
						}
					}
				}
			}
		}

		/// <summary>
		/// 获取当前显示的UI
		/// </summary>
		protected List<UIInstanceItem> GetShowingUIArray()
		{
			return uiInstance.GetUIInstanceList();
		}
	}
}
