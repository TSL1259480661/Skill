using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 扩展按钮组
/// </summary>
public class ExtendButtonGroup : MarkBehaviour
{
	/// <summary>
	/// 按钮列表
	/// </summary>
	protected List<ExtendButton> buttonArray = new List<ExtendButton>();

	/// <summary>
	/// 回调函数
	/// </summary>
	protected Action<int, int> clickCallBack;

	/// <summary>
	/// 当前激活按钮
	/// </summary>
	protected ExtendButton currentActiveButton;

	/// <summary>
	/// 文本激活颜色
	/// </summary>
	[SerializeField]
	protected Color active_color;

	/// <summary>
	/// 反激活颜色
	/// </summary>
	[SerializeField]
	protected Color deactive_color;

	/// <summary>
	/// 当前标识Key
	/// </summary>
	public int currentKey
	{
		get
		{
			if (currentActiveButton != null)
			{
				return currentActiveButton.key;
			}
			return 0;
		}
	}

	/// <summary>
	/// 访问按钮列表
	/// </summary>
	public List<ExtendButton> ButtonArray
	{
		get
		{
			return buttonArray;
		}
	}

	/// <summary>
	/// 设置回调
	/// </summary>
	public void SetClickCallBack(Action<int, int> action, int active_key = 0)
	{
		clickCallBack = action;
		InitActiveKey(active_key);
	}

	/// <summary>
	/// 设置点击音效信息
	/// </summary>
	public void SetClickAudio(AudioManager audioManager, int audio_click_path)
	{
		foreach (ExtendButton button in buttonArray)
		{
			button.SoundId = audio_click_path;
			button.InitSound(audioManager);
		}
	}

	public void SetClickAudio(int audio_click_path)
	{
		foreach (ExtendButton button in buttonArray)
		{
			button.SoundId = audio_click_path;
		}
	}
	/// <summary>
	/// 初始化
	/// </summary>
	protected virtual void Awake()
	{
		buttonArray.Clear();
		ExtendButton[] childArray = GetComponentsInChildren<ExtendButton>();
		for (int index = 0; index < childArray.Length; ++index)
		{
			childArray[index].Init();
			childArray[index].SetClickCallBack(OnClickButtonAction, index + 1);
			childArray[index].SetContextColor(active_color, deactive_color);
			childArray[index].SetStatus(false);
			buttonArray.Add(childArray[index]);
		}
	}

	/// <summary>
	/// 首按钮Key
	/// </summary>
	/// <returns></returns>
	public int FirstButtonKey()
	{
	    if (buttonArray.Count > 0)
		{
			return buttonArray[0].key;
		}
		return 0;
	}

	/// <summary>
	/// 初始激活Key
	/// </summary>
	public void InitActiveKey(int key)
	{
		if (key > 0)
		{
			foreach (ExtendButton button in buttonArray)
			{
				if (key == button.key)
				{
					OnClickButtonAction(button);
					return;
				}
			}
		}
	}

	/// <summary>
	/// 点击回调
	/// </summary>
	protected void OnClickButtonAction(ExtendButton button)
	{
		// 旧标记
		int old_key = 0;
		if (currentActiveButton != null)
		{
			old_key = currentActiveButton.key;
			currentActiveButton.SetStatus(false);
		}

		// 新标记
		int new_key = button.key;
		button.SetStatus(true);
		currentActiveButton = button;

		// 触发回调
		clickCallBack?.Invoke(old_key, new_key);
	}
}
