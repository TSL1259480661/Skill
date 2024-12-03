using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;
/// <summary>
/// 进行递归查找（可查找到所有资源，但是针对的只有纹理）
/// </summary>
public class TextureAtals
{
	private static string savePath = "";
	private static string loadPath = "";
	private static string arrPath = "";
	private static bool isLoad = false;
	[MenuItem("Assets/检索纹理目录，并将其打包进图集")]
	public static void StartTextureClip()
	{ 
		loadPath = "";
		arrPath = "";
		isLoad = false;
		var item = Selection.activeObject;//获取当前的点击对象的实例
		var path = AssetDatabase.GetAssetPath(item);//获取当前点击对象的路径
		savePath = path + "_Atlas";
		loadPath = path;
		Dfs(path);
		ShowProgres.ClearProgressBar();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	private static void Dfs(string path)
	{
		ShowProgres.ShowCurrentPath(path);
		DirectoryInfo rd = new DirectoryInfo(path);
		
		if (isLoad)
		{
			arrPath += "/" + rd.Name;
		}
		if (!isLoad) isLoad = true;

		if (!Directory.Exists(savePath + arrPath))
		{
			Directory.CreateDirectory(savePath + arrPath);
		}
		string currentPath = arrPath;

		var allPath = rd.GetDirectories();

		if (allPath.Length == 0)
		{
			DealDic(path,rd.Name,arrPath);
		}
		else
		{
			foreach (var o in allPath)
			{		
				Dfs(loadPath + arrPath + "/" + o.Name);
				arrPath = currentPath;
			}
		}
	}

	private static void DealDic(string path,string name,string arrPath)
	{
		//Debug.Log($"当前路径:{path},已不存在其他目录，生成图集并设置");
		var allAssets = AssetDatabase.FindAssets("*",new string[] {path});
		bool isTexture = true;
		foreach (var o in allAssets)
		{
			string aPath = AssetDatabase.GUIDToAssetPath(o);
			var type = AssetDatabase.GetMainAssetTypeAtPath(aPath);
			if (type != typeof(Texture2D))
			{
				isTexture = false;
			}
			if (!isTexture) break;
		}
		if (isTexture)
		{
			CreateAtlasPath(path,name,arrPath);
		}
	}

	private static void CreateAtlasPath(string path,string name, string arrPath)
	{
		var texture = AssetDatabase.LoadMainAssetAtPath(path);
		string newSavePath = savePath + arrPath;

		string atlasPath = newSavePath + "/" + name + ".spriteatlas";

		UnityEngine.Object[] textures = new UnityEngine.Object[100];
		Sprite[] sprites = new Sprite[100];
		int index = 0;
		var t = AssetDatabase.FindAssets("*", new string[] { path });
		bool f = false;
		foreach (var o in t)
		{
			var p = AssetDatabase.GUIDToAssetPath(o);
			if (AssetDatabase.GetMainAssetTypeAtPath(p) == typeof(Texture2D))
			{
				var tt = AssetDatabase.LoadMainAssetAtPath(p);
				var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(p);
				var te = tt as Texture2D;

				if (te.width == 1024 && te.height == 1024)
				{
					f = true;
				}

				textures[index] = tt;
				sprites[index++] = sprite;
			}
		}
		if (f)
		{
			SpriteAnimation sAn = ScriptableObject.CreateInstance<SpriteAnimation>();
			sAn.sprites = new Sprite[index];
			for (int i = 0; i < index; i++)
			{
				sAn.sprites[i] = (sprites[i]);
			}
			AssetDatabase.CreateAsset(sAn, newSavePath + "/" + name + ".asset");
		}
		else
		{
			SpriteAtlas sa = new SpriteAtlas();
			SpriteAtlasPackingSettings saSet = new SpriteAtlasPackingSettings()
			{
				blockOffset = 1,
				enableRotation = false,
				enableTightPacking = false,
				padding = 4
			};
			sa.SetPackingSettings(saSet);

			SpriteAtlasTextureSettings textureSet = new SpriteAtlasTextureSettings()
			{
				readable = true,
				generateMipMaps = false,
				sRGB = true,
				filterMode = FilterMode.Bilinear,
			};
			sa.SetTextureSettings(textureSet);

			AssetDatabase.CreateAsset(sa, atlasPath);
			SpriteAtlasExtensions.Add(sa, textures );
			SpriteAtlasTextureSettings textureSetDone = new SpriteAtlasTextureSettings()
			{
				readable = false,
				generateMipMaps = false,
				sRGB = true,
				filterMode = FilterMode.Bilinear,
			};
			sa.SetTextureSettings(textureSetDone);

			AssetDatabase.SaveAssets();
			CreateSpriteAtlass(newSavePath + "/" + name + ".asset", name, sa);
		}
	}

	private static void CreateSpriteAtlass(string path,string name,SpriteAtlas sa)
	{
		SpriteAnimationAltas saa = ScriptableObject.CreateInstance<SpriteAnimationAltas>();
		saa.spriteAtlas = sa;
		AssetDatabase.CreateAsset(saa, path);
	}
}
