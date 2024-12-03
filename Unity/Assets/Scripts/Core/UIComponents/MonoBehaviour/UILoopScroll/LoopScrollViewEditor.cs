#if UNITY_EDITOR
using UILoopScroll;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Axis = UnityEngine.UI.GridLayoutGroup.Axis;
using ChildAlignment = UnityEngine.TextAnchor;
using Corner = UnityEngine.UI.GridLayoutGroup.Corner;
using FitMode = UnityEngine.UI.ContentSizeFitter.FitMode;

[ExecuteAlways]
//[RequireComponent(typeof(RectTransform))]
//[DisallowMultipleComponent]
public partial class LoopScrollView
{
	private void Reset()
	{
		mScrollRect = gameObject.GetComponentInParent<ScrollRect>(true);
		if (content != null)
			CheckContentAnchor();
	}
	void Update()
	{
		if (Application.IsPlaying(gameObject))
		{
			return;
		}
		Init();
		Layout();
	}
	void OnValidate()
	{
		//Layout();
	}

	void Layout()
	{
		if (Application.IsPlaying(gameObject))
		{
			return;
		}

		if (content && mItemsList.Count > 0)
		{
			//RefreshAll();

			Init();
			rollAxis = startAxis == Axis.Horizontal ? Axis.Vertical : Axis.Horizontal;
			UpdateViewRect();
			UpdateRectsCount(mNumItems);
			mContentSize = UpdateRectsPosition();
			UpdateContentSize(mContentSize);

			//ClearItemsList();
			UpdateRender();
		}
	}


	[Title("菜单")]
	[ShowInInspector]
	private int previewNum = 1;
	[Button("预览布局")]
	void Preview()
	{
		Init();
		CheckContentAnchor();
		SetItemPool();
		numItems = previewNum;
	}

	[Button("清理预览")]
	void PreviewClear()
	{
		SetItemPool();
		numItems = 0;
		ClearAllItem();
		itemPool.Clear();
	}

	void SetItemPool()
	{
		itemPool.onGetItemByIndex = (x) =>
		{
			LoopScrollItem item;
			if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(itemPool.ItemPrefab))
			{
				item = (LoopScrollItem)UnityEditor.PrefabUtility.InstantiatePrefab(itemPool.ItemPrefab);
			}
			else
			{
				item = Object.Instantiate(itemPool.ItemPrefab);
			}
			if (itemPool.itemScale != Vector3.zero)
			{
				item.transform.localScale = itemPool.itemScale;
			}
			return item;
		};
		itemPool.onStoreItem = (x, item) => { Object.DestroyImmediate(item.gameObject); };
	}

	bool ExamineItemSize()
	{
		if (itemSize.x == 0 || itemSize.y == 0)
			return true;
		return false;
	}

	void SetNativeSize()
	{
		if (itemPool.ItemPrefab != null)
		{
			var size = itemPool.ItemPrefab.rectTransform.rect.size;
			var scale = itemPool.itemScale != Vector3.zero ? itemPool.itemScale : itemPool.ItemPrefab.rectTransform.localScale;
			this.itemSize = size * scale;
		}
	}

	[ShowInInspector]
	private bool showGizmos = false;
	[ShowInInspector]
	private bool convertLocalGizmos = false;
	void OnDrawGizmos()
	{
		if (!showGizmos)
		{
			return;
		}

		var ran = new System.Random(5044);
		var mViewRect = this.mViewRect;
		mViewRect.y = -mViewRect.y;
		var canvas = mScrollRect.viewport;
		var ccanvas = mScrollRect.content;
		void DrawRect(Rect rect, Rect mask)
		{
			var color = new Color((float)ran.NextDouble(), (float)ran.NextDouble(), (float)ran.NextDouble());
			Gizmos.color = Color.yellow;

			Vector3 leftTop = new Vector2(rect.xMin, -rect.yMax);
			Vector3 rightTop = new Vector2(rect.xMax, -rect.yMax);
			Vector3 leftbottom = new Vector2(rect.xMin, -rect.yMin);
			Vector3 rightbottom = new Vector2(rect.xMax, -rect.yMin);

			if (convertLocalGizmos)
			{
				leftTop = canvas.TransformPoint(leftTop);
				rightTop = canvas.TransformPoint(rightTop);
				leftbottom = canvas.TransformPoint(leftbottom);
				rightbottom = canvas.TransformPoint(rightbottom);
			}

			Debug.DrawLine(leftTop, rightTop, color);
			Debug.DrawLine(rightTop, rightbottom, color);
			Debug.DrawLine(rightbottom, leftbottom, color);
			Debug.DrawLine(leftbottom, leftTop, color);
		}

		void DrawMask(Rect rect, Rect mask)
		{
			Vector3 leftTop = new Vector2(rect.xMin, -rect.yMax);
			Vector3 rightTop = new Vector2(rect.xMax, -rect.yMax);
			Vector3 leftbottom = new Vector2(rect.xMin, -rect.yMin);
			Vector3 rightbottom = new Vector2(rect.xMax, -rect.yMin);

			if (convertLocalGizmos)
			{
				leftTop = canvas.TransformPoint(leftTop);
				rightTop = canvas.TransformPoint(rightTop);
				leftbottom = canvas.TransformPoint(leftbottom);
				rightbottom = canvas.TransformPoint(rightbottom);
			}

			Gizmos.color = Color.yellow;
			Debug.DrawLine(leftTop, rightTop, Color.black);
			Debug.DrawLine(rightTop, rightbottom, Color.black);
			Debug.DrawLine(rightbottom, leftbottom, Color.black);
			Debug.DrawLine(leftbottom, leftTop, Color.black);
		}
		// Debug.DrawLine();

		foreach (var dRect in rectangles)
		{
			DrawRect(dRect.rect, mViewRect);
		}

		mViewRect.y = 0;
		DrawMask(mViewRect, mViewRect);
	}


}

#endif
