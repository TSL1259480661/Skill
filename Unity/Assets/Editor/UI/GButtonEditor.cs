using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.Rendering;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(GButton), true)]
[CanEditMultipleObjects]
public class GButtonEditor : ButtonEditor
{
	SerializedProperty soundNamekProperty;
	SerializedProperty passEventProperty;
	SerializedProperty passEventReverseProperty;
	SerializedProperty passEventBreakProperty;
	SerializedProperty singleClickIntervalProperty;
	SerializedProperty doubleClickThresholdProperty;
	SerializedProperty longPressDurationProperty;
	SerializedProperty repeatIntervalProperty;

	protected override void OnEnable()
	{
		base.OnEnable();
		soundNamekProperty = serializedObject.FindProperty("soundId");
		passEventReverseProperty = serializedObject.FindProperty("passEventType");
		passEventBreakProperty = serializedObject.FindProperty("bPassEventBreak");
		singleClickIntervalProperty = serializedObject.FindProperty("singleClickInterval");
		doubleClickThresholdProperty = serializedObject.FindProperty("doubleClickThreshold");
		longPressDurationProperty = serializedObject.FindProperty("longPressDuration");
		repeatIntervalProperty = serializedObject.FindProperty("longRepeatInterval");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(soundNamekProperty);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(passEventReverseProperty, new GUIContent("事件透传类型"));
		EditorGUILayout.EndHorizontal();
		ePassEventType type = passEventReverseProperty.GetEnumValue<ePassEventType>();
		if (type == ePassEventType.reverse)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(passEventBreakProperty, new GUIContent("透传响应后是否就不再自我响应"));
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(singleClickIntervalProperty);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(doubleClickThresholdProperty);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(longPressDurationProperty);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(repeatIntervalProperty);
		EditorGUILayout.EndHorizontal();

		var btn = target as GButton;
		if (GUILayout.Button(btn.IsGray ? "取消变灰" : "子节点全部变灰"))
		{
			btn.IsGray = (!btn.IsGray);
			EditorUtility.SetDirty(btn);
		}

		serializedObject.ApplyModifiedProperties();
	}
}
