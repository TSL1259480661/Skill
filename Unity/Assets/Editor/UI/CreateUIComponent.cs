/***************************************
 * 作者：
 * 版本：
 * 时间：
 * 描述：我们开发中大部分图片和text是不需要射线检测的,创建一个Graphic组件时，默认取消raycastTarget.
 ***************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateUIComponent : Editor
{
	[MenuItem("GameObject/UI/GImage", false, 12)]
	static void CreatImage()
	{
		var go = CreateUI("Image");
		Undo.RegisterCreatedObjectUndo(go, "Create Image");
		AddImage(go);
		EditorUtility.SetDirty(go);
	}

	[MenuItem("GameObject/UI/ImageGradient", false, 12)]
	static void CreatImageGradient()
	{
		var go = CreateUI("ImageGradient");
		Undo.RegisterCreatedObjectUndo(go, "Create ImageGradient");
		AddGradientImage(go);
		EditorUtility.SetDirty(go);
	}

	[MenuItem("GameObject/UI/GText", false, 12)]
	static void CreatText()
	{
		var go = CreateUI("Text");
		Undo.RegisterCreatedObjectUndo(go, "Create Text");
		AddText(go);
		EditorUtility.SetDirty(go);
	}

	[MenuItem("GameObject/UI/GradientText", false, 12)]
	static void CreatGradientText()
	{
		var go = CreateUI("GradientText");
		Undo.RegisterCreatedObjectUndo(go, "Create GradientText");
		AddGradientText(go);
		EditorUtility.SetDirty(go);
	}

	[MenuItem("GameObject/UI/GButton", false, 12)]
	static void CreatButton()
	{
		var go = CreateUI("Button");
		Undo.RegisterCreatedObjectUndo(go, "Create Button");
		var image = AddImage(go);
		image.raycastTarget = true;
		var button = go.AddComponent<GButton>();
		var textGo = CreateUI("Text", go);
		AddText(textGo);
		EditorUtility.SetDirty(go);
	}

	[MenuItem("GameObject/UI/ExtendButton", false, 12)]
	static void CreatExtendButton()
	{
		var go = CreateUI("Button");
		Undo.RegisterCreatedObjectUndo(go, "Create Button");
		var image = AddImage(go);
		image.raycastTarget = true;
		var button = go.AddComponent<ExtendButton>();
		var textGo = CreateUI("Text", go);
		AddText(textGo);
		EditorUtility.SetDirty(go);
	}

	public static GameObject CreateUI(string name, GameObject parent = null)
	{
		if (parent == null)
		{
			parent = Selection.activeGameObject;
		}
		if (parent == null || parent.GetComponentInParent<Canvas>() == null)
		{
			Debug.LogWarning("No Canvas found in the parent hierarchy. Please select a GameObject with a Canvas.");
			return null;
		}

		GameObject go = new GameObject(name, typeof(RectTransform));
		go.transform.SetParent(parent.transform, false);
		go.transform.localPosition = Vector3.zero;
		go.layer = parent.layer;
		return go;
	}

	public static GText AddText(GameObject go)
	{
		if (go == null)
			return null;
		var text = go.AddComponent<GText>();
		text.raycastTarget = false;
		text.text = "New Text";
		//text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Bundles/Font/zh/方正兰亭粗黑简体加缺字666.ttf");
		return text;
	}

	public static GradientText AddGradientText(GameObject go)
	{
		if (go == null)
			return null;
		var text = go.AddComponent<GradientText>();
		text.raycastTarget = false;
		text.text = "New Text";
		//text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Bundles/Font/zh/方正兰亭粗黑简体加缺字666.ttf");
		return text;
	}

	public static GImage AddImage(GameObject go)
	{
		if (go == null)
			return null;
		var text = go.AddComponent<GImage>();
		text.raycastTarget = false;

		string path = AssetDatabase.GUIDToAssetPath("0db28ca340460fa4ca50a3f9589a8daf");
		var greyMaterial = AssetDatabase.LoadAssetAtPath<Material>(path);
		text.grayMaterial = greyMaterial;

		return text;
	}

	public static ImageGradient AddGradientImage(GameObject go)
	{
		if (go == null)
		{
			return null;
		}
		var text = go.AddComponent<ImageGradient>();
		return text;
	}
}
