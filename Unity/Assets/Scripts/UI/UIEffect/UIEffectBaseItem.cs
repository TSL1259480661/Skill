using System;
using UIEngine;
using UnityEngine;

/// <summary>
/// UI特效控件基类
/// </summary>
public abstract class UIEffectBaseItem : IObjectPoolItem
{
	/// <summary>
	/// 加载委托定义
	/// </summary>
	public delegate IUILoadAssetItem LoadAssetAction(string assetPath, int assetType, Action<IUILoadAssetItem> onLoadDone);

	/// <summary>
	/// 资源加载器
	/// </summary>
	private LoadAssetAction loadAssetAction;

	/// <summary>
	/// 目标结点
	/// </summary>
	protected Transform transform;

	/// <summary>
	/// UI特效实体
	/// </summary>
	private GameObject effect;

	/// <summary>
	/// 模板相对路径
	/// </summary>
	protected virtual string templetePath
	{
		get
		{
			return string.Empty;
		}
	}

	public void OnReuse()
	{

	}

	public void OnRecycle()
	{
		transform = null;
		loadAssetAction = null;
		DestoryUIEffect();
	}

	/// <summary>
	/// 加载UI特效
	/// </summary>
	protected void LoadAsset(Action action)
	{
	    if (effect == null)
		{
			string path = AssetPathHelper.ParseUIEffectPath(templetePath);
			loadAssetAction?.Invoke(path, UIAssetType.Instance, (item) =>
			{
				effect = item.content as GameObject;
				effect.transform.SetParent(transform);
				effect.transform.localScale = Vector3.one;
				effect.transform.localPosition = Vector3.zero;
				action?.Invoke();
			});
		}
		else
		{
			effect.transform.SetParent(transform);
			effect.transform.localScale = Vector3.one;
			effect.transform.localPosition = Vector3.zero;
			action?.Invoke();
		}
	}

	/// <summary>
	/// UI特效绑定
	/// </summary>
	public void Bind(Transform transform, LoadAssetAction action)
	{
		this.transform = transform;
		this.loadAssetAction = action;
	}

	/// <summary>
	/// 释放
	/// </summary>
	public void Recycle()
	{
		OnRecycle();
	}

	/// <summary>
	/// 激活显示
	/// </summary>
	public void SetActive(bool active)
	{
		if (active)
		{
			CreateUIEffect(active);
		}
		else if (effect != null)
		{
			effect.SetActive(false);
		}
	}

	/// <summary>
	/// 销毁UI特效
	/// </summary>
	protected void DestoryUIEffect()
	{
	    if (effect != null)
		{
			GameObject.Destroy(effect);
		}
		effect = null;
	}

	/// <summary>
	/// 创建UI特效
	/// </summary>
	protected void CreateUIEffect(bool active)
	{
		LoadAsset(()=>
		{
			effect.gameObject.SetActive(active);
		});
	}
}
