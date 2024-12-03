using ClientData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UIEngine;
using UnityEngine;

public class ConfigModuleBase : ModuleBase
{
	public override void Clear()
	{
	}

	public override void Init()
	{
	}

	protected Dictionary<string, Type> typeDic = new Dictionary<string, Type>();
	protected Dictionary<string, ProtoObject> configDic = new Dictionary<string, ProtoObject>();
	protected int loadCount = 0;
	protected int maxCount = 0;

	protected Action onInitDone;

	protected void LoadConfig(string assetPath)
	{
#if UNITY_EDITOR
		if (File.Exists(assetPath))
		{
			loadAsset.LoadAsset(assetPath, UIAssetType.Resource, OnLoadDone);
		}
		else
		{
			maxCount--;
		}
#else
		loadAsset.LoadAsset(assetPath, UIAssetType.Resource, OnLoadDone);
#endif
	}

	private void OnLoadDone(IUILoadAssetItem assetItem)
	{
		loadCount++;

		if (assetItem != null)
		{
			byte[] oneConfigBytes = (assetItem.content as TextAsset).bytes;
			Type configType = null;
			if (typeDic.TryGetValue(assetItem.path, out configType))
			{
				configDic[assetItem.path] = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length) as ProtoObject;
			}
			assetItem.Recycle();
		}

		if (loadCount >= maxCount)
		{
			onInitDone?.Invoke();
		}
	}

#if UNITY_EDITOR
	public static void ExportConfigModule()
	{
		string strFilePath = Application.dataPath + "/Bundles/Config/";
		string[] files = Directory.GetFiles(strFilePath, "*.bytes", SearchOption.AllDirectories);

		string exportPath = Application.dataPath + "/Scripts/UI/Module/Base/ConfigModule.cs";
		StreamWriter sw = new StreamWriter(exportPath, false, Encoding.UTF8);
		StringBuilder strBuilder = new StringBuilder();

		strBuilder.AppendLine("using ClientData;");
		strBuilder.AppendLine("using App;");
		strBuilder.AppendLine("using System;");

		strBuilder.AppendLine();
		strBuilder.AppendFormat("public class {0} : ConfigModuleBase \r\n", "ConfigModule")
		.AppendLine("{");
		strBuilder.AppendLine("\tprivate static UDebugger debugger = new UDebugger(\"ConfigModule\");\r\n");

		for (int i = 0; i < files.Length; i++)
		{
			string fileName = Path.GetFileNameWithoutExtension(files[i]);
			string path = fileName + "Path";
			string defineName = char.ToLower(fileName[0]) + fileName.Substring(1);
			string privateName = "_" + defineName;
			strBuilder.AppendFormat("\tprivate static string {0} = \"Assets/Bundles/Config/{1}.bytes\";\r\n", path, fileName);
			strBuilder.AppendFormat("\tprivate {0} {1};\r\n", fileName, privateName);
			strBuilder.AppendFormat("\tpublic {0} {1}\r\n", fileName, defineName);
			strBuilder.AppendLine("\t{");
			strBuilder.AppendLine("\t\tget");
			strBuilder.AppendLine("\t\t{");
			strBuilder.AppendFormat("\t\t\tif ({0} == null)\r\n", privateName);
			strBuilder.AppendLine("\t\t\t{");
			strBuilder.AppendFormat("\t\t\t\t{0} = configDic[{1}] as {2};\r\n", privateName, path, fileName);
			strBuilder.AppendLine("\t\t\t}");
			strBuilder.AppendFormat("\t\t\treturn {0};\r\n", privateName);
			strBuilder.AppendLine("\t\t}");
			strBuilder.AppendLine("\t}");
		}

		strBuilder.AppendLine();
		strBuilder.AppendLine("\tpublic void InitAllConfig(Action onInitDone)");
		strBuilder.AppendLine("\t{");
		strBuilder.AppendFormat("\t\tmaxCount = {0};\r\n", files.Length);
		strBuilder.AppendLine("\t\tthis.onInitDone = onInitDone;\r\n");

		for (int i = 0; i < files.Length; i++)
		{
			string fileName = Path.GetFileNameWithoutExtension(files[i]);
			strBuilder.AppendFormat("\t\ttypeDic[{0}Path] = typeof({0});\r\n", fileName);
		}
		strBuilder.AppendLine();

		for (int i = 0; i < files.Length; i++)
		{
			string fileName = Path.GetFileNameWithoutExtension(files[i]);
			string path = fileName + "Path";
			strBuilder.AppendFormat("\t\tLoadConfig({0});\r\n", path);
		}

		strBuilder.AppendLine("\t}");

		strBuilder.AppendLine("}\r\n");

		sw.Write(strBuilder);
		sw.Flush();
		sw.Close();
		Debug.Log("<color=#00FF00>成功生成 - ConfigModule.cs</color>");
	}
#endif
}
