using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 镂空效果的Mask组件
/// </summary>
public class HollowOutMask  : MaskableGraphic, IPointerClickHandler
{
	/// <summary>
	/// 镂空目标
	/// </summary>
	private RectTransform target;

	/// <summary>
	/// 圆形区域信息
	/// </summary>
	private GuideCircleImage circle;

	/// <summary>
	/// 过程值
	/// </summary>
	private Vector3 cachePosition = Vector3.zero;

	/// <summary>
	/// 目标点 - min
	/// </summary>
	private Vector3 target_min = Vector3.zero;

	/// <summary>
	/// 目标点 - max
	/// </summary>
	private Vector3 target_max = Vector3.zero;

	/// <summary>
	/// 响应回调
	/// </summary>
	private Action callBack = null;

	/// <summary>
	/// 完成条件
	/// </summary>
	private string condition = string.Empty;

	/// <summary>
	/// 是否为全屏区域响应跳过
	/// </summary>
	private bool bFullScreenResponseArea = false;

	/// <summary>
	/// 射线检测结果
	/// </summary>
	private List<RaycastResult> raycastResult = new List<RaycastResult>();

	/// <summary>
	/// 初始化
	/// </summary>
	public void Init(Action action)
	{
		Hide();
		callBack = action;
		circle = transform.Find("#Circle").GetComponent<GuideCircleImage>();
	}

	/// <summary>
	/// 隐藏
	/// </summary>
	public void Hide()
	{
		target = null;
		condition = null;
		bFullScreenResponseArea = false;
		transform.localScale = Vector3.zero;
		RefreshView();
	}

	/// <summary>
	/// 显示
	/// </summary>
	public void Show(RectTransform target, RectTransform view, bool bDrawCircle = false, bool fullScreenResponseArea = false, string condition = null)
	{
		this.target = target;
		this.condition = condition;
		bFullScreenResponseArea = fullScreenResponseArea;
		transform.localScale = Vector3.one;
		SetAreaRectTransfrom(view, bDrawCircle);
		RefreshView();
	}

	/// <summary>
	/// 条件触发结束引导
	/// </summary>
	public void DoConditionTriggerCloseAction(string key)
	{
		if (!string.IsNullOrEmpty(condition))
		{
			if (condition.Equals(key))
			{
				callBack?.Invoke();
			}
		}
	}

	/// <summary>
	/// 点击镂空区域是否有效
	/// </summary>
	public void OnPointerClick(PointerEventData eventData)
	{
		if (null == target || bFullScreenResponseArea)
		{
			TriggerCallBack();
		}
		else if (RectTransformUtility.RectangleContainsScreenPoint(target, eventData.position, eventData.pressEventCamera))
		{
			Button button = target.GetComponent<Button>();
			if (button != null || RaycastButton(eventData, out button))
			{
				if (button.enabled && button.interactable)
				{
					TriggerCallBack();
					if (button is GButton)
					{
						(button as GButton).DoForceClickAction(eventData);
					}
					else
					{
						button.onClick.Invoke();
					}
				}
			}
			else
			{
				TriggerCallBack();
			}
		}
	}

	/// <summary>
	/// 响应回调
	/// </summary>
	private void TriggerCallBack()
	{
		if (string.IsNullOrEmpty(condition))
		{
			callBack?.Invoke();
		}
	}

	/// <summary>
	/// 设置Area
	/// </summary>
	private void SetAreaRectTransfrom(RectTransform view, bool bDrawCircle)
	{
	    // 展示区域
		if (view != null)
		{
			circle.rectTransform.pivot = view.pivot;
			circle.rectTransform.offsetMin = view.offsetMin;
			circle.rectTransform.offsetMax = view.offsetMax;
			circle.rectTransform.rotation = view.rotation;
			circle.rectTransform.position = view.position;
			circle.rectTransform.sizeDelta = view.rect.size;
			cachePosition.Set(circle.rectTransform.localPosition.x, circle.rectTransform.localPosition.y, 0F);
			circle.rectTransform.localPosition = cachePosition;
			circle.enabled = bDrawCircle;
		}
	}

	/// <summary>
	/// 设置镂空区域
	/// </summary>
	private void SetTargetRect(Vector3 min, Vector3 max)
	{
		if (target_min == min && target_max == max)
		{
			return;
		}
		target_min = min;
		target_max = max;
		SetAllDirty();
	}

	/// <summary>
	/// 刷新镂空视图
	/// </summary>
	private void RefreshView()
	{
		if (null == target)
		{
			SetTargetRect(Vector3.zero, Vector3.zero);
		}
		else
		{
			Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, circle.rectTransform);
			SetTargetRect(bounds.min, bounds.max);
		}
	}

	/// <summary>
	/// 重写绘制
	/// </summary>
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		if (target_min == Vector3.zero && target_max == Vector3.zero)
		{
			base.OnPopulateMesh(vh);
			return;
		}
		else
		{
			DrawRectangle(vh);
		}
	}

	/// <summary>
	/// 射线检测按钮
	/// </summary>
	protected bool RaycastButton(PointerEventData eventData, out Button button)
	{
		button = null;
		if (EventSystem.current != null)
		{
			raycastResult.Clear();
			EventSystem.current.RaycastAll(eventData, raycastResult);
			if (raycastResult.Count > 0)
			{
				foreach (RaycastResult result in raycastResult)
				{
					if (result.gameObject != null)
					{
						button = result.gameObject.GetComponent<Button>();
						if (button != null)
						{
							raycastResult.Clear();
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	/// <summary>
	/// 绘制矩形裁剪
	/// </summary>
	protected void DrawRectangle(VertexHelper vh)
	{
		vh.Clear();

		// 填充顶点
		UIVertex vert = UIVertex.simpleVert;
		vert.color = color;

		Vector2 piovt = rectTransform.pivot;
		Rect rect = rectTransform.rect;
		float outer_lx = -piovt.x * rect.width;
		float outer_by = -piovt.y * rect.height;
		float outer_rx = (1F - piovt.x) * rect.width;
		float outer_ty = (1F - piovt.y) * rect.height;

		// 0 - Outer : LT
		vert.position = new Vector3(outer_lx, outer_ty);
		vh.AddVert(vert);

		// 1 - Outer : RT
		vert.position = new Vector3(outer_rx, outer_ty);
		vh.AddVert(vert);

		// 2 - Outer : RB
		vert.position = new Vector3(outer_rx, outer_by);
		vh.AddVert(vert);

		// 3 - Outer : LB
		vert.position = new Vector3(outer_lx, outer_by);
		vh.AddVert(vert);

		// 4 - Inner : LT
		vert.position = new Vector3(target_min.x, target_max.y);
		vh.AddVert(vert);

		// 5 - Inner : RT
		vert.position = new Vector3(target_max.x, target_max.y);
		vh.AddVert(vert);

		// 6 - Inner : RB
		vert.position = new Vector3(target_max.x, target_min.y);
		vh.AddVert(vert);

		// 7 - Inner : LB
		vert.position = new Vector3(target_min.x, target_min.y);
		vh.AddVert(vert);

		// 设定三角形
		vh.AddTriangle(4, 0, 1);
		vh.AddTriangle(4, 1, 5);
		vh.AddTriangle(5, 1, 2);
		vh.AddTriangle(5, 2, 6);
		vh.AddTriangle(6, 2, 3);
		vh.AddTriangle(6, 3, 7);
		vh.AddTriangle(7, 3, 0);
		vh.AddTriangle(7, 0, 4);
	}
}
