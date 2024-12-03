using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;
using Vector3 = UnityEngine.Vector3;

public static class GeometryMathsHelp
{
	public static Vector2 ToVector2(this Vector3 self, DrawPlane plane)
	{
		if (plane == DrawPlane.xy)
		{
			return new Vector2(self.x, self.y);
		}
		else if (plane == DrawPlane.xz)
		{
			return new Vector2(self.x, self.z);
		}
		else
		{
			return new Vector2(self.y, self.z);
		}
	}

	public static Vector3 ToVector3(this Vector2 self, DrawPlane plane)
	{
		if (plane == DrawPlane.xy)
		{
			return new Vector3(self.x, self.y, 0);
		}
		else if (plane == DrawPlane.xz)
		{
			return new Vector3(self.x, 0, self.y);
		}
		else
		{
			return new Vector3(0, self.x, self.y);
		}

	}
}

/// <summary>
/// 几何图形数学
/// </summary>
public class GeometryMaths
{

	public static Vector2 AngleToVector2D(float angle)
	{
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
	}


	public static bool IsCircleInSemicircle(Vector3 circleCenter, float circleRadius, Vector3 semicircleOrigin, float semicircleRadius, float semicircleStartAngle, float semicircleEendAngle, DrawPlane plane, ref float distanceToCircle, bool showLines = true)
	{
		float semicircleAngle = (semicircleEendAngle - semicircleStartAngle);
		var angle = semicircleAngle / 2 + semicircleStartAngle;
		Vector2 semicircleDirection = AngleToVector2D(angle);
		bool result = IsCircleInSemicircle(circleCenter.ToVector2(plane), circleRadius, semicircleOrigin.ToVector2(plane), semicircleRadius, semicircleDirection, semicircleAngle, ref distanceToCircle);
#if UNITY_EDITOR
		if (showLines)
		{
			GeometryMathsDraw.DrawWireCircle(circleCenter, circleRadius, plane, result ? Color.red : Color.blue);
			GeometryMathsDraw.DrawWireSemicircle(semicircleOrigin, semicircleRadius, semicircleDirection, semicircleAngle, plane, Color.yellow);
		}
#endif
		return result;
	}

	public static bool IsCircleInSemicircle(Vector3 circleCenter, float circleRadius, Vector3 semicircleOrigin, float semicircleRadius, Vector3 semicircleDirection, float semicircleAngle, DrawPlane plane, ref float distanceToCircle)
	{
		bool result = IsCircleInSemicircle(circleCenter.ToVector2(plane), circleRadius, semicircleOrigin.ToVector2(plane), semicircleRadius, semicircleDirection.ToVector2(plane), semicircleAngle, ref distanceToCircle);
#if UNITY_EDITOR
		GeometryMathsDraw.DrawWireCircle(circleCenter, circleRadius, plane, result ? Color.red : Color.blue);
		GeometryMathsDraw.DrawWireSemicircle(semicircleOrigin, semicircleRadius, semicircleDirection, semicircleAngle, plane, Color.yellow);
#endif
		return result;

	}
	/// <summary>
	/// 检查一个圆形是否在大的扇形范围内或相交
	/// </summary>
	/// <param name="circleCenter">圆形的中心点</param>
	/// <param name="circleRadius">圆形的半径</param>
	/// <param name="semicircleOrigin">扇形起点位置即圆的中心点</param>
	/// <param name="semicircleDirection">扇形方向</param>
	/// <param name="semicircleAngle">扇形角度</param>
	/// <param name="semicircleRadius">扇形半径</param>
	/// <returns></returns>
	public static bool IsCircleInSemicircle(Vector2 circleCenter, float circleRadius, Vector2 semicircleOrigin, float semicircleRadius, Vector2 semicircleDirection, float semicircleAngle, ref float distanceToCircle)
	{
		bool result = false;
		// 将扇形方向标准化
		semicircleDirection = semicircleDirection.normalized;
		// 计算圆心与扇形中心之间的向量
		Vector2 toCircle = circleCenter - semicircleOrigin;
		// 计算圆心与扇形中心的距离
		distanceToCircle = toCircle.magnitude;
		// 如果圆心距离扇形的中心大于扇形半径加上圆半径，肯定不会相交或包含
		if (distanceToCircle <= semicircleRadius + circleRadius)
		{
			// 计算圆心与扇形中心的夹角（范围在 0 到 180 度之间）
			float angle = Vector2.Angle(semicircleDirection, toCircle);
			if (angle <= semicircleAngle * 0.5f)
			{
				//Debug.Log("在扇形的范围内");
				result = true;
			}
			else
			{
				//Debug.Log("在扇形的范围外");
				// 获取扇形两条边的方向向量
				Vector2 leftEdgeDirection = RotatePoint(semicircleDirection, semicircleAngle * 0.5f);
				Vector2 rightEdgeDirection = RotatePoint(semicircleDirection, -semicircleAngle * 0.5f);
				var leftDirection = PointToLineSegmentDistance(circleCenter, semicircleOrigin, semicircleOrigin + leftEdgeDirection * semicircleRadius, out float leftDot);
				var rightDirection = PointToLineSegmentDistance(circleCenter, semicircleOrigin, semicircleOrigin + rightEdgeDirection * semicircleRadius, out float rightDot);
				result = (leftDirection < circleRadius) || (rightDirection < circleRadius);
			}
		}

		return result;
	}

	// 检查一个点是否在半圆范围内
	public static bool IsPointInSemicircle(Vector2 targetPosition, Vector2 semicircleOrigin, float semicircleRadius, Vector2 semicircleDirection, float semicircleAngle)
	{
		// 计算点与中心之间的向量
		Vector2 direction = targetPosition - semicircleOrigin;

		// 计算点与中心的角度
		float angle = Vector2.Angle(semicircleDirection, direction);
		//float angle = Mathf.Rad2Deg * Mathf.Acos(Vector2.Dot(direction.normalized, semicircleDirection.normalized));

		// 判断点是否在扇形半径范围内且在扇形角度范围内
		return direction.magnitude <= semicircleRadius && angle <= semicircleAngle * 0.5f;
	}

	//检查一个点是否在半圆范围内   TODO:这个函数 未验证准确性
	public static bool IsPointInSemicircle(Vector2 targetPosition, Vector2 semicircleOrigin, float semicircleRadius, float semicircleAngle, float semicircleRotation)
	{
		// 计算点与中心之间的向量
		Vector2 direction = targetPosition - semicircleOrigin;
		//距离超过检测半径
		if (direction.magnitude > semicircleRadius)
		{
			return false;
		}

		// 将半圆中轴线方向旋转到当前角度
		Vector2 rotatedSemicircleDirection = Quaternion.Euler(0, 0, semicircleRotation) * Vector2.right;

		// 计算点与中心的角度
		float angle = Vector2.Angle(rotatedSemicircleDirection, direction);

		// 判断点是否在扇形角度范围内
		return angle <= semicircleAngle * 0.5f;
	}

	/// <summary>
	/// 检查点是否在半圆内
	/// </summary>
	public static bool IsPointInSemicircle(Vector3 targetPosition, Vector3 origin, Vector3 direction, int angle, float radius, Vector3 axis)
	{
		// 计算目标点与半圆中心点的向量
		Vector3 toTarget = targetPosition - origin;
		// 计算夹角（使用 Atan2 函数）
		float angleToTarget = Mathf.Atan2(toTarget.z, toTarget.x) * Mathf.Rad2Deg;
		// 调整夹角，使其在 [0, 360] 范围内
		if (angleToTarget < 0f)
		{
			angleToTarget += 360f;
		}

		// 计算半圆的左右方向向量
		Vector3 leftdir = Quaternion.AngleAxis(-angle * 0.5f, axis) * direction;
		Vector3 rightdir = Quaternion.AngleAxis(angle * 0.5f, axis) * direction;
		// 计算目标点在半圆左右方向上的投影
		float projLeft = Vector3.Dot(toTarget, leftdir);
		float projRight = Vector3.Dot(toTarget, rightdir);
		// 检查夹角是否在半圆角度范围内，并且目标点到半圆中心的距离是否小于等于半圆的半径，
		// 以及目标点在半圆左右方向上的投影是否在范围内
		return angleToTarget >= -angle * 0.5f && angleToTarget <= angle * 0.5f &&
			   toTarget.magnitude <= radius && projLeft >= 0f && projRight >= 0f;
	}

	public static bool IsCircleInsideCircle(Vector2 targetCircleCenter, float targetCircleRadius, Vector2 circleCenter, float circleRadius)
	{
		float distance = Vector2.Distance(circleCenter, targetCircleCenter);
		float radiusDifference = circleRadius + targetCircleRadius;
		return distance < radiusDifference;
	}


	public static bool IsCircleInRotatedRect2D(Vector3 circleCenter, float circleRadius, Vector3 rectCenter, Vector2 rectSize, float rectRotation, Vector2 pivot, float pivotRotation, DrawPlane plane)
	{
		// 将圆心转换到矩形的本地空间;
		Vector2 circleCenter1 = RotatePoint(circleCenter.ToVector2(plane), rectCenter.ToVector2(plane), rectRotation);
		Vector2 circleCenter2 = RotatePoint(circleCenter1, pivot, pivotRotation);
#if UNITY_EDITOR
		if (IsCircleInRect(circleCenter2, circleRadius, rectCenter.ToVector2(plane), rectSize))
		{
			GeometryMathsDraw.DrawWireCircle(circleCenter, circleRadius, DrawPlane.xz, Color.red);
		}
		else
		{
			GeometryMathsDraw.DrawWireCircle(circleCenter, circleRadius, DrawPlane.xz, Color.green);
		}

		Quaternion rectQuaternion;
		Quaternion pivotQuaternion;
		if (plane == DrawPlane.xy)
		{
			rectQuaternion = Quaternion.Euler(0, 0, rectRotation);
			pivotQuaternion = Quaternion.Euler(0, 0, pivotRotation);
		}
		else if (plane == DrawPlane.xz)
		{
			rectQuaternion = Quaternion.Euler(0, rectRotation, 0);
			pivotQuaternion = Quaternion.Euler(0, pivotRotation, 0);
		}
		else
		{
			rectQuaternion = Quaternion.Euler(rectRotation, 0, 0);
			pivotQuaternion = Quaternion.Euler(pivotRotation, 0, 0);
		}

		GeometryMathsDraw.DrawWireRectangle(rectCenter, rectSize, rectQuaternion, pivot.ToVector3(plane), pivotQuaternion, Color.black); ;
#endif
		return IsCircleInRect(circleCenter2, circleRadius, rectCenter.ToVector2(plane), rectSize);
	}


	/// <summary>
	/// 检查圆是否在矩形范围内
	/// </summary>
	public static bool IsCircleInRect(Vector2 circleCenter, float circleRadius, Vector2 rectCenter, Vector2 rectSize)
	{
		// 计算矩形的左上角和右下角坐标
		float rectHalfWidth = rectSize.x / 2f;
		float rectHalfHeight = rectSize.y / 2f;

		float leftX = rectCenter.x - rectHalfWidth;
		float rightX = rectCenter.x + rectHalfWidth;
		float topY = rectCenter.y + rectHalfHeight;
		float bottomY = rectCenter.y - rectHalfHeight;

		// 检查圆心是否在矩形范围内
		bool isXInside = circleCenter.x >= leftX && circleCenter.x <= rightX;
		bool isYInside = circleCenter.y >= bottomY && circleCenter.y <= topY;

		if (isXInside && isYInside)
		{
			// 圆心在矩形范围内，无需进一步计算距离
			return true;
		}

		// 圆心不在矩形范围内，检查圆心到矩形的最近点的距离是否小于圆半径
		float closestX = Mathf.Clamp(circleCenter.x, leftX, rightX);
		float closestY = Mathf.Clamp(circleCenter.y, bottomY, topY);

		float distance = Vector2.Distance(circleCenter, new Vector2(closestX, closestY));

		return distance <= circleRadius;
	}


	/// <summary>
	/// 检查点是否在矩形范围内
	/// </summary>
	public static bool IsPointInRectangle(Vector2 point, Vector2 rectCenter, float rectWidth, float rectHeight)
	{
		// 计算矩形的左上角和右下角坐标
		float rectHalfWidth = rectWidth / 2f;
		float rectHalfHeight = rectHeight / 2f;

		float leftX = rectCenter.x - rectHalfWidth;
		float rightX = rectCenter.x + rectHalfWidth;
		float topY = rectCenter.y + rectHalfHeight;
		float bottomY = rectCenter.y - rectHalfHeight;

		// 检查点是否在矩形范围内
		return point.x >= leftX && point.x <= rightX && point.y >= bottomY && point.y <= topY;
	}


	/// <summary>
	/// 计算两点之间的距离
	/// </summary>
	public static float Distance(Vector2 point1, Vector2 point2)
	{
		return Vector2.Distance(point1, point2);
	}

	public static float Distance(Vector3 point1, Vector3 point2)
	{
		return Vector3.Distance(point1, point2);
	}

	/// <summary>
	/// 计算两点之间的平方距离（避免开方计算，用于比较距离大小）
	/// </summary>
	public static float SqrDistance(Vector2 point1, Vector2 point2)
	{
		return (point1 - point2).sqrMagnitude;
	}

	/// <summary>
	/// 计算两点之间的平方距离（避免开方计算，用于比较距离大小）
	/// </summary>
	public static float SqrDistance(Vector3 point1, Vector3 point2)
	{
		return (point1 - point2).sqrMagnitude;
	}

	/// <summary>
	/// 计算两个向量的夹角（弧度）
	/// </summary>
	public static float AngleBetweenVectors(Vector3 vector1, Vector3 vector2)
	{
		return Mathf.Acos(Vector3.Dot(vector1.normalized, vector2.normalized));
	}

	// 计算点到平面的投影点
	public static Vector3 ProjectPointOntoPlane(Vector3 point, Vector3 planeNormal, Vector3 planePoint)
	{
		float distance = Vector3.Dot(planeNormal, point - planePoint);
		return point - distance * planeNormal;
	}

	// 计算射线与平面的交点
	public static bool RayIntersectsPlane(Ray ray, Vector3 planeNormal, Vector3 planePoint, out Vector3 intersectionPoint)
	{
		float distance;
		bool intersects = RayIntersectsPlane(ray, planeNormal, planePoint, out distance);
		intersectionPoint = ray.origin + ray.direction * distance;
		return intersects;
	}

	private static bool RayIntersectsPlane(Ray ray, Vector3 planeNormal, Vector3 planePoint, out float distance)
	{
		distance = 0f;
		float denom = Vector3.Dot(planeNormal, ray.direction);

		if (Mathf.Approximately(denom, 0f))
		{
			// Ray is parallel to the plane
			return false;
		}

		distance = Vector3.Dot((planePoint - ray.origin), planeNormal) / denom;

		return distance >= 0f;
	}

	/// <summary>
	/// 计算点到直线的距离
	/// </summary>
	public static float DistancePointToLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
	{
		float lineLength = Distance(lineStart, lineEnd);

		if (lineLength < float.Epsilon)
		{
			return Distance(point, lineStart);
		}

		float t = Vector2.Dot(point - lineStart, lineEnd - lineStart) / (lineLength * lineLength);
		t = Mathf.Clamp01(t);

		//点在直线上的投影点
		Vector2 projection = lineStart + t * (lineEnd - lineStart);

		return Distance(point, projection);
	}

	/// <summary>
	/// 计算点到直线的距离
	/// </summary>
	public static float DistancePointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		float lineLength = Distance(lineStart, lineEnd);

		if (lineLength < float.Epsilon)
		{
			return Distance(point, lineStart);
		}

		float t = Vector3.Dot(point - lineStart, lineEnd - lineStart) / (lineLength * lineLength);
		t = Mathf.Clamp01(t);

		Vector3 projection = lineStart + t * (lineEnd - lineStart);

		return Distance(point, projection);
	}

	/// <summary>
	/// 计算点到直线AB的距离
	/// </summary>
	public static float DistanceFromPointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		float paDistance = Vector3.Distance(lineEnd, point);
		Vector3 ab = lineEnd - lineStart;
		Vector3 pb = lineEnd - point;
		float dotResult = Vector3.Dot(ab, pb);
		// 求θ
		float seitaRad = Mathf.Acos(dotResult / (ab.magnitude * paDistance));
		//求p点到ab的距离
		float distance = paDistance * Mathf.Sin(seitaRad);
		return distance;
	}

	/// <summary>
	/// 点到线段的最短距离
	/// </summary>
	/// <param name="point"></param>
	/// <param name="lineStart"></param>
	/// <param name="lineEnd"></param>
	/// <returns></returns>
	public static float PointToLineSegmentDistance(Vector2 point, Vector2 lineStart, Vector2 lineEnd, out float dot)
	{
		Vector2 line = lineEnd - lineStart;
		Vector2 toPoint = point - lineStart;

		float lineLengthSquared = line.sqrMagnitude;
		if (lineLengthSquared == 0f)
		{
			dot = 0;
			// 线段退化成一个点
			return Vector2.Distance(point, lineStart);
		}

		// 计算投影系数t
		dot = Vector2.Dot(toPoint, line);
		float t = dot / lineLengthSquared;

		// 限制t在0到1之间
		t = Mathf.Clamp01(t);

		// 计算投影点C
		Vector2 projection = lineStart + t * line;

		// 返回点P到投影点C的距离
		return Vector2.Distance(point, projection);
	}

	/// <summary>
	/// 点到射线的距离
	/// </summary>
	public static float DistancePointToRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
	{
		// 计算点到射线起点的向量
		Vector2 toPoint = point - rayOrigin;
		// 计算点到射线方向的垂直向量（点到射线的投影向量）
		float projectionLength = Vector2.Dot(toPoint, rayDirection);
		// 计算点到射线的投影向量的长度，即点到射线的距离
		float distance = Vector2.Distance(point, rayOrigin + projectionLength * rayDirection);
		return distance;
	}

	/// <summary>
	/// 点到射线的距离
	/// </summary>
	/// <param name="point">点</param>
	/// <param name="rayOrigin">射线起点</param>
	/// <param name="rayDirection">射线方向</param>
	/// <param name="dot"></param>
	/// <returns>点到射线的最短距离</returns>
	public static float DistancePointToRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection, out float dot)
	{
		// 计算点到射线起点的向量
		Vector2 toPoint = point - rayOrigin;
		// 计算点到射线方向的垂直向量（点到射线的投影向量）
		float projectionLength = Vector2.Dot(toPoint, rayDirection);
		// 计算点到射线的投影向量的长度，即点到射线的距离
		float distance = Vector2.Distance(point, rayOrigin + projectionLength * rayDirection);
		dot = projectionLength;
		return distance;
	}

	/// <summary>
	/// 判断点是否在线段上
	/// </summary>
	public static bool IsPointOnLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
	{
		float distance = Distance(lineStart, lineEnd);
		float distanceToPoint = Distance(point, lineStart) + Distance(point, lineEnd);

		return Mathf.Approximately(distance, distanceToPoint);
	}

	/// <summary>
	/// 判断点是否在直线上
	/// </summary>
	public static bool IsPointOnLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		float distance = Distance(lineStart, lineEnd);
		float distanceToPoint = Distance(point, lineStart) + Distance(point, lineEnd);

		return Mathf.Approximately(distance, distanceToPoint);
	}

	/// <summary>
	/// 判断点是否在射线上
	/// </summary>
	public static bool IsPointOnRay(Vector2 point, Vector2 rayOrigin, Vector2 rayDirection)
	{
		if (Vector2.Dot(point - rayOrigin, rayDirection) < 0f)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// 判断点是否在射线上
	/// </summary>
	public static bool IsPointOnRay(Vector3 point, Vector3 rayOrigin, Vector3 rayDirection)
	{
		if (Vector3.Dot(point - rayOrigin, rayDirection) < 0f)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// 计算二维向量的叉积
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <returns>大于0，表示B在A的左边;小于0，表示B在A的右边;等于0，表示B和A共线;</returns>
	public static float Cross2D(Vector2 a, Vector2 b)
	{
		float z = a.x * b.y - b.x * a.y;
		return z;
	}


	/// <summary>
	/// 点P是否在三角形内部;检查点是否在三角形内
	/// </summary>
	public static bool IsPointInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
	{
		//return true if the point to test is one of the vertices
		if (p.Equals(a) || p.Equals(b) || p.Equals(c))
			return true;

		Vector2 pa = a - p;
		Vector2 pb = b - p;
		Vector2 pc = c - p;
		float t1 = pa.x * pb.y - pa.y * pb.y;   //float t1 = pa.Cross2D(pb);
		float t2 = pb.x * pc.y - pb.y * pc.y;   //float t2 = pb.Cross2D(pc);
		float t3 = pc.x * pa.y - pc.y * pa.y;   //float t3 = pc.Cross2D(pa);
												//return t1 * t2 >= 0 && t1 * t3 >= 0 && t2 * t3 >= 0;
		if (t1 < 0 && t2 < 0 && t3 < 0)
		{
			return true;
		}
		if (t1 > 0 && t2 > 0 && t3 > 0)
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// 点P是否在三角形内部
	/// </summary>
	public static bool IsPointInsideTriangle(Vector3 point, Vector3 a, Vector3 b, Vector3 c)
	{
		//基于点与三角形的三个顶点构成的平面法线方向的点积。如果点在三角形内，那么与三个平面的法线方向的点积应该都大于零
		//return true if the point to test is one of the vertices
		if (point.Equals(a) || point.Equals(b) || point.Equals(c))
			//如果测试点等于三角形的任一顶点，直接返回 true，因为点在三角形的顶点上
			return true;

		Vector3 vectorA = a - point;
		Vector3 vectorB = b - point;
		Vector3 vectorC = c - point;
		Vector3 normalAB = Vector3.Cross(vectorA, vectorB);
		Vector3 normalBC = Vector3.Cross(vectorB, vectorC);
		Vector3 normalCA = Vector3.Cross(vectorC, vectorA);

		float dotAB = Vector3.Dot(normalAB, normalBC);
		float dotBC = Vector3.Dot(normalAB, normalCA);
		float dotCA = Vector3.Dot(normalBC, normalCA);

		if (dotAB > 0 && dotBC > 0 && dotCA > 0) return true;
		return false;
	}

	public static Vector2 ComputeTriangleCentroid(Vector2 a, Vector2 b, Vector2 c)
	{
		return (a + b + c) / 3f;
	}

	public static float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
	{
		float area = b.x * c.y - b.x * c.y;
		area += a.x * b.y - a.x * b.y;
		area += a.x * c.y - a.x * c.y;
		return (area * 0.5f);
	}

	public static float PolygonArea(Vector2[] points)
	{
		int n = points.Length;
		float area = 0.0f;
		for (int q = 0, p = n - 1; q < n; p = q++)
		{
			Vector2 pval = points[p];
			Vector2 qval = points[q];
			area += pval.x * qval.y - qval.x * pval.y;
		}
		return (area * 0.5f);
	}


	//Where is p in relation to a-b
	// <0 -> to the right
	// =0 -> on the line
	// >0 -> to the left
	public static float ComputePointOrientation(Vector2 a, Vector2 b, Vector2 p)
	{
		float determinant = (a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x);
		return determinant;
	}

	//两条线 是否相交
	public static bool AreLinesIntersecting()
	{
		throw new NotImplementedException();
	}

	public static Vector2 RotatePoint(Vector2 point, float degrees)
	{
		float radians = Mathf.Deg2Rad * degrees;
		float sin = Mathf.Sin(radians);
		float cos = Mathf.Cos(radians);
		float newX = point.x * cos - point.y * sin;
		float newY = point.x * sin + point.y * cos;
		return new Vector2(newX, newY);
	}

	//将point绕pivot旋转degrees
	public static Vector2 RotatePoint(Vector2 point, Vector2 pivot, float degrees)
	{
		float radians = Mathf.Deg2Rad * degrees;
		float cos = Mathf.Cos(radians);
		float sin = Mathf.Sin(radians);
		float x = cos * (point.x - pivot.x) - sin * (point.y - pivot.y) + pivot.x;
		float y = sin * (point.x - pivot.x) + cos * (point.y - pivot.y) + pivot.y;
		return new Vector2(x, y);
	}


	public static Vector2 RotateAroundPoint(Vector2 vector, Vector2 pivot, float degrees)
	{
		float radians = Mathf.Deg2Rad * degrees;
		float sin = Mathf.Sin(radians);
		float cos = Mathf.Cos(radians);
		// 将点平移到原点
		vector -= pivot;
		// 进行旋转
		float newX = vector.x * cos - vector.y * sin;
		float newY = vector.x * sin + vector.y * cos;
		// 将点还原
		return new Vector2(newX, newY) + pivot;
	}
	public static Vector3 RotatePoint(Vector3 point, Vector3 pivot, Vector3 angle)
	{
		Quaternion rotation = Quaternion.Euler(angle.x, angle.y, angle.z);
		return RotatePoint(point, pivot, rotation);
	}
	public static Vector3 RotatePoint(Vector3 point, Vector3 pivot, Quaternion rotation)
	{
		Vector3 relativePosition = point - pivot;// 获取当前位置相对于旋转中心的向量
		return rotation * relativePosition + pivot;
	}


	#region Bezier 贝赛尔曲线

	// 线性 贝赛尔曲线
	public static Vector3 Bezier(Vector3 p0, Vector3 p1, float t)
	{
		return (1 - t) * p0 + t * p1;
	}
	public static Vector2 Bezier(Vector2 p0, Vector2 p1, float t)
	{
		return (1 - t) * p0 + t * p1;
	}

	// 二阶曲线 贝赛尔曲线
	public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		Vector3 p0p1 = (1 - t) * p0 + t * p1;
		Vector3 p1p2 = (1 - t) * p1 + t * p2;

		Vector3 result = (1 - t) * p0p1 + t * p1p2;

		return result;
	}
	public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
	{
		Vector2 p0p1 = (1 - t) * p0 + t * p1;
		Vector2 p1p2 = (1 - t) * p1 + t * p2;

		Vector2 result = (1 - t) * p0p1 + t * p1p2;

		return result;
	}

	// 三阶曲线 贝赛尔曲线
	public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		Vector3 p0p1 = (1 - t) * p0 + t * p1;
		Vector3 p1p2 = (1 - t) * p1 + t * p2;
		Vector3 p2p3 = (1 - t) * p2 + t * p3;

		Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
		Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;

		Vector3 result = (1 - t) * p0p1p2 + t * p1p2p3;

		return result;
	}
	public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
	{
		Vector2 p0p1 = (1 - t) * p0 + t * p1;
		Vector2 p1p2 = (1 - t) * p1 + t * p2;
		Vector2 p2p3 = (1 - t) * p2 + t * p3;

		Vector2 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
		Vector2 p1p2p3 = (1 - t) * p1p2 + t * p2p3;

		Vector2 result = (1 - t) * p0p1p2 + t * p1p2p3;

		return result;
	}

	// n阶曲线，递归实现
	public static Vector3 Bezier(List<Vector3> p, float t)
	{
		if (p.Count < 2)
			return p[0];
		List<Vector3> newp = new List<Vector3>();
		for (int i = 0; i < p.Count - 1; i++)
		{
			//Debug.DrawLine(p[i], p[i + 1]);
			Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
			newp.Add(p0p1);
		}
		return Bezier(newp, t);
	}
	public static Vector2 Bezier(List<Vector2> p, float t)
	{
		if (p.Count < 2)
			return p[0];
		List<Vector2> newp = new List<Vector2>();
		for (int i = 0; i < p.Count - 1; i++)
		{
			//Debug.DrawLine(p[i], p[i + 1]);
			Vector2 p0p1 = (1 - t) * p[i] + t * p[i + 1];
			newp.Add(p0p1);
		}
		return Bezier(newp, t);
	}

	#endregion
}
