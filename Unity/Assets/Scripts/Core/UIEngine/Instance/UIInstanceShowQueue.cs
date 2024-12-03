using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIEngine
{
	public class UIInstanceShowQueue
	{
		private List<UIInstanceItem> loadingList = new List<UIInstanceItem>();
		public void Add(UIInstanceItem instanceItem)
		{
			if (instanceItem != null)
			{
				//if (instanceItem.instance != null)
				//{
				//	instanceItem.instance.transform.position = new Vector3(10000f, 0f, 0f);
				//}
				loadingList.Add(instanceItem);
			}
		}

		public void Remove(UIInstanceItem instanceItem)
		{
			loadingList.Remove(instanceItem);
		}

		public void Update()
		{
			while (loadingList.Count > 0)
			{
				UIInstanceItem instanceItem = loadingList[0];
				if (instanceItem.instance != null && instanceItem.PreLoadDone() && instanceItem.PreShowDone())
				{
					loadingList.RemoveAt(0);

					instanceItem.onInit?.Invoke(instanceItem);
					instanceItem.onLoadDone?.Invoke(instanceItem.script);
				}
				else
				{
					break;
				}
			}
		}
	}
}