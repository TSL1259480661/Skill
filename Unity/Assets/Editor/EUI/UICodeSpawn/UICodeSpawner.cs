using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Component = UnityEngine.Component;
using Image = UnityEngine.UI.Image;

public partial class UICodeSpawner
{
	[System.Serializable]
	public sealed class UINodeInfo
	{
		public string name;
		public List<Component> component;
		public bool suffix;//是否添加类名后缀


		public UINodeInfo(string name, List<Component> component, bool suffix = true)
		{
			this.name = name;
			this.component = component;
			this.suffix = suffix;
		}
	}

	static public void SpawnEUICode(GameObject gameObject)
	{
		if (null == gameObject)
		{
			Debug.LogError("UICode Select GameObject is null!");
			return;
		}

		try
		{
			string uiName = gameObject.name;
			if (uiName.StartsWith(UIPanelPrefix))
			{
				Debug.LogWarning($"----------开始生成Dlg{uiName} 相关代码 ----------");
				SpawnDlgCode(gameObject);
				Debug.LogWarning($"生成Dlg{uiName} 完毕!!!");
				return;
			}
			else if (uiName.StartsWith(CommonUIPrefix))
			{
				Debug.LogWarning($"-------- 开始生成子UI: {uiName} 相关代码 -------------");
				SpawnSubUICode(gameObject);
				Debug.LogWarning($"生成子UI: {uiName} 完毕!!!");
				return;
			}
			else if (uiName.StartsWith(UIItemPrefix))
			{
				Debug.LogWarning($"-------- 开始生成滚动列表项: {uiName} 相关代码 -------------");
				SpawnLoopItemCode(gameObject);
				Debug.LogWarning($" 开始生成滚动列表项: {uiName} 完毕！！！");
				return;
			}
			Debug.LogError($"选择的预设物不属于 Dlg, 子UI，滚动列表项，请检查 {uiName}！！！！！！");
		}
		finally
		{
			RefreshUIAssetPaths(false);
			Debug.LogWarning($" 更新资源路径 DONE ");
			Path2WidgetCachedDict?.Clear();
			Path2WidgetCachedDict = null;
		}
	}


	static public void SpawnDlgCode(GameObject gameObject)
	{
		Path2WidgetCachedDict?.Clear();
		Path2WidgetCachedDict = new Dictionary<string, UINodeInfo>();

		FindAllWidgets(gameObject.transform, "");

		SpawnCodeForDlgComponentBehaviour(gameObject);
		SpawnViewCodeForDlgViewBase(gameObject);

		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}

	static void SpawnCodeForDlgComponentBehaviour(GameObject gameObject)
	{
		if (null == gameObject)
		{
			return;
		}
		string strDlgName = gameObject.name;
		string strDlgComponentName = gameObject.name + "ViewBase";


		string strFilePath = Application.dataPath + "/Scripts/UI/View/" + strDlgName;
		if (!System.IO.Directory.Exists(strFilePath))
		{
			System.IO.Directory.CreateDirectory(strFilePath);
		}
		strFilePath = Application.dataPath + "/Scripts/UI/View/" + strDlgName + "/" + strDlgComponentName + ".cs";
		StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
		StringBuilder strBuilder = new StringBuilder();
		strBuilder.AppendLine()
			.AppendLine("using UnityEngine;");
		strBuilder.AppendLine("using UnityEngine.UI;");
		strBuilder.AppendLine("using UIEngine;\r\n");
		//strBuilder.AppendLine("namespace UIEngine");
		//strBuilder.AppendLine("{");
		//strBuilder.AppendLine("\t[ComponentOf(typeof(UIViewBase))]");
		//strBuilder.AppendLine("\t[EnableMethod]");
		strBuilder.AppendFormat("public abstract class {0} : UIViewBase \r\n", strDlgComponentName)
			.AppendLine("{");

		CreateWidgetBindCode(ref strBuilder, gameObject.transform, false);

		CreateDestroyWidgetCode(ref strBuilder);

		CreateDeclareCode(ref strBuilder);
		strBuilder.AppendLine("}");
		//strBuilder.AppendLine("}");

		sw.Write(strBuilder);
		sw.Flush();
		sw.Close();
	}

	static void SpawnViewCodeForDlgViewBase(GameObject gameObject)
	{
		if (null == gameObject)
		{
			return;
		}
		string strDlgName = gameObject.name;
		string strDlgComponentNameBase = gameObject.name + "ViewBase";
		string strDlgComponentName = gameObject.name + "View";


		string strFilePath = Application.dataPath + "/Scripts/UI/View/" + strDlgName;
		if (!System.IO.Directory.Exists(strFilePath))
		{
			System.IO.Directory.CreateDirectory(strFilePath);
		}
		strFilePath = Application.dataPath + "/Scripts/UI/View/" + strDlgName + "/" + strDlgComponentName + ".cs";

		if (!File.Exists(strFilePath))
		{
			StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
			StringBuilder strBuilder = new StringBuilder();
			strBuilder.AppendLine("using UnityEngine;");
			strBuilder.AppendLine("using UnityEngine.UI;");
			strBuilder.AppendLine("using UIEngine;");
			strBuilder.AppendLine("using App;\r\n");
			//strBuilder.AppendLine("namespace UIEngine");
			//strBuilder.AppendLine("{");
			//strBuilder.AppendLine("\t[ComponentOf(typeof(UIViewBase))]");
			//strBuilder.AppendLine("\t[EnableMethod]");
			strBuilder.AppendFormat("public class {0} : {1} \r\n", strDlgComponentName, strDlgComponentNameBase)
				.AppendLine("{");

			strBuilder.AppendFormat("\tprivate static UDebugger debugger = new UDebugger(\"{0}\");\r\n\r\n", strDlgComponentName);
			strBuilder.AppendLine("\tpublic override void BeforeInit(object[] paramList)");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}\r\n");
			strBuilder.AppendLine("\tpublic override void OnInit(object[] paramList)");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}\r\n");
			strBuilder.AppendLine("\tpublic override void BeforeShow(object[] paramList)");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}\r\n");
			strBuilder.AppendLine("\tpublic override void OnShow(object[] paramList)");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}\r\n");
			strBuilder.AppendLine("\tpublic override void OnHide(object[] paramList)");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}\r\n");
			strBuilder.AppendLine("\tpublic override void OnRecycle()");
			strBuilder.AppendLine("\t{\r\n");
			strBuilder.AppendLine("\t}");

			strBuilder.Append("}");
			//strBuilder.AppendLine("}");

			sw.Write(strBuilder);
			sw.Flush();
			sw.Close();
		}
	}

	public static void RefreshUIAssetPaths(bool refreshDataBase = true)
	{
		string strFilePath = Application.dataPath + "/UI/";
		string[] files = Directory.GetFiles(strFilePath, "*.prefab", SearchOption.AllDirectories);

		string exportPath = Application.dataPath + "/Scripts/Enums/UIAssetPaths.cs";
		StreamWriter sw = new StreamWriter(exportPath, false, Encoding.UTF8);
		StringBuilder strBuilder = new StringBuilder();

		strBuilder.AppendFormat("public static class {0} \r\n", "UIAssetPaths")
		.AppendLine("{");

		for (int i = 0; i < files.Length; i++)
		{
			string file = files[i];
			string path = file.Remove(0, file.IndexOf("/Assets/") + 1).Replace("\\", "/");
			strBuilder.AppendFormat("\tpublic static string {0} = \"{1}\";\r\n", Path.GetFileNameWithoutExtension(file), path);
		}

		strBuilder.AppendLine("}");

		sw.Write(strBuilder);
		sw.Flush();
		sw.Close();

		if (refreshDataBase) 
		{
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}
	}

	public static void CreateDestroyWidgetCode(ref StringBuilder strBuilder, bool isScrollItem = false)
	{
		strBuilder.AppendFormat("\toverride public void ClearComponentFields()");
		strBuilder.AppendLine("\n\t{");
		CreateDlgWidgetDisposeCode(ref strBuilder);
		if (isScrollItem)
		{
			//strBuilder.AppendLine("\t\t\tthis.DataId = 0;");
		}
		strBuilder.AppendLine("\t}\n");
	}


	public static void CreateDlgWidgetDisposeCode(ref StringBuilder strBuilder, bool isSelf = false)
	{
		string pointStr = isSelf ? "self" : "this";
		foreach (var pair in Path2WidgetCachedDict)
		{
			foreach (var info in pair.Value.component)
			{
				Component widget = info;
				string strClassType = widget.GetType().ToString();

				if (pair.Key.StartsWith(CommonUIPrefix))
				{
					strBuilder.AppendFormat("\t\t{0}.m_{1}?.ClearComponentFields();\r\n", pointStr, pair.Key.ToLower());
					strBuilder.AppendFormat("\t\t{0}.m_{1}ts = null;\r\n", pointStr, pair.Key.ToLower());
					//strBuilder.AppendFormat("\t\t{0}.m_{1} = null;\r\n", pointStr, pair.Key.ToLower());
					continue;
				}

				string widgetName = widget.name;
				string postfix = NamePostfix(strClassType);
				if (!widget.name.Contains(postfix) && pair.Value.suffix)
				{
					widgetName = widget.name + postfix;
				}
				strBuilder.AppendFormat("\t\t{0}.m_{1} = null;\r\n", pointStr, widgetName);
			}

		}


	}

	public static void CreateWidgetBindCode(ref StringBuilder strBuilder, Transform transRoot, bool ESParent)
	{
		foreach (var pair in Path2WidgetCachedDict)
		{
			foreach (var info in pair.Value.component)
			{
				Component widget = info;
				string strPath = GetWidgetPath(widget.transform, transRoot);
				string strClassType = widget.GetType().ToString();
				string strInterfaceType = strClassType;

				if (pair.Key.StartsWith(CommonUIPrefix))
				{
					var subUIClassPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(widget);
					if (subUIClassPrefab == null)
					{
						Debug.LogError($"公共UI找不到所属的Prefab! {pair.Key}");
						return;
					}
					GetSubUIBaseWindowCode(ref strBuilder, pair.Key, strPath, subUIClassPrefab.name, ESParent);
					continue;
				}
				string widgetName = widget.name;
				string postfix = NamePostfix(strClassType);
				if (!widget.name.Contains(postfix) && pair.Value.suffix)
				{
					widgetName = widget.name + postfix;
				}

				string authority = "protected";
				if (widget.TryGetComponent<CompentPublicLabel>(out CompentPublicLabel label))
				{
					authority = "public";
				}

				strBuilder.AppendFormat("\t{0} {1} {2}\r\n", authority, strInterfaceType, widgetName);
				strBuilder.AppendLine("\t{");
				strBuilder.AppendLine("\t\tget");
				strBuilder.AppendLine("\t\t{");

				strBuilder.AppendLine("\t\t\tif (this.transform == null)");
				strBuilder.AppendLine("\t\t\t{");
				//strBuilder.AppendLine("     				Log.Error(\"transform is null.\");");
				strBuilder.AppendLine("\t\t\t\treturn null;");
				strBuilder.AppendLine("\t\t\t}");

				strBuilder.AppendFormat("\t\t\tif (this.m_{0} == null)\n", widgetName);
				strBuilder.AppendLine("\t\t\t{");
				strBuilder.AppendFormat("\t\t\t\tthis.m_{0} = UIFindHelper.FindDeepChild<{2}>(this.gameObject, \"{1}\");\r\n", widgetName, strPath, strInterfaceType);
				if (widget is ISound)
				{
					strBuilder.AppendFormat("\t\t\t\tthis.m_{0}.InitSound({1});\r\n", widgetName, "audio");
				}
				if (widget is GImage)
				{
					strBuilder.AppendFormat("\t\t\t\tthis.m_{0}.InitView({1});\r\n", widgetName, "this");
				}
				strBuilder.AppendLine("\t\t\t}");
				strBuilder.AppendFormat("\t\t\treturn this.m_{0};\n", widgetName);

				strBuilder.AppendLine("\t\t}");
				strBuilder.AppendLine("\t}\n");
			}
		}
	}

	public static void CreateDeclareCode(ref StringBuilder strBuilder)
	{
		foreach (var pair in Path2WidgetCachedDict)
		{
			foreach (var info in pair.Value.component)
			{
				Component widget = info;
				string strClassType = widget.GetType().ToString();

				if (pair.Key.StartsWith(CommonUIPrefix) && !pair.Key.StartsWith(UISpritePrefix))
				{
					var subUIClassPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(widget);
					if (subUIClassPrefab == null)
					{
						Debug.LogError($"公共UI找不到所属的Prefab! {pair.Key}");
						return;
					}
					string subUIClassType = subUIClassPrefab.name + "View";
					strBuilder.AppendFormat("\tprivate {0} m_{1} = new {0}();\r\n", subUIClassType, pair.Key.ToLower());
					strBuilder.AppendFormat("\tprivate Transform m_{1}ts = null;\r\n", subUIClassType, pair.Key.ToLower());
					continue;
				}
				string widgetName = widget.name;
				string postfix = NamePostfix(strClassType);
				if (!widget.name.Contains(postfix) && pair.Value.suffix)
				{
					widgetName = widget.name + postfix;
				}
				strBuilder.AppendFormat("\tprivate {0} m_{1} = null;\r\n", strClassType, widgetName);
			}

		}
	}
	static string NamePostfix(string strClassType)
	{
		string postfix = strClassType.Split('.').ToList().Last();
		switch (postfix)
		{
			case "GText":
				postfix = "Text";
				break;
			case "GImage":
				postfix = "Image";
				break;
			case "SlicedFilledImage":
				postfix = "Image";
				break;
			case "GButton":
				postfix = "Button";
				break;
			case "GToggle":
				postfix = "Toggle";
				break;
			case "ExtendButtonGroup":
				postfix = "ButtonGroup";
				break;
			case "ExtendDropdown":
				postfix = "Dropdown";
				break;
		}
		return postfix;
	}

	/// <summary>
	/// 是否为特殊标识组件且标识为需要自动生成
	/// </summary>
	static bool IsMarkBehaviourAndAutoCreate(Transform child, out MarkBehaviour behaviour)
	{
		behaviour = child.GetComponent<MarkBehaviour>();
		if (behaviour != null)
		{
			return behaviour.bAutoCreate;
		}
		return false;
	}

	public static void FindAllWidgets(Transform trans, string strPath)
	{
		if (null == trans)
		{
			return;
		}
		List<Transform> transforms = new List<Transform>();
		GetNodes(trans, ref transforms);
		for (int i = 0; i < transforms.Count; ++i)
		{
			var transf = transforms[i];
			var name = transf.name;
			if (IsMarkBehaviourAndAutoCreate(transf, out MarkBehaviour behvaiour))
			{
				if (Path2WidgetCachedDict.ContainsKey(name))
				{
					Path2WidgetCachedDict[name].component.Add(behvaiour);
				}
				else
				{
					List<Component> componentsList = new List<Component> { behvaiour };
					Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, true));
				}
			}
			if (name.StartsWith(CommonUIPrefix) || name.StartsWith("EG"))
			{
				List<Component> componentsList = new List<Component> { transf.GetComponent<RectTransform>() };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, true));
			}
			else if (name.StartsWith(UIWidgetPrefix))
			{
				foreach (var uiComponent in WidgetInterfaceList)
				{
					Component component = transf.GetComponent(uiComponent);
					if (null == component)
					{
						continue;
					}

					if (Path2WidgetCachedDict.ContainsKey(transf.name))
					{
						Path2WidgetCachedDict[transf.name].component.Add(component);
						continue;
					}

					List<Component> componentsList = new List<Component> { component };
					Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, true));
				}
			}
			else if (name.StartsWith("Text_"))
			{
				Component c = transf.GetComponent<Text>();
				c = c ? c : transf.GetComponent<TextMeshProUGUI>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"Text\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("TMP_"))
			{
				var c = transf.GetComponent<TextMeshProUGUI>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"TextMeshProUGUI\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("Image_"))
			{
				var c = transf.GetComponent<Image>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"Image\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("RawImage_"))
			{
				var c = transf.GetComponent<RawImage>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"RawImage\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("Button_"))
			{
				var c = transf.GetComponent<Button>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"Button\".", transf.gameObject);
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("Toggle_"))
			{
				var c = transf.GetComponent<Toggle>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"Toggle\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("InputField_"))
			{
				var c = transf.GetComponent<InputField>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"InputField\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("ScrollRect_"))
			{
				var c = transf.GetComponent<ScrollRect>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"ScrollRect\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("LoopScroll_"))
			{
				var c = transf.GetComponent<LoopScrollView>();
				if (c == null)
					Debug.LogError(transf.name + " has not the component of \"LoopScrollView\".");
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("Mask_"))
			{
				Component c = transf.GetComponent<Mask>();
				c = c ? c : transf.GetComponent<RectMask2D>();
				//c = c ? c : transf.GetComponent<SoftMasking.SoftMask>();
				List<Component> componentsList = new List<Component> { c };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
			else if (name.StartsWith("Trans_") || name.StartsWith("Transform_"))
			{
				List<Component> componentsList = new List<Component> { transf.GetComponent<RectTransform>() };
				Path2WidgetCachedDict.Add(transf.name, new UINodeInfo(transf.name, componentsList, false));
			}
		}
	}

	private static void GetNodes(Transform trans, ref List<Transform> list)
	{
		for (int i = 0; i < trans.childCount; i++)
		{
			var child = trans.GetChild(i);
			list.Add(child);
			string name = child.name;
			if (name.StartsWith(CommonUIPrefix) || name.StartsWith("EF"))//子UI(ES_)和特效(EF_)不需要添加
			{
				Debug.Log($"遇到子UI：{child.name},不生成子UI项代码");
				continue;
			}
			GetNodes(child, ref list);
		}
	}

	static string GetWidgetPath(Transform obj, Transform root)
	{
		string path = obj.name;

		while (obj.parent != null && obj.parent != root)
		{
			obj = obj.transform.parent;
			path = obj.name + "/" + path;
		}
		return path;
	}


	static void GetSubUIBaseWindowCode(ref StringBuilder strBuilder, string widget, string strPath, string subUIClassType, bool ESContainES)
	{
		subUIClassType = subUIClassType + "View";

		strBuilder.AppendFormat("\tprotected {0} {1}\r\n", subUIClassType, widget);
		strBuilder.AppendLine("\t{");
		strBuilder.AppendLine("\t\tget");
		strBuilder.AppendLine("\t\t{");

		strBuilder.AppendLine("\t\t\tif (this.transform == null)");
		strBuilder.AppendLine("\t\t\t{");
		strBuilder.AppendLine("\t\t\t\treturn null;");
		strBuilder.AppendLine("\t\t\t}");

		widget = widget.ToLower();

		strBuilder.AppendFormat("\t\t\tif (this.m_{0}ts == null)\n", widget);
		strBuilder.AppendLine("\t\t\t{");
		strBuilder.AppendFormat("\t\t\t\tthis.m_{0}ts = UIFindHelper.FindDeepChild<Transform>(this.gameObject,\"{1}\");\r\n", widget, strPath);
		if (ESContainES)
		{
			strBuilder.AppendFormat("\t\t\t\tthis.m_{0}.Bind(this.m_{0}ts, this.viewBase);\r\n", widget);
		}
		else
		{
			strBuilder.AppendFormat("\t\t\t\tthis.m_{0}.Bind(this.m_{0}ts, this);\r\n", widget);
		}
		strBuilder.AppendLine("\t\t\t}");

		strBuilder.AppendFormat("\t\t\treturn this.m_{0};\n", widget);
		strBuilder.AppendLine("\t\t}");

		strBuilder.AppendLine("\t}\n");
	}


	static UICodeSpawner()
	{
		WidgetInterfaceList = new List<string>();
		WidgetInterfaceList.Add("Button");
		WidgetInterfaceList.Add("Text");
		WidgetInterfaceList.Add("TMPro.TextMeshProUGUI");
		WidgetInterfaceList.Add("TMPro.TMP_InputField");
		WidgetInterfaceList.Add("Input");
		WidgetInterfaceList.Add("InputField");
		WidgetInterfaceList.Add("Scrollbar");
		WidgetInterfaceList.Add("ToggleGroup");
		WidgetInterfaceList.Add("Toggle");
		WidgetInterfaceList.Add("Dropdown");
		WidgetInterfaceList.Add("Slider");
		WidgetInterfaceList.Add("Image");
		WidgetInterfaceList.Add("RawImage");
		WidgetInterfaceList.Add("Canvas");
		WidgetInterfaceList.Add("CanvasGroup");
		WidgetInterfaceList.Add("UIWarpContent");
		WidgetInterfaceList.Add("LoopScrollView");
		WidgetInterfaceList.Add("UnityEngine.EventSystems.EventTrigger");
		WidgetInterfaceList.Add("SlicedFilledImage");
	}

	private static Dictionary<string, UINodeInfo> Path2WidgetCachedDict = null;
	private static List<string> WidgetInterfaceList = null;
	private const string CommonUIPrefix = "ES";
	private const string UISpritePrefix = "ESprite";
	private const string UIPanelPrefix = "Dlg";
	private const string UIWidgetPrefix = "E";
	private const string UIItemPrefix = "Item";
}

