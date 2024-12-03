using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIEngine
{
	public class ResourceLoadItem : IUILoadAssetItem
	{
		public object content { set; get; }

		public string path
		{
			get
			{
				return assetPath;
			}
		}

		public bool loadDone
		{
			get
			{
				return content != null;
			}
		}

		private string assetPath;
		private Action<string> onUnload;
		private Action<ResourceLoadItem> recycle;
		private Action<IUILoadAssetItem> onLoadDone;
		public void Init(Action<IUILoadAssetItem> onLoadDone, Action<ResourceLoadItem> recycle, GameObject go, string assetPath, Action<string> onUnload)
		{
			this.recycle = recycle;
			content = GameObject.Instantiate(go);
			this.assetPath = assetPath;
			this.onUnload = onUnload;
			this.onLoadDone = onLoadDone;
			onLoadDone?.Invoke(this);
		}

		public void OnRecycle()
		{
			content = null;
		}

		public void OnReuse()
		{
		}

		public void Recycle()
		{
			onUnload?.Invoke(assetPath);
			onUnload = null;
			assetPath = null;
			content = null;
		}
	}
}