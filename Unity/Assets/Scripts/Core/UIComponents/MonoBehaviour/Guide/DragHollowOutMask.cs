using System;
using System.Collections.Generic;
using UIEventType;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 拖动镂空效果组件
/// </summary>
public class DragHollowOutMask : MaskableGraphic, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	/// <summary>
	/// 拖动镂空区域
	/// </summary>
	[SerializeField]
	private RectTransform sourceTarget;

	/// <summary>
	/// 响应镂空区域
	/// </summary>
	[SerializeField]
	private RectTransform responseTagret;

	/// <summary>
	/// 同层区域 - 拖动区域
	/// </summary>
	private RectTransform sourceArea;

	/// <summary>
	/// 同层区域 - 响应区域
	/// </summary>
	private RectTransform responseArea;

	/// <summary>
	/// 拖动区域
	/// </summary>
	private Vector3 source_min = Vector3.zero;

	/// <summary>
	/// 拖动区域
	/// </summary>
	private Vector3 source_max = Vector3.zero;

	/// <summary>
	/// 响应区域
	/// </summary>
	private Vector3 response_min = Vector3.zero;

	/// <summary>
	/// 响应区域
	/// </summary>
	private Vector3 response_max = Vector3.zero;

	/// <summary>
	/// 过程值
	/// </summary>
	private Vector3 cachePosition = Vector3.zero;

	/// <summary>
	/// 射线检测结果
	/// </summary>
	private List<RaycastResult> raycastResult = new List<RaycastResult>();

	/// <summary>
	/// 当前正在拖动的对象
	/// </summary>
	private Transform drag = null;

	/// <summary>
	/// 响应回调
	/// </summary>
	private Action callBack = null;

	/// <summary>
	/// 完成条件
	/// </summary>
	private string condition = string.Empty;

	/// <summary>
	/// 是否为拖动Source
	/// </summary>
	private bool bDragSource = true;

	/// <summary>
	/// 初始化
	/// </summary>
	public void Init(Action action)
	{
		HideAction();
		callBack = action;
		sourceArea = transform.Find("#SourceArea").GetComponent<RectTransform>();
		responseArea = transform.Find("#ResponseArea").GetComponent<RectTransform>();
	}

	/// <summary>
	/// 结束引导
	/// </summary>
	public void HideAction()
	{
		drag = null;
		bDragSource = true;
		sourceTarget = null;
		responseTagret = null;
		transform.localScale = Vector3.zero;
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
	/// 设置镂空的目标
	/// </summary>
	public void Show(RectTransform source, RectTransform response, RectTransform source_view, RectTransform response_view, string condition)
	{
		sourceTarget = source;
		responseTagret = response;
		this.condition = condition;
		transform.localScale = Vector3.one;
		SetSourceArea(source_view);
		SetResponseArea(response_view);
		RefreshView();
	}

	/// <summary>
	/// 设置拖动区域
	/// </summary>
	private void SetSourceArea(RectTransform source_view)
	{
		if (source_view != null)
		{
			sourceArea.pivot = source_view.pivot;
			sourceArea.offsetMin = source_view.offsetMin;
			sourceArea.offsetMax = source_view.offsetMax;
			sourceArea.rotation = source_view.rotation;
			sourceArea.position = source_view.position;
			sourceArea.sizeDelta = source_view.rect.size;
			cachePosition.Set(sourceArea.localPosition.x, sourceArea.localPosition.y, 0F);
			sourceArea.localPosition = cachePosition;
		}
		else
		{
			sourceArea.position = Vector3.zero;
			sourceArea.sizeDelta = Vector2.zero;
		}
	}

	/// <summary>
	/// 设置响应区域
	/// </summary>
	private void SetResponseArea(RectTransform response_view)
	{
		if (response_view != null)
		{
			responseArea.pivot = response_view.pivot;
			responseArea.offsetMin = response_view.offsetMin;
			responseArea.offsetMax = response_view.offsetMax;
			responseArea.rotation = response_view.rotation;
			responseArea.position = response_view.position;
			responseArea.sizeDelta = response_view.rect.size;
			cachePosition.Set(responseArea.localPosition.x, responseArea.localPosition.y, 0F);
			responseArea.localPosition = cachePosition;
		}
		else
		{
			responseArea.position = Vector3.zero;
			responseArea.sizeDelta = Vector2.zero;
		}
	}

	/// <summary>
	/// 刷新镂空
	/// </summary>
	private void RefreshView()
	{
		if (null == sourceTarget || null == responseTagret)
		{
			SetTargetRect(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero);
		}
		else
		{
			SetViewRect();
		}
	}

	/// <summary>
	/// 设置镂空区域
	/// </summary>
	private void SetViewRect()
	{
		Bounds sourceBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, sourceArea);
		Bounds responseBounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, responseArea);
		SetTargetRect(sourceBounds.min, sourceBounds.max, responseBounds.min, responseBounds.max);
	}

	/// <summary>
	/// 设置镂空区域
	/// </summary>
	private void SetTargetRect(Vector3 src_min, Vector3 src_max, Vector3 resp_min, Vector3 resp_max)
	{
		if (source_min == src_min && source_max == src_max && response_min == resp_min && response_max == resp_max)
		{
			return;
		}
		source_min = src_min;
		source_max = src_max;
		response_min = resp_min;
		response_max = resp_max;
		SetAllDirty();
	}

	/// <summary>
	/// 重写绘制
	/// </summary>
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		// 绘制方式
		if (source_max.x < response_min.x)
		{
			DrawPopulateMeshA(vh);
		}
		else if (response_max.x < source_min.x)
		{
			DrawPopulateMeshB(vh);
		}
		else if (source_max.y < response_min.y)
		{
			DrawPopulateMeshC(vh);
		}
		else if (response_max.y < source_min.y)
		{
			DrawPopulateMeshD(vh);
		}
		else
		{
			base.OnPopulateMesh(vh);
		}
	}

	/// <summary>
	/// 拖动区域在响应区域左边
	/// </summary>
	protected void DrawPopulateMeshA(VertexHelper vh)
	{
		vh.Clear();

		// 填充顶点
		UIVertex vert = UIVertex.simpleVert;
		vert.color = color;

		// 外围
		Vector2 piovt = rectTransform.pivot;
		Rect rect = rectTransform.rect;
		float outer_lx = -piovt.x * rect.width;
		float outer_by = -piovt.y * rect.height;
		float outer_rx = (1F - piovt.x) * rect.width;
		float outer_ty = (1F - piovt.y) * rect.height;

		// 分割点
		float division = source_max.x + (response_min.x - source_max.x) / 2F;

		// 0 - Outer : LT
		vert.position = new Vector3(outer_lx, outer_ty);
		vh.AddVert(vert);

		// 1 - Outer : DT
		vert.position = new Vector3(division, outer_ty);
		vh.AddVert(vert);

		// 2 - Outer : RT
		vert.position = new Vector3(outer_rx, outer_ty);
		vh.AddVert(vert);

		// 3 - Outer : RB
		vert.position = new Vector3(outer_rx, outer_by);
		vh.AddVert(vert);

		// 4 - Outer : DB
		vert.position = new Vector3(division, outer_by);
		vh.AddVert(vert);

		// 5 - Outer : LB
		vert.position = new Vector3(outer_lx, outer_by);
		vh.AddVert(vert);

		// 6 - Source : LT
		vert.position = new Vector3(source_min.x, source_max.y);
		vh.AddVert(vert);

		// 7 - Source : RT
		vert.position = new Vector3(source_max.x, source_max.y);
		vh.AddVert(vert);

		// 8 - Source : RB
		vert.position = new Vector3(source_max.x, source_min.y);
		vh.AddVert(vert);

		// 9 - Source : LB
		vert.position = new Vector3(source_min.x, source_min.y);
		vh.AddVert(vert);

		// 10 - Response : LT
		vert.position = new Vector3(response_min.x, response_max.y);
		vh.AddVert(vert);

		// 11 - Response : RT
		vert.position = new Vector3(response_max.x, response_max.y);
		vh.AddVert(vert);

		// 12 - Response : RB
		vert.position = new Vector3(response_max.x, response_min.y);
		vh.AddVert(vert);

		// 13 - Response : LB
		vert.position = new Vector3(response_min.x, response_min.y);
		vh.AddVert(vert);

		// 设定三角形(Source)
		vh.AddTriangle(6, 0, 1);
		vh.AddTriangle(6, 1, 7);
		vh.AddTriangle(7, 1, 4);
		vh.AddTriangle(7, 4, 8);
		vh.AddTriangle(8, 4, 5);
		vh.AddTriangle(8, 5, 9);
		vh.AddTriangle(9, 5, 0);
		vh.AddTriangle(9, 0, 6);

		// 设置三角形（Response）
		vh.AddTriangle(10, 1, 2);
		vh.AddTriangle(10, 2, 11);
		vh.AddTriangle(11, 2, 3);
		vh.AddTriangle(11, 3, 12);
		vh.AddTriangle(12, 3, 4);
		vh.AddTriangle(12, 4, 13);
		vh.AddTriangle(13, 4, 1);
		vh.AddTriangle(13, 1, 10);
	}

	/// <summary>
	/// 拖动区域在响应区域右边
	/// </summary>
	protected void DrawPopulateMeshB(VertexHelper vh)
	{
		vh.Clear();

		// 填充顶点
		UIVertex vert = UIVertex.simpleVert;
		vert.color = color;

		// 外围
		Vector2 piovt = rectTransform.pivot;
		Rect rect = rectTransform.rect;
		float outer_lx = -piovt.x * rect.width;
		float outer_by = -piovt.y * rect.height;
		float outer_rx = (1F - piovt.x) * rect.width;
		float outer_ty = (1F - piovt.y) * rect.height;

		// 分割点
		float division = response_max.x + (source_min.x - response_max.x) / 2F;

		// 0 - Outer : LT
		vert.position = new Vector3(outer_lx, outer_ty);
		vh.AddVert(vert);

		// 1 - Outer : DT
		vert.position = new Vector3(division, outer_ty);
		vh.AddVert(vert);

		// 2 - Outer : RT
		vert.position = new Vector3(outer_rx, outer_ty);
		vh.AddVert(vert);

		// 3 - Outer : RB
		vert.position = new Vector3(outer_rx, outer_by);
		vh.AddVert(vert);

		// 4 - Outer : DB
		vert.position = new Vector3(division, outer_by);
		vh.AddVert(vert);

		// 5 - Outer : LB
		vert.position = new Vector3(outer_lx, outer_by);
		vh.AddVert(vert);

		// 6 - Source : LT
		vert.position = new Vector3(source_min.x, source_max.y);
		vh.AddVert(vert);

		// 7 - Source : RT
		vert.position = new Vector3(source_max.x, source_max.y);
		vh.AddVert(vert);

		// 8 - Source : RB
		vert.position = new Vector3(source_max.x, source_min.y);
		vh.AddVert(vert);

		// 9 - Source : LB
		vert.position = new Vector3(source_min.x, source_min.y);
		vh.AddVert(vert);

		// 10 - Response : LT
		vert.position = new Vector3(response_min.x, response_max.y);
		vh.AddVert(vert);

		// 11 - Response : RT
		vert.position = new Vector3(response_max.x, response_max.y);
		vh.AddVert(vert);

		// 12 - Response : RB
		vert.position = new Vector3(response_max.x, response_min.y);
		vh.AddVert(vert);

		// 13 - Response : LB
		vert.position = new Vector3(response_min.x, response_min.y);
		vh.AddVert(vert);

		// 设置三角形（Source）
		vh.AddTriangle(6, 1, 2);
		vh.AddTriangle(6, 2, 7);
		vh.AddTriangle(7, 2, 3);
		vh.AddTriangle(7, 3, 8);
		vh.AddTriangle(8, 3, 4);
		vh.AddTriangle(8, 4, 9);
		vh.AddTriangle(9, 4, 1);
		vh.AddTriangle(9, 1, 6);

		// 设定三角形(Response)
		vh.AddTriangle(10, 0, 1);
		vh.AddTriangle(10, 1, 11);
		vh.AddTriangle(11, 1, 4);
		vh.AddTriangle(11, 4, 12);
		vh.AddTriangle(12, 4, 5);
		vh.AddTriangle(12, 5, 13);
		vh.AddTriangle(13, 5, 0);
		vh.AddTriangle(13, 0, 10);
	}

	/// <summary>
	/// 拖动区域在响应区域下边
	/// </summary>
	protected void DrawPopulateMeshC(VertexHelper vh)
	{
		vh.Clear();

		// 填充顶点
		UIVertex vert = UIVertex.simpleVert;
		vert.color = color;

		// 外围
		Vector2 piovt = rectTransform.pivot;
		Rect rect = rectTransform.rect;
		float outer_lx = -piovt.x * rect.width;
		float outer_by = -piovt.y * rect.height;
		float outer_rx = (1F - piovt.x) * rect.width;
		float outer_ty = (1F - piovt.y) * rect.height;

		// 分割点
		float division = source_max.y + (response_min.y - source_max.y) / 2F;

		// 0 - Outer : LT
		vert.position = new Vector3(outer_lx, outer_ty);
		vh.AddVert(vert);

		// 1 - Outer : RT
		vert.position = new Vector3(outer_rx, outer_ty);
		vh.AddVert(vert);

		// 2 - Outer : RD
		vert.position = new Vector3(outer_rx, division);
		vh.AddVert(vert);

		// 3 - Outer : RB
		vert.position = new Vector3(outer_rx, outer_by);
		vh.AddVert(vert);

		// 4 - Outer : LB
		vert.position = new Vector3(outer_lx, outer_by);
		vh.AddVert(vert);

		// 5 - Outer : LD
		vert.position = new Vector3(outer_lx, division);
		vh.AddVert(vert);

		// 6 - Source : LT
		vert.position = new Vector3(source_min.x, source_max.y);
		vh.AddVert(vert);

		// 7 - Source : RT
		vert.position = new Vector3(source_max.x, source_max.y);
		vh.AddVert(vert);

		// 8 - Source : RB
		vert.position = new Vector3(source_max.x, source_min.y);
		vh.AddVert(vert);

		// 9 - Source : LB
		vert.position = new Vector3(source_min.x, source_min.y);
		vh.AddVert(vert);

		// 10 - Response : LT
		vert.position = new Vector3(response_min.x, response_max.y);
		vh.AddVert(vert);

		// 11 - Response : RT
		vert.position = new Vector3(response_max.x, response_max.y);
		vh.AddVert(vert);

		// 12 - Response : RB
		vert.position = new Vector3(response_max.x, response_min.y);
		vh.AddVert(vert);

		// 13 - Response : LB
		vert.position = new Vector3(response_min.x, response_min.y);
		vh.AddVert(vert);

		// 设置三角形（Source）
		vh.AddTriangle(6, 5, 2);
		vh.AddTriangle(6, 2, 7);
		vh.AddTriangle(7, 2, 3);
		vh.AddTriangle(7, 3, 8);
		vh.AddTriangle(8, 3, 4);
		vh.AddTriangle(8, 4, 9);
		vh.AddTriangle(9, 4, 5);
		vh.AddTriangle(9, 5, 6);

		// 设定三角形(Response)
		vh.AddTriangle(10, 0, 1);
		vh.AddTriangle(10, 1, 11);
		vh.AddTriangle(11, 1, 2);
		vh.AddTriangle(11, 2, 12);
		vh.AddTriangle(12, 2, 5);
		vh.AddTriangle(12, 5, 13);
		vh.AddTriangle(13, 5, 0);
		vh.AddTriangle(13, 0, 10);
	}

	/// <summary>
	/// 拖动区域在响应区域上边
	/// </summary>
	protected void DrawPopulateMeshD(VertexHelper vh)
	{
		vh.Clear();

		// 填充顶点
		UIVertex vert = UIVertex.simpleVert;
		vert.color = color;

		// 外围
		Vector2 piovt = rectTransform.pivot;
		Rect rect = rectTransform.rect;
		float outer_lx = -piovt.x * rect.width;
		float outer_by = -piovt.y * rect.height;
		float outer_rx = (1F - piovt.x) * rect.width;
		float outer_ty = (1F - piovt.y) * rect.height;

		// 分割点
		float division = response_max.y + (source_min.y - response_max.y) / 2F;

		// 0 - Outer : LT
		vert.position = new Vector3(outer_lx, outer_ty);
		vh.AddVert(vert);

		// 1 - Outer : RT
		vert.position = new Vector3(outer_rx, outer_ty);
		vh.AddVert(vert);

		// 2 - Outer : RD
		vert.position = new Vector3(outer_rx, division);
		vh.AddVert(vert);

		// 3 - Outer : RB
		vert.position = new Vector3(outer_rx, outer_by);
		vh.AddVert(vert);

		// 4 - Outer : LB
		vert.position = new Vector3(outer_lx, outer_by);
		vh.AddVert(vert);

		// 5 - Outer : LD
		vert.position = new Vector3(outer_lx, division);
		vh.AddVert(vert);

		// 6 - Source : LT
		vert.position = new Vector3(source_min.x, source_max.y);
		vh.AddVert(vert);

		// 7 - Source : RT
		vert.position = new Vector3(source_max.x, source_max.y);
		vh.AddVert(vert);

		// 8 - Source : RB
		vert.position = new Vector3(source_max.x, source_min.y);
		vh.AddVert(vert);

		// 9 - Source : LB
		vert.position = new Vector3(source_min.x, source_min.y);
		vh.AddVert(vert);

		// 10 - Response : LT
		vert.position = new Vector3(response_min.x, response_max.y);
		vh.AddVert(vert);

		// 11 - Response : RT
		vert.position = new Vector3(response_max.x, response_max.y);
		vh.AddVert(vert);

		// 12 - Response : RB
		vert.position = new Vector3(response_max.x, response_min.y);
		vh.AddVert(vert);

		// 13 - Response : LB
		vert.position = new Vector3(response_min.x, response_min.y);
		vh.AddVert(vert);

		// 设置三角形（Source）
		vh.AddTriangle(6, 0, 1);
		vh.AddTriangle(6, 1, 7);
		vh.AddTriangle(7, 1, 2);
		vh.AddTriangle(7, 2, 8);
		vh.AddTriangle(8, 2, 4);
		vh.AddTriangle(8, 5, 9);
		vh.AddTriangle(9, 5, 0);
		vh.AddTriangle(9, 0, 6);

		// 设定三角形(Response)
		vh.AddTriangle(10, 5, 2);
		vh.AddTriangle(10, 2, 11);
		vh.AddTriangle(11, 2, 3);
		vh.AddTriangle(11, 3, 12);
		vh.AddTriangle(12, 3, 4);
		vh.AddTriangle(12, 4, 13);
		vh.AddTriangle(13, 4, 5);
		vh.AddTriangle(13, 5, 10);
	}

	/// <summary>
	/// 开始拖动
	/// </summary>
	public void OnBeginDrag(PointerEventData eventData)
	{
		Transform eventDrag = null;
		if (RectTransformUtility.RectangleContainsScreenPoint(sourceTarget, eventData.position, eventData.pressEventCamera))
		{
			if (ComponentDragTarget(sourceTarget.gameObject, out eventDrag) || RaycastDragTarget(eventData, out eventDrag))
			{
				if (eventDrag.gameObject.activeSelf)
				{
					drag = eventDrag;
					bDragSource = true;
				}
			}
		}
		else if (RectTransformUtility.RectangleContainsScreenPoint(responseTagret, eventData.position, eventData.pressEventCamera))
		{
			if (ComponentDragTarget(responseTagret.gameObject, out eventDrag) || RaycastDragTarget(eventData, out eventDrag))
			{
				if (eventDrag.gameObject.activeSelf)
				{
					drag = eventDrag;
					bDragSource = false;
				}
			}
		}
		else
		{
			return;
		}

		// 拖动有效
		if (drag != null)
		{
			ExecuteEvents.Execute(drag.gameObject, eventData, ExecuteEvents.beginDragHandler);
		}
	}

	/// <summary>
	/// 拖动中
	/// </summary>
	public void OnDrag(PointerEventData eventData)
	{
	    if (drag != null)
		{
			ExecuteEvents.Execute(drag.gameObject, eventData, ExecuteEvents.dragHandler);
		}
	}

	/// <summary>
	/// 结束拖动
	/// </summary>
	public void OnEndDrag(PointerEventData eventData)
	{
		if (drag != null)
		{
			RectTransform response = bDragSource ? responseTagret : sourceTarget;
			if (response != null)
			{
				bool bResponseArea = RectTransformUtility.RectangleContainsScreenPoint(response, eventData.position, eventData.enterEventCamera);
				if (bResponseArea)
				{
					ReycastNewPointerCurrentRaycast(ref eventData);
				}
				if (ExecuteEvents.Execute(drag.gameObject, eventData, ExecuteEvents.endDragHandler))
				{
					if (string.IsNullOrEmpty(condition) && bResponseArea)
					{
						callBack?.Invoke();
					}
				}
			}
			drag = null;
		}
	}

	/// <summary>
	/// 自身控件拖动查找
	/// </summary>
	protected bool ComponentDragTarget(GameObject go, out Transform drag)
	{
		// 拖动控件 - 1
		UIDragHandler uiDragHandler = go.GetComponent<UIDragHandler>();
		if (uiDragHandler != null)
		{
			drag = uiDragHandler.transform;
			raycastResult.Clear();
			return true;
		}

		// 拖动控件 - 2
		DragHandler dragHandler = go.GetComponent<DragHandler>();
		if (dragHandler != null)
		{
			drag = dragHandler.transform;
			raycastResult.Clear();
			return true;
		}

		// 未找到
		drag = null;
		return false;
	}

	/// <summary>
	/// 射线检测拖动控件
	/// </summary>
	protected bool RaycastDragTarget(PointerEventData eventData, out Transform drag)
	{
		drag = null;
		if (EventSystem.current != null)
		{
			raycastResult.Clear();
			EventSystem.current.RaycastAll(eventData, raycastResult);
			if (raycastResult.Count > 0)
			{
				foreach (RaycastResult result in raycastResult)
				{
					if (result.gameObject != null && ComponentDragTarget(result.gameObject, out drag))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	/// <summary>
	/// 重新筛选对象
	/// </summary>
	protected void ReycastNewPointerCurrentRaycast(ref PointerEventData eventData)
	{
	    // 发现是自己时，重新筛选
	    if (eventData.pointerCurrentRaycast.gameObject == gameObject)
		{
			if (EventSystem.current != null)
			{
				raycastResult.Clear();
				EventSystem.current.RaycastAll(eventData, raycastResult);
				if (raycastResult.Count > 0)
				{
					foreach (RaycastResult result in raycastResult)
					{
						if (result.gameObject != null && result.gameObject != gameObject)
						{
							eventData.pointerCurrentRaycast = result;
							return;
						}
					}
				}
			}
		}
	}
}
