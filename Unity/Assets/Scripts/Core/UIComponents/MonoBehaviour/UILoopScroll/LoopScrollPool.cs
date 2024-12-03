using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace UILoopScroll
{
	/// <summary>
	/// 根据index实例化item
	/// </summary>
	public interface ILoopScrollPool
	{
		public LoopScrollItem GetItem(int index, Transform parent);
		public void StoreItem(int index, LoopScrollItem item);
	}

	[Serializable]
	public class LoopScrollPool : ILoopScrollPool
	{
		#region 简单对象池
		[LabelText("Item Prefab")]
		[SerializeField]
		private LoopScrollItem itemPrefab;
		[Tooltip("覆盖ItemPrefab的缩放")]
		[SerializeField]
		public Vector3 itemScale = Vector3.zero;
		[Tooltip("使用全局对象池")]
		//[SerializeField]
		private bool globalPool = false;
		//[Tooltip("设置一个合适的对象池List初始容量,防止频繁扩容")]
		//[SerializeField]
		//private int capacity = 5;

		public LoopScrollItem ItemPrefab
		{
			get { return itemPrefab; }
			set
			{
				if (itemPrefab != value)
					Clear();
				itemPrefab = value;
			}
		}
		[NonSerialized]
		public static Dictionary<LoopScrollItem, List<LoopScrollItem>> itemListGlobal = new Dictionary<LoopScrollItem, List<LoopScrollItem>>();

		[NonSerialized]
		public List<LoopScrollItem> itemList;

		public LoopScrollItem Get()
		{
			List<LoopScrollItem> list;
			if (globalPool)
			{
				if (!itemListGlobal.ContainsKey(itemPrefab))
				{
					itemListGlobal.Add(itemPrefab, new List<LoopScrollItem>(30));
				}
				list = itemListGlobal[itemPrefab];
			}
			else
			{
				if (itemList == null)
					itemList = new List<LoopScrollItem>(5);
				list = itemList;
			}

			LoopScrollItem item;
			var count = list.Count;
			if (count > 0)
			{
				item = list[count - 1];
				list.RemoveAt(count - 1);
			}
			else
			{
				item = UnityEngine.Object.Instantiate<LoopScrollItem>(itemPrefab);
			}
			if (itemScale != Vector3.zero)
			{
				item.transform.localScale = itemScale;
			}
			return item;
		}
		public void Store(LoopScrollItem item)
		{
			if (globalPool)
			{
				itemListGlobal[itemPrefab].Add(item);
			}
			else
			{
				itemList.Add(item);
			}
		}

		public void Clear()
		{
			if (itemList != null)
			{
				for (int i = itemList.Count - 1; i >= 0; --i)
				{
					var dynamicItem = itemList[i];
					UnityEngine.Object.Destroy(dynamicItem.gameObject);
				}
				itemList.Clear();
			}
		}

		#endregion


		#region /*            更自由的方式，自定义加载不同的item             */
		public System.Func<int, LoopScrollItem> onGetItemByIndex;
		public System.Action<int, LoopScrollItem> onStoreItem;
		#endregion

		public LoopScrollItem GetItem(int index, Transform parent)
		{
			LoopScrollItem item;
			if (onGetItemByIndex != null && onStoreItem != null)
				item = onGetItemByIndex(index);
			else if (itemPrefab != null)
				item = Get();
			else { throw new Exception("onGetItemByIndex and itemPrefab is null"); }
			item.rectTransform.SetParent(parent, false);
			item.gameObject.SetActive(true);
			item.SetVisible(true);
			//item.rectTransform.localScale = Vector3.one;
			//item.rectTransform.localRotation = Quaternion.identity;
			item.rectTransform.anchoredPosition3D = Vector3.zero;
			return item;
		}

		public void StoreItem(int index, LoopScrollItem item)
		{
			//item.gameObject.SetActive(false);
			item.SetVisible(false);
			if (onGetItemByIndex != null && onStoreItem != null)
				onStoreItem(index, item);
			else
				Store(item);
		}
	}
}
