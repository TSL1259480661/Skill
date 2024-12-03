using System;
using System.IO;
using UnityEditor;
using YooAsset.Editor;

public class ResBuildEditor
{
	public static string BuildPackage = "DefaultPackage";

	//TODO: 
	//[MenuItem("chen/BuildRes")]
	public static void BuildRes()
	{
		string yoo_path = AssetBundleBuilderHelper.GetStreamingAssetsRoot();

		DeleteDirectory(yoo_path);

		AssetDatabase.Refresh();

		string PackageName = BuildPackage;
		var BuildPipeline = EBuildPipeline.ScriptableBuildPipeline;

		var buildMode = AssetBundleBuilderSetting.GetPackageBuildMode(PackageName, BuildPipeline);
		var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(PackageName, BuildPipeline);
		var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(PackageName, BuildPipeline);
		var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(PackageName, BuildPipeline);
		var compressOption = AssetBundleBuilderSetting.GetPackageCompressOption(PackageName, BuildPipeline);

		ScriptableBuildParameters buildParameters = new ScriptableBuildParameters();
		buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
		buildParameters.BuildinFileRoot = yoo_path;
		buildParameters.BuildPipeline = BuildPipeline.ToString();
		buildParameters.BuildTarget = BuildTarget.WebGL;
		buildParameters.BuildMode = buildMode;
		buildParameters.PackageName = PackageName;
		buildParameters.PackageVersion = GetDefaultPackageVersion();
		buildParameters.EnableSharePackRule = true;
		buildParameters.VerifyBuildingResult = true;
		buildParameters.FileNameStyle = fileNameStyle;
		buildParameters.BuildinFileCopyOption = buildinFileCopyOption;
		buildParameters.BuildinFileCopyParams = buildinFileCopyParams;
		buildParameters.EncryptionServices = null;
		buildParameters.CompressOption = compressOption;

		ScriptableBuildPipeline pipeline = new ScriptableBuildPipeline();
		var buildResult = pipeline.Run(buildParameters, true);
		//if (buildResult.Success)//会导致进程被持有住 不能有
		//	EditorUtility.RevealInFinder(buildResult.OutputPackageDirectory);
	}

	public static void DeleteDirectory(string targetDir)
	{
		string[] files = Directory.GetFiles(targetDir);
		string[] dirs = Directory.GetDirectories(targetDir);

		foreach (string file in files)
		{
			File.SetAttributes(file, FileAttributes.Normal);
			File.Delete(file);
		}

		foreach (string dir in dirs)
		{
			DeleteDirectory(dir);
		}

		Directory.Delete(targetDir, false);
	}


	private static string GetDefaultPackageVersion()
	{
		int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
		return DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
	}
}

