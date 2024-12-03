using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(GToggle), true)]
[CanEditMultipleObjects]
public class GToggleEditor : ToggleEditor
{

	SerializedProperty soundNamekProperty;
	SerializedProperty passEventProperty;
	SerializedProperty longPressThresholdProperty;
	SerializedProperty longPressContinueProperty;

	protected override void OnEnable()
	{
		base.OnEnable();
		soundNamekProperty = serializedObject.FindProperty("soundId");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(soundNamekProperty);
		EditorGUILayout.EndHorizontal();

		
		serializedObject.ApplyModifiedProperties();
	}
}
