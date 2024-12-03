using System;
using System.Collections.Generic;
using UIEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MarkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Action<PointerEventData, RectTransform> OnBeginDragAction;
	public Action<PointerEventData, RectTransform> OnDragAction;
	public Action<PointerEventData, RectTransform> OnEndDragAction;
	public Action<IObjectPoolItem, Image> OnEnterHandlerAction;
	public Action<IObjectPoolItem, Image> OnExitHandlerAction;
	public bool isResetPosition = true;

	public Canvas canvas
	{
		get;
		set;
	}

	public RectTransform canvas_root
	{
		get;
		set;
	}

	public Vector2 anchoredPosition
	{
		get;
		set;
	}

	public Transform parent
	{
		get;
		set;
	}

	public IObjectPoolItem target
	{
		get;
		set;
	}

	protected Image image;
	protected float minX, maxX, minY, maxY;

	/// <summary>
	/// 初始化
	/// </summary>
	/// <param name="ui"></param>
	public void Init(Canvas canvas, RectTransform canvas_root)
	{
		this.canvas = canvas;
		this.canvas_root = canvas_root;
		if (image == null)
		{
			image = GetComponent<Image>();
		}
	}

	/// <summary>
	/// 进入拖动区域
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (image != null)
		{
			OnEnterHandlerAction?.Invoke(target, image);
		}
	}

	/// <summary>
	/// 离开拖动区域
	/// </summary>
	public void OnPointerExit(PointerEventData eventData)
	{
		if (image != null)
		{
			OnExitHandlerAction?.Invoke(target, image);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (image != null)
		{
			image.raycastTarget = false;
			OnBeginDragAction?.Invoke(eventData, image.rectTransform);
			parent = transform.parent;
			anchoredPosition = image.rectTransform.anchoredPosition;
			transform.SetParent(canvas.transform);
			transform.localScale = CommonConfig.dragIconScale;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(image.rectTransform, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
			{
				SetDragRange();
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (image != null)
		{
			if (image.raycastTarget)
			{
				return;
			}
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(image.rectTransform, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
			{
				transform.position = DragRangeLimit(globalMousePos);
			}
			OnDragAction?.Invoke(eventData, image.rectTransform);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (image != null)
		{
			if (image.raycastTarget)
			{
				return;
			}
			image.raycastTarget = true;
			transform.SetParent(parent);
			if (isResetPosition)
				image.rectTransform.anchoredPosition = anchoredPosition;
			image.rectTransform.localScale = Vector3.one;
			OnEndDragAction?.Invoke(eventData, image.rectTransform);
		}
	}

	/// <summary>
	/// 设置拖动的范围
	/// </summary>
	/// <param name="self"></param>
	public void SetDragRange()
	{
		// 最小x坐标 = 容器当前x坐标 - 容器轴心距离左边界的距离 + UI轴心距离左边界的距离
		minX = canvas_root.position.x - canvas_root.pivot.x * canvas_root.rect.width * canvas.scaleFactor
				+ image.rectTransform.rect.width * canvas.scaleFactor * image.rectTransform.pivot.x;

		// 最大x坐标 = 容器当前x坐标 + 容器轴心距离右边界的距离 - UI轴心距离右边界的距离
		maxX = canvas_root.position.x + (1 - canvas_root.pivot.x) * canvas_root.rect.width * canvas.scaleFactor
				- image.rectTransform.rect.width * canvas.scaleFactor * (1 - image.rectTransform.pivot.x);

		// 最小y坐标 = 容器当前y坐标 - 容器轴心距离底边的距离 + UI轴心距离底边的距离
		minY = canvas_root.position.y - canvas_root.pivot.y * canvas_root.rect.height * canvas.scaleFactor
				+ image.rectTransform.rect.height * canvas.scaleFactor * image.rectTransform.pivot.y;

		// 最大y坐标 = 容器当前x坐标 + 容器轴心距离顶边的距离 - UI轴心距离顶边的距离
		maxY = canvas_root.position.y + (1 - canvas_root.pivot.y) * canvas_root.rect.height * canvas.scaleFactor
				- image.rectTransform.rect.height * canvas.scaleFactor * (1 - image.rectTransform.pivot.y);
	}

	public Vector3 DragRangeLimit(Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, minX, maxX);
		pos.y = Mathf.Clamp(pos.y, minY, maxY);
		return pos;
	}
}
