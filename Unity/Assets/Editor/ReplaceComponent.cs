using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 替换组件
/// </summary>
public class ReplaceComponent
{

	[MenuItem("CONTEXT/Text/Convert To TMPText", true)]
	static bool CanConvertTMPText(MenuCommand command)
	{
		return command.context is Text;
	}
	[MenuItem("CONTEXT/Text/Convert To TMPText", false)]
	static void ConvertTMPText(MenuCommand command)
	{
		var btn = command.context as Text;
		var gameObject = btn.gameObject;
		var text = btn.text;
		var fontSize = btn.fontSize;
		var resizeTextForBestFit = btn.resizeTextForBestFit;
		var resizeTextMinSize = btn.resizeTextMinSize;
		var resizeTextMaxSize = btn.resizeTextMaxSize;
		var lineSpacing = btn.lineSpacing;
		var supportRichText = btn.supportRichText;
		var alignment = btn.alignment;
		var alignByGeometry = btn.alignByGeometry;
		var horizontalOverflow = btn.horizontalOverflow;
		var verticalOverflow = btn.verticalOverflow;

		var color = btn.color;
		var raycastTarget = btn.raycastTarget;
		Object.DestroyImmediate(btn, true);

		var tmp = gameObject.AddComponent<TextMeshProUGUI>();
		tmp.text = text;
		tmp.color = color;
		tmp.raycastTarget = raycastTarget;
		tmp.fontSize = fontSize;
		tmp.fontSizeMin = resizeTextMinSize;
		tmp.fontSizeMax = resizeTextMaxSize;
		tmp.autoSizeTextContainer = resizeTextForBestFit;
		//tmp.verticalAlignment = alignment;
		EditorUtility.SetDirty(gameObject);
	}

	[MenuItem("CONTEXT/Image/Convert To GImage", true)]
	static bool CanConvertGImage(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<GImage>(command.context);
	}
	[MenuItem("CONTEXT/Image/Convert To GImage", false)]
	static void ConvertGImage(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<GImage>(command.context);
	}

	[MenuItem("CONTEXT/Image/Convert To Imgae", true)]
	static bool CanConvertImage(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<Image>(command.context);
	}
	[MenuItem("CONTEXT/Image/Convert To Imgae", false)]
	static void ConvertImage(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<Image>(command.context);
	}

	[MenuItem("CONTEXT/Text/Convert To GText", true)]
	static bool CanConvertGText(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<GText>(command.context);
	}
	[MenuItem("CONTEXT/Text/Convert To GText", false)]
	static void ConvertGText(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<GText>(command.context);
	}

	[MenuItem("CONTEXT/Text/Convert To Text", true)]
	static bool CanConvertText(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<Text>(command.context);
	}
	[MenuItem("CONTEXT/Text/Convert To Text", false)]
	static void ConvertText(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<Text>(command.context);
	}


	[MenuItem("CONTEXT/Button/Convert To GButton", true)]
	static bool CanConvertGButton(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<GButton>(command.context);
	}
	[MenuItem("CONTEXT/Button/Convert To GButton", false)]
	static void ConvertGButton(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<GButton>(command.context);
	}

	[MenuItem("CONTEXT/Button/Convert To Button", true)]
	static bool CanConvertButton(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<Button>(command.context);
	}
	[MenuItem("CONTEXT/Button/Convert To Button", false)]
	static void ConvertButton(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<Button>(command.context);
	}

	[MenuItem("CONTEXT/Button/Convert To ExtendButton", true)]
	static bool CanConvertToExtendButton(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<ExtendButton>(command.context);
	}

	[MenuItem("CONTEXT/Button/Convert To ExtendButton", false)]
	static void ConvertToExtendButton(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<ExtendButton>(command.context);
	}

	[MenuItem("CONTEXT/Toggle/Convert To GToggle", true)]
	static bool CanConvertGToggle(MenuCommand command)
	{
		Debug.Log(ReplaceComponentReferenc.CanConvertTo<GToggle>(command.context));
		return ReplaceComponentReferenc.CanConvertTo<GToggle>(command.context);
	}
	[MenuItem("CONTEXT/Toggle/Convert To GToggle", false)]
	static void ConvertGToggle(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<GToggle>(command.context);
	}

	[MenuItem("CONTEXT/Toggle/Convert To Toggle", true)]
	static bool CanConvertToggle(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<Toggle>(command.context);
	}
	[MenuItem("CONTEXT/Toggle/Convert To Toggle", false)]
	static void ConvertToggle(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<Toggle>(command.context);
	}

	[MenuItem("CONTEXT/LoopScrollRect/Convert To LoopScrollView", true)]
	static bool CanConvertLoopScrollView(MenuCommand command)
	{
		return true;
	}
	[MenuItem("CONTEXT/LoopScrollRect/Convert To LoopScrollView", false)]
	static void ConvertLoopScrollView(MenuCommand command)
	{
		//var btn = command.context as LoopScrollRect;
		//var gameObject = btn.gameObject;
		//var content = btn.content;
		//var viewport = btn.viewport;
		//var movementType = btn.movementType;
		//var ItemPrefab = btn.prefabSource.ItemPrefab;
		//var isHorizontal = btn is LoopHorizontalScrollRect;
		//Object.DestroyImmediate(btn, true);

		//var ScrollRect = gameObject.AddComponent<ScrollRect>();
		//ScrollRect.content = content;
		//if (viewport != null)
		//	ScrollRect.viewport = viewport;
		//else ScrollRect.viewport = ScrollRect.transform as RectTransform;
		//ScrollRect.horizontal = false;
		//ScrollRect.movementType = (ScrollRect.MovementType)movementType;

		//var loopScrollView = gameObject.AddComponent<LoopScrollView>();
		//if (ItemPrefab)
		//{
		//	loopScrollView.itemPool.itemPrefab = ItemPrefab.GetComponent<LoopScrollItem>();
		//	if (loopScrollView.itemPool.itemPrefab != null)
		//	{
		//		loopScrollView.itemSize = loopScrollView.itemPool.itemPrefab.rectTransform.rect.size * loopScrollView.itemPool.itemPrefab.rectTransform.localScale;
		//	}
		//	else
		//	{
		//		Debug.LogWarning("预制体为空", ItemPrefab);
		//	}
		//}
		//loopScrollView.mScrollRect = ScrollRect;
		//loopScrollView.startAxis = isHorizontal ? GridLayoutGroup.Axis.Vertical : GridLayoutGroup.Axis.Horizontal;
		//loopScrollView.constrain = Constraint.Flexible;
		////loopScrollView.mFixedRowOrColumnCount = 1;
		//var verticalLayoutGroup = content.GetComponent<VerticalLayoutGroup>();
		//if (verticalLayoutGroup)
		//{
		//	loopScrollView.padding = verticalLayoutGroup.padding;
		//	loopScrollView.itemSpacing = new Vector2(0, verticalLayoutGroup.spacing);
		//	loopScrollView.childAlignment = verticalLayoutGroup.childAlignment;
		//	Object.DestroyImmediate(verticalLayoutGroup, true);
		//}
		//var horizontalLayoutGroup = content.GetComponent<HorizontalLayoutGroup>();
		//if (horizontalLayoutGroup)
		//{
		//	loopScrollView.padding = horizontalLayoutGroup.padding;
		//	loopScrollView.itemSpacing = new Vector2(horizontalLayoutGroup.spacing, 0);
		//	loopScrollView.childAlignment = horizontalLayoutGroup.childAlignment;
		//	Object.DestroyImmediate(horizontalLayoutGroup, true);
		//}
		//var gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
		//if (gridLayoutGroup)
		//{
		//	loopScrollView.padding = gridLayoutGroup.padding;
		//	loopScrollView.itemSpacing = gridLayoutGroup.spacing;
		//	loopScrollView.childAlignment = gridLayoutGroup.childAlignment;
		//	loopScrollView.startAxis = gridLayoutGroup.startAxis;
		//	loopScrollView.startCorner = gridLayoutGroup.startCorner;
		//	loopScrollView.itemSize = gridLayoutGroup.cellSize;
		//	Object.DestroyImmediate(gridLayoutGroup, true);
		//}
		//var contentSizeFitter = content.GetComponent<ContentSizeFitter>();
		//if (contentSizeFitter)
		//{
		//	loopScrollView.horizontalFit = contentSizeFitter.horizontalFit;
		//	loopScrollView.verticalFit = contentSizeFitter.verticalFit;
		//	Object.DestroyImmediate(contentSizeFitter, true);
		//}
		//EditorUtility.SetDirty(content.gameObject);
		//EditorUtility.SetDirty(gameObject);
	}

	[MenuItem("CONTEXT/Dropdown/Convert To ExtendDropdown", true)]
	static bool CanConvertToExtendDropdown(MenuCommand command)
	{
		return ReplaceComponentReferenc.CanConvertTo<ExtendDropdown>(command.context);
	}

	[MenuItem("CONTEXT/Dropdown/Convert To ExtendDropdown", false)]
	static void ConvertToExtendDropdown(MenuCommand command)
	{
		ReplaceComponentReferenc.ConvertTo<ExtendDropdown>(command.context);
	}
}


/// <summary>
/// 无损替换组件，脚本组件ID不变，类型修改
/// 实现方式：修改序列化的m_Script属性
/// 例子：GText和Text、GImage和Image 相互转换
/// </summary>
public class ReplaceComponentReferenc
{
	public static bool CanConvertTo<T>(Object context) where T : MonoBehaviour
	{
		if (context == null)
			return false;
		System.Type a = context.GetType();
		System.Type b = typeof(T);
		return a.IsSubclassOf(b) || b.IsSubclassOf(a);
	}

	public static void ConvertTo<T>(Object context) where T : MonoBehaviour
	{
		var isIns = PrefabUtility.IsPartOfPrefabInstance(context);
		if (isIns)
		{
			Debug.LogError("Replacing a transform on a Prefab instance with adifferent type of transform is not allowed.\n" +
				"You can open the Prefab in Prefab Mode to restructure the Prefab Asset itself, or unpack thePrefab instance to remove its Prefab connection.");
			return;
		}
		var target = context as MonoBehaviour;
		var so = new SerializedObject(target);
		so.Update();

		foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
		{
			if (script.GetClass() != typeof(T)) continue;
			so.FindProperty("m_Script").objectReferenceValue = script;
			so.ApplyModifiedProperties();
			break;
		}

		EditorUtility.SetDirty(so.targetObject);
	}

	public static void ConvertTo(MonoBehaviour target, MonoScript targetScript)
	{
		var so = new SerializedObject(target);
		so.Update();
		so.FindProperty("m_Script").objectReferenceValue = targetScript;
		so.ApplyModifiedProperties();
	}

	//T1类型替换成T2
	public static void ConvertTo<T1, T2>(GameObject prefab) where T1 : MonoBehaviour
	{
		var isIns = PrefabUtility.IsPartOfPrefabInstance(prefab);
		if (isIns)
		{
			Debug.LogError("Replacing a transform on a Prefab instance with adifferent type of transform is not allowed.\n" +
				"You can open the Prefab in Prefab Mode to restructure the Prefab Asset itself, or unpack thePrefab instance to remove its Prefab connection.");
			return;
		}

		MonoScript script = null;
		foreach (var ms in Resources.FindObjectsOfTypeAll<MonoScript>())
		{
			if (ms.GetClass() == typeof(T2))
			{
				script = ms;
				break;
			}
		}

		var comps = prefab.gameObject.GetComponentsInChildren<T1>(true);
		foreach (var mono in comps)
		{
			var so = new SerializedObject(mono);
			so.Update();
			so.FindProperty("m_Script").objectReferenceValue = script;
			so.ApplyModifiedProperties();
		}

	}

}
