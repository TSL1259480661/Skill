
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public partial class UICodeSpawner
{
	static public void SpawnLoopItemCode(GameObject gameObject)
	{
		Path2WidgetCachedDict?.Clear();
		Path2WidgetCachedDict = new Dictionary<string, UINodeInfo>();
		FindAllWidgets(gameObject.transform, "");
		SpawnCodeForScrollLoopItemBehaviour(gameObject);
		SpawnCodeForScrollLoopBaseItemBehaviour(gameObject);
		AssetDatabase.Refresh();
	}

	static void SpawnCodeForScrollLoopItemBehaviour(GameObject gameObject)
	{
		if (null == gameObject)
		{
			return;
		}
		string strDlgName = gameObject.name + "View";
		string strDlgNameBase = gameObject.name + "ViewBase";

		string strFilePath = Application.dataPath + "/Scripts/UI/View/Items/" + gameObject.name;

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

			strBuilder.AppendFormat("public enum {0}_Operation\r\n", strDlgName);
			strBuilder.AppendLine("{");
			strBuilder.AppendLine("\t");
			strBuilder.AppendLine("}");
			strBuilder.AppendLine();

			strBuilder.AppendFormat("public class {0} : {1}, IObjectPoolItem\r\n", strDlgName, strDlgNameBase)
				.AppendLine("{");

			strBuilder.AppendFormat("\tprivate static UDebugger debugger = new UDebugger(\"{0}\");\r\n\r\n", strDlgName);

			strBuilder.AppendFormat("\tprotected override void Init(object data, int dataIndex, Action<object, int, {0}_Operation> callback)\r\n", strDlgName);
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}");
			strBuilder.AppendLine();

			strBuilder.AppendLine("\tpublic void OnReuse()");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}");
			strBuilder.AppendLine();

			strBuilder.AppendLine("\tpublic void OnRecycle()");
			strBuilder.AppendLine("\t{");
			strBuilder.AppendLine("\t\ttransform = null;");
			strBuilder.AppendLine("\t\tgameObject = null;");
			strBuilder.AppendLine("\t\tClearLoadAssetItems();");
			strBuilder.AppendLine("\t\tClearComponentFields();");
			strBuilder.AppendLine("\t}");
			strBuilder.AppendLine();

			strBuilder.AppendFormat("\tprivate static UObjectPool<{0}> itemPool = new UObjectPool<{0}>();\r\n", strDlgName);
			strBuilder.AppendLine();
			strBuilder.AppendFormat("\tpublic static {0} Get()\r\n", strDlgName);
			strBuilder.AppendLine("\t{");
			strBuilder.AppendLine("\t\treturn itemPool.Get();");
			strBuilder.AppendLine("\t}");
			strBuilder.AppendLine();
			strBuilder.AppendFormat("\tpublic void Recycle()\r\n", strDlgName);
			strBuilder.AppendLine("\t{");
			strBuilder.AppendLine("\t\titemPool.Recycle(this);");
			strBuilder.AppendLine("\t}");
			strBuilder.AppendLine();

			strBuilder.AppendLine("}");

			sw.Write(strBuilder);
			sw.Flush();
			sw.Close();
		}
	}

	static void SpawnCodeForScrollLoopBaseItemBehaviour(GameObject gameObject)
	{
		if (null == gameObject)
		{
			return;
		}
		string strDlgNameBase = gameObject.name + "ViewBase";
		string strDlgName = gameObject.name + "View";

		string strFilePath = Application.dataPath + "/Scripts/UI/View/Items/" + gameObject.name;

		if (!System.IO.Directory.Exists(strFilePath))
		{
			System.IO.Directory.CreateDirectory(strFilePath);
		}
		strFilePath = strFilePath + "/" + strDlgNameBase + ".cs";
		StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

		StringBuilder strBuilder = new StringBuilder();
		strBuilder.AppendLine("using UnityEngine;");
		strBuilder.AppendLine("using UnityEngine.UI;");
		strBuilder.AppendLine("using System;");
		strBuilder.AppendLine("using UIEngine;");
		strBuilder.AppendLine("using App;");
		strBuilder.AppendLine("using System.Collections.Generic;");

		strBuilder.AppendLine();
		strBuilder.AppendFormat("public abstract class {0} : UIItemBase\r\n", strDlgNameBase)
			.AppendLine("{");

		strBuilder.AppendFormat("\tprotected abstract void Init(object data, int dataIndex, Action<object, int, {0}_Operation> callback);\r\n", strDlgName);
		strBuilder.AppendLine();

		strBuilder.AppendFormat("\tpublic {0} Bind(Transform itemTransform, UIViewBase view, object data = null, int dataIndex = 0, Action<object, int, {1}_Operation> callback = null)\r\n", strDlgNameBase, strDlgName);
		strBuilder.AppendLine("\t{");
		strBuilder.AppendLine("\t\tInitBind(itemTransform, view);");
		strBuilder.AppendLine();
		strBuilder.AppendLine("\t\tthis.Init(data, dataIndex, callback);");
		strBuilder.AppendLine("\t\treturn this;");
		strBuilder.AppendLine("\t}");
		strBuilder.AppendLine();

		CreateWidgetBindCode(ref strBuilder, gameObject.transform, false);
		CreateDestroyWidgetCode(ref strBuilder, true);
		CreateDeclareCode(ref strBuilder);

		strBuilder.AppendLine("}");

		sw.Write(strBuilder);
		sw.Flush();
		sw.Close();

		strBuilder.Clear();
	}
}
