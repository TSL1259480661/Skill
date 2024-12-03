using System.Xml.Schema;
using UnityEditor;
using UnityEditor.SceneTemplate;
using UnityEngine;

/// <summary>
/// 摄像机震动组件
/// </summary>
public class CameraQuiver 
{
	private Camera shakeCamera;//震动摄像机
	public void Init(Camera shakeCamera)
	{
		this.shakeCamera = shakeCamera;
	}

	//损伤值对应函数：（实际震动值）y = (震动系数，代指上界)maxOffset*shake*shake;

	private float maxAangle = 1;//角度倍率（震动旋转幅度）
	private float maxOffset = 1;//位置倍率（震动平移幅度）
	private float time=1;//震动时间(需要一个标准量)
	private float shake = 0;//代指损伤值（动态变化）(范围0-1)

	public void InitData(float maxAangle,float maxOffset,float time)
	{
		this.maxAangle = maxAangle;
		this.maxOffset = maxOffset;
		this.time = time;
	}

	private float GetShakeValue(float shake)
	{
		return shake * shake;
	}

	/// <summary>
	/// 震动
	/// </summary>
	/// <param name="shake">震动值（累加）</param>
	/// <param name="time">震动时间</param>
	public void Shake(float shake,float time = -1)
	{
		this.shake += shake;
		this.time = time == -1 ? this.time : time;
		this.shake = Mathf.Min(this.shake , 1);
		Reset();

		Shake();
	}

	private int shakeId = -1;
	private void Shake()
	{
		cPos = shakeCamera.transform.localPosition;
		shakeId = App.DoTween.Instance.Add(0,shake,time, Shaking,ShakeDone);
	}
	private float angle;
	private float offsetX;
	private float offsetY;
	
	private void Shaking(float val, object o)
	{
		Debug.Log(val);
		this.shake = 1 - val;
		Debug.Log($"当前的震动值{shake}");
		angle = maxAangle * GetShakeValue(shake);

		float dx = Random.Range(-1f, 1f);
		offsetX = maxOffset * GetShakeValue(shake) * dx;
		float dy = Random.Range(-1f, 1f);
		offsetY = maxOffset * GetShakeValue(shake) * dy;
		Debug.Log($"x轴震动值{offsetX}，y轴振动值{offsetY}");
		shakeCamera.transform.localPosition = cPos + new Vector3(offsetX,offsetY,0);
	}

	private Vector3 cPos = Vector3.zero;
	private void ShakeDone(int id,object o)
	{
		this.shake = 0;
		shakeCamera.transform.localPosition = cPos;
	}

	private void Reset()
	{
		if(shakeId != -1)
		{
			App.DoTween.Instance.Remove(shakeId);
			shakeId = -1;
		}
	}
}
