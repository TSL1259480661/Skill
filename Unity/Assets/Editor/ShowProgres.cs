using UnityEngine;
using UnityEditor;

public class ShowProgres 
{
	public static void ShowPro(int num,int val)
	{
		EditorUtility.DisplayCancelableProgressBar("裁剪进度：", $"{val}/" + num + "", 1.0f*val/num);
		if(val == num)
		{
			EditorUtility.ClearProgressBar();
			Debug.Log("清理进度条");
		}
	}
	public static void ShowCurrentPath(string currentPath)
	{
		EditorUtility.DisplayCancelableProgressBar("当前检索路径：", currentPath,0);
	}
	public static void ClearProgressBar()
	{
		EditorUtility.ClearProgressBar();
	}
}
