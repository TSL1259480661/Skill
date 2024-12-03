using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ClientEditor
{
	class UIEditorController
	{
		[MenuItem("GameObject/SpawnEUICode", false, -2)]
		static public void CreateNewCode()
		{
			GameObject go = Selection.activeObject as GameObject;
			UICodeSpawner.SpawnEUICode(go);
		}

		[MenuItem("Assets/AssetBundle/NameUIPrefab")]
		public static void NameAllUIPrefab()
		{
			string suffix = ".unity3d";
			UnityEngine.Object[] selectAsset = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
			for (int i = 0; i < selectAsset.Length; i++)
			{
				string prefabName = AssetDatabase.GetAssetPath(selectAsset[i]);
				//MARKER：判断是否是.prefab
				if (prefabName.EndsWith(".prefab"))
				{
					Debug.Log(prefabName);
					AssetImporter importer = AssetImporter.GetAtPath(prefabName);
					importer.assetBundleName = selectAsset[i].name.ToLower() + suffix;
				}

			}
			AssetDatabase.Refresh();
			AssetDatabase.RemoveUnusedAssetBundleNames();
		}

		[MenuItem("Assets/AssetBundle/ClearABName")]
		public static void ClearABName()
		{
			UnityEngine.Object[] selectAsset = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
			for (int i = 0; i < selectAsset.Length; i++)
			{
				string prefabName = AssetDatabase.GetAssetPath(selectAsset[i]);
				AssetImporter importer = AssetImporter.GetAtPath(prefabName);
				importer.assetBundleName = string.Empty;
				Debug.Log(prefabName);
			}
			AssetDatabase.Refresh();
			AssetDatabase.RemoveUnusedAssetBundleNames();
		}

		[MenuItem("Assets/RefreshUIAssetPaths")]
		static public void RefreshUIAssetPaths()
		{
			UICodeSpawner.RefreshUIAssetPaths();
		}


		[MenuItem("Assets/GenerateConfigModule")]
		static public void GenerateConfigModule()
		{
			ConfigModuleBase.ExportConfigModule();
		}

		[MenuItem("Assets/重设角色怪物的SpriteAnimation")]
		static public void ResetSpriteAnimation()
		{
			string selectedPath = "Assets/Bundles/Units";
			string[] guids = Selection.assetGUIDs;
			string[] tagArry = new string[] { "attack", "dead", "walk", "skill1", "skill2", "skill3", "skill4" };
			if (guids.Length > 0)
			{
				for (int i = 0; i < guids.Length; i++)
				{
					selectedPath = AssetDatabase.GUIDToAssetPath(guids[i]);
					Debug.Log($"路径：{selectedPath}");
					HandleSpriteAnimation(selectedPath, tagArry);
				}
			}
			else
			{
				HandleSpriteAnimation(selectedPath, tagArry);
			}
		}

		private static void HandleSpriteAnimation(string selectedPath, string[] tagArry)
		{
			List<string> foundPaths = new List<string>();
			for (int i = 0; i < tagArry.Length; i++)
			{
				foundPaths.AddRange(FindAllSubfoldersByName(selectedPath, tagArry[i]));
			}
			HandleSpriteAnimation(foundPaths);

		}

		//递归查找具有特定名称的子文件夹
		private static List<string> FindAllSubfoldersByName(string rootPath, string folderName)
		{
			// 查找具有特定名称的所有子文件夹
			List<string> foundPaths = new List<string>();

			if (string.IsNullOrEmpty(rootPath) || string.IsNullOrEmpty(folderName))
				return foundPaths;

			string[] subDirs = Directory.GetDirectories(rootPath);
			foreach (string subDir in subDirs)
			{
				if (Path.GetFileName(subDir) == folderName)
				{
					// 找到了匹配的子文件夹
					foundPaths.Add(subDir);
				}
				else
				{
					// 递归查找子文件夹
					List<string> childFoundPaths = FindAllSubfoldersByName(subDir, folderName);
					if (childFoundPaths.Count > 0)
					{
						foundPaths.AddRange(childFoundPaths);
					}
				}
			}

			return foundPaths;
		}

		//根据路径列表处理
		private static void HandleSpriteAnimation(List<string> pathList)
		{
			List<string> spriteAniParentPathList = new List<string>();
			for (int i = 0; i < pathList.Count; i++)
			{
				Debug.Log($"HandleSpriteAnimation:{pathList[i]}");
				HandleSpriteAnimation(pathList[i]);

				string parentDir = Path.GetDirectoryName(pathList[i]);
				if (spriteAniParentPathList.Contains(parentDir) == false)
				{
					spriteAniParentPathList.Add(parentDir);
				}
			}
			//处理main.asset
			for (int i = 0; i < spriteAniParentPathList.Count; i++)
			{
				CreateMainSpriteAnimation(spriteAniParentPathList[i]);
			}
		}

		//处理SpriteAnimation
		private static void HandleSpriteAnimation(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}

			//----处理.asset文件
			//文件夹名字
			string directoryName = Path.GetFileName(path);
			SpriteAnimation spriteAnimation;
			string scriptAssetPath = path + $"/{directoryName}.asset";
			if (File.Exists(scriptAssetPath) == false)
			{
				spriteAnimation = ScriptableObject.CreateInstance<SpriteAnimation>();
				AssetDatabase.CreateAsset(spriteAnimation, scriptAssetPath);
			}
			else
			{
				spriteAnimation = AssetDatabase.LoadAssetAtPath<SpriteAnimation>(scriptAssetPath);
			}
			//---处理图片
			string[] allPngPath = Directory.GetFiles(path, "*.png");
			Sprite[] spriteArry = new Sprite[allPngPath.Length];
			for (int i = 0; i < allPngPath.Length; i++)
			{
				SetPngImorter(allPngPath[i]);
				Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(allPngPath[i]);
				spriteArry[i] = sprite;
			}
			// 设置动画属性
			//spriteAnimation.sprites = spriteArry;

			// 保存 SpriteAnimation 实例
			EditorUtility.SetDirty(spriteAnimation);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		//设置图片格式
		private static void SetPngImorter(string filePath)
		{
			// 获取文件的 AssetImporter
			TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(filePath);
			// 设置 import settings
			importer.textureType = TextureImporterType.Sprite;
			importer.spriteImportMode = SpriteImportMode.Single;
			importer.SaveAndReimport(); // 保存更改并重新导入
		}

		private static void CreateMainSpriteAnimation(string path)
		{
			//AnimationBehavior animationBehavior; 
			//string scriptAssetPath = path + $"/Main.asset";
			//if (File.Exists(scriptAssetPath) == false)
			//{
			//	animationBehavior = ScriptableObject.CreateInstance<AnimationBehavior>();
			//	AssetDatabase.CreateAsset(animationBehavior, scriptAssetPath);
			//}
			//else
			//{
			//	animationBehavior = AssetDatabase.LoadAssetAtPath<AnimationBehavior>(scriptAssetPath);
			//}

			//string[] allAssetPath = Directory.GetFiles(path, "*" + ".asset", SearchOption.AllDirectories);

			//for (int i = 0; i < allAssetPath.Length; i++)
			//{
			//	SpriteScriptableObject spriteAnimation = AssetDatabase.LoadAssetAtPath<SpriteAnimation>(allAssetPath[i]);
			//	string fileName = Path.GetFileNameWithoutExtension(allAssetPath[i]);

			//	if (fileName == "attack")
			//	{
			//		animationBehavior.Skill11 = spriteAnimation;
			//	}
			//	else if (fileName == "dead")
			//	{
			//		animationBehavior.Death = spriteAnimation;
			//	}
			//	else if (fileName == "walk")
			//	{
			//		animationBehavior.MoveLeft = spriteAnimation;
			//		animationBehavior.MoveRight = spriteAnimation;
			//	}
			//	else if (fileName == "skill1")
			//	{
			//		animationBehavior.Skill12 = spriteAnimation;
			//	}
			//	else if (fileName == "skill2")
			//	{
			//		animationBehavior.Skill13 = spriteAnimation;
			//	}
			//	else if (fileName == "skill3")
			//	{
			//		animationBehavior.Skill14 = spriteAnimation;
			//	}
			//	else if (fileName == "skill4")
			//	{
			//		animationBehavior.Skill15 = spriteAnimation;
			//	}
			//}
			//EditorUtility.SetDirty(animationBehavior);
			//AssetDatabase.SaveAssets();
			//AssetDatabase.Refresh();
		}

		#region png加描边
		static string inputImagePath = "Assets/PNG_WaitAddOutline";
		static Material shaderMaterial;




		//[MenuItem("Assets/PNG_OutLine/CreatePng_InLine")]
		//public static void CreateOutlinePng_InLine()
		//{
		//	CreateOutlinePng_Shader("Assets/Shader/_ImgInLine.mat", "In_");
		//}

		[MenuItem("Assets/PNG_OutLine/CreatePng_OutLine")]
		public static void CreateOutlinePng_OutLine()
		{
			CreateOutlinePng_Shader("Assets/Shader/_ImgOutLine.mat", "Out_");
		}


		[MenuItem("Assets/PNG_OutLine/CreatePng_OutLine(并删除源文件)")]
		public static void CreateOutlinePng_OutLine_DelOriginFile()
		{
			CreateOutlinePng_Shader("Assets/Shader/_ImgOutLine.mat", "Out_", true);
		}

		private static void CreateOutlinePng_Shader(string matPath, string line, bool needDel = false)
		{
			//选中的文件夹
			string[] guids = Selection.assetGUIDs;
			if (guids.Length > 0)
			{
				for (int i = 0; i < guids.Length; i++)
				{
					inputImagePath = AssetDatabase.GUIDToAssetPath(guids[i]);
					Debug.Log($"路径：{inputImagePath}");
					CreateOutlinePng(matPath, line, needDel);
				}
			}
			AssetDatabase.Refresh();
		}

		private static void CreateOutlinePng(string matPath, string line, bool needDel = false)
		{
			shaderMaterial = AssetDatabase.LoadAssetAtPath<Material>(matPath);
			string[] allAssetPath = Directory.GetFiles(inputImagePath, "*" + ".png", SearchOption.AllDirectories);
			for (int i = 0; i < allAssetPath.Length; i++)
			{
				if (shaderMaterial != null)
				{
					Texture2D originalTexture = LoadTextureFromPath(allAssetPath[i]);
					string fileName = Path.GetFileNameWithoutExtension(allAssetPath[i]);
					if (fileName.Contains(line))
					{
						Debug.Log("已经生成过了");
						return;
					}

					string imgPath = Path.GetDirectoryName(allAssetPath[i]);
					if (needDel)
					{
						ApplyShaderAndSave(originalTexture, imgPath + $"/{line}{fileName}.png", imgPath + $"/{fileName}.png");
					}
					else
					{
						ApplyShaderAndSave(originalTexture, imgPath + $"/{line}{fileName}.png");
					}
				}
			}
			string[] ThumbsPath = Directory.GetFiles(inputImagePath, "Thumbs*", SearchOption.AllDirectories);
			for (int i = 0; i < ThumbsPath.Length; i++)
			{
				File.Delete(ThumbsPath[i]);
			}

		}
		private static Texture2D LoadTextureFromPath(string path)
		{
			byte[] fileData = File.ReadAllBytes(path);
			Texture2D texture = new Texture2D(2, 2);
			texture.LoadImage(fileData);
			return texture;
		}

		private static void ApplyShaderAndSave(Texture inputTexture, string outpath, string delPath = "")
		{
			// 创建一个渲染纹理，与输入纹理相同尺寸
			RenderTexture renderTexture = new RenderTexture(inputTexture.width, inputTexture.height, 24);
			RenderTexture.active = renderTexture;

			// 创建一个纹理，用于存储最终的结果
			Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height);

			// 渲染纹理到材质
			Graphics.Blit(inputTexture, renderTexture, shaderMaterial);

			// 读取渲染纹理到输出纹理
			outputTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
			outputTexture.Apply();

			// 释放资源
			RenderTexture.active = null;

			// 保存纹理为PNG文件
			SaveImage(outputTexture, outpath);

			if (string.IsNullOrEmpty(delPath) == false)
			{
				File.Delete(delPath);
			}
		}
		private static void SaveImage(Texture2D texture, string outputPath)
		{
			byte[] bytes = texture.EncodeToPNG();
			File.WriteAllBytes(outputPath, bytes);
			while (!File.Exists(outputPath))
			{
				System.Threading.Thread.Sleep(100);
			}
			AssetDatabase.Refresh();
			SetPngImorter(outputPath);
		}

		/// <summary>
		/// 通用设置资源Asset
		/// </summary>
		[MenuItem("Assets/选中目录收集图片生成.Asset资源，文件名=目录名")]
		private static void CommonSetAsset()
		{
			string[] guids = Selection.assetGUIDs;
			if (guids.Length > 0)
			{
				for (int i = 0; i < guids.Length; i++)
				{
					string selectedPath = AssetDatabase.GUIDToAssetPath(guids[i]);
					Debug.Log($"路径：{selectedPath}");
					HandleSpriteAnimation(selectedPath);
				}
			}
		}
		#endregion

		[MenuItem("Assets/设置预制默认音效(10003)")]
		private static void SetPrefabDefaultSound()
		{
			string[] guids = Selection.assetGUIDs;
			for (int i = 0; i < guids.Length; i++)
			{
				SetPrefabDefaultSound(AssetDatabase.GUIDToAssetPath(guids[i]));
			}
		}

		private static void SetPrefabDefaultSound(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			DirectoryInfo dirInfo = new DirectoryInfo(path);
			foreach (FileInfo fileInfo in dirInfo.GetFiles("*", SearchOption.AllDirectories))
			{
				if (fileInfo.Extension == ".prefab")
				{
					string relativePath = Path.Combine("Assets", fileInfo.FullName.Substring(Application.dataPath.Length + 1));
					relativePath= relativePath.Replace("\\", "/"); 
					UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(relativePath, typeof(GameObject));
					GameObject go = (GameObject)prefab;
					Debug.Log(fileInfo.Name);
					ModifyComponents(go);
				}
			}

			AssetDatabase.SaveAssets();
		}

		private static void ModifyComponents(GameObject go)
		{
			GButton[] gBtnArry = go.GetComponentsInChildren<GButton>(true);
			GToggle[] gTogArry = go.GetComponentsInChildren<GToggle>(true);
			if (gBtnArry != null)
			{
				foreach (GButton tc in gBtnArry)
				{
					SerializedObject so = new SerializedObject(tc);
					SerializedProperty sp = so.FindProperty("soundId");
					if (sp != null && sp.propertyType == SerializedPropertyType.Integer && sp.intValue<AudioIDConst.BattleMusic)
					{
						sp.intValue = 10003;
						so.ApplyModifiedProperties(); // 应用修改
					}
				}
			}
			if (gTogArry != null)
			{

				foreach (GToggle tc in gTogArry)
				{
					SerializedObject so = new SerializedObject(tc);
					SerializedProperty sp = so.FindProperty("soundId");
					if (sp != null && sp.propertyType == SerializedPropertyType.Integer)
					{
						sp.intValue = 10003;
						so.ApplyModifiedProperties(); // 应用修改
					}
				}
			}
		}
	}
}
