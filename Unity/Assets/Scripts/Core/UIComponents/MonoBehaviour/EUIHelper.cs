using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public static class EUIHelper
{

	#region UI辅助方法

	public static void SetText(this Text Label, string content)
	{
		if (null == Label)
		{
			Debug.LogError("label is null");
			return;
		}
		Label.text = content;
	}

	public static void SetVisibleWithScale(this UIBehaviour uiBehaviour, bool isVisible)
	{
		if (null == uiBehaviour)
		{
			Debug.LogError("uibehaviour is null!");
			return;
		}

		if (null == uiBehaviour.gameObject)
		{
			Debug.LogError("uiBehaviour gameObject is null!");
			return;
		}

		if (uiBehaviour.gameObject.activeSelf == isVisible)
		{
			return;
		}
		uiBehaviour.transform.localScale = isVisible ? Vector3.one : Vector3.zero;
	}

	public static void SetVisible(this UIBehaviour uiBehaviour, bool isVisible)
	{
		if (null == uiBehaviour)
		{
			Debug.LogError("uibehaviour is null!");
			return;
		}

		if (null == uiBehaviour.gameObject)
		{
			Debug.LogError("uiBehaviour gameObject is null!");
			return;
		}

		if (isVisible)
		{
			uiBehaviour.transform.localScale = Vector3.one;
		}
		else
		{
			uiBehaviour.transform.localScale = Vector3.zero;
		}
	}

	public static void SetVisibleWithScale(this Transform transform, bool isVisible)
	{
		if (null == transform)
		{
			Debug.LogError("uibehaviour is null!");
			return;
		}

		if (null == transform.gameObject)
		{
			Debug.LogError("uiBehaviour gameObject is null!");
			return;
		}

		transform.localScale = isVisible ? Vector3.one : Vector3.zero;
	}

	public static void SetVisible(this Transform transform, bool isVisible)
	{
		if (null == transform)
		{
			Debug.LogError("uibehaviour is null!");
			return;
		}

		if (null == transform.gameObject)
		{
			Debug.LogError("uiBehaviour gameObject is null!");
			return;
		}

		if (transform.gameObject.activeSelf == isVisible)
		{
			return;
		}
		transform.gameObject.SetActive(isVisible);
	}

	/// <summary>
	/// 设置父节点,坐标缩放重置为0
	/// </summary>
	/// <param name="transform"></param>
	/// <param name="parent"></param>
	public static void SetParentResetPosScaleToZero(this Transform transform,Transform parent) 
	{
		if (null == transform)
		{
			Debug.LogError("uibehaviour is null!");
			return;
		}
		transform.SetParent(parent);
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
	}
	public static void SetVisible(this CanvasGroup canvasGroup, bool isVisible)
	{
		canvasGroup.alpha = isVisible ? 1 : 0;
		canvasGroup.interactable = isVisible;
		canvasGroup.blocksRaycasts = isVisible;
	}

	public static void SetTogglesInteractable(this ToggleGroup toggleGroup, bool isEnable)
	{
		var toggles = toggleGroup.transform.GetComponentsInChildren<Toggle>();
		for (int i = 0; i < toggles.Length; i++)
		{
			toggles[i].interactable = isEnable;
		}
	}


	public static (int, Toggle) GetSelectedToggle(this ToggleGroup toggleGroup)
	{
		var togglesList = toggleGroup.GetComponentsInChildren<Toggle>();
		for (int i = 0; i < togglesList.Length; i++)
		{
			if (togglesList[i].isOn)
			{
				return (i, togglesList[i]);
			}
		}
		Debug.LogError("none Toggle is Selected");
		return (-1, null);
	}


	public static void SetToggleSelected(this ToggleGroup toggleGroup, int index)
	{
		var togglesList = toggleGroup.GetComponentsInChildren<Toggle>();
		for (int i = 0; i < togglesList.Length; i++)
		{
			if (i != index)
			{
				continue;
			}
			togglesList[i].IsSelected(true);
		}
	}


	public static void IsSelected(this Toggle toggle, bool isSelected)
	{
		toggle.isOn = isSelected;
		toggle.onValueChanged?.Invoke(isSelected);
	}




	public static void GetUIComponent<T>(this ReferenceCollector rf, string key, ref T t) where T : class
	{
		GameObject obj = rf.Get<GameObject>(key);

		if (obj == null)
		{
			t = null;
			return;
		}

		t = obj.GetComponent<T>();
	}

	#endregion

	#region UI按钮事件


	public static void AddListener(this Toggle toggle, UnityAction<bool> selectEventHandler)
	{
		toggle.onValueChanged.AddListener(selectEventHandler);
	}

	public static void AddListener(this Button button, UnityAction clickEventHandler)
	{
		button.onClick.AddListener(clickEventHandler);
	}

	public static void AddListener(this GButton button, UnityAction clickEventHandler)
	{
		button.onClick.RemoveListener(button.PlaySound);
		button.onClick.AddListener(button.PlaySound);
		button.onClick.AddListener(clickEventHandler);
	}

	public static void RemoveAllListeners(this Button button)
	{
		button.onClick.RemoveAllListeners();
	}

	public static void RegisterEvent(this EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> callback)
	{
		EventTrigger.Entry entry = null;

		// 查找是否已经存在要注册的事件
		foreach (EventTrigger.Entry existingEntry in trigger.triggers)
		{
			if (existingEntry.eventID == eventType)
			{
				entry = existingEntry;
				break;
			}
		}

		// 如果这个事件不存在，就创建新的实例
		if (entry == null)
		{
			entry = new EventTrigger.Entry();
			entry.eventID = eventType;
		}
		// 添加触发回调并注册事件
		entry.callback.AddListener(callback);
		trigger.triggers.Add(entry);
	}

	static FieldInfo eventBaseCalls;
	static PropertyInfo eventBaseMethodCount;
	/// <summary>
	/// 使用反射访问私有字段 m_PersistentCalls.m_Calls
	/// </summary>
	public static int GetListenerCount(UnityEvent unityEvent)
	{
		if (eventBaseCalls == null || eventBaseMethodCount == null)
		{
			System.Type typeEventBase = typeof(UnityEventBase);
			eventBaseCalls = typeEventBase.GetField("m_Calls", BindingFlags.NonPublic | BindingFlags.Instance);
			System.Type typeInvokableCallList = typeEventBase.Assembly.GetType("UnityEngine.Events.InvokableCallList");
			eventBaseMethodCount = typeInvokableCallList.GetProperty("Count");
		}
		var invokableCallList = eventBaseCalls.GetValue(unityEvent);
		return (int)eventBaseMethodCount.GetValue(invokableCallList);
	}

	#endregion

	/// <summary>
	/// 遍历设置层级
	/// </summary>
	/// <param name="go"></param>
	/// <param name="layer"></param>
	public static void SetLayer(this GameObject go, int layer)
	{
		go.layer = layer;
		for (int index = 0; index < go.transform.childCount; ++index)
		{
			Transform child = go.transform.GetChild(index);
			if (child != null)
			{
				child.gameObject.SetLayer(layer);
			}
		}
	}

	/// <summary>
	/// 向父结点检索控件
	/// 如果找到则返回改控件，不是则返回自己
	/// </summary>
	public static GameObject CheckParent<T>(this GameObject go) where T : Component
	{
		T component = go.GetComponentInParent<T>();
		if (component == null)
		{
			return go;
		}
		else
		{
			return component.gameObject;
		}
	}


	public static void SetGrayAll(GameObject gameObject, bool value)
	{
		var children = gameObject.GetComponentsInChildren<IGray>();
		for (int i = 0; i < children.Length; i++)
		{
			children[i].IsGray = value;
		}
	}


	public static void HideChildren(this GameObject go)
	{
		go.transform.HideChildren();
	}

	public static void HideChildren(this Transform parent)
	{
		for (int i = 0;i < parent.transform.childCount; i++)
		{
			parent.GetChild(i).gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// 创建子物体
	/// </summary>
	/// <param name="parent">父节点</param>
	/// <param name="child">子物体</param>
	/// <param name="count">子物体总数</param>
	public static void CreateChild(this Transform parent,GameObject child,int count) 
	{
		GameObject obj;
		parent.HideChildren();
		int childCount = parent.childCount;
		for (int i = 0; i < count; i++)
		{
			if (i+1>childCount)
			{
				obj = GameObject.Instantiate(child, parent);
			}
			else
			{
				obj = parent.GetChild(i).gameObject;
			}
			obj.SetActive(true);
		}
	}
}

