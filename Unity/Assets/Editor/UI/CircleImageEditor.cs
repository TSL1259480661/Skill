using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.UI;

namespace UnityEditor.UI
{
	[CustomEditor(typeof(CircleImage), true)]
	[CanEditMultipleObjects]
	public class CustomImageEditor : ImageEditor
	{
		SerializedProperty m_Sprite;
		SerializedProperty m_PreserveAspect;
		SerializedProperty m_UseSpriteMesh;
		GUIContent m_SpriteContent;

		protected override void OnEnable()
		{
			base.OnEnable();
			m_SpriteContent = EditorGUIUtility.TrTextContent("Source Image");
			m_Sprite = serializedObject.FindProperty("m_Sprite");
			m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");
			m_UseSpriteMesh = serializedObject.FindProperty("m_UseSpriteMesh");
			SetShowNativeSize(true);
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			ExtendSpriteGUI();
			AppearanceControlsGUI();
			RaycastControlsGUI();
			MaskableControlsGUI();

			SetShowNativeSize(false);
			if (EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField(m_UseSpriteMesh);
				EditorGUILayout.PropertyField(m_PreserveAspect);
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndFadeGroup();
			NativeSizeButtonGUI();

			serializedObject.ApplyModifiedProperties();
		}

		protected void ExtendSpriteGUI()
		{
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_Sprite, m_SpriteContent);
			EditorGUI.EndChangeCheck();
		}

		void SetShowNativeSize(bool instant)
		{
			Image.Type type = Image.Type.Simple;
			bool showNativeSize = (type == Image.Type.Simple || type == Image.Type.Filled) && m_Sprite.objectReferenceValue != null;
			base.SetShowNativeSize(showNativeSize, instant);
		}
	}
}


