using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using YooAsset;

namespace UIEngine
{
	public class UITextureItem : IUILoadAssetItem
	{
		public static UObjectPool<UITextureItem> texturePool = new UObjectPool<UITextureItem>();

		public static UITextureItem Get()
		{
			return texturePool.Get();
		}

		private AssetHandle assetHandle;
		private Texture texture = null;
		private string assetPath;
		private Action<IUILoadAssetItem> onLoadDone;
		private bool completed;
		public object content
		{
			get
			{
				return texture;
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

		public void LoadTexture(ResourcePackage package, string assetPath, Action<IUILoadAssetItem> onLoadDone)
		{
			this.assetPath = assetPath;
			this.onLoadDone = onLoadDone;

			if (assetHandle != null)
			{
				assetHandle.Release();
			}

			assetHandle = package.LoadAssetAsync(assetPath);
			assetHandle.Completed += OnCompleted;
		}

		public Texture GetTexture()
		{
			return texture;
		}

		private void OnCompleted(AssetHandle assetHandle)
		{
			completed = true;
			if (assetHandle != null)
			{
				texture = assetHandle.GetAssetObject<Texture>();
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
			texturePool.Recycle(this);
		}
	}
}