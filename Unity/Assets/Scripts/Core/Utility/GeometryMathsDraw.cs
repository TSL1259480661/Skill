using UnityEngine;
using Color = UnityEngine.Color;


public enum DrawPlane
{
	xy,
	xz,
	yz
}
public class GeometryMathsDraw
{
	//public static void DrawRect(GPositionLimitComponent scenePt, Rect rect, Color color, float duration)
	//{
	//	Vector3 vec1 = scenePt.GetInScenePosition(new Vector3(rect.xMin, rect.yMin)) * GlobalConst.PositionRatio;
	//	Vector3 vec2 = scenePt.GetInScenePosition(new Vector3(rect.xMax, rect.yMin)) * GlobalConst.PositionRatio;
	//	Vector3 vec3 = scenePt.GetInScenePosition(new Vector3(rect.xMax, rect.yMax)) * GlobalConst.PositionRatio;
	//	Vector3 vec4 = scenePt.GetInScenePosition(new Vector3(rect.xMin, rect.yMax)) * GlobalConst.PositionRatio;
	//	Debug.DrawLine(vec1, vec2, color, duration);
	//	Debug.DrawLine(vec2, vec3, color, duration);
	//	Debug.DrawLine(vec3, vec4, color, duration);
	//	Debug.DrawLine(vec4, vec1, color, duration);
	//}

	public static void DrawSemicircle(Vector3 semicircleOrigin, float semicircleRadius, float semicircleStartAngle, float semicircleEendAngle, Color color, float duration, DrawPlane plane = DrawPlane.xy)
	{
		float semicircleAngle = (semicircleEendAngle - semicircleStartAngle);
		var angle = semicircleAngle / 2 + semicircleStartAngle;
		Vector2 semicircleDirection = GeometryMaths.AngleToVector2D(angle);
#if UNITY_EDITOR
		DrawWireSemicircle(semicircleOrigin, semicircleRadius, semicircleDirection, semicircleAngle, plane, color, duration);
#endif
	}

	/// <summary>
	/// 绘制半圆
	/// </summary>
	/// <param name="origin">起点</param>
	/// <param name="direction">方向</param>
	/// <param name="radius">半径</param>
	/// <param name="angle">角度</param>
	/// <param name="axis">轴</param>
	public static void DrawWireSemicircle(Vector3 origin, float radius, Vector3 semicircleDirection, float angle,
		DrawPlane plane, Color drawColor, float drawDur = 1f)
	{
		semicircleDirection = semicircleDirection.normalized;
		Vector3 axis = Vector3.forward;
		if (plane == DrawPlane.xz)
			axis = Vector3.up;
		else if (plane == DrawPlane.yz)
			axis = Vector3.right; ;

		Vector3 leftdir = Quaternion.AngleAxis(-angle / 2, axis) * semicircleDirection;
		Vector3 rightdir = Quaternion.AngleAxis(angle / 2, axis) * semicircleDirection;

		Vector3 currentP = origin + leftdir * radius;
		Vector3 oldP;
		if (angle != 360)
		{
			Debug.DrawLine(origin, currentP, drawColor, drawDur);
		}
		for (int i = 0; i < angle / 10; i++)
		{
			Vector3 dir = Quaternion.AngleAxis(10 * i, axis) * leftdir;
			oldP = currentP;
			currentP = origin + dir * radius;
			Debug.DrawLine(oldP, currentP, drawColor, drawDur);
		}
		oldP = currentP;
		currentP = origin + rightdir * radius;
		Debug.DrawLine(oldP, currentP, drawColor, drawDur);
		if (angle != 360)
		{
			Debug.DrawLine(currentP, origin, drawColor, drawDur);
		}
	}
	/// <summary>
	/// 绘制半圆
	/// </summary>
	/// <param name="origin">起点</param>
	/// <param name="direction">方向</param>
	/// <param name="radius">半径</param>
	/// <param name="angle">角度</param>
	/// <param name="axis">轴</param>
	public static void DrawWireSemicircle(Vector3 origin, float radius, float semicircleStartAngle, float semicircleEendAngle,
		DrawPlane plane, Color drawColor, float drawDur = 1f)
	{
		float semicircleAngle = (semicircleEendAngle - semicircleStartAngle);
		var angle = semicircleAngle / 2 + semicircleStartAngle;
		Vector3 semicircleDirection = GeometryMaths.AngleToVector2D(angle);
		if (plane == DrawPlane.xz)
			semicircleDirection = new Vector3(semicircleDirection.x, 0, semicircleDirection.y);
		else if (plane == DrawPlane.xz)
			semicircleDirection = new Vector3(0, semicircleDirection.x, semicircleDirection.y);
		DrawWireSemicircle(origin, radius, semicircleDirection, angle, plane, drawColor, drawDur);
	}


	public static void DrawWireCircle(Vector3 circleCenter, float circleRadius, DrawPlane plane, Color drawColor, float drawDur = 0.5f)
	{
		int segments = 36; // 调整根据需要的精度
						   //Quaternion rotation = Quaternion.LookRotation(rectDirection, Vector3.up);
		Vector3[] circlePoints = new Vector3[segments];

		for (int i = 0; i < segments; i++)
		{
			float angle = i * 2 * Mathf.PI / segments;
			float x = Mathf.Cos(angle) * circleRadius;
			float y = Mathf.Sin(angle) * circleRadius;
			Vector3 point = new Vector3(x, y, 0);
			if (plane == DrawPlane.xz)
				point = new Vector3(x, 0, y);
			else if (plane == DrawPlane.yz)
				point = new Vector3(0, x, y);

			// point = rotation * point; // 旋转
			circlePoints[i] = circleCenter + point;
		}

		// 连接圆周上的点，形成圆形
		for (int i = 0; i < segments - 1; i++)
		{
			Debug.DrawLine(circlePoints[i], circlePoints[i + 1], drawColor, drawDur);
		}
		Debug.DrawLine(circlePoints[segments - 1], circlePoints[0], drawColor, drawDur); // 连接首尾，形成闭环
	}

	public static void DrawWireRectangle(Vector3 rectCentre, Vector2 rectSize, Vector3 rectDirection)
	{
		// 计算矩形方向的旋转
		Quaternion rectRotation = Quaternion.LookRotation(rectDirection.normalized, Vector3.up);
		DrawWireRectangle(rectCentre, rectSize, rectRotation);
	}
	public static void DrawWireRectangle(Vector3 rectCentre, Vector2 rectSize, Quaternion rectRotation)
	{
		DrawWireRectangle(rectCentre, rectSize, rectRotation, Vector3.zero, Quaternion.identity, Color.yellow);
	}
	public static void DrawWireRectangle(Vector3 rectCentre, Vector2 rectSize, Vector3 pivot, Quaternion pivotRotation)
	{
		DrawWireRectangle(rectCentre, rectSize, Quaternion.identity, pivot, pivotRotation, Color.yellow);
	}

	/// <summary>
	/// 绘制矩形
	/// </summary>
	/// <param name="rectCentre">矩阵中心点</param>
	/// <param name="rectSize">矩阵大小</param>
	/// <param name="rectRotation">矩阵绕自身的旋转</param>
	/// <param name="pivot">旋转支点</param>
	/// <param name="pivotRotation">绕支点的旋转</param>
	public static void DrawWireRectangle(Vector3 rectCentre, Vector2 rectSize, Quaternion rectRotation, Vector3 pivot, Quaternion pivotRotation, Color drawColor, float drawDur = 1f)
	{
		Vector3 halfSize = new Vector3(rectSize.x * 0.5f, 0, rectSize.y * 0.5f);
		// 计算矩形的四个角点
		Vector3 topLeft = rectCentre - halfSize;
		Vector3 topRight = new Vector3(rectCentre.x + halfSize.x, rectCentre.y, rectCentre.z - halfSize.z);
		Vector3 bottomRight = rectCentre + halfSize;
		Vector3 bottomLeft = new Vector3(rectCentre.x - halfSize.x, rectCentre.y, rectCentre.z + halfSize.z);

		//自旋转
		topLeft = GeometryMaths.RotatePoint(topLeft, rectCentre, rectRotation);
		topRight = GeometryMaths.RotatePoint(topRight, rectCentre, rectRotation);
		bottomRight = GeometryMaths.RotatePoint(bottomRight, rectCentre, rectRotation);
		bottomLeft = GeometryMaths.RotatePoint(bottomLeft, rectCentre, rectRotation);
		//绕某一点旋转
		topLeft = GeometryMaths.RotatePoint(topLeft, pivot, pivotRotation);
		topRight = GeometryMaths.RotatePoint(topRight, pivot, pivotRotation);
		bottomRight = GeometryMaths.RotatePoint(bottomRight, pivot, pivotRotation);
		bottomLeft = GeometryMaths.RotatePoint(bottomLeft, pivot, pivotRotation);

		// 绘制矩形的四条边线
		Debug.DrawLine(topLeft, topRight, drawColor, drawDur);
		Debug.DrawLine(topRight, bottomRight, drawColor, drawDur);
		Debug.DrawLine(bottomRight, bottomLeft, drawColor, drawDur);
		Debug.DrawLine(bottomLeft, topLeft, drawColor, drawDur);
	}
}
