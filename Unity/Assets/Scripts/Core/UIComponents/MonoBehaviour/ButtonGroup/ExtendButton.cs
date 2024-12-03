using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 扩展按钮
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ExtendButton : GButton
{
    /// <summary>
    /// 激活显示组件
    /// </summary>
    protected GameObject activeGroup;

    /// <summary>
    /// 反激活显示组件
    /// </summary>
    protected GameObject deactiveGroup;

    /// <summary>
    /// 文本组件
    /// </summary>
    protected Text context;

	/// <summary>
	/// 红点
	/// </summary>
	protected GameObject redPoint;

    /// <summary>
    /// 文本激活颜色
    /// </summary>
    protected Color active_color;

    /// <summary>
    /// 反激活颜色
    /// </summary>
    protected Color deactive_color;

    /// <summary>
    /// 点击回调
    /// </summary>
    protected Action<ExtendButton> clickCallBack;

    /// <summary>
    /// 标记
    /// </summary>
    public int key
    {
        get;
        protected set;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        // 激活显示组件
        key = 0;
        Transform active_transform = transform.Find("#Active");
        if (active_transform != null)
        {
            activeGroup = active_transform.gameObject;
        }

        // 反激活显示组件
        Transform deactive_transform = transform.Find("#Deactive");
        if (deactive_transform != null)
        {
            deactiveGroup = deactive_transform.gameObject;
        }

		// 红点
		Transform red_transform = transform.Find("#RedPoint");
		if (red_transform != null)
		{
			redPoint = red_transform.gameObject;
			redPoint.SetActive(false);
		}

        // 文本组件
        Transform context_transform = transform.Find("#Context");
        if (context_transform != null)
        {
            context = context_transform.GetComponent<Text>();
        }
		active_color = CommonConfig.HexToColor("#FDFF07FF");
		deactive_color = CommonConfig.HexToColor("#FFFFFFFF");
        onClick.AddListener(OnButtonClickAction);
    }

    /// <summary>
    /// 设置点击事件回调
    /// </summary>
    public void SetClickCallBack(Action<ExtendButton> action, int key)
    {
        this.key = key;
        clickCallBack = action;
    }

    /// <summary>
    /// 设置文本内容
    /// </summary>
    public void SetContext(string value)
    {
        if (context != null)
        {
            context.text = value;
        }
    }

    /// <summary>
    /// 设置文本状态颜色
    /// </summary>
    public void SetContextColor(Color active_color, Color deactive_color)
    {
        this.active_color = active_color;
        this.deactive_color = deactive_color;
    }

    /// <summary>
    /// 设置状态
    /// </summary>
    public void SetStatus(bool status)
    {
        if (status)
        {
            interactable = false;
            SetContextStatus(true);
            SetActiveGroupStatus(true);
            SetDeactiveGroupStatus(false);
        }
        else
        {
            interactable = true;
            SetContextStatus(false);
            SetActiveGroupStatus(false);
            SetDeactiveGroupStatus(true);
        }
    }

    /// <summary>
    /// 设置激活显示组件状态
    /// </summary>
    protected void SetActiveGroupStatus(bool status)
    {
        if (activeGroup != null)
        {
            activeGroup.SetActive(status);
        }
    }

    /// <summary>
    /// 设置反激活显示组件状态
    /// </summary>
    protected void SetDeactiveGroupStatus(bool status)
    {
        if (deactiveGroup != null)
        {
            deactiveGroup.SetActive(status);
        }
    }

    /// <summary>
    /// 设置文本状态
    /// </summary>
    protected void SetContextStatus(bool status)
    {
        if (context != null)
        {
            context.color = (status ? active_color : deactive_color);
        }
    }

	/// <summary>
	/// 设置红点状态
	/// </summary>
	public void SetRedPoint(bool active)
	{
		if (redPoint != null)
		{
			redPoint.SetActive(active);
		}
	}

    /// <summary>
    /// 按钮点击
    /// </summary>
    protected void OnButtonClickAction()
    {
        clickCallBack?.Invoke(this);
    }

	/// <summary>
	/// 显示或隐藏
	/// </summary>
	public void SetActive(bool active)
	{
		gameObject.SetActive(active);
	}
}
