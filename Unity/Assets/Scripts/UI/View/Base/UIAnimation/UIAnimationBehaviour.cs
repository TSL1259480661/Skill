using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI窗口表现动画
/// </summary>
public class UIAnimationBehaviour : IObjectPoolItem
{
	private static UObjectPool<UIAnimationBehaviour> itemPool = new UObjectPool<UIAnimationBehaviour>();

	/// <summary>
	/// 表现参数
	/// </summary>
	private UIAnimationBaseParmar animationParmar = null;

	/// <summary>
	/// 访问参数
	/// </summary>>
	public T AnimationParmar<T>() where T : UIAnimationBaseParmar
	{
		return animationParmar as T;
	}

	public static UIAnimationBehaviour Get()
	{
		return itemPool.Get();
	}

	public void OnReuse()
	{

	}

	public void OnRecycle()
	{
		animationParmar = null;
	}

	public void Recycle()
	{
		itemPool.Recycle(this);
	}

	/// <summary>
	/// 设置动画参数
	/// </summary>
	public void SetUIAnimationParmar(UIAnimationBaseParmar parmar)
	{
		animationParmar = parmar;
	}

	/// <summary>
	/// 开启动画播放
	/// </summary>
	public void OpenAnimation(Action action)
	{
		animationParmar.OpenAnimation(action);
	}

	/// <summary>
	/// 关闭动画播放
	/// </summary>
	public void CloseAnimation(Action action)
	{
		animationParmar.CloseAnimation(action);
	}
}
