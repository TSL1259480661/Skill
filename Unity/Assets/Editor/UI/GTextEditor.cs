using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


[CustomEditor(typeof(GText), true)]
[CanEditMultipleObjects]
public class GTextEditor : UnityEditor.UI.TextEditor
{
	SerializedProperty grayColorProperty;
	SerializedProperty graySpriteProperty;

	protected override void OnEnable()
	{
		base.OnEnable();
		grayColorProperty = serializedObject.FindProperty("grayColor");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(grayColorProperty);
		EditorGUILayout.EndHorizontal();

	
		serializedObject.ApplyModifiedProperties();
	}

}
