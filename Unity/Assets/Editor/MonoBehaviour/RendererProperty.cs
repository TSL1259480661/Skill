using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkinnedMeshRenderer))]
public class SkinnedMeshRendererProperty : Editor
{
    /// <summary>
    /// 排序列表
    /// </summary>
    private string[] sorting_layers;

    /// <summary>
    /// 目标
    /// </summary>
    public SkinnedMeshRenderer renderer
    {
        get
        {
            return target as SkinnedMeshRenderer;
        }
    }

    /// <summary>
    /// 排序列表
    /// </summary>
    public string[] SortingLayers
    {
        get
        {
            if (sorting_layers == null || sorting_layers.Length != SortingLayer.layers.Length)
            {
                sorting_layers = new string[SortingLayer.layers.Length];
                for (int index = 0; index < SortingLayer.layers.Length; ++index)
                {
                    sorting_layers[index] = SortingLayer.layers[index].name;
                }
            }
            return sorting_layers;
        }
    }

    /// <summary>
    /// 扩展显示
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        renderer.sortingOrder = EditorGUILayout.IntField("SortingOrder", renderer.sortingOrder, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        int layer = SortingLayer.GetLayerValueFromName(renderer.sortingLayerName);
        int index = EditorGUILayout.Popup("SortingLayerName", layer, SortingLayers, GUILayout.ExpandWidth(true));
        if (layer != index)
        {
            //renderer.sortingLayerName = SortingLayer.IDToName(index);
			renderer.sortingLayerName = SortingLayers[index];
		}
        EditorGUILayout.EndHorizontal();
    }
}

[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererProperty : Editor
{
    /// <summary>
    /// 排序列表
    /// </summary>
    private string[] sorting_layers;

    /// <summary>
    /// 目标
    /// </summary>
    public MeshRenderer renderer
    {
        get
        {
            return target as MeshRenderer;
        }
    }

    /// <summary>
    /// 排序列表
    /// </summary>
    public string[] SortingLayers
    {
        get
        {
            if (sorting_layers == null || sorting_layers.Length != SortingLayer.layers.Length)
            {
                sorting_layers = new string[SortingLayer.layers.Length];
                for (int index = 0; index < SortingLayer.layers.Length; ++index)
                {
                    sorting_layers[index] = SortingLayer.layers[index].name;
                }
            }
            return sorting_layers;
        }
    }

    /// <summary>
    /// 扩展显示
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        renderer.sortingOrder = EditorGUILayout.IntField("SortingOrder", renderer.sortingOrder, GUILayout.ExpandWidth(true));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        int layer = SortingLayer.GetLayerValueFromName(renderer.sortingLayerName);
        int index = EditorGUILayout.Popup("SortingLayerName", layer, SortingLayers, GUILayout.ExpandWidth(true));
        if (layer != index)
        {
			//renderer.sortingLayerName = SortingLayer.IDToName(index);
			renderer.sortingLayerName = SortingLayers[index];
		}
        EditorGUILayout.EndHorizontal();
    }
}
