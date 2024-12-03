using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 进行递归查找（可查找到所有资源，但是针对的只有纹理）
/// </summary>
public class TextureSearch 
{
	[MenuItem("Assets/检索图片资源并进行裁剪替换")]
	public static void StartTextureClip()
	{
		//Debug.Log(Selection.activeObject.name);
		var item = Selection.activeObject;//获取当前的点击对象的实例
		var path = AssetDatabase.GetAssetPath(item);//获取当前点击对象的路径
		//Debug.Log(path);
		Dfs(path,1);
		Debug.Log("------------------------------------>开始进行裁剪");
		Dfs(path,2);
		Debug.Log("------------------------------------>裁剪完成");
		Dfs(path,3);
	}

	private static void Dfs(string path,int type)
	{
		var allAssetsPath = AssetDatabase.FindAssets("*", new string[] { path });
		foreach (var o in allAssetsPath)
		{
			var assetPath = AssetDatabase.GUIDToAssetPath(o);
			CheckPathIsTexture(assetPath, type);
		}
	}

	private static Dictionary<string, bool> dic = new Dictionary<string, bool>();

	private static bool CheckPathIsTexture(string path,int val)
	{
		var type = AssetDatabase.GetMainAssetTypeAtPath(path);
		if(type == typeof(Texture2D) && !dic.ContainsKey(path))
		{
			if(val == 2)
			{
				dic.Add(path, true);
			}
			Debug.Log(path);
			TextureClip.SetTextureParam(AssetDatabase.LoadAssetAtPath<Texture2D>(path), (TextureImporter)AssetImporter.GetAtPath(path),path, val);
			return true;
		}
		return false;
	}
}
