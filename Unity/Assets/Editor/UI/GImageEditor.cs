using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;


[CustomEditor(typeof(GImage), true)]
[CanEditMultipleObjects]
public class GImageEditor : ImageEditor
{
	SerializedProperty grayColorProperty;
	SerializedProperty graySpriteProperty;
	SerializedProperty grayMaterialProperty;

	protected override void OnEnable()
	{
		base.OnEnable();
		grayColorProperty = serializedObject.FindProperty("grayColor");
		graySpriteProperty = serializedObject.FindProperty("graySprite");
		grayMaterialProperty = serializedObject.FindProperty("grayMaterial");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		EditorGUILayout.Space();

		serializedObject.Update();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(grayColorProperty);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(graySpriteProperty);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PropertyField(grayMaterialProperty);
		EditorGUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

}
