using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;

namespace App
{
	using Debug = UnityEngine.Debug;

	public class UDebugger
	{
		public const int NONE_LEVEL = 0;
		public const int ERROR_LEVEL = 1;
		public const int LOG_LEVEL = 2;
		public const int SYSTEM_LEVEL = 3;
		public const int DEBUG_LEVEL = 4;
		public const int ALL_LOG_LEVEL = 5;
#if RELEASE
		private static int _allLevel = ERROR_LEVEL;
#else
		private static int _allLevel = LOG_LEVEL;
#endif
		public static int AllLevel
		{
			get
			{
				return _allLevel;
			}
		}

		/// <summary>
		/// 上报日志回调
		/// </summary>
		private static Action<string> reportAction = null;

		public static void SetAllLevel(int level)
		{
			_allLevel = level;
		}

		public static UDebugger Create(string name)
		{
			UDebugger debugger = new UDebugger(name);
			return debugger;
		}

		public static void Init(Action<string> action)
		{
			reportAction = action;
			Application.logMessageReceivedThreaded += WriteLogLine;
		}

		public static void Clear()
		{
			reportAction = null;
			Application.logMessageReceivedThreaded -= WriteLogLine;
		}

		public static UDebugger Create(string name, int level)
		{
			UDebugger debugger = new UDebugger(name, level);
			return debugger;
		}

		private string name;
		public UDebugger(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				Debug.LogError("UDebugger name can not be null or empty!");
			}
			else
			{
				this.name = LOG_1 + name + LOG_5;
			}
		}

		public UDebugger(string name, int selfLevel)
		{
			this.name = LOG_1 + name + LOG_5;
			this.selfLevel = selfLevel;
		}

		private int selfLevel = LOG_LEVEL;
		public void SetSelfLevel(int level)
		{
			selfLevel = level;
		}

		private System.DateTime currentTime;

		private static string LOG_1 = "[";
		private static string LOG_2 = ":";
		private static string LOG_3 = "-";
		private static string LOG_4 = "_";
		private static string LOG_5 = "] ";
		private void SetTime(StringBuilder builder)
		{
			DateTime dataTime = DateTime.UtcNow.AddHours(8);
			builder.Append(LOG_1);
			builder.Append(dataTime.Hour);
			builder.Append(LOG_2);
			builder.Append(dataTime.Minute);
			builder.Append(LOG_2);
			builder.Append(dataTime.Second);
			builder.Append(LOG_3);
			builder.Append(dataTime.Millisecond);
			builder.Append(LOG_4);
			int count = -1;
			try
			{
				count = Time.frameCount;
			}
			catch
			{

			}
			builder.Append(count);
			builder.Append(LOG_5);
		}

		private static bool _logErrorToFile = false;
		public static void SetLogErrorToFile(bool logErrorToFile)
		{
			_logErrorToFile = logErrorToFile;
		}

		private static bool _logToFile = false;
		public static void SetLogToFile(bool logToFile)
		{
			_logToFile = logToFile;
		}

		private static StringBuilder builder = new StringBuilder(2000);
		private static string NULL = "null";
		private static string LOG_KEY = "[LOG]";
		private static string LOG_ERROR_KEY = "[ERROR]";

		public bool LogEnable()
		{
			return _allLevel >= LOG_LEVEL && selfLevel <= _allLevel && selfLevel >= LOG_LEVEL;
		}

		public bool LogErrorEnable()
		{
			return _allLevel >= ERROR_LEVEL && selfLevel >= ERROR_LEVEL;
		}

		public void Log(params object[] content)
		{
			if (LogEnable())
			{
				builder.Append(LOG_KEY);

				SetTime(builder);

				builder.Append(name);

				if (content != null)
				{
					for (int i = 0; i < content.Length; i++)
					{
						if (content[i] != null)
						{
							builder.Append(content[i]);
						}
						else
						{
							builder.Append(NULL);
						}
					}
				}
				else
				{
					builder.Append(string.Empty);
				}

				string export = builder.ToString();
				builder.Remove(0, builder.Length);
				Debug.Log(export);
				//if (_logToFile)
				//{
				//	WriteLogLine(export);
				//}
			}
		}

		public void LogFormat(string content, params object[] args)
		{
			if (LogEnable())
			{
				if (content != null)
				{
					builder.Append(LOG_KEY);

					SetTime(builder);

					builder.Append(name);

					builder.Append(content);

					Debug.LogFormat(builder.ToString(), args);

					builder.Remove(0, builder.Length);
				}
			}
		}

		public void LogError(params object[] content)
		{
			if (LogErrorEnable())
			{
				builder.Append("<color=red>");
				builder.Append(LOG_ERROR_KEY);
				SetTime(builder);

				builder.Append(name);
				builder.Append("</color>");

				if (content != null)
				{
					for (int i = 0; i < content.Length; i++)
					{
						if (content[i] != null)
						{
							builder.Append(content[i]);
						}
						else
						{
							builder.Append(NULL);
						}
					}
				}
				else
				{
					builder.Append(string.Empty);
				}

				string export = builder.ToString();
				builder.Remove(0, builder.Length);
				Debug.LogError(export);
				//if (_logErrorToFile)
				//{
				//	WriteLogLine(export);
				//}
			}
		}

		public void LogErrorFormat(string content, params object[] args)
		{
			if (LogErrorEnable())
			{
				if (content != null)
				{
					builder.Append(LOG_KEY);

					SetTime(builder);

					builder.Append(name);

					builder.Append(content);

					Debug.LogErrorFormat(builder.ToString(), args);

					builder.Remove(0, builder.Length);
				}
			}
		}

		public void LogWarning(params object[] content)
		{
			if (LogErrorEnable())
			{
				SetTime(builder);

				builder.Append(name);

				if (content != null)
				{
					for (int i = 0; i < content.Length; i++)
					{
						if (content[i] != null)
						{
							builder.Append(content[i]);
						}
						else
						{
							builder.Append(NULL);
						}
					}
				}
				else
				{
					builder.Append(string.Empty);
				}

				Debug.LogWarning(builder);
				builder.Remove(0, builder.Length);
			}
		}

		public void LogException(Exception exception)
		{
			if (_logErrorToFile || _logToFile)
			{
				SetTime(builder);

				builder.Append(name);

				builder.Append(exception.ToString());

				//WriteLogLine(builder.ToString());
			}

			Debug.LogException(exception);
		}

		private static StreamWriter writer = null;
		private static bool disposed = false;
		private static void WriteLogLine(string condition, string stackTrace, LogType type)
		{
			if (type == LogType.Exception)
			{
				//CrashReporter.ReportError(condition);
				reportAction?.Invoke(condition);
			}

			if (!disposed)
			{
				if ((_logErrorToFile && type == LogType.Error || type == LogType.Exception) || _logToFile)
				{
					if (writer == null)
					{
						DateTime dataTime = DateTime.UtcNow.AddHours(8);
						int[] timeArr = new int[] {
													dataTime.Year,
													dataTime.Month,
													dataTime.Day,
													dataTime.Hour,
													dataTime.Minute,
													dataTime.Second,
													};

						string time = string.Format("[{0:D2}_{1:D2}_{2:D2}_{3:D2}_{4:D2}_{5:D2}]", timeArr[0], timeArr[1], timeArr[2], timeArr[3], timeArr[4], timeArr[5]);
						//string path = ResourceDirectoryNames.ResourceDir + "Log/log_" + time + ".txt";
						string path = "Log/log_" + time + ".txt";
						string directory = Path.GetDirectoryName(path);
						if (!Directory.Exists(directory))
						{
							Directory.CreateDirectory(directory);
						}

						writer = new StreamWriter(path, true);
						writer.AutoFlush = true;
					}
					writer.WriteLine(condition);
					writer.WriteLine(stackTrace);
				}
			}
		}

		public static void Dispose()
		{
			if (writer != null)
			{
				writer.Close();
				disposed = true;
			}
		}
	}
}
