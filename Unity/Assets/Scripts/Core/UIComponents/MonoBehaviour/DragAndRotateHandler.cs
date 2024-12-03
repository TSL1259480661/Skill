using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 拖动旋转指定目标
/// </summary>
public class DragAndRotateHandler : MarkBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	/// <summary>
	/// 拖动检测阀值
	/// </summary>
	public const float drag_threshold = 3F;

	/// <summary>
	/// 旋转因子
	/// </summary>
	public const float rotate_radio = 0.25F;

	/// <summary>
	/// 根结点
	/// </summary>
	public RectTransform rectTransform
	{
		get
		{
			return transform as RectTransform;
		}
	}

	/// <summary>
	/// 上一次位置
	/// </summary>
	protected Vector2 last_position
	{
		get;
		private set;
	}

	/// <summary>
	/// 设置拖动目标
	/// </summary>
	protected Transform target
	{
		get;
		private set;
	}

	/// <summary>
	/// 是否正在拖动中
	/// </summary>
	protected bool bDragStatus = false;

	/// <summary>
	/// 设置旋转目标
	/// </summary>
	public void SetTarget(Transform transform)
	{
		target = transform;
	}

    /// <summary>
	/// 触发拖动
	/// </summary>
	public void OnBeginDrag(PointerEventData eventData)
	{
	    if (target != null)
		{
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.enterEventCamera, out Vector3 globalMousePos))
			{
				bDragStatus = true;
				last_position = eventData.position;
			}
		}
	}

	/// <summary>
	/// 拖动中
	/// </summary>
	public void OnDrag(PointerEventData eventData)
	{
	    if (target != null)
		{
		    if (bDragStatus == true)
			{
				float space = eventData.position.x - last_position.x;
				if (MathF.Abs(space) >= drag_threshold)
				{
					RotateAction(space);
					last_position = eventData.position;
				}
			}
		}
	}

	/// <summary>
	/// 结束拖动
	/// </summary>
	public void OnEndDrag(PointerEventData eventData)
	{
		bDragStatus = false;
		last_position = Vector2.zero;
	}

	/// <summary>
	/// 旋转目标
	/// </summary>
	protected void RotateAction(float space)
	{
	    if (target != null)
		{
			Vector3 euler = target.eulerAngles;
			euler.y -= (space * rotate_radio);
			target.rotation = Quaternion.Euler(euler);
		}
	}
}
