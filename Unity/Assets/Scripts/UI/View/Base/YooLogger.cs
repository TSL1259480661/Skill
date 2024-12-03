using App;
using System;
using UnityEngine;
using YooAsset;
using ILogger = YooAsset.ILogger;

public class YooLogger : ILogger
{
	private UDebugger debugger;

	public YooLogger(string name)
	{
		debugger = new UDebugger(name, UDebugger.LOG_LEVEL);
	}

	public void Error(string message)
	{
		debugger.LogError(message);
	}

	public void Exception(Exception exception)
	{
		debugger.LogException(exception);
	}

	public void Log(string message)
	{
		debugger.Log(message);
	}

	public void Warning(string message)
	{
		debugger.LogWarning(message);
	}
}
