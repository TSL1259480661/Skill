using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//原理就是把Mask的裁切区域传给粒子特效Shader，当超出这个区域那么直接让它完全透明即可
// see: https://www.xuanyusong.com/archives/3518/comment-page-1#comments

[ExecuteAlways]
public class RectMask : MonoBehaviour//Mask
{
	Vector3[] corners = new Vector3[4];
	private Vector4 maskRect;

	private List<RectMaskItem> rectMaskItems = new List<RectMaskItem>(4);
	private RectTransform rectTransform;
	private void Awake()
	{
		rectTransform = transform as RectTransform;
	}

	protected void Start()
	{
		UpdateMask();
	}

	// 处理可能的变换变化，如UI元素的大小或位置变化时需要更新裁剪区域
	protected void OnRectTransformDimensionsChange()
	{
		rectTransform = transform as RectTransform;
		UpdateMask(); // 当RectTransform尺寸变化时立即更新裁剪区域
	}

	private void Update()
	{
		UpdateMask();
	}

	void UpdateMask()
	{
		// 获取Mask的世界空间边界
		rectTransform.GetWorldCorners(corners);
		Vector4 newMaskRect = new Vector4(corners[0].x, corners[0].y, corners[2].x, corners[2].y);
		if (newMaskRect != maskRect)
		{
			maskRect = newMaskRect;
			for (int i = 0; i < rectMaskItems.Count; i++)
			{
				rectMaskItems[i].UpdateMaskRect(MaskRect);
			}
		}

		//// 遍历所有子对象的Renderer
		//foreach (var renderer in GetComponentsInChildren<Renderer>())
		//{
		//	var material = renderer.material;
		//	if (material != null && material.HasProperty("_MaskRect"))
		//	{
		//		material.SetVector("_MaskRect", maskRect);
		//	}
		//}

		//// 遍历所有子对象的UI元素（Image, Text, RawImage等）
		//foreach (var graphic in GetComponentsInChildren<Graphic>())
		//{
		//	var material = graphic.material;
		//	if (material != null && material.HasProperty("_MaskRect"))
		//	{
		//		material.SetVector("_MaskRect", maskRect);
		//	}
		//}
	}

	public void Registration(RectMaskItem item)
	{
		if (!rectMaskItems.Contains(item))
		{
			rectMaskItems.Add(item);
		}
	}

	public void UnRegistration(RectMaskItem item)
	{
		rectMaskItems.Remove(item);
	}
	public Vector4 MaskRect { get => maskRect; }
}
