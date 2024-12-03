using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YooAsset;
using App;

namespace UIEngine
{
	public class YooResourceLoad : IUILoadAsset
	{
		private static UDebugger debugger = new UDebugger("IUILoadAsset");

		private static UObjectPool<YooResourceLoadItem> itemPool = new UObjectPool<YooResourceLoadItem>();

		private ResourcePackage package;
		public void Init(ResourcePackage package)
		{
			this.package = package;
		}

		private UIAtlasLoad atlasLoad = new UIAtlasLoad();

		private Queue<IUILoadAssetItem> instanceList = new Queue<IUILoadAssetItem>(10000);

		public IUILoadAssetItem LoadAsset(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone)
		{
			if (assetType == UIAssetType.Instance || assetType == UIAssetType.Resource)
			{
				YooResourceLoadItem item = itemPool.Get();
				item.Init(package, assetType, onLoadDone, Recycle, assetPath, instanceList);
				if (item == null)
				{
					debugger.LogErrorFormat("Asset is null,assetType:{0}, path:{1}", assetType, assetPath);
				}
				return item;
			}
			else if (assetType == UIAssetType.Atlas)
			{
				atlasLoad.LoadAtlas(package, assetPath, onLoadDone);
				return null;
			}
			else if (assetType == UIAssetType.Texture)
			{
				UITextureItem textureItem = UITextureItem.Get();
				textureItem.LoadTexture(package, assetPath, onLoadDone);
				if (textureItem == null)
				{
					debugger.LogErrorFormat("Asset is null,assetType:{0}, path:{1}", assetType, assetPath);
				}
				return textureItem;
			}
			else if (assetType == UIAssetType.Sprite)
			{
				UISpriteItem spriteItem = UISpriteItem.Get();
				spriteItem.LoadSprite(package, assetPath, onLoadDone);
				if (spriteItem == null)
				{
					debugger.LogErrorFormat("Asset is null,assetType:{0}, path:{1}", assetType, assetPath);
				}
				return spriteItem;
			}

			debugger.LogErrorFormat("assetType error ,path:{0}", assetPath);

			return null;
		}

		public void Update()
		{
			atlasLoad.Update();

			for (int i = 0; i < 10; i++)
			{
				if (instanceList.Count > 0)
				{
					(instanceList.Dequeue() as YooResourceLoadItem).Instantiate();
				}
			}
		}

		public void ClearAtlas()
		{
			atlasLoad.Clear();
		}

		private void Recycle(YooResourceLoadItem loadItem)
		{
			itemPool.Recycle(loadItem);
		}

		public void UnloadUnusedAssets()
		{
			package?.UnloadUnusedAssets();
		}
	}
}
