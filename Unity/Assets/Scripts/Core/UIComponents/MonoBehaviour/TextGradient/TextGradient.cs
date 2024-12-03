using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 渐变模式
/// </summary>
public enum eGradientType
{
	/// <summary>
	/// 横向渐变
	/// </summary>
	horzontal,

	/// <summary>
	/// 垂直渐变
	/// </summary>
	vertical,
}

/// <summary>
/// 扩展渐变字体绘制
/// </summary>
[RequireComponent(typeof(GText))]
public class TextGradient : BaseMeshEffect
{
	/// <summary>
	/// 渐变模式
	/// </summary>
	public eGradientType type = eGradientType.horzontal;

	/// <summary>
	/// 初始颜色
	/// </summary>
	public Color sourceColor = Color.white;

	/// <summary>
	/// 结束颜色
	/// </summary>
	public Color targetColor = Color.black;

	/// <summary>
	/// 缓存顶点信息
	/// </summary>
	private List<UIVertex> vertexs = new List<UIVertex>();

	/// <summary>
	/// 重载实现ModifyMesh
	/// </summary>
	public override void ModifyMesh(VertexHelper vh)
	{
	    // 无效
		if (gameObject.activeSelf == false || enabled == false)
		{
			return;
		}

		//  数据无效
		int count = vh.currentVertCount;
		if (count == 0)
		{
			return;
		}

		// 根据类型绘制
		switch (type)
		{
			case eGradientType.horzontal:
				HorzontalModifyMesh(count, vh);
				break;
			case eGradientType.vertical:
				VerticalModifyMesh(count, vh);
				break;
		}
	}

	/// <summary>
	/// 水平渐变
	/// </summary>
	private void HorzontalModifyMesh(int count, VertexHelper vh)
	{
		vertexs.Clear();
		for (int index = 0; index < count; ++index)
		{
			UIVertex vertex = new UIVertex();
			vh.PopulateUIVertex(ref vertex, index);
			vertexs.Add(vertex);
		}
		float left = vertexs[0].position.x;
		float right = vertexs[0].position.x;
		for (int index = 1; index < count; ++index)
		{
			float x = vertexs[index].position.x;
			if (x > right)
			{
				right = x;
			}
			else if (x < left)
			{
				left = x;
			}
		}
		float width = right - left;
		for (int index = 0; index < count; ++index)
		{
			UIVertex vertex = vertexs[index];
			vertex.color = Color.Lerp(sourceColor, targetColor, (vertex.position.x - left) / width);
			vh.SetUIVertex(vertex, index);
		}
		vertexs.Clear();
	}

	/// <summary>
	/// 垂直渐变
	/// </summary>
	private void VerticalModifyMesh(int count, VertexHelper vh)
	{
		vertexs.Clear();
		for (int index = 0; index < count; ++index)
		{
			UIVertex vertex = new UIVertex();
			vh.PopulateUIVertex(ref vertex, index);
			vertexs.Add(vertex);
		}
		float top = vertexs[0].position.y;
		float bottom = vertexs[0].position.y;
		for (int index = 1; index < count; ++index)
		{
			float y = vertexs[index].position.y;
			if (y > top)
			{
				top = y;
			}
			else if (y < bottom)
			{
				bottom = y;
			}
		}
		float height = top - bottom;
		for (int index = 0; index < count; ++index)
		{
			UIVertex vertex = vertexs[index];
			vertex.color = Color.Lerp(targetColor, sourceColor, (vertex.position.y - bottom) / height);
			vh.SetUIVertex(vertex, index);
		}
		vertexs.Clear();
	}
}
