using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Application;

namespace ET
{
	public static class ToolsEditor
	{

		[MenuItem("Tools/ExcelExporter")]
		public static void ExcelExporter()
		{
			SetClearLogStatus(false);

#if UNITY_EDITOR_OSX
            const string tools = "./ExcelExporterApp";
#else
			const string tools = ".\\ExcelExporterApp.exe";
#endif
			string path = AssetDatabase.GUIDToAssetPath("7de67389455806541b7323e5dd50a86d");
			var config = AssetDatabase.LoadAssetAtPath<EditorExportPathConfig>(path);
			if (string.IsNullOrEmpty(config.ExcelPath) || string.IsNullOrEmpty(config.ExcelExportClassPath)
				|| string.IsNullOrEmpty(config.ExcelExportJsonPath) || string.IsNullOrEmpty(config.ExcelExportBytePath))
			{
				Debug.LogError("请检查配置路径(提示：点击定位到配置文件)", config);
				return;
			}


			ClearFolder("Assets/Bundles/Config");
			ClearFolder(config.ExcelExportJsonPath);

			string cmdParams = $"{tools} --ExcelPath={Path.GetFullPath(config.ExcelPath)} --ExcelExportClassPath={Path.GetFullPath(config.ExcelExportClassPath)} --ExcelExportJsonPath={Path.GetFullPath(config.ExcelExportJsonPath)} --ExcelExportBytePath={Path.GetFullPath(config.ExcelExportBytePath)} ";
			bool succeed = ShellHelper.Run(cmdParams, "../ClientTool/");
			if (succeed)
			{
				ConfigModuleBase.ExportConfigModule();
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			}
		}

		[MenuItem("Tools/Proto2CS")]
		public static void Proto2CS()
		{
			SetClearLogStatus(false);

#if UNITY_EDITOR_OSX
            const string tools = "./Proto2CSApp";
#else
			const string tools = ".\\Proto2CSApp.exe";
#endif
			string path = AssetDatabase.GUIDToAssetPath("7de67389455806541b7323e5dd50a86d");
			var config = AssetDatabase.LoadAssetAtPath<EditorExportPathConfig>(path);
			if (string.IsNullOrEmpty(config.ProtoPath) || string.IsNullOrEmpty(config.ProtoExportPath))
			{
				Debug.LogError("请检查配置路径(提示：点击定位到配置文件)", config);
				return;
			}
			string cmdParams = $"{tools} --ProtoPath={Path.GetFullPath(config.ProtoPath)} --ProtoExportPath={Path.GetFullPath(config.ProtoExportPath)}";
			ShellHelper.Run(cmdParams, "../ClientTool/");

			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		static void ClearFolder(string folderPath)
		{
			// 检查文件夹是否存在
			if (Directory.Exists(folderPath))
			{
				// 获取文件夹中的所有文件和子文件夹
				string[] files = Directory.GetFiles(folderPath);
				string[] subdirectories = Directory.GetDirectories(folderPath);

				// 删除所有文件
				foreach (string file in files)
				{
					File.Delete(file);
				}

				// 递归清空所有子文件夹
				foreach (string subdirectory in subdirectories)
				{
					ClearFolder(subdirectory);
				}
			}
		}

		/// <summary>
		/// 刷新加载时禁止清空输出日志
		/// </summary>
		static void SetClearLogStatus(bool status)
		{
			int ClearOnRecompile = 0x1000;
			Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type logEntriesType = unityEditorAssembly.GetType("UnityEditor.LogEntries");
			MethodInfo SetConsoleFlagMethod = logEntriesType.GetMethod("SetConsoleFlag", BindingFlags.Static | BindingFlags.Public);
			SetConsoleFlagMethod.Invoke(null, new object[] { ClearOnRecompile, status });
		}
	}
}
