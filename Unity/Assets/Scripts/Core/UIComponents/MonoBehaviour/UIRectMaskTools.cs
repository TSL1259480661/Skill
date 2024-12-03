using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRectMaskTools : MonoBehaviour
{
	public GameObject[] MaskObjs;

	private void Start()
	{
		var rectMaskItem = transform.GetComponent<RectMaskItem>();

		if (rectMaskItem != null)
		{
			return;
		}
		gameObject.AddComponent<RectMaskItem>();//业务程序集去动态加载 不要直接挂在 不然插件代码会被裁剪掉
	}


}
