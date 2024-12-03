using UnityEngine;
using WeChatWASM;
using UnityEditor;

public class BuildEditor
{
	public static void TestBuild()
	{
		EditorLog.Info("Test Build Success!");

		string[] args = System.Environment.GetCommandLineArgs();
		EditorLog.Info($"==============================================================> len={args.Length}");
	}

	public static void BuildRes()
	{
		// 先导一遍excel
		EditorLog.Info($"开始导表！");
		ET.ToolsEditor.ExcelExporter();
		EditorLog.Info($"导表完成！");

		// 构建资源
		EditorLog.Info($"Start build webgl asset!");
		ResBuildEditor.BuildRes();
		EditorLog.Info($"Build webgl asset done!");

	}


    public static void StartBuild()
	{
		// 清理以前的日志
		EditorLog.ClearLog();		

		try
		{
			BuildRes();

			string[] args = System.Environment.GetCommandLineArgs();

			if (args.Length == 0)
			{
				EditorLog.Error("传入参数数量为空！");
				throw new UnityException();
			}

			// 打印所有命令行参数
			var index = 0;
			foreach (string arg in args)
			{
				EditorLog.Info($"Args {index}: {arg}");
				index += 1;
			}
			
			if(args.Length < 12)
			{
				EditorLog.Info($"命令行参数数量不对！");
				throw new UnityException("命令行参数数量不对！");
			}

			EditorLog.Info($"Set game name");
			string gameName = args[9];

			EditorLog.Info($"Set app id");
			string appId = args[10];

			EditorLog.Info($"Set cdn");
			string cdn = args[11];

			EditorLog.Info($"Set exprot path");
			string exprotPath = args[12];

			EditorLog.Info($"Game Name = {gameName}");
			EditorLog.Info($"AppId = {appId}");
			EditorLog.Info($"Cdn = {cdn}");
			EditorLog.Info($"ExprotPath = {exprotPath}");
 
			var wxConfig = new WxMiniGameConfig();
			wxConfig.ResCdn = cdn;
			wxConfig.AppId = appId;
			wxConfig.Name = gameName;
			wxConfig.ExportPath = exprotPath;

			PlatformBuildEditor.SetWxMiniGameConfig(wxConfig);

			EditorLog.Info($"Start build wxminigame!");
			BuildWxMiniGame();
		}
		catch (UnityException e)
		{
			EditorLog.Error($"打包失败, Error:{e}");
			throw e;
		}
	}

   [MenuItem("Editor/编辑器/测试打包")]
	public static void BuildWxMiniGame()
	{
		// 先设置小游戏配置
		PlatformBuildEditor.SetMiniGameConfig();

		

		// 转换为小游戏
		EditorLog.Info("Start Generate Wx Mini Game!");
		WXConvertCore.DoExport();
		EditorLog.Info("Generate Done!");
	}
}
