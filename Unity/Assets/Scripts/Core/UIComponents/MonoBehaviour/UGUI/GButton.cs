using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 透传类型
/// </summary>
public enum ePassEventType
{
	/// <summary>
	/// 不透传
	/// </summary>
	none,

	/// <summary>
	/// 自身先响应后再透传
	/// </summary>
	order,

	/// <summary>
	/// 先透传后再自身响应
	/// </summary>
	reverse,
}

public class GButton : Button, ISound
{
	//单击
	[Tooltip("单击间隔,小于0表示使用默认值")]
	public float singleClickInterval = -1;
	private float singleLastClickTime = 0;
	//双击
	[Tooltip("双击间隔阈值,小于0表示使用默认值")]
	public float doubleClickThreshold = -1;
	private float doubleLastClickTime = 0;
	private bool isFirstClick = true;
	public UnityEvent onClickDouble = new UnityEvent();
	//长按
	[Tooltip("长按所需时间,小于等于0表示使用默认值")]
	public float longPressDuration = -1;
	[Tooltip("长按重复触发间隔，小于0表示只触发1次,等于0无限触发")]
	public float longRepeatInterval = -1;
	private int longPressCount = 0;//长按触发次数
	private bool isLongPress = false;// 是否处于长按状态
	private float pressStartTime;
	private float longLastInvokeTime;
	[SerializeField]
	private UnityEvent onLongPress = new UnityEvent();

	public UnityEvent onPointerUp = new UnityEvent();


	[SerializeField]
	private bool _isGray;

	/// <summary>
	/// 置灰(包括子节点)
	/// </summary>
	public bool IsGray
	{
		get => _isGray; set
		{
			if (_isGray != value)
				EUIHelper.SetGrayAll(gameObject, value);
			_isGray = value;
		}
	}

	public void SetGrayAll(bool value)
	{
		EUIHelper.SetGrayAll(gameObject, value);
	}

	[Obsolete("use GButton.SetGrayAll")]
	public void SetGray(bool bGray)
	{
		if (bGray == true)
		{
			image.color = colors.disabledColor;
		}
		else
		{
			image.color = colors.normalColor;
		}
	}

	[SerializeField]
	private int soundId;
	public int SoundId { get => soundId; set => soundId = value; }
	private AudioManager _audioManager;

	public void InitSound(AudioManager audioManager)
	{
		_audioManager = audioManager;
		this.onClick.RemoveListener(PlaySound);
		this.onClick.AddListener(PlaySound);
	}

	public void PlaySound()
	{
		if (SoundId != 0 && _audioManager != null)
		{
			//_audioManager.PlayEffect(SoundId);
		}
	}

	/// <summary>
	/// 透传响应类型
	/// </summary>
	[SerializeField]
	private ePassEventType passEventType = ePassEventType.none;

	/// <summary>
	/// 当透传事件成功后终止自身响应事件
	/// </summary>
	[SerializeField]
	private bool bPassEventBreak = false;

	private static List<RaycastResult> results = new List<RaycastResult>();
	//把事件透下去
	public static bool PassEvent<T>(GameObject owner, PointerEventData eventData, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
	{
		//RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
		EventSystem.current.RaycastAll(eventData, results);
		for (int i = 0; i < results.Count; i++)
		{
		    // 新手引导层不作为透传目标
			//GameObject target = results[i].gameObject;
			//if (!target.tag.Equals(BagTag.Guide))
			//{
			//    // 透传目标不能是自己
			//	if (owner != target)
			//	{
			//		return ExecuteEvents.Execute(results[i].gameObject, eventData, function);
			//	}
			//}
		}
		return false;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		// 反序响应透传
		if (passEventType == ePassEventType.reverse)
		{
			bool status = PassEvent(gameObject, eventData, ExecuteEvents.pointerClickHandler);
			if (status & bPassEventBreak)
			{
				return;
			}
		}

		// 自身逻辑执行
		float currentTime = unscaledTime;
		var singleClickInterval = this.singleClickInterval < 0 ? 0.2f : this.singleClickInterval;
		if ((currentTime - singleLastClickTime) > singleClickInterval)
		{
			//超过长按触发时间，并且长按事件不为空，不触发单击事件
			var longPressDuration = LongPressDuration;
			var isTrigger = currentTime - pressStartTime <= longPressDuration;
			isTrigger = isTrigger || GetLongPressEventCount() <= 0;
			if (isTrigger)
			{
				base.OnPointerClick(eventData);
				singleLastClickTime = currentTime;
			}
		}

		// 如果是第一次点击或者两次点击的时间间隔超过阈值，则认为是第一次点击
		var doubleClickThreshold = this.doubleClickThreshold < 0 ? 0.2f : this.doubleClickThreshold;
		if (isFirstClick || currentTime - doubleLastClickTime > doubleClickThreshold)
		{
			isFirstClick = false;
		}
		else
		{
			onClickDouble?.Invoke();
			isFirstClick = true; // 重置第一次点击状态
		}
		doubleLastClickTime = currentTime;

		// 顺向响应透传
		if (passEventType == ePassEventType.order)
		{
			PassEvent(gameObject, eventData, ExecuteEvents.pointerClickHandler);
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		longPressCount = 0;
		isLongPress = true;
		pressStartTime = unscaledTime;
		longLastInvokeTime = unscaledTime;
		base.OnPointerDown(eventData);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		isLongPress = false;
		onPointerUp.Invoke();
	}

	protected virtual void Update()
	{
		var longPressDuration = LongPressDuration;
		if (isLongPress && unscaledTime - pressStartTime > longPressDuration)
		{
			// 判断是否超过重复触发间隔
			if (unscaledTime - longLastInvokeTime > longRepeatInterval)
			{
				longPressCount++;
				onLongPress.Invoke(); // 触发长按事件
				longLastInvokeTime = unscaledTime;
			}
			if (longRepeatInterval < 0)
			{
				isLongPress = false;
			}
		}
	}

	private float unscaledTime { get { return Time.unscaledTime; } }
	private float LongPressDuration { get { return this.longPressDuration < 0 ? 1 : this.longPressDuration; } }


	/// <summary>
	/// 强行终止长按
	/// </summary>
	public void ForceTerminationLongPress()
	{
		if (isLongPress)
		{
			isLongPress = false;
		}
	}

	private int longPressEventCount = 0;
	public void LongPressAddListener(UnityAction action)
	{
		longPressEventCount++;
		onLongPress.AddListener(action);
	}
	public void LongPressRemoveListener(UnityAction action)
	{
		longPressEventCount--;
		onLongPress.RemoveListener(action);
	}
	public void LongPressRemoveAllListeners()
	{
		longPressEventCount = 0;
		onLongPress.RemoveAllListeners();
	}

	/// <summary>
	/// 获取监听器的数量
	/// </summary>
	public int GetLongPressEventCount()
	{
		return longPressEventCount + onLongPress.GetPersistentEventCount();
	}

	/// <summary>
	/// 强行触发点击
	/// </summary>
	public void DoForceClickAction(PointerEventData eventData)
	{
		pressStartTime = unscaledTime;
		OnPointerClick(eventData);
	}
}
