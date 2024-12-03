using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIEngine
{
	public class ResourceLoad : IUILoadAsset
	{
		private static UObjectPool<ResourceLoadItem> itemPool = new UObjectPool<ResourceLoadItem>();
		private Dictionary<string, int> loadCount = new Dictionary<string, int>();

		public IUILoadAssetItem LoadAsset(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone)
		{
			if (!loadCount.ContainsKey(assetPath))
			{
				loadCount[assetPath] = 0;
			}
			loadCount[assetPath]++;
			//Debug.LogFormat("path:{0} ,count:{1}", assetPath, loadCount[assetPath]);

			ResourceLoadItem item = itemPool.Get();
			GameObject go = Resources.Load<GameObject>(assetPath);
			item.Init(onLoadDone, Recycle, go, assetPath, UnloadAsset);
			return item;
		}

		public void UnloadUnusedAssets()
		{
			Resources.UnloadUnusedAssets();
		}

		private void Recycle(ResourceLoadItem loadItem)
		{
			itemPool.Recycle(loadItem);
		}

		private void UnloadAsset(string assetPath)
		{
			int count = 0;
			if (loadCount.TryGetValue(assetPath, out count))
			{
				loadCount[assetPath]--;
				//Debug.LogFormat("path:{0} ,count:{1}", assetPath, loadCount[assetPath]);
			}
		}
	}
}
