using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CreateAssetMenu(fileName = "EditorExportPathConfig", menuName = "ScriptableObjects/EditorExportPathConfig", order = 12)]
public class EditorExportPathConfig : ScriptableObject
{
	public string ProtoPath = "../../Data/Proto";
	public string ProtoExportPath = "Assets/Scripts/Generate/Message";


	public string ExcelPath = "../../Data/Excel";
	public string ExcelExportJsonPath = "../../Data/Excel/Json";
	public string ExcelExportBytePath = "Assets/Bundles/Config";
	public string ExcelExportClassPath = "Assets/Scripts/Generate/Config";
}

#endif
