using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YooAsset;

namespace UIEngine
{
	public class YooResourceLoadItem : IUILoadAssetItem
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
		private Action<YooResourceLoadItem> recycle;
		private Action<IUILoadAssetItem> onLoadDone;
		private AssetHandle assetHandle;
		private int assetType;
		private Queue<IUILoadAssetItem> instanceList;
		public void Init(ResourcePackage package, int assetType, Action<IUILoadAssetItem> onLoadDone, Action<YooResourceLoadItem> recycle, string assetPath, Queue<IUILoadAssetItem> instanceList)
		{
			this.assetType = assetType;
			this.recycle = recycle;
			this.assetPath = assetPath;
			this.onLoadDone = onLoadDone;
			this.instanceList = instanceList;

			assetHandle = package.LoadAssetAsync(assetPath);
			assetHandle.Completed += OnComplete;
		}

		private void OnComplete(AssetHandle assetHandle)
		{
			if (assetType == UIAssetType.Instance)
			{
				instanceList.Enqueue(this);
			}
			else if (assetType == UIAssetType.Resource)
			{
				content = assetHandle.AssetObject;
				onLoadDone?.Invoke(this);
			}
		}

		public void Instantiate()
		{
			if (assetHandle != null)
			{
				GameObject go = assetHandle.AssetObject as GameObject;
				if (go != null)
				{
					content = GameObject.Instantiate(go);
				}
			}

			onLoadDone?.Invoke(this);
		}

		public void OnRecycle()
		{
			if (assetType == UIAssetType.Instance)
			{
				if (content != null && content is GameObject)
				{
					GameObject.Destroy(content as GameObject);
				}
			}

			if (assetHandle != null)
			{
				assetHandle.Release();
			}

			assetHandle = null;
			recycle = null;
			content = null;
			assetPath = null;
		}

		public void OnReuse()
		{
		}

		public void Recycle()
		{
			recycle?.Invoke(this);
			recycle = null;
		}
	}
}
