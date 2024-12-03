using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class RectMaskItem : MonoBehaviour
{
	private RectMask mask;       // 当前 Mask 组件
	private Renderer renderer;   // 当前 Renderer 组件
	private Graphic graphic;     // 当前 UI Graphic 组件


	void Awake()
	{
		// 缓存 Renderer 和 Graphic 组件，减少 GetComponent 调用
		renderer = GetComponent<Renderer>();
		graphic = GetComponent<Graphic>();
	}

	void Start()
	{
		RegisterMask();
	}

	void OnEnable()
	{
		// 确保在对象启用时注册遮罩
		RegisterMask();
	}

	void OnDisable()
	{
		// 在对象禁用时注销遮罩
		UnregisterMask();
	}

	void OnTransformParentChanged()
	{
		RegisterMask();
	}

	/// <summary>
	/// 注册当前组件到父级的 RectMask 中，并设置遮罩区域
	/// </summary>
	private void RegisterMask()
	{
		// 防止重复注册
		UnregisterMask();

		// 获取最近的 RectMask 并进行注册
		mask = GetComponentInParent<RectMask>();
		if (mask != null)
		{
			mask.Registration(this);
			UpdateMaskRect(mask.MaskRect);
		}
		else
		{
			// 没有遮罩时，重置遮罩区域
			UpdateMaskRect(new Vector4(-1, -1, -1, -1));
		}
	}

	/// <summary>
	/// 注销当前组件
	/// </summary>
	private void UnregisterMask()
	{
		if (mask != null)
		{
			mask.UnRegistration(this);
			mask = null; // 清除引用
		}
	}


	/// <summary>
	/// 更新材质的遮罩区域
	/// </summary>
	/// <param name="maskRect">新的遮罩区域</param>
	public void UpdateMaskRect(Vector4 maskRect)
	{
		if (Application.IsPlaying(this))
		{
			if (renderer != null && renderer.material.HasProperty("_MaskRect"))
			{
				SetMaterial(renderer.material, maskRect);
			}
			if (graphic != null && graphic.material.HasProperty("_MaskRect"))
			{
				SetMaterial(graphic.material, maskRect);
			}
		}
		else
		{
			if (renderer != null)
			{
				SetMaterial(renderer.sharedMaterial, maskRect);
			}
			if (graphic != null)
			{
				SetMaterial(graphic.material, maskRect);
			}

		}

	}

	void SetMaterial(Material material, Vector4 maskRect)
	{
		if (material == null || !material.HasProperty("_MaskRect")) return;
		material.SetVector("_MaskRect", maskRect);
	}


}
