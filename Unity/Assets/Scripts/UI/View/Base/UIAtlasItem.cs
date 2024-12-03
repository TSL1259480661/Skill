using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using YooAsset;

namespace UIEngine
{
	public class UIAtlasItem : IObjectPoolItem
	{
		private AssetHandle assetHandle;
		private SpriteAtlas atlas = null;
		public bool completed { private set; get; }

		public void LoadAtlas(ResourcePackage package, string assetPath)
		{
			if (assetHandle == null)
			{
				assetHandle = package.LoadAssetAsync(assetPath);
				assetHandle.Completed += OnCompleted;
			}
		}

		public SpriteAtlas GetAtlas()
		{
			return atlas;
		}

		private void OnCompleted(AssetHandle assetHandle)
		{
			completed = true;
			if (assetHandle != null)
			{
				atlas = assetHandle.GetAssetObject<SpriteAtlas>();
			}
		}

		public void OnReuse()
		{
		}

		public void OnRecycle()
		{
			completed = false;
			if (assetHandle != null)
			{
				assetHandle.Release();
			}
			assetHandle = null;
		}
	}
}