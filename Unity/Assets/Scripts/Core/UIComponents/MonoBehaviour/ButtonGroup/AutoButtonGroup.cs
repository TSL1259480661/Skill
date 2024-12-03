using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按钮配置
/// </summary>
public class ButtonConfig
{
	/// <summary>
	/// 配置ID
	/// </summary>
	public int id;

	/// <summary>
	/// 显示名称
	/// </summary>
	public string name;
}

/// <summary>
/// 扩展按钮组[动态]
/// </summary>
public class AutoButtonGroup : ExtendButtonGroup
{
	/// <summary>
	/// 按钮模板
	/// </summary>
	[SerializeField]
	protected GameObject button_templete;

	/// <summary>
	/// 初始化
	/// </summary>
	protected override void Awake()
	{
		if (button_templete != null)
		{
			button_templete.SetActive(false);
		}
	}

	/// <summary>
	/// 设置按钮信息
	/// </summary>
	public void SetButtonArray(List<ButtonConfig> array, AudioManager manager, int soundName)
	{
		buttonArray.Clear();
		if (button_templete != null)
		{
			foreach (ButtonConfig config in array)
			{
				GameObject go = GameObject.Instantiate(button_templete);
				go.transform.SetParent(button_templete.transform.parent);
				go.transform.localScale = button_templete.transform.localScale;
				go.transform.localPosition = Vector3.zero;
				go.SetActive(true);

				// 按钮控件
				ExtendButton button = go.GetComponent<ExtendButton>();
				if (button != null)
				{
					button.Init();
					button.InitSound(manager);
					button.SoundId = soundName;
					button.SetContext(config.name);
					button.SetClickCallBack(OnClickButtonAction, config.id);
					button.SetContextColor(active_color, deactive_color);
					button.SetStatus(false);
					buttonArray.Add(button);
				}
			}
		}
	}

	/// <summary>
	/// 清理按钮信息
	/// </summary>
	public void ClearButtonArray()
	{
		foreach (ExtendButton element in buttonArray)
		{
			GameObject.Destroy(element.gameObject);
		}
		buttonArray.Clear();
		currentActiveButton = null;
	}

	/// <summary>
	/// 查找指定按钮
	/// </summary>
	public ExtendButton FindExtendButton(int key)
	{
		return buttonArray.Find(node => node.key == key);
	}
}
