using System;
using System.IO;
using System.Text;
using UnityEngine;



public static class EditorLog
{
	public const string LogFileName = "EditorBuildLog.txt";
	public static string LogFilePath = $"{Application.dataPath}/../EditorLog";
	public static string LogFile = $"{LogFilePath}/{LogFileName}";


	public static void Info(string content) 
	{
		var info = FormatLogInfo(LogType.Info.ToString(), content);
		Debug.Log(info);
		SaveFile(info);
	}

	public static void Warnning(string content)
	{
		var info = FormatLogInfo(LogType.Warning.ToString(), content);
		Debug.LogWarning(info);
		SaveFile(info);
	}

	public static void Error(string content)
	{
		var info = FormatLogInfo(LogType.Error.ToString(), content);
		Debug.LogError(info);
		SaveFile(info);
	}

	public static void ClearLog()
	{
		if (File.Exists(LogFile))
		{
			File.Delete(LogFile);
		}
	}


	private static string FormatLogInfo(string logType, string logInfo)
	{
		var formatInfos = new StringBuilder();
		formatInfos.AppendLine($"---------------> Time = {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
		formatInfos.AppendLine($"{logType}: {logInfo}");
		if (_isOpenStack)
		{
			formatInfos.AppendLine("StackTrace:");
			formatInfos.AppendLine($"{StackTraceUtility.ExtractStackTrace()}");
		}
		formatInfos.AppendLine("<---------------");
		formatInfos.AppendLine("\n");

		return formatInfos.ToString();
	}

	private static void SaveFile(string logStr)
	{
		if (!Directory.Exists(LogFilePath))
		{
			Directory.CreateDirectory(LogFilePath);
		}

		if (!File.Exists(LogFile))
		{
			var fs = File.Create(LogFile);
			fs.Close();
		}

		File.AppendAllText($"{LogFilePath}/{LogFileName}", logStr, Encoding.Default);
	}

	private enum LogType
	{
		Info,
		Warning,
		Error,
	}

	private static string _aLogInfo;
	private static bool _isOpenStack = false;
}
