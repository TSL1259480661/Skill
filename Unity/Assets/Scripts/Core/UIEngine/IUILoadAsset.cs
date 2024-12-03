using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UIEngine
{
	public static class UIAssetType
	{
		public static int Resource = 1;
		public static int Instance = 2;
		public static int Effect = 3;
		public static int Model = 4;
		public static int Sprite = 5;
		public static int Texture = 6;
		public static int Atlas = 7;
	}

	public interface IUILoadAssetItem : IObjectPoolItem
	{
		public object content { get; }
		public bool loadDone { get; }
		public string path { get; }
		public void Recycle();
	}

	public interface IUILoadAsset
	{
		public IUILoadAssetItem LoadAsset(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone);
		public void UnloadUnusedAssets();
	}
}
