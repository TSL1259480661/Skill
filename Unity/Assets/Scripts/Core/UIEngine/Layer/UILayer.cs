using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WeChatWASM;

namespace UIEngine
{
	public class UILayer : IUILayer
	{
		private int layerMask = 5;
		private int maxOrder = 0;
		private Canvas layerCanvas;
		private CanvasScaler layerScaler;
		private string layerName;
		public RectTransform layerRoot { private set; get; }
		public float PlaneDistance
		{
			get
			{
				return layerCanvas.planeDistance;
			}
		}

		public UILayer(GameObject rootGo, int planeDistance, int layerMask, string layerName, Vector2 resolutionRatio, Camera camera = null)
		{
			this.layerMask = layerMask;
			this.layerName = layerName;

			GameObject go = new GameObject(layerName + "_UI");
			go.layer = layerMask;
			maxOrder = 1;

			layerCanvas = go.AddComponent<Canvas>();
			layerScaler = go.AddComponent<CanvasScaler>();
			go.AddComponent<GraphicRaycaster>();

			layerRoot = go.GetComponent<RectTransform>();
			layerRoot.SetParent(rootGo.transform);

			layerCanvas.renderMode = RenderMode.ScreenSpaceCamera;
			layerCanvas.planeDistance = planeDistance;
			layerCanvas.sortingOrder = 0;

			layerScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			layerScaler.referenceResolution = resolutionRatio;
			layerScaler.matchWidthOrHeight = 0f;

			layerCanvas.worldCamera = camera;
			layerCanvas.sortingLayerName = layerName;
		}

		public void Add(GameObject go)
		{
			Add(go, null);
		}

		public void Add(GameObject go, GameObject parent = null)
		{
			if (go != null)
			{
				if (parent != null)
				{
					Add(go.transform, parent.transform);
				}
				else
				{
					Add(go.transform, null);
				}
			}
		}

		public void Add(Transform transform, Transform parent = null)
		{
			if (parent == null)
			{
				parent = layerRoot;
			}

			if (transform != null)
			{
				transform.SetParent(parent);

				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				transform.localEulerAngles = Vector3.zero;
				transform.SetAsLastSibling();

				Transform tempParent = transform;
				while (tempParent.parent != layerRoot)
				{
					tempParent = tempParent.parent;
				}

				bool needSort = false;

				if (maxOrder >= 100000)
				{
					maxOrder = 1;
					needSort = true;
				}

				for (int i = 0; i < layerRoot.childCount; i++)
				{
					Transform child = layerRoot.GetChild(i);
					if (child == tempParent)
					{
						needSort = true;
					}

					if (needSort)
					{
						SetSortingProperties(child);
					}
				}
			}
		}

		public void SetSortingProperties(Transform transform)
		{
			transform.gameObject.layer = layerMask;

			Canvas canvas = transform.GetComponent<Canvas>();
			if (canvas != null)
			{
				canvas.overrideSorting = true;
				canvas.sortingLayerName = layerName;
				canvas.sortingOrder = maxOrder;
				RectTransform canvasTransform = (transform as RectTransform); ;
				if (canvasTransform != null)
				{
					canvasTransform.anchorMin = Vector2.zero;
					canvasTransform.anchorMax = Vector2.one;
					canvasTransform.offsetMax = Vector2.zero;
					canvasTransform.offsetMin = Vector2.zero;
				}
				maxOrder++;
			}
			else
			{
				Renderer renderer = transform.GetComponent<Renderer>();
				if (renderer != null)
				{
					renderer.sortingLayerName = layerName;
					renderer.sortingOrder = maxOrder;
					maxOrder++;
				}
			}

			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				SetSortingProperties(child);
			}
		}

		public void ResetOrderInLayers()
		{
			maxOrder = 1;

			for (int i = 0; i < layerRoot.childCount; i++)
			{
				Transform child = layerRoot.GetChild(i);
				SetSortingProperties(child);
			}
		}

		

		public void Add(ILayerItem item)
		{
		}
	}
}
