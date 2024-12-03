using UILoopScroll;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Axis = UnityEngine.UI.GridLayoutGroup.Axis;
using ChildAlignment = UnityEngine.TextAnchor;
using Corner = UnityEngine.UI.GridLayoutGroup.Corner;
using FitMode = UnityEngine.UI.ContentSizeFitter.FitMode;
using App;

// 
// 动态无限列表
// @panhaijie https://blog.csdn.net/ldy597321444/article/details/79990150
//

/// 1.预览布局后需要清空预览
/// 2.content的中心点只能是左上角,锚点为左上角或铺满
/// 3.点击事件需要在item的Inspector面板绑定OnClickItem或者运行时自行注册
/// 4.修改属性后要显示调用RefreshAll()，防止连续修改属性导致无意义的消耗
namespace UILoopScroll
{
	/// <summary>
	/// Constraint type on either the number of columns or rows.
	/// </summary>
	public enum Constraint
	{
		/// <summary>
		/// 自动换行/列
		/// </summary>
		Flexible = 0,
		/// <summary>
		/// 固定行/列
		/// </summary>
		FixedRowOrColumnCount = 1,
	}
}

public partial class LoopScrollView : MonoBehaviour
{

	[Required]
	public ScrollRect mScrollRect;
	//public RectTransform viewport;
	protected RectTransform content;

	//[Title("Item Pool")]
	public LoopScrollPool itemPool = new LoopScrollPool();

	[Title("Layout")]
	public RectOffset padding;
	[Tooltip("默认尺寸")]
	[InlineButton("SetNativeSize")]
	[InfoBox("itemSize不能为0", InfoMessageType = InfoMessageType.Error, VisibleIf = "ExamineItemSize")]
	[FormerlySerializedAs("cellSize")]
	public Vector2 itemSize;
	[FormerlySerializedAs("cellSpacing")]
	public Vector2 itemSpacing;


	[Tooltip("第一个元素所在的角。")]//暂未支持 尺寸不一致的item
	[EnableIf("mIsItemSameSize")]
	public Corner startCorner = Corner.UpperLeft;
	[Tooltip("沿哪个主轴放置元素。")]
	public Axis startAxis = Axis.Horizontal;
	[Tooltip("用于布局元素的对齐方式（如果这些元素未填满可用空间）。")]//暂未支持 尺寸不一致的item
	[EnableIf("mIsItemSameSize")]
	public ChildAlignment childAlignment = ChildAlignment.UpperLeft;
	public Constraint constrain = Constraint.FixedRowOrColumnCount;
	[LabelText("Fixed Row Or Column Count"), Tooltip("固定行数或列数")]
	[ShowIf("constrain", Constraint.FixedRowOrColumnCount)]
	[Indent, Min(1)]
	public int mFixedRowOrColumnCount = 1;
	[DisableIf("@true"), ShowInInspector]
	protected Axis rollAxis = Axis.Vertical;

	#region ContentSizeFitter
	[Title("ContentSizeFitter")]
	[SerializeField, Tooltip("限制content的最小尺寸为ScrollRect.viewport尺寸")]
	public bool restrictContentMinSize = true;
	[ShowIf("startAxis", Axis.Horizontal)]
	public FitMode horizontalFit = FitMode.Unconstrained;
	[ShowIf("startAxis", Axis.Vertical)]
	public FitMode verticalFit = FitMode.Unconstrained;
	#endregion


	[Title("其他属性")]
	[SerializeField, LabelText("ItemSameSize"), Tooltip("每个item是否一致尺寸.取消勾选则表示每个item大小不一致")]
	private bool mIsItemSameSize = true;
	[SerializeField, LabelText("ItemSort"), Tooltip("是否排序item层级")]
	public bool isItemSort = false;
	[SerializeField, Tooltip("强制设置item的尺寸,如果勾选会强制设置item尺寸为逻辑大小")]
	public bool forceItemSize = false;

	#region

	protected Rect mViewRect;
	protected Dictionary<int, DRect> inOverlaps = new Dictionary<int, DRect>();
	//[SerializeField]
	protected List<DRect> rectangles = new List<DRect>();
	//[SerializeField]//支持预先序列化item
	protected List<LoopScrollItem> mItemsList = new List<LoopScrollItem>();
	protected Vector2 mContentSize = Vector2.zero;

	protected bool mHasInited = false;
	public void Init()
	{
		if (!mHasInited)
		{
			mHasInited = true;
			mScrollRect.onValueChanged.AddListener(ScrollRectEvent);
			content = mScrollRect.content;
			CheckContentAnchor();
			if (itemSize.x == 0 || itemSize.y == 0)
			{
				Debug.LogError("The size of item cannot be zero");
			}
			if (itemPool == null) itemPool = new LoopScrollPool();
			rectangles.Clear();
			ClearItemsList();
		}
	}
	void CheckContentAnchor()
	{
		content.pivot = Vector2.up;
		if (content.anchorMin == Vector2.up && content.anchorMax == Vector2.up)
		{
		}
		else if (content.anchorMin == Vector2.zero && content.anchorMax == Vector2.one)
		{
		}
		else
		{
			content.anchorMin = Vector2.zero;
			content.anchorMax = Vector2.one;
			content.offsetMin = Vector2.zero;
			content.offsetMax = Vector2.zero;
		}
	}

	protected LoopScrollItem GetItem(int index, DRect dRect)
	{
		LoopScrollItem item = itemPool.GetItem(index, content);
		item.listView = this;
		item.DRect = dRect;
		item.DRect.isBind = true;
		mItemsList.Add(item);
		return item;
	}

	protected void RemoveItem(int itemIndex, LoopScrollItem item)
	{
		int index = -1;
		if (item.DRect != null)
		{
			index = item.DRect.Index;
			item.DRect.isBind = false;
			item.DRect = null;
		}
		mItemsList.RemoveAt(itemIndex);
		itemPool.StoreItem(index, item);
	}

	public void UpdateViewRect()
	{
		var mMaskSize = mScrollRect.viewport.rect.size;
		mViewRect = new Rect(0, 0, mMaskSize.x, mMaskSize.y);
	}

	protected void UpdateContentSize(Vector2 size)
	{
		if (restrictContentMinSize)
		{
			size.x = Mathf.Max(size.x, mViewRect.width);
			size.y = Mathf.Max(size.y, mViewRect.height);
		}
		if (rollAxis == Axis.Horizontal)
		{
			content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
			if (verticalFit == FitMode.PreferredSize)
			{
				content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
			}
			else if (verticalFit == FitMode.MinSize)
			{
				content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, mViewRect.height);
			}
		}
		else
		{
			content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
			if (horizontalFit == FitMode.PreferredSize)
			{
				content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
			}
			else if (horizontalFit == FitMode.MinSize)
			{
				content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mViewRect.width);
			}
		}
	}
	public void UpdateItemPos(LoopScrollItem item, int index)
	{
		//UGUI的y坐标和逻辑坐标相反的
		var position = rectangles[index].rect.position;
		position.y = -position.y;
		//pivot
		Vector2 size = item.rectTransform.rect.size * item.rectTransform.localScale;
		position.x += size.x * item.rectTransform.pivot.x;
		position.y -= size.y * (1 - item.rectTransform.pivot.y);
		//anchor
		var parentSize = (item.rectTransform.parent.transform as RectTransform).rect.size;
		position.x = position.x - parentSize.x * item.rectTransform.anchorMin.x;
		position.y = position.y + parentSize.y * (1 - item.rectTransform.anchorMin.y);

		item.rectTransform.anchoredPosition = position;
	}


	protected void UpdateRectsCount(int count)
	{
		int oldNumItems = rectangles.Count;
		for (int i = oldNumItems; i < count; i++)
		{
			var rect = DRect.Get(0, 0, itemSize.x, itemSize.y, i);
			rectangles.Add(rect);
		}

		for (int i = oldNumItems - 1; i >= count; i--)
		{
			var rect = rectangles[i];
			DRect.Store(rect);
			rectangles.RemoveAt(i);
		}

	}

	#region 布局

	protected virtual Vector2 UpdateRectsPosition2()
	{
		Vector2 contentSize = Vector2.zero;
		int count = rectangles.Count;
		int fixedCount = mFixedRowOrColumnCount;
		float startX = padding.left, startY = padding.top;
		float endX, endY;

		if (constrain == Constraint.FixedRowOrColumnCount)
		{
			for (int i = 0; i < count; ++i)
			{
				//int row = i / fixedCount, col = i % fixedCount;
				var rect = rectangles[i];
				rect.position = new Vector2(startX, startY);
				endX = rect.rect.xMax;
				endY = rect.rect.yMax;
				contentSize.x = contentSize.x < endX ? endX : contentSize.x;
				contentSize.y = contentSize.y < endY ? endY : contentSize.y;

				if (startAxis == Axis.Horizontal)
				{
					startX = endX + itemSpacing.x;
					if ((i + 1) % fixedCount == 0)
					{//换行
						startY = endY + itemSpacing.y;
						startX = padding.left;
					}
					startY = i >= fixedCount ? (rectangles[i + 1 - fixedCount].yMax + itemSpacing.y) : startY;
				}
				else
				{
					startY = endY + itemSpacing.y;
					if ((i + 1) % fixedCount == 0)
					{//换列
						startX = endX + itemSpacing.x;
						startY = padding.top;
					}
					startX = i >= fixedCount ? (rectangles[i + 1 - fixedCount].xMax + itemSpacing.x) : startX;
				}
			}
		}
		else
		{
			for (int i = 0; i < count; ++i)
			{
				var rect = rectangles[i];
				var nextRect = i == count - 1 ? null : rectangles[i + 1];
				rect.position = new Vector2(startX, startY);
				endX = rect.rect.xMax;
				endY = rect.rect.yMax;
				contentSize.x = contentSize.x < endX ? endX : contentSize.x;
				contentSize.y = contentSize.y < endY ? endY : contentSize.y;
				if (startAxis == Axis.Horizontal)
				{
					startX = endX + itemSpacing.x;
					if (nextRect != null && (startX + nextRect.rect.width) > mViewRect.width)
					{//换行
						startY = endY + itemSpacing.y;
						startX = padding.left;
					}
				}
				else
				{
					startY = endY + itemSpacing.y;
					if (nextRect != null && (startY + nextRect.rect.height) > mViewRect.height)
					{//换列
						startX = endX + itemSpacing.x;
						startY = padding.top;
					}
				}
			}
		}
		contentSize.x += padding.right;
		contentSize.y += padding.bottom;

#if UNITY_EDITOR
		for (int i = 0; i < count - 1; i++)
		{
			if (rectangles[i].Overlaps(rectangles[i + 1].rect))
			{
				Debug.Log("Rect Overlaps:" + i);
			}
		}
		for (int i = 0; i < count; i++)
		{
			int row = i / fixedCount, col = i % fixedCount;
			if (row > 0)
			{
				if (rectangles[i].Overlaps(rectangles[i - fixedCount].rect))
				{
					Debug.Log("Rect Overlaps:" + i + "/" + (i - fixedCount));
				}
			}
		}
#endif
		return contentSize;
	}

	//重写时,需要计算 mContentSize
	protected virtual Vector2 UpdateRectsPosition()
	{
		Vector2 contentSize = Vector2.zero;
		if (!isItemSameSize)
		{
			contentSize = UpdateRectsPosition2();
			return contentSize;
		}
		int count = rectangles.Count;
		int fixedCount = mFixedRowOrColumnCount;
		if (constrain == Constraint.Flexible)
		{
			if (startAxis == Axis.Horizontal)
			{
				float width = content.rect.size.x;
				fixedCount = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + itemSpacing.x + 0.001f) / (itemSize.x + itemSpacing.x)));
			}
			else
			{
				float height = content.rect.size.y;
				fixedCount = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + itemSpacing.y + 0.001f) / (itemSize.y + itemSpacing.y)));
			}
		}

		int cornerX = (int)startCorner % 2;
		int cornerY = (int)startCorner / 2;
		int cellCountX, cellCountY;
		if (fixedCount > count)
		{
			cellCountX = count;
			cellCountY = 1;
		}
		else
		{
			cellCountX = fixedCount;
			cellCountY = count / fixedCount + (count % fixedCount > 0 ? 1 : 0);
		}
		if (startAxis == Axis.Vertical)
		{
			(cellCountX, cellCountY) = (cellCountY, cellCountX);
		}
		Vector2 requiredSpace = new Vector2(
			cellCountX * itemSize.x + (cellCountX - 1) * itemSpacing.x,
			cellCountY * itemSize.y + (cellCountY - 1) * itemSpacing.y
		);
		contentSize = requiredSpace + new Vector2(padding.horizontal, padding.vertical);

		Vector2 startOffset = new Vector2(
			GetStartOffset(0, requiredSpace.x),
			GetStartOffset(1, requiredSpace.y)
		);
		for (int i = 0; i < count; ++i)
		{
			int positionX, positionY;
			if (startAxis == Axis.Horizontal)
			{
				positionX = i % fixedCount;
				positionY = i / fixedCount;
			}
			else
			{
				positionX = i / fixedCount;
				positionY = i % fixedCount;
			}
			if (cornerX == 1)
				positionX = cellCountX - 1 - positionX;
			if (cornerY == 1)
				positionY = cellCountY - 1 - positionY;
			var x = startOffset.x + (itemSize[0] + itemSpacing[0]) * positionX;
			var y = startOffset.y + (itemSize[1] + itemSpacing[1]) * positionY;
			rectangles[i].Reset(x, y, itemSize.x, itemSize.y, i);
		}
		return contentSize;
	}
	protected float GetStartOffset(int axis, float requiredSpaceWithoutPadding)
	{
		float requiredSpace = requiredSpaceWithoutPadding + (axis == 0 ? padding.horizontal : padding.vertical);
		float availableSpace = content.rect.size[axis];
		float surplusSpace = availableSpace - requiredSpace;
		float alignmentOnAxis = GetAlignmentOnAxis(axis);
		return (axis == 0 ? padding.left : padding.top) + surplusSpace * alignmentOnAxis;
	}
	protected float GetAlignmentOnAxis(int axis)
	{
		if (axis == 0)
			return ((int)childAlignment % 3) * 0.5f;
		else
			return ((int)childAlignment / 3) * 0.5f;
	}

	#endregion

	protected int checkId = 0;//防止OnRenderer死循环
	public void UpdateRender()
	{
		this.checkId++;
		var checkId = this.checkId;

		if (content.pivot.x == 0)
			mViewRect.x = -content.anchoredPosition.x;
		else if (content.pivot.x == 1)
			mViewRect.x = content.anchoredPosition.x;
		if (content.pivot.y == 0)
			mViewRect.y = content.anchoredPosition.y + content.rect.height;
		else if (content.pivot.y == 1)
			mViewRect.y = content.anchoredPosition.y;

		inOverlaps.Clear();
		for (int i = 0; i < rectangles.Count; i++)
		{
			var dRect = rectangles[i];
			if (dRect.Overlaps(mViewRect))
			{
				inOverlaps.Add(dRect.Index, dRect);
			}
		}

		//检查item的逻辑矩形是否还在视野范围内
		for (int i = mItemsList.Count - 1; i >= 0; --i)
		{
			var item = mItemsList[i];
			if (item.DRect != null && !inOverlaps.ContainsKey(item.DRect.Index))
			{
				//这个item的逻辑矩形已经离开视野范围了
				//Debug.Log("leave " + item.DRect.Index);
				RemoveItem(i, item);
			}
		}
		foreach (var dRect in inOverlaps.Values)
		{
			//这是一个新加入视野范围内的矩形,为新加入视野的矩形绑定item,并发送OnRenderer事件
			if (dRect.isBind == false)
			{
				//Debug.Log("enter " + dRect.Index);
				LoopScrollItem item = GetItem(dRect.Index, dRect);
				if (forceItemSize)
				{
					item.rectTransform.sizeDelta = dRect.size;
				}
				UpdateItemPos(item, dRect.Index);
				try
				{
					item.OnRenderer();
					if (this.checkId != checkId)
					{
						return;
					}
				}
				catch (Exception e)
				{
					Debug.LogError(e);
				}
			}
		}
		int siblingIndex = 0;
		foreach (var item in mItemsList)
		{
			if (isItemSort)
			{
				item.rectTransform.SetSiblingIndex(siblingIndex);
				siblingIndex++;
			}
			UpdateItemPos(item, item.DRect.Index);
		}
	}

	void ScrollRectEvent(Vector2 value)
	{
		if (mHasInited)
		{
			UpdateRender();
		}
	}

	void ClearItemsList()
	{
		for (int i = mItemsList.Count - 1; i >= 0; --i)
		{
			if (mItemsList[i] != null)
				RemoveItem(i, mItemsList[i]);
			else
			{
				//其实走到这里是异常的，这样做的原因是防止在编辑器模式下手动删除Item，导致为空
				mItemsList.RemoveAt(i);
			}
		}
	}

	#endregion

	public void ClearAllItem()
	{
		ClearItemsList();
		mItemsList.Clear();
		rectangles.Clear();
		inOverlaps.Clear();
		//itemPool.Clear();
	}

	#region public API

	/// <summary>
	/// Callback function when an item is needed to update its look.
	/// </summary>
	/// <param name="index">Item index.</param>
	/// <param name="item">Item object.</param>
	public System.Action<int, GameObject> onRendererItem;
	/// <summary>
	/// Dispatched when a list item being clicked.
	/// </summary>
	/// <param name="index">Item index.</param>
	/// <param name="item">Item object.</param>
	public System.Action<int, GameObject> onClickItem;

	protected int mNumItems = 0;
	/// <summary>
	///  Set the list item count. specified number of items will be created.
	/// </summary>
	public int numItems
	{
		get { return mNumItems; }
		set
		{
			mNumItems = value;
			RefreshAll();
		}
	}

	public bool isItemSameSize
	{
		get { return mIsItemSameSize; }
		set
		{
			mIsItemSameSize = value;
			if (mIsItemSameSize)
			{
				for (int i = 0; i < rectangles.Count; i++)
				{
					rectangles[i].Reset(0, 0, itemSize.x, itemSize.y, i);
				}
			}
		}
	}

	public void RefreshAll()
	{
		Refresh(mNumItems, true);
	}

	/// <summary>
	/// 刷新
	/// </summary>
	/// <param name="count">虚拟Item数量</param>
	/// <param name="refreshAll">刷新全部item（只是减少新增数量和数据没有改变,不需要刷新全部）</param>
	public void Refresh(int count, bool refreshAll = false)
	{
		mNumItems = count;

		Init();
		rollAxis = startAxis == Axis.Horizontal ? Axis.Vertical : Axis.Horizontal;
		UpdateViewRect();
		UpdateRectsCount(mNumItems);
		mContentSize = UpdateRectsPosition();
		UpdateContentSize(mContentSize);
		if (refreshAll)
		{
			ClearItemsList();
		}
		UpdateRender();
	}


	public void RefreshAt(int index)
	{
		var item = TryFindItem(index);
		if (item != null)
		{
			item.OnRenderer();
		}
	}

	public LoopScrollItem TryFindItem(int index)
	{
		for (int i = 0; i < mItemsList.Count; i++)
		{
			if (mItemsList[i].DRect.Index == index)
			{
				return mItemsList[i];
			}
		}
		return null;
	}

	protected int tween_event_id = 0;

	protected Vector2 cachePosition = Vector2.zero;

	void TweenTo(Vector2 a, Vector2 b, float duration)
	{
		content.anchoredPosition = a;
		tween_event_id = DoTween.Instance.Add(a.x, a.y, b.x, b.y, duration, (x, y, obj) =>
		{
			cachePosition.Set(x, y);
			content.anchoredPosition = cachePosition;
			UpdateRender();
		},
		(id, obj) =>
		{
			TweenSotp();
		});
	}

	void TweenSotp()
	{
		if (tween_event_id > 0)
		{
			DoTween.Instance.Remove(tween_event_id);
			tween_event_id = 0;
		}
	}

	/// <summary>
	/// Scroll the list to make an item with certain index visible.
	/// 滚动列表以使具有特定索引的项可见。
	/// </summary>
	/// <param name="index"></param>
	/// <param name="animDuration">移动完成时间</param>
	/// <param name="offsetPercent">额外偏移百分比，值的范围0-1</param>
	/// <param name="offset">额外偏移多少像素</param>
	public void ScrollToView(int index, float animDuration = 0, float offsetPercent = 0, float offset = 0)
	{
		if (index < 0 || index >= mNumItems)
			throw new IndexOutOfRangeException("Locate Index Error " + index);
		var viewport = mScrollRect.viewport;
		Vector2 startPos = content.anchoredPosition;
		Vector2 toPos = startPos;
		if (rollAxis == Axis.Horizontal)
		{
			toPos.x = -rectangles[index].rect.x + (viewport.rect.width * offsetPercent + offset);
			if (toPos.x < -(content.rect.width - mViewRect.width))
				toPos.x = -(content.rect.width - mViewRect.width);
		}
		else
		{
			toPos.y = rectangles[index].rect.y - (viewport.rect.height * offsetPercent + offset);
			if (toPos.y > (content.rect.height - mViewRect.height))
				toPos.y = (content.rect.height - mViewRect.height);
			if (toPos.y < 0)
				toPos.y = 0;
		}

		mScrollRect.StopMovement();
		TweenSotp();
		if (animDuration > 0)
			TweenTo(startPos, toPos, animDuration);
		else
		{
			content.anchoredPosition = toPos;
			UpdateRender();
		}
	}

	/// <summary>
	/// 移动列表到顶部
	/// </summary>
	public void ScrollToTop()
	{
		mScrollRect.StopMovement();
		content.anchoredPosition = Vector2.zero;
		UpdateRender();
	}


	public void AddRect(Vector2 size)
	{
		var index = rectangles.Count;
		rectangles.Add(DRect.Get(0, 0, size.x, size.y, index));
	}

	public void AddRect(float width, float height)
	{
		var index = rectangles.Count;
		rectangles.Add(DRect.Get(0, 0, width, height, index));
	}

	public void RemoveRect(int index)
	{
		DRect.Store(rectangles[index]);
		rectangles.RemoveAt(index);
	}

	public void ChangeRect(int index, float x, float y)
	{
		ChangeRect(index, new Vector2(x, y));
	}
	public void ChangeRect(int index, Vector2 size)
	{
		if (rectangles[index].rect.size != size)
		{
			rectangles[index].SetSize(size.x, size.y);
		}
	}


	#endregion




	#region 兼容其它脚本的API

	public void AddItemRefreshListener(Action<Transform, int> action)
	{
		onRendererItem = (index, item) =>
		{
			action(item.transform, index);
		};
	}

	public void SetVisible(bool isVisible, int count = 0)
	{
		SetActive(isVisible);
		if (isVisible == true)
		{
			numItems = count;
			ScrollToTop();
		}
	}

	protected void SetActive(bool active)
	{
		enabled = active;
		if (active)
		{
			transform.localScale = Vector3.one;
			if (gameObject.activeSelf == false)
			{
				gameObject.SetActive(true);
			}
		}
		else
		{
			transform.localScale = Vector3.zero;
		}
	}

	#endregion

}


