using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.UIElements;
using YooAsset;

namespace UIEngine
{
	public class UIAtlasLoad
	{
		private class ItemVo : IObjectPoolItem, IUILoadAssetItem
		{
			public string assetPath;
			public UIAtlasItem atlas;
			public Action<IUILoadAssetItem> onLoadDone;

			public object content { get; set; }

			public bool loadDone
			{
				get
				{
					return atlas == null || atlas.completed;
				}
			}

			public string path { get; }

			public void OnRecycle()
			{
				atlas = null;
				assetPath = null;
				onLoadDone = null;
				content = null;
			}

			public void OnReuse()
			{
			}

			public void Recycle()
			{
			}
		}

		private UObjectPool<ItemVo> pool = new UObjectPool<ItemVo>();
		private UObjectPool<UIAtlasItem> atlasPool = new UObjectPool<UIAtlasItem>();

		private List<ItemVo> loadingList = new List<ItemVo>();

		private Dictionary<string, UIAtlasItem> atlasDic = new Dictionary<string, UIAtlasItem>();
		public void LoadAtlas(ResourcePackage package, string assetPath, Action<IUILoadAssetItem> onLoadDone)
		{
			UIAtlasItem atlas = null;
			if (!atlasDic.TryGetValue(assetPath, out atlas))
			{
				atlas = atlasPool.Get();
				atlasDic[assetPath] = atlas;
				atlas.LoadAtlas(package, assetPath);
			}

			if (atlas.completed)
			{
				ItemVo vo = pool.Get();
				vo.content = atlas.GetAtlas();
				onLoadDone?.Invoke(vo);
				pool.Recycle(vo);
			}
			else
			{
				ItemVo vo = pool.Get();
				vo.atlas = atlas;
				vo.assetPath = assetPath;
				vo.onLoadDone = onLoadDone;
				loadingList.Add(vo);
			}
		}

		public void Update()
		{
			if (loadingList.Count > 0)
			{
				for (int i = loadingList.Count - 1; i >= 0; i--)
				{
					ItemVo item = loadingList[i];
					if (item.atlas != null)
					{
						if (item.atlas.completed)
						{
							item.content = item.atlas.GetAtlas();
							item.onLoadDone?.Invoke(item);
							loadingList.RemoveAt(i);
							pool.Recycle(item);
						}
					}
					else
					{
						loadingList.RemoveAt(i);
						pool.Recycle(item);
					}
				}
			}
		}

		public void Clear()
		{
			foreach (var item in atlasDic)
			{
				atlasPool.Recycle(item.Value);
			}
			atlasDic.Clear();

			for (int i = 0; i < loadingList.Count; i++)
			{
				pool.Recycle(loadingList[i]);
			}
			loadingList.Clear();
		}
	}
}