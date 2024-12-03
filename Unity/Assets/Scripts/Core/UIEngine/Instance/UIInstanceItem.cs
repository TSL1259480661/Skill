using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIEngine
{
	public class UIInstanceItem : IObjectPoolItem
	{
		public GameObject instance { private set; get; }

		public UIViewBase script;

		public string scriptName;
		public UIViewBase autoHideItem;
		public object[] paramList;
		public UILayerType layerType;
		public Action<UIViewBase> onLoadDone;
		public Action<UIInstanceItem> onInit;
		public Action<UIInstanceItem> recycle;
		public string assetPath { get; private set; }

		private IUILoadAsset loadAsset;
		public void Init(IUILoadAsset loadAsset)
		{
			this.loadAsset = loadAsset;
		}

		public void Load(string assetPath)
		{
			this.assetPath = assetPath;
			assetItem = loadAsset.LoadAsset(assetPath, UIAssetType.Instance, OnLoadDone);
		}

		public bool PreLoadDone()
		{
			return script == null || script.CheckPreLoadDone();
		}

		public bool PreShowDone()
		{
			return script == null || script.CheckPreShowDone();
		}

		private IUILoadAssetItem assetItem;
		private void OnLoadDone(IUILoadAssetItem assetItem)
		{
			if (assetItem != null)
			{
				instance = assetItem.content as GameObject;
			}
		}

		public bool Equal(string assetPath, string scriptName)
		{
			if (!string.IsNullOrEmpty(this.assetPath) && !string.IsNullOrEmpty(this.scriptName))
			{
				return this.assetPath.Equals(assetPath, StringComparison.OrdinalIgnoreCase) &&
				this.scriptName.Equals(scriptName, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		private void Unload()
		{
			if (assetItem != null)
			{
				assetItem.Recycle();
				assetItem = null;
			}
		}

		public void OnRecycle()
		{
			Unload();

			autoHideItem = null;
			this.paramList = null;
			loadAsset = null;
			this.assetPath = null;
			scriptName = null;
			onLoadDone = null;
			onInit = null;
			recycle = null;
			instance = null;
		}

		public void OnReuse()
		{
		}

		public void Recycle()
		{
			recycle?.Invoke(this);
		}
	}
}