using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ET
{
	public static class ShellHelper
	{

		public static bool Run(string cmd, string workDirectory, List<string> environmentVars = null)
		{
			bool succeed = true;
			System.Diagnostics.Process process = new();
			try
			{
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
                string app = "bash";
                string splitChar = ":";
                string arguments = "-c";
#elif UNITY_EDITOR_WIN
				string app = "cmd.exe";
				string splitChar = ";";
				string arguments = "/c chcp 65001 &"; // chcp 65001 解决unity控制台 中文乱码
#endif
				ProcessStartInfo start = new ProcessStartInfo(app, $"{arguments} {cmd}");

				if (environmentVars != null)
				{
					foreach (string var in environmentVars)
					{
						start.EnvironmentVariables["PATH"] += (splitChar + var);
					}
				}

				process.StartInfo = start;
				start.WorkingDirectory = workDirectory;
				start.ErrorDialog = true;
				start.CreateNoWindow = true;
				start.UseShellExecute = false;

				if (start.UseShellExecute)
				{
					start.RedirectStandardOutput = false;
					start.RedirectStandardError = false;
					start.RedirectStandardInput = false;
				}
				else
				{
					//不使用系统外壳程序启动,重定向输出的话必须设为true
					start.RedirectStandardOutput = true;
					start.RedirectStandardError = true;
					start.RedirectStandardInput = true;
					start.StandardOutputEncoding = System.Text.Encoding.UTF8;
					start.StandardErrorEncoding = System.Text.Encoding.UTF8;
				}

				bool endOutput = false;
				bool endError = false;

				process.OutputDataReceived += (sender, args) =>
				{
					if (args.Data != null)
					{
						if (args.Data.StartsWith("Error"))
						{
							UnityEngine.Debug.LogError(args.Data);
							succeed = false;
						}
						else
						{
							UnityEngine.Debug.Log(args.Data);
						}
					}
					else
					{
						endOutput = true;
					}
				};

				process.ErrorDataReceived += (sender, args) =>
				{
					if (args.Data != null)
					{
						UnityEngine.Debug.LogError(args.Data);
						succeed = false;
					}
					else
					{
						endError = true;
					}
				};

				process.Start();
				if (start.RedirectStandardOutput)
					process.BeginOutputReadLine();
				if (start.RedirectStandardError)
					process.BeginErrorReadLine();

				//process.WaitForExit();
				while (!endOutput || !endError)
				{
				}

				if (start.RedirectStandardOutput)
					process.CancelOutputRead();
				if (start.RedirectStandardError)
					process.CancelErrorRead();
			}
			catch (Exception e)
			{
				succeed = false;
				UnityEngine.Debug.LogException(e);
			}
			finally
			{
				process.Close();
			}
			return succeed;
		}
	}
}
