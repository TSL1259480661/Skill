using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIEngine
{
	public class UIInstanceQueue
	{
		private static UObjectPool<UIInstanceItem> itemPool = new UObjectPool<UIInstanceItem>();

		private List<UIInstanceItem> instanceList = new List<UIInstanceItem>();

		private int maxCount;
		public UIInstanceQueue(int maxCount)
		{
			this.maxCount = maxCount;
		}

		public void Clear()
		{
			for (int i = instanceList.Count - 1; i >= 0; i--)
			{
				bool canDestroy = true;

				UIInstanceItem item = instanceList[i];

				if (item.script != null && item.script.alwaysKeep)
				{
					canDestroy = false;
				}

				if (canDestroy)
				{
					item.Recycle();
				}
			}
		}

		public bool Remove(UIInstanceItem uiInstanceItem)
		{
			if (uiInstanceItem != null)
			{
				instanceList.Remove(uiInstanceItem);

				itemPool.Recycle(uiInstanceItem);
				return true;
			}
			return false;
		}

		public UIInstanceItem Get(string assetPath, string scriptName, IUILoadAsset loadAsset)
		{
			UIInstanceItem item;
			for (int i = 0; i < instanceList.Count; i++)
			{
				item = instanceList[i];
				if (item.Equal(assetPath, scriptName))
				{
					return item;
				}
			}

			item = itemPool.Get();

			instanceList.Add(item);

			item.Init(loadAsset);
			item.Load(assetPath);

			int removeIndex = 0;
			while (instanceList.Count > removeIndex && (instanceList.Count > maxCount || maxCount <= 0))
			{
				item = instanceList[removeIndex];
				if (item.script == null || !item.script.alwaysKeep)
				{
					item.Recycle();
				}
				else
				{
					removeIndex++;
				}
			}

			return item;
		}

		public List<UIInstanceItem> GetUIInstanceList()
		{
			return instanceList;
		}
	}
}
