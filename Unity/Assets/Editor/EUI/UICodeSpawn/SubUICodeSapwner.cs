using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;


public partial class UICodeSpawner
{
	static public void SpawnSubUICode(GameObject gameObject)
	{
		Path2WidgetCachedDict?.Clear();
		Path2WidgetCachedDict = new Dictionary<string, UINodeInfo>();
		FindAllWidgets(gameObject.transform, "");
		SpawnCodeForSubUIBehaviour(gameObject);
		SpawnCodeForSubUIBehaviour2(gameObject);
		AssetDatabase.Refresh();
	}

	static void SpawnCodeForSubUIBehaviour(GameObject objPanel)
	{
		if (null == objPanel)
		{
			return;
		}
		string strDlgName = objPanel.name + "View";
		string strDlgNameBase = objPanel.name + "ViewBase";

		string strFilePath = Application.dataPath + "/Scripts/UI/View/" + objPanel.name;
		if (!System.IO.Directory.Exists(strFilePath))
		{
			System.IO.Directory.CreateDirectory(strFilePath);
		}
		strFilePath = Application.dataPath + "/Scripts/UI/View/" + objPanel.name + "/" + strDlgNameBase + ".cs";

		StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

		StringBuilder strBuilder = new StringBuilder();
		strBuilder.AppendLine()
			.AppendLine("using UIEngine;");
		strBuilder.AppendLine("using UnityEngine;");
		strBuilder.AppendLine("using UnityEngine.UI;");
		strBuilder.AppendFormat("public  class {0} : UIItemBase \r\n", strDlgNameBase)
			.AppendLine("{");

		CreateDeclareCode(ref strBuilder);

		strBuilder.AppendLine("");
		strBuilder.AppendFormat("\tpublic {0} Bind(Transform itemTransform, UIViewBase view)\r\n", strDlgNameBase);
		strBuilder.AppendLine("\t{");
		strBuilder.AppendLine("\t\tInitBind(itemTransform, view);");
		strBuilder.AppendLine("\t\treturn this;");
		strBuilder.AppendLine("\t}");
		strBuilder.AppendLine("");

		CreateWidgetBindCode(ref strBuilder, objPanel.transform, true);
		CreateDestroyWidgetCode(ref strBuilder);
		strBuilder.AppendLine("}");

		sw.Write(strBuilder);
		sw.Flush();
		sw.Close();
	}

	static void SpawnCodeForSubUIBehaviour2(GameObject gameObject)
	{
		if (null == gameObject)
		{
			return;
		}

		string strDlgName = gameObject.name + "View";
		string strDlgNameBase = gameObject.name + "ViewBase";
		string strFilePath = Application.dataPath + "/Scripts/UI/View/" + gameObject.name;
		if (!System.IO.Directory.Exists(strFilePath))
		{
			System.IO.Directory.CreateDirectory(strFilePath);
		}
		strFilePath = strFilePath + "/" + strDlgName + ".cs";

		if (!File.Exists(strFilePath))
		{
			StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

			StringBuilder strBuilder = new StringBuilder();
			strBuilder.AppendLine("using UnityEngine;");
			strBuilder.AppendLine("using UnityEngine.UI;");
			strBuilder.AppendLine("using App;");
			strBuilder.AppendLine("using System;");
			strBuilder.AppendLine();

			strBuilder.AppendFormat("public class {0} : {1}\r\n", strDlgName, strDlgNameBase)
				.AppendLine("{");

			strBuilder.AppendFormat("\tprivate static UDebugger debugger = new UDebugger(\"{0}\");\r\n\r\n", strDlgName);

			strBuilder.AppendLine("}");

			sw.Write(strBuilder);
			sw.Flush();
			sw.Close();
		}
	}
}
