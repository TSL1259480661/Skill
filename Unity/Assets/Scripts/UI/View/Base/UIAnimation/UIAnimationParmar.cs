using App;
using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI表现动画参数基类
/// </summary>
public abstract class UIAnimationBaseParmar
{
	/// <summary>
	/// 表现动画时间
	/// </summary>
	protected float time = 0.2F;

	/// <summary>
	/// 待缩放的窗口
	/// </summary>
	protected RectTransform window;

	/// <summary>
	/// 动画期间禁用的按钮
	/// </summary>
	protected List<GButton> buttonArray = new List<GButton>();

	/// <summary>
	/// 设置按钮状态
	/// </summary>
	protected void SetButtonStatus(bool status)
	{
		foreach (GButton button in buttonArray)
		{
			button.interactable = status;
		}
	}

	/// <summary>
	/// 开启动画播放
	/// </summary>
	public virtual void OpenAnimation(Action action)
	{

	}

	/// <summary>
	/// 关闭动画播放
	/// </summary>
	public virtual void CloseAnimation(Action action)
	{

	}
}

/// <summary>
/// 居中Y缩放动画
/// </summary>
public class UICenterScaleParmar : UIAnimationBaseParmar
{
	/// <summary>
	/// 初始缩放
	/// </summary>
	protected Vector3 scale = new Vector3(1F, 0F, 1F);

	/// <summary>
	/// 过程值
	/// </summary>
	protected Vector3 cache = new Vector3(1F, 0F, 1F);

	/// <summary>
	/// 开启动画播放
	/// </summary>
	public override void OpenAnimation(Action action)
	{
		SetButtonStatus(false);
		window.localScale = scale;
		DoTween.Instance.Add(0F, 1F, time, (value, obj) =>
		{
			cache.Set(1F, value, 1F);
			window.localScale = cache;
		}, (id, obj) =>
		{
			SetButtonStatus(true);
			DoTween.Instance.Remove(id);
			window.localScale = Vector3.one;
			action?.Invoke();
		});
	}

	/// <summary>
	/// 关闭动画播放
	/// </summary>
	public override void CloseAnimation(Action action)
	{
		SetButtonStatus(false);
		window.localScale = Vector3.one;
		DoTween.Instance.Add(1F, 0F, time, (value, obj) =>
		{
			cache.Set(1F, value, 1F);
			window.localScale = cache;
		}, (id, obj) =>
		{
			SetButtonStatus(true);
			DoTween.Instance.Remove(id);
			window.localScale = scale;
			action?.Invoke();
		});
	}

	/// <summary>
	/// 创建动画参数
	/// </summary>
	public static UICenterScaleParmar Create(RectTransform window, float time, List<GButton> array)
	{
		return new UICenterScaleParmar()
		{
			time = time,
			window = window,
			buttonArray = array
		};
	}
}

/// <summary>
/// UI坐标变化动画
/// </summary>
public class UIAnchoredPositionParmar : UIAnimationBaseParmar
{
	/// <summary>
	/// 起点坐标
	/// </summary>
	protected Vector2 start = new Vector2(0F, 0F);

	/// <summary>
	/// 结束坐标
	/// </summary>
	protected Vector2 end = new Vector2(1F, 1F);

	/// <summary>
	/// 过程坐标
	/// </summary>
	protected Vector2 cache = new Vector2(1F, 1F);

	/// <summary>
	/// 开启动画播放
	/// </summary>
	public override void OpenAnimation(Action action)
	{
		SetButtonStatus(false);
		window.anchoredPosition = start;
		DoTween.Instance.Add(start.x, start.y, end.x, end.y, time, (x, y, obj) =>
		{
			cache.Set(x, y);
			window.anchoredPosition = cache;
		}, (id, obj) =>
		{
			SetButtonStatus(true);
			DoTween.Instance.Remove(id);
			window.anchoredPosition = end;
			action?.Invoke();
		});
	}

	/// <summary>
	/// 关闭动画播放
	/// </summary>
	public override void CloseAnimation(Action action)
	{
		SetButtonStatus(false);
		window.anchoredPosition = end;
		DoTween.Instance.Add(end.x, end.y, start.x, start.y, time, (x, y, obj) =>
		{
			cache.Set(x, y);
			window.anchoredPosition = cache;
		}, (id, obj) =>
		{
			SetButtonStatus(true);
			DoTween.Instance.Remove(id);
			window.anchoredPosition = start;
			action?.Invoke();
		});
	}

	/// <summary>
	/// 变更坐标
	/// </summary>
	public void ChangeStartAndEndPosition(Vector2 start, Vector2 end)
	{
		this.end = end;
		this.start = start;
	}

	/// <summary>
	/// 创建动画参数
	/// </summary>
	public static UIAnchoredPositionParmar Create(RectTransform window, float time, List<GButton> array, Vector2 start, Vector2 end)
	{
		return new UIAnchoredPositionParmar()
		{
			end = end,
			time = time,
			start = start,
			window = window,
			buttonArray = array
		};
	}
}

/// <summary>
/// UI透明度变化动画
/// </summary>
public class UICanvasAlphaParmar : UIAnimationBaseParmar
{
	/// <summary>
	/// 透明度对象
	/// </summary>
	protected CanvasGroup canvas;

	/// <summary>
	/// 开启动画播放
	/// </summary>
	public override void OpenAnimation(Action action)
	{
		canvas.alpha = 0F;
		SetButtonStatus(false);
		DoTween.Instance.Add(0F, 1F, time, (value, obj) =>
		{
			canvas.alpha = value;
		}, (id, obj) =>
		{
			SetButtonStatus(true);
			DoTween.Instance.Remove(id);
			canvas.alpha = 1F;
			action?.Invoke();
		});
	}

	/// <summary>
	/// 关闭动画播放
	/// </summary>
	public override void CloseAnimation(Action action)
	{
		canvas.alpha = 1F;
		SetButtonStatus(false);
		DoTween.Instance.Add(1F, 0F, time, (value, obj) =>
		{
			canvas.alpha = value;
		}, (id, obj) =>
		{
			SetButtonStatus(true);
			DoTween.Instance.Remove(id);
			canvas.alpha = 0F;
			action?.Invoke();
		});
	}

	/// <summary>
	/// 创建动画参数
	/// </summary>
	public static UICanvasAlphaParmar Create(RectTransform window, float time, List<GButton> array)
	{
		return new UICanvasAlphaParmar()
		{
			time = time,
			window = window,
			buttonArray = array,
			canvas = window.GetComponent<CanvasGroup>()
		};
	}
}
