using System.Collections.Generic;
using UnityEngine;

public class Rectangler : IObjectPoolItem
{
	private static UObjectPool<Rectangler> itemPool = new UObjectPool<Rectangler>();

	public float width { get; private set; }
	public float height { get; private set; }

	public Vector2 center { get; private set; }
	public Vector2 leftDown { get; private set; }
	public Vector2 leftTop { get; private set; }
	public Vector2 rightTop { get; private set; }
	public Vector2 rightDown { get; private set; }


	public void Init(Vector2 leftDown, float width, float height)
	{
		this.leftDown = leftDown;
		this.width = width;
		this.height = height;

		this.center = new Vector2(leftDown.x + width / 2, leftDown.y + height / 2);
		this.leftTop = new Vector2(leftDown.x, leftDown.y + height);
		this.rightTop = new Vector2(leftDown.x + width, leftDown.y + height);
		this.rightDown = new Vector2(leftDown.x + width, leftDown.y);
	}

	public void InitByCenter(Vector2 center, float width, float height)
	{
		Vector2 leftDown = new Vector2(center.x - width / 2f, center.y - height / 2f);
		this.Init(leftDown, width, height);
	}

	public void Draw(Vector2 offset, Color color, float durtion)
	{
#if UNITY_EDITOR
		//float scale = GlobalConst.PositionRatio;
		//Debug.DrawLine((offset + leftDown) * scale, (offset + leftTop) * scale, color, durtion);
		//Debug.DrawLine((offset + leftTop) * scale, (offset + rightTop) * scale, color, durtion);
		//Debug.DrawLine((offset + rightTop) * scale, (offset + rightDown) * scale, color, durtion);
		//Debug.DrawLine((offset + rightDown) * scale, (offset + leftDown) * scale, color, durtion);
#endif
	}

	//矩形内随机一个点
	public Vector2 RandomPoint()
	{
		float x = Random.Range(this.leftDown.x, this.rightTop.x);
		float y = Random.Range(this.leftDown.y, this.rightTop.y);
		return new Vector2(x, y);
	}

	public void AddPointInSector(List<Vector2> points, Vector2 center, float startAngle, float endAngle)
	{
		if (IsPointInAngles(center, startAngle, endAngle, leftTop))
		{
			points.Add(leftTop);
		}
		if (IsPointInAngles(center, startAngle, endAngle, leftDown))
		{
			points.Add(leftDown);
		}
		if (IsPointInAngles(center, startAngle, endAngle, rightDown))
		{
			points.Add(rightDown);
		}
		if (IsPointInAngles(center, startAngle, endAngle, rightTop))
		{
			points.Add(rightTop);
		}
	}

	//判断点是否在两射线夹角内
	private bool IsPointInAngles(Vector2 center, float startAngle, float endAngle, Vector2 point)
	{
		Vector3 direction = point - center;
		// 计算向量的方向角度（相对于正X轴）
		float angle = Vector2.SignedAngle(Vector2.right, new Vector2(direction.x, direction.y));
		// 调整角度，使其在0到360度之间
		angle = (angle + 360) % 360;
		// 检查角度是否在扇形的角度范围内
		bool angleInRange = false;
		if (startAngle <= endAngle)
		{
			angleInRange = angle >= startAngle && angle <= endAngle;
		}
		else
		{
			angleInRange = angle >= startAngle || angle <= endAngle;
		}
		// 判断点是否在扇形内
		return angleInRange;
	}

	//射线与矩形边的交点
	public Vector2? IntersectSideRay(Vector2 rayStart, Vector2 rayDir)
	{
		Vector2? point;

		point = IntersectLineRay(rayStart, rayDir, leftDown, rightDown);
		if (point.HasValue)
			return point.Value;
		point = IntersectLineRay(rayStart, rayDir, rightDown, rightTop);
		if (point.HasValue)
			return point.Value;
		point = IntersectLineRay(rayStart, rayDir, rightTop, leftTop);
		if (point.HasValue)
			return point.Value;
		point = IntersectLineRay(rayStart, rayDir, leftTop, leftDown);
		if (point.HasValue)
			return point.Value;

		return null;
	}

	//射线与矩形边的交点
	public (Vector2, bool) IntersectSideRay2(Vector2 rayStart, Vector2 rayDir)
	{
		Vector2? point;

		point = IntersectLineRay(rayStart, rayDir, leftDown, rightDown);
		if (point.HasValue)
			return (point.Value, true);
		point = IntersectLineRay(rayStart, rayDir, rightDown, rightTop);
		if (point.HasValue)
			return (point.Value, false);
		point = IntersectLineRay(rayStart, rayDir, rightTop, leftTop);
		if (point.HasValue)
			return (point.Value, true);
		point = IntersectLineRay(rayStart, rayDir, leftTop, leftDown);
		if (point.HasValue)
			return (point.Value, false);

		return (Vector2.zero, false);
	}

	//射线与一条线段是否相交
	public static Vector2? IntersectLineRay(Vector2 rayStart, Vector2 rayDir, Vector2 lineStart, Vector2 lineEnd)
	{
		Vector2 lineVec = lineEnd - lineStart;
		Vector2 rayVec = rayDir.normalized;
		Vector2 startToRayOrigin = rayStart - lineStart;

		float a = Vector2.Dot(rayVec, rayVec);
		float b = Vector2.Dot(rayVec, lineVec);
		float c = Vector2.Dot(lineVec, lineVec);
		float d = Vector2.Dot(startToRayOrigin, rayVec);
		float e = Vector2.Dot(startToRayOrigin, lineVec);
		float det = a * c - b * b;

		if (Mathf.Abs(det) < Mathf.Epsilon) return null; // 射线平行于线段

		float t = (b * e - c * d) / det;
		float u = (a * e - b * d) / det;

		if (t >= 0 && u >= 0 && u <= 1)
		{
			return rayStart + t * rayVec;
		}
		else
		{
			return null;
		}
	}


	public static Rectangler Get()
	{
		return itemPool.Get();
	}


	public static void Recycle(Rectangler item)
	{
		item.Recycle();
	}


	public static void Clear()
	{
		itemPool.RecycleAll();
	}

	public void Recycle()
	{
		itemPool.Recycle(this);
	}


	public void OnRecycle()
	{
	}

	public void OnReuse()
	{
	}
}
