using Sirenix.Serialization.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

namespace UnityEngine.UI
{
	/// <summary>
	/// 新手引导圆形裁剪区域
	/// </summary>
	public class GuideCircleImage : MaskableGraphic
	{
		/// <summary>
		/// 切割点
		/// </summary>
		private const int segments = 100;

		/// <summary>
		/// 圆形中心点
		/// </summary>
		private Vector2 origin = Vector2.zero;

		/// <summary>
		/// 计算缓存值
		/// </summary>
		private Vector3 cache = Vector3.zero;

		/// <summary>
		/// 重新绘制
		/// </summary>
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();
			Vector2 piovt = rectTransform.pivot;
			float width = rectTransform.rect.width;
			float height = rectTransform.rect.height;
			float outer_lx = -piovt.x * width;
			float outer_by = -piovt.y * height;
			float outer_rx = outer_lx + width;
			float outer_ty = outer_by + height;
			float center_x = outer_lx + width / 2F;
			float center_y = outer_by + height / 2F;
			origin.Set(center_x, center_y);

			// 外顶点
			UIVertex vert = UIVertex.simpleVert;
			vert.color = color;

			// 0 - Outer : LT
			cache.Set(outer_lx, outer_ty, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 1 - Outer : CT
			cache.Set(center_x, outer_ty, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 2 - Outer : RT
			cache.Set(outer_rx, outer_ty, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 3 - Outer : RC
			cache.Set(outer_rx, center_y, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 4 - Outer : RB
			cache.Set(outer_rx, outer_by, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 5 - Outer : CB
			cache.Set(center_x, outer_by, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 6 - Outer : LB
			cache.Set(outer_lx, outer_by, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 7 - Outer : LC
			cache.Set(outer_lx, center_y, 0F);
			vert.position = cache;
			vh.AddVert(vert);

			// 圆边缘点
			float curRadian = 0;
			Vector3 old = Vector3.zero;
			float radius = width * 0.5F;
			float radian = Mathf.PI * 2 / segments;
			for (int index = 0; index <= segments; ++index)
			{
				float x = Mathf.Cos(curRadian) * radius;
				float y = Mathf.Sin(curRadian) * radius;
				curRadian += radian;

				UIVertex uvTemp = UIVertex.simpleVert;
				uvTemp.color = color;
				uvTemp.position.Set(x + origin.x, y + origin.y, 0F);
				vh.AddVert(uvTemp);

				if (index > 1)
				{
					int key_index = index + 8;
					if (uvTemp.position.x < center_x && old.x > center_x)
					{
						vh.AddTriangle(0, 1, key_index);
						vh.AddTriangle(1, 2, key_index - 1);
					}
					else if (uvTemp.position.x > center_x && old.x < center_x)
					{
						vh.AddTriangle(4, 5, key_index);
						vh.AddTriangle(5, 6, key_index - 1);
					}
					else if (uvTemp.position.y < center_y && old.y > center_y)
					{
						vh.AddTriangle(6, 7, key_index);
						vh.AddTriangle(0, 7, key_index - 1);
					}
					else if (uvTemp.position.y > center_y && old.y < center_y)
					{
						vh.AddTriangle(2, 3, key_index);
						vh.AddTriangle(3, 4, key_index - 1);
					}
					else if (uvTemp.position.x > center_x && old.x > center_x)
					{
	                    if (uvTemp.position.y > center_y && old.y > center_y)
						{
							vh.AddTriangle(2, key_index, key_index - 1);
						}
						else
						{
							vh.AddTriangle(4, key_index, key_index - 1);
						}
					}
					else if (uvTemp.position.x < center_x && old.x < center_x)
					{
						if (uvTemp.position.y > center_y && old.y > center_y)
						{
							vh.AddTriangle(0, key_index, key_index - 1);
						}
						else
						{
							vh.AddTriangle(6, key_index, key_index - 1);
						}
					}
				}
				old = uvTemp.position;
			}
		}
	}
}
