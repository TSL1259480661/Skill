using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Action<PointerEventData> OnBeginDragAction;
	public Action<PointerEventData> OnDragAction;
	public Action<PointerEventData> OnEndDragAction;
	////存储当前拖拽图片的RectTransform组件
	private RectTransform mRect;

	private Vector2 mLeft_Up;

	private Vector2 mRight_Down;

	private Vector2 mLastP;
	public Camera uiCamera;
	private bool isDragging = false;

	public void Init(RectTransform rect, Vector2 leftUp, Vector2 rightDown)
	{
		mRect = rect;
		mLeft_Up = leftUp;
		mRight_Down = rightDown;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
		Vector2 uiLocalPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(mRect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out uiLocalPos);
		_offset = mRect.anchoredPosition - uiLocalPos;

		mLastP = eventData.position;
		OnBeginDragAction?.Invoke(eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 uiLocalPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(mRect.parent as RectTransform, eventData.position, eventData.pressEventCamera, out uiLocalPos);
		mRect.anchoredPosition = uiLocalPos + (Vector2)_offset;

		//if (mRect != null)
		//{
		//	Vector2 vec = eventData.position - mLastP;
		//	Vector2 pos = mRect.anchoredPosition;
		//	float x = pos.x + vec.x;
		//	float y = pos.y + vec.y;
		//	if (x >= mLeft_Up.x && x <= mRight_Down.x) pos.x = x;
		//	if (y >= mRight_Down.y && y <= mLeft_Up.y) pos.y = y;
		//	mRect.anchoredPosition = pos;
		//}

		mLastP = eventData.position;
		OnDragAction?.Invoke(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
		OnEndDragAction?.Invoke(eventData);
	}
	private void OnDisable()
	{
		if (isDragging)
		{
			// Handle any cleanup if the object is disabled during dragging
			isDragging = false;
			OnEndDragAction?.Invoke(null);
		}
	}

	////是否精确拖拽
	//public bool _isPrecision = true;

	//存储图片中心点与鼠标点击点的偏移量
	public Vector3 _offset;

	////存储当前鼠标所在位置
	//public Vector3 _globalMousePos;

	////存储当前拖拽图片的RectTransform组件
	//private RectTransform m_rt;


	//void Start()
	//{
	//    //初始化
	//    m_rt = gameObject.GetComponent<RectTransform>();
	//}

	//public void OnBeginDrag(PointerEventData eventData)
	//{
	//    if (m_rt == null)
	//    {
	//        m_rt = gameObject.GetComponent<RectTransform>();
	//    }
	//    //如果精确拖拽则进行计算偏移量操作
	//    if (_isPrecision)
	//    {
	//        // 存储点击时的鼠标坐标
	//        Vector3 tWorldPos;
	//        //UI屏幕坐标转换为世界坐标
	//        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rt, eventData.position, eventData.pressEventCamera, out tWorldPos);
	//        //计算偏移量
	//        _offset = transform.position - tWorldPos;
	//    }
	//    //否则，默认偏移量为0
	//    else
	//    {
	//        _offset = Vector3.zero;
	//    }
	//    SetDraggedPosition(eventData);
	//    OnBeginDragAction?.Invoke(eventData);
	//}

	//public void OnDrag(PointerEventData eventData)
	//{
	//    SetDraggedPosition(eventData);
	//    OnDragAction?.Invoke(eventData);
	//}

	//public void OnEndDrag(PointerEventData eventData)
	//{
	//    SetDraggedPosition(eventData);
	//    OnEndDragAction?.Invoke(eventData);
	//}

	///// <summary>
	///// 设置图片位置方法
	///// </summary>
	///// <param name="eventData"></param>
	//private void SetDraggedPosition(PointerEventData eventData)
	//{
	//    //UI屏幕坐标转换为世界坐标
	//    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rt, eventData.position, eventData.pressEventCamera, out _globalMousePos))
	//    {
	//        //设置位置及偏移量
	//        m_rt.position = _globalMousePos + _offset;
	//    }
	//}
}
