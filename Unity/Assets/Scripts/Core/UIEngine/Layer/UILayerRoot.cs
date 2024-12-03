using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UIEngine
{
	public class UILayerRoot
	{
#if UNITY_EDITOR
		[MenuItem("UI/初始化层级标签")]
		public static void InitTags()
		{
			string[] values = System.Enum.GetNames(typeof(UILayerType));
			SerializedObject tagsAndLayersManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty sortingLayersProp = tagsAndLayersManager.FindProperty("m_SortingLayers");

			bool addEnable = true;
			string start = values[0];
			for (int i = 0; i < sortingLayersProp.arraySize; i++)
			{
				var newlayer = sortingLayersProp.GetArrayElementAtIndex(i);
				if (newlayer.FindPropertyRelative("name").stringValue == start)
				{
					int endIndex = i + values.Length;
					if (endIndex <= sortingLayersProp.arraySize)
					{
						bool allEqual = true;
						for (int j = 0; j < values.Length; j++)
						{
							newlayer = sortingLayersProp.GetArrayElementAtIndex(i + j);
							if (newlayer.FindPropertyRelative("name").stringValue != values[j])
							{
								allEqual = false;
								break;
							}
						}

						if (allEqual)
						{
							addEnable = false;
						}
					}
					else
					{
						break;
					}
				}
			}

			if (addEnable)
			{
				for (int i = 0; i < values.Length; i++)
				{
					sortingLayersProp.InsertArrayElementAtIndex(sortingLayersProp.arraySize);
					var newlayer = sortingLayersProp.GetArrayElementAtIndex(sortingLayersProp.arraySize - 1);
					newlayer.FindPropertyRelative("uniqueID").intValue = sortingLayersProp.arraySize - 1;
					newlayer.FindPropertyRelative("name").stringValue = values[i];
					tagsAndLayersManager.ApplyModifiedProperties();
				}
			}
		}
#endif
		private GameObject rootGo;
		private Transform rootTransform;
		private UICamera uiCamera;
		private Vector2 resolutionRatio;

		public UILayerRoot(GameObject rootGo, UICamera uiCamera, int layerDistance, Vector2 resolutionRatio)
		{
			if (rootGo != null)
			{
				rootTransform = rootGo.transform;
				GameObject.DontDestroyOnLoad(rootGo);
			}

			this.rootGo = rootGo;

			this.uiCamera = uiCamera;
			this.resolutionRatio = resolutionRatio;
			InitLayers(layerDistance);
		}

		private int layerDistance;
		private int startPlaneDistance;

		private IUILayer[] layerDic;
		private void InitLayers(int layerDistance)
		{
			this.layerDistance = layerDistance;

			int layerMask = LayerMask.NameToLayer("UI");

			rootGo.layer = layerMask;

			uiCamera.camera.transform.position = new Vector3(0f, 0f, 0f);

			UILayerType[] values = System.Enum.GetValues(typeof(UILayerType)) as UILayerType[];
			layerDic = new IUILayer[values.Length];

			uiCamera.SetFarClipPlane((layerDic.Length + 1) * layerDistance);

			startPlaneDistance = layerDic.Length * layerDistance;

			CreateLayer(UILayerType.Ground, layerMask);
			CreateLayer(UILayerType.GroundEffect, layerMask);
			CreateLayer(UILayerType.Character, layerMask);
			CreateLayer(UILayerType.CharacterEffect, layerMask);
			CreateLayer(UILayerType.Air, layerMask);
			CreateLayer(UILayerType.AirEffect, layerMask);

			CreateLayer(UILayerType.Background, layerMask, true);
			CreateLayer(UILayerType.Bottom, layerMask, true);
			CreateLayer(UILayerType.Normal, layerMask, true);
			CreateLayer(UILayerType.Pop, layerMask, true);
			CreateLayer(UILayerType.Top, layerMask, true);
			CreateLayer(UILayerType.System, layerMask, true);
		}
		//566 * 202
		private void CreateLayer(UILayerType layer, int layerMask, bool isUIlayer = false)
		{
			if (isUIlayer)
			{
				layerDic[(int)layer] = new UILayer(rootGo, startPlaneDistance, layerMask, layer.ToString(), resolutionRatio, uiCamera.camera);
			}
			else
			{
				layerDic[(int)layer] = new GameSceneLayer(layer.ToString(), rootTransform, new Vector3(0f, 0f, startPlaneDistance), layerMask);
			}

			startPlaneDistance -= layerDistance;
		}

		public IUILayer GetLayer(UILayerType layerType)
		{
			return layerDic[(int)layerType];
		}

		public IUILayer Add(UILayerType layer, GameObject go)
		{
			var uiLayer = GetLayer(layer);
			uiLayer?.Add(go);
			return uiLayer;
		}

		public IUILayer Add(GameObject instance, UILayerType layerType)
		{
			IUILayer layer = layerDic[(int)layerType];
			layer.Add(instance);
			return layer;
		}
	}
}
