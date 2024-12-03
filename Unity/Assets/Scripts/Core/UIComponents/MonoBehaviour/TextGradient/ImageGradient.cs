using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 精灵颜色渐变
/// </summary>
[RequireComponent(typeof(GImage))]
public class ImageGradient : BaseMeshEffect
{
	/// <summary>
	/// 渐变模式
	/// </summary>
	public eGradientType type = eGradientType.horzontal;

	/// <summary>
	/// 渐变处理
	/// </summary>
	public Gradient colorGradient = new Gradient();
	
	/// <summary>
	/// 计算过度
	/// </summary>
	private UIVertex vertex = UIVertex.simpleVert;

	/// <summary>
	/// 绘制对象
	/// </summary>
	private GImage image = null;

	/// <summary>
	/// 根目录访问
	/// </summary>
	protected RectTransform rectTransform
	{
		get
		{
			return transform as RectTransform;
		}
	}

	/// <summary>
	/// 绘制对象访问
	/// </summary>
	protected GImage Image
	{
		get
		{
			if (image == null)
			{
				image = rectTransform.GetComponent<GImage>();
			}
			return image;
		}
	}

	/// <summary>
	/// 重载实现ModifyMesh
	/// </summary>
	public override void ModifyMesh(VertexHelper vh)
	{
	    // 未激活
	    if (gameObject.activeSelf == false || enabled == false)
		{
			return;
		}

		// 渐变数据有效
		int count = colorGradient.colorKeys.Length * 2;
		if (count < 4)
		{
			return;
		}

		// 处理渐变
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
	/// 解读裁剪
	/// </summary>
	private void ParseWidthAndLeft(out float right, out float left)
	{
	    if (Image != null)
		{
		   switch (Image.type)
		   {
				case UnityEngine.UI.Image.Type.Filled:
					{
					    if (Image.fillMethod == UnityEngine.UI.Image.FillMethod.Horizontal)
						{
							if (Image.fillOrigin == (int)UnityEngine.UI.Image.OriginHorizontal.Left)
							{
								left = -rectTransform.pivot.x * rectTransform.rect.width;
								right = left + (rectTransform.rect.width * Image.fillAmount);
							}
							else
							{
								right = (1F -rectTransform.pivot.x) * rectTransform.rect.width;
								left = right - (Image.fillAmount * rectTransform.rect.width);
							}
							break;
						}
						right = rectTransform.rect.width;
						left = -rectTransform.pivot.x * right;
					}  
					break;
				default:
					{
						right = rectTransform.rect.width;
						left = -rectTransform.pivot.x * right;
					}
					break;
			}
		}
		else
		{
			right = rectTransform.rect.width;
			left = -rectTransform.pivot.x * right;
		}
	}

	/// <summary>
	/// 水平渐变
	/// </summary>
	private void HorzontalModifyMesh(int count, VertexHelper vh)
	{
		// 构建顶点信息
		vh.Clear();
		ParseWidthAndLeft(out float width, out float left);
		float bottom = -rectTransform.pivot.y * rectTransform.rect.height;
		float top = rectTransform.rect.height + bottom;
		for (int index = 0; index < count; ++index)
		{
			vertex = UIVertex.simpleVert;
			int gradientIndex = index / 2;
			bool bBottomVertex = (index % 2 == 0);
			float pos_x = colorGradient.colorKeys[gradientIndex].time * width + left;
			float pos_y = (bBottomVertex ? bottom : top);
			float uv_x = colorGradient.colorKeys[gradientIndex].time;
			float uv_y = (bBottomVertex ? 0 : 1);

			// 颜色及透明度
			Color color = colorGradient.colorKeys[gradientIndex].color;
			color.a = graphic.color.a;
			vertex.color = color;
			vertex.position.Set(pos_x, pos_y, 0F);
			vertex.uv0.Set(uv_x, uv_y, 0F, 0F);
			vh.AddVert(vertex);
		}

		// 添加三角绘制
		for (int index = 2; index < count; index += 2)
		{
			vh.AddTriangle(index - 2, index + 1, index - 1);
			vh.AddTriangle(index - 2, index, index + 1);
		}
	}

	/// <summary>
	/// 垂直渐变
	/// </summary>
	private void VerticalModifyMesh(int count, VertexHelper vh)
	{
		// 构建顶点信息
		vh.Clear();
		float height = rectTransform.rect.height;
		float bottom = rectTransform.pivot.y * height;
		ParseWidthAndLeft(out float right, out float left);
		for (int index = 0; index < count; ++index)
		{
			vertex = UIVertex.simpleVert;
			int gradientIndex = index / 2;
			bool bLeftVertex = (index % 2 == 0);
			float pos_y = (1 - colorGradient.colorKeys[gradientIndex].time) * height - bottom;
			float pos_x = (bLeftVertex ? left : right);
			float uv_y = (1 - colorGradient.colorKeys[gradientIndex].time);
			float uv_x = (bLeftVertex ? 0 : 1);

			// 颜色及透明度
			Color color = colorGradient.colorKeys[gradientIndex].color;
			color.a = graphic.color.a;
			vertex.color = color;
			vertex.position.Set(pos_x, pos_y, 0F);
			vertex.uv0.Set(uv_x, uv_y, 0F, 0F);
			vh.AddVert(vertex);
		}

		// 添加三角绘制
		for (int index = 2; index < count; index += 2)
		{
			vh.AddTriangle(index - 2, index + 1, index - 1);
			vh.AddTriangle(index - 2, index, index + 1);
		}
	}
}
