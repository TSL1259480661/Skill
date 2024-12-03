using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ExtendDropdown), true)]
public class ExtendDropdownEditor : UnityEditor.UI.DropdownEditor
{
	SerializedProperty templateSizeProperty;
	SerializedProperty templateAnchoredPositionProperty;
	SerializedProperty controlCheckActiveProperty;

	/// <summary>
	/// 模板
	/// </summary>
	protected RectTransform template
	{
		get
		{
			return (target as ExtendDropdown).template;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		templateSizeProperty = serializedObject.FindProperty("templateSize");
		templateAnchoredPositionProperty = serializedObject.FindProperty("templateAnchoredPosition");
		controlCheckActiveProperty = serializedObject.FindProperty("controlCheckActive");
		if (template != null)
		{
			if (templateSizeProperty.vector2Value == Vector2.zero)
			{
				templateSizeProperty.vector2Value = template.rect.size;
				serializedObject.ApplyModifiedProperties();
			}
		    if (templateAnchoredPositionProperty.vector2Value == Vector2.zero)
			{
				templateAnchoredPositionProperty.vector2Value = template.anchoredPosition;
				serializedObject.ApplyModifiedProperties();
			}

			//controlCheckActiveProperty.boolValue = false;
			//serializedObject.ApplyModifiedProperties();
		}
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		serializedObject.Update();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("模板尺寸", GUILayout.Width(60));
		EditorGUILayout.PropertyField(templateSizeProperty, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("模板AnchoredPosition", GUILayout.Width(80));
		EditorGUILayout.PropertyField(templateAnchoredPositionProperty, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("控制CheckMark的Active", GUILayout.Width(80));
		EditorGUILayout.PropertyField(controlCheckActiveProperty, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();
		serializedObject.ApplyModifiedProperties();
	}
}
