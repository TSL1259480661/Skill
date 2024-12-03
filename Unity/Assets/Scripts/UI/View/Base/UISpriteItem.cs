using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using YooAsset;

namespace UIEngine
{
	public class UISpriteItem : IUILoadAssetItem
	{
		public static UObjectPool<UISpriteItem> spritePool = new UObjectPool<UISpriteItem>();

		public static UISpriteItem Get()
		{
			return spritePool.Get();
		}

		private AssetHandle assetHandle;
		private Sprite sprite = null;
		private string assetPath;
		private Action<IUILoadAssetItem> onLoadDone;
		private bool completed;
		public object content
		{
			get
			{
				return sprite;
			}
			set { }
		}

		public bool loadDone
		{
			get
			{
				return completed;
			}
		}

		public string path => throw new NotImplementedException();

		public void LoadSprite(ResourcePackage package, string assetPath, Action<IUILoadAssetItem> onLoadDone)
		{
			this.assetPath = assetPath;
			this.onLoadDone = onLoadDone;

			if (assetHandle != null)
			{
				assetHandle.Release();
			}

			assetHandle = package.LoadAssetAsync(assetPath, typeof(Sprite));
			assetHandle.Completed += OnCompleted;
		}

		public Sprite GetSprite()
		{
			return sprite;
		}

		private void OnCompleted(AssetHandle assetHandle)
		{
			completed = true;
			if (assetHandle != null)
			{
				sprite = assetHandle.GetAssetObject<Sprite>();
			}
			onLoadDone?.Invoke(this);
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

		public void Recycle()
		{
			spritePool.Recycle(this);
		}
	}
}
