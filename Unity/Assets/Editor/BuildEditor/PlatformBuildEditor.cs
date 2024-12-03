using System.IO;
using UnityEditor;
using WeChatWASM;



public class PlatformBuildEditor
{

	public static string WxMiniGameConfigPath = @"Assets/WX-WASM-SDK-V2/Editor/MiniGameConfig.asset";


	public static void SetWxMiniGameConfig(WxMiniGameConfig config)
	{
		_wxConfig = config;
	}


	public static void SetMiniGameConfig()
	{
		if (!File.Exists(WxMiniGameConfigPath))
		{
			EditorLog.Error($"路径={WxMiniGameConfigPath}资源不存在，请检查！");
			return;
		}
		if (_wxConfig == null) _wxConfig = new WxMiniGameConfig();
		var wxConfig = AssetDatabase.LoadAssetAtPath<WXEditorScriptObject>(WxMiniGameConfigPath);

		wxConfig.ProjectConf.CDN = _wxConfig.ResCdn;
		wxConfig.ProjectConf.Appid = _wxConfig.AppId;
		wxConfig.ProjectConf.DST = _wxConfig.ExportPath;
		wxConfig.ProjectConf.projectName = _wxConfig.Name;
		wxConfig.ProjectConf.MemorySize = _wxConfig.UnityHeap;
		wxConfig.ProjectConf.assetLoadType = _wxConfig.FirstPackLoadType;
		wxConfig.ProjectConf.Orientation = _wxConfig.ScreenOrientation;

		// TODO: 后续再改
		// 优化相关选项 
		//wxConfig.ProjectConf.compressDataPackage = true;
		//wxConfig.CompileOptions.DeleteStreamingAssets = _wxConfig.IsClearStreamingAssets;
		////wxConfig.CompileOptions.enableIOSPerformancePlus = true;	
		//wxConfig.CompileOptions.Il2CppOptimizeSize = true;		

		EditorUtility.SetDirty(wxConfig);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		EditorLog.Info("已更新微信小游戏的配置！");
	}

	private static WxMiniGameConfig _wxConfig;
}
